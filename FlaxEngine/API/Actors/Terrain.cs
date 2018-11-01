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
        R16G16B16A16_Raw = 1,
    };

    public sealed partial class Terrain
    {
        /// <summary>
        /// Setups the terrain patch using the specified heightmap data.
        /// </summary>
        /// <param name="x">The patch location x.</param>
        /// <param name="z">The patch location z.</param>
        /// <param name="format">The heightmap storage format.</param>
        /// <param name="heightMap">The height map. Each array item contains a height value (2D inlined array). It should has size equal (chunkSize*4+1)^2.</param>
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
        /// <param name="heightMapLength">The height map array length. It must match the terrain descriptor, so it should be equal (chunkSize*4+1)^2. Patch is a 4 by 4 square made of chunks. Each chunk has chunkSize quads on edge.</param>
        /// <param name="heightMap">The height map. Each array item contains a height value (2D inlined array).</param>
        /// <param name="visibilityMap">The visibility map (optional). Normalized to 0-1 range values with visibility per-vertex. Must match the heightmap dimensions.</param>
        /// <param name="forceUseVirtualStorage">If set to <c>true</c> patch will use virtual storage by force. Otherwise it can use normal texture asset storage on drive (valid only during Editor). Runtime-created terrain can only use virtual storage (in RAM).</param>
        /// <returns>True if failed, otherwise false.</returns>
        public unsafe void SetupPatch(int x, int z, TerrainHeightmapFormat format, int heightMapLength, float* heightMap, float* visibilityMap = null, bool forceUseVirtualStorage = false)
        {
            if (Internal_SetupPatch(unmanagedPtr, x, z, format, heightMapLength, heightMap, visibilityMap, forceUseVirtualStorage))
                throw new Exception("Failed to setup terrain patch. See log for more info.");
        }

        /// <summary>
        /// Performs a raycast against terrain collider.
        /// </summary>
        /// <param name="origin">The origin of the ray.</param>
        /// <param name="direction">The normalized direction of the ray.</param>
        /// <param name="maxDistance">The maximum distance the ray should check for collisions.</param>
        /// <returns>True if ray hits an object, otherwise false.</returns>
        public bool RayCast(Vector3 origin, Vector3 direction, float maxDistance = float.MaxValue)
        {
            return Internal_RayCast1(unmanagedPtr, ref origin, ref direction, maxDistance);
        }

        /// <summary>
        /// Performs a raycast against terrain collider.
        /// </summary>
        /// <param name="origin">The origin of the ray.</param>
        /// <param name="direction">The normalized direction of the ray.</param>
        /// <param name="resultHitDistance">The raycast result hit position distance from the ray origin. Valid only if raycast hits anything.</param>
        /// <param name="maxDistance">The maximum distance the ray should check for collisions.</param>
        /// <returns>True if ray hits an object, otherwise false.</returns>
        public bool RayCast(Vector3 origin, Vector3 direction, out float resultHitDistance, float maxDistance = float.MaxValue)
        {
            return Internal_RayCast2(unmanagedPtr, ref origin, ref direction, out resultHitDistance, maxDistance);
        }

        /// <summary>
        /// Performs a raycast against terrain collider, returns results in a RaycastHit structure.
        /// </summary>
        /// <param name="origin">The origin of the ray.</param>
        /// <param name="direction">The normalized direction of the ray.</param>
        /// <param name="hitInfo">The result hit information. Valid only when method returns true.</param>
        /// <param name="maxDistance">The maximum distance the ray should check for collisions.</param>
        /// <returns>True if ray hits an object, otherwise false.</returns>
        public bool RayCast(Vector3 origin, Vector3 direction, out RayCastHit hitInfo, float maxDistance = float.MaxValue)
        {
            return Internal_RayCast3(unmanagedPtr, ref origin, ref direction, out hitInfo, maxDistance);
        }

        /// <summary>
        /// Gets a point on the terrain collision surface that is closest to a given location.
        /// Can be used to find a hit location or position to apply explosion force or any other special effects.
        /// </summary>
        /// <param name="position">The position to find the closest point to it.</param>
        /// <returns>The result point on the collider that is closest to the specified location.</returns>
        public Vector3 ClosestPoint(Vector3 position)
        {
            Vector3 result;
            Internal_ClosestPoint(unmanagedPtr, ref position, out result);
            return result;
        }

        #region Internal Calls

#if !UNIT_TEST_COMPILANT
        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern unsafe bool Internal_SetupPatch(IntPtr obj, int x, int z, TerrainHeightmapFormat format, int heightMapLength, float* heightMap, float* visibilityMap, bool forceUseVirtualStorage);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_RayCast1(IntPtr obj, ref Vector3 origin, ref Vector3 direction, float maxDistance);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_RayCast2(IntPtr obj, ref Vector3 origin, ref Vector3 direction, out float resultHitDistance, float maxDistance);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_RayCast3(IntPtr obj, ref Vector3 origin, ref Vector3 direction, out RayCastHit hitInfo, float maxDistance);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_ClosestPoint(IntPtr obj, ref Vector3 position, out Vector3 result);
#endif

        #endregion
    }
}
