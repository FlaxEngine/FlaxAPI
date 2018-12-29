// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;

// ReSharper disable UnusedMember.Global
// ReSharper disable InconsistentNaming
// ReSharper disable NonReadonlyMemberInGetHashCode

namespace FlaxEngine.Rendering
{
    /// <summary>
    /// Eye adaptation technique.
    /// </summary>
    public enum EyeAdaptationTechnique
    {
        /// <summary>
        /// The none.
        /// </summary>
        None = 0,

        /// <summary>
        /// The manual.
        /// </summary>
        Manual = 1,

        /// <summary>
        /// The automatic.
        /// </summary>
        Auto = 2
    }

    /// <summary>
    /// Depth of field bokeh shape types.
    /// </summary>
    public enum BokehShapeType
    {
        /// <summary>
        /// The hexagon shape.
        /// </summary>
        Hexagon = 0,

        /// <summary>
        /// The octagon shape.
        /// </summary>
        Octagon = 1,

        /// <summary>
        /// The circle shape.
        /// </summary>
        Circle = 2,

        /// <summary>
        /// The cross shape.
        /// </summary>
        Cross = 3,

        /// <summary>
        /// The custom texture shape.
        /// </summary>
        Custom = 4
    }

    /// <summary>
    /// The effect pass resolution.
    /// </summary>
    public enum ResolutionMode
    {
        /// <summary>
        /// Full resolution
        /// </summary>
        Full = 1,

        /// <summary>
        /// Half resolution
        /// </summary>
        Half = 2,
    }

    /// <summary>
    /// Contains settings for rendering advanced visual effects and post effects.
    /// </summary>
    [Serializable]
    public struct PostProcessSettings : IEquatable<PostProcessSettings>
    {
        /// <summary>
        /// Packed settings storage container used with C++ interop.
        /// </summary>
        [Serializable, StructLayout(LayoutKind.Sequential)]
        internal struct Data
        {
            // Flags

            // Every property has order eg. 601, 812. We use dozens number as group index and rest as a property index.
            // Bit fields contain flags if override every parameter.
            // Helper functions GetFlag/SetFlags are used to modify override flag per single setting.

            public int Flags0; //
            public int Flags1; // AO
            public int Flags2; // Bloom
            public int Flags3; // ToneMap
            public int Flags4; // Eye
            public int Flags5; // Cam
            public int Flags6; // Flare
            public int Flags7; // DOF
            public int Flags8; // SSR
            public int Flags9; // ColorGrading
            public int Flags10; // MB

            // Ambient Occlusion

            public byte AO_Enabled;
            public float AO_Intensity;
            public float AO_Power;
            public float AO_Radius;
            public float AO_FadeOutDistance;

            // Screen Space Reflections

            public float SSR_Intensity;
            public ResolutionMode SSR_DepthResolution;
            public ResolutionMode SSR_RayTracePassResolution;
            public float SSR_BRDFBias;
            public float SSR_RoughnessThreshold;
            public float SSR_WorldAntiSelfOcclusionBias;
            public ResolutionMode SSR_ResolvePassResolution;
            public int SSR_ResolveSamples;
            public byte SSR_UseColorBufferMips;
            public float SSR_EdgeFadeFactor;
            public byte SSR_TemporalEffect;
            public float SSR_TemporalScale;
            public float SSR_TemporalResponse;

            // Bloom

            public byte Bloom_Enabled;
            public float Bloom_Intensity;
            public float Bloom_Threshold;
            public float Bloom_BlurSigma;
            public float Bloom_Scale;

            // Tone Mapping

            public float ToneMap_WhiteTemp;
            public float ToneMap_WhiteTint;
            public float ToneMap_FilmSlope;
            public float ToneMap_FilmToe;
            public float ToneMap_FilmShoulder;
            public float ToneMap_FilmBlackClip;
            public float ToneMap_FilmWhiteClip;

            // Color Grading

            public Vector4 ColorGrading_ColorSaturation;
            public Vector4 ColorGrading_ColorContrast;
            public Vector4 ColorGrading_ColorGamma;
            public Vector4 ColorGrading_ColorGain;
            public Vector4 ColorGrading_ColorOffset;
            public Vector4 ColorGrading_ColorSaturationShadows;
            public Vector4 ColorGrading_ColorContrastShadows;
            public Vector4 ColorGrading_ColorGammaShadows;
            public Vector4 ColorGrading_ColorGainShadows;
            public Vector4 ColorGrading_ColorOffsetShadows;
            public Vector4 ColorGrading_ColorSaturationMidtones;
            public Vector4 ColorGrading_ColorContrastMidtones;
            public Vector4 ColorGrading_ColorGammaMidtones;
            public Vector4 ColorGrading_ColorGainMidtones;
            public Vector4 ColorGrading_ColorOffsetMidtones;
            public Vector4 ColorGrading_ColorSaturationHighlights;
            public Vector4 ColorGrading_ColorContrastHighlights;
            public Vector4 ColorGrading_ColorGammaHighlights;
            public Vector4 ColorGrading_ColorGainHighlights;
            public Vector4 ColorGrading_ColorOffsetHighlights;
            public float ColorGrading_ShadowsMax;
            public float ColorGrading_HighlightsMin;

            // Eye Adaptation

            public EyeAdaptationTechnique Eye_Technique;
            public float Eye_SpeedUp;
            public float Eye_SpeedDown;
            public float Eye_Exposure;
            public float Eye_KeyValue;
            public float Eye_MinLuminance;
            public float Eye_MaxLuminance;

            // Camera Artifacts

            public float Cam_VignetteIntensity;
            public Vector3 Cam_VignetteColor;
            public float Cam_VignetteShapeFactor;
            public float Cam_GrainAmount;
            public float Cam_GrainParticleSize;
            public float Cam_GrainSpeed;
            public float Cam_ChromaticDistortion;

            // Lens Flares

            public float Flare_Intensity;
            public int Flare_Ghosts;
            public float Flare_HaloWidth;
            public float Flare_HaloIntensity;
            public float Flare_GhostDispersal;
            public float Flare_Distortion;
            public float Flare_ThresholdBias;
            public float Flare_ThresholdScale;
            public Guid Flare_LensColor;
            public Guid Flare_LensStar;
            public Guid Flare_LensDirt;
            public float Flare_LensDirtIntensity;

            // Depth Of Field

            public byte DOF_Enabled;
            public float DOF_BlurStrength;
            public float DOF_FocalDistance;
            public float DOF_FocalRegion;
            public float DOF_NearTransitionRange;
            public float DOF_FarTransitionRange;
            public float DOF_DepthLimit;
            public byte DOF_BokehEnabled;
            public float DOF_BokehSize;
            public BokehShapeType DOF_BokehShape;
            public Guid DOF_BokehShapeCustom;
            public float DOF_BokehBrightnessThreshold;
            public float DOF_BokehBlurThreshold;
            public float DOF_BokehFalloff;
            public float DOF_BokehDepthCutoff;

            // Motion Blur

            public byte MB_Enabled;
            public float MB_Scale;
            public int MB_SampleCount;
            public ResolutionMode MB_MotionVectorsResolution;

            // Post Fx Materials

            public int PostFxMaterialsCount;
            public Guid PostFxMaterial0;
            public Guid PostFxMaterial1;
            public Guid PostFxMaterial2;
            public Guid PostFxMaterial3;
            public Guid PostFxMaterial4;
            public Guid PostFxMaterial5;
            public Guid PostFxMaterial6;
            public Guid PostFxMaterial7;

            public bool GetFlag(int order)
            {
                int groupIndex = order / 100;
                int propertyIndex = order - groupIndex * 100;
                return GetFlag(groupIndex, propertyIndex);
            }

            public unsafe bool GetFlag(int groupIndex, int propertyIndex)
            {
                fixed (int* ptr = &Flags0)
                {
                    int* groupFlag = (int*)((long)ptr + groupIndex * sizeof(int));
                    int propertyFlag = 1 << propertyIndex;
                    return (*groupFlag & propertyFlag) != 0;
                }
            }

            public void SetFlag(int order, bool value)
            {
                int groupIndex = order / 100;
                int propertyIndex = order - groupIndex * 100;
                SetFlag(groupIndex, propertyIndex, value);
            }

            public unsafe void SetFlag(int groupIndex, int propertyIndex, bool value)
            {
                fixed (int* ptr = &Flags0)
                {
                    int* groupFlag = (int*)((long)ptr + groupIndex * sizeof(int));
                    int propertyFlag = 1 << propertyIndex;
                    if (value)
                        *groupFlag |= propertyFlag;
                    else
                        *groupFlag &= ~propertyFlag;
                }
            }
        }

