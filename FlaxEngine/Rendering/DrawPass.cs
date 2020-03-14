// Copyright (c) 2012-2020 Wojciech Figat. All rights reserved.

using System;

namespace FlaxEngine
{
    /// <summary>
    /// The objects drawing pass types. Used as a flags for objects drawing masking.
    /// </summary>
    [Flags]
    public enum DrawPass : int
    {
        /// <summary>
        /// The none.
        /// </summary>
        None = 0,

        /// <summary>
        /// The hardware depth rendering to the depth buffer (used for shadow maps rendering).
        /// </summary>
        [Tooltip("The hardware depth rendering to the depth buffer (used for shadow maps rendering).")]
        Depth = 1,

        /// <summary>
        /// The base pass rendering to the GBuffer (for opaque materials).
        /// </summary>
        [Tooltip("The base pass rendering to the GBuffer (for opaque materials).")]
        [EditorDisplay(name: "GBuffer")]
        GBuffer = 1 << 1,

        /// <summary>
        /// The forward pass rendering (for transparent materials).
        /// </summary>
        [Tooltip("The forward pass rendering (for transparent materials).")]
        Forward = 1 << 2,

        /// <summary>
        /// The transparent objects distortion vectors rendering (with blending).
        /// </summary>
        [Tooltip("The transparent objects distortion vectors rendering (with blending).")]
        Distortion = 1 << 3,

        /// <summary>
        /// The motion vectors (velocity) rendering pass (for movable objects).
        /// </summary>
        [Tooltip("The motion vectors (velocity) rendering pass (for movable objects).")]
        MotionVectors = 1 << 4,

        /// <summary>
        /// The default set of draw passes for the scene objects.
        /// </summary>
        [HideInEditor]
        Default = Depth | GBuffer | Forward | Distortion | MotionVectors,

        /// <summary>
        /// The all draw passes combined into a single mask.
        /// </summary>
        [HideInEditor]
        All = Depth | GBuffer | Forward | Distortion | MotionVectors,
    };
}
