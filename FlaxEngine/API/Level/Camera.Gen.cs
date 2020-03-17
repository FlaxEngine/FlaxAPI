// This code was auto-generated. Do not modify it.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace FlaxEngine
{
    /// <summary>
    /// Describes the camera projection and view. Provides information about how to render scene (viewport location and direction, etc.).
    /// </summary>
    [Tooltip("Describes the camera projection and view. Provides information about how to render scene (viewport location and direction, etc.).")]
    public sealed unsafe partial class Camera : Actor
    {
        private Camera() : base()
        {
        }

        /// <summary>
        /// Creates new instance of <see cref="Camera"/> object.
        /// </summary>
        /// <returns>The created object.</returns>
        public static Camera New()
        {
            return Internal_Create(typeof(Camera)) as Camera;
        }

        /// <summary>
        /// The overriden main camera.
        /// </summary>
        [Tooltip("The overriden main camera.")]
        public static Camera OverrideMainCamera
        {
            get { return Internal_GetOverrideMainCamera(); }
            set { Internal_SetOverrideMainCamera(FlaxEngine.Object.GetUnmanagedPtr(value)); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern Camera Internal_GetOverrideMainCamera();

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetOverrideMainCamera(IntPtr value);

        /// <summary>
        /// Gets the main camera.
        /// </summary>
        [Tooltip("The main camera.")]
        public static Camera MainCamera
        {
            get { return Internal_GetMainCamera(); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern Camera Internal_GetMainCamera();

        /// <summary>
        /// Gets the view matrix.
        /// </summary>
        [Tooltip("The view matrix.")]
        public Matrix View
        {
            get { Internal_GetView(unmanagedPtr, out var resultAsRef); return resultAsRef; }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_GetView(IntPtr obj, out Matrix resultAsRef);

        /// <summary>
        /// Gets the projection matrix.
        /// </summary>
        [Tooltip("The projection matrix.")]
        public Matrix Projection
        {
            get { Internal_GetProjection(unmanagedPtr, out var resultAsRef); return resultAsRef; }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_GetProjection(IntPtr obj, out Matrix resultAsRef);

        /// <summary>
        /// Gets the frustum.
        /// </summary>
        [Tooltip("The frustum.")]
        public BoundingFrustum Frustum
        {
            get { Internal_GetFrustum(unmanagedPtr, out var resultAsRef); return resultAsRef; }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_GetFrustum(IntPtr obj, out BoundingFrustum resultAsRef);

        /// <summary>
        /// Gets or sets the value indicating if camera should use perspective rendering mode, otherwise it will use orthographic projection.
        /// </summary>
        [EditorOrder(20), DefaultValue(true), EditorDisplay("Camera"), Tooltip("Enables perspective projection mode, otherwise uses orthographic.")]
        public bool UsePerspective
        {
            get { return Internal_GetUsePerspective(unmanagedPtr); }
            set { Internal_SetUsePerspective(unmanagedPtr, value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_GetUsePerspective(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetUsePerspective(IntPtr obj, bool value);

        /// <summary>
        /// Gets or sets the camera's field of view (in degrees).
        /// </summary>
        [EditorOrder(10), DefaultValue(60.0f), Limit(0, 179), EditorDisplay("Camera", "Field Of View"), Tooltip("Field of view angle in degrees.")]
        public float FieldOfView
        {
            get { return Internal_GetFieldOfView(unmanagedPtr); }
            set { Internal_SetFieldOfView(unmanagedPtr, value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern float Internal_GetFieldOfView(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetFieldOfView(IntPtr obj, float value);

        /// <summary>
        /// Gets or sets the custom aspect ratio. 0 if not use custom value.
        /// </summary>
        [EditorOrder(50), DefaultValue(0.0f), Limit(0, 10, 0.01f), EditorDisplay("Camera"), Tooltip("Custom aspect ratio to use. Set to 0 to disable.")]
        public float CustomAspectRatio
        {
            get { return Internal_GetCustomAspectRatio(unmanagedPtr); }
            set { Internal_SetCustomAspectRatio(unmanagedPtr, value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern float Internal_GetCustomAspectRatio(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetCustomAspectRatio(IntPtr obj, float value);

        /// <summary>
        /// Gets or sets camera's near plane distance.
        /// </summary>
        [EditorOrder(30), DefaultValue(10.0f), Limit(0, 1000, 0.05f), EditorDisplay("Camera"), Tooltip("Near clipping plane distance")]
        public float NearPlane
        {
            get { return Internal_GetNearPlane(unmanagedPtr); }
            set { Internal_SetNearPlane(unmanagedPtr, value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern float Internal_GetNearPlane(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetNearPlane(IntPtr obj, float value);

        /// <summary>
        /// Gets or sets camera's far plane distance.
        /// </summary>
        [EditorOrder(40), DefaultValue(40000.0f), Limit(0, float.MaxValue, 5), EditorDisplay("Camera"), Tooltip("Far clipping plane distance")]
        public float FarPlane
        {
            get { return Internal_GetFarPlane(unmanagedPtr); }
            set { Internal_SetFarPlane(unmanagedPtr, value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern float Internal_GetFarPlane(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetFarPlane(IntPtr obj, float value);

        /// <summary>
        /// Gets or sets the orthographic projection scale.
        /// </summary>
        [EditorOrder(60), DefaultValue(1.0f), Limit(0.0001f, 1000, 0.01f), EditorDisplay("Camera"), Tooltip("Orthographic projection scale")]
        public float OrthographicScale
        {
            get { return Internal_GetOrthographicScale(unmanagedPtr); }
            set { Internal_SetOrthographicScale(unmanagedPtr, value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern float Internal_GetOrthographicScale(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetOrthographicScale(IntPtr obj, float value);

        /// <summary>
        /// Gets the camera viewport.
        /// </summary>
        [Tooltip("The camera viewport.")]
        public Viewport Viewport
        {
            get { Internal_GetViewport(unmanagedPtr, out var resultAsRef); return resultAsRef; }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_GetViewport(IntPtr obj, out Viewport resultAsRef);

        /// <summary>
        /// Projects the point from 3D world-space to the camera screen-space (in screen pixels for default viewport calculated from <see cref="Viewport"/>).
        /// </summary>
        /// <param name="worldSpaceLocation">The input world-space location (XYZ in world).</param>
        /// <param name="screenSpaceLocation">The output screen-space location (XY in screen pixels).</param>
        public void ProjectPoint(Vector3 worldSpaceLocation, out Vector2 screenSpaceLocation)
        {
            Internal_ProjectPoint(unmanagedPtr, ref worldSpaceLocation, out screenSpaceLocation);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_ProjectPoint(IntPtr obj, ref Vector3 worldSpaceLocation, out Vector2 screenSpaceLocation);

        /// <summary>
        /// Projects the point from 3D world-space to the camera screen-space (in screen pixels for given viewport).
        /// </summary>
        /// <param name="worldSpaceLocation">The input world-space location (XYZ in world).</param>
        /// <param name="screenSpaceLocation">The output screen-space location (XY in screen pixels).</param>
        /// <param name="viewport">The viewport.</param>
        public void ProjectPoint(Vector3 worldSpaceLocation, out Vector2 screenSpaceLocation, ref Viewport viewport)
        {
            Internal_ProjectPoint1(unmanagedPtr, ref worldSpaceLocation, out screenSpaceLocation, ref viewport);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_ProjectPoint1(IntPtr obj, ref Vector3 worldSpaceLocation, out Vector2 screenSpaceLocation, ref Viewport viewport);

        /// <summary>
        /// Converts the mouse position to 3D ray.
        /// </summary>
        /// <param name="mousePosition">The mouse position.</param>
        /// <returns>Mouse ray</returns>
        public Ray ConvertMouseToRay(Vector2 mousePosition)
        {
            Internal_ConvertMouseToRay(unmanagedPtr, ref mousePosition, out var resultAsRef); return resultAsRef;
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_ConvertMouseToRay(IntPtr obj, ref Vector2 mousePosition, out Ray resultAsRef);

        /// <summary>
        /// Converts the mouse position to 3D ray.
        /// </summary>
        /// <param name="mousePosition">The mouse position.</param>
        /// <param name="viewport">The viewport.</param>
        /// <returns>Mouse ray</returns>
        public Ray ConvertMouseToRay(Vector2 mousePosition, ref Viewport viewport)
        {
            Internal_ConvertMouseToRay1(unmanagedPtr, ref mousePosition, ref viewport, out var resultAsRef); return resultAsRef;
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_ConvertMouseToRay1(IntPtr obj, ref Vector2 mousePosition, ref Viewport viewport, out Ray resultAsRef);

        /// <summary>
        /// Calculates the view and the projection matrices for the camera.
        /// </summary>
        /// <param name="view">The result camera view matrix.</param>
        /// <param name="projection">The result camera projection matrix.</param>
        public void GetMatrices(out Matrix view, out Matrix projection)
        {
            Internal_GetMatrices(unmanagedPtr, out view, out projection);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_GetMatrices(IntPtr obj, out Matrix view, out Matrix projection);

        /// <summary>
        /// Calculates the view and the projection matrices for the camera. Support using custom viewport.
        /// </summary>
        /// <param name="view">The result camera view matrix.</param>
        /// <param name="projection">The result camera projection matrix.</param>
        /// <param name="viewport">The custom output viewport. Use null to skip it.</param>
        public void GetMatrices(out Matrix view, out Matrix projection, ref Viewport viewport)
        {
            Internal_GetMatrices1(unmanagedPtr, out view, out projection, ref viewport);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_GetMatrices1(IntPtr obj, out Matrix view, out Matrix projection, ref Viewport viewport);

        public bool IntersectsItselfEditor(ref Ray ray, out float distance)
        {
            return Internal_IntersectsItselfEditor(unmanagedPtr, ref ray, out distance);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_IntersectsItselfEditor(IntPtr obj, ref Ray ray, out float distance);
    }
}
