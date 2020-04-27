// This code was auto-generated. Do not modify it.

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace FlaxEngine
{
    /// <summary>
    /// Environment Probe can capture space around the objects to provide reflections.
    /// </summary>
    [Tooltip("Environment Probe can capture space around the objects to provide reflections.")]
    public unsafe partial class EnvironmentProbe : Actor
    {
        /// <inheritdoc />
        protected EnvironmentProbe() : base()
        {
        }

        /// <summary>
        /// Creates new instance of <see cref="EnvironmentProbe"/> object.
        /// </summary>
        /// <returns>The created object.</returns>
        public static EnvironmentProbe New()
        {
            return Internal_Create(typeof(EnvironmentProbe)) as EnvironmentProbe;
        }

        /// <summary>
        /// The reflections brightness.
        /// </summary>
        [EditorOrder(10), DefaultValue(1.0f), Limit(0, 1000, 0.01f), EditorDisplay("Probe")]
        [Tooltip("The reflections brightness.")]
        public float Brightness
        {
            get { return Internal_GetBrightness(unmanagedPtr); }
            set { Internal_SetBrightness(unmanagedPtr, value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern float Internal_GetBrightness(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetBrightness(IntPtr obj, float value);

        /// <summary>
        /// Value indicating if probe should be updated automatically on change.
        /// </summary>
        [EditorOrder(30), DefaultValue(false), EditorDisplay("Probe")]
        [Tooltip("Value indicating if probe should be updated automatically on change.")]
        public bool AutoUpdate
        {
            get { return Internal_GetAutoUpdate(unmanagedPtr); }
            set { Internal_SetAutoUpdate(unmanagedPtr, value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_GetAutoUpdate(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetAutoUpdate(IntPtr obj, bool value);

        /// <summary>
        /// The probe capture camera near plane distance.
        /// </summary>
        [EditorOrder(30), DefaultValue(10.0f), Limit(0, float.MaxValue, 0.01f), EditorDisplay("Probe")]
        [Tooltip("The probe capture camera near plane distance.")]
        public float CaptureNearPlane
        {
            get { return Internal_GetCaptureNearPlane(unmanagedPtr); }
            set { Internal_SetCaptureNearPlane(unmanagedPtr, value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern float Internal_GetCaptureNearPlane(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetCaptureNearPlane(IntPtr obj, float value);

        /// <summary>
        /// Gets or sets the probe radius.
        /// </summary>
        [EditorOrder(20), DefaultValue(3000.0f), Limit(0), EditorDisplay("Probe")]
        [Tooltip("The probe radius.")]
        public float Radius
        {
            get { return Internal_GetRadius(unmanagedPtr); }
            set { Internal_SetRadius(unmanagedPtr, value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern float Internal_GetRadius(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetRadius(IntPtr obj, float value);

        /// <summary>
        /// Gets probe scaled radius.
        /// </summary>
        [Tooltip("Gets probe scaled radius.")]
        public float ScaledRadius
        {
            get { return Internal_GetScaledRadius(unmanagedPtr); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern float Internal_GetScaledRadius(IntPtr obj);

        /// <summary>
        /// Returns true if env probe has cube texture assigned.
        /// </summary>
        [Tooltip("Returns true if env probe has cube texture assigned.")]
        public bool HasProbe
        {
            get { return Internal_HasProbe(unmanagedPtr); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_HasProbe(IntPtr obj);

        /// <summary>
        /// Returns true if env probe has cube texture assigned.
        /// </summary>
        [Tooltip("Returns true if env probe has cube texture assigned.")]
        public bool HasProbeLoaded
        {
            get { return Internal_HasProbeLoaded(unmanagedPtr); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_HasProbeLoaded(IntPtr obj);

        /// <summary>
        /// Gets the probe texture used during rendering (baked or custom one).
        /// </summary>
        [Tooltip("The probe texture used during rendering (baked or custom one).")]
        public CubeTexture Probe
        {
            get { return Internal_GetProbe(unmanagedPtr); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern CubeTexture Internal_GetProbe(IntPtr obj);

        /// <summary>
        /// True if probe is using custom cube texture (not baked).
        /// </summary>
        [Tooltip("True if probe is using custom cube texture (not baked).")]
        public bool IsUsingCustomProbe
        {
            get { return Internal_IsUsingCustomProbe(unmanagedPtr); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_IsUsingCustomProbe(IntPtr obj);

        /// <summary>
        /// Gets or sets the custom probe (null if using baked one or none).
        /// </summary>
        [EditorOrder(40), DefaultValue(null), EditorDisplay("Probe")]
        [Tooltip("The custom probe (null if using baked one or none).")]
        public CubeTexture CustomProbe
        {
            get { return Internal_GetCustomProbe(unmanagedPtr); }
            set { Internal_SetCustomProbe(unmanagedPtr, FlaxEngine.Object.GetUnmanagedPtr(value)); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern CubeTexture Internal_GetCustomProbe(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetCustomProbe(IntPtr obj, IntPtr probe);

        /// <summary>
        /// Bakes that probe. It won't be performed now but on async graphics rendering task.
        /// </summary>
        /// <param name="timeout">The timeout in seconds left to bake it (aka startup time).</param>
        public void Bake(float timeout = 0)
        {
            Internal_Bake(unmanagedPtr, timeout);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_Bake(IntPtr obj, float timeout);
    }
}
