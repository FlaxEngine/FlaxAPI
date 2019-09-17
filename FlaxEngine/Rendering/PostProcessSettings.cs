// Copyright (c) 2012-2019 Wojciech Figat. All rights reserved.

using System;
using System.ComponentModel;
using System.Runtime.InteropServices;

// ReSharper disable UnusedMember.Global
// ReSharper disable EnumUnderlyingTypeIsInt
// ReSharper disable ShiftExpressionRealShiftCountIsZero

namespace FlaxEngine.Rendering
{
    /// <summary>
    /// Tone mapping effect rendering modes.
    /// </summary>
    public enum ToneMappingMode
    {
        /// <summary>
        /// Disabled tone mapping effect.
        /// </summary>
        None = 0,

        /// <summary>
        /// The neutral tonemapper.
        /// </summary>
        Neutral = 1,

        /// <summary>
        /// The ACES Filmic reference tonemapper (approximation).
        /// </summary>
        ACES = 2,
    }

    /// <summary>
    /// Eye adaptation effect rendering modes.
    /// </summary>
    public enum EyeAdaptationMode
    {
        /// <summary>
        /// Disabled eye adaptation effect.
        /// </summary>
        None = 0,

        /// <summary>
        /// The manual mode that uses a fixed exposure values.
        /// </summary>
        Manual = 1,

        /// <summary>
        /// The automatic mode applies the eye adaptation exposure based on the scene color luminance blending using the histogram. Requires compute shader support.
        /// </summary>
        AutomaticHistogram = 2,

        /// <summary>
        /// The automatic mode applies the eye adaptation exposure based on the scene color luminance blending using the average luminance.
        /// </summary>
        AutomaticAverageLuminance = 3,
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
    /// Anti-aliasing modes.
    /// </summary>
    public enum AntialiasingMode
    {
        /// <summary>
        /// The none.
        /// </summary>
        None = 0,

        /// <summary>
        /// Fast-Approximate Anti-Aliasing effect.
        /// </summary>
        FastApproximateAntialiasing = 1,

        /// <summary>
        /// Temporal Anti-Aliasing effect.
        /// </summary>
        TemporalAntialiasing = 2,

        /// <summary>
        /// Subpixel Morphological Anti-Aliasing effect.
        /// </summary>
        SubpixelMorphologicalAntialiasing = 3,
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

    internal class PostProcessSettingAttribute : Attribute
    {
        public int Bit;

        public PostProcessSettingAttribute(int bit)
        {
            Bit = bit;
        }
    }

    /// <summary>
    /// Contains settings for Ambient Occlusion effect rendering.
    /// </summary>
    [Serializable, StructLayout(LayoutKind.Sequential)]
    public struct AmbientOcclusionSettings
    {
        /// <summary>
        /// The structure members override flags.
        /// </summary>
        [Flags]
        public enum Override : int
        {
            /// <summary>
            /// None properties.
            /// </summary>
            None = 0,

            /// <summary>
            /// Overrides <see cref="AmbientOcclusionSettings.Enabled"/> property.
            /// </summary>
            Enabled = 1 << 0,

            /// <summary>
            /// Overrides <see cref="AmbientOcclusionSettings.Intensity"/> property.
            /// </summary>
            Intensity = 1 << 1,

            /// <summary>
            /// Overrides <see cref="AmbientOcclusionSettings.Power"/> property.
            /// </summary>
            Power = 1 << 2,

            /// <summary>
            /// Overrides <see cref="AmbientOcclusionSettings.Radius"/> property.
            /// </summary>
            Radius = 1 << 3,

            /// <summary>
            /// Overrides <see cref="AmbientOcclusionSettings.FadeOutDistance"/> property.
            /// </summary>
            FadeOutDistance = 1 << 4,

            /// <summary>
            /// Overrides <see cref="AmbientOcclusionSettings.FadeDistance"/> property.
            /// </summary>
            FadeDistance = 1 << 5,

            /// <summary>
            /// All properties.
            /// </summary>
            All = Enabled | Intensity | Power | Radius | FadeOutDistance | FadeDistance,
        };

        /// <summary>
        /// The flags for overriden properties.
        /// </summary>
        [HideInEditor]
        public Override OverrideFlags;

        /// <summary>
        /// Enable/disable ambient occlusion effect.
        /// </summary>
        [DefaultValue(true)]
        [EditorOrder(0), PostProcessSetting((int)Override.Enabled)]
        public bool Enabled;

        /// <summary>
        /// Gets or sets the ambient occlusion intensity.
        /// </summary>
        [DefaultValue(0.6f), Limit(0, 2.0f, 0.01f)]
        [EditorOrder(1), PostProcessSetting((int)Override.Intensity)]
        public float Intensity;

        /// <summary>
        /// Gets or sets the ambient occlusion power.
        /// </summary>
        [DefaultValue(0.75f), Limit(0, 4.0f, 0.01f)]
        [EditorOrder(2), PostProcessSetting((int)Override.Power)]
        public float Power;

        /// <summary>
        /// Gets or sets the ambient occlusion check range radius.
        /// </summary>
        [DefaultValue(0.7f), Limit(0, 100.0f, 0.01f)]
        [EditorOrder(3), PostProcessSetting((int)Override.Radius)]
        public float Radius;

        /// <summary>
        /// Gets or sets the ambient occlusion fade out end distance from camera (in world units).
        /// </summary>
        [DefaultValue(5000.0f), Limit(0.0f)]
        [EditorOrder(4), PostProcessSetting((int)Override.FadeOutDistance)]
        public float FadeOutDistance;

        /// <summary>
        /// Gets or sets the ambient occlusion fade distance (in world units). Defines the size of the effect fade from fully visible to fully invisible at FadeOutDistance.
        /// </summary>
        [DefaultValue(500.0f), Limit(0.0f)]
        [EditorOrder(5), PostProcessSetting((int)Override.FadeDistance)]
        public float FadeDistance;
    }

    /// <summary>
    /// Contains settings for Bloom effect rendering.
    /// </summary>
    [Serializable, StructLayout(LayoutKind.Sequential)]
    public struct BloomSettings
    {
        /// <summary>
        /// The structure members override flags.
        /// </summary>
        [Flags]
        public enum Override : int
        {
            /// <summary>
            /// None properties.
            /// </summary>
            None = 0,

            /// <summary>
            /// Overrides <see cref="BloomSettings.Enabled"/> property.
            /// </summary>
            Enabled = 1 << 0,

            /// <summary>
            /// Overrides <see cref="BloomSettings.Intensity"/> property.
            /// </summary>
            Intensity = 1 << 1,

            /// <summary>
            /// Overrides <see cref="BloomSettings.Threshold"/> property.
            /// </summary>
            Threshold = 1 << 2,

            /// <summary>
            /// Overrides <see cref="BloomSettings.BlurSigma"/> property.
            /// </summary>
            BlurSigma = 1 << 3,

            /// <summary>
            /// Overrides <see cref="BloomSettings.Limit"/> property.
            /// </summary>
            Limit = 1 << 4,

            /// <summary>
            /// All properties.
            /// </summary>
            All = Enabled | Intensity | Threshold | BlurSigma | Limit,
        };

        /// <summary>
        /// The flags for overriden properties.
        /// </summary>
        [HideInEditor]
        public Override OverrideFlags;

        /// <summary>
        /// Enables/disables bloom effect.
        /// </summary>
        [DefaultValue(true)]
        [EditorOrder(0), PostProcessSetting((int)Override.Enabled)]
        public bool Enabled;

        /// <summary>
        /// Gets or sets the bloom intensity.
        /// </summary>
        [DefaultValue(1.0f), Limit(0, 20.0f, 0.01f)]
        [EditorOrder(1), PostProcessSetting((int)Override.Intensity)]
        public float Intensity;

        /// <summary>
        /// Gets or sets the bloom threshold. Pixels with higher luminance are glowing.
        /// </summary>
        [DefaultValue(3.0f), Limit(0, 15.0f, 0.01f)]
        [EditorOrder(2), PostProcessSetting((int)Override.Threshold)]
        public float Threshold;

        /// <summary>
        /// Gets or sets the bloom blur sigma parameter.
        /// </summary>
        [DefaultValue(4.0f), Limit(0, 20.0f, 0.01f)]
        [EditorOrder(3), PostProcessSetting((int)Override.BlurSigma)]
        public float BlurSigma;

        /// <summary>
        /// Gets or sets the bloom brightness limit. Pixels with higher luminance will be capped to this brightness level.
        /// </summary>
        [DefaultValue(10.0f), Limit(0, 100.0f, 0.01f)]
        [EditorOrder(4), PostProcessSetting((int)Override.Limit)]
        public float Limit;
    }

    /// <summary>
    /// Contains settings for Tone Mapping effect rendering.
    /// </summary>
    [Serializable, StructLayout(LayoutKind.Sequential)]
    public struct ToneMappingSettings
    {
        /// <summary>
        /// The structure members override flags.
        /// </summary>
        [Flags]
        public enum Override : int
        {
            /// <summary>
            /// None properties.
            /// </summary>
            None = 0,

            /// <summary>
            /// Overrides <see cref="ToneMappingSettings.WhiteTemperature"/> property.
            /// </summary>
            WhiteTemperature = 1 << 0,

            /// <summary>
            /// Overrides <see cref="ToneMappingSettings.WhiteTint"/> property.
            /// </summary>
            WhiteTint = 1 << 1,

