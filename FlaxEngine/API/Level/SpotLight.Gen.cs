// This code was auto-generated. Do not modify it.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace FlaxEngine
{
    /// <summary>
    /// Spot light emits light from the point in a given direction.
    /// </summary>
    [Tooltip("Spot light emits light from the point in a given direction.")]
    public unsafe partial class SpotLight : LightWithShadow
    {
        /// <inheritdoc />
        protected SpotLight() : base()
        {
        }

        /// <summary>
        /// Creates new instance of <see cref="SpotLight"/> object.
        /// </summary>
        /// <returns>The created object.</returns>
        public static SpotLight New()
        {
            return Internal_Create(typeof(SpotLight)) as SpotLight;
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
        /// Gets light radius
        /// </summary>
        [EditorOrder(1), DefaultValue(1000.0f), EditorDisplay("Light"), Tooltip("Light radius"), Limit(0, 10000, 0.1f)]
        public float Radius
        {
            get { return Internal_GetRadius(unmanagedPtr); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern float Internal_GetRadius(IntPtr obj);

        /// <summary>
        /// Gets the spot light's outer cone angle (in degrees)
        /// </summary>
        [EditorOrder(22), DefaultValue(43.0f), EditorDisplay("Light"), Limit(1, 80, 0.1f)]
        [Tooltip("The spot light's outer cone angle (in degrees)")]
        public float OuterConeAngle
        {
            get { return Internal_GetOuterConeAngle(unmanagedPtr); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern float Internal_GetOuterConeAngle(IntPtr obj);

        /// <summary>
        /// Sets the spot light's inner cone angle (in degrees)
        /// </summary>
        [EditorOrder(21), DefaultValue(10.0f), EditorDisplay("Light"), Limit(1, 80, 0.1f)]
        [Tooltip("Sets the spot light's inner cone angle (in degrees)")]
        public float InnerConeAngle
        {
            get { return Internal_GetInnerConeAngle(unmanagedPtr); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern float Internal_GetInnerConeAngle(IntPtr obj);
    }
}