        [NoSerialize]
        internal bool isDataDirty;

        [Serialize]
        internal Data data;

        /// <summary>
        /// The maximum allowed amount custom post fx materials assigned to <see cref="PostProcessSettings"/>.
        /// </summary>
        public const int MaxPostFxMaterials = 8;

        /// <summary>
        /// Gets the override flag for the given property.
        /// </summary>
        /// <param name="p">The property.</param>
        /// <returns>True if property value is being overriden, otherwise false.</returns>
        public bool GetOverrideFlag(PropertyInfo p)
        {
            var attributes = p.GetCustomAttributes(true);
            var order = (EditorOrderAttribute)attributes.First(x => x is EditorOrderAttribute);
            return data.GetFlag(order.Order);
        }

        /// <summary>
        /// Sets the override flag for the given property.
        /// </summary>
        /// <param name="p">The property.</param>
        /// <param name="value">True if override the property value, otherwise false..</param>
        public void SetOverrideFlag(PropertyInfo p, bool value)
        {
            var attributes = p.GetCustomAttributes(true);
            var order = (EditorOrderAttribute)attributes.First(x => x is EditorOrderAttribute);
            data.SetFlag(order.Order, value);
        }

        /// <summary>
        /// Gets the override flag for the given property.
        /// </summary>
        /// <param name="order">The property order (see <see cref="EditorOrderAttribute"/> order value for properties).</param>
        /// <returns>True if property value is being overriden, otherwise false.</returns>
        public bool GetOverrideFlag(int order)
        {
            return data.GetFlag(order);
        }

        /// <summary>
        /// Sets the override flag for the given property.
        /// </summary>
        /// <param name="order">The property order (see <see cref="EditorOrderAttribute"/> order value for properties.</param>
        /// <param name="value">True if override the property value, otherwise false..</param>
        public void SetOverrideFlag(int order, bool value)
        {
            data.SetFlag(order, value);
        }

        #region Ambient Occlusion

        /// <summary>
        /// Enable/disable ambient occlusion effect.
        /// </summary>
        [DefaultValue(true)]
        [NoSerialize, EditorOrder(100), EditorDisplay("Ambient Occlusion", "Enabled")]
        public bool AO_Enabled
        {
            get => data.AO_Enabled != 0;
            set
            {
                data.AO_Enabled = (byte)(value ? 1 : 0);
                isDataDirty = true;
            }
        }

        /// <summary>
        /// Gets or sets the ambient occlusion intensity.
        /// </summary>
        [DefaultValue(0.6f), Limit(0, 2.0f, 0.01f)]
        [NoSerialize, EditorOrder(101), EditorDisplay("Ambient Occlusion", "Intensity")]
        public float AO_Intensity
        {
            get => data.AO_Intensity;
            set
            {
                data.AO_Intensity = value;
                isDataDirty = true;
            }
        }

        /// <summary>
        /// Gets or sets the ambient occlusion power.
        /// </summary>
        [DefaultValue(0.75f), Limit(0, 4.0f, 0.01f)]
        [NoSerialize, EditorOrder(102), EditorDisplay("Ambient Occlusion", "Power")]
        public float AO_Power
        {
            get => data.AO_Power;
            set
            {
                data.AO_Power = value;
                isDataDirty = true;
            }
        }

        /// <summary>
        /// Gets or sets the ambient occlusion check range radius.
        /// </summary>
        [DefaultValue(0.7f), Limit(0, 16.0f, 0.01f)]
        [NoSerialize, EditorOrder(103), EditorDisplay("Ambient Occlusion", "Radius")]
        public float AO_Radius
        {
            get => data.AO_Radius;
            set
            {
                data.AO_Radius = value;
                isDataDirty = true;
            }
        }

        /// <summary>
        /// Gets or sets the ambient occlusion fade out distance.
        /// </summary>
        [DefaultValue(3000.0f), Limit(0, 1000000.0f)]
        [NoSerialize, EditorOrder(104), EditorDisplay("Ambient Occlusion", "Fade Out Distance")]
        public float AO_FadeOutDistance
        {
            get => data.AO_FadeOutDistance;
            set
            {
                data.AO_FadeOutDistance = value;
                isDataDirty = true;
            }
        }

        #endregion

        #region Bloom

        /// <summary>
        /// Enables/disables bloom effect.
        /// </summary>
        [DefaultValue(true)]
        [NoSerialize, EditorOrder(200), EditorDisplay("Bloom", "Enabled")]
        public bool Bloom_Enabled
        {
            get => data.Bloom_Enabled != 0;
            set
            {
                data.Bloom_Enabled = (byte)(value ? 1 : 0);
                isDataDirty = true;
            }
        }

        /// <summary>
        /// Gets or sets the bloom intensity.
        /// </summary>
        [DefaultValue(1.0f), Limit(0, 10.0f, 0.1f)]
        [NoSerialize, EditorOrder(201), EditorDisplay("Bloom", "Intensity")]
        public float Bloom_Intensity
        {
            get => data.Bloom_Intensity;
            set
            {
                data.Bloom_Intensity = value;
                isDataDirty = true;
            }
        }

        /// <summary>
        /// Gets or sets the bloom threshold. Pixels with higher luminance are glowing.
        /// </summary>
        [DefaultValue(3.0f), Limit(0, 15.0f, 0.01f)]
        [NoSerialize, EditorOrder(202), EditorDisplay("Bloom", "Threshold")]
        public float Bloom_Threshold
        {
            get => data.Bloom_Threshold;
            set
            {
                data.Bloom_Threshold = value;
                isDataDirty = true;
            }
        }

        /// <summary>
        /// Gets or sets the bloom blur sigma parameter.
        /// </summary>
        [DefaultValue(3.6f)]
        [NoSerialize, EditorOrder(203), EditorDisplay("Bloom", "Blur Sigma")]
        public float Bloom_BlurSigma
        {
            get => data.Bloom_BlurSigma;
            set
            {
                data.Bloom_BlurSigma = value;
                isDataDirty = true;
            }
        }

        /// <summary>
        /// Gets or sets the bloom blur scale.
        /// </summary>
        [DefaultValue(1.8f)]
        [NoSerialize, EditorOrder(204), EditorDisplay("Bloom", "Scale")]
        public float Bloom_Scale
        {
            get => data.Bloom_Scale;
            set
            {
                data.Bloom_Scale = value;
                isDataDirty = true;
            }
        }

        #endregion

        #region Tone Mapping

        /// <summary>
        /// Gets or sets the white color temperature. Default is 6500.
        /// </summary>
        [DefaultValue(6500.0f), Limit(1500, 15000)]
        [NoSerialize, EditorOrder(300), EditorDisplay("Tone Mapping", "White Temperature"), Tooltip("White color temperature. Default is 6500.")]
        public float ToneMap_WhiteTemp
        {
            get => data.ToneMap_WhiteTemp;
            set
            {
                data.ToneMap_WhiteTemp = value;
                isDataDirty = true;
            }
        }

        /// <summary>
        /// Gets or sets the white tint. Default is 0.
        /// </summary>
        [DefaultValue(0.0f), Limit(-1, 1, 0.001f)]
        [NoSerialize, EditorOrder(301), EditorDisplay("Tone Mapping", "White Tint"), Tooltip("White color tint. Default is 0.")]
        public float ToneMap_WhiteTint
        {
            get => data.ToneMap_WhiteTint;
            set
            {
                data.ToneMap_WhiteTint = value;
                isDataDirty = true;
            }
        }

        /// <summary>
        /// Gets or sets the film curve slope. Default is 0.88.
        /// </summary>
        [DefaultValue(0.88f), Limit(0, 1, 0.01f)]
        [NoSerialize, EditorOrder(302), EditorDisplay("Tone Mapping", "Film Slope"), Tooltip("This will adjust the steepness of the S-curve used for the tone mapper, where larger values will make the slope steeper (darker) and lower values will make the slope less steep (lighter). Default is 0.88.")]
        public float ToneMap_FilmSlope
        {
            get => data.ToneMap_FilmSlope;
            set
            {
                data.ToneMap_FilmSlope = value;
                isDataDirty = true;
            }
        }

        /// <summary>
        /// Gets or sets the film curve toe. Default is 0.55.
        /// </summary>
        [DefaultValue(0.55f), Limit(0, 1, 0.01f)]
        [NoSerialize, EditorOrder(303), EditorDisplay("Tone Mapping", "Film Toe"), Tooltip("This will adjust the dark color in the tone mapper. Default is 0.55.")]
        public float ToneMap_FilmToe
        {
            get => data.ToneMap_FilmToe;
            set
            {
                data.ToneMap_FilmToe = value;
                isDataDirty = true;
            }
        }

