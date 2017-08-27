////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

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
        /// <seealso cref="FlaxEditor.SceneGraph.SceneGraphNode" />
        public sealed class SideLinkNode : SceneGraphNode
        {
            private int _index;
            private Vector3 _offset;

            /// <summary>
            /// Initializes a new instance of the <see cref="SideLinkNode"/> class.
            /// </summary>
            /// <param name="actor">The parent node.</param>
            /// <param name="id">The identifier.</param>
            /// <param name="index">The index.</param>
            public SideLinkNode(PostFxVolumeNode actor, Guid id, int index)
                : base(id)
            {
                _index = index;

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

                ParentNode = actor;
            }

            /// <inheritdoc />
            public override string Name => ParentNode.Name + "." + _index;

            /// <inheritdoc />
            public override SceneNode ParentScene => ParentNode.ParentScene;

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
                    float centerScale = _index % 2 == 0 ? 0.5f : -0.5f;
                    actor.Size += localOffsetDelta;
                    actor.Center += localOffsetDelta * centerScale;
                }
            }

            /// <inheritdoc />
            public override bool IsActive => ParentNode.IsActive;

            /// <inheritdoc />
            public override bool IsActiveInHierarchy => ParentNode.IsActiveInHierarchy;

            /// <inheritdoc />
            public override int OrderInParent
            {
                get => _index;
                set { }
            }

            /// <inheritdoc />
            public override bool CanDelete => false;

            /// <inheritdoc />
            public override bool CanCopyPaste => false;

            /// <inheritdoc />
            public override bool CanDrag => false;

            /// <inheritdoc />
            public override object EditableObject => ParentNode.EditableObject;

            /// <inheritdoc />
            public override bool RayCastSelf(ref Ray ray, out float distance)
            {
                var sphere = new BoundingSphere(Transform.Translation, 1.0f);
                return sphere.Intersects(ref ray, out distance);
            }

            /// <inheritdoc />
            public override void OnDebugDraw(List<IntPtr> actorsPtr)
            {
                ParentNode.OnDebugDraw(actorsPtr);
            }
        }

        /// <inheritdoc />
        public PostFxVolumeNode(Actor actor)
            : base(actor)
        {
            var id = ID;
            var bytes = id.ToByteArray();
            bytes[0] += 1;
            new SideLinkNode(this, new Guid(bytes), 0);
            bytes[0] += 1;
            new SideLinkNode(this, new Guid(bytes), 1);
            bytes[0] += 1;
            new SideLinkNode(this, new Guid(bytes), 2);
            bytes[0] += 1;
            new SideLinkNode(this, new Guid(bytes), 3);
            bytes[0] += 1;
            new SideLinkNode(this, new Guid(bytes), 4);
            bytes[0] += 1;
            new SideLinkNode(this, new Guid(bytes), 5);
        }

        /// <inheritdoc />
        public override bool RayCastSelf(ref Ray ray, out float distance)
        {
            var volume = (PostFxVolume)_actor;
            var box = volume.OrientedBox;
            if (!box.Intersects(ref ray, out distance))
                return false;

            return true;
            //box.Scale(0.8f);
            //return !box.Intersects(ref ray, out distance);
        }
    }
}
