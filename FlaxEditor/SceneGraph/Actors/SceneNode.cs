////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

namespace FlaxEditor.SceneGraph.Actors
{
    /// <summary>
    /// Actor tree node for <see cref="FlaxEngine.Scene"/> objects.
    /// </summary>
    /// <seealso cref="ActorNode" />
    public sealed class SceneNode : ActorNode
    {
        /// <summary>
        /// Gets the scene.
        /// </summary>
        /// <value>
        /// The scene.
        /// </value>
        public FlaxEngine.Scene Scene => _actor as FlaxEngine.Scene;

        /// <summary>
        /// Initializes a new instance of the <see cref="SceneNode"/> class.
        /// </summary>
        /// <param name="scene">The scene.</param>
        public SceneNode(FlaxEngine.Scene scene)
            : base(scene)
        {
        }
    }
}
