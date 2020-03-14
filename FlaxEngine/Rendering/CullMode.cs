// Copyright (c) 2012-2020 Wojciech Figat. All rights reserved.

namespace FlaxEngine
{
    /// <summary>
    /// Primitives culling mode.
    /// </summary>
    public enum CullMode : byte
    {
        /// <summary>
        /// Cull back-facing geometry.
        /// </summary>
        Normal = 0,

        /// <summary>
        /// Cull front-facing geometry.
        /// </summary>
        Inverted = 1,

        /// <summary>
        /// Disable culling.
        /// </summary>
        TwoSided = 2,
    }
}
