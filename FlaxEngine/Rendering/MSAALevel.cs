// Copyright (c) 2012-2020 Wojciech Figat. All rights reserved.

namespace FlaxEngine
{
    /// <summary>
    /// Multisample count level.
    /// </summary>
    public enum MSAALevel
    {
        /// <summary>
        /// Disabled multisampling.
        /// </summary>
        [Tooltip("Disabled multisampling.")]
        None = 1,

        /// <summary>
        /// Two samples per pixel.
        /// </summary>
        [Tooltip("Two samples per pixel.")]
        X2 = 2,

        /// <summary>
        /// Four samples per pixel.
        /// </summary>
        [Tooltip("Four samples per pixel.")]
        X4 = 4,

        /// <summary>
        /// Eight samples per pixel.
        /// </summary>
        [Tooltip("Eight samples per pixel.")]
        X8 = 8
    }
}
