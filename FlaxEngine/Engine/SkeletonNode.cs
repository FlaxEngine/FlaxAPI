// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

namespace FlaxEngine
{
    /// <summary>
    /// Describes a single skeleton node data. Used by the runtime.
    /// </summary>
    public struct SkeletonNode
    {
        /// <summary>
        /// The parent node index. The root node uses value -1.
        /// </summary>
        public int ParentIndex;

        /// <summary>
        /// The local transformation of the node, relative to the parent node.
        /// </summary>
        public Transform LocalTransform;

        /// <summary>
        /// The name of this node.
        /// </summary>
        public string Name;
    }
}
