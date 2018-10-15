// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using FlaxEditor.GUI;
using FlaxEditor.GUI.Drag;
using FlaxEngine;
using FlaxEngine.GUI;

namespace FlaxEditor.Content
{
    /// <summary>
    /// Content folder tree node.
    /// </summary>
    /// <seealso cref="TreeNode" />
    public class ContentTreeNode : TreeNode
    {
        private DragItems _dragOverItems;

        /// <summary>
        /// The folder.
        /// </summary>
        protected ContentFolder _folder;

        /// <summary>
        /// Gets the content folder item.
        /// </summary>
        public ContentFolder Folder => _folder;

        /// <summary>
        /// Gets the type of the folder.
        /// </summary>
        /// <value>
        /// The type of the folder.
        /// </value>
        public ContentFolderType FolderType => _folder.FolderType;

        /// <summary>
        /// Returns true if that folder can import/manage scripts.
        /// </summary>
        /// <returns>True if can contain scripts for project, otherwise false</returns>
        public bool CanHaveScripts => _folder.CanHaveScripts;

        /// <summary>
        /// Returns true if that folder can import/manage assets.
        /// </summary>
        /// <returns>True if can contain assets for project, otherwise false</returns>
        public bool CanHaveAssets => _folder.CanHaveAssets;

        /// <summary>
        /// Returns true if that folder belongs to the project workspace.
        /// </summary>
        /// <returns>True if folder belongs to the project workspace otherwise false</returns>
        public bool IsProjectOnly => _folder.IsProjectOnly;

        /// <summary>
        /// Returns true if that folder belongs to the Engine or Editor private files.
        /// </summary>
        /// <returns>True if folder belongs to Engine private files otherwise false</returns>
        public bool IsEnginePrivate => _folder.IsEnginePrivate;

        /// <summary>
        /// Gets the parent node.
        /// </summary>
        public ContentTreeNode ParentNode => Parent as ContentTreeNode;

        /// <summary>
        /// Gets the folder path.
        /// </summary>
        public string Path => _folder.Path;

        /// <summary>
        /// Gets the navigation button label.
        /// </summary>
        public virtual string NavButtonLabel => _folder.ShortName;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContentTreeNode"/> class.
        /// </summary>
        /// <param name="parent">The parent node.</param>
        /// <param name="path">The folder path.</param>
        public ContentTreeNode(ContentTreeNode parent, string path)
        : this(parent?.FolderType ?? ContentFolderType.Other, path)
        {
            if (parent != null)
            {
                // Link
                Folder.ParentFolder = parent.Folder;
                Parent = parent;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContentTreeNode"/> class.
        /// </summary>
        /// <param name="type">The folder type.</param>
        /// <param name="path">The folder path.</param>
        protected ContentTreeNode(ContentFolderType type, string path)
        : base(false, Editor.Instance.Icons.FolderClosed12, Editor.Instance.Icons.FolderOpened12)
        {
            _folder = new ContentFolder(type, path, this);
            Text = _folder.ShortName;
        }

        /// <summary>
        /// Shows the rename popup for the item.
        /// </summary>
        public void StartRenaming()
        {
            if (!_folder.CanRename)
                return;

            // Start renaming the folder
            var dialog = RenamePopup.Show(this, HeaderRect, _folder.ShortName, false);
            dialog.Tag = _folder;
            dialog.Renamed += popup => Editor.Instance.Windows.ContentWin.Rename((ContentFolder)popup.Tag, popup.Text);
        }

        /// <inheritdoc />
        public override void OnDestroy()
        {
            // Delete folder item
            _folder.Dispose();

            base.OnDestroy();
        }

        private DragDropEffect GetDragEffect(DragData data)
        {
            if (data is DragDataFiles)
            {
                if (_folder.CanHaveAssets)
                    return DragDropEffect.Copy;
            }
            else
            {
                if (_dragOverItems.HasValidDrag)
                    return DragDropEffect.Move;
            }

            return DragDropEffect.None;
        }

        private bool ValidateDragItem(ContentItem item)
        {
            // Reject itself and any parent
            return item != _folder && !item.Find(_folder);
        }

        /// <inheritdoc />
        protected override DragDropEffect OnDragEnterHeader(DragData data)
        {
            if (_dragOverItems == null)
                _dragOverItems = new DragItems(ValidateDragItem);

            _dragOverItems.OnDragEnter(data);
            return GetDragEffect(data);
        }

        /// <inheritdoc />
        protected override DragDropEffect OnDragMoveHeader(DragData data)
        {
            return GetDragEffect(data);
        }

        /// <inheritdoc />
        protected override void OnDragLeaveHeader()
        {
            _dragOverItems.OnDragLeave();
            base.OnDragLeaveHeader();
        }

        /// <inheritdoc />
        protected override DragDropEffect OnDragDropHeader(DragData data)
        {
            var result = DragDropEffect.None;

            // Check if drop element or files
            if (data is DragDataFiles files)
            {
                // Import files
                Editor.Instance.ContentImporting.Import(files.Files, _folder);
                result = DragDropEffect.Copy;

                Expand();
            }
            else if (_dragOverItems.HasValidDrag)
            {
                // Move items
                Editor.Instance.ContentDatabase.Move(_dragOverItems.Objects, _folder);
                result = DragDropEffect.Move;

                Expand();
            }

            _dragOverItems.OnDragDrop();

            return result;
        }

        /// <inheritdoc />
        protected override void DoDragDrop()
        {
            DoDragDrop(DragItems.GetDragData(_folder));
        }

        /// <inheritdoc />
        protected override void OnLongPress()
        {
            Select();

            StartRenaming();
        }

        /// <inheritdoc />
        public override bool OnKeyDown(Keys key)
        {
            if (IsFocused)
            {
                if (key == Keys.F2)
                {
                    StartRenaming();
                    return true;
                }
            }

            return base.OnKeyDown(key);
        }
    }
}
