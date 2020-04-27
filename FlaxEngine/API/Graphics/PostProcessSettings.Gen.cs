// This code was auto-generated. Do not modify it.

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace FlaxEngine
{
    /// <summary>
    /// Tone mapping effect rendering modes.
    /// </summary>
    [Tooltip("Tone mapping effect rendering modes.")]
    public enum ToneMappingMode
    {
        /// <summary>
        /// Disabled tone mapping effect.
        /// </summary>
        [Tooltip("Disabled tone mapping effect.")]
        None = 0,

        /// <summary>
        /// The neutral tonemapper.
        /// </summary>
        [Tooltip("The neutral tonemapper.")]
        Neutral = 1,

        /// <summary>
        /// The ACES Filmic reference tonemapper (approximation).
        /// </summary>
        [Tooltip("The ACES Filmic reference tonemapper (approximation).")]
        ACES = 2,
    }
}

namespace FlaxEngine
{
    /// <summary>
    /// Eye adaptation effect rendering modes.
    /// </summary>
    [Tooltip("Eye adaptation effect rendering modes.")]
    public enum EyeAdaptationMode
    {
        /// <summary>
        /// Disabled eye adaptation effect.
        /// </summary>
        [Tooltip("Disabled eye adaptation effect.")]
        None = 0,

        /// <summary>
        /// The manual mode that uses a fixed exposure values.
        /// </summary>
        [Tooltip("The manual mode that uses a fixed exposure values.")]
        Manual = 1,

        /// <summary>
        /// The automatic mode applies the eye adaptation exposure based on the scene color luminance blending using the histogram. Requires compute shader support.
        /// </summary>
        [Tooltip("The automatic mode applies the eye adaptation exposure based on the scene color luminance blending using the histogram. Requires compute shader support.")]
        AutomaticHistogram = 2,

        /// <summary>
        /// The automatic mode applies the eye adaptation exposure based on the scene color luminance blending using the average luminance.
        /// </summary>
        [Tooltip("The automatic mode applies the eye adaptation exposure based on the scene color luminance blending using the average luminance.")]
        AutomaticAverageLuminance = 3,
    }
}

namespace FlaxEngine
{
    /// <summary>
    /// Depth of field bokeh shape types.
    /// </summary>
    [Tooltip("Depth of field bokeh shape types.")]
    public enum BokehShapeType
    {
        /// <summary>
        /// The hexagon shape.
        /// </summary>
        [Tooltip("The hexagon shape.")]
        Hexagon = 0,

        /// <summary>
        /// The octagon shape.
        /// </summary>
        [Tooltip("The octagon shape.")]
        Octagon = 1,

        /// <summary>
        /// The circle shape.
        /// </summary>
        [Tooltip("The circle shape.")]
        Circle = 2,

        /// <summary>
        /// The cross shape.
        /// </summary>
        [Tooltip("The cross shape.")]
        Cross = 3,

        /// <summary>
        /// The custom texture shape.
        /// </summary>
        [Tooltip("The custom texture shape.")]
        Custom = 4,
    }
}

namespace FlaxEngine
{
    /// <summary>
    /// Anti-aliasing modes.
    /// </summary>
    [Tooltip("Anti-aliasing modes.")]
    public enum AntialiasingMode
    {
        /// <summary>
        /// The none.
        /// </summary>
        [Tooltip("The none.")]
        None = 0,

        /// <summary>
        /// Fast-Approximate Anti-Aliasing effect.
        /// </summary>
        [Tooltip("Fast-Approximate Anti-Aliasing effect.")]
        FastApproximateAntialiasing = 1,

        /// <summary>
        /// Temporal Anti-Aliasing effect.
        /// </summary>
        [Tooltip("Temporal Anti-Aliasing effect.")]
        TemporalAntialiasing = 2,

        /// <summary>
        /// Subpixel Morphological Anti-Aliasing effect.
        /// </summary>
        [Tooltip("Subpixel Morphological Anti-Aliasing effect.")]
        SubpixelMorphologicalAntialiasing = 3,
    }
}

namespace FlaxEngine
{
    /// <summary>
    /// The effect pass resolution.
    /// </summary>
    [Tooltip("The effect pass resolution.")]
    public enum ResolutionMode : int
    {
        /// <summary>
        /// Full resolution
        /// </summary>
        [Tooltip("Full resolution")]
        Full = 1,

        /// <summary>
        /// Half resolution
        /// </summary>
        [Tooltip("Half resolution")]
        Half = 2,
    }
}

namespace FlaxEngine
{
    /// <summary>
    /// The <see cref="AmbientOcclusionSettings"/> structure members override flags.
    /// </summary>
    [Flags]
    [Tooltip("The <see cref=\"AmbientOcclusionSettings\"/> structure members override flags.")]
    public enum AmbientOcclusionSettingsOverride : int
    {
        /// <summary>
        /// None properties.
        /// </summary>
        [Tooltip("None properties.")]
        None = 0,

        /// <summary>
        /// Overrides <see cref="AmbientOcclusionSettings.Enabled"/> property.
        /// </summary>
        [Tooltip("Overrides <see cref=\"AmbientOcclusionSettings.Enabled\"/> property.")]
        Enabled = 1 << 0,

        /// <summary>
        /// Overrides <see cref="AmbientOcclusionSettings.Intensity"/> property.
        /// </summary>
        [Tooltip("Overrides <see cref=\"AmbientOcclusionSettings.Intensity\"/> property.")]
        Intensity = 1 << 1,

        /// <summary>
        /// Overrides <see cref="AmbientOcclusionSettings.Power"/> property.
        /// </summary>
        [Tooltip("Overrides <see cref=\"AmbientOcclusionSettings.Power\"/> property.")]
        Power = 1 << 2,

        /// <summary>
        /// Overrides <see cref="AmbientOcclusionSettings.Radius"/> property.
        /// </summary>
        [Tooltip("Overrides <see cref=\"AmbientOcclusionSettings.Radius\"/> property.")]
        Radius = 1 << 3,

        /// <summary>
        /// Overrides <see cref="AmbientOcclusionSettings.FadeOutDistance"/> property.
        /// </summary>
        [Tooltip("Overrides <see cref=\"AmbientOcclusionSettings.FadeOutDistance\"/> property.")]
        FadeOutDistance = 1 << 4,

        /// <summary>
        /// Overrides <see cref="AmbientOcclusionSettings.FadeDistance"/> property.
        /// </summary>
        [Tooltip("Overrides <see cref=\"AmbientOcclusionSettings.FadeDistance\"/> property.")]
        FadeDistance = 1 << 5,

        /// <summary>
        /// All properties.
        /// </summary>
        [Tooltip("All properties.")]
        All = Enabled | Intensity | Power | Radius | FadeOutDistance | FadeDistance,
    }
}

namespace FlaxEngine
{
    /// <summary>
    /// Contains settings for Ambient Occlusion effect rendering.
    /// </summary>
    [Tooltip("Contains settings for Ambient Occlusion effect rendering.")]
    [StructLayout(LayoutKind.Sequential)]
    public unsafe partial struct AmbientOcclusionSettings
    {
        /// <summary>
        /// The flags for overriden properties.
        /// </summary>
        [HideInEditor]
        [Tooltip("The flags for overriden properties.")]
        public AmbientOcclusionSettingsOverride OverrideFlags;

        /// <summary>
        /// Enable/disable ambient occlusion effect.
        /// </summary>
        [DefaultValue(true), EditorOrder(0), PostProcessSetting((int)AmbientOcclusionSettingsOverride.Enabled)]
        [Tooltip("Enable/disable ambient occlusion effect.")]
        public bool Enabled;

        /// <summary>
        /// Ambient occlusion intensity.
        /// </summary>
        [DefaultValue(0.8f), Limit(0, 5.0f, 0.01f), EditorOrder(1), PostProcessSetting((int)AmbientOcclusionSettingsOverride.Intensity)]
        [Tooltip("Ambient occlusion intensity.")]
        public float Intensity;

        /// <summary>
        /// Ambient occlusion power.
        /// </summary>
        [DefaultValue(0.75f), Limit(0, 4.0f, 0.01f), EditorOrder(2), PostProcessSetting((int)AmbientOcclusionSettingsOverride.Power)]
        [Tooltip("Ambient occlusion power.")]
        public float Power;

        /// <summary>
        /// Ambient occlusion check range radius.
        /// </summary>
        [DefaultValue(0.7f), Limit(0, 100.0f, 0.01f), EditorOrder(3), PostProcessSetting((int)AmbientOcclusionSettingsOverride.Radius)]
        [Tooltip("Ambient occlusion check range radius.")]
        public float Radius;

        /// <summary>
        /// Ambient occlusion fade out end distance from camera (in world units).
        /// </summary>
        [DefaultValue(5000.0f), Limit(0.0f), EditorOrder(4), PostProcessSetting((int)AmbientOcclusionSettingsOverride.FadeOutDistance)]
        [Tooltip("Ambient occlusion fade out end distance from camera (in world units).")]
        public float FadeOutDistance;

        /// <summary>
        /// Ambient occlusion fade distance (in world units). Defines the size of the effect fade from fully visible to fully invisible at FadeOutDistance.
        /// </summary>
        [DefaultValue(500.0f), Limit(0.0f), EditorOrder(5), PostProcessSetting((int)AmbientOcclusionSettingsOverride.FadeDistance)]
        [Tooltip("Ambient occlusion fade distance (in world units). Defines the size of the effect fade from fully visible to fully invisible at FadeOutDistance.")]
        public float FadeDistance;
    }
}

namespace FlaxEngine
{
    /// <summary>
    /// The structure members override flags.
    /// </summary>
    [Flags]
    [Tooltip("The structure members override flags.")]
    public enum BloomSettingsOverride : int
    {
        /// <summary>
        /// None properties.
        /// </summary>
        [Tooltip("None properties.")]
        None = 0,

        /// <summary>
        /// Overrides <see cref="BloomSettings.Enabled"/> property.
        /// </summary>
        [Tooltip("Overrides <see cref=\"BloomSettings.Enabled\"/> property.")]
        Enabled = 1 << 0,

        /// <summary>
        /// Overrides <see cref="BloomSettings.Intensity"/> property.
        /// </summary>
        [Tooltip("Overrides <see cref=\"BloomSettings.Intensity\"/> property.")]
        Intensity = 1 << 1,

        /// <summary>
        /// Overrides <see cref="BloomSettings.Threshold"/> property.
        /// </summary>
        [Tooltip("Overrides <see cref=\"BloomSettings.Threshold\"/> property.")]
        Threshold = 1 << 2,

        /// <summary>
        /// Overrides <see cref="BloomSettings.BlurSigma"/> property.
        /// </summary>
        [Tooltip("Overrides <see cref=\"BloomSettings.BlurSigma\"/> property.")]
        BlurSigma = 1 << 3,

        /// <summary>
        /// Overrides <see cref="BloomSettings.Limit"/> property.
        /// </summary>
        [Tooltip("Overrides <see cref=\"BloomSettings.Limit\"/> property.")]
        Limit = 1 << 4,

        /// <summary>
        /// All properties.
        /// </summary>
        [Tooltip("All properties.")]
        All = Enabled | Intensity | Threshold | BlurSigma | Limit,
    }
}

namespace FlaxEngine
{
    /// <summary>
    /// Contains settings for Bloom effect rendering.
    /// </summary>
    [Tooltip("Contains settings for Bloom effect rendering.")]
    [StructLayout(LayoutKind.Sequential)]
    public unsafe partial struct BloomSettings
    {
        /// <summary>
        /// The flags for overriden properties.
        /// </summary>
        [HideInEditor]
        [Tooltip("The flags for overriden properties.")]
        public BloomSettingsOverride OverrideFlags;

        /// <summary>
        /// If checked, bloom effect will be rendered.
        /// </summary>
        [DefaultValue(true), EditorOrder(0), PostProcessSetting((int)BloomSettingsOverride.Enabled)]
        [Tooltip("If checked, bloom effect will be rendered.")]
        public bool Enabled;

        /// <summary>
        /// Bloom effect strength. Value 0 disabled is, while higher values increase the effect.
        /// </summary>
        [DefaultValue(1.0f), Limit(0, 20.0f, 0.01f), EditorOrder(1), PostProcessSetting((int)BloomSettingsOverride.Intensity)]
        [Tooltip("Bloom effect strength. Value 0 disabled is, while higher values increase the effect.")]
        public float Intensity;

        /// <summary>
        /// Minimum pixel brightness value to start blowing. Values below the threshold are skipped.
        /// </summary>
        [DefaultValue(3.0f), Limit(0, 15.0f, 0.01f), EditorOrder(2), PostProcessSetting((int)BloomSettingsOverride.Threshold)]
        [Tooltip("Minimum pixel brightness value to start blowing. Values below the threshold are skipped.")]
        public float Threshold;