            /// <summary>
            /// Overrides <see cref="ToneMappingSettings.Mode"/> property.
            /// </summary>
            Mode = 1 << 2,

            /// <summary>
            /// All properties.
            /// </summary>
            All = WhiteTemperature | WhiteTint | Mode,
        };

        /// <summary>
        /// The flags for overriden properties.
        /// </summary>
        [HideInEditor]
        public Override OverrideFlags;

        /// <summary>
        /// Gets or sets the white color temperature. Default is 6500.
        /// </summary>
        [DefaultValue(6500.0f), Limit(1500, 15000)]
        [EditorOrder(0), PostProcessSetting((int)Override.WhiteTemperature)]
        [Tooltip("White color temperature. Default is 6500.")]
        public float WhiteTemperature;

        /// <summary>
        /// Gets or sets the white tint. Default is 0.
        /// </summary>
        [DefaultValue(0.0f), Limit(-1, 1, 0.001f)]
        [EditorOrder(1), PostProcessSetting((int)Override.WhiteTint)]
        [Tooltip("White color tint. Default is 0.")]
        public float WhiteTint;

        /// <summary>
        /// The tone mapping mode to use for the color grading process.
        /// </summary>
        [DefaultValue(ToneMappingMode.ACES)]
        [Tooltip("The tone mapping mode to use for the color grading process.")]
        [EditorOrder(2), PostProcessSetting((int)Override.Mode)]
        public ToneMappingMode Mode;
    }

    /// <summary>
    /// Contains settings for Color Grading effect rendering.
    /// </summary>
    [Serializable, StructLayout(LayoutKind.Sequential)]
    public struct ColorGradingSettings
    {
        /// <summary>
        /// The structure members override flags.
        /// </summary>
        [Flags]
        public enum Override : int
        {
            /// <summary>
            /// None properties.
            /// </summary>
            None = 0,

            /// <summary>
            /// Overrides <see cref="ColorGradingSettings.ColorSaturation"/> property.
            /// </summary>
            ColorSaturation = 1 << 0,

            /// <summary>
            /// Overrides <see cref="ColorGradingSettings.ColorContrast"/> property.
            /// </summary>
            ColorContrast = 1 << 1,

            /// <summary>
            /// Overrides <see cref="ColorGradingSettings.ColorGamma"/> property.
            /// </summary>
            ColorGamma = 1 << 2,

            /// <summary>
            /// Overrides <see cref="ColorGradingSettings.ColorGain"/> property.
            /// </summary>
            ColorGain = 1 << 3,

            /// <summary>
            /// Overrides <see cref="ColorGradingSettings.ColorOffset"/> property.
            /// </summary>
            ColorOffset = 1 << 4,

            /// <summary>
            /// Overrides <see cref="ColorGradingSettings.ColorSaturationShadows"/> property.
            /// </summary>
            ColorSaturationShadows = 1 << 5,

            /// <summary>
            /// Overrides <see cref="ColorGradingSettings.ColorContrastShadows"/> property.
            /// </summary>
            ColorContrastShadows = 1 << 6,

            /// <summary>
            /// Overrides <see cref="ColorGradingSettings.ColorGammaShadows"/> property.
            /// </summary>
            ColorGammaShadows = 1 << 7,

            /// <summary>
            /// Overrides <see cref="ColorGradingSettings.ColorGainShadows"/> property.
            /// </summary>
            ColorGainShadows = 1 << 8,

            /// <summary>
            /// Overrides <see cref="ColorGradingSettings.ColorOffsetShadows"/> property.
            /// </summary>
            ColorOffsetShadows = 1 << 9,

            /// <summary>
            /// Overrides <see cref="ColorGradingSettings.ColorSaturationMidtones"/> property.
            /// </summary>
            ColorSaturationMidtones = 1 << 10,

            /// <summary>
            /// Overrides <see cref="ColorGradingSettings.ColorContrastMidtones"/> property.
            /// </summary>
            ColorContrastMidtones = 1 << 11,

            /// <summary>
            /// Overrides <see cref="ColorGradingSettings.ColorGammaMidtones"/> property.
            /// </summary>
            ColorGammaMidtones = 1 << 12,

            /// <summary>
            /// Overrides <see cref="ColorGradingSettings.ColorGainMidtones"/> property.
            /// </summary>
            ColorGainMidtones = 1 << 13,

            /// <summary>
            /// Overrides <see cref="ColorGradingSettings.ColorOffsetMidtones"/> property.
            /// </summary>
            ColorOffsetMidtones = 1 << 14,

            /// <summary>
            /// Overrides <see cref="ColorGradingSettings.ColorSaturationHighlights"/> property.
            /// </summary>
            ColorSaturationHighlights = 1 << 15,

            /// <summary>
            /// Overrides <see cref="ColorGradingSettings.ColorContrastHighlights"/> property.
            /// </summary>
            ColorContrastHighlights = 1 << 16,

            /// <summary>
            /// Overrides <see cref="ColorGradingSettings.ColorGammaHighlights"/> property.
            /// </summary>
            ColorGammaHighlights = 1 << 17,

            /// <summary>
            /// Overrides <see cref="ColorGradingSettings.ColorGainHighlights"/> property.
            /// </summary>
            ColorGainHighlights = 1 << 18,

            /// <summary>
            /// Overrides <see cref="ColorGradingSettings.ColorOffsetHighlights"/> property.
            /// </summary>
            ColorOffsetHighlights = 1 << 19,

            /// <summary>
            /// Overrides <see cref="ColorGradingSettings.ShadowsMax"/> property.
            /// </summary>
            ShadowsMax = 1 << 20,

            /// <summary>
            /// Overrides <see cref="ColorGradingSettings.HighlightsMin"/> property.
            /// </summary>
            HighlightsMin = 1 << 21,

            /// <summary>
            /// All properties.
            /// </summary>
            All = ColorSaturation | ColorContrast | ColorGamma | ColorGain | ColorOffset |
                  ColorSaturationShadows | ColorContrastShadows | ColorGammaShadows | ColorGainShadows | ColorOffsetShadows |
                  ColorSaturationMidtones | ColorContrastMidtones | ColorGammaMidtones | ColorGainMidtones | ColorOffsetMidtones |
                  ColorSaturationHighlights | ColorContrastHighlights | ColorGammaHighlights | ColorGainHighlights | ColorOffsetHighlights |
                  ShadowsMax | HighlightsMin,
        };

        /// <summary>
        /// The flags for overriden properties.
        /// </summary>
        [HideInEditor]
        public Override OverrideFlags;

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
        [EditorOrder(0), PostProcessSetting((int)Override.ColorSaturation)]
        [Limit(0, 2, 0.01f), EditorDisplay("Global", "Saturation")]
        [Tooltip("Color saturation (applies globally to the whole image). Default is 1.")]
        public Vector4 ColorSaturation;

        /// <summary>
        /// Gets or sets the color contrast (applies globally to the whole image). Default is 1.
        /// </summary>
        [DefaultValue(typeof(Vector4), "1,1,1,1")]
        [CustomEditorAlias(TrackBallEditorTypename)]
        [EditorOrder(1), PostProcessSetting((int)Override.ColorContrast)]
        [Limit(0, 2, 0.01f), EditorDisplay("Global", "Contrast")]
        [Tooltip("Color contrast (applies globally to the whole image). Default is 1.")]
        public Vector4 ColorContrast;

        /// <summary>
        /// Gets or sets the color gamma (applies globally to the whole image). Default is 1.
        /// </summary>
        [DefaultValue(typeof(Vector4), "1,1,1,1")]
        [CustomEditorAlias(TrackBallEditorTypename)]
        [EditorOrder(2), PostProcessSetting((int)Override.ColorGamma)]
        [Limit(0, 2, 0.01f), EditorDisplay("Global", "Gamma")]
        [Tooltip("Color gamma (applies globally to the whole image). Default is 1.")]
        public Vector4 ColorGamma;

        /// <summary>
        /// Gets or sets the color gain (applies globally to the whole image). Default is 1.
        /// </summary>
        [DefaultValue(typeof(Vector4), "1,1,1,1")]
        [CustomEditorAlias(TrackBallEditorTypename)]
        [EditorOrder(3), PostProcessSetting((int)Override.ColorGain)]
        [Limit(0, 2, 0.01f), EditorDisplay("Global", "Gain")]
        [Tooltip("Color gain (applies globally to the whole image). Default is 1.")]
        public Vector4 ColorGain;

        /// <summary>
        /// Gets or sets the color offset (applies globally to the whole image). Default is 0.
        /// </summary>
        [DefaultValue(typeof(Vector4), "0,0,0,0")]
        [CustomEditorAlias(TrackBallEditorTypename)]
        [EditorOrder(4), PostProcessSetting((int)Override.ColorOffset)]
        [Limit(-1, 1, 0.001f), EditorDisplay("Global", "Offset")]
        [Tooltip("Color offset (applies globally to the whole image). Default is 0.")]
        public Vector4 ColorOffset;

        #endregion

        #region Shadows

        /// <summary>
        /// Gets or sets the color saturation (applies to shadows only). Default is 1.
        /// </summary>
        [DefaultValue(typeof(Vector4), "1,1,1,1")]
        [CustomEditorAlias(TrackBallEditorTypename)]
        [EditorOrder(5), PostProcessSetting((int)Override.ColorSaturationShadows)]
        [Limit(0, 2, 0.01f), EditorDisplay("Shadows", "Shadows Saturation")]
        [Tooltip("Color saturation (applies to shadows only). Default is 1.")]
        public Vector4 ColorSaturationShadows;

