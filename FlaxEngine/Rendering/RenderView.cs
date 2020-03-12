// Copyright (c) 2012-2020 Wojciech Figat. All rights reserved.

using System.Runtime.InteropServices;

namespace FlaxEngine
{
    /// <summary>
    /// Rendering view description object. Contains information about viewport location and orientation in space.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
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
        /// The projection matrix with no camera offset (no jittering). 
        /// For many temporal image effects, the camera that is currently rendering needs to be slightly offset from the default projection (that is, the camera is ‘jittered’). 
        /// If you use motion vectors and camera jittering together, use this property to keep the motion vectors stable between frames.
        /// </summary>
        public Matrix NonJitteredProjection;

        /// <summary>
        /// The inverted view matrix.
        /// </summary>
        public Matrix IV;

        /// <summary>
        /// The inverted projection matrix.
        /// </summary>
        public Matrix IP;

        /// <summary>
        /// The inverted projection view matrix.
        /// </summary>
        public Matrix IVP;

        /// <summary>
        /// The view frustum.
        /// </summary>
        public BoundingFrustum Frustum;

        /// <summary>
        /// The view frustum used for culling (can be different than Frustum in some cases e.g. cascaded shadow map rendering).
        /// </summary>
        public BoundingFrustum CullingFrustum;

        /// <summary>
        /// The draw passes mask for the current view rendering.
        /// </summary>
        public DrawPass Pass;

        /// <summary>
        /// Flag used by static, offline rendering passes (eg. reflections rendering, lightmap rendering etc.)
        /// </summary>
        public bool IsOfflinePass;

        /// <summary>
        /// The static flags mask used to hide objects that don't have a given static flags. Eg. use StaticFlags::Lightmap to render only objects that can use lightmap.
        /// </summary>
        public StaticFlags StaticFlagsMask;

        /// <summary>
        /// The view flags.
        /// </summary>
        public ViewFlags Flags;

        /// <summary>
        /// The view mode.
        /// </summary>
        public ViewMode Mode;

        /// <summary>
        /// Maximum allowed shadows quality for this view
        /// </summary>
        public Quality MaxShadowsQuality;

        /// <summary>
        /// The model LOD bias. Default is 0. Applied to all the objects in the render view.
        /// </summary>
        public int ModelLODBias;

        /// <summary>
        /// The model LOD distance scale factor. Default is 1. Applied to all the objects in the render view. Higher values increase LODs quality.
        /// </summary>
        public float ModelLODDistanceFactor;

        /// <summary>
        /// The model LOD bias. Default is 0. Applied to all the objects in the shadow maps render views. Can be used to improve shadows rendering performance or increase quality.
        /// </summary>
        public int ShadowModelLODBias;

        /// <summary>
        /// The model LOD distance scale factor. Default is 1. Applied to all the objects in the shadow maps render views. Higher values increase LODs quality. Can be used to improve shadows rendering performance or increase quality.
        /// </summary>
        public float ShadowModelLODDistanceFactor;

        /// <summary>
        /// The Temporal Anti-Aliasing jitter frame index.
        /// </summary>
        public int TaaFrameIndex;

        /// <summary>
        /// The view information vector with packed components to reconstruct linear depth and view position from the hardware depth buffer. Cached before rendering.
        /// </summary>
        public Vector4 ViewInfo;

        /// <summary>
        /// The screen size packed (x - width, y - height, zw - inv width, w - inv height). Cached before rendering.
        /// </summary>
        public Vector4 ScreenSize;

        /// <summary>
        /// The temporal AA jitter packed (xy - this frame jitter, zw - previous frame jitter). Cached before rendering. Zero if TAA is disabled. The value added to projection matrix (in clip space).
        /// </summary>
        public Vector4 TemporalAAJitter;

        /// <summary>
        /// The previous frame view matrix.
        /// </summary>
        public Matrix PrevView;

