////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.IO;
using FlaxEditor.GUI.Dialogs;
using FlaxEngine;
using FlaxEngine.GUI;

namespace FlaxEditor.Content.Import
{
    /// <summary>
    /// Dialog used to edit import files settings.
    /// </summary>
    /// <seealso cref="FlaxEditor.GUI.Dialogs.Dialog" />
    public class ImportFilesDialog : Dialog
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ImportFilesDialog"/> class.
        /// </summary>
        /// <param name="entries">The entries to edit settings.</param>
        public ImportFilesDialog(List<FileEntry> entries)
            : base("Import files settings")
        {
            if (entries == null || entries.Count < 1)
                throw new ArgumentNullException();
            
            const float TotalWidth = 920;
            const float EditorHeight = 450;

            // Header and help description
            var headerLabel = new Label(false, 0, 0, TotalWidth, 40);
            headerLabel.Text = "Import settings";
            headerLabel.Parent = this;
            headerLabel.Font = Style.Current.FontTitle;
            var infoLabel = new Label(false, 10, headerLabel.Bottom + 5, TotalWidth - 20, 40);
            infoLabel.Text = "Specify options for importing files. Every file can have different settings. Select entries on the left panel to modify them.\nPro Tip: hold CTRL key and select entries to edit multiple at once.";
            infoLabel.HorizontalAlignment = TextAlignment.Near;
            infoLabel.Parent = this;

            // TODO: ok button

            // TODO: cancel button

            // Split panel for entries list and settings editor
            var splitPanel = new SplitPanel(Orientation.Horizontal, ScrollBars.Vertical, ScrollBars.Vertical);
            splitPanel.Y = infoLabel.Bottom;
            splitPanel.Size = new Vector2(TotalWidth, EditorHeight);
            splitPanel.Parent = this;
            
            // TODO: add custom editor presenter for per entry settings editing

            // Setup tree
            var tree = new Tree(true);
            tree.DockStyle = DockStyle.Top;
            tree.Parent = splitPanel.Panel1;
            var root = new TreeNode(false);
            for (int i = 0; i < entries.Count; i++)
            {
                var entry = entries[i];
                
                // TODO: add icons for textures/models/etc from FileEntry to tree node??
                var node = new TreeNode(false);
                node.Text = Path.GetFileName(entry.Url);
                node.Tag = entry;
                node.Parent = root;
            }
            root.Expand();
            root.Parent = tree;
            tree.OnSelectedChanged += OnSelectedChanged;

            // Select the first item
            tree.Select(root.Children[0] as TreeNode);

            Size = new Vector2(TotalWidth, splitPanel.Bottom);
        }

        private void OnSelectedChanged(List<TreeNode> before, List<TreeNode> after)
        {
            List<object> selection = new List<object>(after.Count);
            for (int i = 0; i < after.Count; i++)
            {
                var fileEntry = after[i].Tag as FileEntry;
                if (fileEntry != null)
                    selection.Add(fileEntry.Settings);
            }
            
            // TODO: select objects
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
