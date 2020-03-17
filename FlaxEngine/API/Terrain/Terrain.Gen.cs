// This code was auto-generated. Do not modify it.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace FlaxEngine
{
    /// <summary>
    /// Represents a single terrain object.
    /// </summary>
    /// <seealso cref="Actor" />
    /// <seealso cref="PhysicsColliderActor" />
    [Tooltip("Represents a single terrain object.")]
    public sealed unsafe partial class Terrain : PhysicsColliderActor
    {
        private Terrain() : base()
        {
        }

        /// <summary>
        /// Creates new instance of <see cref="Terrain"/> object.
        /// </summary>
        /// <returns>The created object.</returns>
        public static Terrain New()
        {
            return Internal_Create(typeof(Terrain)) as Terrain;
        }

        /// <summary>
        /// The default material used for terrain rendering (chunks can override this).
        /// </summary>
        [EditorOrder(100), DefaultValue(null), EditorDisplay("Terrain")]
        [Tooltip("The default material used for terrain rendering (chunks can override this).")]
        public MaterialBase Material
        {
            get { return Internal_GetMaterial(unmanagedPtr); }
            set { Internal_SetMaterial(unmanagedPtr, FlaxEngine.Object.GetUnmanagedPtr(value)); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern MaterialBase Internal_GetMaterial(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetMaterial(IntPtr obj, IntPtr value);

        /// <summary>
        /// The physical material used to define the terrain collider physical properties.
        /// </summary>
        [EditorOrder(520), DefaultValue(null), Limit(-1, 100, 0.1f), EditorDisplay("Collision"), AssetReference(typeof(PhysicalMaterial), true)]
        [Tooltip("The physical material used to define the terrain collider physical properties.")]
        public JsonAsset PhysicalMaterial
        {
            get { return Internal_GetPhysicalMaterial(unmanagedPtr); }
            set { Internal_SetPhysicalMaterial(unmanagedPtr, FlaxEngine.Object.GetUnmanagedPtr(value)); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern JsonAsset Internal_GetPhysicalMaterial(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetPhysicalMaterial(IntPtr obj, IntPtr value);

        /// <summary>
        /// The draw passes to use for rendering this object.
        /// </summary>
        [EditorOrder(115), DefaultValue(DrawPass.Default), EditorDisplay("Terrain")]
        [Tooltip("The draw passes to use for rendering this object.")]
        public DrawPass DrawModes
        {
            get { return Internal_GetDrawModes(unmanagedPtr); }
            set { Internal_SetDrawModes(unmanagedPtr, value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern DrawPass Internal_GetDrawModes(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetDrawModes(IntPtr obj, DrawPass value);

        /// <summary>
        /// Gets or sets the terrain Level Of Detail bias value. Allows to increase or decrease rendered terrain quality.
        /// </summary>
        [EditorOrder(50), DefaultValue(0), Limit(-100, 100, 0.1f), EditorDisplay("Terrain", "LOD Bias")]
        [Tooltip("The terrain Level Of Detail bias value. Allows to increase or decrease rendered terrain quality.")]
        public int LODBias
        {
            get { return Internal_GetLODBias(unmanagedPtr); }
            set { Internal_SetLODBias(unmanagedPtr, value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern int Internal_GetLODBias(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetLODBias(IntPtr obj, int value);

        /// <summary>
        /// Gets or sets the terrain forced Level Of Detail index. Allows to bind the given chunks LOD to show. Value -1 disables this feature.
        /// </summary>
        [EditorOrder(60), DefaultValue(-1), Limit(-1, 100, 0.1f), EditorDisplay("Terrain", "Forced LOD")]
        [Tooltip("The terrain forced Level Of Detail index. Allows to bind the given chunks LOD to show. Value -1 disables this feature.")]
        public int ForcedLOD
        {
            get { return Internal_GetForcedLOD(unmanagedPtr); }
            set { Internal_SetForcedLOD(unmanagedPtr, value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern int Internal_GetForcedLOD(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetForcedLOD(IntPtr obj, int value);

        /// <summary>
        /// Gets or sets the terrain LODs distribution parameter. Adjusts terrain chunks transitions distances. Use lower value to increase terrain quality or higher value to increase performance. Default value is 0.75.
        /// </summary>
        [EditorOrder(70), DefaultValue(0.6f), Limit(0, 5, 0.01f), EditorDisplay("Terrain", "LOD Distribution")]
        [Tooltip("The terrain LODs distribution parameter. Adjusts terrain chunks transitions distances. Use lower value to increase terrain quality or higher value to increase performance. Default value is 0.75.")]
        public float LODDistribution
        {
            get { return Internal_GetLODDistribution(unmanagedPtr); }
            set { Internal_SetLODDistribution(unmanagedPtr, value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern float Internal_GetLODDistribution(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetLODDistribution(IntPtr obj, float value);

        /// <summary>
        /// Gets or sets the terrain scale in lightmap (applied to all the chunks). Use value higher than 1 to increase baked lighting resolution.
        /// </summary>
        [EditorOrder(110), DefaultValue(0.1f), Limit(0, 10000, 0.1f), EditorDisplay("Terrain", "Scale In Lightmap")]
        [Tooltip("The terrain scale in lightmap (applied to all the chunks). Use value higher than 1 to increase baked lighting resolution.")]
        public float ScaleInLightmap
        {
            get { return Internal_GetScaleInLightmap(unmanagedPtr); }
            set { Internal_SetScaleInLightmap(unmanagedPtr, value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern float Internal_GetScaleInLightmap(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetScaleInLightmap(IntPtr obj, float value);

        /// <summary>
        /// Gets or sets the terrain chunks bounds extent. Values used to expand terrain chunks bounding boxes. Use it when your terrain material is performing vertex offset operations on a GPU.
        /// </summary>
        [EditorOrder(120), EditorDisplay("Terrain")]
        [Tooltip("The terrain chunks bounds extent. Values used to expand terrain chunks bounding boxes. Use it when your terrain material is performing vertex offset operations on a GPU.")]
        public Vector3 BoundsExtent
        {
            get { Internal_GetBoundsExtent(unmanagedPtr, out var resultAsRef); return resultAsRef; }
            set { Internal_SetBoundsExtent(unmanagedPtr, ref value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_GetBoundsExtent(IntPtr obj, out Vector3 resultAsRef);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetBoundsExtent(IntPtr obj, ref Vector3 value);

        /// <summary>
        /// Gets or sets the terrain geometry LOD index used for collision.
        /// </summary>
        [EditorOrder(500), DefaultValue(-1), Limit(-1, 100, 0.1f), EditorDisplay("Collision", "Collision LOD")]
        [Tooltip("The terrain geometry LOD index used for collision.")]
        public int CollisionLOD
        {
            get { return Internal_GetCollisionLOD(unmanagedPtr); }
            set { Internal_SetCollisionLOD(unmanagedPtr, value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern int Internal_GetCollisionLOD(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetCollisionLOD(IntPtr obj, int value);

        /// <summary>
        /// Gets the terrain Level Of Detail count.
        /// </summary>
        [Tooltip("The terrain Level Of Detail count.")]
        public int LODCount
        {
            get { return Internal_GetLODCount(unmanagedPtr); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern int Internal_GetLODCount(IntPtr obj);

        /// <summary>
        /// Gets the terrain chunk vertices amount per edge (square).
        /// </summary>
        [Tooltip("The terrain chunk vertices amount per edge (square).")]
        public int ChunkSize
        {
            get { return Internal_GetChunkSize(unmanagedPtr); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern int Internal_GetChunkSize(IntPtr obj);

        /// <summary>
        /// Gets the terrain patches count. Each patch contains 16 chunks arranged into a 4x4 square.
        /// </summary>
        [Tooltip("The terrain patches count. Each patch contains 16 chunks arranged into a 4x4 square.")]
        public int PatchesCount
        {
            get { return Internal_GetPatchesCount(unmanagedPtr); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern int Internal_GetPatchesCount(IntPtr obj);

        /// <summary>
        /// Checks if terrain has the patch at the given coordinates.
        /// </summary>
        /// <param name="patchCoord">The patch location (x and z).</param>
        /// <returns>True if has patch added, otherwise false.</returns>
        public bool HasPatch(ref Int2 patchCoord)
        {
            return Internal_HasPatch(unmanagedPtr, ref patchCoord);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_HasPatch(IntPtr obj, ref Int2 patchCoord);

        /// <summary>
        /// Gets the zero-based index of the terrain patch in the terrain patches collection.
        /// </summary>
        /// <param name="patchCoord">The patch location (x and z).</param>
        /// <returns>The zero-based index of the terrain patch in the terrain patches collection. Returns -1 if patch coordinates are invalid.</returns>
        public int GetPatchIndex(ref Int2 patchCoord)
        {
            return Internal_GetPatchIndex(unmanagedPtr, ref patchCoord);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern int Internal_GetPatchIndex(IntPtr obj, ref Int2 patchCoord);

        /// <summary>
        /// Gets the terrain patch coordinates (x and z) at the given index.
        /// </summary>
        /// <param name="patchIndex">The zero-based index of the terrain patch in the terrain patches collection.</param>
        /// <param name="patchCoord">The patch location (x and z).</param>
        public void GetPatchCoord(int patchIndex, out Int2 patchCoord)
        {
            Internal_GetPatchCoord(unmanagedPtr, patchIndex, out patchCoord);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_GetPatchCoord(IntPtr obj, int patchIndex, out Int2 patchCoord);

        /// <summary>
        /// Gets the terrain patch world bounds at the given index.
        /// </summary>
        /// <param name="patchIndex">The zero-based index of the terrain patch in the terrain patches collection.</param>
        /// <param name="bounds">The patch world bounds.</param>
        public void GetPatchBounds(int patchIndex, out BoundingBox bounds)
        {
            Internal_GetPatchBounds(unmanagedPtr, patchIndex, out bounds);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_GetPatchBounds(IntPtr obj, int patchIndex, out BoundingBox bounds);

        /// <summary>
        /// Gets the terrain chunk world bounds at the given index.
        /// </summary>
        /// <param name="patchIndex">The zero-based index of the terrain patch in the terrain patches collection.</param>
        /// <param name="chunkIndex">The zero-based index of the terrain chunk in the terrain patch chunks collection (in range 0-15).</param>
        /// <param name="bounds">The chunk world bounds.</param>
        public void GetChunkBounds(int patchIndex, int chunkIndex, out BoundingBox bounds)
        {
            Internal_GetChunkBounds(unmanagedPtr, patchIndex, chunkIndex, out bounds);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_GetChunkBounds(IntPtr obj, int patchIndex, int chunkIndex, out BoundingBox bounds);

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

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern MaterialBase Internal_GetChunkOverrideMaterial(IntPtr obj, ref Int2 patchCoord, ref Int2 chunkCoord);

        /// <summary>
        /// Sets the chunk material to override the terrain default one.
        /// </summary>
        /// <param name="patchCoord">The patch coordinates (x and z).</param>
        /// <param name="chunkCoord">The chunk coordinates (x and z).</param>
        /// <param name="value">The value to set.</param>
        public void SetChunkOverrideMaterial(ref Int2 patchCoord, ref Int2 chunkCoord, MaterialBase value)
        {
            Internal_SetChunkOverrideMaterial(unmanagedPtr, ref patchCoord, ref chunkCoord, FlaxEngine.Object.GetUnmanagedPtr(value));
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetChunkOverrideMaterial(IntPtr obj, ref Int2 patchCoord, ref Int2 chunkCoord, IntPtr value);

        /// <summary>
        /// Setups the terrain patch using the specified heightmap data.
        /// </summary>
        /// <param name="patchCoord">The patch coordinates (x and z).</param>
        /// <param name="heightMapLength">The height map array length. It must match the terrain descriptor, so it should be (chunkSize*4+1)^2. Patch is a 4 by 4 square made of chunks. Each chunk has chunkSize quads on edge.</param>
        /// <param name="heightMap">The height map. Each array item contains a height value.</param>
        /// <param name="holesMask">The holes mask (optional). Normalized to 0-1 range values with holes mask per-vertex. Must match the heightmap dimensions.</param>
        /// <param name="forceUseVirtualStorage">If set to <c>true</c> patch will use virtual storage by force. Otherwise it can use normal texture asset storage on drive (valid only during Editor). Runtime-created terrain can only use virtual storage (in RAM).</param>
        /// <returns>True if failed, otherwise false.</returns>
        public bool SetupPatchHeightMap(ref Int2 patchCoord, int heightMapLength, float* heightMap, byte* holesMask = null, bool forceUseVirtualStorage = false)
        {
            return Internal_SetupPatchHeightMap(unmanagedPtr, ref patchCoord, heightMapLength, heightMap, holesMask, forceUseVirtualStorage);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_SetupPatchHeightMap(IntPtr obj, ref Int2 patchCoord, int heightMapLength, float* heightMap, byte* holesMask, bool forceUseVirtualStorage);

        /// <summary>
        /// Setups the terrain patch layer weights using the specified splatmaps data.
        /// </summary>
        /// <param name="patchCoord">The patch coordinates (x and z).</param>
        /// <param name="index">The zero-based index of the splatmap texture.</param>
        /// <param name="splatMapLength">The splatmap map array length. It must match the terrain descriptor, so it should be (chunkSize*4+1)^2. Patch is a 4 by 4 square made of chunks. Each chunk has chunkSize quads on edge.</param>
        /// <param name="splatMap">The splat map. Each array item contains 4 layer weights.</param>
        /// <param name="forceUseVirtualStorage">If set to <c>true</c> patch will use virtual storage by force. Otherwise it can use normal texture asset storage on drive (valid only during Editor). Runtime-created terrain can only use virtual storage (in RAM).</param>
        /// <returns>True if failed, otherwise false.</returns>
        public bool SetupPatchSplatMap(ref Int2 patchCoord, int index, int splatMapLength, Color32* splatMap, bool forceUseVirtualStorage = false)
        {
            return Internal_SetupPatchSplatMap(unmanagedPtr, ref patchCoord, index, splatMapLength, splatMap, forceUseVirtualStorage);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_SetupPatchSplatMap(IntPtr obj, ref Int2 patchCoord, int index, int splatMapLength, Color32* splatMap, bool forceUseVirtualStorage);

        /// <summary>
        /// Setups the terrain. Clears the existing data.
        /// </summary>
        /// <param name="lodCount">The LODs count. The actual amount of LODs may be lower due to provided chunk size (each LOD has 4 times less quads).</param>
        /// <param name="chunkSize">The size of the chunk (amount of quads per edge for the highest LOD). Must be power of two minus one (eg. 63 or 127).</param>
        public void Setup(int lodCount = 6, int chunkSize = 127)
        {
            Internal_Setup(unmanagedPtr, lodCount, chunkSize);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_Setup(IntPtr obj, int lodCount, int chunkSize);

        /// <summary>
        /// Adds the patches to the terrain (clears existing ones).
        /// </summary>
        /// <param name="numberOfPatches">The number of patches (x and z axis).</param>
        public void AddPatches(ref Int2 numberOfPatches)
        {
            Internal_AddPatches(unmanagedPtr, ref numberOfPatches);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_AddPatches(IntPtr obj, ref Int2 numberOfPatches);

        /// <summary>
        /// Adds the patch.
        /// </summary>
        /// <param name="patchCoord">The patch location (x and z).</param>
        public void AddPatch(ref Int2 patchCoord)
        {
            Internal_AddPatch(unmanagedPtr, ref patchCoord);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_AddPatch(IntPtr obj, ref Int2 patchCoord);

        /// <summary>
        /// Removes the patch.
        /// </summary>
        /// <param name="patchCoord">The patch location (x and z).</param>
        public void RemovePatch(ref Int2 patchCoord)
        {
            Internal_RemovePatch(unmanagedPtr, ref patchCoord);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_RemovePatch(IntPtr obj, ref Int2 patchCoord);

        /// <summary>
        /// Performs a raycast against this terrain collision shape.
        /// </summary>
        /// <param name="origin">The origin of the ray.</param>
        /// <param name="direction">The normalized direction of the ray.</param>
        /// <param name="resultHitDistance">The raycast result hit position distance from the ray origin. Valid only if raycast hits anything.</param>
        /// <param name="maxDistance">The maximum distance the ray should check for collisions.</param>
        /// <returns>True if ray hits an object, otherwise false.</returns>
        public bool RayCast(Vector3 origin, Vector3 direction, float resultHitDistance, float maxDistance = float.MaxValue)
        {
            return Internal_RayCast(unmanagedPtr, ref origin, ref direction, ref resultHitDistance, maxDistance);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_RayCast(IntPtr obj, ref Vector3 origin, ref Vector3 direction, ref float resultHitDistance, float maxDistance);

        /// <summary>
        /// Performs a raycast against terrain collision, returns results in a RayCastHit structure.
        /// </summary>
        /// <param name="origin">The origin of the ray.</param>
        /// <param name="direction">The normalized direction of the ray.</param>
        /// <param name="hitInfo">The result hit information. Valid only when method returns true.</param>
        /// <param name="maxDistance">The maximum distance the ray should check for collisions.</param>
        /// <returns>True if ray hits an object, otherwise false.</returns>
        public bool RayCast(Vector3 origin, Vector3 direction, out RayCastHit hitInfo, float maxDistance = float.MaxValue)
        {
            return Internal_RayCast1(unmanagedPtr, ref origin, ref direction, out hitInfo, maxDistance);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_RayCast1(IntPtr obj, ref Vector3 origin, ref Vector3 direction, out RayCastHit hitInfo, float maxDistance);

        /// <summary>
        /// Gets a point on the terrain collider that is closest to a given location. Can be used to find a hit location or position to apply explosion force or any other special effects.
        /// </summary>
        /// <param name="position">The position to find the closest point to it.</param>
        /// <param name="result">The result point on the collider that is closest to the specified location.</param>
        public void ClosestPoint(Vector3 position, out Vector3* result)
        {
            Internal_ClosestPoint(unmanagedPtr, ref position, out result);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_ClosestPoint(IntPtr obj, ref Vector3 position, out Vector3* result);

        /// <summary>
        /// Draws the terrain patch.
        /// </summary>
        /// <param name="renderContext">The rendering context.</param>
        /// <param name="patchCoord">The patch location (x and z).</param>
        /// <param name="material">The material to use for rendering.</param>
        /// <param name="lodIndex">The LOD index.</param>
        public void DrawPatch(ref RenderContext renderContext, ref Int2 patchCoord, MaterialBase material, int lodIndex = 0)
        {
            Internal_DrawPatch(unmanagedPtr, ref renderContext, ref patchCoord, FlaxEngine.Object.GetUnmanagedPtr(material), lodIndex);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_DrawPatch(IntPtr obj, ref RenderContext renderContext, ref Int2 patchCoord, IntPtr material, int lodIndex);

        /// <summary>
        /// Draws the terrain chunk.
        /// </summary>
        /// <param name="renderContext">The rendering context.</param>
        /// <param name="patchCoord">The patch location (x and z).</param>
        /// <param name="chunkCoord">The chunk location (x and z).</param>
        /// <param name="material">The material to use for rendering.</param>
        /// <param name="lodIndex">The LOD index.</param>
        public void DrawChunk(ref RenderContext renderContext, ref Int2 patchCoord, ref Int2 chunkCoord, MaterialBase material, int lodIndex = 0)
        {
            Internal_DrawChunk(unmanagedPtr, ref renderContext, ref patchCoord, ref chunkCoord, FlaxEngine.Object.GetUnmanagedPtr(material), lodIndex);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_DrawChunk(IntPtr obj, ref RenderContext renderContext, ref Int2 patchCoord, ref Int2 chunkCoord, IntPtr material, int lodIndex);
    }
}
