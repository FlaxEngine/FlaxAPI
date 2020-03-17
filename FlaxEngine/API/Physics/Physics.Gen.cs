// This code was auto-generated. Do not modify it.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace FlaxEngine
{
    /// <summary>
    /// Raycast hit result data.
    /// </summary>
    [Tooltip("Raycast hit result data.")]
    [StructLayout(LayoutKind.Sequential)]
    public unsafe partial struct RayCastHit
    {
        /// <summary>
        /// The collider that was hit.
        /// </summary>
        [Tooltip("The collider that was hit.")]
        public PhysicsColliderActor Collider;

        /// <summary>
        /// The normal of the surface the ray hit.
        /// </summary>
        [Tooltip("The normal of the surface the ray hit.")]
        public Vector3 Normal;

        /// <summary>
        /// The distance from the ray's origin to the hit location.
        /// </summary>
        [Tooltip("The distance from the ray's origin to the hit location.")]
        public float Distance;

        /// <summary>
        /// The point in the world space where ray hit the collider.
        /// </summary>
        [Tooltip("The point in the world space where ray hit the collider.")]
        public Vector3 Point;

        /// <summary>
        /// The barycentric coordinates of hit point, for triangle mesh and height field.
        /// </summary>
        [Tooltip("The barycentric coordinates of hit point, for triangle mesh and height field.")]
        public Vector2 UV;
    }
}
