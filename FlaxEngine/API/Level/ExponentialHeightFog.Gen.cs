// This code was auto-generated. Do not modify it.

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace FlaxEngine
{
    /// <summary>
    /// Used to create fogging effects such as clouds but with a density that is related to the height of the fog.
    /// </summary>
    [Tooltip("Used to create fogging effects such as clouds but with a density that is related to the height of the fog.")]
    public unsafe partial class ExponentialHeightFog : Actor
    {
        /// <inheritdoc />
        protected ExponentialHeightFog() : base()
        {
        }

        /// <summary>
        /// Creates new instance of <see cref="ExponentialHeightFog"/> object.
        /// </summary>
        /// <returns>The created object.</returns>
        public static ExponentialHeightFog New()
        {
            return Internal_Create(typeof(ExponentialHeightFog)) as ExponentialHeightFog;
        }

        /// <summary>
        /// The fog density factor.
        /// </summary>
        [EditorOrder(10), DefaultValue(0.02f), Limit(0.000001f, 0.8f, 0.001f), EditorDisplay("Exponential Height Fog")]
        [Tooltip("The fog density factor.")]
        public float FogDensity
        {
            get { return Internal_GetFogDensity(unmanagedPtr); }
            set { Internal_SetFogDensity(unmanagedPtr, value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern float Internal_GetFogDensity(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetFogDensity(IntPtr obj, float value);

        /// <summary>
        /// The fog height density factor that controls how the density increases as height decreases. The smaller values produce more visible transition larger.
        /// </summary>
        [EditorOrder(20), DefaultValue(0.2f), Limit(0.001f, 2.0f, 0.001f), EditorDisplay("Exponential Height Fog")]
        [Tooltip("The fog height density factor that controls how the density increases as height decreases. The smaller values produce more visible transition larger.")]
        public float FogHeightFalloff
        {
            get { return Internal_GetFogHeightFalloff(unmanagedPtr); }
            set { Internal_SetFogHeightFalloff(unmanagedPtr, value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern float Internal_GetFogHeightFalloff(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetFogHeightFalloff(IntPtr obj, float value);

        /// <summary>
        /// Color of the fog.
        /// </summary>
        [EditorOrder(30), DefaultValue(typeof(Color), "0.448,0.634,1.0"), EditorDisplay("Exponential Height Fog")]
        [Tooltip("Color of the fog.")]
        public Color FogInscatteringColor
        {
            get { Internal_GetFogInscatteringColor(unmanagedPtr, out var resultAsRef); return resultAsRef; }
            set { Internal_SetFogInscatteringColor(unmanagedPtr, ref value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_GetFogInscatteringColor(IntPtr obj, out Color resultAsRef);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetFogInscatteringColor(IntPtr obj, ref Color value);

        /// <summary>
        /// Maximum opacity of the fog.
        /// A value of 1 means the fog can become fully opaque at a distance and replace scene color completely.
        /// A value of 0 means the fog color will not be factored in at all.
        /// </summary>
        [EditorOrder(40), DefaultValue(1.0f), Limit(0, 1, 0.001f), EditorDisplay("Exponential Height Fog")]
        public float FogMaxOpacity
        {
            get { return Internal_GetFogMaxOpacity(unmanagedPtr); }
            set { Internal_SetFogMaxOpacity(unmanagedPtr, value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern float Internal_GetFogMaxOpacity(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetFogMaxOpacity(IntPtr obj, float value);

        /// <summary>
        /// Distance from the camera that the fog will start, in world units.
        /// </summary>
        [EditorOrder(50), DefaultValue(0.0f), Limit(0), EditorDisplay("Exponential Height Fog")]
        [Tooltip("Distance from the camera that the fog will start, in world units.")]
        public float StartDistance
        {
            get { return Internal_GetStartDistance(unmanagedPtr); }
            set { Internal_SetStartDistance(unmanagedPtr, value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern float Internal_GetStartDistance(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetStartDistance(IntPtr obj, float value);

        /// <summary>
        /// Scene elements past this distance will not have fog applied. This is useful for excluding skyboxes which already have fog baked in.
        /// </summary>
        [EditorOrder(60), DefaultValue(0.0f), Limit(0), EditorDisplay("Exponential Height Fog")]
        [Tooltip("Scene elements past this distance will not have fog applied. This is useful for excluding skyboxes which already have fog baked in.")]
        public float FogCutoffDistance
        {
            get { return Internal_GetFogCutoffDistance(unmanagedPtr); }
            set { Internal_SetFogCutoffDistance(unmanagedPtr, value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern float Internal_GetFogCutoffDistance(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetFogCutoffDistance(IntPtr obj, float value);

        /// <summary>
        /// Directional light used for Directional Inscattering.
        /// </summary>
        [EditorOrder(200), DefaultValue(null), EditorDisplay("Directional Inscattering", "Light")]
        [Tooltip("Directional light used for Directional Inscattering.")]
        public DirectionalLight DirectionalInscatteringLight
        {
            get { return Internal_GetDirectionalInscatteringLight(unmanagedPtr); }
            set { Internal_SetDirectionalInscatteringLight(unmanagedPtr, FlaxEngine.Object.GetUnmanagedPtr(value)); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern DirectionalLight Internal_GetDirectionalInscatteringLight(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetDirectionalInscatteringLight(IntPtr obj, IntPtr value);

        /// <summary>
        /// Controls the size of the directional inscattering cone, which is used to approximate inscattering from a directional light.
        /// Note: there must be a directional light enabled for DirectionalInscattering to be used. Range: 2-64.
        /// </summary>
        [EditorOrder(210), DefaultValue(4.0f), Limit(2, 64, 0.1f), EditorDisplay("Directional Inscattering", "Exponent")]
        public float DirectionalInscatteringExponent
        {
            get { return Internal_GetDirectionalInscatteringExponent(unmanagedPtr); }
            set { Internal_SetDirectionalInscatteringExponent(unmanagedPtr, value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern float Internal_GetDirectionalInscatteringExponent(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetDirectionalInscatteringExponent(IntPtr obj, float value);

        /// <summary>
        /// Controls the start distance from the viewer of the directional inscattering, which is used to approximate inscattering from a directional light.
        /// Note: there must be a directional light enabled for DirectionalInscattering to be used.
        /// </summary>
        [EditorOrder(220), DefaultValue(10000.0f), Limit(0), EditorDisplay("Directional Inscattering", "Start Distance")]
        public float DirectionalInscatteringStartDistance
        {
            get { return Internal_GetDirectionalInscatteringStartDistance(unmanagedPtr); }
            set { Internal_SetDirectionalInscatteringStartDistance(unmanagedPtr, value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern float Internal_GetDirectionalInscatteringStartDistance(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetDirectionalInscatteringStartDistance(IntPtr obj, float value);

        /// <summary>
        /// Controls the color of the directional inscattering, which is used to approximate inscattering from a directional light.
        /// Note: there must be a directional light enabled for DirectionalInscattering to be used.
        /// </summary>
        [EditorOrder(230), DefaultValue(typeof(Color), "0.25,0.25,0.125"), EditorDisplay("Directional Inscattering", "Color")]
        public Color DirectionalInscatteringColor
        {
            get { Internal_GetDirectionalInscatteringColor(unmanagedPtr, out var resultAsRef); return resultAsRef; }
            set { Internal_SetDirectionalInscatteringColor(unmanagedPtr, ref value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_GetDirectionalInscatteringColor(IntPtr obj, out Color resultAsRef);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetDirectionalInscatteringColor(IntPtr obj, ref Color value);

        /// <summary>
        /// Whether to enable Volumetric fog. Graphics quality settings control the resolution of the fog simulation.
        /// </summary>
        [EditorOrder(300), DefaultValue(false), EditorDisplay("Volumetric Fog", "Enable")]
        [Tooltip("Whether to enable Volumetric fog. Graphics quality settings control the resolution of the fog simulation.")]
        public bool VolumetricFogEnable
        {
            get { return Internal_GetVolumetricFogEnable(unmanagedPtr); }
            set { Internal_SetVolumetricFogEnable(unmanagedPtr, value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_GetVolumetricFogEnable(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetVolumetricFogEnable(IntPtr obj, bool value);

        /// <summary>
        /// Controls the scattering phase function - how much incoming light scatters in various directions.
        /// A distribution value of 0 scatters equally in all directions, while 0.9 scatters predominantly in the light direction.
        /// In order to have visible volumetric fog light shafts from the side, the distribution will need to be closer to 0. Range: -0.9-0.9.
        /// </summary>
        [EditorOrder(310), DefaultValue(0.2f), Limit(-0.9f, 0.9f, 0.001f), EditorDisplay("Volumetric Fog", "Scattering Distribution")]
        public float VolumetricFogScatteringDistribution
        {
            get { return Internal_GetVolumetricFogScatteringDistribution(unmanagedPtr); }
            set { Internal_SetVolumetricFogScatteringDistribution(unmanagedPtr, value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern float Internal_GetVolumetricFogScatteringDistribution(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetVolumetricFogScatteringDistribution(IntPtr obj, float value);

        /// <summary>
        /// The height fog particle reflectiveness used by volumetric fog.
        /// Water particles in air have an albedo near white, while dust has slightly darker value.
        /// </summary>
        [EditorOrder(320), DefaultValue(typeof(Color), "1,1,1,1"), EditorDisplay("Volumetric Fog", "Albedo")]
        public Color VolumetricFogAlbedo
        {
            get { Internal_GetVolumetricFogAlbedo(unmanagedPtr, out var resultAsRef); return resultAsRef; }
            set { Internal_SetVolumetricFogAlbedo(unmanagedPtr, ref value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_GetVolumetricFogAlbedo(IntPtr obj, out Color resultAsRef);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetVolumetricFogAlbedo(IntPtr obj, ref Color value);

        /// <summary>
        /// Light emitted by height fog. This is a density so more light is emitted the further you are looking through the fog.
        /// In most cases using a Skylight is a better choice, however, it may be useful in certain scenarios.
        /// </summary>
        [EditorOrder(330), DefaultValue(typeof(Color), "0,0,0,1"), EditorDisplay("Volumetric Fog", "Emissive")]
        public Color VolumetricFogEmissive
        {
            get { Internal_GetVolumetricFogEmissive(unmanagedPtr, out var resultAsRef); return resultAsRef; }
            set { Internal_SetVolumetricFogEmissive(unmanagedPtr, ref value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_GetVolumetricFogEmissive(IntPtr obj, out Color resultAsRef);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetVolumetricFogEmissive(IntPtr obj, ref Color value);

        /// <summary>
        /// Scales the height fog particle extinction amount used by volumetric fog.
        /// Values larger than 1 cause fog particles everywhere absorb more light. Range: 0.1-10.
        /// </summary>
        [EditorOrder(340), DefaultValue(1.0f), Limit(0.1f, 10, 0.1f), EditorDisplay("Volumetric Fog", "Extinction Scale")]
        public float VolumetricFogExtinctionScale
        {
            get { return Internal_GetVolumetricFogExtinctionScale(unmanagedPtr); }
            set { Internal_SetVolumetricFogExtinctionScale(unmanagedPtr, value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern float Internal_GetVolumetricFogExtinctionScale(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetVolumetricFogExtinctionScale(IntPtr obj, float value);

        /// <summary>
        /// Distance over which volumetric fog should be computed. Larger values extend the effect into the distance but expose under-sampling artifacts in details.
        /// </summary>
        [EditorOrder(350), DefaultValue(6000.0f), Limit(0), EditorDisplay("Volumetric Fog", "Distance")]
        [Tooltip("Distance over which volumetric fog should be computed. Larger values extend the effect into the distance but expose under-sampling artifacts in details.")]
        public float VolumetricFogDistance
        {
            get { return Internal_GetVolumetricFogDistance(unmanagedPtr); }
            set { Internal_SetVolumetricFogDistance(unmanagedPtr, value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern float Internal_GetVolumetricFogDistance(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetVolumetricFogDistance(IntPtr obj, float value);
    }
}
