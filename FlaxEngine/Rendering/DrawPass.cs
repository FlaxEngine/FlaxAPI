// Copyright (c) 2012-2019 Wojciech Figat. All rights reserved.

using System;

namespace FlaxEngine.Rendering
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
        Depth = 1,

        /// <summary>
        /// The base pass rendering to the GBuffer (for opaque materials).
        /// </summary>
        GBuffer = 1 << 1,

        /// <summary>
        /// The forward pass rendering (for transparent materials).
        /// </summary>
        Forward = 1 << 2,

        /// <summary>
        /// The transparent objects distortion vectors rendering (with blending).
        /// </summary>
        Distortion = 1 << 3,

        /// <summary>
        /// The motion vectors (velocity) rendering pass (for movable objects).
        /// </summary>
        MotionVectors = 1 << 4,

        /// <summary>
        /// The default set of draw passes for the scene objects.
        /// </summary>
        Default = Depth | GBuffer | Forward | Distortion | MotionVectors,

        /// <summary>
        /// The all draw passes combined into a single mask.
        /// </summary>
        All = Depth | GBuffer | Forward | Distortion | MotionVectors,
    };
}