        /// <summary>
        /// Gets or sets the film curve shoulder. Default is 0.26.
        /// </summary>
        [DefaultValue(0.26f), Limit(0, 1, 0.01f)]
        [NoSerialize, EditorOrder(304), EditorDisplay("Tone Mapping", "Film Shoulder"), Tooltip("This will adjust the bright color in the tone mapper. Default is 0.26.")]
        public float ToneMap_FilmShoulder
        {
            get => data.ToneMap_FilmShoulder;
            set
            {
                data.ToneMap_FilmShoulder = value;
                isDataDirty = true;
            }
        }

        /// <summary>
        /// Gets or sets the film curve black clip. Default is 0.
        /// </summary>
        [DefaultValue(0.0f), Limit(0, 1, 0.01f)]
        [NoSerialize, EditorOrder(305), EditorDisplay("Tone Mapping", "Film Black Clip"), Tooltip("This will set where the crossover happens where black's start to cut off their value. In general, this value should not be adjusted. Default is 0.")]
        public float ToneMap_FilmBlackClip
        {
            get => data.ToneMap_FilmBlackClip;
            set
            {
                data.ToneMap_FilmBlackClip = value;
                isDataDirty = true;
            }
        }

        /// <summary>
        /// Gets or sets the film curve white clip. Default is 0.04.
        /// </summary>
        [DefaultValue(0.04f), Limit(0, 1, 0.01f)]
        [NoSerialize, EditorOrder(306), EditorDisplay("Tone Mapping", "Film White Clip"), Tooltip("This will set where the crossover happens where white's start to cut off their values. This will appear as a subtle change in most cases. Default is 0.04.")]
        public float ToneMap_FilmWhiteClip
        {
            get => data.ToneMap_FilmWhiteClip;
            set
            {
                data.ToneMap_FilmWhiteClip = value;
                isDataDirty = true;
            }
        }

        #endregion

        #region Eye Adaptation

        /// <summary>
        /// Gets or sets the eye adaptation mode.
        /// </summary>
        [DefaultValue(EyeAdaptationTechnique.Auto)]
        [NoSerialize, EditorOrder(400), EditorDisplay("Eye Adaptation", "Technique")]
        public EyeAdaptationTechnique Eye_Technique
        {
            get => data.Eye_Technique;
            set
            {
                data.Eye_Technique = value;
                isDataDirty = true;
            }
        }

        /// <summary>
        /// Gets or sets the speed up of the eye adaptation effect.
        /// </summary>
        [DefaultValue(3.0f), Limit(0, 100.0f, 0.01f)]
        [NoSerialize, EditorOrder(401), EditorDisplay("Eye Adaptation", "Speed Up")]
        public float Eye_SpeedUp
        {
            get => data.Eye_SpeedUp;
            set
            {
                data.Eye_SpeedUp = value;
                isDataDirty = true;
            }
        }

        /// <summary>
        /// Gets or sets the speed up of the eye adaptation effect.
        /// </summary>
        [DefaultValue(1.0f), Limit(0, 100.0f, 0.01f)]
        [NoSerialize, EditorOrder(402), EditorDisplay("Eye Adaptation", "Speed Down")]
        public float Eye_SpeedDown
        {
            get => data.Eye_SpeedDown;
            set
            {
                data.Eye_SpeedDown = value;
                isDataDirty = true;
            }
        }

        /// <summary>
        /// Gets or sets the camera exposure.
        /// </summary>
        [DefaultValue(1.5f), Limit(-1000, 1000, 0.001f)]
        [NoSerialize, EditorOrder(403), EditorDisplay("Eye Adaptation", "Exposure")]
        public float Eye_Exposure
        {
            get => data.Eye_Exposure;
            set
            {
                data.Eye_Exposure = value;
                isDataDirty = true;
            }
        }

        /// <summary>
        /// Gets or sets the pixels light value to achieve.
        /// </summary>
        [DefaultValue(0.2f), Limit(-100, 100, 0.001f)]
        [NoSerialize, EditorOrder(404), EditorDisplay("Eye Adaptation", "Key Value")]
        public float Eye_KeyValue
        {
            get => data.Eye_KeyValue;
            set
            {
                data.Eye_KeyValue = value;
                isDataDirty = true;
            }
        }

        /// <summary>
        /// Gets or sets the minimum luminance value used for tone mapping.
        /// </summary>
        [DefaultValue(0.01f), Limit(0, 50.0f, 0.01f)]
        [NoSerialize, EditorOrder(405), EditorDisplay("Eye Adaptation", "Minimum luminance")]
        public float Eye_MinLuminance
        {
            get => data.Eye_MinLuminance;
            set
            {
                data.Eye_MinLuminance = value;
                isDataDirty = true;
            }
        }

        /// <summary>
        /// Gets or sets the maximum luminance value used for tone mapping.
        /// </summary>
        [DefaultValue(100.0f), Limit(0, 100.0f, 0.01f)]
        [NoSerialize, EditorOrder(406), EditorDisplay("Eye Adaptation", "Maximum luminance")]
        public float Eye_MaxLuminance
        {
            get => data.Eye_MaxLuminance;
            set
            {
                data.Eye_MaxLuminance = value;
                isDataDirty = true;
            }
        }

        #endregion

        #region Camera Artifacts

        /// <summary>
        /// Gets or sets the vignette intensity.
        /// </summary>
        [DefaultValue(0.8f), Limit(0, 2, 0.001f)]
        [NoSerialize, EditorOrder(500), EditorDisplay("Camera Artifacts", "Vignette Intensity")]
        public float Cam_VignetteIntensity
        {
            get => data.Cam_VignetteIntensity;
            set
            {
                data.Cam_VignetteIntensity = value;
                isDataDirty = true;
            }
        }

        /// <summary>
        /// Gets or sets the vignette color.
        /// </summary>
        [NoSerialize, EditorOrder(501), EditorDisplay("Camera Artifacts", "Vignette Color")]
        public Color Cam_VignetteColor
        {
            get => data.Cam_VignetteColor;
            set
            {
                data.Cam_VignetteColor = value;
                isDataDirty = true;
            }
        }

        /// <summary>
        /// Gets or sets the vignette shape factor.
        /// </summary>
        [DefaultValue(0.125f), Limit(0.0001f, 2.0f, 0.001f)]
        [NoSerialize, EditorOrder(502), EditorDisplay("Camera Artifacts", "Vignette Shape Factor")]
        public float Cam_VignetteShapeFactor
        {
            get => data.Cam_VignetteShapeFactor;
            set
            {
                data.Cam_VignetteShapeFactor = value;
                isDataDirty = true;
            }
        }

        /// <summary>
        /// Gets or sets the grain noise amount.
        /// </summary>
        [DefaultValue(0.006f), Limit(0.0f, 2.0f, 0.005f)]
        [NoSerialize, EditorOrder(503), EditorDisplay("Camera Artifacts", "Grain Amount")]
        public float Cam_GrainAmount
        {
            get => data.Cam_GrainAmount;
            set
            {
                data.Cam_GrainAmount = value;
                isDataDirty = true;
            }
        }

        /// <summary>
        /// Gets or sets the grain noise particles size.
        /// </summary>
        [DefaultValue(1.6f), Limit(1.0f, 3.0f, 0.01f)]
        [NoSerialize, EditorOrder(504), EditorDisplay("Camera Artifacts", "Grain Particle Size")]
        public float Cam_GrainParticleSize
        {
            get => data.Cam_GrainParticleSize;
            set
            {
                data.Cam_GrainParticleSize = value;
                isDataDirty = true;
            }
        }

        /// <summary>
        /// Gets or sets the grain noise particles size.
        /// </summary>
        [DefaultValue(1.0f), Limit(0.0f, 10.0f, 0.01f)]
        [NoSerialize, EditorOrder(505), EditorDisplay("Camera Artifacts", "Grain Speed"), Tooltip("Specifies grain particles animation speed")]
        public float Cam_GrainSpeed
        {
            get => data.Cam_GrainSpeed;
            set
            {
                data.Cam_GrainSpeed = value;
                isDataDirty = true;
            }
        }

        /// <summary>
        /// Gets or sets the chromatic aberration distortion intensity.
        /// </summary>
        [DefaultValue(0.0f), Limit(0.0f, 1.0f, 0.01f)]
        [NoSerialize, EditorOrder(506), EditorDisplay("Camera Artifacts", "Chromatic Distortion")]
        public float Cam_ChromaticDistortion
        {
            get => data.Cam_ChromaticDistortion;
            set
            {
                data.Cam_ChromaticDistortion = value;
                isDataDirty = true;
            }
        }

        #endregion

