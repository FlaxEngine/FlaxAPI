////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

namespace FlaxEngine.Rendering
{
    /// <summary>
    /// Primitives culling mode.
    /// </summary>
    public enum CullMode : byte
    {
        /// <summary>
        /// Cull back-facing geometry.
        /// </summary>
        Normal = 3,

        /// <summary>
        /// Cull front-facing geometry.
        /// </summary>
        Inverted = 2,

        /// <summary>
        /// Disable culling.
        /// </summary>
        TwoSided = 1,
    }
}
