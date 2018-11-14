// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

namespace FlaxEditor.Surface.Archetypes
{
    /// <summary>
    /// Alpha blending modes.
    /// </summary>
    public enum AlphaBlendMode
    {
        /// <summary>
        /// Linear interpolation.
        /// </summary>
        Linear = 0,

        /// <summary>
        /// Cubic-in interpolation.
        /// </summary>
        Cubic,

        /// <summary>
        /// Hermite-Cubic.
        /// </summary>
        HermiteCubic,

        /// <summary>
        /// Sinusoidal interpolation.
        /// </summary>
        Sinusoidal,

        /// <summary>
        /// Quadratic in-out interpolation.
        /// </summary>
        QuadraticInOut,

        /// <summary>
        /// Cubic in-out interpolation.
        /// </summary>
        CubicInOut,

        /// <summary>
        /// Quartic in-out interpolation.
        /// </summary>
        QuarticInOut,

        /// <summary>
        /// Quintic in-out interpolation.
        /// </summary>
        QuinticInOut,

        /// <summary>
        /// Circular-in interpolation.
        /// </summary>
        CircularIn,

        /// <summary>
        /// Circular-out interpolation.
        /// </summary>
        CircularOut,

        /// <summary>
        /// Circular in-out interpolation.
        /// </summary>
        CircularInOut,

        /// <summary>
        /// Exponential-in interpolation.
        /// </summary>
        ExpIn,

        /// <summary>
        /// Exponential-Out interpolation.
        /// </summary>
        ExpOut,

        /// <summary>
        /// Exponential in-out interpolation.
        /// </summary>
        ExpInOut,
    }
}
