////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

namespace FlaxEngine.Rendering
{
    /// <summary>
    /// Rendering view description object. Contains information about viewport location and orientation in space.
    /// </summary>
    public struct RenderView
    {
        /// <summary>
        /// The position of the view.
        /// </summary>
        public Vector3 Position;

        /// <summary>
        /// The direction of the view.
        /// </summary>
        public Vector3 Direction;

        /// <summary>
        /// The near plane.
        /// </summary>
        public float Near;

        /// <summary>
        /// The far plane.
        /// </summary>
        public float Far;

        /// <summary>
        /// The view matrix.
        /// </summary>
        public Matrix View;

        /// <summary>
        /// The projection matrix.
        /// </summary>
        public Matrix Projection;

        /// <summary>
        /// Flag used by static, offline rendering passes (eg. reflections rendering, lightmap rendering etc.)
        /// </summary>
        public bool IsOfflinePass;

        /// <summary>
        /// Maximum allowed shadows quality for this view
        /// </summary>
        public Quality MaxShadowsQuality;

        /// <summary>
        /// The view flags.
        /// </summary>
        public ViewFlags Flags;

        /// <summary>
        /// The view mode.
        /// </summary>
        public ViewMode Move;
    }
}