        /// <summary>
        /// This affects the fall-off of the bloom. It's the standard deviation (sigma) used in the Gaussian blur formula when calculating the kernel of the bloom.
        /// </summary>
        [DefaultValue(4.0f), Limit(0, 20.0f, 0.01f), EditorOrder(3), PostProcessSetting((int)BloomSettingsOverride.BlurSigma)]
        [Tooltip("This affects the fall-off of the bloom. It's the standard deviation (sigma) used in the Gaussian blur formula when calculating the kernel of the bloom.")]
        public float BlurSigma;

        /// <summary>
        /// Bloom effect brightness limit. Pixels with higher luminance will be capped to this brightness level.
        /// </summary>
        [DefaultValue(10.0f), Limit(0, 100.0f, 0.01f), EditorOrder(4), PostProcessSetting((int)BloomSettingsOverride.Limit)]
        [Tooltip("Bloom effect brightness limit. Pixels with higher luminance will be capped to this brightness level.")]
        public float Limit;
    }
}

namespace FlaxEngine
{
    /// <summary>
    /// The structure members override flags.
    /// </summary>
    [Flags]
    [Tooltip("The structure members override flags.")]
    public enum ToneMappingSettingsOverride : int
    {
        /// <summary>
        /// None properties.
        /// </summary>
        [Tooltip("None properties.")]
        None = 0,

        /// <summary>
        /// Overrides <see cref="ToneMappingSettings.WhiteTemperature"/> property.
        /// </summary>
        [Tooltip("Overrides <see cref=\"ToneMappingSettings.WhiteTemperature\"/> property.")]
        WhiteTemperature = 1 << 0,

        /// <summary>
        /// Overrides <see cref="ToneMappingSettings.WhiteTint"/> property.
        /// </summary>
        [Tooltip("Overrides <see cref=\"ToneMappingSettings.WhiteTint\"/> property.")]
        WhiteTint = 1 << 1,

        /// <summary>
        /// Overrides <see cref="ToneMappingSettings.Mode"/> property.
        /// </summary>
        [Tooltip("Overrides <see cref=\"ToneMappingSettings.Mode\"/> property.")]
        Mode = 1 << 2,

        /// <summary>
        /// All properties.
        /// </summary>
        [Tooltip("All properties.")]
        All = WhiteTemperature | WhiteTint | Mode,
    }
}

namespace FlaxEngine
{
    /// <summary>
    /// Contains settings for Tone Mapping effect rendering.
    /// </summary>
    [Tooltip("Contains settings for Tone Mapping effect rendering.")]
    [StructLayout(LayoutKind.Sequential)]
    public unsafe partial struct ToneMappingSettings
    {
        /// <summary>
        /// The flags for overriden properties.
        /// </summary>
        [HideInEditor]
        [Tooltip("The flags for overriden properties.")]
        public ToneMappingSettingsOverride OverrideFlags;

        /// <summary>
        /// Adjusts the white balance in relation to the temperature of the light in the scene. When the light temperature and this one match the light will appear white. When a value is used that is higher than the light in the scene it will yield a "warm" or yellow color, and, conversely, if the value is lower, it would yield a "cool" or blue color. The default value is `6500`.
        /// </summary>
        [DefaultValue(6500.0f), Limit(1500, 15000), EditorOrder(0), PostProcessSetting((int)ToneMappingSettingsOverride.WhiteTemperature)]
        [Tooltip("Adjusts the white balance in relation to the temperature of the light in the scene. When the light temperature and this one match the light will appear white. When a value is used that is higher than the light in the scene it will yield a \"warm\" or yellow color, and, conversely, if the value is lower, it would yield a \"cool\" or blue color. The default value is `6500`.")]
        public float WhiteTemperature;

        /// <summary>
        /// Adjusts the white balance temperature tint for the scene by adjusting the cyan and magenta color ranges. Ideally, this setting should be used once you've adjusted the white balance temporature to get accurate colors. Under some light temperatures, the colors may appear to be more yellow or blue. This can be used to balance the resulting color to look more natural. The default value is `0`.
        /// </summary>
        [DefaultValue(0.0f), Limit(-1, 1, 0.001f), EditorOrder(1), PostProcessSetting((int)ToneMappingSettingsOverride.WhiteTint)]
        [Tooltip("Adjusts the white balance temperature tint for the scene by adjusting the cyan and magenta color ranges. Ideally, this setting should be used once you've adjusted the white balance temporature to get accurate colors. Under some light temperatures, the colors may appear to be more yellow or blue. This can be used to balance the resulting color to look more natural. The default value is `0`.")]
        public float WhiteTint;

        /// <summary>
        /// The tone mapping mode to use for the color grading process.
        /// </summary>
        [DefaultValue(ToneMappingMode.ACES), EditorOrder(2), PostProcessSetting((int)ToneMappingSettingsOverride.Mode)]
        [Tooltip("The tone mapping mode to use for the color grading process.")]
        public ToneMappingMode Mode;
    }
}

namespace FlaxEngine
{
    /// <summary>
    /// The structure members override flags.
    /// </summary>
    [Flags]
    [Tooltip("The structure members override flags.")]
    public enum ColorGradingSettingsOverride : int
    {
        /// <summary>
        /// None properties.
        /// </summary>
        [Tooltip("None properties.")]
        None = 0,

        /// <summary>
        /// Overrides <see cref="ColorGradingSettings.ColorSaturation"/> property.
        /// </summary>
        [Tooltip("Overrides <see cref=\"ColorGradingSettings.ColorSaturation\"/> property.")]
        ColorSaturation = 1 << 0,

        /// <summary>
        /// Overrides <see cref="ColorGradingSettings.ColorContrast"/> property.
        /// </summary>
        [Tooltip("Overrides <see cref=\"ColorGradingSettings.ColorContrast\"/> property.")]
        ColorContrast = 1 << 1,

        /// <summary>
        /// Overrides <see cref="ColorGradingSettings.ColorGamma"/> property.
        /// </summary>
        [Tooltip("Overrides <see cref=\"ColorGradingSettings.ColorGamma\"/> property.")]
        ColorGamma = 1 << 2,

        /// <summary>
        /// Overrides <see cref="ColorGradingSettings.ColorGain"/> property.
        /// </summary>
        [Tooltip("Overrides <see cref=\"ColorGradingSettings.ColorGain\"/> property.")]
        ColorGain = 1 << 3,

        /// <summary>
        /// Overrides <see cref="ColorGradingSettings.ColorOffset"/> property.
        /// </summary>
        [Tooltip("Overrides <see cref=\"ColorGradingSettings.ColorOffset\"/> property.")]
        ColorOffset = 1 << 4,

        /// <summary>
        /// Overrides <see cref="ColorGradingSettings.ColorSaturationShadows"/> property.
        /// </summary>
        [Tooltip("Overrides <see cref=\"ColorGradingSettings.ColorSaturationShadows\"/> property.")]
        ColorSaturationShadows = 1 << 5,

        /// <summary>
        /// Overrides <see cref="ColorGradingSettings.ColorContrastShadows"/> property.
        /// </summary>
        [Tooltip("Overrides <see cref=\"ColorGradingSettings.ColorContrastShadows\"/> property.")]
        ColorContrastShadows = 1 << 6,

        /// <summary>
        /// Overrides <see cref="ColorGradingSettings.ColorGammaShadows"/> property.
        /// </summary>
        [Tooltip("Overrides <see cref=\"ColorGradingSettings.ColorGammaShadows\"/> property.")]
        ColorGammaShadows = 1 << 7,

        /// <summary>
        /// Overrides <see cref="ColorGradingSettings.ColorGainShadows"/> property.
        /// </summary>
        [Tooltip("Overrides <see cref=\"ColorGradingSettings.ColorGainShadows\"/> property.")]
        ColorGainShadows = 1 << 8,

        /// <summary>
        /// Overrides <see cref="ColorGradingSettings.ColorOffsetShadows"/> property.
        /// </summary>
        [Tooltip("Overrides <see cref=\"ColorGradingSettings.ColorOffsetShadows\"/> property.")]
        ColorOffsetShadows = 1 << 9,

        /// <summary>
        /// Overrides <see cref="ColorGradingSettings.ColorSaturationMidtones"/> property.
        /// </summary>
        [Tooltip("Overrides <see cref=\"ColorGradingSettings.ColorSaturationMidtones\"/> property.")]
        ColorSaturationMidtones = 1 << 10,

        /// <summary>
        /// Overrides <see cref="ColorGradingSettings.ColorContrastMidtones"/> property.
        /// </summary>
        [Tooltip("Overrides <see cref=\"ColorGradingSettings.ColorContrastMidtones\"/> property.")]
        ColorContrastMidtones = 1 << 11,

        /// <summary>
        /// Overrides <see cref="ColorGradingSettings.ColorGammaMidtones"/> property.
        /// </summary>
        [Tooltip("Overrides <see cref=\"ColorGradingSettings.ColorGammaMidtones\"/> property.")]
        ColorGammaMidtones = 1 << 12,

        /// <summary>
        /// Overrides <see cref="ColorGradingSettings.ColorGainMidtones"/> property.
        /// </summary>
        [Tooltip("Overrides <see cref=\"ColorGradingSettings.ColorGainMidtones\"/> property.")]
        ColorGainMidtones = 1 << 13,

        /// <summary>
        /// Overrides <see cref="ColorGradingSettings.ColorOffsetMidtones"/> property.
        /// </summary>
        [Tooltip("Overrides <see cref=\"ColorGradingSettings.ColorOffsetMidtones\"/> property.")]
        ColorOffsetMidtones = 1 << 14,

        /// <summary>
        /// Overrides <see cref="ColorGradingSettings.ColorSaturationHighlights"/> property.
        /// </summary>
        [Tooltip("Overrides <see cref=\"ColorGradingSettings.ColorSaturationHighlights\"/> property.")]
        ColorSaturationHighlights = 1 << 15,

        /// <summary>
        /// Overrides <see cref="ColorGradingSettings.ColorContrastHighlights"/> property.
        /// </summary>
        [Tooltip("Overrides <see cref=\"ColorGradingSettings.ColorContrastHighlights\"/> property.")]
        ColorContrastHighlights = 1 << 16,

        /// <summary>
        /// Overrides <see cref="ColorGradingSettings.ColorGammaHighlights"/> property.
        /// </summary>
        [Tooltip("Overrides <see cref=\"ColorGradingSettings.ColorGammaHighlights\"/> property.")]
        ColorGammaHighlights = 1 << 17,

        /// <summary>
        /// Overrides <see cref="ColorGradingSettings.ColorGainHighlights"/> property.
        /// </summary>
        [Tooltip("Overrides <see cref=\"ColorGradingSettings.ColorGainHighlights\"/> property.")]
        ColorGainHighlights = 1 << 18,

        /// <summary>
        /// Overrides <see cref="ColorGradingSettings.ColorOffsetHighlights"/> property.
        /// </summary>
        [Tooltip("Overrides <see cref=\"ColorGradingSettings.ColorOffsetHighlights\"/> property.")]
        ColorOffsetHighlights = 1 << 19,

        /// <summary>
        /// Overrides <see cref="ColorGradingSettings.ShadowsMax"/> property.
        /// </summary>
        [Tooltip("Overrides <see cref=\"ColorGradingSettings.ShadowsMax\"/> property.")]
        ShadowsMax = 1 << 20,

        /// <summary>
        /// Overrides <see cref="ColorGradingSettings.HighlightsMin"/> property.
        /// </summary>
        [Tooltip("Overrides <see cref=\"ColorGradingSettings.HighlightsMin\"/> property.")]
        HighlightsMin = 1 << 21,

        /// <summary>
        /// All properties.
        /// </summary>
        [Tooltip("All properties.")]
        All = ColorSaturation | ColorContrast | ColorGamma | ColorGain | ColorOffset | ColorSaturationShadows | ColorContrastShadows | ColorGammaShadows | ColorGainShadows | ColorOffsetShadows | ColorSaturationMidtones | ColorContrastMidtones | ColorGammaMidtones | ColorGainMidtones | ColorOffsetMidtones | ColorSaturationHighlights | ColorContrastHighlights | ColorGammaHighlights | ColorGainHighlights | ColorOffsetHighlights | ShadowsMax | HighlightsMin,
    }
}

