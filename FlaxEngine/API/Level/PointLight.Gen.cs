// This code was auto-generated. Do not modify it.

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace FlaxEngine
{
    /// <summary>
    /// Point light emits light from point in all directions.
    /// </summary>
    [Tooltip("Point light emits light from point in all directions.")]
    public unsafe partial class PointLight : LightWithShadow
    {
        /// <inheritdoc />
        protected PointLight() : base()
        {
        }

        /// <summary>
        /// Creates new instance of <see cref="PointLight"/> object.
        /// </summary>
        /// <returns>The created object.</returns>
        public static PointLight New()
        {
            return Internal_Create(typeof(PointLight)) as PointLight;
        }

        /// <summary>
        /// Light source bulb radius
        /// </summary>
        [EditorOrder(2), DefaultValue(0.0f), EditorDisplay("Light"), Limit(0, 1000, 0.01f)]
        [Tooltip("Light source bulb radius")]
        public float SourceRadius
        {
            get { return Internal_GetSourceRadius(unmanagedPtr); }
            set { Internal_SetSourceRadius(unmanagedPtr, value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern float Internal_GetSourceRadius(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetSourceRadius(IntPtr obj, float value);

        /// <summary>
        /// Light source bulb length
        /// </summary>
        [EditorOrder(3), DefaultValue(0.0f), EditorDisplay("Light"), Limit(0, 1000, 0.01f)]
        [Tooltip("Light source bulb length")]
        public float SourceLength
        {
            get { return Internal_GetSourceLength(unmanagedPtr); }
            set { Internal_SetSourceLength(unmanagedPtr, value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern float Internal_GetSourceLength(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetSourceLength(IntPtr obj, float value);

        /// <summary>
        /// Controls the radial falloff of light when UseInverseSquaredFalloff is disabled.
        /// </summary>
        [EditorOrder(13), DefaultValue(8.0f), EditorDisplay("Light"), Limit(2, 16, 0.01f)]
        [Tooltip("Controls the radial falloff of light when UseInverseSquaredFalloff is disabled.")]
        public float FallOffExponent
        {
            get { return Internal_GetFallOffExponent(unmanagedPtr); }
            set { Internal_SetFallOffExponent(unmanagedPtr, value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern float Internal_GetFallOffExponent(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetFallOffExponent(IntPtr obj, float value);

        /// <summary>
        /// Whether to use physically based inverse squared distance falloff, where Radius is only clamping the light's contribution.
        /// </summary>
        [EditorOrder(14), DefaultValue(false), EditorDisplay("Light")]
        [Tooltip("Whether to use physically based inverse squared distance falloff, where Radius is only clamping the light's contribution.")]
        public bool UseInverseSquaredFalloff
        {
            get { return Internal_GetUseInverseSquaredFalloff(unmanagedPtr); }
            set { Internal_SetUseInverseSquaredFalloff(unmanagedPtr, value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_GetUseInverseSquaredFalloff(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetUseInverseSquaredFalloff(IntPtr obj, bool value);

        /// <summary>
        /// IES texture (light profiles from real world measured data)
        /// </summary>
        [EditorOrder(211), DefaultValue(null), EditorDisplay("IES Profile", "IES Texture")]
        [Tooltip("IES texture (light profiles from real world measured data)")]
        public IESProfile IESTexture
        {
            get { return Internal_GetIESTexture(unmanagedPtr); }
            set { Internal_SetIESTexture(unmanagedPtr, FlaxEngine.Object.GetUnmanagedPtr(value)); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern IESProfile Internal_GetIESTexture(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetIESTexture(IntPtr obj, IntPtr value);

        /// <summary>
        /// Enable/disable using light brightness from IES profile
        /// </summary>
        [EditorOrder(212), DefaultValue(false), EditorDisplay("IES Profile", "Use IES Brightness")]
        [Tooltip("Enable/disable using light brightness from IES profile")]
        public bool UseIESBrightness
        {
            get { return Internal_GetUseIESBrightness(unmanagedPtr); }
            set { Internal_SetUseIESBrightness(unmanagedPtr, value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_GetUseIESBrightness(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetUseIESBrightness(IntPtr obj, bool value);

        /// <summary>
        /// Global scale for IES brightness contribution
        /// </summary>
        [EditorOrder(213), DefaultValue(1.0f), Limit(0, 10000, 0.01f), EditorDisplay("IES Profile", "Brightness Scale")]
        [Tooltip("Global scale for IES brightness contribution")]
        public float IESBrightnessScale
        {
            get { return Internal_GetIESBrightnessScale(unmanagedPtr); }
            set { Internal_SetIESBrightnessScale(unmanagedPtr, value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern float Internal_GetIESBrightnessScale(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetIESBrightnessScale(IntPtr obj, float value);

        /// <summary>
        /// Gets or sets light radius
        /// </summary>
        [EditorOrder(1), DefaultValue(1000.0f), EditorDisplay("Light"), Limit(0, 100000, 0.1f)]
        [Tooltip("Gets light radius")]
        public float Radius
        {
            get { return Internal_GetRadius(unmanagedPtr); }
            set { Internal_SetRadius(unmanagedPtr, value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern float Internal_GetRadius(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetRadius(IntPtr obj, float value);
    }
}
