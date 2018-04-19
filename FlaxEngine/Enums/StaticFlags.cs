// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System;

namespace FlaxEngine
{
    /// <summary>
    /// Static flags for the actor
    /// </summary>
    [Flags]
    public enum StaticFlags
    {
        /// <summary>
        /// Non-static object
        /// </summary>
        None = 0,

        /// <summary>
        /// Object is considered to be static for reflection probes offline caching
        /// </summary>
        ReflectionProbe = 1 << 0,

        /// <summary>
        /// Object is considered to be static for static lightmaps
        /// </summary>
        Lightmap = 1 << 1,

        /// <summary>
        /// Object is considered to have static transformation in space (no dynamic physics and any movement at runtime)
        /// </summary>
        Transform = 1 << 2,

        /// <summary>
        /// Objects is fully static on the scene
        /// </summary>
        FullyStatic = Transform | ReflectionProbe | Lightmap,
    };
}
