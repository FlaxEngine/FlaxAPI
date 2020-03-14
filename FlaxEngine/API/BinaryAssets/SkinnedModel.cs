// Copyright (c) 2012-2020 Wojciech Figat. All rights reserved.

using System;
using System.Runtime.CompilerServices;

namespace FlaxEngine
{
    public sealed partial class SkinnedModel
    {
        private MaterialSlot[] _slots;
        private SkinnedMesh[] _meshes;
        private SkeletonNode[] _nodes;
        private SkeletonBone[] _bones;

        /// <summary>
        /// The maximum allowed amount of skeleton bones to be used with skinned model.
        /// </summary>
        public const int MaxBones = 256;

        /// <summary>
        /// Gets the material slots collection. Each slot contains information how to render mesh or meshes using it.
        /// </summary>
        public MaterialSlot[] MaterialSlots
        {
            get
            {
                if (_slots == null)
                {
                    // Ask unmanaged world for amount of material slots
                    int count = Model.Internal_GetSlots(unmanagedPtr);
                    if (count > 0)
                    {
                        _slots = new MaterialSlot[count];
                        for (int i = 0; i < count; i++)
                            _slots[i] = new MaterialSlot(this, i);
                    }
                }

                return _slots;
            }
            internal set
            {
                // Hidden by default
            }
        }

        /// <summary>
        /// Gets the amount of the material slots used by this model.
        /// </summary>
        public int MaterialSlotsCount => Model.Internal_GetSlots(unmanagedPtr);