        #region Lens Flares

        /// <summary>
        /// Gets or sets the lens flares intensity.
        /// </summary>
        [DefaultValue(1.0f), Limit(0, 10.0f, 0.01f)]
        [NoSerialize, EditorOrder(600), EditorDisplay("Lens Flares", "Intensity")]
        public float Flare_Intensity
        {
            get => data.Flare_Intensity;
            set
            {
                data.Flare_Intensity = value;
                isDataDirty = true;
            }
        }

        /// <summary>
        /// Gets or sets the amount of lens flares ghosts.
        /// </summary>
        [DefaultValue(8), Limit(0, 16)]
        [NoSerialize, EditorOrder(601), EditorDisplay("Lens Flares", "Ghosts")]
        public int Flare_Ghosts
        {
            get => data.Flare_Ghosts;
            set
            {
                data.Flare_Ghosts = value;
                isDataDirty = true;
            }
        }

        /// <summary>
        /// Gets or sets the lens flares halo size.
        /// </summary>
        [DefaultValue(0.16f)]
        [NoSerialize, EditorOrder(602), EditorDisplay("Lens Flares", "Halo Width")]
        public float Flare_HaloWidth
        {
            get => data.Flare_HaloWidth;
            set
            {
                data.Flare_HaloWidth = value;
                isDataDirty = true;
            }
        }

        /// <summary>
        /// Gets or sets the lens flares halo intensity.
        /// </summary>
        [DefaultValue(0.666f), Limit(0, 10.0f, 0.01f)]
        [NoSerialize, EditorOrder(603), EditorDisplay("Lens Flares", "Halo Intensity")]
        public float Flare_HaloIntensity
        {
            get => data.Flare_HaloIntensity;
            set
            {
                data.Flare_HaloIntensity = value;
                isDataDirty = true;
            }
        }

        /// <summary>
        /// Gets or sets the lens flares ghosts dispersal.
        /// </summary>
        [DefaultValue(0.3f)]
        [NoSerialize, EditorOrder(604), EditorDisplay("Lens Flares", "Ghost Dispersal")]
        public float Flare_GhostDispersal
        {
            get => data.Flare_GhostDispersal;
            set
            {
                data.Flare_GhostDispersal = value;
                isDataDirty = true;
            }
        }

        /// <summary>
        /// Gets or sets the lens flares distortion.
        /// </summary>
        [DefaultValue(1.5f)]
        [NoSerialize, EditorOrder(605), EditorDisplay("Lens Flares", "Distortion")]
        public float Flare_Distortion
        {
            get => data.Flare_Distortion;
            set
            {
                data.Flare_Distortion = value;
                isDataDirty = true;
            }
        }

        /// <summary>
        /// Gets or sets the lens flares threshold bias.
        /// </summary>
        [DefaultValue(-0.5f)]
        [NoSerialize, EditorOrder(606), EditorDisplay("Lens Flares", "Threshold Bias")]
        public float Flare_ThresholdBias
        {
            get => data.Flare_ThresholdBias;
            set
            {
                data.Flare_ThresholdBias = value;
                isDataDirty = true;
            }
        }

        /// <summary>
        /// Gets or sets the lens flares threshold scale.
        /// </summary>
        [DefaultValue(0.22f)]
        [NoSerialize, EditorOrder(607), EditorDisplay("Lens Flares", "Threshold Scale")]
        public float Flare_ThresholdScale
        {
            get => data.Flare_ThresholdScale;
            set
            {
                data.Flare_ThresholdScale = value;
                isDataDirty = true;
            }
        }

        /// <summary>
        /// Gets or sets the camera lens dirt texture.
        /// </summary>
        [DefaultValue(null)]
        [NoSerialize, EditorOrder(608), EditorDisplay("Lens Flares", "Lens Dirt"), Tooltip("Custom texture for camera dirt")]
        public Texture Flare_LensDirt
        {
            get => Content.LoadAsync<Texture>(data.Flare_LensDirt);
            set
            {
                data.Flare_LensDirt = value?.ID ?? Guid.Empty;
                isDataDirty = true;
            }
        }

        /// <summary>
        /// Gets or sets the lens dirt intensity.
        /// </summary>
        [DefaultValue(1.0f), Limit(0, 100, 0.01f)]
        [NoSerialize, EditorOrder(609), EditorDisplay("Lens Flares", "Lens Dirt Intensity")]
        public float Flare_LensDirtIntensity
        {
            get => data.Flare_LensDirtIntensity;
            set
            {
                data.Flare_LensDirtIntensity = value;
                isDataDirty = true;
            }
        }

        /// <summary>
        /// Gets or sets the camera lens color lookup texture.
        /// </summary>
        [DefaultValue(null)]
        [NoSerialize, EditorOrder(610), EditorDisplay("Lens Flares", "Lens Color"), Tooltip("Custom texture for lens flares color")]
        public Texture Flare_LensColor
        {
            get => Content.LoadAsync<Texture>(data.Flare_LensColor);
            set
            {
                data.Flare_LensColor = value?.ID ?? Guid.Empty;
                isDataDirty = true;
            }
        }

        /// <summary>
        /// Gets or sets the lens star lookup texture.
        /// </summary>
        [DefaultValue(null)]
        [NoSerialize, EditorOrder(611), EditorDisplay("Lens Flares", "Lens Star"), Tooltip("Custom texture for lens flares star")]
        public Texture Flare_LensStar
        {
            get => Content.LoadAsync<Texture>(data.Flare_LensStar);
            set
            {
                data.Flare_LensStar = value?.ID ?? Guid.Empty;
                isDataDirty = true;
            }
        }

        #endregion

        #region Depth of Field

        /// <summary>
        /// Gets or sets a value indicating whether Depth of Field is enabled.
        /// </summary>
        [DefaultValue(false)]
        [NoSerialize, EditorOrder(700), EditorDisplay("Depth of Field", "Enabled"), Tooltip("Enable depth of field effect")]
        public bool DOF_Enabled
        {
            get => data.DOF_Enabled != 0;
            set
            {
                data.DOF_Enabled = (byte)(value ? 1 : 0);
                isDataDirty = true;
            }
        }

        /// <summary>
        /// Gets or sets the blur intensity in the out-of-focus areas. Allows reducing blur amount by scaling down the Gaussian Blur radius. Normalized to range 0-1.
        /// </summary>
        [DefaultValue(1.0f), Limit(0, 1, 0.1f)]
        [NoSerialize, EditorOrder(701), EditorDisplay("Depth of Field", "Blur Strength"), Tooltip("The blur intensity in the out-of-focus areas. Allows reducing blur amount by scaling down the Gaussian Blur radius. Normalized to range 0-1.")]
        public float DOF_BlurStrength
        {
            get => data.DOF_BlurStrength;
            set
            {
                data.DOF_BlurStrength = value;
                isDataDirty = true;
            }
        }

        /// <summary>
        /// Gets or sets the distance in World Units from the camera that acts as the center of the region where the scene is perfectly in focus and no blurring occurs.
        /// </summary>
        [DefaultValue(550.0f), Limit(0)]
        [NoSerialize, EditorOrder(702), EditorDisplay("Depth of Field", "Focal Distance"), Tooltip("The distance in World Units from the camera that acts as the center of the region where the scene is perfectly in focus and no blurring occurs")]
        public float DOF_FocalDistance
        {
            get => data.DOF_FocalDistance;
            set
            {
                data.DOF_FocalDistance = value;
                isDataDirty = true;
            }
        }

        /// <summary>
        /// Gets or sets the distance in World Units beyond the focal distance where the scene is perfectly in focus and no blurring occurs.
        /// </summary>
        [DefaultValue(1000.0f), Limit(0)]
        [NoSerialize, EditorOrder(703), EditorDisplay("Depth of Field", "Focal Region"), Tooltip("The distance in World Units beyond the focal distance where the scene is perfectly in focus and no blurring occurs")]
        public float DOF_FocalRegion
        {
            get => data.DOF_FocalRegion;
            set
            {
                data.DOF_FocalRegion = value;
                isDataDirty = true;
            }
        }

        /// <summary>
        /// Gets or sets the distance in World Units from the focal region on the side nearer to the camera over which the scene transitions from focused to blurred.
        /// </summary>
        [DefaultValue(80.0f), Limit(0)]
        [NoSerialize, EditorOrder(704), EditorDisplay("Depth of Field", "Near Transition Range"), Tooltip("The distance in World Units from the focal region on the side nearer to the camera over which the scene transitions from focused to blurred")]
        public float DOF_NearTransitionRange
        {
            get => data.DOF_NearTransitionRange;
            set
            {
                data.DOF_NearTransitionRange = value;
                isDataDirty = true;
            }
        }

