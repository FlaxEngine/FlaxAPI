// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System.IO;
using FlaxEngine;

namespace FlaxEditor.Content
{
    /// <summary>
    /// Content tree node used for main directories.
    /// </summary>
    /// <seealso cref="FlaxEditor.Content.ContentTreeNode" />
    public class MainContentTreeNode : ContentTreeNode
    {
        private FileSystemWatcher _watcher;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainContentTreeNode"/> class.
        /// </summary>
        /// <param name="type">The folder type.</param>
        /// <param name="path">The folder path.</param>
        public MainContentTreeNode(ContentFolderType type, string path)
        : base(type, path)
        {
            _watcher = new FileSystemWatcher(path)
            {
                IncludeSubdirectories = true,
                EnableRaisingEvents = true
            };
            _watcher.Changed += onEvent;
            _watcher.Created += onEvent;
            _watcher.Deleted += onEvent;
            _watcher.Renamed += onEvent;
        }

        private void onEvent(object sender, FileSystemEventArgs e)
        {
            Editor.Instance.ContentDatabase.OnDirectoryEvent(this, e);
        }

        /// <inheritdoc />
        protected override void DoDragDrop()
        {
            // No drag for root nodes
        }

        /// <inheritdoc />
        public override void OnDestroy()
        {
            _watcher.EnableRaisingEvents = false;
            _watcher.Dispose();

            base.OnDestroy();
        }
    }
}
