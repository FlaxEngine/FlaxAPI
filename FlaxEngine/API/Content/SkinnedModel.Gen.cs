// This code was auto-generated. Do not modify it.

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace FlaxEngine
{
    /// <summary>
    /// Skinned model asset that contains model object made of meshes that can be rendered on the GPU using skeleton bones skinning.
    /// </summary>
    [Tooltip("Skinned model asset that contains model object made of meshes that can be rendered on the GPU using skeleton bones skinning.")]
    public unsafe partial class SkinnedModel : ModelBase
    {
        /// <inheritdoc />
        protected SkinnedModel() : base()
        {
        }

        /// <summary>
        /// Model level of details. The first entry is the highest quality LOD0 followed by more optimized versions.
        /// </summary>
        [Tooltip("Model level of details. The first entry is the highest quality LOD0 followed by more optimized versions.")]
        public SkinnedModelLOD[] LODs
        {
            get { return Internal_GetLODs(unmanagedPtr, typeof(SkinnedModelLOD)); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern SkinnedModelLOD[] Internal_GetLODs(IntPtr obj, System.Type resultArrayItemType0);

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
        /// Gets the skeleton nodes hierarchy.
        /// </summary>
        [Tooltip("The skeleton nodes hierarchy.")]
        public SkeletonNode[] Nodes
        {
            get { return Internal_GetNodes(unmanagedPtr, typeof(SkeletonNode)); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern SkeletonNode[] Internal_GetNodes(IntPtr obj, System.Type resultArrayItemType0);

        /// <summary>
        /// Gets the skeleton bones hierarchy.
        /// </summary>
        [Tooltip("The skeleton bones hierarchy.")]
        public SkeletonBone[] Bones
        {
            get { return Internal_GetBones(unmanagedPtr, typeof(SkeletonBone)); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern SkeletonBone[] Internal_GetBones(IntPtr obj, System.Type resultArrayItemType0);

        /// <summary>
        /// Gets the blend shapes names used by the skinned model meshes (from LOD 0 only).
        /// </summary>
        [Tooltip("The blend shapes names used by the skinned model meshes (from LOD 0 only).")]
        public string[] BlendShapes
        {
            get { return Internal_GetBlendShapes(unmanagedPtr); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern string[] Internal_GetBlendShapes(IntPtr obj);

        /// <summary>
        /// Finds the node with the given name.
        /// </summary>
        /// <param name="name">Thr name of the node.</param>
        /// <returns>The index of the node or -1 if not found.</returns>
        public int FindNode(string name)
        {
            return Internal_FindNode(unmanagedPtr, name);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern int Internal_FindNode(IntPtr obj, string name);

        /// <summary>
        /// Finds the bone with the given name.
        /// </summary>
        /// <param name="name">Thr name of the node used by the bone.</param>
        /// <returns>The index of the bone or -1 if not found.</returns>
        public int FindBone(string name)
        {
            return Internal_FindBone(unmanagedPtr, name);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern int Internal_FindBone(IntPtr obj, string name);

        /// <summary>
        /// Finds the bone that is using a given node index.
        /// </summary>
        /// <param name="nodeIndex">The index of the node.</param>
        /// <returns>The index of the bone or -1 if not found.</returns>
        public int FindBone(int nodeIndex)
        {
            return Internal_FindBone1(unmanagedPtr, nodeIndex);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern int Internal_FindBone1(IntPtr obj, int nodeIndex);

        /// <summary>
        /// Gets the model bounding box in custom matrix world space (rig pose transformed by matrix, not animated).
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
        /// Gets the model bounding box in local space (rig pose, not animated).
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
        /// Setups the skinned model skeleton. Uses the same nodes layout for skeleton bones and calculates the offset matrix by auto.
        /// </summary>
        /// <param name="nodes">The nodes hierarchy. The first node must be a root one (with parent index equal -1).</param>
        /// <returns>True if failed, otherwise false.</returns>
        public bool SetupSkeleton(SkeletonNode[] nodes)
        {
            return Internal_SetupSkeleton(unmanagedPtr, nodes);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_SetupSkeleton(IntPtr obj, SkeletonNode[] nodes);

        /// <summary>
        /// Setups the skinned model skeleton.
        /// </summary>
        /// <param name="nodes">The nodes hierarchy. The first node must be a root one (with parent index equal -1).</param>
        /// <param name="bones">The bones hierarchy.</param>
        /// <param name="autoCalculateOffsetMatrix">If true then the OffsetMatrix for each bone will be auto-calculated by the engine, otherwise the provided values will be used.</param>
        /// <returns>True if failed, otherwise false.</returns>
        public bool SetupSkeleton(SkeletonNode[] nodes, SkeletonBone[] bones, bool autoCalculateOffsetMatrix)
        {
            return Internal_SetupSkeleton1(unmanagedPtr, nodes, bones, autoCalculateOffsetMatrix);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_SetupSkeleton1(IntPtr obj, SkeletonNode[] nodes, SkeletonBone[] bones, bool autoCalculateOffsetMatrix);

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
