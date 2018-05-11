// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

namespace FlaxEngine
{
    /// <summary>
    /// Raycast hit result data.
    /// </summary>
    public struct RayCastHit
    {
        /// <summary>
        /// The collider that was hit.
        /// </summary>
        public Collider Collider;

        /// <summary>
        /// The normal of the surface the ray hit.
        /// </summary>
        public Vector3 Normal;

        /// <summary>
        /// The distance from the ray's origin to the hit location.
        /// </summary>
        public float Distance;

        /// <summary>
        /// The point in the world space where ray hit the collider.
        /// </summary>
        public Vector3 Point;

        /// <summary>
        /// The barycentric coordinates of hit point, for triangle mesh and height field.
        /// </summary>
        public Vector2 UV;
    }
}
