// This code was auto-generated. Do not modify it.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace FlaxEngine
{
    /// <summary>
    /// Static flags for the actor object.
    /// </summary>
    [Flags]
    [Tooltip("Static flags for the actor object.")]
    public enum StaticFlags
    {
        /// <summary>
        /// Non-static object.
        /// </summary>
        [Tooltip("Non-static object.")]
        None = 0,

        /// <summary>
        /// Object is considered to be static for reflection probes offline caching.
        /// </summary>
        [Tooltip("Object is considered to be static for reflection probes offline caching.")]
        ReflectionProbe = 1 << 0,

        /// <summary>
        /// Object is considered to be static for static lightmaps.
        /// </summary>
        [Tooltip("Object is considered to be static for static lightmaps.")]
        Lightmap = 1 << 1,

        /// <summary>
        /// Object is considered to have static transformation in space (no dynamic physics and any movement at runtime).
        /// </summary>
        [Tooltip("Object is considered to have static transformation in space (no dynamic physics and any movement at runtime).")]
        Transform = 1 << 2,

        /// <summary>
        /// Object is considered to affect navigation (static occluder or walkable surface).
        /// </summary>
        [Tooltip("Object is considered to affect navigation (static occluder or walkable surface).")]
        Navigation = 1 << 3,

        /// <summary>
        /// Objects is fully static on the scene.
        /// </summary>
        [Tooltip("Objects is fully static on the scene.")]
        FullyStatic = Transform | ReflectionProbe | Lightmap | Navigation,

        /// <summary>
        /// Maximum value of the enum (force to int).
        /// </summary>
        [HideInEditor]
        [Tooltip("Maximum value of the enum (force to int).")]
        MAX = 1 << 31,
    }
}

namespace FlaxEngine
{
    /// <summary>
    /// Object hide state description flags. Control object appearance.
    /// </summary>
    [Flags]
    [Tooltip("Object hide state description flags. Control object appearance.")]
    public enum HideFlags
    {
        /// <summary>
        /// The default state.
        /// </summary>
        [Tooltip("The default state.")]
        None = 0,

        /// <summary>
        /// The object will not be visible in the hierarchy.
        /// </summary>
        [Tooltip("The object will not be visible in the hierarchy.")]
        HideInHierarchy = 1,

        /// <summary>
        /// The object will not be saved.
        /// </summary>
        [Tooltip("The object will not be saved.")]
        DontSave = 2,

        /// <summary>
        /// The object will not selectable in the editor viewport.
        /// </summary>
        [Tooltip("The object will not selectable in the editor viewport.")]
        DontSelect = 4,

        /// <summary>
        /// The fully hidden object flags mask.
        /// </summary>
        [Tooltip("The fully hidden object flags mask.")]
        FullyHidden = HideInHierarchy | DontSave | DontSelect,
    }
}
