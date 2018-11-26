// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System;
using System.Runtime.CompilerServices;

namespace FlaxEngine
{
    public sealed partial class Terrain
    {
        /// <summary>
        /// The constant amount of units per terrain geometry vertex (can be adjusted per terrain instance using non-uniform scale factor).
        /// </summary>
        public const float UnitsPerVertex = 100.0f;

        /// <summary>
        /// The maximum amount of levels of detail for the terrain chunks.
        /// </summary>
        public const int MaxLODs = 6;

        /// <summary>
        /// The constant amount of terrain chunks per terrain patch object.
        /// </summary>
        public const int PatchChunksCount = 16;

        /// <summary>
        /// The constant amount of terrain chunks on terrain patch object edge.
        /// </summary>
        public const int PatchEdgeChunksCount = 4;

        /// <summary>
        /// The terrain splatmaps amount limit. Each splatmap can hold up to 4 layer weights.
        /// </summary>
        public const int MaxSplatmapsCount = 2;

        /// <summary>
        /// Adds the patch.
        /// </summary>
        /// <param name="patchX">The patch X location (coordinate).</param>
        /// <param name="patchZ">The patch Z location (coordinate).</param>
        public void AddPatch(int patchX, int patchZ)
        {
            var patchCoord = new Int2(patchX, patchZ);
            AddPatch(ref patchCoord);
        }

        /// <summary>
        /// Setups the terrain patch using the specified heightmap data.
        /// </summary>
        /// <param name="patchCoord">The patch location (x and z coordinates).</param>
        /// <param name="heightMap">The height map. Each array item contains a height value (2D inlined array). It should has size equal (chunkSize*4+1)^2.</param>
        /// <param name="holesMask">The holes mask (optional). Normalized to 0-1 range values with holes mask per-vertex. Must match the heightmap dimensions.</param>
        /// <param name="forceUseVirtualStorage">If set to <c>true</c> patch will use virtual storage by force. Otherwise it can use normal texture asset storage on drive (valid only during Editor). Runtime-created terrain can only use virtual storage (in RAM).</param>
        public unsafe void SetupPatchHeightMap(ref Int2 patchCoord, float[] heightMap, byte[] holesMask = null, bool forceUseVirtualStorage = false)
        {
            if (heightMap == null)
                throw new ArgumentNullException(nameof(heightMap));

            fixed (float* heightMapPtr = heightMap)
            {
                fixed (byte* holesMaskPtr = holesMask)
                {
                    SetupPatchHeightMap(ref patchCoord, heightMap.Length, heightMapPtr, holesMaskPtr, forceUseVirtualStorage);
                }
            }
        }

        /// <summary>
        /// Setups the terrain patch using the specified heightmap data.
        /// </summary>
        /// <param name="patchCoord">The patch location (x and z coordinates).</param>
        /// <param name="heightMapLength">The height map array length. It must match the terrain descriptor, so it should be equal (chunkSize*4+1)^2. Patch is a 4 by 4 square made of chunks. Each chunk has chunkSize quads on edge.</param>
        /// <param name="heightMap">The height map. Each array item contains a height value (2D inlined array).</param>
        /// <param name="holesMask">The holes mask (optional). Normalized to 0-1 range values with holes mask per-vertex. Must match the heightmap dimensions.</param>
        /// <param name="forceUseVirtualStorage">If set to <c>true</c> patch will use virtual storage by force. Otherwise it can use normal texture asset storage on drive (valid only during Editor). Runtime-created terrain can only use virtual storage (in RAM).</param>
        public unsafe void SetupPatchHeightMap(ref Int2 patchCoord, int heightMapLength, float* heightMap, byte* holesMask = null, bool forceUseVirtualStorage = false)
        {
            if (Internal_SetupPatchHeightMap(unmanagedPtr, ref patchCoord, heightMapLength, heightMap, holesMask, forceUseVirtualStorage))
                throw new Exception("Failed to setup terrain patch. See log for more info.");
        }

