////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using FlaxEngine;

namespace FlaxEditor
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
            : base(null)
        {
        }

        #region [SceneTreeNodeBase] implementation

        /// <inheritdoc />
        public override string Name => "Root";

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

        /// <inheritdoc />
        public override Vector3 Position
        {
            get => Vector3.Zero;
            set { }
        }

        /// <inheritdoc />
        public override Quaternion Orientation
        {
            get => Quaternion.Identity;
            set { }
        }

        /// <inheritdoc />
        public override Vector3 Scale
        {
            get => Vector3.One;
            set { }
        }

        /// <inheritdoc />
        public override bool RayCastSelf(ref Ray ray, ref float distance)
        {
            return false;
        }

        #endregion
    }
}
