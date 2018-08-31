// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FlaxEditor.CustomEditors;
using FlaxEditor.GUI.Dialogs;
using FlaxEditor.Scripting;
using FlaxEngine;
using FlaxEngine.GUI;

namespace FlaxEditor.Windows
{
    /// <summary>
    /// Dialog used to export plugin with its content.
    /// </summary>
    /// <seealso cref="FlaxEditor.GUI.Dialogs.Dialog" />
    public class PluginExporterDialog : Dialog
    {
        private class ExportOptions
        {
            [HideInEditor]
            public readonly PluginDescription Description;

            [HideInEditor]
            public readonly GamePlugin GamePlugin;

            [HideInEditor]
            public readonly EditorPlugin EditorPlugin;

            [HideInEditor]
            public string OutputPath => Path.Combine(Globals.ProjectFolder, Output, ShortName);

            [EditorOrder(0), Tooltip("The plugin short name used for the output assemblies naming.")]
            public string ShortName;

            [EditorOrder(10), Tooltip("The plugin icon, used only in the editor's plugin manager.")]
            public Texture Icon;

            [EditorOrder(20), Tooltip("If checked, the project content directory will be exported alongside the plugin modules.")]
            public bool IncludeContent = true;

            [EditorOrder(30), Tooltip("Output folder path (relative to the project root directory).")]
            public string Output = "Output\\";

            [EditorOrder(40), Tooltip("Plugin code configuration mode.")]
            public BuildMode Configuration = BuildMode.Release;

            public ExportOptions(GamePlugin gamePlugin, EditorPlugin editorPlugin)
            {
                GamePlugin = gamePlugin;
                EditorPlugin = editorPlugin;
                Description = gamePlugin?.Description ?? editorPlugin.Description;
                ShortName = ((Plugin)gamePlugin ?? editorPlugin).GetType().Name;
            }
        }

        /// <summary>
        /// Shows the dialog.
        /// </summary>
        public static void ShowIt()
        {
            var parentWin = Editor.Instance.Windows.MainWindow;

            if (!PluginUtils.GetPluginToExport(out var gamePlugin, out var editorPlugin, out var errorMsg))
            {
                MessageBox.Show(parentWin, errorMsg, "Cannot export plugin", MessageBox.Buttons.OK, MessageBox.Icon.Error);
                return;
            }

            var dialog = new PluginExporterDialog(gamePlugin, editorPlugin);
            dialog.Show(parentWin);
        }

        private readonly ExportOptions _options;

        private PluginExporterDialog(GamePlugin gamePlugin, EditorPlugin editorPlugin)
        : base("Export Plugin")
        {
            const float TotalWidth = 720;
            Width = TotalWidth;

            _options = new ExportOptions(gamePlugin, editorPlugin);

            // Header and help description
            var headerLabel = new Label(0, 0, TotalWidth, 40)
            {
                Text = "Export plugin " + _options.Description.Name,
                DockStyle = DockStyle.Top,
                Parent = this,
                Font = new FontReference(Style.Current.FontTitle)
            };
            var infoLabel = new Label(10, headerLabel.Bottom + 5, TotalWidth - 20, 40)
            {
                Text = "Specify options for exporting plugin. To learn more about it see the online documentation.",
                HorizontalAlignment = TextAlignment.Near,
                Margin = new Margin(7),
                DockStyle = DockStyle.Top,
                Parent = this
            };

            // Buttons
            const float ButtonsWidth = 60;
            const float ButtonsMargin = 8;
            var exportButton = new Button(TotalWidth - ButtonsMargin - ButtonsWidth, infoLabel.Bottom - 30, ButtonsWidth)
            {
                Text = "Export",
                AnchorStyle = AnchorStyle.UpperRight,
                Parent = this
            };
            exportButton.Clicked += OnExport;
            var cancelButton = new Button(exportButton.Left - ButtonsMargin - ButtonsWidth, exportButton.Y, ButtonsWidth)
            {
                Text = "Cancel",
                AnchorStyle = AnchorStyle.UpperRight,
                Parent = this
            };
            cancelButton.Clicked += OnCancel;

            // Settings editor
            var editor = new CustomEditorPresenter(null);
            editor.Panel.DockStyle = DockStyle.Fill;
            editor.Panel.Parent = this;

            editor.Select(_options);

            Size = new Vector2(TotalWidth, 300);
        }

        private void OnExport()
        {
            // TODO: consider doing this stuff on a custom thread with a progress reporting

            var errorMsg = DoExport();
            if (errorMsg != null)
            {
                MessageBox.Show("Cannot export plugin. " + errorMsg, "Failed to export plugin.", MessageBox.Buttons.OK, MessageBox.Icon.Error);
                return;
            }

            MessageBox.Show("Plugin exported!", "Plugin exported!", MessageBox.Buttons.OK, MessageBox.Icon.Information);

            // Show the output folder
            Application.StartProcess(_options.OutputPath);

            Close(DialogResult.OK);
        }