        /// <summary>
        /// Setups the terrain patch using the specified heightmap data.
        /// </summary>
        /// <param name="patchCoord">The patch location (x and z coordinates).</param>
        /// <param name="index">The zero-based index of the splatmap texture.</param>
        /// <param name="splatMap">The splat map. Each array item contains 4 layer weights. It must match the terrain descriptor, so it should be (chunkSize*4+1)^2. Patch is a 4 by 4 square made of chunks. Each chunk has chunkSize quads on edge.</param>
        /// <param name="forceUseVirtualStorage">If set to <c>true</c> patch will use virtual storage by force. Otherwise it can use normal texture asset storage on drive (valid only during Editor). Runtime-created terrain can only use virtual storage (in RAM).</param>
        public unsafe void SetupPatchSplatMap(ref Int2 patchCoord, int index, Color32[] splatMap, bool forceUseVirtualStorage = false)
        {
            if (splatMap == null)
                throw new ArgumentNullException(nameof(splatMap));

            fixed (Color32* splatMapPtr = splatMap)
            {
                SetupPatchSplatMap(ref patchCoord, index, splatMap.Length, splatMapPtr, forceUseVirtualStorage);
            }
        }

        /// <summary>
        /// Setups the terrain patch layer weights using the specified splatmaps data.
        /// </summary>
        /// <param name="patchCoord">The patch location (x and z coordinates).</param>
        /// <param name="index">The zero-based index of the splatmap texture.</param>
        /// <param name="splatMapLength">The splatmap map array length. It must match the terrain descriptor, so it should be (chunkSize*4+1)^2. Patch is a 4 by 4 square made of chunks. Each chunk has chunkSize quads on edge.</param>
        /// <param name="splatMap">The splat map. Each array item contains 4 layer weights.</param>
        /// <param name="forceUseVirtualStorage">If set to <c>true</c> patch will use virtual storage by force. Otherwise it can use normal texture asset storage on drive (valid only during Editor). Runtime-created terrain can only use virtual storage (in RAM).</param>
        public unsafe void SetupPatchSplatMap(ref Int2 patchCoord, int index, int splatMapLength, Color32* splatMap, bool forceUseVirtualStorage = false)
        {
            if (Internal_SetupPatchSplatMap(unmanagedPtr, ref patchCoord, index, splatMapLength, splatMap, forceUseVirtualStorage))
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

        /// <summary>
        /// Gets the chunk material that overrides the terrain default one.
        /// </summary>
        /// <param name="patchCoord">The patch coordinates (x and z).</param>
        /// <param name="chunkCoord">The chunk coordinates (x and z).</param>
        /// <returns>The value.</returns>
        public MaterialBase GetChunkOverrideMaterial(ref Int2 patchCoord, ref Int2 chunkCoord)
        {
            return Internal_GetChunkOverrideMaterial(unmanagedPtr, ref patchCoord, ref chunkCoord);
        }

        /// <summary>
        /// Sets the chunk material to override the terrain default one.
        /// </summary>
        /// <param name="patchCoord">The patch coordinates (x and z).</param>
        /// <param name="chunkCoord">The chunk coordinates (x and z).</param>
        /// <param name="value">The value to set.</param>
        public void SetChunkOverrideMaterial(ref Int2 patchCoord, ref Int2 chunkCoord, MaterialBase value)
        {
            Internal_SetChunkOverrideMaterial(unmanagedPtr, ref patchCoord, ref chunkCoord, Object.GetUnmanagedPtr(value));
        }

        #region Internal Calls

#if !UNIT_TEST_COMPILANT
        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern unsafe bool Internal_SetupPatchHeightMap(IntPtr obj, ref Int2 patchCoord, int heightMapLength, float* heightMap, byte* holesMask, bool forceUseVirtualStorage);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern unsafe bool Internal_SetupPatchSplatMap(IntPtr obj, ref Int2 patchCoord, int index, int splatMapLength, Color32* splatMap, bool forceUseVirtualStorage);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_RayCast1(IntPtr obj, ref Vector3 origin, ref Vector3 direction, float maxDistance);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_RayCast2(IntPtr obj, ref Vector3 origin, ref Vector3 direction, out float resultHitDistance, float maxDistance);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_RayCast3(IntPtr obj, ref Vector3 origin, ref Vector3 direction, out RayCastHit hitInfo, float maxDistance);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_ClosestPoint(IntPtr obj, ref Vector3 position, out Vector3 result);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern MaterialBase Internal_GetChunkOverrideMaterial(IntPtr obj, ref Int2 patchCoord, ref Int2 chunkCoord);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetChunkOverrideMaterial(IntPtr obj, ref Int2 patchCoord, ref Int2 chunkCoord, IntPtr value);
#endif

        #endregion
    }
}