namespace FlaxEngine
{
    /// <summary>
    /// Contains settings for Color Grading effect rendering.
    /// </summary>
    [Tooltip("Contains settings for Color Grading effect rendering.")]
    [StructLayout(LayoutKind.Sequential)]
    public unsafe partial struct ColorGradingSettings
    {
        /// <summary>
        /// The flags for overriden properties.
        /// </summary>
        [HideInEditor]
        [Tooltip("The flags for overriden properties.")]
        public ColorGradingSettingsOverride OverrideFlags;

        /// <summary>
        /// Gets or sets the color saturation (applies globally to the whole image). Default is 1.
        /// </summary>
        [DefaultValue(typeof(Vector4), "1,1,1,1"), CustomEditorAlias("FlaxEditor.CustomEditors.Editors.ColorTrackball"), EditorOrder(0), PostProcessSetting((int)ColorGradingSettingsOverride.ColorSaturation), Limit(0, 2, 0.01f), EditorDisplay("Global", "Saturation")]
        [Tooltip("Gets or sets the color saturation (applies globally to the whole image). Default is 1.")]
        public Vector4 ColorSaturation;

        /// <summary>
        /// Gets or sets the color contrast (applies globally to the whole image). Default is 1.
        /// </summary>
        [DefaultValue(typeof(Vector4), "1,1,1,1"), CustomEditorAlias("FlaxEditor.CustomEditors.Editors.ColorTrackball"), EditorOrder(1), PostProcessSetting((int)ColorGradingSettingsOverride.ColorContrast), Limit(0, 2, 0.01f), EditorDisplay("Global", "Contrast")]
        [Tooltip("Gets or sets the color contrast (applies globally to the whole image). Default is 1.")]
        public Vector4 ColorContrast;

        /// <summary>
        /// Gets or sets the color gamma (applies globally to the whole image). Default is 1.
        /// </summary>
        [DefaultValue(typeof(Vector4), "1,1,1,1"), CustomEditorAlias("FlaxEditor.CustomEditors.Editors.ColorTrackball"), EditorOrder(2), PostProcessSetting((int)ColorGradingSettingsOverride.ColorGamma), Limit(0, 2, 0.01f), EditorDisplay("Global", "Gamma")]
        [Tooltip("Gets or sets the color gamma (applies globally to the whole image). Default is 1.")]
        public Vector4 ColorGamma;

        /// <summary>
        /// Gets or sets the color gain (applies globally to the whole image). Default is 1.
        /// </summary>
        [DefaultValue(typeof(Vector4), "1,1,1,1"), CustomEditorAlias("FlaxEditor.CustomEditors.Editors.ColorTrackball"), EditorOrder(3), PostProcessSetting((int)ColorGradingSettingsOverride.ColorGain), Limit(0, 2, 0.01f), EditorDisplay("Global", "Gain")]
        [Tooltip("Gets or sets the color gain (applies globally to the whole image). Default is 1.")]
        public Vector4 ColorGain;

        /// <summary>
        /// Gets or sets the color offset (applies globally to the whole image). Default is 0.
        /// </summary>
        [DefaultValue(typeof(Vector4), "0,0,0,0"), CustomEditorAlias("FlaxEditor.CustomEditors.Editors.ColorTrackball"), EditorOrder(4), PostProcessSetting((int)ColorGradingSettingsOverride.ColorOffset), Limit(-1, 1, 0.001f), EditorDisplay("Global", "Offset")]
        [Tooltip("Gets or sets the color offset (applies globally to the whole image). Default is 0.")]
        public Vector4 ColorOffset;

        /// <summary>
        /// Gets or sets the color saturation (applies to shadows only). Default is 1.
        /// </summary>
        [DefaultValue(typeof(Vector4), "1,1,1,1"), CustomEditorAlias("FlaxEditor.CustomEditors.Editors.ColorTrackball"), EditorOrder(5), PostProcessSetting((int)ColorGradingSettingsOverride.ColorSaturationShadows), Limit(0, 2, 0.01f), EditorDisplay("Shadows", "Shadows Saturation")]
        [Tooltip("Gets or sets the color saturation (applies to shadows only). Default is 1.")]
        public Vector4 ColorSaturationShadows;

        /// <summary>
        /// Gets or sets the color contrast (applies to shadows only). Default is 1.
        /// </summary>
        [DefaultValue(typeof(Vector4), "1,1,1,1"), CustomEditorAlias("FlaxEditor.CustomEditors.Editors.ColorTrackball"), EditorOrder(6), PostProcessSetting((int)ColorGradingSettingsOverride.ColorContrastShadows), Limit(0, 2, 0.01f), EditorDisplay("Shadows", "Shadows Contrast")]
        [Tooltip("Gets or sets the color contrast (applies to shadows only). Default is 1.")]
        public Vector4 ColorContrastShadows;

        /// <summary>
        /// Gets or sets the color gamma (applies to shadows only). Default is 1.
        /// </summary>
        [DefaultValue(typeof(Vector4), "1,1,1,1"), CustomEditorAlias("FlaxEditor.CustomEditors.Editors.ColorTrackball"), EditorOrder(7), PostProcessSetting((int)ColorGradingSettingsOverride.ColorGammaShadows), Limit(0, 2, 0.01f), EditorDisplay("Shadows", "Shadows Gamma")]
        [Tooltip("Gets or sets the color gamma (applies to shadows only). Default is 1.")]
        public Vector4 ColorGammaShadows;

        /// <summary>
        /// Gets or sets the color gain (applies to shadows only). Default is 1.
        /// </summary>
        [DefaultValue(typeof(Vector4), "1,1,1,1"), CustomEditorAlias("FlaxEditor.CustomEditors.Editors.ColorTrackball"), EditorOrder(8), PostProcessSetting((int)ColorGradingSettingsOverride.ColorGainShadows), Limit(0, 2, 0.01f), EditorDisplay("Shadows", "Shadows Gain")]
        [Tooltip("Gets or sets the color gain (applies to shadows only). Default is 1.")]
        public Vector4 ColorGainShadows;

        /// <summary>
        /// Gets or sets the color offset (applies to shadows only). Default is 0.
        /// </summary>
        [DefaultValue(typeof(Vector4), "0,0,0,0"), CustomEditorAlias("FlaxEditor.CustomEditors.Editors.ColorTrackball"), EditorOrder(9), PostProcessSetting((int)ColorGradingSettingsOverride.ColorOffsetShadows), Limit(-1, 1, 0.001f), EditorDisplay("Shadows", "Shadows Offset")]
        [Tooltip("Gets or sets the color offset (applies to shadows only). Default is 0.")]
        public Vector4 ColorOffsetShadows;

        /// <summary>
        /// Gets or sets the color saturation (applies to midtones only). Default is 1.
        /// </summary>
        [DefaultValue(typeof(Vector4), "1,1,1,1"), CustomEditorAlias("FlaxEditor.CustomEditors.Editors.ColorTrackball"), EditorOrder(10), PostProcessSetting((int)ColorGradingSettingsOverride.ColorSaturationMidtones), Limit(0, 2, 0.01f), EditorDisplay("Midtones", "Midtones Saturation")]
        [Tooltip("Gets or sets the color saturation (applies to midtones only). Default is 1.")]
        public Vector4 ColorSaturationMidtones;

        /// <summary>
        /// Gets or sets the color contrast (applies to midtones only). Default is 1.
        /// </summary>
        [DefaultValue(typeof(Vector4), "1,1,1,1"), CustomEditorAlias("FlaxEditor.CustomEditors.Editors.ColorTrackball"), EditorOrder(11), PostProcessSetting((int)ColorGradingSettingsOverride.ColorContrastMidtones), Limit(0, 2, 0.01f), EditorDisplay("Midtones", "Midtones Contrast")]
        [Tooltip("Gets or sets the color contrast (applies to midtones only). Default is 1.")]
        public Vector4 ColorContrastMidtones;

        /// <summary>
        /// Gets or sets the color gamma (applies to midtones only). Default is 1.
        /// </summary>
        [DefaultValue(typeof(Vector4), "1,1,1,1"), CustomEditorAlias("FlaxEditor.CustomEditors.Editors.ColorTrackball"), EditorOrder(12), PostProcessSetting((int)ColorGradingSettingsOverride.ColorGammaMidtones), Limit(0, 2, 0.01f), EditorDisplay("Midtones", "Midtones Gamma")]
        [Tooltip("Gets or sets the color gamma (applies to midtones only). Default is 1.")]
        public Vector4 ColorGammaMidtones;

        /// <summary>
        /// Gets or sets the color gain (applies to midtones only). Default is 1.
        /// </summary>
        [DefaultValue(typeof(Vector4), "1,1,1,1"), CustomEditorAlias("FlaxEditor.CustomEditors.Editors.ColorTrackball"), EditorOrder(13), PostProcessSetting((int)ColorGradingSettingsOverride.ColorGainMidtones), Limit(0, 2, 0.01f), EditorDisplay("Midtones", "Midtones Gain")]
        [Tooltip("Gets or sets the color gain (applies to midtones only). Default is 1.")]
        public Vector4 ColorGainMidtones;

        /// <summary>
        /// Gets or sets the color offset (applies to midtones only). Default is 0.
        /// </summary>
        [DefaultValue(typeof(Vector4), "0,0,0,0"), CustomEditorAlias("FlaxEditor.CustomEditors.Editors.ColorTrackball"), EditorOrder(14), PostProcessSetting((int)ColorGradingSettingsOverride.ColorOffsetMidtones), Limit(-1, 1, 0.001f), EditorDisplay("Midtones", "Midtones Offset")]
        [Tooltip("Gets or sets the color offset (applies to midtones only). Default is 0.")]
        public Vector4 ColorOffsetMidtones;

        /// <summary>
        /// Gets or sets the color saturation (applies to highlights only). Default is 1.
        /// </summary>
        [DefaultValue(typeof(Vector4), "1,1,1,1"), CustomEditorAlias("FlaxEditor.CustomEditors.Editors.ColorTrackball"), EditorOrder(15), PostProcessSetting((int)ColorGradingSettingsOverride.ColorSaturationHighlights), Limit(0, 2, 0.01f), EditorDisplay("Highlights", "Highlights Saturation")]
        [Tooltip("Gets or sets the color saturation (applies to highlights only). Default is 1.")]
        public Vector4 ColorSaturationHighlights;

        /// <summary>
        /// Gets or sets the color contrast (applies to highlights only). Default is 1.
        /// </summary>
        [DefaultValue(typeof(Vector4), "1,1,1,1"), CustomEditorAlias("FlaxEditor.CustomEditors.Editors.ColorTrackball"), EditorOrder(16), PostProcessSetting((int)ColorGradingSettingsOverride.ColorContrastHighlights), Limit(0, 2, 0.01f), EditorDisplay("Highlights", "Highlights Contrast")]
        [Tooltip("Gets or sets the color contrast (applies to highlights only). Default is 1.")]
        public Vector4 ColorContrastHighlights;

        /// <summary>
        /// Gets or sets the color gamma (applies to highlights only). Default is 1.
        /// </summary>
        [DefaultValue(typeof(Vector4), "1,1,1,1"), CustomEditorAlias("FlaxEditor.CustomEditors.Editors.ColorTrackball"), EditorOrder(17), PostProcessSetting((int)ColorGradingSettingsOverride.ColorGammaHighlights), Limit(0, 2, 0.01f), EditorDisplay("Highlights", "Highlights Gamma")]
        [Tooltip("Gets or sets the color gamma (applies to highlights only). Default is 1.")]
        public Vector4 ColorGammaHighlights;

        /// <summary>
        /// Gets or sets the color gain (applies to highlights only). Default is 1.
        /// </summary>
        [DefaultValue(typeof(Vector4), "1,1,1,1"), CustomEditorAlias("FlaxEditor.CustomEditors.Editors.ColorTrackball"), EditorOrder(18), PostProcessSetting((int)ColorGradingSettingsOverride.ColorGainHighlights), Limit(0, 2, 0.01f), EditorDisplay("Highlights", "Highlights Gain")]
        [Tooltip("Gets or sets the color gain (applies to highlights only). Default is 1.")]
        public Vector4 ColorGainHighlights;

        /// <summary>
        /// Gets or sets the color offset (applies to highlights only). Default is 0.
        /// </summary>
        [DefaultValue(typeof(Vector4), "0,0,0,0"), CustomEditorAlias("FlaxEditor.CustomEditors.Editors.ColorTrackball"), EditorOrder(19), PostProcessSetting((int)ColorGradingSettingsOverride.ColorOffsetHighlights), Limit(-1, 1, 0.001f), EditorDisplay("Highlights", "Highlights Offset")]
        [Tooltip("Gets or sets the color offset (applies to highlights only). Default is 0.")]
        public Vector4 ColorOffsetHighlights;

