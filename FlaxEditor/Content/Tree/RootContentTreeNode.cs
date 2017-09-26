////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

namespace FlaxEditor.Content
{
    /// <summary>
    /// Root tree node for the content workspace.
    /// </summary>
    /// <seealso cref="FlaxEditor.Content.ContentTreeNode" />
    public sealed class RootContentTreeNode : ContentTreeNode
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RootContentTreeNode"/> class.
        /// </summary>
        /// <inheritdoc />
        public RootContentTreeNode()
            : base(null, string.Empty)
        {
        }

        /// <inheritdoc />
        public override string NavButtonLabel => Editor.ProjectName;
    }
}
