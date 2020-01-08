// Copyright (c) 2012-2020 Wojciech Figat. All rights reserved.

using System.Runtime.InteropServices;

// ReSharper disable ConvertToAutoProperty

namespace FlaxEngine
{
    /// <summary>
    /// Contains a contact point data for the collision location.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct ContactPoint
    {
        private Vector3 _point;
        private float _separation;
        private Vector3 _normal;

        /// <summary>
        /// Gets the contact point location in the world space.
        /// </summary>
        public Vector3 Point => _point;

        /// <summary>
        /// Gets the separation value (negative implies penetration).
        /// </summary>
        public float Separation => _separation;

        /// <summary>
        /// Gets the contact normal.
        /// </summary>
        public Vector3 Normal => _normal;

        internal ContactPoint(ref Collision.ContactPointData data)
        {
            _point = data.Point;
            _separation = data.Separation;
            _normal = data.Normal;
        }
    }
}
