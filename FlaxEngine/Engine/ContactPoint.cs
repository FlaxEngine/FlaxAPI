// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System;
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
        private Guid _thisCollider;
        private Guid _otherCollider;

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

        /// <summary>
        /// Gets the first collider in the contact point (this instance).
        /// </summary>
        public Collider ThisCollider => Object.Find<Collider>(ref _thisCollider);

        /// <summary>
        /// Gets the other collider in the contact point (other instance).
        /// </summary>
        public Collider OtherCollider => Object.Find<Collider>(ref _otherCollider);

        internal ContactPoint(ref Collision.ContactPointData data, ref Guid colliderA, ref Guid colliderB)
        {
            _point = data.Point;
            _separation = data.Separation;
            _normal = data.Normal;
            _thisCollider = colliderA;
            _otherCollider = colliderB;
        }

        internal void SwapObjects()
        {
            var tmp = _thisCollider;
            _thisCollider = _otherCollider;
            _otherCollider = tmp;
        }
    }
}
