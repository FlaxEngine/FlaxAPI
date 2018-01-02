////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2018 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using FlaxEngine;

namespace FlaxEditor.SceneGraph.Actors
{
    /// <summary>
    /// Scene tree node for <see cref="DirectionalLight"/> actor type.
    /// </summary>
    /// <seealso cref="ActorNodeWithIcon" />
    public sealed class DirectionalLightNode : ActorNodeWithIcon
    {
        /// <inheritdoc />
        public DirectionalLightNode(Actor actor)
            : base(actor)
        {
        }
    }
}
