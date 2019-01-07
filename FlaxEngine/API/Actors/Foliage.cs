// Copyright (c) 2012-2019 Wojciech Figat. All rights reserved.

using System.Runtime.InteropServices;

namespace FlaxEngine
{
    public sealed partial class Foliage
    {
        /// <summary>
        /// The foliage instance data.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct Instance
        {
            /// <summary>
            /// The local-space transformation of the mesh relative to the foliage actor.
            /// </summary>
            public Transform Transform;

            /// <summary>
            /// The foliage type index. Foliage types are hold in foliage actor and shared by instances using the same model.
            /// </summary>
            public int Type;

            /// <summary>
            /// The cached instance bounds (in world space).
            /// </summary>
            public BoundingSphere Bounds;
        }
    }
}
