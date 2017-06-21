////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

namespace FlaxEditor
{
    /// <summary>
    /// Represents root node of the whole scene graph.
    /// </summary>
    /// <seealso cref="FlaxEditor.ActorTreeNode" />
    public sealed class RootTreeNode : ActorTreeNode
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RootTreeNode"/> class.
        /// </summary>
        public RootTreeNode()
            : base(null)
        {
        }
    }
}