        /// <summary>
        /// Gets or sets the color contrast (applies to shadows only). Default is 1.
        /// </summary>
        [DefaultValue(typeof(Vector4), "1,1,1,1")]
        [CustomEditorAlias(TrackBallEditorTypename)]
        [EditorOrder(6), PostProcessSetting((int)Override.ColorContrastShadows)]
        [Limit(0, 2, 0.01f), EditorDisplay("Shadows", "Shadows Contrast")]
        [Tooltip("Color contrast (applies to shadows only). Default is 1.")]
        public Vector4 ColorContrastShadows;

        /// <summary>
        /// Gets or sets the color gamma (applies to shadows only). Default is 1.
        /// </summary>
        [DefaultValue(typeof(Vector4), "1,1,1,1")]
        [CustomEditorAlias(TrackBallEditorTypename)]
        [EditorOrder(7), PostProcessSetting((int)Override.ColorGammaShadows)]
        [Limit(0, 2, 0.01f), EditorDisplay("Shadows", "Shadows Gamma")]
        [Tooltip("Color gamma (applies to shadows only). Default is 1.")]
        public Vector4 ColorGammaShadows;

        /// <summary>
        /// Gets or sets the color gain (applies to shadows only). Default is 1.
        /// </summary>
        [DefaultValue(typeof(Vector4), "1,1,1,1")]
        [CustomEditorAlias(TrackBallEditorTypename)]
        [EditorOrder(8), PostProcessSetting((int)Override.ColorGainShadows)]
        [Limit(0, 2, 0.01f), EditorDisplay("Shadows", "Shadows Gain")]
        [Tooltip("Color gain (applies to shadows only). Default is 1.")]
        public Vector4 ColorGainShadows;

        /// <summary>
        /// Gets or sets the color offset (applies to shadows only). Default is 0.
        /// </summary>
        [DefaultValue(typeof(Vector4), "0,0,0,0")]
        [CustomEditorAlias(TrackBallEditorTypename)]
        [EditorOrder(9), PostProcessSetting((int)Override.ColorOffsetShadows)]
        [Limit(-1, 1, 0.001f), EditorDisplay("Shadows", "Shadows Offset")]
        [Tooltip("Color offset (applies to shadows only). Default is 0.")]
        public Vector4 ColorOffsetShadows;

        #endregion

        #region Midtones

        /// <summary>
        /// Gets or sets the color saturation (applies to midtones only). Default is 1.
        /// </summary>
        [DefaultValue(typeof(Vector4), "1,1,1,1")]
        [CustomEditorAlias(TrackBallEditorTypename)]
        [EditorOrder(10), PostProcessSetting((int)Override.ColorSaturationMidtones)]
        [Limit(0, 2, 0.01f), EditorDisplay("Midtones", "Midtones Saturation")]
        [Tooltip("Color saturation (applies to midtones only). Default is 1.")]
        public Vector4 ColorSaturationMidtones;

        /// <summary>
        /// Gets or sets the color contrast (applies to midtones only). Default is 1.
        /// </summary>
        [DefaultValue(typeof(Vector4), "1,1,1,1")]
        [CustomEditorAlias(TrackBallEditorTypename)]
        [EditorOrder(11), PostProcessSetting((int)Override.ColorContrastMidtones)]
        [Limit(0, 2, 0.01f), EditorDisplay("Midtones", "Midtones Contrast")]
        [Tooltip("Color contrast (applies to midtones only). Default is 1.")]
        public Vector4 ColorContrastMidtones;

        /// <summary>
        /// Gets or sets the color gamma (applies to midtones only). Default is 1.
        /// </summary>
        [DefaultValue(typeof(Vector4), "1,1,1,1")]
        [CustomEditorAlias(TrackBallEditorTypename)]
        [EditorOrder(12), PostProcessSetting((int)Override.ColorGammaMidtones)]
        [Limit(0, 2, 0.01f), EditorDisplay("Midtones", "Midtones Gamma")]
        [Tooltip("Color gamma (applies to midtones only). Default is 1.")]
        public Vector4 ColorGammaMidtones;

        /// <summary>
        /// Gets or sets the color gain (applies to midtones only). Default is 1.
        /// </summary>
        [DefaultValue(typeof(Vector4), "1,1,1,1")]
        [CustomEditorAlias(TrackBallEditorTypename)]
        [EditorOrder(13), PostProcessSetting((int)Override.ColorGainMidtones)]
        [Limit(0, 2, 0.01f), EditorDisplay("Midtones", "Midtones Gain")]
        [Tooltip("Color gain (applies to midtones only). Default is 1.")]
        public Vector4 ColorGainMidtones;

        /// <summary>
        /// Gets or sets the color offset (applies to midtones only). Default is 0.
        /// </summary>
        [DefaultValue(typeof(Vector4), "0,0,0,0")]
        [CustomEditorAlias(TrackBallEditorTypename)]
        [EditorOrder(14), PostProcessSetting((int)Override.ColorOffsetMidtones)]
        [Limit(-1, 1, 0.001f), EditorDisplay("Midtones", "Midtones Offset")]
        [Tooltip("Color offset (applies to midtones only). Default is 0.")]
        public Vector4 ColorOffsetMidtones;

        #endregion

        #region Highlights

        /// <summary>
        /// Gets or sets the color saturation (applies to highlights only). Default is 1.
        /// </summary>
        [DefaultValue(typeof(Vector4), "1,1,1,1")]
        [CustomEditorAlias(TrackBallEditorTypename)]
        [EditorOrder(15), PostProcessSetting((int)Override.ColorSaturationHighlights)]
        [Limit(0, 2, 0.01f), EditorDisplay("Highlights", "Highlights Saturation")]
        [Tooltip("Color saturation (applies to highlights only). Default is 1.")]
        public Vector4 ColorSaturationHighlights;

        /// <summary>
        /// Gets or sets the color contrast (applies to highlights only). Default is 1.
        /// </summary>
        [DefaultValue(typeof(Vector4), "1,1,1,1")]
        [CustomEditorAlias(TrackBallEditorTypename)]
        [EditorOrder(16), PostProcessSetting((int)Override.ColorContrastHighlights)]
        [Limit(0, 2, 0.01f), EditorDisplay("Highlights", "Highlights Contrast")]
        [Tooltip("Color contrast (applies to highlights only). Default is 1.")]
        public Vector4 ColorContrastHighlights;

        /// <summary>
        /// Gets or sets the color gamma (applies to highlights only). Default is 1.
        /// </summary>
        [DefaultValue(typeof(Vector4), "1,1,1,1")]
        [CustomEditorAlias(TrackBallEditorTypename)]
        [EditorOrder(17), PostProcessSetting((int)Override.ColorGammaHighlights)]
        [Limit(0, 2, 0.01f), EditorDisplay("Highlights", "Highlights Gamma")]
        [Tooltip("Color gamma (applies to highlights only). Default is 1.")]
        public Vector4 ColorGammaHighlights;

        /// <summary>
        /// Gets or sets the color gain (applies to highlights only). Default is 1.
        /// </summary>
        [DefaultValue(typeof(Vector4), "1,1,1,1")]
        [CustomEditorAlias(TrackBallEditorTypename)]
        [EditorOrder(18), PostProcessSetting((int)Override.ColorGainHighlights)]
        [Limit(0, 2, 0.01f), EditorDisplay("Highlights", "Highlights Gain")]
        [Tooltip("Color gain (applies to highlights only). Default is 1.")]
        public Vector4 ColorGainHighlights;

        /// <summary>
        /// Gets or sets the color offset (applies to highlights only). Default is 0.
        /// </summary>
        [DefaultValue(typeof(Vector4), "0,0,0,0")]
        [CustomEditorAlias(TrackBallEditorTypename)]
        [EditorOrder(19), PostProcessSetting((int)Override.ColorOffsetHighlights)]
        [Limit(-1, 1, 0.001f), EditorDisplay("Highlights", "Highlights Offset")]
        [Tooltip("Color offset (applies to highlights only). Default is 0.")]
        public Vector4 ColorOffsetHighlights;

        #endregion

        /// <summary>
        /// Gets or sets the shadows maximum value. Default is 0.09.
        /// </summary>
        [DefaultValue(0.09f), Limit(-1, 1, 0.01f)]
        [EditorOrder(20), PostProcessSetting((int)Override.ShadowsMax)]
        [Tooltip("Shadows maximum value. Default is 0.09.")]
        public float ShadowsMax;

        /// <summary>
        /// Gets or sets the highlights minimum value. Default is 0.5.
        /// </summary>
        [DefaultValue(0.5f), Limit(-1, 1, 0.01f)]
        [EditorOrder(21), PostProcessSetting((int)Override.HighlightsMin)]
        [Tooltip("Highlights minimum value. Default is 0.5.")]
        public float HighlightsMin;
    }

    /// <summary>
    /// Contains settings for Eye Adaptation effect rendering.
    /// </summary>
    [Serializable, StructLayout(LayoutKind.Sequential)]
    public struct EyeAdaptationSettings
    {
        /// <summary>
        /// The structure members override flags.
        /// </summary>
        [Flags]
        public enum Override : int
        {
            /// <summary>
            /// None properties.
            /// </summary>
            None = 0,

            /// <summary>
            /// Overrides <see cref="EyeAdaptationSettings.Mode"/> property.
            /// </summary>
            Mode = 1 << 0,

