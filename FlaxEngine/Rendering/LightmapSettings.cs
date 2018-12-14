// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

// ReSharper disable InconsistentNaming

using System.Runtime.InteropServices;

namespace FlaxEngine.Rendering
{
    /// <summary>
    /// Describes lightmap generation options
    /// </summary>
    public struct LightmapSettings
    {
        /// <summary>
        /// Lightmap atlas sizes to use
        /// </summary>
        public enum AtlasSizes
        {
            /// <summary>
            /// 64x64
            /// </summary>
            _64 = 64,

            /// <summary>
            /// 128x128
            /// </summary>
            _128 = 128,

            /// <summary>
            /// 256x256
            /// </summary>
            _256 = 256,

            /// <summary>
            /// 512x512
            /// </summary>
            _512 = 512,

            /// <summary>
            /// 1024x1024
            /// </summary>
            _1024 = 1024,

            /// <summary>
            /// 2048x2048
            /// </summary>
            _2048 = 2048,

            /// <summary>
            /// 4096x4096
            /// </summary>
            _4096 = 4096,
        }

        /// <summary>
        /// Controls how much all lights will contribute indirect lighting.
        /// </summary>
        [EditorOrder(0), Limit(0, 100.0f, 0.1f), Tooltip("Controls how much all lights will contribute indirect lighting.")]
        public float IndirectLightingIntensity;

        /// <summary>
        /// Global scale for objects in lightmap to increase quality
        /// </summary>
        [EditorOrder(10), Limit(0, 100.0f, 0.1f), Tooltip("Global scale for objects in lightmap to increase quality")]
        public float GlobalObjectsScale;

        /// <summary>
        /// Amount of pixels space between charts in lightmap atlas
        /// </summary>
        [EditorOrder(20), Limit(0, 16, 0.1f), Tooltip("Amount of pixels space between charts in lightmap atlas")]
        public int ChartsPadding;

        /// <summary>
        /// Single lightmap atlas size (width and height)
        /// </summary>
        [EditorOrder(30), Tooltip("Single lightmap atlas size (width and height)")]
        public AtlasSizes AtlasSize;

        /// <summary>
        /// Amount of indirect light GI bounce passes
        /// </summary>
        [EditorOrder(40), Limit(1, 16, 0.1f), Tooltip("Amount of indirect light GI bounce passes")]
        public int BounceCount;

        /// <summary>
        /// Enable/disable rendering static light for geometry with missing or empty material slots
        /// </summary>
        [EditorOrder(50), Tooltip("Enable/disable rendering static light for geometry with missing or empty material slots")]
        public bool UseGeometryWithNoMaterials;

        /// <summary>
        /// GI quality (range  [0;100])
        /// </summary>
        [EditorOrder(60), Limit(0, 100, 0.1f), Tooltip("GI quality (range  [0;100])")]
        public int Quality;

        [StructLayout(LayoutKind.Sequential)]
        internal struct Internal
        {
            public float IndirectLightingIntensity;
            public float GlobalObjectsScale;
            public int ChartsPadding;
            public int AtlasSize;
            public int BounceCount;
            public byte UseGeometryWithNoMaterials;
            public int Quality;
        }

        internal static LightmapSettings FromInternal(ref Internal data)
        {
            return new LightmapSettings
            {
                IndirectLightingIntensity = data.IndirectLightingIntensity,
                GlobalObjectsScale = data.GlobalObjectsScale,
                ChartsPadding = data.ChartsPadding,
                AtlasSize = (AtlasSizes)data.AtlasSize,
                BounceCount = data.BounceCount,
                UseGeometryWithNoMaterials = data.UseGeometryWithNoMaterials != 0,
                Quality = data.Quality,
            };
        }

        internal static Internal ToInternal(ref LightmapSettings data)
        {
            return new Internal
            {
                IndirectLightingIntensity = data.IndirectLightingIntensity,
                GlobalObjectsScale = data.GlobalObjectsScale,
                ChartsPadding = data.ChartsPadding,
                AtlasSize = (int)data.AtlasSize,
                BounceCount = data.BounceCount,
                UseGeometryWithNoMaterials = (byte)(data.UseGeometryWithNoMaterials ? 1 : 0),
                Quality = data.Quality,
            };
        }
    }
}
