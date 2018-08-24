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
    public abstract class RootNode : ActorNode
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RootNode"/> class.
        /// </summary>
        protected RootNode()
        : base(null, Guid.NewGuid())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RootNode"/> class.
        /// </summary>
        /// <param name="id">The node id.</param>
        public RootNode(Guid id)
        : base(null, id)
        {
        }

        /// <inheritdoc />
        public override string Name => "Root";

        /// <inheritdoc />
        public override SceneNode ParentScene => null;

        /// <inheritdoc />
        public override RootNode Root => this;

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
        /// Performs raycasting over nodes hierarchy trying to get the closest object hit by the given ray.
        /// </summary>
        /// <param name="ray">The ray.</param>
        /// <param name="distance">The result distance.</param>
        /// <param name="flags">The raycasting flags.</param>
        /// <returns>Hit object or null if there is no intersection at all.</returns>
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

        /// <summary>
        /// Spawns the specified actor.
        /// </summary>
        /// <param name="actor">The actor.</param>
        /// <param name="parent">The parent.</param>
        public abstract void Spawn(Actor actor, Actor parent);

        /// <summary>
        /// Gets the undo.
        /// </summary>
        public abstract Undo Undo { get; }
    }
}