        /// <summary>
        /// Gets or sets the distance in World Units from the focal region on the side farther from the camera over which the scene transitions from focused to blurred.
        /// </summary>
        [DefaultValue(100.0f), Limit(0)]
        [NoSerialize, EditorOrder(705), EditorDisplay("Depth of Field", "Far Transition Range"), Tooltip("The distance in World Units from the focal region on the side farther from the camera over which the scene transitions from focused to blurred")]
        public float DOF_FarTransitionRange
        {
            get => data.DOF_FarTransitionRange;
            set
            {
                data.DOF_FarTransitionRange = value;
                isDataDirty = true;
            }
        }

        /// <summary>
        /// Gets or sets the distance in World Units which describes border after that there is no blur (useful to disable DoF on sky). Use 0 to disable that feature.
        /// </summary>
        [DefaultValue(6000.0f), Limit(0, float.MaxValue, 2)]
        [NoSerialize, EditorOrder(706), EditorDisplay("Depth of Field", "Depth Limit"), Tooltip("The distance in World Units which describes border after that there is no blur (useful to disable DoF on sky). Use 0 to disable that feature.")]
        public float DOF_DepthLimit
        {
            get => data.DOF_DepthLimit;
            set
            {
                data.DOF_DepthLimit = value;
                isDataDirty = true;
            }
        }

        /// <summary>
        /// Enables/disables generating Bokeh shapes.
        /// </summary>
        [DefaultValue(true)]
        [NoSerialize, EditorOrder(707), EditorDisplay("Depth of Field", "Bokeh Enable"), Tooltip("Enables/disables generating Bokeh shapes")]
        public bool DOF_BokehEnabled
        {
            get => data.DOF_BokehEnabled != 0;
            set
            {
                data.DOF_BokehEnabled = (byte)(value ? 1 : 0);
                isDataDirty = true;
            }
        }

        /// <summary>
        /// Controls Bokeh shapes maximum size.
        /// </summary>
        [DefaultValue(25.0f), Limit(0, 200.0f, 0.1f)]
        [NoSerialize, EditorOrder(708), EditorDisplay("Depth of Field", "Bokeh Size"), Tooltip("Controls Bokeh shapes maximum size")]
        public float DOF_BokehSize
        {
            get => data.DOF_BokehSize;
            set
            {
                data.DOF_BokehSize = value;
                isDataDirty = true;
            }
        }

        /// <summary>
        /// Gets or sets the Bokeh shapes style.
        /// </summary>
        [DefaultValue(BokehShapeType.Circle)]
        [NoSerialize, EditorOrder(709), EditorDisplay("Depth of Field", "Bokeh Shape"), Tooltip("Bokeh shapes style")]
        public BokehShapeType DOF_BokehShape
        {
            get => data.DOF_BokehShape;
            set
            {
                data.DOF_BokehShape = value;
                isDataDirty = true;
            }
        }

        /// <summary>
        /// Gets or sets the custom texture for bokeh shapes.
        /// </summary>
        [DefaultValue(null)]
        [NoSerialize, EditorOrder(710), EditorDisplay("Depth of Field", "Bokeh Shape Custom Texture"), Tooltip("Custom texture for bokeh shapes")]
        public Texture DOF_BokehShapeCustom
        {
            get => Content.LoadAsync<Texture>(data.DOF_BokehShapeCustom);
            set
            {
                data.DOF_BokehShapeCustom = value?.ID ?? Guid.Empty;
                isDataDirty = true;
            }
        }

        /// <summary>
        /// Controls Bokeh shapes generating minimum pixel brightness to appear.
        /// </summary>
        [DefaultValue(0.8f), Limit(0, 10.0f, 0.01f)]
        [NoSerialize, EditorOrder(711), EditorDisplay("Depth of Field", "Bokeh Brightness Threshold"), Tooltip("Controls Bokeh shapes generating minimum pixel brightness to appear")]
        public float DOF_BokehBrightnessThreshold
        {
            get => data.DOF_BokehBrightnessThreshold;
            set
            {
                data.DOF_BokehBrightnessThreshold = value;
                isDataDirty = true;
            }
        }

        /// <summary>
        /// Controls Bokeh shapes blur threshold.
        /// </summary>
        [DefaultValue(0.05f), Limit(0, 1.0f, 0.001f)]
        [NoSerialize, EditorOrder(712), EditorDisplay("Depth of Field", "Bokeh Blur Threshold"), Tooltip("Controls Bokeh shapes blur threshold")]
        public float DOF_BokehBlurThreshold
        {
            get => data.DOF_BokehBlurThreshold;
            set
            {
                data.DOF_BokehBlurThreshold = value;
                isDataDirty = true;
            }
        }

        /// <summary>
        /// Controls Bokeh shapes brightness falloff parameter.
        /// </summary>
        [DefaultValue(0.5f), Limit(0, 2.0f, 0.001f)]
        [NoSerialize, EditorOrder(713), EditorDisplay("Depth of Field", "Bokeh Falloff"), Tooltip("Controls Bokeh shapes brightness falloff parameter")]
        public float DOF_BokehFalloff
        {
            get => data.DOF_BokehFalloff;
            set
            {
                data.DOF_BokehFalloff = value;
                isDataDirty = true;
            }
        }

        /// <summary>
        /// Controls Bokeh shapes depth cutoff parameter.
        /// </summary>
        [DefaultValue(1.5f), Limit(0, 5.0f, 0.001f)]
        [NoSerialize, EditorOrder(714), EditorDisplay("Depth of Field", "Bokeh Depth Cutoff"), Tooltip("Controls Bokeh shapes depth cutoff parameter")]
        public float DOF_BokehDepthCutoff
        {
            get => data.DOF_BokehDepthCutoff;
            set
            {
                data.DOF_BokehDepthCutoff = value;
                isDataDirty = true;
            }
        }

        #endregion

        #region Screen Space Reflections

        /// <summary>
        /// Gets or sets the effect intensity (normalized to range [0;1]). Use 0 to disable it.
        /// </summary>
        [DefaultValue(1.0f), Limit(0, 1.0f, 0.01f)]
        [NoSerialize, EditorOrder(800), EditorDisplay("Screen Space Reflections", "Intensity"), Tooltip("Effect intensity (normalized to range [0;1]). Use 0 to disable it.")]
        public float SSR_Intensity
        {
            get => data.SSR_Intensity;
            set
            {
                data.SSR_Intensity = value;
                isDataDirty = true;
            }
        }

        /// <summary>
        /// Gets or sets the input depth resolution mode.
        /// </summary>
        [DefaultValue(ResolutionMode.Half)]
        [NoSerialize, EditorOrder(801), EditorDisplay("Screen Space Reflections", "Depth Resolution"), Tooltip("The depth buffer downscale option to optimize raycast performance. Full gives better quality, but half improves performance. The default value is half.")]
        public ResolutionMode SSR_DepthResolution
        {
            get => data.SSR_DepthResolution;
            set
            {
                data.SSR_DepthResolution = value;
                isDataDirty = true;
            }
        }

        /// <summary>
        /// Gets or sets the ray trace pass resolution mode.
        /// </summary>
        [DefaultValue(ResolutionMode.Half)]
        [NoSerialize, EditorOrder(802), EditorDisplay("Screen Space Reflections", "Ray Trace Resolution"), Tooltip("The raycast resolution. Full gives better quality, but half improves performance. The default value is half.")]
        public ResolutionMode SSR_RayTracePassResolution
        {
            get => data.SSR_RayTracePassResolution;
            set
            {
                data.SSR_RayTracePassResolution = value;
                isDataDirty = true;
            }
        }

        /// <summary>
        /// Gets or sets the BRDF bias. This value controls source roughness effect on reflections blur.
        /// Smaller values produce wider reflections spread but also introduce more noise.
        /// Higher values provide more mirror-like reflections. Default value is 0.8.
        /// </summary>
        [DefaultValue(0.82f), Limit(0, 1.0f, 0.01f)]
        [NoSerialize, EditorOrder(803), EditorDisplay("Screen Space Reflections", "BRDF Bias"), Tooltip("The reflection spread. Higher values provide finer, more mirror-like reflections. This setting has no effect on performance. The default value is 0.82")]
        public float SSR_BRDFBias
        {
            get => data.SSR_BRDFBias;
            set
            {
                data.SSR_BRDFBias = value;
                isDataDirty = true;
            }
        }