        /// <summary>
        /// Gets the material slot by the name.
        /// </summary>
        /// <param name="name">The slot name.</param>
        /// <returns>The material slot with the given name or null if cannot find it (asset may be not loaded yet).</returns>
        public MaterialSlot GetSlot(string name)
        {
            MaterialSlot result = null;
            var slots = MaterialSlots;
            if (slots != null)
            {
                for (int i = 0; i < slots.Length; i++)
                {
                    if (string.Equals(slots[i].Name, name, StringComparison.Ordinal))
                    {
                        result = slots[i];
                        break;
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Gets the skinned meshes collection.
        /// </summary>
        public SkinnedMesh[] Meshes
        {
            get
            {
                if (_meshes == null)
                {
                    // Ask unmanaged world for amount of meshes
                    int count = MeshesCount;
                    if (count > 0)
                    {
                        _meshes = new SkinnedMesh[count];
                        for (int i = 0; i < count; i++)
                            _meshes[i] = new SkinnedMesh(this, i);
                    }
                }

                return _meshes;
            }
        }

        /// <summary>
        /// Gets or sets the skeleton nodes hierarchy.
        /// </summary>
        /// <remarks>
        /// Editing skeleton via <see cref="SetupSkeleton"/> is only supported for the virtual assets (see <see cref="Asset.IsVirtual"/> and <see cref="Content.CreateVirtualAsset{T}"/>).
        /// </remarks>
        public SkeletonNode[] Nodes
        {
            get
            {
                if (_nodes == null)
                {
                    _nodes = Internal_GetNodes(unmanagedPtr, typeof(SkeletonNode));
                }

                return _nodes;
            }
        }

        /// <summary>
        /// Gets or sets the skeleton bones hierarchy.
        /// </summary>
        /// <remarks>
        /// Editing skeleton via <see cref="SetupSkeleton"/> is only supported for the virtual assets (see <see cref="Asset.IsVirtual"/> and <see cref="Content.CreateVirtualAsset{T}"/>).
        /// </remarks>
        public SkeletonBone[] Bones
        {
            get
            {
                if (_bones == null)
                {
                    _bones = Internal_GetBones(unmanagedPtr, typeof(SkeletonBone));
                }

                return _bones;
            }
        }

        /// <summary>
        /// Setups the skinned model including meshes creation and skeleton definition setup. Ensure to init SkeletonData manually after the call.
        /// </summary>
        /// <remarks>
        /// Can be used only for virtual assets (see <see cref="Asset.IsVirtual"/> and <see cref="Content.CreateVirtualAsset{T}"/>).
        /// </remarks>
        /// <param name="meshesCount">The meshes count.</param>
        public void SetupMeshes(int meshesCount)
        {
            // Validate state and input
            if (!IsVirtual)
                throw new InvalidOperationException("Only virtual models can be modified at runtime.");
            if (meshesCount <= 0 || meshesCount > Model.MaxLODs)
                throw new ArgumentOutOfRangeException(nameof(meshesCount));

            // Cleanup data
            _meshes = null;

            // Call backend
            if (Internal_SetupMeshes(unmanagedPtr, meshesCount))
                throw new FlaxException("Failed to update skinned model meshes collection.");
        }

        /// <summary>
        /// Setups the skinned model skeleton.
        /// </summary>
        /// <param name="nodes">The nodes hierarchy. The first node must be a root one (with parent index equal -1).</param>
        /// <param name="bones">The bones hierarchy.</param>
        /// <param name="autoCalculateOffsetMatrix">If true then the OffsetMatrix for each bone will be auto-calculated by the engine, otherwise the provided values will be used.</param>
        public void SetupSkeleton(SkeletonNode[] nodes, SkeletonBone[] bones, bool autoCalculateOffsetMatrix)
        {
            // Validate state and input
            if (!IsVirtual)
                throw new InvalidOperationException("Only virtual models can be modified at runtime.");
            if (nodes == null)
                throw new ArgumentNullException(nameof(nodes));
            if (bones == null)
                throw new ArgumentNullException(nameof(bones));
            if (bones.Length > MaxBones)
                throw new ArgumentOutOfRangeException(nameof(bones));

            // Cleanup data
            _nodes = null;
            _bones = null;

            // Call backend
            if (Internal_SetupSkeleton1(unmanagedPtr, nodes, bones, autoCalculateOffsetMatrix))
                throw new FlaxException("Failed to update skinned model skeleton.");
        }

        /// <summary>
        /// Setups the skinned model skeleton. Uses the same nodes layout for skeleton bones and calculates the offset matrix by auto.
        /// </summary>
        /// <param name="nodes">The nodes hierarchy. The first node must be a root one (with parent index equal -1).</param>
        public void SetupSkeleton(SkeletonNode[] nodes)
        {
            // Validate state and input
            if (!IsVirtual)
                throw new InvalidOperationException("Only virtual models can be modified at runtime.");
            if (nodes == null)
                throw new ArgumentNullException(nameof(nodes));

            // Cleanup data
            _nodes = null;
            _bones = null;

            // Call backend
            if (Internal_SetupSkeleton2(unmanagedPtr, nodes))
                throw new FlaxException("Failed to update skinned model skeleton.");
        }

        /// <summary>
        /// Setups the material slots collection.
        /// </summary>
        /// <param name="slotsCount">The slots count.</param>
        public void SetupMaterialSlots(int slotsCount)
        {
            // Validate state and input
            if (!IsVirtual && WaitForLoaded())
                throw new FlaxException("Failed to update skinned model if asset is not virtual and loading failed.");
            if (slotsCount <= 0 || slotsCount > Model.MaxMaterialSlots)
                throw new ArgumentOutOfRangeException(nameof(slotsCount));

            // Cleanup data
            _slots = null;

            // Call backend
            if (Internal_SetupSlots(unmanagedPtr, slotsCount))
                throw new FlaxException("Failed to update skinned model material slots collection.");
        }

        /// <summary>
        /// Gets the skeleton node index by node name.
        /// </summary>
        /// <param name="name">The node name.</param>
        /// <param name="index">The found node index or -1 if not found.</param>
        /// <returns>True if found that node, otherwise false.</returns>
        public bool GetNodeByName(string name, out int index)
        {
            index = -1;

            if (WaitForLoaded())
                return false;

            var nodes = Nodes;
            if (nodes == null)
                return false;

            for (int i = 0; i < nodes.Length; i++)
            {
                if (string.Equals(nodes[i].Name, name, StringComparison.OrdinalIgnoreCase))
                {
                    index = i;
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Gets the skeleton bone index by node name.
        /// </summary>
        /// <param name="name">The node name.</param>
        /// <param name="index">The found bone index or -1 if not found.</param>
        /// <returns>True if found that bone, otherwise false.</returns>
        public bool GetBoneByName(string name, out int index)
        {
            index = -1;

            if (WaitForLoaded())
                return false;

            var nodes = Nodes;
            var bones = Bones;
            if (nodes == null || bones == null)
                return false;

            for (int i = 0; i < bones.Length; i++)
            {
                if (string.Equals(nodes[bones[i].NodeIndex].Name, name, StringComparison.OrdinalIgnoreCase))
                {
                    index = i;
                    return true;
                }
            }

            return false;
        }

        internal void Internal_OnUnload()
        {
            // Clear cached data
            _slots = null;
            _meshes = null;
            _nodes = null;
            _bones = null;
        }

        #region Internal Calls

#if !UNIT_TEST_COMPILANT
        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern SkeletonNode[] Internal_GetNodes(IntPtr obj, Type type);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern SkeletonBone[] Internal_GetBones(IntPtr obj, Type type);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_SetupMeshes(IntPtr obj, int meshesCount);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_SetupSkeleton1(IntPtr obj, SkeletonNode[] nodes, SkeletonBone[] bones, bool autoCalculateOffsetMatrix);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_SetupSkeleton2(IntPtr obj, SkeletonNode[] nodes);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_SetupSlots(IntPtr obj, int slotsCount);
#endif

        #endregion
    }
}
