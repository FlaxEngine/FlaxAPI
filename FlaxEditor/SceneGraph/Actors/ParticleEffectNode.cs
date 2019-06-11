// Copyright (c) 2012-2019 Wojciech Figat. All rights reserved.

using FlaxEngine;

namespace FlaxEditor.SceneGraph.Actors
{
    /// <summary>
    /// Scene tree node for <see cref="ParticleEffect"/> actor type.
    /// </summary>
    /// <seealso cref="ActorNodeWithIcon" />
    public sealed class ParticleEffectNode : ActorNodeWithIcon
    {
        /// <inheritdoc />
        public ParticleEffectNode(Actor actor)
        : base(actor)
        {
        }
    }
}
