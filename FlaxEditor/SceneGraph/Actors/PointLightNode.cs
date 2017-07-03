////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using FlaxEngine;

namespace FlaxEditor.SceneGraph.Actors
{
    /// <summary>
    /// Scene tree node for <see cref="PointLight"/> actor type.
    /// </summary>
    /// <seealso cref="ActorNode" />
    public sealed class PointLightNode : ActorNode
    {
        /// <inheritdoc />
        public PointLightNode(Actor actor)
            : base(actor)
        {
        }
    }
}