            /// <summary>
            /// Overrides <see cref="EyeAdaptationSettings.SpeedUp"/> property.
            /// </summary>
            SpeedUp = 1 << 1,

            /// <summary>
            /// Overrides <see cref="EyeAdaptationSettings.SpeedDown"/> property.
            /// </summary>
            SpeedDown = 1 << 2,

            /// <summary>
            /// Overrides <see cref="EyeAdaptationSettings.PreExposure"/> property.
            /// </summary>
            PreExposure = 1 << 3,

            /// <summary>
            /// Overrides <see cref="EyeAdaptationSettings.PostExposure"/> property.
            /// </summary>
            PostExposure = 1 << 4,

            /// <summary>
            /// Overrides <see cref="EyeAdaptationSettings.MinBrightness"/> property.
            /// </summary>
            MinBrightness = 1 << 5,

            /// <summary>
            /// Overrides <see cref="EyeAdaptationSettings.MaxBrightness"/> property.
            /// </summary>
            MaxBrightness = 1 << 6,

            /// <summary>
            /// Overrides <see cref="EyeAdaptationSettings.HistogramLowPercent"/> property.
            /// </summary>
            HistogramLowPercent = 1 << 7,

            /// <summary>
            /// Overrides <see cref="EyeAdaptationSettings.HistogramHighPercent"/> property.
            /// </summary>
            HistogramHighPercent = 1 << 8,

            /// <summary>
            /// All properties.
            /// </summary>
            All = Mode | SpeedUp | SpeedDown | PreExposure | PostExposure | MinBrightness | MaxBrightness | HistogramLowPercent | HistogramHighPercent,
        };

        /// <summary>
        /// The flags for overriden properties.
        /// </summary>
        [HideInEditor]
        public Override OverrideFlags;

        /// <summary>
        /// The effect rendering mode used for the exposure processing.
        /// </summary>
        [DefaultValue(EyeAdaptationMode.AutomaticHistogram)]
        [Tooltip("The effect rendering mode used for the exposure processing.")]
        [EditorOrder(0), PostProcessSetting((int)Override.Mode)]
        public EyeAdaptationMode Mode;

        /// <summary>
        /// The speed at which the exposure changes when the scene brightness moves from a dark area to a bright area (brightness goes up).
        /// </summary>
        [DefaultValue(3.0f), Limit(0, 100.0f, 0.01f)]
        [Tooltip("The speed at which the exposure changes when the scene brightness moves from a dark area to a bright area (brightness goes up).")]
        [EditorOrder(1), PostProcessSetting((int)Override.SpeedUp)]
        public float SpeedUp;

        /// <summary>
        /// The speed at which the exposure changes when the scene brightness moves from a bright area to a dark area (brightness goes down).
        /// </summary>
        [DefaultValue(1.0f), Limit(0, 100.0f, 0.01f)]
        [Tooltip("The speed at which the exposure changes when the scene brightness moves from a bright area to a dark area (brightness goes down).")]
        [EditorOrder(2), PostProcessSetting((int)Override.SpeedDown)]
        public float SpeedDown;

        /// <summary>
        /// The pre-exposure value applied to the scene color before performing post-processing (such as bloom, lens flares, etc.).
        /// </summary>
        [DefaultValue(0.0f), Limit(-100, 100, 0.01f)]
        [Tooltip("The pre-exposure value applied to the scene color before performing post-processing (such as bloom, lens flares, etc.).")]
        [EditorOrder(3), PostProcessSetting((int)Override.PreExposure)]
        public float PreExposure;

        /// <summary>
        /// The post-exposure value applied to the scene color after performing post-processing (such as bloom, lens flares, etc.) but before color grading and tone mapping.
        /// </summary>
        [DefaultValue(0.0f), Limit(-100, 100, 0.01f)]
        [Tooltip("The post-exposure value applied to the scene color after performing post-processing (such as bloom, lens flares, etc.) but before color grading and tone mapping.")]
        [EditorOrder(3), PostProcessSetting((int)Override.PostExposure)]
        public float PostExposure;

        /// <summary>
        /// The minimum brightness for the auto exposure which limits the lower brightness the eye can adapt within.
        /// </summary>
        [DefaultValue(0.03f), Limit(0, 20.0f, 0.01f)]
        [Tooltip("The minimum brightness for the auto exposure which limits the lower brightness the eye can adapt within.")]
        [EditorOrder(5), PostProcessSetting((int)Override.MinBrightness)]
        [EditorDisplay(null, "Minimum Brightness")]
        public float MinBrightness;

        /// <summary>
        /// The maximum brightness for the auto exposure which limits the upper brightness the eye can adapt within.
        /// </summary>
        [DefaultValue(2.0f), Limit(0, 100.0f, 0.01f)]
        [Tooltip("The maximum brightness for the auto exposure which limits the upper brightness the eye can adapt within.")]
        [EditorOrder(6), PostProcessSetting((int)Override.MaxBrightness)]
        [EditorDisplay(null, "Maximum Brightness")]
        public float MaxBrightness;

        /// <summary>
        /// The lower bound for the luminance histogram of the scene color. Value is in percent and limits the pixels below this brightness. Use values from range 60-80. Used only in AutomaticHistogram mode.
        /// </summary>
        [DefaultValue(75.0f), Limit(1, 99, 0.001f)]
        [Tooltip("The lower bound for the luminance histogram of the scene color. Value is in percent and limits the pixels below this brightness. Use values from range 60-80. Used only in AutomaticHistogram mode.")]
        [EditorOrder(3), PostProcessSetting((int)Override.HistogramLowPercent)]
        public float HistogramLowPercent;

        /// <summary>
        /// The upper bound for the luminance histogram of the scene color. Value is in percent and limits the pixels above this brightness. Use values from range 80-95. Used only in AutomaticHistogram mode.
        /// </summary>
        [DefaultValue(98.0f), Limit(1, 99, 0.001f)]
        [Tooltip("The upper bound for the luminance histogram of the scene color. Value is in percent and limits the pixels above this brightness. Use values from range 80-95. Used only in AutomaticHistogram mode.")]
        [EditorOrder(3), PostProcessSetting((int)Override.HistogramHighPercent)]
        public float HistogramHighPercent;
    }

    /// <summary>
    /// Contains settings for Camera Artifacts effect rendering.
    /// </summary>
    [Serializable, StructLayout(LayoutKind.Sequential)]
    public struct CameraArtifactsSettings
    {
        /// <summary>
        /// The structure members override flags.
        /// </summary>
        [Flags]
        public enum Override : int
        {
            /// <summary>
            /// None properties.
            /// </summary>
            None = 0,

            /// <summary>
            /// Overrides <see cref="CameraArtifactsSettings.VignetteIntensity"/> property.
            /// </summary>
            VignetteIntensity = 1 << 0,

            /// <summary>
            /// Overrides <see cref="CameraArtifactsSettings.VignetteColor"/> property.
            /// </summary>
            VignetteColor = 1 << 1,

            /// <summary>
            /// Overrides <see cref="CameraArtifactsSettings.VignetteShapeFactor"/> property.
            /// </summary>
            VignetteShapeFactor = 1 << 2,

            /// <summary>
            /// Overrides <see cref="CameraArtifactsSettings.GrainAmount"/> property.
            /// </summary>
            GrainAmount = 1 << 3,

            /// <summary>
            /// Overrides <see cref="CameraArtifactsSettings.GrainParticleSize"/> property.
            /// </summary>
            GrainParticleSize = 1 << 4,

            /// <summary>
            /// Overrides <see cref="CameraArtifactsSettings.GrainSpeed"/> property.
            /// </summary>
            GrainSpeed = 1 << 5,

            /// <summary>
            /// Overrides <see cref="CameraArtifactsSettings.ChromaticDistortion"/> property.
            /// </summary>
            ChromaticDistortion = 1 << 6,

            /// <summary>
            /// Overrides <see cref="CameraArtifactsSettings.ScreenFadeColor"/> property.
            /// </summary>
            ScreenFadeColor = 1 << 7,

            /// <summary>
            /// All properties.
            /// </summary>
            All = VignetteIntensity | VignetteColor | VignetteShapeFactor | GrainAmount | GrainParticleSize |
                  GrainSpeed | ChromaticDistortion | ScreenFadeColor,
        };

        /// <summary>
        /// The flags for overriden properties.
        /// </summary>
        [HideInEditor]
        public Override OverrideFlags;

        /// <summary>
        /// Gets or sets the vignette intensity.
        /// </summary>
        [DefaultValue(0.8f), Limit(0, 2, 0.001f)]
        [EditorOrder(0), PostProcessSetting((int)Override.VignetteIntensity)]
        public float VignetteIntensity;

        /// <summary>
        /// Gets or sets the vignette color.
        /// </summary>
        [EditorOrder(1), PostProcessSetting((int)Override.VignetteColor)]
        public Color VignetteColor;

        /// <summary>
        /// Gets or sets the vignette shape factor.
        /// </summary>
        [DefaultValue(0.125f), Limit(0.0001f, 2.0f, 0.001f)]
        [EditorOrder(2), PostProcessSetting((int)Override.VignetteShapeFactor)]
        public float VignetteShapeFactor;

        /// <summary>
        /// Gets or sets the grain noise amount.
        /// </summary>
        [DefaultValue(0.006f), Limit(0.0f, 2.0f, 0.005f)]
        [EditorOrder(3), PostProcessSetting((int)Override.GrainAmount)]
        public float GrainAmount;

