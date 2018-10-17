// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System;
using System.Runtime.CompilerServices;
using FlaxEngine.Rendering;

namespace FlaxEngine
{
    /// <summary>
    /// Terrain heightmap with normal map format. Defines the quality of the terrain data.
    /// </summary>
    /// <remarks>
    /// Terrain data per vertex (stored in texture texels): R: Height, G: NormalX, B: NormalY, A: Visibility. 
    /// </remarks>
    public enum TerrainHeightmapFormat : int
    {
        /// <summary>
        /// Red:8 bit, Green:8 bit, Blue:8 bit, Alpha:8 bit, uncompressed, raw data. See <see cref="PixelFormat.R8G8B8A8_UNorm"/>.
        /// </summary>
        R8G8B8A8_Raw = 0,

        /// <summary>
        /// Red:16 bit, Green:16 bit, Blue:16 bit, Alpha:16 bit, uncompressed, raw data. See <see cref="PixelFormat.R16G16B16A16_UNorm"/>.
        /// </summary>
        R16G16B16A16_Raw = 0,
    };

    public sealed partial class Terrain
    {
        /// <summary>
        /// Setups the terrain patch using the specified heightmap data.
        /// </summary>
        /// <param name="x">The patch location x.</param>
        /// <param name="z">The patch location z.</param>
        /// <param name="format">The heightmap storage format.</param>
        /// <param name="heightMap">The height map. Each array item contains a height value. It should has size equal (chunkSize+1)*(chunkSize+1)*16.</param>
        /// <param name="visibilityMap">The visibility map (optional). Normalized to 0-1 range values with visibility per-vertex. Must match the heightmap dimensions.</param>
        /// <param name="forceUseVirtualStorage">If set to <c>true</c> patch will use virtual storage by force. Otherwise it can use normal texture asset storage on drive (valid only during Editor). Runtime-created terrain can only use virtual storage (in RAM).</param>
        /// <returns>True if failed, otherwise false.</returns>
        public unsafe void SetupPatch(int x, int z, TerrainHeightmapFormat format, float[] heightMap, float[] visibilityMap = null, bool forceUseVirtualStorage = false)
        {
            fixed (float* heightMapPtr = heightMap)
            {
                fixed (float* visibilityMapPtr = visibilityMap)
                {
                    SetupPatch(x, z, format, heightMap.Length, heightMapPtr, visibilityMapPtr, forceUseVirtualStorage);
                }
            }
        }

        /// <summary>
        /// Setups the terrain patch using the specified heightmap data.
        /// </summary>
        /// <param name="x">The patch location x.</param>
        /// <param name="z">The patch location z.</param>
        /// <param name="format">The heightmap storage format.</param>
        /// <param name="heightMapLength">The height map array length. It must match the terrain descriptor, so it should be (chunkSize+1)*(chunkSize+1)*16. Patch is a 4 by 4 square made of chunks. Each chunk has chunkSize quads on edge.</param>
        /// <param name="heightMap">The height map. Each array item contains a height value.</param>
        /// <param name="visibilityMap">The visibility map (optional). Normalized to 0-1 range values with visibility per-vertex. Must match the heightmap dimensions.</param>
        /// <param name="forceUseVirtualStorage">If set to <c>true</c> patch will use virtual storage by force. Otherwise it can use normal texture asset storage on drive (valid only during Editor). Runtime-created terrain can only use virtual storage (in RAM).</param>
        /// <returns>True if failed, otherwise false.</returns>
        public unsafe void SetupPatch(int x, int z, TerrainHeightmapFormat format, float heightMapLength, float* heightMap, float* visibilityMap = null, bool forceUseVirtualStorage = false)
        {
            if (Internal_SetupPatch(unmanagedPtr, x, z, format, heightMapLength, heightMap, visibilityMap, forceUseVirtualStorage))
                throw new Exception("Failed to setup terrain patch. See log for more info.");
        }

        #region Internal Calls

#if !UNIT_TEST_COMPILANT
        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern unsafe bool Internal_SetupPatch(IntPtr obj, int x, int z, TerrainHeightmapFormat format, float heightMapLength, float* heightMap, float* visibilityMap, bool forceUseVirtualStorage);
#endif

        #endregion
    }
}
