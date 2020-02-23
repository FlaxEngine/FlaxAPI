// Copyright (c) 2012-2020 Wojciech Figat. All rights reserved.

using System;

namespace FlaxEngine
{
    partial class SkinnedModel
    {
        /// <summary>
        /// The maximum allowed amount of skeleton bones to be used with skinned model.
        /// </summary>
        public const int MaxBones = 256;

        /// <summary>
        /// Gets the skeleton nodes hierarchy.
        /// </summary>
        public SkeletonNode[] Nodes => (SkeletonNode[])Internal_GetNodes(unmanagedPtr, typeof(SkeletonNode));

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

            // Call backend
            if (Internal_SetupSkeleton2(unmanagedPtr, nodes))
                throw new FlaxException("Failed to update skinned model skeleton.");
        }

        /// <summary>
        /// Finds the node with the given name.
        /// </summary>
        /// <param name="name">Thr name of the node.</param>
        /// <returns>The index of the node or -1 if not found.</returns>
        public int FindNode(string name)
        {
            return Internal_FindNode(unmanagedPtr, name);
        }

        /// <summary>
        /// Finds the bone with the given name.
        /// </summary>
        /// <param name="name">Thr name of the node used by the bone.</param>
        /// <returns>The index of the bone or -1 if not found.</returns>
        public int FindBone(string name)
        {
            return Internal_FindBone(unmanagedPtr, Internal_FindNode(unmanagedPtr, name));
        }

        /// <summary>
        /// Finds the bone that is using a given node index.
        /// </summary>
        /// <param name="nodeIndex">The index of the node.</param>
        /// <returns>The index of the bone or -1 if not found.</returns>
        public int FindBone(int nodeIndex)
        {
            return Internal_FindBone(unmanagedPtr, nodeIndex);
        }
    }
}