        /// <summary>
        /// Minimum allowed surface roughness value to use local reflections.
        /// Pixels with higher values won't be affected by the effect.
        /// </summary>
        [DefaultValue(0.45f), Limit(0, 1.0f, 0.01f)]
        [NoSerialize, EditorOrder(804), EditorDisplay("Screen Space Reflections", "Roughness Threshold"), Tooltip("The maximum amount of roughness a material must have to reflect the scene. For example, if this value is set to 0.4, only materials with a roughness value of 0.4 or below reflect the scene. The default value is 0.45.")]
        public float SSR_RoughnessThreshold
        {
            get => data.SSR_RoughnessThreshold;
            set
            {
                data.SSR_RoughnessThreshold = value;
                isDataDirty = true;
            }
        }

        /// <summary>
        /// Ray tracing starting position is offset by a percent of the normal in world space to avoid self occlusions.
        /// </summary>
        [DefaultValue(0.1f), Limit(0, 10.0f, 0.01f)]
        [NoSerialize, EditorOrder(805), EditorDisplay("Screen Space Reflections", "World Anti Self Occlusion Bias"), Tooltip("The offset of the raycast origin. Lower values produce more correct reflection placement, but produce more artifacts. We recommend values of 0.3 or lower. The default value is 0.1.")]
        public float SSR_WorldAntiSelfOcclusionBias
        {
            get => data.SSR_WorldAntiSelfOcclusionBias;
            set
            {
                data.SSR_WorldAntiSelfOcclusionBias = value;
                isDataDirty = true;
            }
        }

        /// <summary>
        /// Gets or sets the resolve pass resolution mode.
        /// </summary>
        [DefaultValue(ResolutionMode.Full)]
        [NoSerialize, EditorOrder(806), EditorDisplay("Screen Space Reflections", "Resolve Resolution"), Tooltip("The raycast resolution. Full gives better quality, but half improves performance. The default value is half.")]
        public ResolutionMode SSR_ResolvePassResolution
        {
            get => data.SSR_ResolvePassResolution;
            set
            {
                data.SSR_ResolvePassResolution = value;
                isDataDirty = true;
            }
        }

        /// <summary>
        /// Gets or sets the resolve pass samples amount. Higher values provide better quality but reduce effect performance.
        /// Default value is 4. Use 1 for the highest speed.
        /// </summary>
        [DefaultValue(4), Limit(1, 8)]
        [NoSerialize, EditorOrder(807), EditorDisplay("Screen Space Reflections", "Resolve Samples"), Tooltip("The number of rays used to resolve the reflection color. Higher values produce less noise, but worse performance. The default value is 4.")]
        public int SSR_ResolveSamples
        {
            get => data.SSR_ResolveSamples;
            set
            {
                data.SSR_ResolveSamples = value;
                isDataDirty = true;
            }
        }

