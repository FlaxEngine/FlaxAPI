// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using FlaxEngine;

namespace FlaxEditor.SceneGraph.Actors
{
    /// <summary>
    /// Scene tree node for <see cref="BoneSocket"/> actor type.
    /// </summary>
    /// <seealso cref="ActorNode" />
    public sealed class BoneSocketNode : ActorNode
    {
        /// <inheritdoc />
        public BoneSocketNode(Actor actor)
        : base(actor)
        {
        }
    }
}
