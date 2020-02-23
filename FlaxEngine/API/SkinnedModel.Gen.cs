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
    public partial class SkinnedModel : ModelBase
    {
        /// <inheritdoc />
        protected SkinnedModel() : base()
        {
        }

        /// <summary>
        /// Array with all meshes.
        /// </summary>
        [Tooltip("Array with all meshes.")]
        public SkinnedMesh[] Meshes
        {
            get { return Internal_GetMeshes(unmanagedPtr, typeof(SkinnedMesh)); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern SkinnedMesh[] Internal_GetMeshes(IntPtr obj, System.Type resultArrayItemType0);

        /// <summary>
        /// Determines whether this skinned has all the meshes loaded (vertex/index buffers data is on a GPU).
        /// </summary>
        [Tooltip("Determines whether this skinned has all the meshes loaded (vertex/index buffers data is on a GPU).")]
        public bool HasMeshesLoaded
        {
            get { return Internal_HasMeshesLoaded(unmanagedPtr); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_HasMeshesLoaded(IntPtr obj);

        /// <summary>
        /// Gets the amount of meshes in the skinned model.
        /// </summary>
        [Tooltip("The amount of meshes in the skinned model.")]
        public int MeshesCount
        {
            get { return Internal_MeshesCount(unmanagedPtr); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern int Internal_MeshesCount(IntPtr obj);

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
        /// Gets the model bounding box in custom matrix world space (rig pose transformed by matrix, not animated).
        /// </summary>
        /// <param name="world">The transformation matrix.</param>
        /// <returns>The bounding box.</returns>
        public BoundingBox GetBox(Matrix world)
        {
            Internal_GetBox(unmanagedPtr, ref world, out var resultAsRef); return resultAsRef;
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_GetBox(IntPtr obj, ref Matrix world, out BoundingBox resultAsRef);

        /// <summary>
        /// Gets the model bounding box in local space (rig pose, not animated).
        /// </summary>
        /// <returns>The bounding box.</returns>
        public BoundingBox GetBox()
        {
            Internal_GetBox1(unmanagedPtr, out var resultAsRef); return resultAsRef;
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_GetBox1(IntPtr obj, out BoundingBox resultAsRef);

        /// <summary>
        /// Setups the skinned model including meshes creation and skeleton definition setup. Ensure to init SkeletonData manually after the call.
        /// </summary>
        /// <param name="meshesCount">The meshes count.</param>
        /// <returns>True if failed, otherwise false.</returns>
        public bool SetupMeshes(int meshesCount)
        {
            return Internal_SetupMeshes(unmanagedPtr, meshesCount);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_SetupMeshes(IntPtr obj, int meshesCount);

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

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern Array Internal_GetNodes(IntPtr obj, System.Type type);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_SetupSkeleton1(IntPtr obj, Array nodesObj, Array bonesObj, bool autoCalculateOffsetMatrix);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_SetupSkeleton2(IntPtr obj, Array nodesObj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern int Internal_FindNode(IntPtr obj, string name);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern int Internal_FindBone(IntPtr obj, int nodeIndex);
    }
}