        private string DoExport()
        {
            var startTime = DateTime.Now;
            Editor.Log("Exporting plugin " + _options.Description.Name);

            // Validate data
            if (string.IsNullOrEmpty(_options.ShortName))
                return "Missing short name.";
            if (_options.ShortName == "Assembly" || _options.ShortName.Contains(' '))
                return "Invalid short name.";

            // Compile project scripts as a plugin
            var assemblyName = _options.ShortName;
            var solutionPath = ScriptsBuilder.SolutionPath;
            if(ScriptsBuilder.GeneratePluginProject(assemblyName))
                return "Failed to generate plugin solution and project files.";
            if (ScriptsBuilder.Compile(solutionPath, _options.Configuration))
                return "Scripts compilation failed. See Debug Console or log file to learn more.";
            if (ScriptsBuilder.GenerateProject(true, true))
                return "Failed to generate restore solution and project files.";

            // Setup output directory
            var outputPath = _options.OutputPath;
            try
            {
                Editor.Log("Setup output directory");
                if (Directory.Exists(outputPath))
                    Directory.Delete(outputPath, true);
                Directory.CreateDirectory(outputPath);
            }
            catch (Exception ex)
            {
                Editor.LogWarning(ex);
                return "Failed to setup output directory.";
            }

            // Copy plugin binaries
            try
            {
                Editor.Log("Exporting plugin binaries");

                var files = Directory.GetFiles(Path.Combine(Globals.ProjectCacheFolder, "bin")).ToList();

                // Don't copy Flax files
                RemoveStartsWith(files, "FlaxEngine");
                RemoveStartsWith(files, "FlaxEditor");
                RemoveStartsWith(files, "Newtonsoft.Json");

                // Don't copy pdb files if release mode is checked
                if (_options.Configuration == BuildMode.Release)
                {
                    RemoveEndsWith(files, ".pdb");
                }

                // Don't copy game assembly if plugin is editor-only and vice versa
                if (_options.GamePlugin == null)
                {
                    Remove(files, assemblyName);
                }
                else if (_options.EditorPlugin == null)
                {
                    Remove(files, assemblyName + ".Editor");
                }

                // Don't copy previous game assembly
                Remove(files, "Assembly");
                Remove(files, "Assembly.Editor");

                Editor.Log(files.Count + " files");
                for (int i = 0; i < files.Count; i++)
                {
                    var src = files[i];
                    var filename = Path.GetFileName(src);
                    var dst = Path.Combine(outputPath, filename);
                    File.Copy(src, dst);
                }
            }
            catch (Exception ex)
            {
                Editor.LogWarning(ex);
                return "Failed to copy plugin binaries.";
            }

            // Copy content
            try
            {
                if (_options.IncludeContent)
                {
                    Editor.Log("Exporting plugin content");

                    // Copy all assets
                    var src = Globals.ContentFolder;
                    var dst = Path.Combine(outputPath, "Content");
                    Utilities.Utils.DirectoryCopy(src, dst);

                    // Remove some unwanted data
                    Utilities.Utils.RemoveFileIfExists(Path.Combine(dst, "GameSettings.json"));
                }
            }
            catch (Exception ex)
            {
                Editor.LogWarning(ex);
                return "Failed to copy plugin content.";
            }

            // Copy plugin icon
            try
            {
                if (_options.Icon)
                {
                    Editor.Log("Exporting plugin icon");

                    var src = _options.Icon.Path;
                    var dst = Path.Combine(outputPath, assemblyName + ".Icon.flax");
                    File.Copy(src, dst);
                }
            }
            catch (Exception ex)
            {
                Editor.LogWarning(ex);
                return "Failed to copy plugin icon.";
            }

            // Generate plugin description json file
            try
            {
                Editor.Log("Exporting plugin description");

                var dst = Path.Combine(outputPath, assemblyName + ".json");
                if (Editor.SaveJsonAsset(dst, _options.Description))
                    throw new FlaxException("Failed to save json asset.");
            }
            catch (Exception ex)
            {
                Editor.LogWarning(ex);
                return "Failed to generate plugin description.";
            }

            // Done!
            var endTime = DateTime.Now;
            Editor.Log(string.Format("Plugin exported in {0}s", Math.Max(1, (int)(endTime - startTime).TotalSeconds)));
            return null;
        }

        private void Remove(List<string> files, string filenameNoExt)
        {
            for (int i = files.Count - 1; i >= 0 && files.Count > 0; i--)
            {
                if (Path.GetFileNameWithoutExtension(files[i]).Equals(filenameNoExt, StringComparison.OrdinalIgnoreCase))
                {
                    files.RemoveAt(i);
                }
            }
        }

        private void RemoveStartsWith(List<string> files, string prefix)
        {
            for (int i = files.Count - 1; i >= 0 && files.Count > 0; i--)
            {
                if (Path.GetFileName(files[i]).StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
                {
                    files.RemoveAt(i);
                }
            }
        }

        private void RemoveEndsWith(List<string> files, string postfix)
        {
            for (int i = files.Count - 1; i >= 0 && files.Count > 0; i--)
            {
                if (files[i].EndsWith(postfix, StringComparison.OrdinalIgnoreCase))
                {
                    files.RemoveAt(i);
                }
            }
        }

        private void OnCancel()
        {
            Close(DialogResult.Cancel);
        }

        /// <inheritdoc />
        protected override void SetupWindowSettings(ref CreateWindowSettings settings)
        {
            base.SetupWindowSettings(ref settings);

            settings.MinimumSize = new Vector2(300, 400);
            settings.HasSizingFrame = true;
        }
    }
}
