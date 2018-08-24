// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

namespace FlaxEngine
{
    /// <summary>
    /// Defines the single skeleton hierarchy bone including its default bind pose, name and parent node index.
    /// </summary>
    public struct SkeletonBone
    {
        /// <summary>
        /// The parent bone index. The root node uses value -1.
        /// </summary>
        public int ParentIndex;

        /// <summary>
        /// The local transformation of the bone, relative to the parent bone (in bind pose).
        /// </summary>
        public Transform LocalTransform;

        /// <summary>
        /// The name of this bone.
        /// </summary>
        public string Name;
    }
}