        /// <summary>
        /// The shadows maximum value. Default is 0.09.
        /// </summary>
        [DefaultValue(0.09f), Limit(-1, 1, 0.01f), EditorOrder(20), PostProcessSetting((int)ColorGradingSettingsOverride.ShadowsMax)]
        [Tooltip("The shadows maximum value. Default is 0.09.")]
        public float ShadowsMax;

        /// <summary>
        /// The highlights minimum value. Default is 0.5.
        /// </summary>
        [DefaultValue(0.5f), Limit(-1, 1, 0.01f), EditorOrder(21), PostProcessSetting((int)ColorGradingSettingsOverride.HighlightsMin)]
        [Tooltip("The highlights minimum value. Default is 0.5.")]
        public float HighlightsMin;
    }
}

namespace FlaxEngine
{
    /// <summary>
    /// The structure members override flags.
    /// </summary>
    [Flags]
    [Tooltip("The structure members override flags.")]
    public enum EyeAdaptationSettingsOverride : int
    {
        /// <summary>
        /// None properties.
        /// </summary>
        [Tooltip("None properties.")]
        None = 0,

        /// <summary>
        /// Overrides <see cref="EyeAdaptationSettings.Mode"/> property.
        /// </summary>
        [Tooltip("Overrides <see cref=\"EyeAdaptationSettings.Mode\"/> property.")]
        Mode = 1 << 0,

        /// <summary>
        /// Overrides <see cref="EyeAdaptationSettings.SpeedUp"/> property.
        /// </summary>
        [Tooltip("Overrides <see cref=\"EyeAdaptationSettings.SpeedUp\"/> property.")]
        SpeedUp = 1 << 1,

        /// <summary>
        /// Overrides <see cref="EyeAdaptationSettings.SpeedDown"/> property.
        /// </summary>
        [Tooltip("Overrides <see cref=\"EyeAdaptationSettings.SpeedDown\"/> property.")]
        SpeedDown = 1 << 2,

        /// <summary>
        /// Overrides <see cref="EyeAdaptationSettings.PreExposure"/> property.
        /// </summary>
        [Tooltip("Overrides <see cref=\"EyeAdaptationSettings.PreExposure\"/> property.")]
        PreExposure = 1 << 3,

        /// <summary>
        /// Overrides <see cref="EyeAdaptationSettings.PostExposure"/> property.
        /// </summary>
        [Tooltip("Overrides <see cref=\"EyeAdaptationSettings.PostExposure\"/> property.")]
        PostExposure = 1 << 4,

        /// <summary>
        /// Overrides <see cref="EyeAdaptationSettings.MinBrightness"/> property.
        /// </summary>
        [Tooltip("Overrides <see cref=\"EyeAdaptationSettings.MinBrightness\"/> property.")]
        MinBrightness = 1 << 5,

        /// <summary>
        /// Overrides <see cref="EyeAdaptationSettings.MaxBrightness"/> property.
        /// </summary>
        [Tooltip("Overrides <see cref=\"EyeAdaptationSettings.MaxBrightness\"/> property.")]
        MaxBrightness = 1 << 6,

        /// <summary>
        /// Overrides <see cref="EyeAdaptationSettings.HistogramLowPercent"/> property.
        /// </summary>
        [Tooltip("Overrides <see cref=\"EyeAdaptationSettings.HistogramLowPercent\"/> property.")]
        HistogramLowPercent = 1 << 7,

        /// <summary>
        /// Overrides <see cref="EyeAdaptationSettings.HistogramHighPercent"/> property.
        /// </summary>
        [Tooltip("Overrides <see cref=\"EyeAdaptationSettings.HistogramHighPercent\"/> property.")]
        HistogramHighPercent = 1 << 8,

        /// <summary>
        /// All properties.
        /// </summary>
        [Tooltip("All properties.")]
        All = Mode | SpeedUp | SpeedDown | PreExposure | PostExposure | MinBrightness | MaxBrightness | HistogramLowPercent | HistogramHighPercent,
    }
}

namespace FlaxEngine
{
    /// <summary>
    /// Contains settings for Eye Adaptation effect rendering.
    /// </summary>
    [Tooltip("Contains settings for Eye Adaptation effect rendering.")]
    [StructLayout(LayoutKind.Sequential)]
    public unsafe partial struct EyeAdaptationSettings
    {
        /// <summary>
        /// The flags for overriden properties.
        /// </summary>
        [HideInEditor]
        [Tooltip("The flags for overriden properties.")]
        public EyeAdaptationSettingsOverride OverrideFlags;

        /// <summary>
        /// The effect rendering mode used for the exposure processing.
        /// </summary>
        [DefaultValue(EyeAdaptationMode.AutomaticHistogram), EditorOrder(0), PostProcessSetting((int)EyeAdaptationSettingsOverride.Mode)]
        [Tooltip("The effect rendering mode used for the exposure processing.")]
        public EyeAdaptationMode Mode;

        /// <summary>
        /// The speed at which the exposure changes when the scene brightness moves from a dark area to a bright area (brightness goes up).
        /// </summary>
        [DefaultValue(3.0f), Limit(0, 100.0f, 0.01f), EditorOrder(1), PostProcessSetting((int)EyeAdaptationSettingsOverride.SpeedUp)]
        [Tooltip("The speed at which the exposure changes when the scene brightness moves from a dark area to a bright area (brightness goes up).")]
        public float SpeedUp;

        /// <summary>
        /// The speed at which the exposure changes when the scene brightness moves from a bright area to a dark area (brightness goes down).
        /// </summary>
        [DefaultValue(1.0f), Limit(0, 100.0f, 0.01f), EditorOrder(2), PostProcessSetting((int)EyeAdaptationSettingsOverride.SpeedDown)]
        [Tooltip("The speed at which the exposure changes when the scene brightness moves from a bright area to a dark area (brightness goes down).")]
        public float SpeedDown;

        /// <summary>
        /// The pre-exposure value applied to the scene color before performing post-processing (such as bloom, lens flares, etc.).
        /// </summary>
        [DefaultValue(0.0f), Limit(-100, 100, 0.01f), EditorOrder(3), PostProcessSetting((int)EyeAdaptationSettingsOverride.PreExposure)]
        [Tooltip("The pre-exposure value applied to the scene color before performing post-processing (such as bloom, lens flares, etc.).")]
        public float PreExposure;

        /// <summary>
        /// The post-exposure value applied to the scene color after performing post-processing (such as bloom, lens flares, etc.) but before color grading and tone mapping.
        /// </summary>
        [DefaultValue(0.0f), Limit(-100, 100, 0.01f), EditorOrder(3), PostProcessSetting((int)EyeAdaptationSettingsOverride.PostExposure)]
        [Tooltip("The post-exposure value applied to the scene color after performing post-processing (such as bloom, lens flares, etc.) but before color grading and tone mapping.")]
        public float PostExposure;

        /// <summary>
        /// The minimum brightness for the auto exposure which limits the lower brightness the eye can adapt within.
        /// </summary>
        [DefaultValue(0.03f), Limit(0, 20.0f, 0.01f), EditorOrder(5), PostProcessSetting((int)EyeAdaptationSettingsOverride.MinBrightness), EditorDisplay(null, "Minimum Brightness")]
        [Tooltip("The minimum brightness for the auto exposure which limits the lower brightness the eye can adapt within.")]
        public float MinBrightness;

        /// <summary>
        /// The maximum brightness for the auto exposure which limits the upper brightness the eye can adapt within.
        /// </summary>
        [DefaultValue(2.0f), Limit(0, 100.0f, 0.01f), EditorOrder(6), PostProcessSetting((int)EyeAdaptationSettingsOverride.MaxBrightness), EditorDisplay(null, "Maximum Brightness")]
        [Tooltip("The maximum brightness for the auto exposure which limits the upper brightness the eye can adapt within.")]
        public float MaxBrightness;

        /// <summary>
        /// The lower bound for the luminance histogram of the scene color. Value is in percent and limits the pixels below this brightness. Use values from range 60-80. Used only in AutomaticHistogram mode.
        /// </summary>
        [DefaultValue(75.0f), Limit(1, 99, 0.001f), EditorOrder(3), PostProcessSetting((int)EyeAdaptationSettingsOverride.HistogramLowPercent)]
        [Tooltip("The lower bound for the luminance histogram of the scene color. Value is in percent and limits the pixels below this brightness. Use values from range 60-80. Used only in AutomaticHistogram mode.")]
        public float HistogramLowPercent;

        /// <summary>
        /// The upper bound for the luminance histogram of the scene color. Value is in percent and limits the pixels above this brightness. Use values from range 80-95. Used only in AutomaticHistogram mode.
        /// </summary>
        [DefaultValue(98.0f), Limit(1, 99, 0.001f), EditorOrder(3), PostProcessSetting((int)EyeAdaptationSettingsOverride.HistogramHighPercent)]
        [Tooltip("The upper bound for the luminance histogram of the scene color. Value is in percent and limits the pixels above this brightness. Use values from range 80-95. Used only in AutomaticHistogram mode.")]
        public float HistogramHighPercent;
    }
}

namespace FlaxEngine
{
    /// <summary>
    /// The structure members override flags.
    /// </summary>
    [Flags]
    [Tooltip("The structure members override flags.")]
    public enum CameraArtifactsSettingsOverride : int
    {
        /// <summary>
        /// None properties.
        /// </summary>
        [Tooltip("None properties.")]
        None = 0,

        /// <summary>
        /// Overrides <see cref="CameraArtifactsSettings.VignetteIntensity"/> property.
        /// </summary>
        [Tooltip("Overrides <see cref=\"CameraArtifactsSettings.VignetteIntensity\"/> property.")]
        VignetteIntensity = 1 << 0,

        /// <summary>
        /// Overrides <see cref="CameraArtifactsSettings.VignetteColor"/> property.
        /// </summary>
        [Tooltip("Overrides <see cref=\"CameraArtifactsSettings.VignetteColor\"/> property.")]
        VignetteColor = 1 << 1,

        /// <summary>
        /// Overrides <see cref="CameraArtifactsSettings.VignetteShapeFactor"/> property.
        /// </summary>
        [Tooltip("Overrides <see cref=\"CameraArtifactsSettings.VignetteShapeFactor\"/> property.")]
        VignetteShapeFactor = 1 << 2,

        /// <summary>
        /// Overrides <see cref="CameraArtifactsSettings.GrainAmount"/> property.
        /// </summary>
        [Tooltip("Overrides <see cref=\"CameraArtifactsSettings.GrainAmount\"/> property.")]
        GrainAmount = 1 << 3,

        /// <summary>
        /// Overrides <see cref="CameraArtifactsSettings.GrainParticleSize"/> property.
        /// </summary>
        [Tooltip("Overrides <see cref=\"CameraArtifactsSettings.GrainParticleSize\"/> property.")]
        GrainParticleSize = 1 << 4,

        /// <summary>
        /// Overrides <see cref="CameraArtifactsSettings.GrainSpeed"/> property.
        /// </summary>
        [Tooltip("Overrides <see cref=\"CameraArtifactsSettings.GrainSpeed\"/> property.")]
        GrainSpeed = 1 << 5,

        /// <summary>
        /// Overrides <see cref="CameraArtifactsSettings.ChromaticDistortion"/> property.
        /// </summary>
        [Tooltip("Overrides <see cref=\"CameraArtifactsSettings.ChromaticDistortion\"/> property.")]
        ChromaticDistortion = 1 << 6,

        /// <summary>
        /// Overrides <see cref="CameraArtifactsSettings.ScreenFadeColor"/> property.
        /// </summary>
        [Tooltip("Overrides <see cref=\"CameraArtifactsSettings.ScreenFadeColor\"/> property.")]
        ScreenFadeColor = 1 << 7,

        /// <summary>
        /// All properties.
        /// </summary>
        [Tooltip("All properties.")]
        All = VignetteIntensity | VignetteColor | VignetteShapeFactor | GrainAmount | GrainParticleSize | GrainSpeed | ChromaticDistortion | ScreenFadeColor,
    }
}

namespace FlaxEngine
{
    /// <summary>
    /// Contains settings for Camera Artifacts effect rendering.
    /// </summary>
    [Tooltip("Contains settings for Camera Artifacts effect rendering.")]
    [StructLayout(LayoutKind.Sequential)]
    public unsafe partial struct CameraArtifactsSettings
    {
        /// <summary>
        /// The flags for overriden properties.
        /// </summary>
        [HideInEditor]
        [Tooltip("The flags for overriden properties.")]
        public CameraArtifactsSettingsOverride OverrideFlags;

