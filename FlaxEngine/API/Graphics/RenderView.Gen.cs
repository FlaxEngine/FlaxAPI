// This code was auto-generated. Do not modify it.

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace FlaxEngine
{
    /// <summary>
    /// Rendering view description that defines how to render the objects (camera placement, rendering properties, etc.).
    /// </summary>
    [Tooltip("Rendering view description that defines how to render the objects (camera placement, rendering properties, etc.).")]
    [StructLayout(LayoutKind.Sequential)]
    public unsafe partial struct RenderView
    {
        /// <summary>
        /// The position of the view.
        /// </summary>
        [Tooltip("The position of the view.")]
        public Vector3 Position;

        /// <summary>
        /// The direction of the view.
        /// </summary>
        [Tooltip("The direction of the view.")]
        public Vector3 Direction;

        /// <summary>
        /// The near plane.
        /// </summary>
        [Tooltip("The near plane.")]
        public float Near;

        /// <summary>
        /// The far plane.
        /// </summary>
        [Tooltip("The far plane.")]
        public float Far;

        /// <summary>
        /// The view matrix.
        /// </summary>
        [Tooltip("The view matrix.")]
        public Matrix View;

        /// <summary>
        /// The projection matrix.
        /// </summary>
        [Tooltip("The projection matrix.")]
        public Matrix Projection;

        /// <summary>
        /// The projection matrix with no camera offset (no jittering).
        /// For many temporal image effects, the camera that is currently rendering needs to be slightly offset from the default projection (that is, the camera is ‘jittered’).
        /// If you use motion vectors and camera jittering together, use this property to keep the motion vectors stable between frames.
        /// </summary>
        public Matrix NonJitteredProjection;

        /// <summary>
        /// The inverted view matrix.
        /// </summary>
        [Tooltip("The inverted view matrix.")]
        public Matrix IV;

        /// <summary>
        /// The inverted projection matrix.
        /// </summary>
        [Tooltip("The inverted projection matrix.")]
        public Matrix IP;

        /// <summary>
        /// The inverted projection view matrix.
        /// </summary>
        [Tooltip("The inverted projection view matrix.")]
        public Matrix IVP;

        /// <summary>
        /// The view frustum.
        /// </summary>
        [Tooltip("The view frustum.")]
        public BoundingFrustum Frustum;

        /// <summary>
        /// The view frustum used for culling (can be different than Frustum in some cases e.g. cascaded shadow map rendering).
        /// </summary>
        [Tooltip("The view frustum used for culling (can be different than Frustum in some cases e.g. cascaded shadow map rendering).")]
        public BoundingFrustum CullingFrustum;

        /// <summary>
        /// The draw passes mask for the current view rendering.
        /// </summary>
        [Tooltip("The draw passes mask for the current view rendering.")]
        public DrawPass Pass;

        /// <summary>
        /// Flag used by static, offline rendering passes (eg. reflections rendering, lightmap rendering etc.)
        /// </summary>
        [Tooltip("Flag used by static, offline rendering passes (eg. reflections rendering, lightmap rendering etc.)")]
        public bool IsOfflinePass;

        /// <summary>
        /// The static flags mask used to hide objects that don't have a given static flags. Eg. use StaticFlags::Lightmap to render only objects that can use lightmap.
        /// </summary>
        [Tooltip("The static flags mask used to hide objects that don't have a given static flags. Eg. use StaticFlags::Lightmap to render only objects that can use lightmap.")]
        public StaticFlags StaticFlagsMask;

        /// <summary>
        /// The view flags.
        /// </summary>
        [Tooltip("The view flags.")]
        public ViewFlags Flags;

        /// <summary>
        /// The view mode.
        /// </summary>
        [Tooltip("The view mode.")]
        public ViewMode Mode;

        /// <summary>
        /// Maximum allowed shadows quality for this view
        /// </summary>
        [Tooltip("Maximum allowed shadows quality for this view")]
        public Quality MaxShadowsQuality;

        /// <summary>
        /// The model LOD bias. Default is 0. Applied to all the objects in the render view.
        /// </summary>
        [Tooltip("The model LOD bias. Default is 0. Applied to all the objects in the render view.")]
        public int ModelLODBias;

        /// <summary>
        /// The model LOD distance scale factor. Default is 1. Applied to all the objects in the render view. Higher values increase LODs quality.
        /// </summary>
        [Tooltip("The model LOD distance scale factor. Default is 1. Applied to all the objects in the render view. Higher values increase LODs quality.")]
        public float ModelLODDistanceFactor;

        /// <summary>
        /// The model LOD bias. Default is 0. Applied to all the objects in the shadow maps render views. Can be used to improve shadows rendering performance or increase quality.
        /// </summary>
        [Tooltip("The model LOD bias. Default is 0. Applied to all the objects in the shadow maps render views. Can be used to improve shadows rendering performance or increase quality.")]
        public int ShadowModelLODBias;

        /// <summary>
        /// The model LOD distance scale factor. Default is 1. Applied to all the objects in the shadow maps render views. Higher values increase LODs quality. Can be used to improve shadows rendering performance or increase quality.
        /// </summary>
        [Tooltip("The model LOD distance scale factor. Default is 1. Applied to all the objects in the shadow maps render views. Higher values increase LODs quality. Can be used to improve shadows rendering performance or increase quality.")]
        public float ShadowModelLODDistanceFactor;

        /// <summary>
        /// The Temporal Anti-Aliasing jitter frame index.
        /// </summary>
        [Tooltip("The Temporal Anti-Aliasing jitter frame index.")]
        public int TaaFrameIndex;

        /// <summary>
        /// The view information vector with packed components to reconstruct linear depth and view position from the hardware depth buffer. Cached before rendering.
        /// </summary>
        [Tooltip("The view information vector with packed components to reconstruct linear depth and view position from the hardware depth buffer. Cached before rendering.")]
        public Vector4 ViewInfo;

        /// <summary>
        /// The screen size packed (x - width, y - height, zw - inv width, w - inv height). Cached before rendering.
        /// </summary>
        [Tooltip("The screen size packed (x - width, y - height, zw - inv width, w - inv height). Cached before rendering.")]
        public Vector4 ScreenSize;

        /// <summary>
        /// The temporal AA jitter packed (xy - this frame jitter, zw - previous frame jitter). Cached before rendering. Zero if TAA is disabled. The value added to projection matrix (in clip space).
        /// </summary>
        [Tooltip("The temporal AA jitter packed (xy - this frame jitter, zw - previous frame jitter). Cached before rendering. Zero if TAA is disabled. The value added to projection matrix (in clip space).")]
        public Vector4 TemporalAAJitter;

        /// <summary>
        /// The previous frame view matrix.
        /// </summary>
        [Tooltip("The previous frame view matrix.")]
        public Matrix PrevView;

        /// <summary>
        /// The previous frame projection matrix.
        /// </summary>
        [Tooltip("The previous frame projection matrix.")]
        public Matrix PrevProjection;

        /// <summary>
        /// The previous frame view * projection matrix.
        /// </summary>
        [Tooltip("The previous frame view * projection matrix.")]
        public Matrix PrevViewProjection;

        /// <summary>
        /// Square of <see cref="ModelLODDistanceFactor"/>. Cached by rendering backend.
        /// </summary>
        [Tooltip("Square of <see cref=\"ModelLODDistanceFactor\"/>. Cached by rendering backend.")]
        public float ModelLODDistanceFactorSqrt;
    }
}
