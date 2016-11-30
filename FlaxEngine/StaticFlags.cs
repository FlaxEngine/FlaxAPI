// Flax Engine scripting API

namespace FlaxEngine
{
    /// <summary>
    /// Static flags for the actor
    /// </summary>
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
        /// Objects is fully static on the scene
        /// </summary>
        FullyStatic = ReflectionProbe | Lightmap,

        /// <summary>
        /// Maximum value of the enum (force to int)
        /// </summary>
        Max = 1 << 31,
    };
}