        /// <summary>
        /// Gets or sets the grain noise particles size.
        /// </summary>
        [DefaultValue(1.6f), Limit(1.0f, 3.0f, 0.01f)]
        [EditorOrder(4), PostProcessSetting((int)Override.GrainParticleSize)]
        public float GrainParticleSize;

        /// <summary>
        /// Gets or sets the grain noise particles size.
        /// </summary>
        [DefaultValue(1.0f), Limit(0.0f, 10.0f, 0.01f)]
        [EditorOrder(5), PostProcessSetting((int)Override.GrainSpeed)]
        [Tooltip("Specifies grain particles animation speed")]
        public float GrainSpeed;

        /// <summary>
        /// Gets or sets the chromatic aberration distortion intensity.
        /// </summary>
        [DefaultValue(0.0f), Limit(0.0f, 1.0f, 0.01f)]
        [EditorOrder(6), PostProcessSetting((int)Override.ChromaticDistortion)]
        public float ChromaticDistortion;

        /// <summary>
        /// Gets or sets the screen tint color (alpha channel defines the blending factor)..
        /// </summary>
        [DefaultValue(typeof(Color), "0,0,0,0")]
        [EditorOrder(7), PostProcessSetting((int)Override.ScreenFadeColor)]
        [Tooltip("Screen fade color (alpha channel defines the blending factor)")]
        public Color ScreenFadeColor;
    }

    /// <summary>
    /// Contains settings for Lens Flares effect rendering.
    /// </summary>
    [Serializable, StructLayout(LayoutKind.Sequential)]
    public struct LensFlaresSettings
    {
        /// <summary>
        /// The structure members override flags.
        /// </summary>
        [Flags]
        public enum Override : int
        {
            /// <summary>
            /// None properties.
            /// </summary>
            None = 0,

            /// <summary>
            /// Overrides <see cref="LensFlaresSettings.Intensity"/> property.
            /// </summary>
            Intensity = 1 << 0,

            /// <summary>
            /// Overrides <see cref="LensFlaresSettings.Ghosts"/> property.
            /// </summary>
            Ghosts = 1 << 1,

            /// <summary>
            /// Overrides <see cref="LensFlaresSettings.HaloWidth"/> property.
            /// </summary>
            HaloWidth = 1 << 2,

            /// <summary>
            /// Overrides <see cref="LensFlaresSettings.HaloIntensity"/> property.
            /// </summary>
            HaloIntensity = 1 << 3,

            /// <summary>
            /// Overrides <see cref="LensFlaresSettings.GhostDispersal"/> property.
            /// </summary>
            GhostDispersal = 1 << 4,

            /// <summary>
            /// Overrides <see cref="LensFlaresSettings.Distortion"/> property.
            /// </summary>
            Distortion = 1 << 5,

            /// <summary>
            /// Overrides <see cref="LensFlaresSettings.ThresholdBias"/> property.
            /// </summary>
            ThresholdBias = 1 << 6,

            /// <summary>
            /// Overrides <see cref="LensFlaresSettings.ThresholdScale"/> property.
            /// </summary>
            ThresholdScale = 1 << 7,

            /// <summary>
            /// Overrides <see cref="LensFlaresSettings.LensDirt"/> property.
            /// </summary>
            LensDirt = 1 << 8,

            /// <summary>
            /// Overrides <see cref="LensFlaresSettings.LensDirtIntensity"/> property.
            /// </summary>
            LensDirtIntensity = 1 << 9,

            /// <summary>
            /// Overrides <see cref="LensFlaresSettings.LensColor"/> property.
            /// </summary>
            LensColor = 1 << 10,

            /// <summary>
            /// Overrides <see cref="LensFlaresSettings.LensStar"/> property.
            /// </summary>
            LensStar = 1 << 11,

            /// <summary>
            /// All properties.
            /// </summary>
            All = Intensity | Ghosts | HaloWidth | HaloIntensity | GhostDispersal | Distortion |
                  ThresholdBias | ThresholdScale | LensDirt | LensDirtIntensity | LensColor | LensStar,
        };

        /// <summary>
        /// The flags for overriden properties.
        /// </summary>
        [HideInEditor]
        public Override OverrideFlags;

        /// <summary>
        /// Gets or sets the lens flares intensity.
        /// </summary>
        [DefaultValue(1.0f), Limit(0, 10.0f, 0.01f)]
        [EditorOrder(0), PostProcessSetting((int)Override.Intensity)]
        public float Intensity;

        /// <summary>
        /// Gets or sets the amount of lens flares ghosts.
        /// </summary>
        [DefaultValue(8), Limit(0, 16)]
        [EditorOrder(1), PostProcessSetting((int)Override.Ghosts)]
        public int Ghosts;

        /// <summary>
        /// Gets or sets the lens flares halo size.
        /// </summary>
        [DefaultValue(0.16f)]
        [EditorOrder(2), PostProcessSetting((int)Override.HaloWidth)]
        public float HaloWidth;

        /// <summary>
        /// Gets or sets the lens flares halo intensity.
        /// </summary>
        [DefaultValue(0.666f), Limit(0, 10.0f, 0.01f)]
        [EditorOrder(3), PostProcessSetting((int)Override.HaloIntensity)]
        public float HaloIntensity;

        /// <summary>
        /// Gets or sets the lens flares ghosts dispersal.
        /// </summary>
        [DefaultValue(0.3f)]
        [EditorOrder(4), PostProcessSetting((int)Override.GhostDispersal)]
        public float GhostDispersal;

        /// <summary>
        /// Gets or sets the lens flares distortion.
        /// </summary>
        [DefaultValue(1.5f)]
        [EditorOrder(5), PostProcessSetting((int)Override.Distortion)]
        public float Distortion;

        /// <summary>
        /// Gets or sets the lens flares threshold bias.
        /// </summary>
        [DefaultValue(-0.5f)]
        [EditorOrder(6), PostProcessSetting((int)Override.ThresholdBias)]
        public float ThresholdBias;

        /// <summary>
        /// Gets or sets the lens flares threshold scale.
        /// </summary>
        [DefaultValue(0.22f)]
        [EditorOrder(7), PostProcessSetting((int)Override.ThresholdScale)]
        public float ThresholdScale;

        private Guid _LensDirt;

        /// <summary>
        /// Gets or sets the camera lens dirt texture.
        /// </summary>
        [DefaultValue(null)]
        [EditorOrder(8), PostProcessSetting((int)Override.LensDirt)]
        [Tooltip("Custom texture for camera dirt")]
        public Texture LensDirt
        {
            get => Content.LoadAsync<Texture>(_LensDirt);
            set => _LensDirt = value?.ID ?? Guid.Empty;
        }

        /// <summary>
        /// Gets or sets the lens dirt intensity.
        /// </summary>
        [DefaultValue(1.0f), Limit(0, 100, 0.01f)]
        [EditorOrder(9), PostProcessSetting((int)Override.LensDirtIntensity)]
        public float LensDirtIntensity;

        private Guid _LensColor;

        /// <summary>
        /// Gets or sets the camera lens color lookup texture.
        /// </summary>
        [DefaultValue(null)]
        [EditorOrder(10), PostProcessSetting((int)Override.LensColor)]
        [Tooltip("Custom texture for lens flares color")]
        public Texture LensColor
        {
            get => Content.LoadAsync<Texture>(_LensColor);
            set => _LensColor = value?.ID ?? Guid.Empty;
        }

        private Guid _LensStar;

        /// <summary>
        /// Gets or sets the lens star lookup texture.
        /// </summary>
        [DefaultValue(null)]
        [EditorOrder(11), PostProcessSetting((int)Override.LensStar)]
        [Tooltip("Custom texture for lens flares star")]
        public Texture LensStar
        {
            get => Content.LoadAsync<Texture>(_LensStar);
            set => _LensStar = value?.ID ?? Guid.Empty;
        }
    }

    /// <summary>
    /// Contains settings for Depth Of Field effect rendering.
    /// </summary>
    [Serializable, StructLayout(LayoutKind.Sequential)]
    public struct DepthOfFieldSettings
    {
        /// <summary>
        /// The structure members override flags.
        /// </summary>
        [Flags]
        public enum Override : int
        {
            /// <summary>
            /// None properties.
            /// </summary>
            None = 0,

            /// <summary>
            /// Overrides <see cref="DepthOfFieldSettings.Enabled"/> property.
            /// </summary>
            Enabled = 1 << 0,

            /// <summary>
            /// Overrides <see cref="DepthOfFieldSettings.BlurStrength"/> property.
            /// </summary>
            BlurStrength = 1 << 1,

            /// <summary>
            /// Overrides <see cref="DepthOfFieldSettings.FocalDistance"/> property.
            /// </summary>
            FocalDistance = 1 << 2,

            /// <summary>
            /// Overrides <see cref="DepthOfFieldSettings.FocalRegion"/> property.
            /// </summary>
            FocalRegion = 1 << 3,

            /// <summary>
            /// Overrides <see cref="DepthOfFieldSettings.NearTransitionRange"/> property.
            /// </summary>
            NearTransitionRange = 1 << 4,

            /// <summary>
            /// Overrides <see cref="DepthOfFieldSettings.FarTransitionRange"/> property.
            /// </summary>
            FarTransitionRange = 1 << 5,

            /// <summary>
            /// Overrides <see cref="DepthOfFieldSettings.DepthLimit"/> property.
            /// </summary>
            DepthLimit = 1 << 6,

