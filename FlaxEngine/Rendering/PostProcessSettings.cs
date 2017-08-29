////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Runtime.InteropServices;
// ReSharper disable InconsistentNaming

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
    /// Tone mapping techniques.
    /// </summary>
    public enum ToneMappingTechniqe
    {
        /// <summary>
        /// The none.
        /// </summary>
        None = 0,

        /// <summary>
        /// The logarithmic.
        /// </summary>
        Logarithmic = 1,

        /// <summary>
        /// The exponential.
        /// </summary>
        Exponential = 2,

        /// <summary>
        /// The Drago-logarithmic.
        /// </summary>
        DragoLogarithmic = 3,

        /// <summary>
        /// The Reinhard.
        /// </summary>
        Reinhard = 4,

        /// <summary>
        /// The modified Reinhard.
        /// </summary>
        ReinhardModified = 5,

        /// <summary>
        /// The filmic ALU.
        /// </summary>
        FilmicALU = 6
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
    /// Contains settings for rendering advanced visual effects and post effects.
    /// </summary>
    [Serializable]
    public struct PostProcessSettings
    {
        /// <summary>
        /// Packed setings storage container used with C++ interop.
        /// </summary>
        [Serializable, StructLayout(LayoutKind.Sequential)]
        internal struct Data
        {
            // Flags

            // Every property has order eg. 601, 812. We use dozens number as group index and rest as a property index.
            // Bit fields contain flags if override every parameter.

            public int Flags0;//
            public int Flags1;// AO
            public int Flags2;// Bloom
            public int Flags3;// ToneMap
            public int Flags4;// Eye
            public int Flags5;// Cam
            public int Flags6;// Flare
            public int Flags7;// DOF
            public int Flags8;// SSR
            public int Flags9;// 

            // Ambient Occlusion

            public bool AO_Enabled;
            public float AO_Intensity;
            public float AO_Power;
            public float AO_Radius;
            public float AO_FadeOutDistance;

            // Screen Space Reflections

            public bool SSR_Enabled;
            public float SSR_MaxRoughness;

            // Bloom

            public bool Bloom_Enabled;
            public float Bloom_Intensity;
            public float Bloom_Threshold;
            public float Bloom_BlurSigma;
            public float Bloom_Scale;

            // ToneMappingSettings

            public ToneMappingTechniqe ToneMap_Technique;
            public float ToneMap_WhiteLevel;
            public float ToneMap_LuminanceSaturation;
            public float ToneMap_Bias;

            // Eye Adaptation

            public EyeAdaptationTechnique Eye_Technique;
            public float Eye_Speed;
            public float Eye_Exposure;
            public float Eye_KeyValue;
            public float Eye_MinLuminance;
            public float Eye_MaxLuminance;

            // CameraArtifactsSettings

            public float Cam_VignetteIntensity;
            public Vector3 Cam_VignetteColor;
            public float Cam_VignetteShapeFactor;
            public float Cam_GrainAmount;
            public float Cam_GrainParticleSize;
            public Vector3 Cam_ChromaticDistortion;

            // LensFlaresSettings

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

            // DepthOfFieldSettings

            public bool DOF_Enabled;
            public float DOF_FocalDistance;
            public float DOF_FocalRegion;
            public float DOF_NearTransitionRange;
            public float DOF_FarTransitionRange;
            public float DOF_DepthLimit;
            public bool DOF_BokehEnabled;
            public float DOF_BokehSize;
            public BokehShapeType DOF_BokehShape;
            public Guid DOF_BokehShapeCustom;
            public float DOF_BokehBrightnessThreshold;
            public float DOF_BokehBlurThreshold;
            public float DOF_BokehFalloff;
            public float DOF_BokehDepthCutoff;

            // PostFxMaterials

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

        [Serialize]
        internal MaterialBase[] postFxMaterials;

        #region Ambient Occlusion

        /// <summary>
        /// Enable/disable ambient occlusion effect.
        /// </summary>
        [NoSerialize, EditorOrder(100), EditorDisplay("Ambient Occlusion", "Enabled")]
        public bool AO_Enabled
        {
            get => data.AO_Enabled;
            set
            {
                data.AO_Enabled = value;
                isDataDirty = true;
            }
        }

        /// <summary>
        /// Gets or sets the ambient occlusion intensity.
        /// </summary>
        [NoSerialize, EditorOrder(101), EditorDisplay("Ambient Occlusion", "Intensity"), Limit(0, 2.0f, 0.01f)]
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
        /// Gets or sets the ambient occluion power.
        /// </summary>
        [NoSerialize, EditorOrder(102), EditorDisplay("Ambient Occlusion", "Power"), Limit(0, 4.0f, 0.01f)]
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
        [NoSerialize, EditorOrder(103), EditorDisplay("Ambient Occlusion", "Radius"), Limit(0, 16.0f, 0.01f)]
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
        [NoSerialize, EditorOrder(104), EditorDisplay("Ambient Occlusion", "Fade Out Distance"), Limit(0, 1000000.0f)]
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
        [NoSerialize, EditorOrder(200), EditorDisplay("Bloom", "Enabled")]
        public bool Bloom_Enabled
        {
            get => data.Bloom_Enabled;
            set
            {
                data.Bloom_Enabled = value;
                isDataDirty = true;
            }
        }

        /// <summary>
        /// Gets or sets the bloom intensity.
        /// </summary>
        [NoSerialize, EditorOrder(201), EditorDisplay("Bloom", "Intensity"), Limit(0, 10.0f, 0.1f)]
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
        [NoSerialize, EditorOrder(202), EditorDisplay("Bloom", "Threshold"), Limit(0, 15.0f, 0.01f)]
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
        [NoSerialize, EditorOrder(203), EditorDisplay("Bloom", "BlurSigma")]
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
        /// Gets or sets the tone mapping mode.
        /// </summary>
        [NoSerialize, EditorOrder(300), EditorDisplay("Tone Mapping", "Technique")]
        public ToneMappingTechniqe ToneMap_Technique
        {
            get => data.ToneMap_Technique;
            set
            {
                data.ToneMap_Technique = value;
                isDataDirty = true;
            }
        }

        /// <summary>
        /// Gets or sets the tone mapping white level.
        /// </summary>
        [NoSerialize, EditorOrder(301), EditorDisplay("Tone Mapping", "White Level")]
        public float ToneMap_WhiteLevel
        {
            get => data.ToneMap_WhiteLevel;
            set
            {
                data.ToneMap_WhiteLevel = value;
                isDataDirty = true;
            }
        }

        /// <summary>
        /// Gets or sets the tone mapping pixels luminance saturation.
        /// </summary>
        [NoSerialize, EditorOrder(302), EditorDisplay("Tone Mapping", "Luminance Saturation")]
        public float ToneMap_LuminanceSaturation
        {
            get => data.ToneMap_LuminanceSaturation;
            set
            {
                data.ToneMap_LuminanceSaturation = value;
                isDataDirty = true;
            }
        }

        /// <summary>
        /// Gets or sets the tone mapping bias.
        /// </summary>
        [NoSerialize, EditorOrder(303), EditorDisplay("Tone Mapping", "Bias")]
        public float ToneMap_Bias
        {
            get => data.ToneMap_Bias;
            set
            {
                data.ToneMap_Bias = value;
                isDataDirty = true;
            }
        }

        #endregion

        #region Eye Adaptation

        /// <summary>
        /// Gets or sets the eye adaptation mode.
        /// </summary>
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
        /// Gets or sets the speed of the eye adaptation effect.
        /// </summary>
        [NoSerialize, EditorOrder(401), EditorDisplay("Eye Adaptation", "Speed"), Limit(0, 100.0f, 0.1f)]
        public float Eye_Speed
        {
            get => data.Eye_Speed;
            set
            {
                data.Eye_Speed = value;
                isDataDirty = true;
            }
        }

        /// <summary>
        /// Gets or sets the camera exposure.
        /// </summary>
        [NoSerialize, EditorOrder(402), EditorDisplay("Eye Adaptation", "Exposure")]
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
        [NoSerialize, EditorOrder(403), EditorDisplay("Eye Adaptation", "Key Value")]
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
        [NoSerialize, EditorOrder(404), EditorDisplay("Eye Adaptation", "Minimum luminance"), Limit(0, 10.0f, 0.001f)]
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
        [NoSerialize, EditorOrder(405), EditorDisplay("Eye Adaptation", "Maximum luminance"), Limit(0, 100.0f, 0.001f)]
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
        [NoSerialize, EditorOrder(500), EditorDisplay("Camera Artifacts", "Vignette Intensity"), Limit(0, 2, 0.001f)]
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
        [NoSerialize, EditorOrder(502), EditorDisplay("Camera Artifacts", "Vignette Shape Factor"), Limit(0.0001f, 2.0f, 0.001f)]
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
        [NoSerialize, EditorOrder(503), EditorDisplay("Camera Artifacts", "Grain Amount"), Limit(0.0f, 2.0f, 0.005f)]
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
        [NoSerialize, EditorOrder(504), EditorDisplay("Camera Artifacts", "Grain Particle Size"), Limit(1.0f, 3.0f, 0.01f)]
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
        /// Gets or sets the chromatic aberration distortion (per channel).
        /// </summary>
        [NoSerialize, EditorOrder(505), EditorDisplay("Camera Artifacts", "Chromatic Distortion"), Limit(-30.0f, 30.0f, 0.1f)]
        public Vector3 Cam_ChromaticDistortion
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
        [NoSerialize, EditorOrder(600), EditorDisplay("Lens Flares", "Intensity"), Limit(0, 2.0f, 0.01f)]
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
        [NoSerialize, EditorOrder(601), EditorDisplay("Lens Flares", "Ghosts"), Limit(0, 16)]
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
        [NoSerialize, EditorOrder(603), EditorDisplay("Lens Flares", "Halo Intensity"), Limit(0, 2.0f, 0.01f)]
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
        /// Gets or sets the camera lens color lookup texture.
        /// </summary>
        [NoSerialize, EditorOrder(609), EditorDisplay("Lens Flares", "Lens Color"), Tooltip("Custom texture for lens flares color")]
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
        [NoSerialize, EditorOrder(610), EditorDisplay("Lens Flares", "Lens Star"), Tooltip("Custom texture for lens flares star")]
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
        [NoSerialize, EditorOrder(700), EditorDisplay("Depth of Field", "Enabled"), Tooltip("Enable depth of field effect")]
        public bool DOF_Enabled
        {
            get => data.DOF_Enabled;
            set
            {
                data.DOF_Enabled = value;
                isDataDirty = true;
            }
        }

        /// <summary>
        /// Gets or sets the distance in World Units from the camera that acts as the center of the region where the scene is perfectly in focus and no blurring occurs.
        /// </summary>
        [NoSerialize, EditorOrder(701), EditorDisplay("Depth of Field", "Focal Distance"), Tooltip("The distance in World Units from the camera that acts as the center of the region where the scene is perfectly in focus and no blurring occurs"), Limit(0, 100000.0f)]
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
        [NoSerialize, EditorOrder(702), EditorDisplay("Depth of Field", "Focal Region"), Tooltip("The distance in World Units beyond the focal distance where the scene is perfectly in focus and no blurring occurs"), Limit(0, 100000.0f)]
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
        [NoSerialize, EditorOrder(703), EditorDisplay("Depth of Field", "Near Transition Range"), Tooltip("The distance in World Units from the focal region on the side nearer to the camera over which the scene transitions from focused to blurred"), Limit(0, 500.0f)]
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
        [NoSerialize, EditorOrder(704), EditorDisplay("Depth of Field", "Far Transition Range"), Tooltip("The distance in World Units from the focal region on the side farther from the camera over which the scene transitions from focused to blurred"), Limit(0, 1000.0f)]
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
        /// Gets or sets the distance in World Units which describes border after that there is no blur (usefull to disable DoF on sky).
        /// </summary>
        [NoSerialize, EditorOrder(705), EditorDisplay("Depth of Field", "Depth Limit"), Tooltip("The distance in World Units which describes border after that there is no blur (usefull to disable DoF on sky)"), Limit(50, 1000000.0f, 2)]
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
        [NoSerialize, EditorOrder(706), EditorDisplay("Depth of Field", "Bokeh Enable"), Tooltip("Enables/disables generating Bokeh shapes")]
        public bool DOF_BokehEnabled
        {
            get => data.DOF_BokehEnabled;
            set
            {
                data.DOF_BokehEnabled = value;
                isDataDirty = true;
            }
        }

        /// <summary>
        /// Controls Bokeh shapes maximum size.
        /// </summary>
        [NoSerialize, EditorOrder(707), EditorDisplay("Depth of Field", "Bokeh Size"), Tooltip("Controls Bokeh shapes maximum size"), Limit(0, 100.0f, 0.1f)]
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
        [NoSerialize, EditorOrder(708), EditorDisplay("Depth of Field", "Bokeh Shape"), Tooltip("Bokeh shapes style")]
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
        [NoSerialize, EditorOrder(709), EditorDisplay("Depth of Field", "Bokeh Shape Custom Texture"), Tooltip("Custom texture for bokeh shapes")]
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
        [NoSerialize, EditorOrder(710), EditorDisplay("Depth of Field", "Bokeh Brightness Threshold"), Tooltip("Controls Bokeh shapes generating minimum pixel brightness to appear"), Limit(0, 10.0f, 0.01f)]
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
        /// Controls Bokeh shapes blur threashold.
        /// </summary>
        [NoSerialize, EditorOrder(711), EditorDisplay("Depth of Field", "Bokeh Blur Threshold"), Tooltip("Controls Bokeh shapes blur threashold"), Limit(0, 1.0f, 0.001f)]
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
        /// Controls Bokeh shapes brightness fallouff parameter.
        /// </summary>
        [NoSerialize, EditorOrder(712), EditorDisplay("Depth of Field", "Bokeh Falloff"), Tooltip("Controls Bokeh shapes brightness fallouff parameter"), Limit(0, 2.0f, 0.001f)]
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
        [NoSerialize, EditorOrder(713), EditorDisplay("Depth of Field", "Bokeh Depth Cutoff"), Tooltip("Controls Bokeh shapes depth cutoff parameter"), Limit(0, 5.0f, 0.001f)]
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
        /// Enables or disables rendering of Screen Space Reflections.
        /// </summary>
        [NoSerialize, EditorOrder(800), EditorDisplay("Screen Space Reflections", "Enabled"), Tooltip("Enables or disables rendering Screen Space Reflections effect on camera")]
        public bool SSR_Enabled
        {
            get => data.SSR_Enabled;
            set
            {
                data.SSR_Enabled = value;
                isDataDirty = true;
            }
        }

        /// <summary>
        /// Gets or sets the maximum surface roughness to calculate screen space reflections for it.
        /// </summary>
        [NoSerialize, EditorOrder(801), EditorDisplay("Screen Space Reflections", "Max Roughness"), Tooltip("Maximum surface roughness values that can accept Screen Space Reflections"), Limit(0, 1.0f, 0.0001f)]
        public float SSR_MaxRoughness
        {
            get => data.SSR_MaxRoughness;
            set
            {
                data.SSR_MaxRoughness = value;
                isDataDirty = true;
            }
        }

        #endregion

        #region PostFx Materials

        /// <summary>
        /// Gets the post effect materials collection.
        /// </summary>
        [NoSerialize, EditorOrder(900), EditorDisplay("PostFx Materials", "__inline__"), Tooltip("Post effect materials to render")]
        public MaterialBase[] PostFxMaterials
        {
            get => postFxMaterials;
            set => postFxMaterials = value;
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
            // ReSharper disable once NonReadonlyMemberInGetHashCode
            return data.GetHashCode();
        }
    }
}
