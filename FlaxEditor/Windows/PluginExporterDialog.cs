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
        /// <summary>
        /// Plugin exporting options.
        /// </summary>
        public class ExportOptions
        {
            /// <summary>
            /// The description.
            /// </summary>
            [HideInEditor]
            public readonly PluginDescription Description;

            /// <summary>
            /// The game plugin.
            /// </summary>
            [HideInEditor]
            public readonly GamePlugin GamePlugin;

            /// <summary>
            /// The editor plugin.
            /// </summary>
            [HideInEditor]
            public readonly EditorPlugin EditorPlugin;

            /// <summary>
            /// Gets the output path.
            /// </summary>
            [HideInEditor]
            public string OutputPath => Path.Combine(Globals.ProjectFolder, Output, ShortName);

            /// <summary>
            /// The short name.
            /// </summary>
            [EditorOrder(0), Tooltip("The plugin short name used for the output assemblies naming.")]
            public string ShortName;

            /// <summary>
            /// The icon.
            /// </summary>
            [EditorOrder(10), Tooltip("The plugin icon, used only in the editor's plugin manager.")]
            public Texture Icon;

            /// <summary>
            /// The include content flag.
            /// </summary>
            [EditorOrder(20), Tooltip("If checked, the project content directory will be exported alongside the plugin modules.")]
            public bool IncludeContent = true;

            /// <summary>
            /// The output.
            /// </summary>
            [EditorOrder(30), Tooltip("Output folder path (relative to the project root directory).")]
            public string Output = "Output\\";

            /// <summary>
            /// The configuration mode.
            /// </summary>
            [EditorOrder(40), Tooltip("Plugin code configuration mode.")]
            public BuildMode Configuration = BuildMode.Release;

            /// <summary>
            /// Initializes a new instance of the <see cref="ExportOptions"/> class.
            /// </summary>
            /// <param name="gamePlugin">The game plugin.</param>
            /// <param name="editorPlugin">The editor plugin.</param>
            public ExportOptions(GamePlugin gamePlugin, EditorPlugin editorPlugin)
            {
                if (gamePlugin == null && editorPlugin == null)
                    throw new ArgumentException();

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

        private ExportOptions _options;

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

            var errorMsg = DoExport(ref _options);
            if (errorMsg != null)
            {
                MessageBox.Show("Cannot export plugin. " + errorMsg, "Failed to export plugin.", MessageBox.Buttons.OK, MessageBox.Icon.Error);
                return;
            }

            // Show the output folder
            Application.StartProcess(_options.OutputPath);

            Close(DialogResult.OK);
        }

        /// <summary>
        /// Performs the plugin exporting.
        /// </summary>
        /// <param name="options">The export options.</param>
        /// <returns>The error message or null if not failed.</returns>
        public static string DoExport(ref ExportOptions options)
        {
            var startTime = DateTime.Now;
            Editor.Log("Exporting plugin " + options.Description.Name);

            // Validate data
            if (string.IsNullOrEmpty(options.ShortName))
                return "Missing short name.";
            if (options.ShortName == "Assembly")
                return "Invalid short name. Don't use restricted names.";
            if (options.ShortName.Contains(' ') || options.ShortName.Contains('\n') || options.ShortName.Contains('\r') || options.ShortName.Contains('\t'))
                return "Invalid short name. It cannot contain whitespace characters.";
            if (Utilities.Utils.HasInvalidPathChar(options.ShortName))
                return "Invalid short name. It must be a valid path name.";

            // Prepare
            var assemblyName = options.ShortName;
            var solutionPath = ScriptsBuilder.SolutionPath;
            var assembliesOutputPath = Path.Combine(Globals.ProjectCacheFolder, "bin");

            // Remove previous compilation artifacts (if user changes plugin name it may leave some old name assemblies)
            try
            {
                Editor.Log("Cleanup assemblies output directory");

                var files = Directory.GetFiles(assembliesOutputPath, "*.dll").ToList();
                RemoveStartsWith(files, "FlaxEngine");
                RemoveStartsWith(files, "FlaxEditor");
                RemoveStartsWith(files, "Newtonsoft.Json");
                Remove(files, "Assembly");
                Remove(files, "Assembly.Editor");
                Remove(files, assemblyName);
                Remove(files, assemblyName + ".Editor");
                
                for (int i = 0; i < files.Count; i++)
                {
                    Editor.Log("Removing " + files[i]);
                    File.Delete(files[i]);
                }
            }
            catch (Exception ex)
            {
                Editor.LogWarning(ex);
                return "Failed to cleanup assemblies output directory.";
            }

            // Compile project scripts as a plugin
            if (ScriptsBuilder.GeneratePluginProject(assemblyName))
                return "Failed to generate plugin solution and project files.";
            if (ScriptsBuilder.Compile(solutionPath, options.Configuration))
                return "Scripts compilation failed. See Debug Console or log file to learn more.";
            if (ScriptsBuilder.GenerateProject(true, true))
                return "Failed to generate restore solution and project files.";

            // Setup output directory
            var outputPath = options.OutputPath;
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

                var files = Directory.GetFiles(assembliesOutputPath).ToList();

                // Don't copy Flax files
                RemoveStartsWith(files, "FlaxEngine");
                RemoveStartsWith(files, "FlaxEditor");
                RemoveStartsWith(files, "Newtonsoft.Json");

                // Don't copy pdb files if release mode is checked
                if (options.Configuration == BuildMode.Release)
                {
                    RemoveEndsWith(files, ".pdb");
                }

                // Don't copy game assembly if plugin is editor-only and vice versa
                if (options.GamePlugin == null)
                {
                    Remove(files, assemblyName);
                }
                else if (options.EditorPlugin == null)
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
                if (options.IncludeContent)
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
                if (options.Icon)
                {
                    Editor.Log("Exporting plugin icon");

                    var src = options.Icon.Path;
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
                if (Editor.SaveJsonAsset(dst, options.Description))
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

        private static void Remove(List<string> files, string filenameNoExt)
        {
            for (int i = files.Count - 1; i >= 0 && files.Count > 0; i--)
            {
                if (Path.GetFileNameWithoutExtension(files[i]).Equals(filenameNoExt, StringComparison.OrdinalIgnoreCase))
                {
                    files.RemoveAt(i);
                }
            }
        }

        private static void RemoveStartsWith(List<string> files, string prefix)
        {
            for (int i = files.Count - 1; i >= 0 && files.Count > 0; i--)
            {
                if (Path.GetFileName(files[i]).StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
                {
                    files.RemoveAt(i);
                }
            }
        }

        private static void RemoveEndsWith(List<string> files, string postfix)
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
