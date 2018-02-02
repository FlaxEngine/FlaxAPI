////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2018 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

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
        None = 0,

        /// <summary>
        /// Render shadows only in static views (env probes, lightmaps, etc.).
        /// </summary>
        StaticOnly,

        /// <summary>
        /// Render shados only in dynamic views (game, editor, etc.).
        /// </summary>
        DynamicOnly,

        /// <summary>
        /// Always render shadows.
        /// </summary>
        All
    }
}