        /// <summary>
        /// Strength of the vignette effect. Value 0 hides it. The default value is 0.8.
        /// </summary>
        [DefaultValue(0.8f), Limit(0, 2, 0.001f), EditorOrder(0), PostProcessSetting((int)CameraArtifactsSettingsOverride.VignetteIntensity)]
        [Tooltip("Strength of the vignette effect. Value 0 hides it. The default value is 0.8.")]
        public float VignetteIntensity;

        /// <summary>
        /// Color of the vignette.
        /// </summary>
        [EditorOrder(1), PostProcessSetting((int)CameraArtifactsSettingsOverride.VignetteColor)]
        [Tooltip("Color of the vignette.")]
        public Vector3 VignetteColor;

        /// <summary>
        /// Controls shape of the vignette. Values near 0 produce rectangle shape. Higher values result in round shape. The default value is 0.125.
        /// </summary>
        [DefaultValue(0.125f), Limit(0.0001f, 2.0f, 0.001f), EditorOrder(2), PostProcessSetting((int)CameraArtifactsSettingsOverride.VignetteShapeFactor)]
        [Tooltip("Controls shape of the vignette. Values near 0 produce rectangle shape. Higher values result in round shape. The default value is 0.125.")]
        public float VignetteShapeFactor;

        /// <summary>
        /// Intensity of the grain filter. Value 0 hides it. The default value is 0.005.
        /// </summary>
        [DefaultValue(0.006f), Limit(0.0f, 2.0f, 0.005f), EditorOrder(3), PostProcessSetting((int)CameraArtifactsSettingsOverride.GrainAmount)]
        [Tooltip("Intensity of the grain filter. Value 0 hides it. The default value is 0.005.")]
        public float GrainAmount;

        /// <summary>
        /// Size of the grain particles. The default value is 1.6.
        /// </summary>
        [DefaultValue(1.6f), Limit(1.0f, 3.0f, 0.01f), EditorOrder(4), PostProcessSetting((int)CameraArtifactsSettingsOverride.GrainParticleSize)]
        [Tooltip("Size of the grain particles. The default value is 1.6.")]
        public float GrainParticleSize;

        /// <summary>
        /// Speed of the grain particles animation.
        /// </summary>
        [DefaultValue(1.0f), Limit(0.0f, 10.0f, 0.01f), EditorOrder(5), PostProcessSetting((int)CameraArtifactsSettingsOverride.GrainSpeed)]
        [Tooltip("Speed of the grain particles animation.")]
        public float GrainSpeed;

        /// <summary>
        /// Controls chromatic aberration effect strength. Value 0 hides it.
        /// </summary>
        [DefaultValue(0.0f), Limit(0.0f, 1.0f, 0.01f), EditorOrder(6), PostProcessSetting((int)CameraArtifactsSettingsOverride.ChromaticDistortion)]
        [Tooltip("Controls chromatic aberration effect strength. Value 0 hides it.")]
        public float ChromaticDistortion;

        /// <summary>
        /// Screen tint color (alpha channel defines the blending factor).
        /// </summary>
        [DefaultValue(typeof(Color), "0,0,0,0"), EditorOrder(7), PostProcessSetting((int)CameraArtifactsSettingsOverride.ScreenFadeColor)]
        [Tooltip("Screen tint color (alpha channel defines the blending factor).")]
        public Color ScreenFadeColor;
    }
}

namespace FlaxEngine
{
    /// <summary>
    /// The structure members override flags.
    /// </summary>
    [Flags]
    [Tooltip("The structure members override flags.")]
    public enum LensFlaresSettingsOverride : int
    {
        /// <summary>
        /// None properties.
        /// </summary>
        [Tooltip("None properties.")]
        None = 0,

        /// <summary>
        /// Overrides <see cref="LensFlaresSettings.Intensity"/> property.
        /// </summary>
        [Tooltip("Overrides <see cref=\"LensFlaresSettings.Intensity\"/> property.")]
        Intensity = 1 << 0,

        /// <summary>
        /// Overrides <see cref="LensFlaresSettings.Ghosts"/> property.
        /// </summary>
        [Tooltip("Overrides <see cref=\"LensFlaresSettings.Ghosts\"/> property.")]
        Ghosts = 1 << 1,

        /// <summary>
        /// Overrides <see cref="LensFlaresSettings.HaloWidth"/> property.
        /// </summary>
        [Tooltip("Overrides <see cref=\"LensFlaresSettings.HaloWidth\"/> property.")]
        HaloWidth = 1 << 2,

        /// <summary>
        /// Overrides <see cref="LensFlaresSettings.HaloIntensity"/> property.
        /// </summary>
        [Tooltip("Overrides <see cref=\"LensFlaresSettings.HaloIntensity\"/> property.")]
        HaloIntensity = 1 << 3,

        /// <summary>
        /// Overrides <see cref="LensFlaresSettings.GhostDispersal"/> property.
        /// </summary>
        [Tooltip("Overrides <see cref=\"LensFlaresSettings.GhostDispersal\"/> property.")]
        GhostDispersal = 1 << 4,

        /// <summary>
        /// Overrides <see cref="LensFlaresSettings.Distortion"/> property.
        /// </summary>
        [Tooltip("Overrides <see cref=\"LensFlaresSettings.Distortion\"/> property.")]
        Distortion = 1 << 5,

        /// <summary>
        /// Overrides <see cref="LensFlaresSettings.ThresholdBias"/> property.
        /// </summary>
        [Tooltip("Overrides <see cref=\"LensFlaresSettings.ThresholdBias\"/> property.")]
        ThresholdBias = 1 << 6,

        /// <summary>
        /// Overrides <see cref="LensFlaresSettings.ThresholdScale"/> property.
        /// </summary>
        [Tooltip("Overrides <see cref=\"LensFlaresSettings.ThresholdScale\"/> property.")]
        ThresholdScale = 1 << 7,

        /// <summary>
        /// Overrides <see cref="LensFlaresSettings.LensDirt"/> property.
        /// </summary>
        [Tooltip("Overrides <see cref=\"LensFlaresSettings.LensDirt\"/> property.")]
        LensDirt = 1 << 8,

        /// <summary>
        /// Overrides <see cref="LensFlaresSettings.LensDirtIntensity"/> property.
        /// </summary>
        [Tooltip("Overrides <see cref=\"LensFlaresSettings.LensDirtIntensity\"/> property.")]
        LensDirtIntensity = 1 << 9,

        /// <summary>
        /// Overrides <see cref="LensFlaresSettings.LensColor"/> property.
        /// </summary>
        [Tooltip("Overrides <see cref=\"LensFlaresSettings.LensColor\"/> property.")]
        LensColor = 1 << 10,

        /// <summary>
        /// Overrides <see cref="LensFlaresSettings.LensStar"/> property.
        /// </summary>
        [Tooltip("Overrides <see cref=\"LensFlaresSettings.LensStar\"/> property.")]
        LensStar = 1 << 11,

        /// <summary>
        /// All properties.
        /// </summary>
        [Tooltip("All properties.")]
        All = Intensity | Ghosts | HaloWidth | HaloIntensity | GhostDispersal | Distortion | ThresholdBias | ThresholdScale | LensDirt | LensDirtIntensity | LensColor | LensStar,
    }
}

namespace FlaxEngine
{
    /// <summary>
    /// Contains settings for Lens Flares effect rendering.
    /// </summary>
    [Tooltip("Contains settings for Lens Flares effect rendering.")]
    [StructLayout(LayoutKind.Sequential)]
    public unsafe partial struct LensFlaresSettings
    {
        /// <summary>
        /// The flags for overriden properties.
        /// </summary>
        [HideInEditor]
        [Tooltip("The flags for overriden properties.")]
        public LensFlaresSettingsOverride OverrideFlags;

        /// <summary>
        /// Strength of the effect. Value 0 disabled it.
        /// </summary>
        [DefaultValue(1.0f), Limit(0, 10.0f, 0.01f), EditorOrder(0), PostProcessSetting((int)LensFlaresSettingsOverride.Intensity)]
        [Tooltip("Strength of the effect. Value 0 disabled it.")]
        public float Intensity;

        /// <summary>
        /// Amount of lens flares ghosts.
        /// </summary>
        [DefaultValue(8), Limit(0, 16), EditorOrder(1), PostProcessSetting((int)LensFlaresSettingsOverride.Ghosts)]
        [Tooltip("Amount of lens flares ghosts.")]
        public int Ghosts;

        /// <summary>
        /// Lens flares halo width.
        /// </summary>
        [DefaultValue(0.16f), EditorOrder(2), PostProcessSetting((int)LensFlaresSettingsOverride.HaloWidth)]
        [Tooltip("Lens flares halo width.")]
        public float HaloWidth;

        /// <summary>
        /// Lens flares halo intensity.
        /// </summary>
        [DefaultValue(0.666f), Limit(0, 10.0f, 0.01f), EditorOrder(3), PostProcessSetting((int)LensFlaresSettingsOverride.HaloIntensity)]
        [Tooltip("Lens flares halo intensity.")]
        public float HaloIntensity;

        /// <summary>
        /// Ghost samples dispersal parameter.
        /// </summary>
        [DefaultValue(0.3f), EditorOrder(4), PostProcessSetting((int)LensFlaresSettingsOverride.GhostDispersal)]
        [Tooltip("Ghost samples dispersal parameter.")]
        public float GhostDispersal;

        /// <summary>
        /// Lens flares color distortion parameter.
        /// </summary>
        [DefaultValue(1.5f), EditorOrder(5), PostProcessSetting((int)LensFlaresSettingsOverride.Distortion)]
        [Tooltip("Lens flares color distortion parameter.")]
        public float Distortion;

        /// <summary>
        /// Input image brightness threshold. Added to input pixels.
        /// </summary>
        [DefaultValue(-0.5f), EditorOrder(6), PostProcessSetting((int)LensFlaresSettingsOverride.ThresholdBias)]
        [Tooltip("Input image brightness threshold. Added to input pixels.")]
        public float ThresholdBias;

        /// <summary>
        /// Input image brightness threshold scale. Used to multiply input pixels.
        /// </summary>
        [DefaultValue(0.22f), EditorOrder(7), PostProcessSetting((int)LensFlaresSettingsOverride.ThresholdScale)]
        [Tooltip("Input image brightness threshold scale. Used to multiply input pixels.")]
        public float ThresholdScale;

        /// <summary>
        /// Fullscreen lens dirt texture.
        /// </summary>
        [DefaultValue(null), EditorOrder(8), PostProcessSetting((int)LensFlaresSettingsOverride.LensDirt)]
        [Tooltip("Fullscreen lens dirt texture.")]
        public Texture LensDirt;

        /// <summary>
        /// Fullscreen lens dirt intensity parameter. Allows to tune dirt visibility.
        /// </summary>
        [DefaultValue(1.0f), Limit(0, 100, 0.01f), EditorOrder(9), PostProcessSetting((int)LensFlaresSettingsOverride.LensDirtIntensity)]
        [Tooltip("Fullscreen lens dirt intensity parameter. Allows to tune dirt visibility.")]
        public float LensDirtIntensity;

        /// <summary>
        /// Custom lens color texture (1D) used for lens color spectrum.
        /// </summary>
        [DefaultValue(null), EditorOrder(10), PostProcessSetting((int)LensFlaresSettingsOverride.LensColor)]
        [Tooltip("Custom lens color texture (1D) used for lens color spectrum.")]
        public Texture LensColor;

        /// <summary>
        /// Custom lens star texture sampled by lens flares.
        /// </summary>
        [DefaultValue(null), EditorOrder(11), PostProcessSetting((int)LensFlaresSettingsOverride.LensStar)]
        [Tooltip("Custom lens star texture sampled by lens flares.")]
        public Texture LensStar;
    }
}

namespace FlaxEngine
{
    /// <summary>
    /// The structure members override flags.
    /// </summary>
    [Flags]
    [Tooltip("The structure members override flags.")]
    public enum DepthOfFieldSettingsOverride : int
    {
        /// <summary>
        /// None properties.
        /// </summary>
        [Tooltip("None properties.")]
        None = 0,

        /// <summary>
        /// Overrides <see cref="DepthOfFieldSettings.Enabled"/> property.
        /// </summary>
        [Tooltip("Overrides <see cref=\"DepthOfFieldSettings.Enabled\"/> property.")]
        Enabled = 1 << 0,

        /// <summary>
        /// Overrides <see cref="DepthOfFieldSettings.BlurStrength"/> property.
        /// </summary>
        [Tooltip("Overrides <see cref=\"DepthOfFieldSettings.BlurStrength\"/> property.")]
        BlurStrength = 1 << 1,

        /// <summary>
        /// Overrides <see cref="DepthOfFieldSettings.FocalDistance"/> property.
        /// </summary>
        [Tooltip("Overrides <see cref=\"DepthOfFieldSettings.FocalDistance\"/> property.")]
        FocalDistance = 1 << 2,

