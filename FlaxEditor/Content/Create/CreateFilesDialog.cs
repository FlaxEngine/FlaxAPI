// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System;
using System.Collections.Generic;
using FlaxEditor.CustomEditors;
using FlaxEditor.GUI;
using FlaxEditor.GUI.Dialogs;
using FlaxEngine;
using FlaxEngine.GUI;

namespace FlaxEditor.Content.Create
{
    /// <summary>
    /// Dialog used to edit new file settings.
    /// </summary>
    /// <seealso cref="FlaxEditor.GUI.Dialogs.Dialog" />
    public class CreateFilesDialog : Dialog
    {
        private CreateFileEntry _entry;
        private CustomEditorPresenter _settingsEditor;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateFilesDialog"/> class.
        /// </summary>
        /// <param name="entry">The entry to edit it's settings.</param>
        public CreateFilesDialog(CreateFileEntry entry)
        : base("Create file settings")
        {
            _entry = entry ?? throw new ArgumentNullException();

            const float TotalWidth = 520;
            const float EditorHeight = 250;
            Width = TotalWidth;

            // Header and help description
            var headerLabel = new Label(0, 0, TotalWidth, 40)
            {
                Text = "Asset Options",
                DockStyle = DockStyle.Top,
                Parent = this,
                Font = new FontReference(Style.Current.FontTitle)
            };
            var infoLabel = new Label(10, headerLabel.Bottom + 5, TotalWidth - 20, 40)
            {
                Text = "Specify options for creating new asset",
                HorizontalAlignment = TextAlignment.Near,
                Margin = new Margin(7),
                DockStyle = DockStyle.Top,
                Parent = this
            };

            // Buttons
            const float ButtonsWidth = 60;
            const float ButtonsMargin = 8;
            var createButton = new Button(TotalWidth - ButtonsMargin - ButtonsWidth, infoLabel.Bottom - 30, ButtonsWidth)
            {
                Text = "Create",
                AnchorStyle = AnchorStyle.UpperRight,
                Parent = this
            };
            createButton.Clicked += OnCreate;
            var cancelButton = new Button(createButton.Left - ButtonsMargin - ButtonsWidth, createButton.Y, ButtonsWidth)
            {
                Text = "Cancel",
                AnchorStyle = AnchorStyle.UpperRight,
                Parent = this
            };
            cancelButton.Clicked += OnCancel;

            // Panel for settings editor
            var panel = new Panel(ScrollBars.Vertical)
            {
                Y = infoLabel.Bottom,
                Size = new Vector2(TotalWidth, EditorHeight),
                DockStyle = DockStyle.Fill,
                Parent = this
            };

            // Settings editor
            _settingsEditor = new CustomEditorPresenter(null);
            _settingsEditor.Panel.Parent = panel;

            Size = new Vector2(TotalWidth, panel.Bottom);

            _settingsEditor.Select(_entry.Settings);
        }

        private void OnCreate()
        {
            Editor.Instance.ContentImporting.LetThemBeCreatedxD(_entry);
            Close(DialogResult.OK);
        }

        private void OnCancel()
        {
            Close(DialogResult.Cancel);
        }

        private void OnSelectedChanged(List<TreeNode> before, List<TreeNode> after)
        {
            var selection = new List<object>(after.Count);
            for (int i = 0; i < after.Count; i++)
            {
                if (after[i].Tag is CreateFileEntry fileEntry && fileEntry.HasSettings)
                    selection.Add(fileEntry.Settings);
            }

            _settingsEditor.Select(selection);
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