        /// <summary>
        /// The previous frame projection matrix.
        /// </summary>
        public Matrix PrevProjection;

        /// <summary>
        /// The previous frame view * projection matrix.
        /// </summary>
        public Matrix PrevViewProjection;

        /// <summary>
        /// Square of <see cref="ModelLODDistanceFactor"/>. Cached by rendering backend.
        /// </summary>
        public float ModelLODDistanceFactorSqrt;

        /// <summary>
        /// Initializes this view with default options.
        /// </summary>
        public void Init()
        {
            MaxShadowsQuality = Quality.Ultra;
            ModelLODDistanceFactor = 1.0f;
            ModelLODDistanceFactorSqrt = 1.0f;
            ShadowModelLODDistanceFactor = 1.0f;
            Flags = ViewFlags.DefaultGame;
            Mode = ViewMode.Default;
        }

        /// <summary>
        /// Updates the cached data for the view (inverse matrices, etc.).
        /// </summary>
        public void UpdateCachedData()
        {
            Matrix.Invert(ref View, out IV);
            Matrix.Invert(ref Projection, out IP);
            Matrix.Multiply(ref View, ref Projection, out var viewProjection);
            Frustum = new BoundingFrustum(viewProjection);
            Matrix.Invert(ref viewProjection, out IVP);
            CullingFrustum = Frustum;
        }

        /// <summary>
        /// Initializes render view data.
        /// </summary>
        /// <param name="view">The view.</param>
        /// <param name="projection">The projection.</param>
        public void SetUp(ref Matrix view, ref Matrix projection)
        {
            Position = view.TranslationVector;
            Projection = projection;
            NonJitteredProjection = projection;
            TemporalAAJitter = Vector4.Zero;
            View = view;

            UpdateCachedData();
        }

        /// <summary>
        /// Set up view for projector rendering.
        /// </summary>
        /// <param name="nearPlane">Near plane</param>
        /// <param name="farPlane">Far plane</param>
        /// <param name="position">Camera's position</param>
        /// <param name="direction">Camera's direction vector</param>
        /// <param name="up">Camera's up vector</param>
        /// <param name="angle">Camera's FOV angle (in degrees)</param>
        public void SetProjector(float nearPlane, float farPlane, Vector3 position, Vector3 direction, Vector3 up, float angle)
        {
            // Copy data
            Near = nearPlane;
            Far = farPlane;
            Position = position;

            // Create projection matrix
            Matrix.PerspectiveFov(angle * Mathf.DegreesToRadians, 1.0f, nearPlane, farPlane, out Projection);
            NonJitteredProjection = Projection;
            TemporalAAJitter = Vector4.Zero;

            // Create view matrix
            Direction = direction;
            Vector3 target = Position + Direction;
            Matrix.LookAt(ref Position, ref target, ref up, out View);

            UpdateCachedData();
        }

        /// <summary>
        /// Copies render view data from the camera.
        /// </summary>
        /// <param name="camera">The camera.</param>
        public void CopyFrom(Camera camera)
        {
            Position = camera.Position;
            Direction = camera.Direction;
            Near = camera.NearPlane;
            Far = camera.FarPlane;
            View = camera.View;
            Projection = camera.Projection;
            NonJitteredProjection = Projection;
            TemporalAAJitter = Vector4.Zero;

            UpdateCachedData();
        }

        /// <summary>
        /// Copies render view data from the camera.
        /// </summary>
        /// <param name="camera">The camera.</param>
        /// <param name="customViewport">The custom viewport to use for view/projeection matrices override.</param>
        public void CopyFrom(Camera camera, ref Viewport customViewport)
        {
            Position = camera.Position;
            Direction = camera.Direction;
            Near = camera.NearPlane;
            Far = camera.FarPlane;
            camera.GetMatrices(out View, out Projection, ref customViewport);
            NonJitteredProjection = Projection;
            TemporalAAJitter = Vector4.Zero;

            UpdateCachedData();
        }
    }
}