        /// <summary>
        /// Overrides <see cref="DepthOfFieldSettings.FocalRegion"/> property.
        /// </summary>
        [Tooltip("Overrides <see cref=\"DepthOfFieldSettings.FocalRegion\"/> property.")]
        FocalRegion = 1 << 3,

        /// <summary>
        /// Overrides <see cref="DepthOfFieldSettings.NearTransitionRange"/> property.
        /// </summary>
        [Tooltip("Overrides <see cref=\"DepthOfFieldSettings.NearTransitionRange\"/> property.")]
        NearTransitionRange = 1 << 4,

        /// <summary>
        /// Overrides <see cref="DepthOfFieldSettings.FarTransitionRange"/> property.
        /// </summary>
        [Tooltip("Overrides <see cref=\"DepthOfFieldSettings.FarTransitionRange\"/> property.")]
        FarTransitionRange = 1 << 5,

        /// <summary>
        /// Overrides <see cref="DepthOfFieldSettings.DepthLimit"/> property.
        /// </summary>
        [Tooltip("Overrides <see cref=\"DepthOfFieldSettings.DepthLimit\"/> property.")]
        DepthLimit = 1 << 6,

        /// <summary>
        /// Overrides <see cref="DepthOfFieldSettings.BokehEnabled"/> property.
        /// </summary>
        [Tooltip("Overrides <see cref=\"DepthOfFieldSettings.BokehEnabled\"/> property.")]
        BokehEnabled = 1 << 7,

        /// <summary>
        /// Overrides <see cref="DepthOfFieldSettings.BokehSize"/> property.
        /// </summary>
        [Tooltip("Overrides <see cref=\"DepthOfFieldSettings.BokehSize\"/> property.")]
        BokehSize = 1 << 8,

        /// <summary>
        /// Overrides <see cref="DepthOfFieldSettings.BokehShape"/> property.
        /// </summary>
        [Tooltip("Overrides <see cref=\"DepthOfFieldSettings.BokehShape\"/> property.")]
        BokehShape = 1 << 9,

        /// <summary>
        /// Overrides <see cref="DepthOfFieldSettings.BokehShapeCustom"/> property.
        /// </summary>
        [Tooltip("Overrides <see cref=\"DepthOfFieldSettings.BokehShapeCustom\"/> property.")]
        BokehShapeCustom = 1 << 10,

        /// <summary>
        /// Overrides <see cref="DepthOfFieldSettings.BokehBrightnessThreshold"/> property.
        /// </summary>
        [Tooltip("Overrides <see cref=\"DepthOfFieldSettings.BokehBrightnessThreshold\"/> property.")]
        BokehBrightnessThreshold = 1 << 11,

        /// <summary>
        /// Overrides <see cref="DepthOfFieldSettings.BokehBlurThreshold"/> property.
        /// </summary>
        [Tooltip("Overrides <see cref=\"DepthOfFieldSettings.BokehBlurThreshold\"/> property.")]
        BokehBlurThreshold = 1 << 12,

        /// <summary>
        /// Overrides <see cref="DepthOfFieldSettings.BokehFalloff"/> property.
        /// </summary>
        [Tooltip("Overrides <see cref=\"DepthOfFieldSettings.BokehFalloff\"/> property.")]
        BokehFalloff = 1 << 13,

        /// <summary>
        /// Overrides <see cref="DepthOfFieldSettings.BokehDepthCutoff"/> property.
        /// </summary>
        [Tooltip("Overrides <see cref=\"DepthOfFieldSettings.BokehDepthCutoff\"/> property.")]
        BokehDepthCutoff = 1 << 14,

        /// <summary>
        /// All properties.
        /// </summary>
        [Tooltip("All properties.")]
        All = Enabled | BlurStrength | FocalDistance | FocalRegion | NearTransitionRange | FarTransitionRange | DepthLimit | BokehEnabled | BokehSize | BokehShape | BokehShapeCustom | BokehBrightnessThreshold | BokehBlurThreshold | BokehFalloff | BokehDepthCutoff,
    }
}

namespace FlaxEngine
{
    /// <summary>
    /// Contains settings for Depth Of Field effect rendering.
    /// </summary>
    [Tooltip("Contains settings for Depth Of Field effect rendering.")]
    [StructLayout(LayoutKind.Sequential)]
    public unsafe partial struct DepthOfFieldSettings
    {
        /// <summary>
        /// The flags for overriden properties.
        /// </summary>
        [HideInEditor]
        [Tooltip("The flags for overriden properties.")]
        public DepthOfFieldSettingsOverride OverrideFlags;

        /// <summary>
        /// If checked, depth of field effect will be visible.
        /// </summary>
        [DefaultValue(false), EditorOrder(0), PostProcessSetting((int)DepthOfFieldSettingsOverride.Enabled)]
        [Tooltip("If checked, depth of field effect will be visible.")]
        public bool Enabled;

        /// <summary>
        /// The blur intensity in the out-of-focus areas. Allows reducing blur amount by scaling down the Gaussian Blur radius. Normalized to range 0-1.
        /// </summary>
        [DefaultValue(1.0f), Limit(0, 1, 0.01f), EditorOrder(1), PostProcessSetting((int)DepthOfFieldSettingsOverride.BlurStrength)]
        [Tooltip("The blur intensity in the out-of-focus areas. Allows reducing blur amount by scaling down the Gaussian Blur radius. Normalized to range 0-1.")]
        public float BlurStrength;

        /// <summary>
        /// The distance in World Units from the camera that acts as the center of the region where the scene is perfectly in focus and no blurring occurs.
        /// </summary>
        [DefaultValue(1700.0f), Limit(0), EditorOrder(2), PostProcessSetting((int)DepthOfFieldSettingsOverride.FocalDistance)]
        [Tooltip("The distance in World Units from the camera that acts as the center of the region where the scene is perfectly in focus and no blurring occurs.")]
        public float FocalDistance;

        /// <summary>
        /// The distance in World Units beyond the focal distance where the scene is perfectly in focus and no blurring occurs.
        /// </summary>
        [DefaultValue(3000.0f), Limit(0), EditorOrder(3), PostProcessSetting((int)DepthOfFieldSettingsOverride.FocalRegion)]
        [Tooltip("The distance in World Units beyond the focal distance where the scene is perfectly in focus and no blurring occurs.")]
        public float FocalRegion;

        /// <summary>
        /// The distance in World Units from the focal region on the side nearer to the camera over which the scene transitions from focused to blurred.
        /// </summary>
        [DefaultValue(200.0f), Limit(0), EditorOrder(4), PostProcessSetting((int)DepthOfFieldSettingsOverride.NearTransitionRange)]
        [Tooltip("The distance in World Units from the focal region on the side nearer to the camera over which the scene transitions from focused to blurred.")]
        public float NearTransitionRange;

        /// <summary>
        /// The distance in World Units from the focal region on the side farther from the camera over which the scene transitions from focused to blurred.
        /// </summary>
        [DefaultValue(300.0f), Limit(0), EditorOrder(5), PostProcessSetting((int)DepthOfFieldSettingsOverride.FarTransitionRange)]
        [Tooltip("The distance in World Units from the focal region on the side farther from the camera over which the scene transitions from focused to blurred.")]
        public float FarTransitionRange;

        /// <summary>
        /// The distance in World Units which describes border after that there is no blur (useful to disable DoF on sky). Use 0 to disable that feature.
        /// </summary>
        [DefaultValue(0.0f), Limit(0, float.MaxValue, 2), EditorOrder(6), PostProcessSetting((int)DepthOfFieldSettingsOverride.DepthLimit)]
        [Tooltip("The distance in World Units which describes border after that there is no blur (useful to disable DoF on sky). Use 0 to disable that feature.")]
        public float DepthLimit;

        /// <summary>
        /// If checked, bokeh shapes will be rendered.
        /// </summary>
        [DefaultValue(true), EditorOrder(7), PostProcessSetting((int)DepthOfFieldSettingsOverride.BokehEnabled)]
        [Tooltip("If checked, bokeh shapes will be rendered.")]
        public bool BokehEnabled;

        /// <summary>
        /// Controls size of the bokeh shapes.
        /// </summary>
        [DefaultValue(25.0f), Limit(0, 200.0f, 0.1f), EditorOrder(8), PostProcessSetting((int)DepthOfFieldSettingsOverride.BokehSize)]
        [Tooltip("Controls size of the bokeh shapes.")]
        public float BokehSize;

        /// <summary>
        /// Defines bokeh shapes type.
        /// </summary>
        [DefaultValue(BokehShapeType.Octagon), EditorOrder(9), PostProcessSetting((int)DepthOfFieldSettingsOverride.BokehShape)]
        [Tooltip("Defines bokeh shapes type.")]
        public BokehShapeType BokehShape;

        /// <summary>
        /// If BokehShape is set to Custom, then this texture will be used for the bokeh shapes. For best performance, use small, compressed, grayscale textures (for instance 32px).
        /// </summary>
        [DefaultValue(null), EditorOrder(10), PostProcessSetting((int)DepthOfFieldSettingsOverride.BokehShapeCustom)]
        [Tooltip("If BokehShape is set to Custom, then this texture will be used for the bokeh shapes. For best performance, use small, compressed, grayscale textures (for instance 32px).")]
        public Texture BokehShapeCustom;

        /// <summary>
        /// The minimum pixel brightness to create bokeh. Pixels with lower brightness will be skipped.
        /// </summary>
        [DefaultValue(3.0f), Limit(0, 20.0f, 0.01f), EditorOrder(11), PostProcessSetting((int)DepthOfFieldSettingsOverride.BokehBrightnessThreshold)]
        [Tooltip("The minimum pixel brightness to create bokeh. Pixels with lower brightness will be skipped.")]
        public float BokehBrightnessThreshold;

        /// <summary>
        /// Depth of Field bokeh shapes blur threshold.
        /// </summary>
        [DefaultValue(0.05f), Limit(0, 1.0f, 0.001f), EditorOrder(12), PostProcessSetting((int)DepthOfFieldSettingsOverride.BokehBlurThreshold)]
        [Tooltip("Depth of Field bokeh shapes blur threshold.")]
        public float BokehBlurThreshold;

        /// <summary>
        /// Controls bokeh shapes brightness falloff. Higher values reduce bokeh visibility.
        /// </summary>
        [DefaultValue(0.5f), Limit(0, 2.0f, 0.001f), EditorOrder(13), PostProcessSetting((int)DepthOfFieldSettingsOverride.BokehFalloff)]
        [Tooltip("Controls bokeh shapes brightness falloff. Higher values reduce bokeh visibility.")]
        public float BokehFalloff;

        /// <summary>
        /// Controls bokeh shape generation for depth discontinuities.
        /// </summary>
        [DefaultValue(1.5f), Limit(0, 5.0f, 0.001f), EditorOrder(14), PostProcessSetting((int)DepthOfFieldSettingsOverride.BokehDepthCutoff)]
        [Tooltip("Controls bokeh shape generation for depth discontinuities.")]
        public float BokehDepthCutoff;
    }
}

namespace FlaxEngine
{
    /// <summary>
    /// The structure members override flags.
    /// </summary>
    [Flags]
    [Tooltip("The structure members override flags.")]
    public enum MotionBlurSettingsOverride : int
    {
        /// <summary>
        /// None properties.
        /// </summary>
        [Tooltip("None properties.")]
        None = 0,

        /// <summary>
        /// Overrides <see cref="MotionBlurSettings.Enabled"/> property.
        /// </summary>
        [Tooltip("Overrides <see cref=\"MotionBlurSettings.Enabled\"/> property.")]
        Enabled = 1 << 0,

        /// <summary>
        /// Overrides <see cref="MotionBlurSettings.Scale"/> property.
        /// </summary>
        [Tooltip("Overrides <see cref=\"MotionBlurSettings.Scale\"/> property.")]
        Scale = 1 << 1,

        /// <summary>
        /// Overrides <see cref="MotionBlurSettings.SampleCount"/> property.
        /// </summary>
        [Tooltip("Overrides <see cref=\"MotionBlurSettings.SampleCount\"/> property.")]
        SampleCount = 1 << 2,

        /// <summary>
        /// Overrides <see cref="MotionBlurSettings.MotionVectorsResolution"/> property.
        /// </summary>
        [Tooltip("Overrides <see cref=\"MotionBlurSettings.MotionVectorsResolution\"/> property.")]
        MotionVectorsResolution = 1 << 3,

        /// <summary>
        /// All properties.
        /// </summary>
        [Tooltip("All properties.")]
        All = Enabled | Scale | SampleCount | MotionVectorsResolution,
    }
}

