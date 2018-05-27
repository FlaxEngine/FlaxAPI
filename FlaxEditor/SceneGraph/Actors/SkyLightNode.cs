// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using FlaxEngine;

namespace FlaxEditor.SceneGraph.Actors
{
    /// <summary>
    /// Scene tree node for <see cref="SkyLight"/> actor type.
    /// </summary>
    /// <seealso cref="ActorNodeWithIcon" />
    public sealed class SkyLightNode : ActorNodeWithIcon
    {
        /// <inheritdoc />
        public SkyLightNode(Actor actor)
        : base(actor)
        {
        }
    }
}
