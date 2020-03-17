// This code was auto-generated. Do not modify it.

using System;
using System.Collections.Generic;
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
