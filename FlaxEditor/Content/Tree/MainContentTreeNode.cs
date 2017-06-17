////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

namespace FlaxEditor.Content
{
    /// <summary>
    /// Content tree node used for main directories.
    /// </summary>
    /// <seealso cref="FlaxEditor.Content.ContentTreeNode" />
    public class MainContentTreeNode : ContentTreeNode
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MainContentTreeNode"/> class.
        /// </summary>
        /// <param name="type">The folder type.</param>
        /// <param name="path">The folder path.</param>
        public MainContentTreeNode(ContentFolderType type, string path)
            : base(type, path)
        {
        }
    }
}