            /// <summary>
            /// Overrides <see cref="DepthOfFieldSettings.BokehEnabled"/> property.
            /// </summary>
            BokehEnabled = 1 << 7,

            /// <summary>
            /// Overrides <see cref="DepthOfFieldSettings.BokehSize"/> property.
            /// </summary>
            BokehSize = 1 << 8,

            /// <summary>
            /// Overrides <see cref="DepthOfFieldSettings.BokehShape"/> property.
            /// </summary>
            BokehShape = 1 << 9,

            /// <summary>
            /// Overrides <see cref="DepthOfFieldSettings.BokehShapeCustom"/> property.
            /// </summary>
            BokehShapeCustom = 1 << 10,

            /// <summary>
            /// Overrides <see cref="DepthOfFieldSettings.BokehBrightnessThreshold"/> property.
            /// </summary>
            BokehBrightnessThreshold = 1 << 11,

            /// <summary>
            /// Overrides <see cref="DepthOfFieldSettings.BokehBlurThreshold"/> property.
            /// </summary>
            BokehBlurThreshold = 1 << 12,

            /// <summary>
            /// Overrides <see cref="DepthOfFieldSettings.BokehFalloff"/> property.
            /// </summary>
            BokehFalloff = 1 << 13,

            /// <summary>
            /// Overrides <see cref="DepthOfFieldSettings.BokehDepthCutoff"/> property.
            /// </summary>
            BokehDepthCutoff = 1 << 14,

            /// <summary>
            /// All properties.
            /// </summary>
            All = Enabled | BlurStrength | FocalDistance | FocalRegion | NearTransitionRange | FarTransitionRange |
                  DepthLimit | BokehEnabled | BokehSize | BokehShape | BokehShapeCustom | BokehBrightnessThreshold |
                  BokehBlurThreshold | BokehFalloff | BokehDepthCutoff,
        };

        /// <summary>
        /// The flags for overriden properties.
        /// </summary>
        [HideInEditor]
        public Override OverrideFlags;

        /// <summary>
        /// Gets or sets a value indicating whether Depth of Field is enabled.
        /// </summary>
        [DefaultValue(false)]
        [EditorOrder(0), PostProcessSetting((int)Override.Enabled)]
        [Tooltip("Enable depth of field effect")]
        public bool Enabled;

        /// <summary>
        /// Gets or sets the blur intensity in the out-of-focus areas. Allows reducing blur amount by scaling down the Gaussian Blur radius. Normalized to range 0-1.
        /// </summary>
        [DefaultValue(1.0f), Limit(0, 1, 0.1f)]
        [EditorOrder(1), PostProcessSetting((int)Override.BlurStrength)]
        [Tooltip("The blur intensity in the out-of-focus areas. Allows reducing blur amount by scaling down the Gaussian Blur radius. Normalized to range 0-1.")]
        public float BlurStrength;

        /// <summary>
        /// Gets or sets the distance in World Units from the camera that acts as the center of the region where the scene is perfectly in focus and no blurring occurs.
        /// </summary>
        [DefaultValue(550.0f), Limit(0)]
        [EditorOrder(2), PostProcessSetting((int)Override.FocalDistance)]
        [Tooltip("The distance in World Units from the camera that acts as the center of the region where the scene is perfectly in focus and no blurring occurs")]
        public float FocalDistance;

        /// <summary>
        /// Gets or sets the distance in World Units beyond the focal distance where the scene is perfectly in focus and no blurring occurs.
        /// </summary>
        [DefaultValue(1000.0f), Limit(0)]
        [EditorOrder(3), PostProcessSetting((int)Override.FocalRegion)]
        [Tooltip("The distance in World Units beyond the focal distance where the scene is perfectly in focus and no blurring occurs")]
        public float FocalRegion;

        /// <summary>
        /// Gets or sets the distance in World Units from the focal region on the side nearer to the camera over which the scene transitions from focused to blurred.
        /// </summary>
        [DefaultValue(80.0f), Limit(0)]
        [EditorOrder(4), PostProcessSetting((int)Override.NearTransitionRange)]
        [Tooltip("The distance in World Units from the focal region on the side nearer to the camera over which the scene transitions from focused to blurred")]
        public float NearTransitionRange;

        /// <summary>
        /// Gets or sets the distance in World Units from the focal region on the side farther from the camera over which the scene transitions from focused to blurred.
        /// </summary>
        [DefaultValue(100.0f), Limit(0)]
        [EditorOrder(5), PostProcessSetting((int)Override.FarTransitionRange)]
        [Tooltip("The distance in World Units from the focal region on the side farther from the camera over which the scene transitions from focused to blurred")]
        public float FarTransitionRange;

        /// <summary>
        /// Gets or sets the distance in World Units which describes border after that there is no blur (useful to disable DoF on sky). Use 0 to disable that feature.
        /// </summary>
        [DefaultValue(6000.0f), Limit(0, float.MaxValue, 2)]
        [EditorOrder(6), PostProcessSetting((int)Override.DepthLimit)]
        [Tooltip("The distance in World Units which describes border after that there is no blur (useful to disable DoF on sky). Use 0 to disable that feature.")]
        public float DepthLimit;

        /// <summary>
        /// Enables/disables generating Bokeh shapes.
        /// </summary>
        [DefaultValue(true)]
        [EditorOrder(7), PostProcessSetting((int)Override.BokehEnabled)]
        [Tooltip("Enables/disables generating Bokeh shapes")]
        public bool BokehEnabled;

        /// <summary>
        /// Controls Bokeh shapes maximum size.
        /// </summary>
        [DefaultValue(25.0f), Limit(0, 200.0f, 0.1f)]
        [EditorOrder(8), PostProcessSetting((int)Override.BokehSize)]
        [Tooltip("Controls Bokeh shapes maximum size")]
        public float BokehSize;

        /// <summary>
        /// Gets or sets the Bokeh shapes style.
        /// </summary>
        [DefaultValue(BokehShapeType.Circle)]
        [EditorOrder(9), PostProcessSetting((int)Override.BokehShape)]
        [Tooltip("Bokeh shapes style")]
        public BokehShapeType BokehShape;

        private Guid _BokehShapeCustom;

        /// <summary>
        /// Gets or sets the custom texture for bokeh shapes.
        /// </summary>
        [DefaultValue(null)]
        [EditorOrder(10), PostProcessSetting((int)Override.BokehShapeCustom)]
        [Tooltip("Custom texture for bokeh shapes")]
        public Texture BokehShapeCustom
        {
            get => Content.LoadAsync<Texture>(_BokehShapeCustom);
            set => _BokehShapeCustom = value?.ID ?? Guid.Empty;
        }

        /// <summary>
        /// Controls Bokeh shapes generating minimum pixel brightness to appear.
        /// </summary>
        [DefaultValue(0.8f), Limit(0, 10.0f, 0.01f)]
        [EditorOrder(11), PostProcessSetting((int)Override.BokehBrightnessThreshold)]
        [Tooltip("Controls Bokeh shapes generating minimum pixel brightness to appear")]
        public float BokehBrightnessThreshold;

        /// <summary>
        /// Controls Bokeh shapes blur threshold.
        /// </summary>
        [DefaultValue(0.05f), Limit(0, 1.0f, 0.001f)]
        [EditorOrder(12), PostProcessSetting((int)Override.BokehBlurThreshold)]
        [Tooltip("Controls Bokeh shapes blur threshold")]
        public float BokehBlurThreshold;

        /// <summary>
        /// Controls Bokeh shapes brightness falloff parameter.
        /// </summary>
        [DefaultValue(0.5f), Limit(0, 2.0f, 0.001f)]
        [EditorOrder(13), PostProcessSetting((int)Override.BokehFalloff)]
        [Tooltip("Controls Bokeh shapes brightness falloff parameter")]
        public float BokehFalloff;

        /// <summary>
        /// Controls Bokeh shapes depth cutoff parameter.
        /// </summary>
        [DefaultValue(1.5f), Limit(0, 5.0f, 0.001f)]
        [EditorOrder(14), PostProcessSetting((int)Override.BokehDepthCutoff)]
        [Tooltip("Controls Bokeh shapes depth cutoff parameter")]
        public float BokehDepthCutoff;
    }

    /// <summary>
    /// Contains settings for Motion Blur effect rendering.
    /// </summary>
    [Serializable, StructLayout(LayoutKind.Sequential)]
    public struct MotionBlurSettings
    {
        /// <summary>
        /// The structure members override flags.
        /// </summary>
        [Flags]
        public enum Override : int
        {
            /// <summary>
            /// None properties.
            /// </summary>
            None = 0,

            /// <summary>
            /// Overrides <see cref="MotionBlurSettings.Enabled"/> property.
            /// </summary>
            Enabled = 1 << 0,

            /// <summary>
            /// Overrides <see cref="MotionBlurSettings.Scale"/> property.
            /// </summary>
            Scale = 1 << 1,

            /// <summary>
            /// Overrides <see cref="MotionBlurSettings.SampleCount"/> property.
            /// </summary>
            SampleCount = 1 << 2,

            /// <summary>
            /// Overrides <see cref="MotionBlurSettings.MotionVectorsResolution"/> property.
            /// </summary>
            MotionVectorsResolution = 1 << 3,

            /// <summary>
            /// All properties.
            /// </summary>
            All = Enabled | Scale | SampleCount | MotionVectorsResolution,
        };