        /// <summary>
        /// Gets or sets the edge fade factor. It's used to fade off effect on screen edges to provide smoother image.
        /// </summary>
        [DefaultValue(0.1f), Limit(0, 1.0f, 0.02f)]
        [NoSerialize, EditorOrder(808), EditorDisplay("Screen Space Reflections", "Edge Fade Factor"), Tooltip("The point at which the far edges of the reflection begin to fade. Has no effect on performance. The default value is 0.1.")]
        public float SSR_EdgeFadeFactor
        {
            get => data.SSR_EdgeFadeFactor;
            set
            {
                data.SSR_EdgeFadeFactor = value;
                isDataDirty = true;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether use color buffer mipmaps chain; otherwise will use raw input color buffer to sample reflections color.
        /// Using mipmaps improves resolve pass performance and reduces GPU cache misses.
        /// </summary>
        [DefaultValue(true)]
        [NoSerialize, EditorOrder(809), EditorDisplay("Screen Space Reflections", "Use Color Buffer Mips"), Tooltip("The input color buffer downscale mode that uses blurred mipmaps when resolving the reflection color. Produces more realistic results by blurring distant parts of reflections in rough (low-gloss) materials. It also improves performance on most platforms but uses more memory.")]
        public bool SSR_UseColorBufferMips
        {
            get => data.SSR_UseColorBufferMips != 0;
            set
            {
                data.SSR_UseColorBufferMips = (byte)(value ? 1 : 0);
                isDataDirty = true;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether use temporal effect to smooth reflections.
        /// </summary>
        [DefaultValue(true)]
        [NoSerialize, EditorOrder(810), EditorDisplay("Screen Space Reflections", "Enable Temporal Effect"), Tooltip("Enables the temporal pass. Reduces noise, but produces an animated \"jittering\" effect that's sometimes noticeable. If disabled, the properties below have no effect.")]
        public bool SSR_TemporalEffect
        {
            get => data.SSR_TemporalEffect != 0;
            set
            {
                data.SSR_TemporalEffect = (byte)(value ? 1 : 0);
                isDataDirty = true;
            }
        }

        /// <summary>
        /// Gets or sets the temporal effect scale. Default is 8.
        /// </summary>
        [DefaultValue(8.0f), Limit(0, 20.0f, 0.5f)]
        [NoSerialize, EditorOrder(811), EditorDisplay("Screen Space Reflections", "Temporal Scale"), Tooltip("The intensity of the temporal effect. Lower values produce reflections faster, but more noise. The default value is 8.")]
        public float SSR_TemporalScale
        {
            get => data.SSR_TemporalScale;
            set
            {
                data.SSR_TemporalScale = value;
                isDataDirty = true;
            }
        }

        /// <summary>
        /// Gets or sets the temporal response. Default is 0.8.
        /// </summary>
        [DefaultValue(0.8f), Limit(0.05f, 1.0f, 0.01f)]
        [NoSerialize, EditorOrder(812), EditorDisplay("Screen Space Reflections", "Temporal Response"), Tooltip("How quickly reflections blend between the reflection in the current frame and the history buffer. Lower values produce reflections faster, but with more jittering. If the camera in your game doesn't move much, we recommend values closer to 1. The default value is 0.8.")]
        public float SSR_TemporalResponse
        {
            get => data.SSR_TemporalResponse;
            set
            {
                data.SSR_TemporalResponse = value;
                isDataDirty = true;
            }
        }

        #endregion

        #region Color Grading

        /// <summary>
        /// The track ball editor typename used for color grading knobs. Use custom editor alias because FlaxEditor assembly is not referenced by the FlaxEngine.
        /// </summary>
        private const string TrackBallEditorTypename = "FlaxEditor.CustomEditors.Editors.ColorTrackball";

        #region Global

        /// <summary>
        /// Gets or sets the color saturation (applies globally to the whole image). Default is 1.
        /// </summary>
        [DefaultValue(typeof(Vector4), "1,1,1,1")]
        [CustomEditorAlias(TrackBallEditorTypename)]
        [NoSerialize, EditorOrder(900), Limit(0, 2, 0.01f), EditorDisplay("Color Grading", "Saturation"), Tooltip("Color saturation (applies globally to the whole image). Default is 1.")]
        public Vector4 ColorGrading_ColorSaturation
        {
            get => data.ColorGrading_ColorSaturation;
            set
            {
                data.ColorGrading_ColorSaturation = value;
                isDataDirty = true;
            }
        }

        /// <summary>
        /// Gets or sets the color contrast (applies globally to the whole image). Default is 1.
        /// </summary>
        [DefaultValue(typeof(Vector4), "1,1,1,1")]
        [CustomEditorAlias(TrackBallEditorTypename)]
        [NoSerialize, EditorOrder(901), Limit(0, 2, 0.01f), EditorDisplay("Color Grading", "Contrast"), Tooltip("Color contrast (applies globally to the whole image). Default is 1.")]
        public Vector4 ColorGrading_ColorContrast
        {
            get => data.ColorGrading_ColorContrast;
            set
            {
                data.ColorGrading_ColorContrast = value;
                isDataDirty = true;
            }
        }

        /// <summary>
        /// Gets or sets the color gamma (applies globally to the whole image). Default is 1.
        /// </summary>
        [DefaultValue(typeof(Vector4), "1,1,1,1")]
        [CustomEditorAlias(TrackBallEditorTypename)]
        [NoSerialize, EditorOrder(902), Limit(0, 2, 0.01f), EditorDisplay("Color Grading", "Gamma"), Tooltip("Color gamma (applies globally to the whole image). Default is 1.")]
        public Vector4 ColorGrading_ColorGamma
        {
            get => data.ColorGrading_ColorGamma;
            set
            {
                data.ColorGrading_ColorGamma = value;
                isDataDirty = true;
            }
        }

        /// <summary>
        /// Gets or sets the color gain (applies globally to the whole image). Default is 1.
        /// </summary>
        [DefaultValue(typeof(Vector4), "1,1,1,1")]
        [CustomEditorAlias(TrackBallEditorTypename)]
        [NoSerialize, EditorOrder(903), Limit(0, 2, 0.01f), EditorDisplay("Color Grading", "Gain"), Tooltip("Color gain (applies globally to the whole image). Default is 1.")]
        public Vector4 ColorGrading_ColorGain
        {
            get => data.ColorGrading_ColorGain;
            set
            {
                data.ColorGrading_ColorGain = value;
                isDataDirty = true;
            }
        }

        /// <summary>
        /// Gets or sets the color offset (applies globally to the whole image). Default is 0.
        /// </summary>
        [DefaultValue(typeof(Vector4), "0,0,0,0")]
        [CustomEditorAlias(TrackBallEditorTypename)]
        [NoSerialize, EditorOrder(904), Limit(-1, 1, 0.001f), EditorDisplay("Color Grading", "Offset"), Tooltip("Color offset (applies globally to the whole image). Default is 0.")]
        public Vector4 ColorGrading_ColorOffset
        {
            get => data.ColorGrading_ColorOffset;
            set
            {
                data.ColorGrading_ColorOffset = value;
                isDataDirty = true;
            }
        }

        #endregion

        #region Shadows

        /// <summary>
        /// Gets or sets the color saturation (applies to shadows only). Default is 1.
        /// </summary>
        [DefaultValue(typeof(Vector4), "1,1,1,1")]
        [CustomEditorAlias(TrackBallEditorTypename)]
        [NoSerialize, EditorOrder(905), Limit(0, 2, 0.01f), EditorDisplay("Color Grading", "Shadows Saturation"), Tooltip("Color saturation (applies to shadows only). Default is 1.")]
        public Vector4 ColorGrading_ColorSaturationShadows
        {
            get => data.ColorGrading_ColorSaturationShadows;
            set
            {
                data.ColorGrading_ColorSaturationShadows = value;
                isDataDirty = true;
            }
        }

        /// <summary>
        /// Gets or sets the color contrast (applies to shadows only). Default is 1.
        /// </summary>
        [DefaultValue(typeof(Vector4), "1,1,1,1")]
        [CustomEditorAlias(TrackBallEditorTypename)]
        [NoSerialize, EditorOrder(906), Limit(0, 2, 0.01f), EditorDisplay("Color Grading", "Shadows Contrast"), Tooltip("Color contrast (applies to shadows only). Default is 1.")]
        public Vector4 ColorGrading_ColorContrastShadows
        {
            get => data.ColorGrading_ColorContrastShadows;
            set
            {
                data.ColorGrading_ColorContrastShadows = value;
                isDataDirty = true;
            }
        }

        /// <summary>
        /// Gets or sets the color gamma (applies to shadows only). Default is 1.
        /// </summary>
        [DefaultValue(typeof(Vector4), "1,1,1,1")]
        [CustomEditorAlias(TrackBallEditorTypename)]
        [NoSerialize, EditorOrder(907), Limit(0, 2, 0.01f), EditorDisplay("Color Grading", "Shadows Gamma"), Tooltip("Color gamma (applies to shadows only). Default is 1.")]
        public Vector4 ColorGrading_ColorGammaShadows
        {
            get => data.ColorGrading_ColorGammaShadows;
            set
            {
                data.ColorGrading_ColorGammaShadows = value;
                isDataDirty = true;
            }
        }

        /// <summary>
        /// Gets or sets the color gain (applies to shadows only). Default is 1.
        /// </summary>
        [DefaultValue(typeof(Vector4), "1,1,1,1")]
        [CustomEditorAlias(TrackBallEditorTypename)]
        [NoSerialize, EditorOrder(908), Limit(0, 2, 0.01f), EditorDisplay("Color Grading", "Shadows Gain"), Tooltip("Color gain (applies to shadows only). Default is 1.")]
        public Vector4 ColorGrading_ColorGainShadows
        {
            get => data.ColorGrading_ColorGainShadows;
            set
            {
                data.ColorGrading_ColorGainShadows = value;
                isDataDirty = true;
            }
        }

        /// <summary>
        /// Gets or sets the color offset (applies to shadows only). Default is 0.
        /// </summary>
        [DefaultValue(typeof(Vector4), "0,0,0,0")]
        [CustomEditorAlias(TrackBallEditorTypename)]
        [NoSerialize, EditorOrder(909), Limit(-1, 1, 0.001f), EditorDisplay("Color Grading", "Shadows Offset"), Tooltip("Color offset (applies to shadows only). Default is 0.")]
        public Vector4 ColorGrading_ColorOffsetShadows
        {
            get => data.ColorGrading_ColorOffsetShadows;
            set
            {
                data.ColorGrading_ColorOffsetShadows = value;
                isDataDirty = true;
            }
        }

        #endregion

        #region Midtones

        /// <summary>
        /// Gets or sets the color saturation (applies to midtones only). Default is 1.
        /// </summary>
        [DefaultValue(typeof(Vector4), "1,1,1,1")]
        [CustomEditorAlias(TrackBallEditorTypename)]
        [NoSerialize, EditorOrder(910), Limit(0, 2, 0.01f), EditorDisplay("Color Grading", "Midtones Saturation"), Tooltip("Color saturation (applies to midtones only). Default is 1.")]
        public Vector4 ColorGrading_ColorSaturationMidtones
        {
            get => data.ColorGrading_ColorSaturationMidtones;
            set
            {
                data.ColorGrading_ColorSaturationMidtones = value;
                isDataDirty = true;
            }
        }

        /// <summary>
        /// Gets or sets the color contrast (applies to midtones only). Default is 1.
        /// </summary>
        [DefaultValue(typeof(Vector4), "1,1,1,1")]
        [CustomEditorAlias(TrackBallEditorTypename)]
        [NoSerialize, EditorOrder(911), Limit(0, 2, 0.01f), EditorDisplay("Color Grading", "Midtones Contrast"), Tooltip("Color contrast (applies to midtones only). Default is 1.")]
        public Vector4 ColorGrading_ColorContrastMidtones
        {
            get => data.ColorGrading_ColorContrastMidtones;
            set
            {
                data.ColorGrading_ColorContrastMidtones = value;
                isDataDirty = true;
            }
        }

        /// <summary>
        /// Gets or sets the color gamma (applies to midtones only). Default is 1.
        /// </summary>
        [DefaultValue(typeof(Vector4), "1,1,1,1")]
        [CustomEditorAlias(TrackBallEditorTypename)]
        [NoSerialize, EditorOrder(912), Limit(0, 2, 0.01f), EditorDisplay("Color Grading", "Midtones Gamma"), Tooltip("Color gamma (applies to midtones only). Default is 1.")]
        public Vector4 ColorGrading_ColorGammaMidtones
        {
            get => data.ColorGrading_ColorGammaMidtones;
            set
            {
                data.ColorGrading_ColorGammaMidtones = value;
                isDataDirty = true;
            }
        }

        /// <summary>
        /// Gets or sets the color gain (applies to midtones only). Default is 1.
        /// </summary>
        [DefaultValue(typeof(Vector4), "1,1,1,1")]
        [CustomEditorAlias(TrackBallEditorTypename)]
        [NoSerialize, EditorOrder(913), Limit(0, 2, 0.01f), EditorDisplay("Color Grading", "Midtones Gain"), Tooltip("Color gain (applies to midtones only). Default is 1.")]
        public Vector4 ColorGrading_ColorGainMidtones
        {
            get => data.ColorGrading_ColorGainMidtones;
            set
            {
                data.ColorGrading_ColorGainMidtones = value;
                isDataDirty = true;
            }
        }

        /// <summary>
        /// Gets or sets the color offset (applies to midtones only). Default is 0.
        /// </summary>
        [DefaultValue(typeof(Vector4), "0,0,0,0")]
        [CustomEditorAlias(TrackBallEditorTypename)]
        [NoSerialize, EditorOrder(914), Limit(-1, 1, 0.001f), EditorDisplay("Color Grading", "Midtones Offset"), Tooltip("Color offset (applies to midtones only). Default is 0.")]
        public Vector4 ColorGrading_ColorOffsetMidtones
        {
            get => data.ColorGrading_ColorOffsetMidtones;
            set
            {
                data.ColorGrading_ColorOffsetMidtones = value;
                isDataDirty = true;
            }
        }

        #endregion

        #region Highlights

        /// <summary>
        /// Gets or sets the color saturation (applies to highlights only). Default is 1.
        /// </summary>
        [DefaultValue(typeof(Vector4), "1,1,1,1")]
        [CustomEditorAlias(TrackBallEditorTypename)]
        [NoSerialize, EditorOrder(915), Limit(0, 2, 0.01f), EditorDisplay("Color Grading", "Highlights Saturation"), Tooltip("Color saturation (applies to highlights only). Default is 1.")]
        public Vector4 ColorGrading_ColorSaturationHighlights
        {
            get => data.ColorGrading_ColorSaturationHighlights;
            set
            {
                data.ColorGrading_ColorSaturationHighlights = value;
                isDataDirty = true;
            }
        }

        /// <summary>
        /// Gets or sets the color contrast (applies to highlights only). Default is 1.
        /// </summary>
        [DefaultValue(typeof(Vector4), "1,1,1,1")]
        [CustomEditorAlias(TrackBallEditorTypename)]
        [NoSerialize, EditorOrder(916), Limit(0, 2, 0.01f), EditorDisplay("Color Grading", "Highlights Contrast"), Tooltip("Color contrast (applies to highlights only). Default is 1.")]
        public Vector4 ColorGrading_ColorContrastHighlights
        {
            get => data.ColorGrading_ColorContrastHighlights;
            set
            {
                data.ColorGrading_ColorContrastHighlights = value;
                isDataDirty = true;
            }
        }

        /// <summary>
        /// Gets or sets the color gamma (applies to highlights only). Default is 1.
        /// </summary>
        [DefaultValue(typeof(Vector4), "1,1,1,1")]
        [CustomEditorAlias(TrackBallEditorTypename)]
        [NoSerialize, EditorOrder(917), Limit(0, 2, 0.01f), EditorDisplay("Color Grading", "Highlights Gamma"), Tooltip("Color gamma (applies to highlights only). Default is 1.")]
        public Vector4 ColorGrading_ColorGammaHighlights
        {
            get => data.ColorGrading_ColorGammaHighlights;
            set
            {
                data.ColorGrading_ColorGammaHighlights = value;
                isDataDirty = true;
            }
        }

        /// <summary>
        /// Gets or sets the color gain (applies to highlights only). Default is 1.
        /// </summary>
        [DefaultValue(typeof(Vector4), "1,1,1,1")]
        [CustomEditorAlias(TrackBallEditorTypename)]
        [NoSerialize, EditorOrder(918), Limit(0, 2, 0.01f), EditorDisplay("Color Grading", "Highlights Gain"), Tooltip("Color gain (applies to highlights only). Default is 1.")]
        public Vector4 ColorGrading_ColorGainHighlights
        {
            get => data.ColorGrading_ColorGainHighlights;
            set
            {
                data.ColorGrading_ColorGainHighlights = value;
                isDataDirty = true;
            }
        }

        /// <summary>
        /// Gets or sets the color offset (applies to highlights only). Default is 0.
        /// </summary>
        [DefaultValue(typeof(Vector4), "0,0,0,0")]
        [CustomEditorAlias(TrackBallEditorTypename)]
        [NoSerialize, EditorOrder(919), Limit(-1, 1, 0.001f), EditorDisplay("Color Grading", "Highlights Offset"), Tooltip("Color offset (applies to highlights only). Default is 0.")]
        public Vector4 ColorGrading_ColorOffsetHighlights
        {
            get => data.ColorGrading_ColorOffsetHighlights;
            set
            {
                data.ColorGrading_ColorOffsetHighlights = value;
                isDataDirty = true;
            }
        }

        #endregion

        /// <summary>
        /// Gets or sets the shadows maximum value. Default is 0.09.
        /// </summary>
        [DefaultValue(0.09f), Limit(-1, 1, 0.01f)]
        [NoSerialize, EditorOrder(920), EditorDisplay("Color Grading", "Shadows Max"), Tooltip("Shadows maximum value. Default is 0.09.")]
        public float ColorGrading_ShadowsMax
        {
            get => data.ColorGrading_ShadowsMax;
            set
            {
                data.ColorGrading_ShadowsMax = value;
                isDataDirty = true;
            }
        }

        /// <summary>
        /// Gets or sets the highlights minimum value. Default is 0.5.
        /// </summary>
        [DefaultValue(0.5f), Limit(-1, 1, 0.01f)]
        [NoSerialize, EditorOrder(921), EditorDisplay("Color Grading", "Highlights Min"), Tooltip("Highlights minimum value. Default is 0.5.")]
        public float ColorGrading_HighlightsMin
        {
            get => data.ColorGrading_HighlightsMin;
            set
            {
                data.ColorGrading_HighlightsMin = value;
                isDataDirty = true;
            }
        }

        #endregion

        #region Motion Blur

        /// <summary>
        /// Gets or sets a value indicating whether Motion Blur is enabled.
        /// </summary>
        [DefaultValue(true)]
        [NoSerialize, EditorOrder(1000), EditorDisplay("Motion Blur", "Enabled"), Tooltip("Enable motion blur effect")]
        public bool MB_Enabled
        {
            get => data.MB_Enabled != 0;
            set
            {
                data.MB_Enabled = (byte)(value ? 1 : 0);
                isDataDirty = true;
            }
        }

        /// <summary>
        /// Gets or sets the motion blur effect scale.
        /// </summary>
        [DefaultValue(1.0f), Limit(0, 5, 0.01f)]
        [NoSerialize, EditorOrder(1001), EditorDisplay("Motion Blur", "Scale"), Tooltip("The motion blur effect scale.")]
        public float MB_Scale
        {
            get => data.MB_Scale;
            set
            {
                data.MB_Scale = value;
                isDataDirty = true;
            }
        }

        /// <summary>
        /// Gets or sets the amount of sample points used during motion blur rendering. It affects quality and performances.
        /// </summary>
        [DefaultValue(10), Limit(4, 32, 0.1f)]
        [NoSerialize, EditorOrder(1002), EditorDisplay("Motion Blur", "Sample Count"), Tooltip("The amount of sample points used during motion blur rendering. It affects quality and performances.")]
        public int MB_SampleCount
        {
            get => data.MB_SampleCount;
            set
            {
                data.MB_SampleCount = value;
                isDataDirty = true;
            }
        }

        /// <summary>
        /// Gets or sets the motion vectors texture resolution. Motion blur uses per-pixel motion vectors buffer that contains objects movement information. Use lowe resolution to improve performance.
        /// </summary>
        [DefaultValue(ResolutionMode.Half)]
        [NoSerialize, EditorOrder(1003), EditorDisplay("Motion Blur", "Motion Vectors Resolution"), Tooltip("The motion vectors texture resolution. Motion blur uses per-pixel motion vectors buffer that contains objects movement information. Use lowe resolution to improve performance.")]
        public ResolutionMode MB_MotionVectorsResolution
        {
            get => data.MB_MotionVectorsResolution;
            set
            {
                data.MB_MotionVectorsResolution = value;
                isDataDirty = true;
            }
        }

        #endregion

        #region PostFx Materials

        /// <summary>
        /// Gets the post effect materials collection.
        /// </summary>
        [NoSerialize, EditorOrder(10000), EditorDisplay("PostFx Materials", EditorDisplayAttribute.InlineStyle), Tooltip("Post effect materials to render")]
        public unsafe MaterialBase[] PostFxMaterials
        {
            get
            {
                var result = new MaterialBase[data.PostFxMaterialsCount];
                fixed (Guid* postFxMaterials = &data.PostFxMaterial0)
                {
                    for (int i = 0; i < data.PostFxMaterialsCount; i++)
                    {
                        result[i] = Content.LoadAsync<MaterialBase>(postFxMaterials[i]);
                    }
                }

                return result;
            }
            set
            {
                fixed (Guid* postFxMaterials = &data.PostFxMaterial0)
                {
                    var postFxLength = Mathf.Min(value?.Length ?? 0, MaxPostFxMaterials);
                    bool posFxMaterialsChanged = data.PostFxMaterialsCount != postFxLength;

                    for (int i = 0; i < postFxLength; i++)
                    {
                        // ReSharper disable once PossibleNullReferenceException
                        Guid id = value[i]?.ID ?? Guid.Empty;
                        if (postFxMaterials[i] != id)
                        {
                            posFxMaterialsChanged = true;
                        }

                        postFxMaterials[i] = id;
                    }

                    for (int i = postFxLength; i < MaxPostFxMaterials; i++)
                    {
                        if (postFxMaterials[i] != Guid.Empty)
                        {
                            posFxMaterialsChanged = true;
                        }

                        postFxMaterials[i] = Guid.Empty;
                    }

                    data.PostFxMaterialsCount = postFxLength;
                    if (posFxMaterialsChanged)
                        isDataDirty = true;
                }
            }
        }

        #endregion

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (obj is PostProcessSettings other)
                return data.Equals(other.data);
            return false;
        }

        /// <inheritdoc />
        public bool Equals(PostProcessSettings other)
        {
            return data.Equals(other.data);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return data.GetHashCode();
        }
    }
}
