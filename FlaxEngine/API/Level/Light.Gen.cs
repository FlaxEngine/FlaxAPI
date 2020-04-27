// This code was auto-generated. Do not modify it.

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace FlaxEngine
{
    /// <summary>
    /// Base class for all light types.
    /// </summary>
    [Tooltip("Base class for all light types.")]
    public abstract unsafe partial class Light : Actor
    {
        /// <inheritdoc />
        protected Light() : base()
        {
        }

        /// <summary>
        /// Color of the light
        /// </summary>
        [EditorOrder(20), EditorDisplay("Light")]
        [Tooltip("Color of the light")]
        public Color Color
        {
            get { Internal_GetColor(unmanagedPtr, out var resultAsRef); return resultAsRef; }
            set { Internal_SetColor(unmanagedPtr, ref value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_GetColor(IntPtr obj, out Color resultAsRef);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetColor(IntPtr obj, ref Color value);

        /// <summary>
        /// Brightness of the light
        /// </summary>
        [EditorOrder(30), EditorDisplay("Light"), Limit(0.0f, 100000000.0f, 0.1f)]
        [Tooltip("Brightness of the light")]
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
        /// Controls light visibility range. The distance at which the light be completely faded. Use value 0 to always draw light.
        /// </summary>
        [EditorOrder(35), Limit(0, float.MaxValue, 10.0f), EditorDisplay("Light")]
        [Tooltip("Controls light visibility range. The distance at which the light be completely faded. Use value 0 to always draw light.")]
        public float ViewDistance
        {
            get { return Internal_GetViewDistance(unmanagedPtr); }
            set { Internal_SetViewDistance(unmanagedPtr, value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern float Internal_GetViewDistance(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetViewDistance(IntPtr obj, float value);

        /// <summary>
        /// Controls how much this light will contribute indirect lighting. When set to 0, there is no GI from the light. The default value is 1.
        /// </summary>
        [EditorOrder(40), DefaultValue(1.0f), Limit(0, 100, 0.1f), EditorDisplay("Light", "Indirect Lighting Intensity")]
        [Tooltip("Controls how much this light will contribute indirect lighting. When set to 0, there is no GI from the light. The default value is 1.")]
        public float IndirectLightingIntensity
        {
            get { return Internal_GetIndirectLightingIntensity(unmanagedPtr); }
            set { Internal_SetIndirectLightingIntensity(unmanagedPtr, value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern float Internal_GetIndirectLightingIntensity(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetIndirectLightingIntensity(IntPtr obj, float value);

        /// <summary>
        /// Controls how much this light will contribute to the Volumetric Fog. When set to 0, there is no contribution.
        /// </summary>
        [EditorOrder(110),DefaultValue(1.0f),  Limit(0, 100, 0.01f), EditorDisplay("Volumetric Fog", "Scattering Intensity")]
        [Tooltip("Controls how much this light will contribute to the Volumetric Fog. When set to 0, there is no contribution.")]
        public float VolumetricScatteringIntensity
        {
            get { return Internal_GetVolumetricScatteringIntensity(unmanagedPtr); }
            set { Internal_SetVolumetricScatteringIntensity(unmanagedPtr, value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern float Internal_GetVolumetricScatteringIntensity(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetVolumetricScatteringIntensity(IntPtr obj, float value);

        /// <summary>
        /// Toggles whether or not to cast a volumetric shadow for lights contributing to Volumetric Fog. Also shadows casting by this light should be enabled in order to make it cast volumetric fog shadow.
        /// </summary>
        [EditorOrder(120), EditorDisplay("Volumetric Fog", "Cast Shadow")]
        [Tooltip("Toggles whether or not to cast a volumetric shadow for lights contributing to Volumetric Fog. Also shadows casting by this light should be enabled in order to make it cast volumetric fog shadow.")]
        public bool CastVolumetricShadow
        {
            get { return Internal_GetCastVolumetricShadow(unmanagedPtr); }
            set { Internal_SetCastVolumetricShadow(unmanagedPtr, value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_GetCastVolumetricShadow(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetCastVolumetricShadow(IntPtr obj, bool value);
    }
}

namespace FlaxEngine
{
    /// <summary>
    /// Base class for all light types that can cast dynamic or static shadow. Contains more shared properties for point/spot/directional lights.
    /// </summary>
    [Tooltip("Base class for all light types that can cast dynamic or static shadow. Contains more shared properties for point/spot/directional lights.")]
    public abstract unsafe partial class LightWithShadow : Light
    {
        /// <inheritdoc />
        protected LightWithShadow() : base()
        {
        }

        /// <summary>
        /// The minimum roughness value used to clamp material surface roughness during shading pixel.
        /// </summary>
        [EditorOrder(40), EditorDisplay("Light", "Minimum Roughness"), Limit(0.0f, 1.0f, 0.01f)]
        [Tooltip("The minimum roughness value used to clamp material surface roughness during shading pixel.")]
        public float MinRoughness
        {
            get { return Internal_GetMinRoughness(unmanagedPtr); }
            set { Internal_SetMinRoughness(unmanagedPtr, value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern float Internal_GetMinRoughness(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetMinRoughness(IntPtr obj, float value);

        /// <summary>
        /// The light shadows casting distance from view.
        /// </summary>
        [EditorOrder(80), EditorDisplay("Shadow", "Distance"), Limit(0, 1000000)]
        [Tooltip("The light shadows casting distance from view.")]
        public float ShadowsDistance
        {
            get { return Internal_GetShadowsDistance(unmanagedPtr); }
            set { Internal_SetShadowsDistance(unmanagedPtr, value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern float Internal_GetShadowsDistance(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetShadowsDistance(IntPtr obj, float value);

        /// <summary>
        /// The light shadows fade off distance
        /// </summary>
        [EditorOrder(90), EditorDisplay("Shadow", "Fade Distance"), Limit(0.0f, 10000.0f, 0.1f)]
        [Tooltip("The light shadows fade off distance")]
        public float ShadowsFadeDistance
        {
            get { return Internal_GetShadowsFadeDistance(unmanagedPtr); }
            set { Internal_SetShadowsFadeDistance(unmanagedPtr, value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern float Internal_GetShadowsFadeDistance(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetShadowsFadeDistance(IntPtr obj, float value);

        /// <summary>
        /// The light shadows edges sharpness
        /// </summary>
        [EditorOrder(70), EditorDisplay("Shadow", "Sharpness"), Limit(1.0f, 10.0f, 0.001f)]
        [Tooltip("The light shadows edges sharpness")]
        public float ShadowsSharpness
        {
            get { return Internal_GetShadowsSharpness(unmanagedPtr); }
            set { Internal_SetShadowsSharpness(unmanagedPtr, value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern float Internal_GetShadowsSharpness(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetShadowsSharpness(IntPtr obj, float value);

        /// <summary>
        /// Dynamic shadows blending strength. Default is 1 for fully opaque shadows, value 0 disables shadows.
        /// </summary>
        [EditorOrder(75), EditorDisplay("Shadow", "Strength"), Limit(0.0f, 1.0f, 0.001f)]
        [Tooltip("Dynamic shadows blending strength. Default is 1 for fully opaque shadows, value 0 disables shadows.")]
        public float ShadowsStrength
        {
            get { return Internal_GetShadowsStrength(unmanagedPtr); }
            set { Internal_SetShadowsStrength(unmanagedPtr, value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern float Internal_GetShadowsStrength(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetShadowsStrength(IntPtr obj, float value);

        /// <summary>
        /// The depth bias used for shadow map comparison.
        /// </summary>
        [EditorOrder(95), EditorDisplay("Shadow", "Depth Bias"), Limit(0.0f, 10.0f, 0.0001f)]
        [Tooltip("The depth bias used for shadow map comparison.")]
        public float ShadowsDepthBias
        {
            get { return Internal_GetShadowsDepthBias(unmanagedPtr); }
            set { Internal_SetShadowsDepthBias(unmanagedPtr, value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern float Internal_GetShadowsDepthBias(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetShadowsDepthBias(IntPtr obj, float value);

        /// <summary>
        /// A factor specifying the offset to add to the calculated shadow map depth with respect to the surface normal.
        /// </summary>
        [EditorOrder(96), EditorDisplay("Shadow", "Normal Offset Scale"), Limit(0.0f, 100.0f, 0.1f)]
        [Tooltip("A factor specifying the offset to add to the calculated shadow map depth with respect to the surface normal.")]
        public float ShadowsNormalOffsetScale
        {
            get { return Internal_GetShadowsNormalOffsetScale(unmanagedPtr); }
            set { Internal_SetShadowsNormalOffsetScale(unmanagedPtr, value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern float Internal_GetShadowsNormalOffsetScale(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetShadowsNormalOffsetScale(IntPtr obj, float value);

        /// <summary>
        /// Shadows casting mode by this visual element
        /// </summary>
        [EditorOrder(60), EditorDisplay("Shadow", "Mode")]
        [Tooltip("Shadows casting mode by this visual element")]
        public ShadowsCastingMode ShadowsMode
        {
            get { return Internal_GetShadowsMode(unmanagedPtr); }
            set { Internal_SetShadowsMode(unmanagedPtr, value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern ShadowsCastingMode Internal_GetShadowsMode(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetShadowsMode(IntPtr obj, ShadowsCastingMode value);
    }
}
