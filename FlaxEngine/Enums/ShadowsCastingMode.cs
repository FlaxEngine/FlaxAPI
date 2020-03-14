// Copyright (c) 2012-2020 Wojciech Figat. All rights reserved.

namespace FlaxEngine
{
    /// <summary>
    /// Shadows casting modes by visual elements.
    /// </summary>
    public enum ShadowsCastingMode
    {
        /// <summary>
        /// Never render shadows.
        /// </summary>
        [Tooltip("Never render shadows.")]
        None = 0,

        /// <summary>
        /// Render shadows only in static views (env probes, lightmaps, etc.).
        /// </summary>
        [Tooltip("Render shadows only in static views (env probes, lightmaps, etc.).")]
        StaticOnly,

        /// <summary>
        /// Render shadows only in dynamic views (game, editor, etc.).
        /// </summary>
        [Tooltip("Render shadows only in dynamic views (game, editor, etc.).")]
        DynamicOnly,

        /// <summary>
        /// Always render shadows.
        /// </summary>
        [Tooltip("Always render shadows.")]
        All,
    }
}
