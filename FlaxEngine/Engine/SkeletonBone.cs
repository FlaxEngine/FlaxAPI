// Copyright (c) 2012-2019 Wojciech Figat. All rights reserved.

namespace FlaxEngine
{
    /// <summary>
    /// Defines the single skeleton hierarchy bone including its default bind pose, name and parent node index.
    /// </summary>
    /// <remarks>
    /// Skeleton bones are subset of the skeleton nodes collection that are actually used by the skinned model meshes.
    /// </remarks>
    public struct SkeletonBone
    {
        /// <summary>
        /// The parent bone index. The root node uses value -1.
        /// </summary>
        public int ParentIndex;

        /// <summary>
        /// The index of the skeleton node where bone is 'attached'. Used as a animation transformation source.
        /// </summary>
        public int NodeIndex;

        /// <summary>
        /// The local transformation of the bone, relative to the parent bone (in bind pose).
        /// </summary>
        public Transform LocalTransform;

        /// <summary>
        /// The matrix that transforms from mesh space to bone space in bind pose (inverse bind pose).
        /// </summary>
        public Matrix OffsetMatrix;
    }
}
