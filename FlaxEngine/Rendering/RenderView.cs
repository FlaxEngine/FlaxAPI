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
        /// The near plane.
        /// </summary>
        public float Near;

        /// <summary>
        /// The direction of the view.
        /// </summary>
        public Vector3 Direction;

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
        /// The temporal AA jitter packed (xy - this frame jitter, zw - previous frame jitter). Cached before rendering. Zero if TAA is disabled. The value added to projection matrix (in clip space).
        /// </summary>
        public Vector4 TemporalAAJitter;

        /// <summary>
        /// Flag used by static, offline rendering passes (eg. reflections rendering, lightmap rendering etc.)
        /// </summary>
        public bool IsOfflinePass;

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
        /// The view flags.
        /// </summary>
        public ViewFlags Flags;

        /// <summary>
        /// The view mode.
        /// </summary>
        public ViewMode Mode;

        /// <summary>
        /// Initializes this view with default options.
        /// </summary>
        public void Init()
        {
            MaxShadowsQuality = Quality.Ultra;
            ModelLODDistanceFactor = 1.0f;
            ShadowModelLODDistanceFactor = 1.0f;
            Flags = ViewFlags.DefaultGame;
            Mode = ViewMode.Default;
        }

        /// <summary>
        /// Initializes render view data.
        /// </summary>
        /// <param name="view">The view.</param>
        /// <param name="projection">The projection.</param>
        public void SetUp(ref Matrix view, ref Matrix projection)
        {
            // Copy data
            Position = view.TranslationVector;
            Projection = projection;
            NonJitteredProjection = projection;
            TemporalAAJitter = Vector4.Zero;
            View = view;
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
        }

        /// <summary>
        /// Copies render view data from the camera.
        /// </summary>
        /// <param name="camera">The camera.</param>
        public void CopyFrom(Camera camera)
        {
            // Get data
            Position = camera.Position;
            Direction = camera.Direction;
            Near = camera.NearPlane;
            Far = camera.FarPlane;
            View = camera.View;
            Projection = camera.Projection;
            NonJitteredProjection = Projection;
            TemporalAAJitter = Vector4.Zero;
        }

        /// <summary>
        /// Copies render view data from the camera.
        /// </summary>
        /// <param name="camera">The camera.</param>
        /// <param name="customViewport">The custom viewport to use for view/projeection matrices override.</param>
        public void CopyFrom(Camera camera, ref Viewport customViewport)
        {
            // Get data
            Position = camera.Position;
            Direction = camera.Direction;
            Near = camera.NearPlane;
            Far = camera.FarPlane;
            camera.GetMatrices(out View, out Projection, ref customViewport);
            NonJitteredProjection = Projection;
            TemporalAAJitter = Vector4.Zero;
        }
    }
}
