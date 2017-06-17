////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using FlaxEngine;
using FlaxEngine.GUI;

namespace FlaxEditor.Content
{
    /// <summary>
    /// Content folder tree node.
    /// </summary>
    /// <seealso cref="FlaxEngine.GUI.TreeNode" />
    public class ContentTreeNode : TreeNode
    {
        protected ContentFolder _folder;

        /// <summary>
        /// Gets the content folder item.
        /// </summary>
        /// <value>
        /// The folder.
        /// </value>
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
        /// <returns>True if folder belogns to the project workspace otherwise false</returns>
        public bool IsProjectOnly => _folder.IsProjectOnly;

        /// <summary>
        /// Returns true if that folder belongs to the Engine or Editor private files.
        /// </summary>
        /// <returns>True if folder belogns to Engine private files otherwise false</returns>
        public bool IsEnginePrivate => _folder.IsEnginePrivate;

        /// <summary>
        /// Gets the parent node.
        /// </summary>
        /// <value>
        /// The parent node.
        /// </value>
        public ContentTreeNode ParentNode => Parent as ContentTreeNode;

        /// <summary>
        /// Gets the folderpath.
        /// </summary>
        /// <value>
        /// The path.
        /// </value>
        public string Path => _folder.Path;

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
            : base(false, Editor.Instance.UI.FolderClosed12, Editor.Instance.UI.FolderOpened12)
        {
            _folder = new ContentFolder(type, path, this);
        }

        /// <inheritdoc />
        public override void OnDestroy()
        {
            // Delete folder item
            _folder.Dispose();

            base.OnDestroy();
        }
    }
}
