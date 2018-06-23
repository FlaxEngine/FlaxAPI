// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System;
using FlaxEditor.SceneGraph.Actors;
using FlaxEngine;

namespace FlaxEditor.SceneGraph
{
    /// <summary>
    /// Represents root node of the whole scene graph.
    /// </summary>
    /// <seealso cref="ActorNode" />
    public sealed class RootNode : ActorNode
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RootNode"/> class.
        /// </summary>
        public RootNode()
        : base(null, new Guid(1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1))
        {
        }

        /// <inheritdoc />
        public override string Name => "Root";

        /// <inheritdoc />
        public override SceneNode ParentScene => null;

        /// <inheritdoc />
        public override bool CanCopyPaste => false;

        /// <inheritdoc />
        public override bool CanDelete => false;

        /// <inheritdoc />
        public override bool CanDrag => false;

        /// <inheritdoc />
        public override bool IsActive => true;

        /// <inheritdoc />
        public override bool IsActiveInHierarchy => true;

        /// <inheritdoc />
        public override Transform Transform
        {
            get => Transform.Identity;
            set { }
        }

        /// <summary>
        /// Performs raycasting over nodes hierarchy trying to get the closest object hited by the given ray.
        /// </summary>
        /// <param name="ray">The ray.</param>
        /// <param name="distance">The result distance.</param>
        /// <param name="flags">The raycasting flags.</param>
        /// <returns>Hitted object or null if there is no interseciotn at all.</returns>
        public SceneGraphNode RayCast(ref Ray ray, ref float distance, RayCastData.FlagTypes flags = RayCastData.FlagTypes.None)
        {
            RayCastData data;
            data.Ray = ray;
            data.Flags = flags;

            return RayCast(ref data, ref distance);
        }

        /// <inheritdoc />
        public override bool RayCastSelf(ref RayCastData ray, out float distance)
        {
            distance = 0;
            return false;
        }

        /// <inheritdoc />
        public override void OnDebugDraw(ViewportDebugDrawData data)
        {
        }

        /// <inheritdoc />
        public override void Delete()
        {
        }
    }
}
