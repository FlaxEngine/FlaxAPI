// Copyright (c) 2012-2019 Wojciech Figat. All rights reserved.

using FlaxEngine;

namespace FlaxEditor.SceneGraph.Actors
{
    /// <summary>
    /// Scene tree node for <see cref="Skybox"/> actor type.
    /// </summary>
    /// <seealso cref="ActorNodeWithIcon" />
    public sealed class SkyboxNode : ActorNodeWithIcon
    {
        /// <inheritdoc />
        public SkyboxNode(Actor actor)
        : base(actor)
        {
        }
    }
}