namespace FlaxEngine
{
    /// <summary>
    /// Contains settings for Motion Blur effect rendering.
    /// </summary>
    [Tooltip("Contains settings for Motion Blur effect rendering.")]
    [StructLayout(LayoutKind.Sequential)]
    public unsafe partial struct MotionBlurSettings
    {
        /// <summary>
        /// The flags for overriden properties.
        /// </summary>
        [HideInEditor]
        [Tooltip("The flags for overriden properties.")]
        public MotionBlurSettingsOverride OverrideFlags;

        /// <summary>
        /// If checked, motion blur effect will be rendered.
        /// </summary>
        [DefaultValue(true), EditorOrder(0), PostProcessSetting((int)MotionBlurSettingsOverride.Enabled)]
        [Tooltip("If checked, motion blur effect will be rendered.")]
        public bool Enabled;

        /// <summary>
        /// The blur effect strength. Value 0 disabled is, while higher values increase the effect.
        /// </summary>
        [DefaultValue(1.0f), Limit(0, 5, 0.01f), EditorOrder(1), PostProcessSetting((int)MotionBlurSettingsOverride.Scale)]
        [Tooltip("The blur effect strength. Value 0 disabled is, while higher values increase the effect.")]
        public float Scale;

        /// <summary>
        /// The amount of sample points used during motion blur rendering. It affects quality and performance.
        /// </summary>
        [DefaultValue(10), Limit(4, 32, 0.1f), EditorOrder(2), PostProcessSetting((int)MotionBlurSettingsOverride.SampleCount)]
        [Tooltip("The amount of sample points used during motion blur rendering. It affects quality and performance.")]
        public int SampleCount;

        /// <summary>
        /// The motion vectors texture resolution. Motion blur uses per-pixel motion vectors buffer that contains objects movement information. Use lower resolution to improve performance.
        /// </summary>
        [DefaultValue(ResolutionMode.Half), EditorOrder(3), PostProcessSetting((int)MotionBlurSettingsOverride.MotionVectorsResolution)]
        [Tooltip("The motion vectors texture resolution. Motion blur uses per-pixel motion vectors buffer that contains objects movement information. Use lower resolution to improve performance.")]
        public ResolutionMode MotionVectorsResolution;
    }
}

namespace FlaxEngine
{
    /// <summary>
    /// The structure members override flags.
    /// </summary>
    [Flags]
    [Tooltip("The structure members override flags.")]
    public enum ScreenSpaceReflectionsSettingsOverride : int
    {
        /// <summary>
        /// None properties.
        /// </summary>
        [Tooltip("None properties.")]
        None = 0,

        /// <summary>
        /// Overrides <see cref="ScreenSpaceReflectionsSettings.Intensity"/> property.
        /// </summary>
        [Tooltip("Overrides <see cref=\"ScreenSpaceReflectionsSettings.Intensity\"/> property.")]
        Intensity = 1 << 0,

        /// <summary>
        /// Overrides <see cref="ScreenSpaceReflectionsSettings.DepthResolution"/> property.
        /// </summary>
        [Tooltip("Overrides <see cref=\"ScreenSpaceReflectionsSettings.DepthResolution\"/> property.")]
        DepthResolution = 1 << 1,

        /// <summary>
        /// Overrides <see cref="ScreenSpaceReflectionsSettings.RayTracePassResolution"/> property.
        /// </summary>
        [Tooltip("Overrides <see cref=\"ScreenSpaceReflectionsSettings.RayTracePassResolution\"/> property.")]
        RayTracePassResolution = 1 << 2,

        /// <summary>
        /// Overrides <see cref="ScreenSpaceReflectionsSettings.BRDFBias"/> property.
        /// </summary>
        [Tooltip("Overrides <see cref=\"ScreenSpaceReflectionsSettings.BRDFBias\"/> property.")]
        BRDFBias = 1 << 3,

        /// <summary>
        /// Overrides <see cref="ScreenSpaceReflectionsSettings.RoughnessThreshold"/> property.
        /// </summary>
        [Tooltip("Overrides <see cref=\"ScreenSpaceReflectionsSettings.RoughnessThreshold\"/> property.")]
        RoughnessThreshold = 1 << 4,

        /// <summary>
        /// Overrides <see cref="ScreenSpaceReflectionsSettings.WorldAntiSelfOcclusionBias"/> property.
        /// </summary>
        [Tooltip("Overrides <see cref=\"ScreenSpaceReflectionsSettings.WorldAntiSelfOcclusionBias\"/> property.")]
        WorldAntiSelfOcclusionBias = 1 << 5,

        /// <summary>
        /// Overrides <see cref="ScreenSpaceReflectionsSettings.ResolvePassResolution"/> property.
        /// </summary>
        [Tooltip("Overrides <see cref=\"ScreenSpaceReflectionsSettings.ResolvePassResolution\"/> property.")]
        ResolvePassResolution = 1 << 6,

        /// <summary>
        /// Overrides <see cref="ScreenSpaceReflectionsSettings.ResolveSamples"/> property.
        /// </summary>
        [Tooltip("Overrides <see cref=\"ScreenSpaceReflectionsSettings.ResolveSamples\"/> property.")]
        ResolveSamples = 1 << 7,

        /// <summary>
        /// Overrides <see cref="ScreenSpaceReflectionsSettings.EdgeFadeFactor"/> property.
        /// </summary>
        [Tooltip("Overrides <see cref=\"ScreenSpaceReflectionsSettings.EdgeFadeFactor\"/> property.")]
        EdgeFadeFactor = 1 << 8,

        /// <summary>
        /// Overrides <see cref="ScreenSpaceReflectionsSettings.UseColorBufferMips"/> property.
        /// </summary>
        [Tooltip("Overrides <see cref=\"ScreenSpaceReflectionsSettings.UseColorBufferMips\"/> property.")]
        UseColorBufferMips = 1 << 9,

        /// <summary>
        /// Overrides <see cref="ScreenSpaceReflectionsSettings.TemporalEffect"/> property.
        /// </summary>
        [Tooltip("Overrides <see cref=\"ScreenSpaceReflectionsSettings.TemporalEffect\"/> property.")]
        TemporalEffect = 1 << 10,

        /// <summary>
        /// Overrides <see cref="ScreenSpaceReflectionsSettings.TemporalScale"/> property.
        /// </summary>
        [Tooltip("Overrides <see cref=\"ScreenSpaceReflectionsSettings.TemporalScale\"/> property.")]
        TemporalScale = 1 << 11,

        /// <summary>
        /// Overrides <see cref="ScreenSpaceReflectionsSettings.TemporalResponse"/> property.
        /// </summary>
        [Tooltip("Overrides <see cref=\"ScreenSpaceReflectionsSettings.TemporalResponse\"/> property.")]
        TemporalResponse = 1 << 12,

        /// <summary>
        /// Overrides <see cref="ScreenSpaceReflectionsSettings.FadeOutDistance"/> property.
        /// </summary>
        [Tooltip("Overrides <see cref=\"ScreenSpaceReflectionsSettings.FadeOutDistance\"/> property.")]
        FadeOutDistance = 1 << 13,

        /// <summary>
        /// Overrides <see cref="ScreenSpaceReflectionsSettings.FadeDistance"/> property.
        /// </summary>
        [Tooltip("Overrides <see cref=\"ScreenSpaceReflectionsSettings.FadeDistance\"/> property.")]
        FadeDistance = 1 << 14,

        /// <summary>
        /// All properties.
        /// </summary>
        [Tooltip("All properties.")]
        All = Intensity | DepthResolution | RayTracePassResolution | BRDFBias | RoughnessThreshold | WorldAntiSelfOcclusionBias | ResolvePassResolution | ResolveSamples | EdgeFadeFactor | UseColorBufferMips | TemporalEffect | TemporalScale | TemporalResponse | FadeOutDistance | FadeDistance,
    }
}

namespace FlaxEngine
{
    /// <summary>
    /// Contains settings for Screen Space Reflections effect rendering.
    /// </summary>
    [Tooltip("Contains settings for Screen Space Reflections effect rendering.")]
    [StructLayout(LayoutKind.Sequential)]
    public unsafe partial struct ScreenSpaceReflectionsSettings
    {
        /// <summary>
        /// The flags for overriden properties.
        /// </summary>
        [HideInEditor]
        [Tooltip("The flags for overriden properties.")]
        public ScreenSpaceReflectionsSettingsOverride OverrideFlags;

        /// <summary>
        /// The effect intensity (normalized to range [0;1]). Use 0 to disable it.
        /// </summary>
        [DefaultValue(1.0f), Limit(0, 5.0f, 0.01f), EditorOrder(0), PostProcessSetting((int)ScreenSpaceReflectionsSettingsOverride.Intensity)]
        [Tooltip("The effect intensity (normalized to range [0;1]). Use 0 to disable it.")]
        public float Intensity;

        /// <summary>
        /// The depth buffer downscale option to optimize raycast performance. Full gives better quality, but half improves performance. The default value is half.
        /// </summary>
        [DefaultValue(ResolutionMode.Half), EditorOrder(1), PostProcessSetting((int)ScreenSpaceReflectionsSettingsOverride.DepthResolution)]
        [Tooltip("The depth buffer downscale option to optimize raycast performance. Full gives better quality, but half improves performance. The default value is half.")]
        public ResolutionMode DepthResolution;

        /// <summary>
        /// The raycast resolution. Full gives better quality, but half improves performance. The default value is half.
        /// </summary>
        [DefaultValue(ResolutionMode.Half), EditorOrder(2), PostProcessSetting((int)ScreenSpaceReflectionsSettingsOverride.RayTracePassResolution)]
        [Tooltip("The raycast resolution. Full gives better quality, but half improves performance. The default value is half.")]
        public ResolutionMode RayTracePassResolution;

        /// <summary>
        /// The reflection spread parameter. This value controls source roughness effect on reflections blur. Smaller values produce wider reflections spread but also introduce more noise. Higher values provide more mirror-like reflections. Default value is 0.82.
        /// </summary>
        [DefaultValue(0.82f), Limit(0, 1.0f, 0.01f), EditorOrder(3), PostProcessSetting((int)ScreenSpaceReflectionsSettingsOverride.BRDFBias), EditorDisplay(null, "BRDF Bias")]
        [Tooltip("The reflection spread parameter. This value controls source roughness effect on reflections blur. Smaller values produce wider reflections spread but also introduce more noise. Higher values provide more mirror-like reflections. Default value is 0.82.")]
        public float BRDFBias;

        /// <summary>
        /// The maximum amount of roughness a material must have to reflect the scene. For example, if this value is set to 0.4, only materials with a roughness value of 0.4 or below reflect the scene. The default value is 0.45.
        /// </summary>
        [DefaultValue(0.45f), Limit(0, 1.0f, 0.01f), EditorOrder(4), PostProcessSetting((int)ScreenSpaceReflectionsSettingsOverride.RoughnessThreshold)]
        [Tooltip("The maximum amount of roughness a material must have to reflect the scene. For example, if this value is set to 0.4, only materials with a roughness value of 0.4 or below reflect the scene. The default value is 0.45.")]
        public float RoughnessThreshold;

        /// <summary>
        /// The offset of the raycast origin. Lower values produce more correct reflection placement, but produce more artifacts. We recommend values of 0.3 or lower. The default value is 0.1.
        /// </summary>
        [DefaultValue(0.1f), Limit(0, 10.0f, 0.01f), EditorOrder(5), PostProcessSetting((int)ScreenSpaceReflectionsSettingsOverride.WorldAntiSelfOcclusionBias)]
        [Tooltip("The offset of the raycast origin. Lower values produce more correct reflection placement, but produce more artifacts. We recommend values of 0.3 or lower. The default value is 0.1.")]
        public float WorldAntiSelfOcclusionBias;

        /// <summary>
        /// The raycast resolution. Full gives better quality, but half improves performance. The default value is half.
        /// </summary>
        [DefaultValue(ResolutionMode.Full), EditorOrder(6), PostProcessSetting((int)ScreenSpaceReflectionsSettingsOverride.ResolvePassResolution)]
        [Tooltip("The raycast resolution. Full gives better quality, but half improves performance. The default value is half.")]
        public ResolutionMode ResolvePassResolution;

        /// <summary>
        /// The number of rays used to resolve the reflection color. Higher values provide better quality but reduce effect performance. Default value is 4. Use 1 for the highest speed.
        /// </summary>
        [DefaultValue(4), Limit(1, 8), EditorOrder(7), PostProcessSetting((int)ScreenSpaceReflectionsSettingsOverride.ResolveSamples)]
        [Tooltip("The number of rays used to resolve the reflection color. Higher values provide better quality but reduce effect performance. Default value is 4. Use 1 for the highest speed.")]
        public int ResolveSamples;

