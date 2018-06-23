// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System;
using System.Collections.Generic;
using FlaxEngine;

namespace FlaxEditor.SceneGraph.Actors
{
    /// <summary>
    /// Actor node for <see cref="PostFxVolume"/>.
    /// </summary>
    /// <seealso cref="ActorNode" />
    public sealed class PostFxVolumeNode : ActorNode
    {
        /// <summary>
        /// Sub actor node used to edit volume.
        /// </summary>
        /// <seealso cref="FlaxEditor.SceneGraph.ActorChildNode{T}" />
        public sealed class SideLinkNode : ActorChildNode<PostFxVolumeNode>
        {
            private Vector3 _offset;

            /// <summary>
            /// Initializes a new instance of the <see cref="SideLinkNode"/> class.
            /// </summary>
            /// <param name="actor">The parent node.</param>
            /// <param name="id">The identifier.</param>
            /// <param name="index">The index.</param>
            public SideLinkNode(PostFxVolumeNode actor, Guid id, int index)
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
                    var actor = (PostFxVolume)((PostFxVolumeNode)ParentNode).Actor;
                    var localOffset = _offset * actor.Size + actor.Center;
                    Transform localTrans = new Transform(localOffset);
                    return actor.Transform.LocalToWorld(localTrans);
                }
                set
                {
                    var actor = (PostFxVolume)((PostFxVolumeNode)ParentNode).Actor;
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
            public override bool RayCastSelf(ref RayCastData ray, out float distance)
            {
                var sphere = new BoundingSphere(Transform.Translation, 1.0f);
                return sphere.Intersects(ref ray.Ray, out distance);
            }

            /// <inheritdoc />
            public override void OnDebugDraw(ViewportDebugDrawData data)
            {
                ParentNode.OnDebugDraw(data);
            }
        }

        /// <inheritdoc />
        public PostFxVolumeNode(Actor actor)
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
            var volume = (PostFxVolume)_actor;
            var box = volume.OrientedBox;
            if (!box.Intersects(ref ray.Ray, out distance))
                return false;

            return true;
            //box.Scale(0.8f);
            //return !box.Intersects(ref ray, out distance);
        }
    }
}