        /// <summary>
        /// The flags for overriden properties.
        /// </summary>
        [HideInEditor]
        public Override OverrideFlags;

        /// <summary>
        /// Gets or sets a value indicating whether Motion Blur is enabled.
        /// </summary>
        [DefaultValue(true)]
        [EditorOrder(0), PostProcessSetting((int)Override.Enabled)]
        [Tooltip("Enable motion blur effect")]
        public bool Enabled;

        /// <summary>
        /// Gets or sets the motion blur effect scale.
        /// </summary>
        [DefaultValue(1.0f), Limit(0, 5, 0.01f)]
        [EditorOrder(1), PostProcessSetting((int)Override.Scale)]
        [Tooltip("The motion blur effect scale.")]
        public float Scale;

        /// <summary>
        /// Gets or sets the amount of sample points used during motion blur rendering. It affects quality and performances.
        /// </summary>
        [DefaultValue(10), Limit(4, 32, 0.1f)]
        [EditorOrder(2), PostProcessSetting((int)Override.SampleCount)]
        [Tooltip("The amount of sample points used during motion blur rendering. It affects quality and performances.")]
        public int SampleCount;

        /// <summary>
        /// Gets or sets the motion vectors texture resolution. Motion blur uses per-pixel motion vectors buffer that contains objects movement information. Use lower resolution to improve performance.
        /// </summary>
        [DefaultValue(ResolutionMode.Half)]
        [EditorOrder(3), PostProcessSetting((int)Override.MotionVectorsResolution)]
        [Tooltip("The motion vectors texture resolution. Motion blur uses per-pixel motion vectors buffer that contains objects movement information. Use lower resolution to improve performance.")]
        public ResolutionMode MotionVectorsResolution;
    }

    /// <summary>
    /// Contains settings for Screen Space Reflections effect rendering.
    /// </summary>
    [Serializable, StructLayout(LayoutKind.Sequential)]
    public struct ScreenSpaceReflectionsSettings
    {
        /// <summary>
        /// The structure members override flags.
        /// </summary>
        [Flags]
        public enum Override : int
        {
            /// <summary>
            /// None properties.
            /// </summary>
            None = 0,

            /// <summary>
            /// Overrides <see cref="ScreenSpaceReflectionsSettings.Intensity"/> property.
            /// </summary>
            Intensity = 1 << 0,

            /// <summary>
            /// Overrides <see cref="ScreenSpaceReflectionsSettings.DepthResolution"/> property.
            /// </summary>
            DepthResolution = 1 << 1,

            /// <summary>
            /// Overrides <see cref="ScreenSpaceReflectionsSettings.RayTracePassResolution"/> property.
            /// </summary>
            RayTracePassResolution = 1 << 2,

            /// <summary>
            /// Overrides <see cref="ScreenSpaceReflectionsSettings.BRDFBias"/> property.
            /// </summary>
            BRDFBias = 1 << 3,

            /// <summary>
            /// Overrides <see cref="ScreenSpaceReflectionsSettings.RoughnessThreshold"/> property.
            /// </summary>
            RoughnessThreshold = 1 << 4,

            /// <summary>
            /// Overrides <see cref="ScreenSpaceReflectionsSettings.WorldAntiSelfOcclusionBias"/> property.
            /// </summary>
            WorldAntiSelfOcclusionBias = 1 << 5,

            /// <summary>
            /// Overrides <see cref="ScreenSpaceReflectionsSettings.ResolvePassResolution"/> property.
            /// </summary>
            ResolvePassResolution = 1 << 6,

            /// <summary>
            /// Overrides <see cref="ScreenSpaceReflectionsSettings.ResolveSamples"/> property.
            /// </summary>
            ResolveSamples = 1 << 7,

            /// <summary>
            /// Overrides <see cref="ScreenSpaceReflectionsSettings.EdgeFadeFactor"/> property.
            /// </summary>
            EdgeFadeFactor = 1 << 8,

            /// <summary>
            /// Overrides <see cref="ScreenSpaceReflectionsSettings.UseColorBufferMips"/> property.
            /// </summary>
            UseColorBufferMips = 1 << 9,

            /// <summary>
            /// Overrides <see cref="ScreenSpaceReflectionsSettings.TemporalEffect"/> property.
            /// </summary>
            TemporalEffect = 1 << 10,

            /// <summary>
            /// Overrides <see cref="ScreenSpaceReflectionsSettings.TemporalScale"/> property.
            /// </summary>
            TemporalScale = 1 << 11,

            /// <summary>
            /// Overrides <see cref="ScreenSpaceReflectionsSettings.TemporalResponse"/> property.
            /// </summary>
            TemporalResponse = 1 << 12,

            /// <summary>
            /// All properties.
            /// </summary>
            All = Intensity | DepthResolution | RayTracePassResolution | BRDFBias | RoughnessThreshold | WorldAntiSelfOcclusionBias |
                  ResolvePassResolution | ResolveSamples | EdgeFadeFactor | UseColorBufferMips | TemporalEffect | TemporalScale | TemporalResponse,
        };

        /// <summary>
        /// The flags for overriden properties.
        /// </summary>
        [HideInEditor]
        public Override OverrideFlags;

        /// <summary>
        /// Gets or sets the effect intensity (normalized to range [0;1]). Use 0 to disable it.
        /// </summary>
        [DefaultValue(1.0f), Limit(0, 1.0f, 0.01f)]
        [EditorOrder(0), PostProcessSetting((int)Override.Intensity)]
        [Tooltip("Effect intensity (normalized to range [0;1]). Use 0 to disable it.")]
        public float Intensity;

        /// <summary>
        /// Gets or sets the input depth resolution mode.
        /// </summary>
        [DefaultValue(ResolutionMode.Half)]
        [EditorOrder(1), PostProcessSetting((int)Override.DepthResolution)]
        [Tooltip("The depth buffer downscale option to optimize raycast performance. Full gives better quality, but half improves performance. The default value is half.")]
        public ResolutionMode DepthResolution;

        /// <summary>
        /// Gets or sets the ray trace pass resolution mode.
        /// </summary>
        [DefaultValue(ResolutionMode.Half)]
        [EditorOrder(2), PostProcessSetting((int)Override.RayTracePassResolution)]
        [Tooltip("The raycast resolution. Full gives better quality, but half improves performance. The default value is half.")]
        public ResolutionMode RayTracePassResolution;

        /// <summary>
        /// Gets or sets the BRDF bias. This value controls source roughness effect on reflections blur.
        /// Smaller values produce wider reflections spread but also introduce more noise.
        /// Higher values provide more mirror-like reflections. Default value is 0.8.
        /// </summary>
        [DefaultValue(0.82f), Limit(0, 1.0f, 0.01f)]
        [EditorOrder(3), PostProcessSetting((int)Override.BRDFBias)]
        [EditorDisplay(null, "BRDF Bias")]
        [Tooltip("The reflection spread. Higher values provide finer, more mirror-like reflections. This setting has no effect on performance. The default value is 0.82")]
        public float BRDFBias;

        /// <summary>
        /// Minimum allowed surface roughness value to use local reflections.
        /// Pixels with higher values won't be affected by the effect.
        /// </summary>
        [DefaultValue(0.45f), Limit(0, 1.0f, 0.01f)]
        [EditorOrder(4), PostProcessSetting((int)Override.RoughnessThreshold)]
        [Tooltip("The maximum amount of roughness a material must have to reflect the scene. For example, if this value is set to 0.4, only materials with a roughness value of 0.4 or below reflect the scene. The default value is 0.45.")]
        public float RoughnessThreshold;

        /// <summary>
        /// Ray tracing starting position is offset by a percent of the normal in world space to avoid self occlusions.
        /// </summary>
        [DefaultValue(0.1f), Limit(0, 10.0f, 0.01f)]
        [EditorOrder(5), PostProcessSetting((int)Override.WorldAntiSelfOcclusionBias)]
        [Tooltip("The offset of the raycast origin. Lower values produce more correct reflection placement, but produce more artifacts. We recommend values of 0.3 or lower. The default value is 0.1.")]
        public float WorldAntiSelfOcclusionBias;

        /// <summary>
        /// Gets or sets the resolve pass resolution mode.
        /// </summary>
        [DefaultValue(ResolutionMode.Full)]
        [EditorOrder(6), PostProcessSetting((int)Override.ResolvePassResolution)]
        [Tooltip("The raycast resolution. Full gives better quality, but half improves performance. The default value is half.")]
        public ResolutionMode ResolvePassResolution;

        /// <summary>
        /// Gets or sets the resolve pass samples amount. Higher values provide better quality but reduce effect performance.
        /// Default value is 4. Use 1 for the highest speed.
        /// </summary>
        [DefaultValue(4), Limit(1, 8)]
        [EditorOrder(7), PostProcessSetting((int)Override.ResolveSamples)]
        [Tooltip("The number of rays used to resolve the reflection color. Higher values produce less noise, but worse performance. The default value is 4.")]
        public int ResolveSamples;

        /// <summary>
        /// Gets or sets the edge fade factor. It's used to fade off effect on screen edges to provide smoother image.
        /// </summary>
        [DefaultValue(0.1f), Limit(0, 1.0f, 0.02f)]
        [EditorOrder(8), PostProcessSetting((int)Override.EdgeFadeFactor)]
        [Tooltip("The point at which the far edges of the reflection begin to fade. Has no effect on performance. The default value is 0.1.")]
        public float EdgeFadeFactor;

