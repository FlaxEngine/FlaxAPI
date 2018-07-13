// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using FlaxEngine;

namespace FlaxEditor.SceneGraph.Actors
{
    /// <summary>
    /// Scene tree node for <see cref="SpotLight"/> actor type.
    /// </summary>
    /// <seealso cref="ActorNodeWithIcon" />
    public sealed class SpotLightNode : ActorNodeWithIcon
    {
        /// <inheritdoc />
        public SpotLightNode(Actor actor)
        : base(actor)
        {
        }

        /// <inheritdoc />
        public override void OnDebugDraw(ViewportDebugDrawData data)
        {
            base.OnDebugDraw(data);

            var transform = Actor.Transform;
            DebugDraw.DrawArrow(transform.Translation, transform.Orientation, Color.Red, 0.3f, 0.0f, false);
        }
    }
}
