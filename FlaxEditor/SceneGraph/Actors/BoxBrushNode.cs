// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System;
using FlaxEngine;

namespace FlaxEditor.SceneGraph.Actors
{
    /// <summary>
    /// Actor node for <see cref="BoxBrush"/>.
    /// </summary>
    /// <seealso cref="ActorNode" />
    public sealed class BoxBrushNode : ActorNode
    {
        /// <summary>
        /// Sub actor node used to edit volume.
        /// </summary>
        /// <seealso cref="FlaxEditor.SceneGraph.ActorChildNode{T}" />
        public sealed class SideLinkNode : ActorChildNode<BoxBrushNode>
        {
            private Vector3 _offset;

            /// <summary>
            /// Gets the brush actor.
            /// </summary>
            public BoxBrush Brush => (BoxBrush)((BoxBrushNode)ParentNode).Actor;

            /// <summary>
            /// Gets the brush surface.
            /// </summary>
            public BrushSurface Surface => Brush.Surfaces[Index];

            /// <summary>
            /// Initializes a new instance of the <see cref="SideLinkNode"/> class.
            /// </summary>
            /// <param name="actor">The parent node.</param>
            /// <param name="id">The identifier.</param>
            /// <param name="index">The index.</param>
            public SideLinkNode(BoxBrushNode actor, Guid id, int index)
            : base(actor, id, index)
            {
                switch (index)
                {
                case 0:
                    _offset = new Vector3(0.5f, 0, 0);
                    break;
                case 1:
                    _offset = new Vector3(-0.5f, 0, 0);
                    break;
                case 2:
                    _offset = new Vector3(0, 0.5f, 0);
                    break;
                case 3:
                    _offset = new Vector3(0, -0.5f, 0);
                    break;
                case 4:
                    _offset = new Vector3(0, 0, 0.5f);
                    break;
                case 5:
                    _offset = new Vector3(0, 0, -0.5f);
                    break;
                }
            }

            /// <inheritdoc />
            public override Transform Transform
            {
                get
                {
                    var actor = Brush;
                    var localOffset = _offset * actor.Size + actor.Center;
                    Transform localTrans = new Transform(localOffset);
                    return actor.Transform.LocalToWorld(localTrans);
                }
                set
                {
                    var actor = Brush;
                    Transform localTrans = actor.Transform.WorldToLocal(value);
                    var prevLocalOffset = _offset * actor.Size + actor.Center;
                    var localOffset = Vector3.Abs(_offset) * 2.0f * localTrans.Translation;
                    var localOffsetDelta = localOffset - prevLocalOffset;
                    float centerScale = Index % 2 == 0 ? 0.5f : -0.5f;
                    actor.Size += localOffsetDelta;
                    actor.Center += localOffsetDelta * centerScale;
                }
            }

            /// <inheritdoc />
            public override object EditableObject => Brush.Surfaces[Index];

            /// <inheritdoc />
            public override bool RayCastSelf(ref RayCastData ray, out float distance)
            {
                return Brush.Surfaces[Index].Intersects(ref ray.Ray, out distance);
            }

            /// <inheritdoc />
            public override void OnDebugDraw(ViewportDebugDrawData data)
            {
                ParentNode.OnDebugDraw(data);
                data.HighlightBrushSurface(Brush.Surfaces[Index]);
            }
        }

        /// <inheritdoc />
        public BoxBrushNode(Actor actor)
        : base(actor)
        {
            var id = ID;
            var bytes = id.ToByteArray();
            for (int i = 0; i < 6; i++)
            {
                bytes[0] += 1;
                AddChildNode(new SideLinkNode(this, new Guid(bytes), i));
            }
        }

        /// <inheritdoc />
        public override bool RayCastSelf(ref RayCastData ray, out float distance)
        {
            if (((BoxBrush)_actor).OrientedBox.Intersects(ref ray.Ray))
            {
                for (int i = 0; i < ChildNodes.Count; i++)
                {
                    if (ChildNodes[i] is SideLinkNode node && node.RayCastSelf(ref ray, out distance))
                        return true;
                }
            }

            distance = 0;
            return false;
        }
    }
}