        /// <summary>
        /// Gets or sets a value indicating whether use color buffer mipmaps chain; otherwise will use raw input color buffer to sample reflections color.
        /// Using mipmaps improves resolve pass performance and reduces GPU cache misses.
        /// </summary>
        [DefaultValue(true)]
        [EditorOrder(9), PostProcessSetting((int)Override.UseColorBufferMips)]
        [EditorDisplay(null, "Use Color Buffer Mips")]
        [Tooltip("The input color buffer downscale mode that uses blurred mipmaps when resolving the reflection color. Produces more realistic results by blurring distant parts of reflections in rough (low-gloss) materials. It also improves performance on most platforms but uses more memory.")]
        public bool UseColorBufferMips;

        /// <summary>
        /// Gets or sets a value indicating whether use temporal effect to smooth reflections.
        /// </summary>
        [DefaultValue(true)]
        [EditorOrder(10), PostProcessSetting((int)Override.TemporalEffect)]
        [EditorDisplay(null, "Enable Temporal Effect")]
        [Tooltip("Enables the temporal pass. Reduces noise, but produces an animated \"jittering\" effect that's sometimes noticeable. If disabled, the properties below have no effect.")]
        public bool TemporalEffect;

        /// <summary>
        /// Gets or sets the temporal effect scale. Default is 8.
        /// </summary>
        [DefaultValue(8.0f), Limit(0, 20.0f, 0.5f)]
        [EditorOrder(11), PostProcessSetting((int)Override.TemporalScale)]
        [Tooltip("The intensity of the temporal effect. Lower values produce reflections faster, but more noise. The default value is 8.")]
        public float TemporalScale;

        /// <summary>
        /// Gets or sets the temporal response. Default is 0.8.
        /// </summary>
        [DefaultValue(0.8f), Limit(0.05f, 1.0f, 0.01f)]
        [EditorOrder(12), PostProcessSetting((int)Override.TemporalResponse)]
        [Tooltip("How quickly reflections blend between the reflection in the current frame and the history buffer. Lower values produce reflections faster, but with more jittering. If the camera in your game doesn't move much, we recommend values closer to 1. The default value is 0.8.")]
        public float TemporalResponse;
    }

    /// <summary>
    /// Contains settings for Anti Aliasing effect rendering.
    /// </summary>
    [Serializable, StructLayout(LayoutKind.Sequential)]
    public struct AntiAliasingSettings
    {
        /// <summary>
        /// The structure members override flags.
        /// </summary>
        [Flags]
        public enum Override : int
        {
            /// <summary>
            /// None properties.
            /// </summary>
            None = 0,

            /// <summary>
            /// Overrides <see cref="AntiAliasingSettings.Mode"/> property.
            /// </summary>
            Mode = 1 << 0,

            /// <summary>
            /// Overrides <see cref="AntiAliasingSettings.TAA_JitterSpread"/> property.
            /// </summary>
            TAA_JitterSpread = 1 << 1,

            /// <summary>
            /// Overrides <see cref="AntiAliasingSettings.TAA_Sharpness"/> property.
            /// </summary>
            TAA_Sharpness = 1 << 2,

            /// <summary>
            /// Overrides <see cref="AntiAliasingSettings.TAA_StationaryBlending"/> property.
            /// </summary>
            TAA_StationaryBlending = 1 << 3,

            /// <summary>
            /// Overrides <see cref="AntiAliasingSettings.TAA_MotionBlending"/> property.
            /// </summary>
            TAA_MotionBlending = 1 << 4,

            /// <summary>
            /// All properties.
            /// </summary>
            All = Mode | TAA_JitterSpread | TAA_Sharpness | TAA_StationaryBlending | TAA_MotionBlending,
        };

        /// <summary>
        /// The flags for overriden properties.
        /// </summary>
        [HideInEditor]
        public Override OverrideFlags;

        /// <summary>
        /// Gets or sets the anti-aliasing effect mode.
        /// </summary>
        [DefaultValue(AntialiasingMode.FastApproximateAntialiasing)]
        [EditorOrder(0), PostProcessSetting((int)Override.Mode)]
        [Tooltip("The anti-aliasing effect mode.")]
        public AntialiasingMode Mode;

        /// <summary>
        /// Gets or sets the diameter (in texels) inside which jitter samples are spread. Smaller values result
        /// in crisper but more aliased output, while larger values result in more stable but
        /// blurrier output.
        /// </summary>
        [DefaultValue(0.75f), Limit(0.1f, 1f, 0.001f)]
        [EditorOrder(1), PostProcessSetting((int)Override.TAA_JitterSpread)]
        [EditorDisplay(null, "TAA Jitter Spread")]
        [Tooltip("The diameter (in texels) inside which jitter samples are spread. Smaller values result in crisper but more aliased output, while larger values result in more stable but blurrier output.")]
        public float TAA_JitterSpread;

        /// <summary>
        /// Gets or sets the amount of sharpening applied to the color buffer. High values may introduce dark-border artifacts.
        /// </summary>
        [DefaultValue(0f), Limit(0, 3f, 0.001f)]
        [EditorOrder(2), PostProcessSetting((int)Override.TAA_Sharpness)]
        [EditorDisplay(null, "TAA Sharpness")]
        [Tooltip("Controls the amount of sharpening applied to the color buffer. High values may introduce dark-border artifacts.")]
        public float TAA_Sharpness;

        /// <summary>
        /// Gets or sets the blend coefficient for a stationary fragment. Controls the percentage of history sample blended into the final color.
        /// </summary>
        [DefaultValue(0.95f), Limit(0, 0.99f, 0.001f)]
        [EditorOrder(3), PostProcessSetting((int)Override.TAA_StationaryBlending)]
        [EditorDisplay(null, "TAA Stationary Blending")]
        [Tooltip("The anti-aliasing effect mode.")]
        public float TAA_StationaryBlending;

        /// <summary>
        /// Gets or sets the blend coefficient for a fragment with significant motion. Controls the percentage of history sample blended into the final color.
        /// </summary>
        [DefaultValue(0.4f), Limit(0, 0.99f, 0.001f)]
        [EditorOrder(4), PostProcessSetting((int)Override.TAA_MotionBlending)]
        [EditorDisplay(null, "TAA Motion Blending")]
        [Tooltip("The anti-aliasing effect mode.")]
        public float TAA_MotionBlending;
    }

    /// <summary>
    /// Contains settings for custom PostFx materials rendering.
    /// </summary>
    [Serializable, StructLayout(LayoutKind.Sequential)]
    public struct PostFxMaterialsSettings
    {
        private int _MaterialsCount;
        private Guid _Material0;
        private Guid _Material1;
        private Guid _Material2;
        private Guid _Material3;
        private Guid _Material4;
        private Guid _Material5;
        private Guid _Material6;
        private Guid _Material7;

        /// <summary>
        /// The maximum allowed amount custom post fx materials assigned to <see cref="PostFxMaterialsSettings"/>.
        /// </summary>
        public const int MaxPostFxMaterials = 8;

        [NoSerialize]
        private MaterialBase[] _Materials;

        /// <summary>
        /// Gets the post effect materials collection.
        /// </summary>
        [CustomEditorAlias("FlaxEditor.CustomEditors.Editors.PostFxMaterials")]
        [EditorDisplay(null, EditorDisplayAttribute.InlineStyle)]
        [Tooltip("Post effect materials to render")]
        public unsafe MaterialBase[] Materials
        {
            get
            {
                if (_Materials == null || _Materials.Length != _MaterialsCount)
                {
                    _Materials = _MaterialsCount != 0 ? new MaterialBase[_MaterialsCount] : Utils.GetEmptyArray<MaterialBase>();
                }

                fixed (Guid* postFxMaterials = &_Material0)
                {
                    for (int i = 0; i < _MaterialsCount; i++)
                    {
                        _Materials[i] = Content.LoadAsync<MaterialBase>(postFxMaterials[i]);
                    }
                }

                return _Materials;
            }
            set
            {
                fixed (Guid* postFxMaterials = &_Material0)
                {
                    var postFxLength = Mathf.Min(value?.Length ?? 0, MaxPostFxMaterials);
                    bool posFxMaterialsChanged = _MaterialsCount != postFxLength;

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

                    _MaterialsCount = postFxLength;
                    if (posFxMaterialsChanged)
                    {
                        _Materials = null;
                    }
                }
            }
        }

        /// <summary>
        /// Adds the material to the settings.
        /// </summary>
        /// <param name="material">The material.</param>
        public unsafe void AddMaterial(MaterialBase material)
        {
            if (_MaterialsCount < MaxPostFxMaterials && material)
            {
                fixed (Guid* postFxMaterials = &_Material0)
                {
                    postFxMaterials[_MaterialsCount++] = material.ID;
                    _Materials = null;
                }
            }
        }

        /// <summary>
        /// Removes the material from the settings.
        /// </summary>
        /// <param name="material">The material.</param>
        public unsafe void RemoveMaterial(MaterialBase material)
        {
            if (_MaterialsCount != 0 && material)
            {
                fixed (Guid* postFxMaterials = &_Material0)
                {
                    var id = material.ID;
                    for (int i = 0; i < _MaterialsCount; i++)
                    {
                        if (postFxMaterials[i] == id)
                        {
                            _MaterialsCount--;
                            for (int j = i; j < _MaterialsCount; j++)
                            {
                                postFxMaterials[j] = postFxMaterials[j + 1];
                            }
                            _Materials = null;
                            break;
                        }
                    }
                }
            }
        }
    }
}
