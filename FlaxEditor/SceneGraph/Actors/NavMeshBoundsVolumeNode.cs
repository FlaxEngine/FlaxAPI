// Copyright (c) 2012-2020 Wojciech Figat. All rights reserved.

using System;
using FlaxEngine;

namespace FlaxEditor.SceneGraph.Actors
{
    /// <summary>
    /// Actor node for <see cref="NavMeshBoundsVolume"/>.
    /// </summary>
    /// <seealso cref="ActorNode" />
    public sealed class NavMeshBoundsVolumeNode : ActorNode
    {
        /// <summary>
        /// Sub actor node used to edit volume.
        /// </summary>
        /// <seealso cref="FlaxEditor.SceneGraph.ActorChildNode{T}" />
        public sealed class SideLinkNode : ActorChildNode<NavMeshBoundsVolumeNode>
        {
            private Vector3 _offset;

            /// <summary>
            /// Initializes a new instance of the <see cref="SideLinkNode"/> class.
            /// </summary>
            /// <param name="actor">The parent node.</param>
            /// <param name="id">The identifier.</param>
            /// <param name="index">The index.</param>
            public SideLinkNode(NavMeshBoundsVolumeNode actor, Guid id, int index)
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
                    var actor = (NavMeshBoundsVolume)((NavMeshBoundsVolumeNode)ParentNode).Actor;
                    var localOffset = _offset * actor.Size;
                    Transform localTrans = new Transform(localOffset);
                    return actor.Transform.LocalToWorld(localTrans);
                }
                set
                {
                    var actor = (NavMeshBoundsVolume)((NavMeshBoundsVolumeNode)ParentNode).Actor;
                    Transform localTrans = actor.Transform.WorldToLocal(value);
                    var prevLocalOffset = _offset * actor.Size;
                    var localOffset = Vector3.Abs(_offset) * 2.0f * localTrans.Translation;
                    var localOffsetDelta = localOffset - prevLocalOffset;
                    float centerScale = Index % 2 == 0 ? 1.0f : -1.0f;
                    actor.Size += localOffsetDelta * centerScale;
                    actor.Position += localOffsetDelta * 0.5f;
                }
            }

            /// <inheritdoc />
            public override bool RayCastSelf(ref RayCastData ray, out float distance, out Vector3 normal)
            {
                normal = Vector3.Up;
                var sphere = new BoundingSphere(Transform.Translation, 5.0f);
                return sphere.Intersects(ref ray.Ray, out distance);
            }

            /// <inheritdoc />
            public override void OnDebugDraw(ViewportDebugDrawData data)
            {
                ParentNode.OnDebugDraw(data);
            }
        }

        /// <inheritdoc />
        public NavMeshBoundsVolumeNode(Actor actor)
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
        public override bool RayCastSelf(ref RayCastData ray, out float distance, out Vector3 normal)
        {
            normal = Vector3.Up;

            // Check if skip raycasts
            if ((ray.Flags & RayCastData.FlagTypes.SkipEditorPrimitives) == RayCastData.FlagTypes.SkipEditorPrimitives)
            {
                distance = 0;
                return false;
            }

            // Skip itself if any link gets hit
            if (RayCastChildren(ref ray, out distance, out normal) != null)
                return false;

            // Check itself
            var volume = (NavMeshBoundsVolume)_actor;
            var box = volume.OrientedBox;
            return box.Contains(ref ray.Ray.Position) == ContainmentType.Disjoint && box.Intersects(ref ray.Ray, out distance);
        }
    }
}
