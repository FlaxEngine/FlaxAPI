// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

namespace FlaxEditor.Content
{
    /// <summary>
    /// Root tree node for the content workspace.
    /// </summary>
    /// <seealso cref="FlaxEditor.Content.ContentTreeNode" />
    public sealed class RootContentTreeNode : ContentTreeNode
    {
        private string _name;

        /// <summary>
        /// Initializes a new instance of the <see cref="RootContentTreeNode"/> class.
        /// </summary>
        public RootContentTreeNode()
        : base(null, string.Empty)
        {
            _name = Editor.Instance.ProjectInfo.Name;
        }

        /// <inheritdoc />
        public override string NavButtonLabel => _name;
    }
}