        /// <summary>
        /// The point at which the far edges of the reflection begin to fade. Has no effect on performance. The default value is 0.1.
        /// </summary>
        [DefaultValue(0.1f), Limit(0, 1.0f, 0.02f), EditorOrder(8), PostProcessSetting((int)ScreenSpaceReflectionsSettingsOverride.EdgeFadeFactor)]
        [Tooltip("The point at which the far edges of the reflection begin to fade. Has no effect on performance. The default value is 0.1.")]
        public float EdgeFadeFactor;

        /// <summary>
        /// The effect fade out end distance from camera (in world units).
        /// </summary>
        [DefaultValue(5000.0f), Limit(0), EditorOrder(9), PostProcessSetting((int)ScreenSpaceReflectionsSettingsOverride.FadeOutDistance)]
        [Tooltip("The effect fade out end distance from camera (in world units).")]
        public float FadeOutDistance;

        /// <summary>
        /// The effect fade distance (in world units). Defines the size of the effect fade from fully visible to fully invisible at FadeOutDistance.
        /// </summary>
        [DefaultValue(500.0f), Limit(0), EditorOrder(10), PostProcessSetting((int)ScreenSpaceReflectionsSettingsOverride.FadeDistance)]
        [Tooltip("The effect fade distance (in world units). Defines the size of the effect fade from fully visible to fully invisible at FadeOutDistance.")]
        public float FadeDistance;

        /// <summary>
        /// "The input color buffer downscale mode that uses blurred mipmaps when resolving the reflection color. Produces more realistic results by blurring distant parts of reflections in rough (low-gloss) materials. It also improves performance on most platforms but uses more memory.
        /// </summary>
        [DefaultValue(true), EditorOrder(11), PostProcessSetting((int)ScreenSpaceReflectionsSettingsOverride.UseColorBufferMips), EditorDisplay(null, "Use Color Buffer Mips")]
        [Tooltip("\"The input color buffer downscale mode that uses blurred mipmaps when resolving the reflection color. Produces more realistic results by blurring distant parts of reflections in rough (low-gloss) materials. It also improves performance on most platforms but uses more memory.")]
        public bool UseColorBufferMips;

        /// <summary>
        /// If checked, enables the temporal pass. Reduces noise, but produces an animated "jittering" effect that's sometimes noticeable. If disabled, the properties below have no effect.
        /// </summary>
        [DefaultValue(true), EditorOrder(12), PostProcessSetting((int)ScreenSpaceReflectionsSettingsOverride.TemporalEffect), EditorDisplay(null, "Enable Temporal Effect")]
        [Tooltip("If checked, enables the temporal pass. Reduces noise, but produces an animated \"jittering\" effect that's sometimes noticeable. If disabled, the properties below have no effect.")]
        public bool TemporalEffect;

        /// <summary>
        /// The intensity of the temporal effect. Lower values produce reflections faster, but more noise. The default value is 8.
        /// </summary>
        [DefaultValue(8.0f), Limit(0, 20.0f, 0.5f), EditorOrder(13), PostProcessSetting((int)ScreenSpaceReflectionsSettingsOverride.TemporalScale)]
        [Tooltip("The intensity of the temporal effect. Lower values produce reflections faster, but more noise. The default value is 8.")]
        public float TemporalScale;

        /// <summary>
        /// Defines how quickly reflections blend between the reflection in the current frame and the history buffer. Lower values produce reflections faster, but with more jittering. If the camera in your game doesn't move much, we recommend values closer to 1. The default value is 0.8.
        /// </summary>
        [DefaultValue(0.8f), Limit(0.05f, 1.0f, 0.01f), EditorOrder(14), PostProcessSetting((int)ScreenSpaceReflectionsSettingsOverride.TemporalResponse)]
        [Tooltip("Defines how quickly reflections blend between the reflection in the current frame and the history buffer. Lower values produce reflections faster, but with more jittering. If the camera in your game doesn't move much, we recommend values closer to 1. The default value is 0.8.")]
        public float TemporalResponse;
    }
}

namespace FlaxEngine
{
    /// <summary>
    /// The structure members override flags.
    /// </summary>
    [Flags]
    [Tooltip("The structure members override flags.")]
    public enum AntiAliasingSettingsOverride : int
    {
        /// <summary>
        /// None properties.
        /// </summary>
        [Tooltip("None properties.")]
        None = 0,

        /// <summary>
        /// Overrides <see cref="AntiAliasingSettings.Mode"/> property.
        /// </summary>
        [Tooltip("Overrides <see cref=\"AntiAliasingSettings.Mode\"/> property.")]
        Mode = 1 << 0,

        /// <summary>
        /// Overrides <see cref="AntiAliasingSettings.TAA_JitterSpread"/> property.
        /// </summary>
        [Tooltip("Overrides <see cref=\"AntiAliasingSettings.TAA_JitterSpread\"/> property.")]
        TAA_JitterSpread = 1 << 1,

        /// <summary>
        /// Overrides <see cref="AntiAliasingSettings.TAA_Sharpness"/> property.
        /// </summary>
        [Tooltip("Overrides <see cref=\"AntiAliasingSettings.TAA_Sharpness\"/> property.")]
        TAA_Sharpness = 1 << 2,

        /// <summary>
        /// Overrides <see cref="AntiAliasingSettings.TAA_StationaryBlending"/> property.
        /// </summary>
        [Tooltip("Overrides <see cref=\"AntiAliasingSettings.TAA_StationaryBlending\"/> property.")]
        TAA_StationaryBlending = 1 << 3,

        /// <summary>
        /// Overrides <see cref="AntiAliasingSettings.TAA_MotionBlending"/> property.
        /// </summary>
        [Tooltip("Overrides <see cref=\"AntiAliasingSettings.TAA_MotionBlending\"/> property.")]
        TAA_MotionBlending = 1 << 4,

        /// <summary>
        /// All properties.
        /// </summary>
        [Tooltip("All properties.")]
        All = Mode | TAA_JitterSpread | TAA_Sharpness | TAA_StationaryBlending | TAA_MotionBlending,
    }
}

namespace FlaxEngine
{
    /// <summary>
    /// Contains settings for Anti Aliasing effect rendering.
    /// </summary>
    [Tooltip("Contains settings for Anti Aliasing effect rendering.")]
    [StructLayout(LayoutKind.Sequential)]
    public unsafe partial struct AntiAliasingSettings
    {
        /// <summary>
        /// The flags for overriden properties.
        /// </summary>
        [HideInEditor]
        [Tooltip("The flags for overriden properties.")]
        public AntiAliasingSettingsOverride OverrideFlags;

        /// <summary>
        /// The anti-aliasing effect mode.
        /// </summary>
        [DefaultValue(AntialiasingMode.FastApproximateAntialiasing), EditorOrder(0), PostProcessSetting((int)AntiAliasingSettingsOverride.Mode)]
        [Tooltip("The anti-aliasing effect mode.")]
        public AntialiasingMode Mode;

        /// <summary>
        /// The diameter (in texels) inside which jitter samples are spread. Smaller values result in crisper but more aliased output, while larger values result in more stable but blurrier output.
        /// </summary>
        [DefaultValue(0.75f), Limit(0.1f, 1f, 0.001f), EditorOrder(1), PostProcessSetting((int)AntiAliasingSettingsOverride.TAA_JitterSpread), EditorDisplay(null, "TAA Jitter Spread")]
        [Tooltip("The diameter (in texels) inside which jitter samples are spread. Smaller values result in crisper but more aliased output, while larger values result in more stable but blurrier output.")]
        public float TAA_JitterSpread;

        /// <summary>
        /// Controls the amount of sharpening applied to the color buffer. TAA can induce a slight loss of details in high frequency regions. Sharpening alleviates this issue. High values may introduce dark-border artifacts.
        /// </summary>
        [DefaultValue(0f), Limit(0, 3f, 0.001f), EditorOrder(2), PostProcessSetting((int)AntiAliasingSettingsOverride.TAA_Sharpness), EditorDisplay(null, "TAA Sharpness")]
        [Tooltip("Controls the amount of sharpening applied to the color buffer. TAA can induce a slight loss of details in high frequency regions. Sharpening alleviates this issue. High values may introduce dark-border artifacts.")]
        public float TAA_Sharpness;

        /// <summary>
        /// The blend coefficient for stationary fragments. Controls the percentage of history sample blended into final color for fragments with minimal active motion.
        /// </summary>
        [DefaultValue(0.95f), Limit(0, 0.99f, 0.001f), EditorOrder(3), PostProcessSetting((int)AntiAliasingSettingsOverride.TAA_StationaryBlending), EditorDisplay(null, "TAA Stationary Blending")]
        [Tooltip("The blend coefficient for stationary fragments. Controls the percentage of history sample blended into final color for fragments with minimal active motion.")]
        public float TAA_StationaryBlending;

        /// <summary>
        /// The blending coefficient for moving fragments. Controls the percentage of history sample blended into the final color for fragments with significant active motion.
        /// </summary>
        [DefaultValue(0.4f), Limit(0, 0.99f, 0.001f), EditorOrder(4), PostProcessSetting((int)AntiAliasingSettingsOverride.TAA_MotionBlending), EditorDisplay(null, "TAA Motion Blending")]
        [Tooltip("The blending coefficient for moving fragments. Controls the percentage of history sample blended into the final color for fragments with significant active motion.")]
        public float TAA_MotionBlending;
    }
}

namespace FlaxEngine
{
    /// <summary>
    /// Contains settings for custom PostFx materials rendering.
    /// </summary>
    [Tooltip("Contains settings for custom PostFx materials rendering.")]
    [StructLayout(LayoutKind.Sequential)]
    public unsafe partial struct PostFxMaterialsSettings
    {
        /// <summary>
        /// The post-process materials collection for rendering (fixed capacity).
        /// </summary>
        [EditorDisplay(null, EditorDisplayAttribute.InlineStyle)]
        [Tooltip("The post-process materials collection for rendering (fixed capacity).")]
        public MaterialBase[] Materials;
    }
}

namespace FlaxEngine
{
    /// <summary>
    /// Contains settings for rendering advanced visual effects and post effects.
    /// </summary>
    [Tooltip("Contains settings for rendering advanced visual effects and post effects.")]
    [StructLayout(LayoutKind.Sequential)]
    public unsafe partial struct PostProcessSettings
    {
        /// <summary>
        /// The ambient occlusion effect settings.
        /// </summary>
        [Tooltip("The ambient occlusion effect settings.")]
        public AmbientOcclusionSettings AmbientOcclusion;

        /// <summary>
        /// The bloom effect settings.
        /// </summary>
        [Tooltip("The bloom effect settings.")]
        public BloomSettings Bloom;

        /// <summary>
        /// The tone mapping effect settings.
        /// </summary>
        [Tooltip("The tone mapping effect settings.")]
        public ToneMappingSettings ToneMapping;

        /// <summary>
        /// The color grading effect settings.
        /// </summary>
        [Tooltip("The color grading effect settings.")]
        public ColorGradingSettings ColorGrading;

        /// <summary>
        /// The eye adaptation effect settings.
        /// </summary>
        [Tooltip("The eye adaptation effect settings.")]
        public EyeAdaptationSettings EyeAdaptation;

        /// <summary>
        /// The camera artifacts effect settings.
        /// </summary>
        [Tooltip("The camera artifacts effect settings.")]
        public CameraArtifactsSettings CameraArtifacts;

        /// <summary>
        /// The lens flares effect settings.
        /// </summary>
        [Tooltip("The lens flares effect settings.")]
        public LensFlaresSettings LensFlares;

        /// <summary>
        /// The depth of field effect settings.
        /// </summary>
        [Tooltip("The depth of field effect settings.")]
        public DepthOfFieldSettings DepthOfField;

        /// <summary>
        /// The motion blur effect settings.
        /// </summary>
        [Tooltip("The motion blur effect settings.")]
        public MotionBlurSettings MotionBlur;

        /// <summary>
        /// The screen space reflections effect settings.
        /// </summary>
        [Tooltip("The screen space reflections effect settings.")]
        public ScreenSpaceReflectionsSettings ScreenSpaceReflections;

        /// <summary>
        /// The anti-aliasing effect settings.
        /// </summary>
        [Tooltip("The anti-aliasing effect settings.")]
        public AntiAliasingSettings AntiAliasing;

        /// <summary>
        /// The PostFx materials rendering settings.
        /// </summary>
        [Tooltip("The PostFx materials rendering settings.")]
        public PostFxMaterialsSettings PostFxMaterials;
    }
}
