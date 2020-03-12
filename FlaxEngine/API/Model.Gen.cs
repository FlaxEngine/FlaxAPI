// This code was auto-generated. Do not modify it.

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace FlaxEngine
{
    /// <summary>
    /// Model asset that contains model object made of meshes which can rendered on the GPU.
    /// </summary>
    [Tooltip("Model asset that contains model object made of meshes which can rendered on the GPU.")]
    public unsafe partial class Model : ModelBase
    {
        /// <inheritdoc />
        protected Model() : base()
        {
        }

        /// <summary>
        /// Model level of details. The first entry is the highest quality LOD0 followed by more optimized versions.
        /// </summary>
        [Tooltip("Model level of details. The first entry is the highest quality LOD0 followed by more optimized versions.")]
        public ModelLOD[] LODs
        {
            get { return Internal_GetLODs(unmanagedPtr, typeof(ModelLOD)); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern ModelLOD[] Internal_GetLODs(IntPtr obj, System.Type resultArrayItemType0);

        /// <summary>
        /// Gets the amount of loaded model LODs.
        /// </summary>
        [Tooltip("The amount of loaded model LODs.")]
        public int LoadedLODs
        {
            get { return Internal_GetLoadedLODs(unmanagedPtr); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern int Internal_GetLoadedLODs(IntPtr obj);

        /// <summary>
        /// Gets the model bounding box in custom matrix world space.
        /// </summary>
        /// <param name="world">The transformation matrix.</param>
        /// <param name="lodIndex">The Level Of Detail index.</param>
        /// <returns>The bounding box.</returns>
        public BoundingBox GetBox(Matrix world, int lodIndex = 0)
        {
            Internal_GetBox(unmanagedPtr, ref world, lodIndex, out var resultAsRef); return resultAsRef;
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_GetBox(IntPtr obj, ref Matrix world, int lodIndex, out BoundingBox resultAsRef);

        /// <summary>
        /// Gets the model bounding box in local space.
        /// </summary>
        /// <param name="lodIndex">The Level Of Detail index.</param>
        /// <returns>The bounding box.</returns>
        public BoundingBox GetBox(int lodIndex = 0)
        {
            Internal_GetBox1(unmanagedPtr, lodIndex, out var resultAsRef); return resultAsRef;
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_GetBox1(IntPtr obj, int lodIndex, out BoundingBox resultAsRef);

        /// <summary>
        /// Draws the model.
        /// </summary>
        /// <param name="renderContext">The rendering context.</param>
        /// <param name="material">The material to use for rendering.</param>
        /// <param name="world">The world transformation of the model.</param>
        /// <param name="flags">The object static flags.</param>
        /// <param name="receiveDecals">True if rendered geometry can receive decals, otherwise false.</param>
        public void Draw(ref RenderContext renderContext, MaterialBase material, ref Matrix world, StaticFlags flags = StaticFlags.None, bool receiveDecals = true)
        {
            Internal_Draw(unmanagedPtr, ref renderContext, FlaxEngine.Object.GetUnmanagedPtr(material), ref world, flags, receiveDecals);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_Draw(IntPtr obj, ref RenderContext renderContext, IntPtr material, ref Matrix world, StaticFlags flags, bool receiveDecals);

        /// <summary>
        /// Setups the model LODs collection including meshes creation.
        /// </summary>
        /// <param name="meshesCountPerLod">The meshes count per lod array (amount of meshes per LOD).</param>
        /// <returns>True if failed, otherwise false.</returns>
        public bool SetupLODs(int[] meshesCountPerLod)
        {
            return Internal_SetupLODs(unmanagedPtr, meshesCountPerLod);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_SetupLODs(IntPtr obj, int[] meshesCountPerLod);

        /// <summary>
        /// Saves this asset to the file. Supported only in Editor.
        /// </summary>
        /// <remarks>If you use saving with the GPU mesh data then the call has to be provided from the thread other than the main game thread.</remarks>
        /// <param name="withMeshDataFromGpu">True if save also GPU mesh buffers, otherwise will keep data in storage unmodified. Valid only if saving the same asset to the same location and it's loaded.</param>
        /// <param name="path">The custom asset path to use for the saving. Use empty value to save this asset to its own storage location. Can be used to duplicate asset. Must be specified when saving virtual asset.</param>
        /// <returns>True if cannot save data, otherwise false.</returns>
        public bool Save(bool withMeshDataFromGpu = false, string path = null)
        {
            return Internal_Save(unmanagedPtr, withMeshDataFromGpu, path);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_Save(IntPtr obj, bool withMeshDataFromGpu, string path);
    }
}
