// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using FlaxEditor.CustomEditors;
using FlaxEditor.GUI.Dialogs;
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
