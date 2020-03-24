// This code was auto-generated. Do not modify it.

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace FlaxEngine
{
    /// <summary>
    /// Describes lightmap generation options
    /// </summary>
    [Tooltip("Describes lightmap generation options")]
    [StructLayout(LayoutKind.Sequential)]
    public unsafe partial struct LightmapSettings
    {
        /// <summary>
        /// Controls how much all lights will contribute indirect lighting.
        /// </summary>
        [EditorOrder(0), Limit(0, 100.0f, 0.1f)]
        [Tooltip("Controls how much all lights will contribute indirect lighting.")]
        public float IndirectLightingIntensity;

        /// <summary>
        /// Global scale for objects in lightmap to increase quality
        /// </summary>
        [EditorOrder(10), Limit(0, 100.0f, 0.1f)]
        [Tooltip("Global scale for objects in lightmap to increase quality")]
        public float GlobalObjectsScale;

        /// <summary>
        /// Amount of pixels space between charts in lightmap atlas
        /// </summary>
        [EditorOrder(20), Limit(0, 16, 0.1f)]
        [Tooltip("Amount of pixels space between charts in lightmap atlas")]
        public int ChartsPadding;

        /// <summary>
        /// Single lightmap atlas size (width and height in pixels)
        /// </summary>
        [EditorOrder(30)]
        [Tooltip("Single lightmap atlas size (width and height in pixels)")]
        public AtlasSizes AtlasSize;

        /// <summary>
        /// Amount of indirect light GI bounce passes
        /// </summary>
        [EditorOrder(40), Limit(1, 16, 0.1f)]
        [Tooltip("Amount of indirect light GI bounce passes")]
        public int BounceCount;

        /// <summary>
        /// Enable/disable rendering static light for geometry with missing or empty material slots
        /// </summary>
        [EditorOrder(50)]
        [Tooltip("Enable/disable rendering static light for geometry with missing or empty material slots")]
        public bool UseGeometryWithNoMaterials;

        /// <summary>
        /// GI quality (range  [0;100])
        /// </summary>
        [EditorOrder(60), Limit(0, 100, 0.1f)]
        [Tooltip("GI quality (range  [0;100])")]
        public int Quality;

        /// <summary>
        /// Lightmap atlas sizes (in pixels).
        /// </summary>
        [Tooltip("Lightmap atlas sizes (in pixels).")]
        public enum AtlasSizes
        {
            /// <summary>
            /// 64x64
            /// </summary>
            [Tooltip("64x64")]
            _64 = 64,

            /// <summary>
            /// 128x128
            /// </summary>
            [Tooltip("128x128")]
            _128 = 128,

            /// <summary>
            /// 256x256
            /// </summary>
            [Tooltip("256x256")]
            _256 = 256,

            /// <summary>
            /// 512x512
            /// </summary>
            [Tooltip("512x512")]
            _512 = 512,

            /// <summary>
            /// 1024x1024
            /// </summary>
            [Tooltip("1024x1024")]
            _1024 = 1024,

            /// <summary>
            /// 2048x2048
            /// </summary>
            [Tooltip("2048x2048")]
            _2048 = 2048,

            /// <summary>
            /// 4096x4096
            /// </summary>
            [Tooltip("4096x4096")]
            _4096 = 4096,
        }
    }
}
