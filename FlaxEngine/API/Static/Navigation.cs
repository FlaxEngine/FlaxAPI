// Copyright (c) 2012-2020 Wojciech Figat. All rights reserved.

using System.Runtime.InteropServices;

namespace FlaxEngine
{
    /// <summary>
    /// The result information for navigation mesh queries.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct NavMeshHit
    {
        /// <summary>
        /// The hit point position.
        /// </summary>
        public Vector3 Position;

        /// <summary>
        /// The distance to hit point (from the query origin).
        /// </summary>
        public float Distance;

        /// <summary>
        /// The hit point normal vector.
        /// </summary>
        public Vector3 Normal;
    }

    public static partial class Navigation
    {
    }
}
