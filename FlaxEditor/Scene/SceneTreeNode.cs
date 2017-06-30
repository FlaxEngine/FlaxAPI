////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

namespace FlaxEditor
{
    /// <summary>
    /// Actor tree node for <see cref="FlaxEngine.Scene"/> objects.
    /// </summary>
    /// <seealso cref="ActorTreeNode" />
    public sealed class SceneTreeNode : ActorTreeNode
    {
        /// <summary>
        /// Gets the scene.
        /// </summary>
        /// <value>
        /// The scene.
        /// </value>
        public FlaxEngine.Scene Scene => _actor as FlaxEngine.Scene;

        /// <summary>
        /// Initializes a new instance of the <see cref="SceneTreeNode"/> class.
        /// </summary>
        /// <param name="scene">The scene.</param>
        public SceneTreeNode(FlaxEngine.Scene scene)
            : base(scene)
        {
        }
    }
}
