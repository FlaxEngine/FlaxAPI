// This code was auto-generated. Do not modify it.

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using FlaxEngine;

namespace FlaxEditor
{
    /// <summary>
    /// Terrain* tools for editor. Allows to create and modify terrain.
    /// </summary>
    [Tooltip("Terrain* tools for editor. Allows to create and modify terrain.")]
    public static unsafe partial class TerrainTools
    {
        /// <summary>
        /// Checks if a given ray hits any of the terrain patches sides to add a new patch there.
        /// </summary>
        /// <param name="terrain">The terrain.</param>
        /// <param name="ray">The ray to use for tracing (eg. mouse ray in world space).</param>
        /// <param name="result">The result patch coordinates (x and z). Valid only when method returns true.</param>
        /// <returns>True if result is valid, otherwise nothing to add there.</returns>
        public static bool TryGetPatchCoordToAdd(Terrain terrain, Ray ray, out Int2 result)
        {
            return Internal_TryGetPatchCoordToAdd(FlaxEngine.Object.GetUnmanagedPtr(terrain), ref ray, out result);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_TryGetPatchCoordToAdd(IntPtr terrain, ref Ray ray, out Int2 result);

        /// <summary>
        /// Generates the terrain from the input heightmap and splat maps.
        /// </summary>
        /// <param name="terrain">The terrain.</param>
        /// <param name="numberOfPatches">The number of patches (X and Z axis).</param>
        /// <param name="heightmap">The heightmap texture.</param>
        /// <param name="heightmapScale">The heightmap scale. Applied to adjust the normalized heightmap values into the world units.</param>
        /// <param name="splatmap1">The custom terrain splat map used as a source of the terrain layers weights. Each channel from RGBA is used as an independent layer weight for terrain layers compositing. It's optional.</param>
        /// <param name="splatmap2">The custom terrain splat map used as a source of the terrain layers weights. Each channel from RGBA is used as an independent layer weight for terrain layers compositing. It's optional.</param>
        /// <returns>True if failed, otherwise false.</returns>
        public static bool GenerateTerrain(Terrain terrain, ref Int2 numberOfPatches, Texture heightmap, float heightmapScale, Texture splatmap1, Texture splatmap2)
        {
            return Internal_GenerateTerrain(FlaxEngine.Object.GetUnmanagedPtr(terrain), ref numberOfPatches, FlaxEngine.Object.GetUnmanagedPtr(heightmap), heightmapScale, FlaxEngine.Object.GetUnmanagedPtr(splatmap1), FlaxEngine.Object.GetUnmanagedPtr(splatmap2));
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_GenerateTerrain(IntPtr terrain, ref Int2 numberOfPatches, IntPtr heightmap, float heightmapScale, IntPtr splatmap1, IntPtr splatmap2);

        /// <summary>
        /// Serializes the terrain chunk data to JSON string.
        /// </summary>
        /// <param name="terrain">The terrain.</param>
        /// <param name="patchCoord">The patch coordinates (x and z).</param>
        /// <returns>The serialized chunk data.</returns>
        public static string SerializePatch(Terrain terrain, ref Int2 patchCoord)
        {
            return Internal_SerializePatch(FlaxEngine.Object.GetUnmanagedPtr(terrain), ref patchCoord);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern string Internal_SerializePatch(IntPtr terrain, ref Int2 patchCoord);

        /// <summary>
        /// Deserializes the terrain chunk data from the JSON string.
        /// </summary>
        /// <param name="terrain">The terrain.</param>
        /// <param name="patchCoord">The patch coordinates (x and z).</param>
        /// <param name="value">The JSON string with serialized patch data.</param>
        public static void DeserializePatch(Terrain terrain, ref Int2 patchCoord, string value)
        {
            Internal_DeserializePatch(FlaxEngine.Object.GetUnmanagedPtr(terrain), ref patchCoord, value);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_DeserializePatch(IntPtr terrain, ref Int2 patchCoord, string value);

        /// <summary>
        /// Initializes the patch heightmap and collision to the default flat level.
        /// </summary>
        /// <param name="terrain">The terrain.</param>
        /// <param name="patchCoord">The patch coordinates (x and z) to initialize it.</param>
        /// <returns>True if failed, otherwise false.</returns>
        public static bool InitializePatch(Terrain terrain, ref Int2 patchCoord)
        {
            return Internal_InitializePatch(FlaxEngine.Object.GetUnmanagedPtr(terrain), ref patchCoord);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_InitializePatch(IntPtr terrain, ref Int2 patchCoord);

        /// <summary>
        /// Modifies the terrain patch heightmap with the given samples.
        /// </summary>
        /// <param name="terrain">The terrain.</param>
        /// <param name="patchCoord">The patch coordinates (x and z) to modify it.</param>
        /// <param name="samples">The samples. The array length is size.X*size.Y. It has to be type of float.</param>
        /// <param name="offset">The offset from the first row and column of the heightmap data (offset destination x and z start position).</param>
        /// <param name="size">The size of the heightmap to modify (x and z). Amount of samples in each direction.</param>
        /// <returns>True if failed, otherwise false.</returns>
        public static bool ModifyHeightMap(Terrain terrain, ref Int2 patchCoord, float* samples, ref Int2 offset, ref Int2 size)
        {
            return Internal_ModifyHeightMap(FlaxEngine.Object.GetUnmanagedPtr(terrain), ref patchCoord, samples, ref offset, ref size);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_ModifyHeightMap(IntPtr terrain, ref Int2 patchCoord, float* samples, ref Int2 offset, ref Int2 size);

        /// <summary>
        /// Modifies the terrain patch holes mask with the given samples.
        /// </summary>
        /// <param name="terrain">The terrain.</param>
        /// <param name="patchCoord">The patch coordinates (x and z) to modify it.</param>
        /// <param name="samples">The samples. The array length is size.X*size.Y. It has to be type of byte.</param>
        /// <param name="offset">The offset from the first row and column of the mask data (offset destination x and z start position).</param>
        /// <param name="size">The size of the mask to modify (x and z). Amount of samples in each direction.</param>
        /// <returns>True if failed, otherwise false.</returns>
        public static bool ModifyHolesMask(Terrain terrain, ref Int2 patchCoord, byte* samples, ref Int2 offset, ref Int2 size)
        {
            return Internal_ModifyHolesMask(FlaxEngine.Object.GetUnmanagedPtr(terrain), ref patchCoord, samples, ref offset, ref size);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_ModifyHolesMask(IntPtr terrain, ref Int2 patchCoord, byte* samples, ref Int2 offset, ref Int2 size);

        /// <summary>
        /// Modifies the terrain patch splat map (layers mask) with the given samples.
        /// </summary>
        /// <param name="terrain">The terrain.</param>
        /// <param name="patchCoord">The patch coordinates (x and z) to modify it.</param>
        /// <param name="index">The zero-based splatmap texture index.</param>
        /// <param name="samples">The samples. The array length is size.X*size.Y. It has to be type of <see cref="Color32"/>.</param>
        /// <param name="offset">The offset from the first row and column of the splatmap data (offset destination x and z start position).</param>
        /// <param name="size">The size of the splatmap to modify (x and z). Amount of samples in each direction.</param>
        /// <returns>True if failed, otherwise false.</returns>
        public static bool ModifySplatMap(Terrain terrain, ref Int2 patchCoord, int index, Color32* samples, ref Int2 offset, ref Int2 size)
        {
            return Internal_ModifySplatMap(FlaxEngine.Object.GetUnmanagedPtr(terrain), ref patchCoord, index, samples, ref offset, ref size);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_ModifySplatMap(IntPtr terrain, ref Int2 patchCoord, int index, Color32* samples, ref Int2 offset, ref Int2 size);

        /// <summary>
        /// Gets the raw pointer to the heightmap data (cached internally by the c++ core in editor).
        /// </summary>
        /// <param name="terrain">The terrain.</param>
        /// <param name="patchCoord">The patch coordinates (x and z) to gather it.</param>
        /// <returns>The pointer to the array of floats with terrain patch heights data. Null if failed to gather the data.</returns>
        public static float* GetHeightmapData(Terrain terrain, ref Int2 patchCoord)
        {
            return Internal_GetHeightmapData(FlaxEngine.Object.GetUnmanagedPtr(terrain), ref patchCoord);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern float* Internal_GetHeightmapData(IntPtr terrain, ref Int2 patchCoord);

        /// <summary>
        /// Gets the raw pointer to the holes mask data (cached internally by the c++ core in editor).
        /// </summary>
        /// <param name="terrain">The terrain.</param>
        /// <param name="patchCoord">The patch coordinates (x and z) to gather it.</param>
        /// <returns>The pointer to the array of bytes with terrain patch holes mask data. Null if failed to gather the data.</returns>
        public static byte* GetHolesMaskData(Terrain terrain, ref Int2 patchCoord)
        {
            return Internal_GetHolesMaskData(FlaxEngine.Object.GetUnmanagedPtr(terrain), ref patchCoord);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern byte* Internal_GetHolesMaskData(IntPtr terrain, ref Int2 patchCoord);

        /// <summary>
        /// Gets the raw pointer to the splatmap data (cached internally by the c++ core in editor).
        /// </summary>
        /// <param name="terrain">The terrain.</param>
        /// <param name="patchCoord">The patch coordinates (x and z) to gather it.</param>
        /// <param name="index">The zero-based splatmap texture index.</param>
        /// <returns>The pointer to the array of Color32 with terrain patch packed splatmap data. Null if failed to gather the data.</returns>
        public static Color32* GetSplatMapData(Terrain terrain, ref Int2 patchCoord, int index)
        {
            return Internal_GetSplatMapData(FlaxEngine.Object.GetUnmanagedPtr(terrain), ref patchCoord, index);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern Color32* Internal_GetSplatMapData(IntPtr terrain, ref Int2 patchCoord, int index);

        /// <summary>
        /// Export terrain's heightmap as a texture.
        /// </summary>
        /// <param name="terrain">The terrain.</param>
        /// <param name="outputFolder">The output folder path</param>
        /// <returns>True if failed, otherwise false.</returns>
        public static bool ExportTerrain(Terrain terrain, string outputFolder)
        {
            return Internal_ExportTerrain(FlaxEngine.Object.GetUnmanagedPtr(terrain), outputFolder);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_ExportTerrain(IntPtr terrain, string outputFolder);
    }
}
