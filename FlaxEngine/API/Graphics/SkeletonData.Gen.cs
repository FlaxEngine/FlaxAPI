// This code was auto-generated. Do not modify it.

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace FlaxEngine
{
    /// <summary>
    /// Describes a single skeleton node data. Used by the runtime.
    /// </summary>
    [Tooltip("Describes a single skeleton node data. Used by the runtime.")]
    [StructLayout(LayoutKind.Sequential)]
    public unsafe partial struct SkeletonNode
    {
        /// <summary>
        /// The parent node index. The root node uses value -1.
        /// </summary>
        [Tooltip("The parent node index. The root node uses value -1.")]
        public int ParentIndex;

        /// <summary>
        /// The local transformation of the node, relative to the parent node.
        /// </summary>
        [Tooltip("The local transformation of the node, relative to the parent node.")]
        public Transform LocalTransform;

        /// <summary>
        /// The name of this node.
        /// </summary>
        [Tooltip("The name of this node.")]
        public string Name;
    }
}

namespace FlaxEngine
{
    /// <summary>
    /// Describes a single skeleton bone data. Used by the runtime. Skeleton bones are subset of the skeleton nodes collection that are actually used by the skinned model meshes.
    /// </summary>
    [Tooltip("Describes a single skeleton bone data. Used by the runtime. Skeleton bones are subset of the skeleton nodes collection that are actually used by the skinned model meshes.")]
    [StructLayout(LayoutKind.Sequential)]
    public unsafe partial struct SkeletonBone
    {
        /// <summary>
        /// The parent bone index. The root bone uses value -1.
        /// </summary>
        [Tooltip("The parent bone index. The root bone uses value -1.")]
        public int ParentIndex;

        /// <summary>
        /// The index of the skeleton node where bone is 'attached'. Used as a animation transformation source.
        /// </summary>
        [Tooltip("The index of the skeleton node where bone is 'attached'. Used as a animation transformation source.")]
        public int NodeIndex;

        /// <summary>
        /// The local transformation of the bone, relative to the parent bone (in bind pose).
        /// </summary>
        [Tooltip("The local transformation of the bone, relative to the parent bone (in bind pose).")]
        public Transform LocalTransform;

        /// <summary>
        /// The matrix that transforms from mesh space to bone space in bind pose (inverse bind pose).
        /// </summary>
        [Tooltip("The matrix that transforms from mesh space to bone space in bind pose (inverse bind pose).")]
        public Matrix OffsetMatrix;
    }
}
