////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System.Collections.Generic;

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
        Hexagon,

        /// <summary>
        /// The octagon shape.
        /// </summary>
        Octagon,

        /// <summary>
        /// The circle shape.
        /// </summary>
        Circle,

        /// <summary>
        /// The cross shape.
        /// </summary>
        Cross,

        /// <summary>
        /// The custom texture shape.
        /// </summary>
        Custom
    }

    /// <summary>
    /// Contains settings for rendering advanced visual effects and post effects.
    /// </summary>
    public sealed class PostProcessSettings
    {
        // TODO: get stuff from c++ into here
    }
}
