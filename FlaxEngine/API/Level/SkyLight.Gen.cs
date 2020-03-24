// This code was auto-generated. Do not modify it.

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace FlaxEngine
{
    /// <summary>
    /// Sky light captures the distant parts of the scene and applies it as a light. Allows to add ambient light.
    /// </summary>
    [Tooltip("Sky light captures the distant parts of the scene and applies it as a light. Allows to add ambient light.")]
    public unsafe partial class SkyLight : Light
    {
        /// <inheritdoc />
        protected SkyLight() : base()
        {
        }

        /// <summary>
        /// Creates new instance of <see cref="SkyLight"/> object.
        /// </summary>
        /// <returns>The created object.</returns>
        public static SkyLight New()
        {
            return Internal_Create(typeof(SkyLight)) as SkyLight;
        }

        /// <summary>
        /// Additional color to add. Source texture colors are summed with it. Can be used to apply custom ambient color.
        /// </summary>
        [EditorOrder(21), DefaultValue(typeof(Color), "0,0,0,1"), EditorDisplay("Light")]
        [Tooltip("Additional color to add. Source texture colors are summed with it. Can be used to apply custom ambient color.")]
        public Color AdditiveColor
        {
            get { Internal_GetAdditiveColor(unmanagedPtr, out var resultAsRef); return resultAsRef; }
            set { Internal_SetAdditiveColor(unmanagedPtr, ref value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_GetAdditiveColor(IntPtr obj, out Color resultAsRef);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetAdditiveColor(IntPtr obj, ref Color value);

        /// <summary>
        /// Distance from the light at which any geometry should be treated as part of the sky.
        /// </summary>
        [EditorOrder(45), DefaultValue(150000.0f), Limit(0), EditorDisplay("Probe")]
        [Tooltip("Distance from the light at which any geometry should be treated as part of the sky.")]
        public float SkyDistanceThreshold
        {
            get { return Internal_GetSkyDistanceThreshold(unmanagedPtr); }
            set { Internal_SetSkyDistanceThreshold(unmanagedPtr, value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern float Internal_GetSkyDistanceThreshold(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetSkyDistanceThreshold(IntPtr obj, float value);

        /// <summary>
        /// The current light source mode.
        /// </summary>
        [EditorOrder(40), DefaultValue(Modes.CustomTexture), EditorDisplay("Probe")]
        [Tooltip("The current light source mode.")]
        public Modes Mode
        {
            get { return Internal_GetMode(unmanagedPtr); }
            set { Internal_SetMode(unmanagedPtr, value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern Modes Internal_GetMode(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetMode(IntPtr obj, Modes value);

        /// <summary>
        /// The custom texture.
        /// </summary>
        [EditorOrder(50), DefaultValue(null), EditorDisplay("Probe")]
        [Tooltip("The custom texture.")]
        public CubeTexture CustomTexture
        {
            get { return Internal_GetCustomTexture(unmanagedPtr); }
            set { Internal_SetCustomTexture(unmanagedPtr, FlaxEngine.Object.GetUnmanagedPtr(value)); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern CubeTexture Internal_GetCustomTexture(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetCustomTexture(IntPtr obj, IntPtr value);

        /// <summary>
        /// Gets the radius.
        /// </summary>
        [EditorOrder(29), DefaultValue(1000000.0f), Limit(0), EditorDisplay("Light")]
        [Tooltip("The radius.")]
        public float Radius
        {
            get { return Internal_GetRadius(unmanagedPtr); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern float Internal_GetRadius(IntPtr obj);

        /// <summary>
        /// Bakes that probe.
        /// </summary>
        /// <param name="timeout">The timeout in seconds left to bake it (aka startup time).</param>
        public void Bake(float timeout = 0)
        {
            Internal_Bake(unmanagedPtr, timeout);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_Bake(IntPtr obj, float timeout);

        /// <summary>
        /// Sky light source mode.
        /// </summary>
        [Tooltip("Sky light source mode.")]
        public enum Modes
        {
            /// <summary>
            /// The captured scene will be used as a light source.
            /// </summary>
            [Tooltip("The captured scene will be used as a light source.")]
            CaptureScene = 0,

            /// <summary>
            /// The custom cube texture will be used as a light source.
            /// </summary>
            [Tooltip("The custom cube texture will be used as a light source.")]
            CustomTexture = 1,
        }
    }
}
