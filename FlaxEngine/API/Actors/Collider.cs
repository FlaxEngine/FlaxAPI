// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System;
using System.Runtime.CompilerServices;

namespace FlaxEngine
{
    /// <summary>
    /// Function delegate used for the collision events.
    /// </summary>
    /// <param name="collision">The collision information.</param>
    public delegate void CollisionDelegate(Collision collision);

    /// <summary>
    /// Function delegate used for the trigger events.
    /// </summary>
    /// <param name="collider">The other collider.</param>
    public delegate void TriggerDelegate(Collider collider);

    public abstract partial class Collider
    {
        /// <summary>
        /// Occurs when a collision start gets registered for this collider (it collides with something).
        /// </summary>
        public event CollisionDelegate CollisionEnter;

        /// <summary>
        /// Occurs when a collision end gets registered for this collider (it ends colliding with something).
        /// </summary>
        public event CollisionDelegate CollisionExit;

        internal void OnCollisionEnter(Collision collision)
        {
            CollisionEnter?.Invoke(collision);
        }

        internal void OnCollisionExit(Collision collision)
        {
            CollisionExit?.Invoke(collision);
        }

        /// <summary>
        /// Occurs when a trigger touching start gets registered for this collider (the other collider enters it and triggers the event).
        /// </summary>
        public event TriggerDelegate TriggerEnter;

        /// <summary>
        /// Occurs when a trigger touching end gets registered for this collider (the other collider enters it and triggers the event).
        /// </summary>
        public event TriggerDelegate TriggerExit;

        internal void OnTriggerEnter(Collider collider)
        {
            TriggerEnter?.Invoke(collider);
        }

        internal void OnTriggerExit(Collider collider)
        {
            TriggerExit?.Invoke(collider);
        }

        /// <summary>
        /// Performs a raycast against this collider.
        /// </summary>
        /// <param name="origin">The origin of the ray.</param>
        /// <param name="direction">The normalized direction of the ray.</param>
        /// <param name="maxDistance">The maximum distance the ray should check for collisions.</param>
        /// <returns>True if ray hits an object, otherwise false.</returns>
        public bool RayCast(Vector3 origin, Vector3 direction, float maxDistance = float.MaxValue)
        {
            return Internal_RayCast1(unmanagedPtr, ref origin, ref direction, maxDistance);
        }

        /// <summary>
        /// Performs a raycast against this collider.
        /// </summary>
        /// <param name="origin">The origin of the ray.</param>
        /// <param name="direction">The normalized direction of the ray.</param>
        /// <param name="resultHitDistance">The raycast result hit position distance from the ray origin. Valid only if raycast hits anything.</param>
        /// <param name="maxDistance">The maximum distance the ray should check for collisions.</param>
        /// <returns>True if ray hits an object, otherwise false.</returns>
        public bool RayCast(Vector3 origin, Vector3 direction, out float resultHitDistance, float maxDistance = float.MaxValue)
        {
            return Internal_RayCast2(unmanagedPtr, ref origin, ref direction, out resultHitDistance, maxDistance);
        }

        /// <summary>
        /// Performs a raycast against this collider, returns results in a RaycastHit structure.
        /// </summary>
        /// <param name="origin">The origin of the ray.</param>
        /// <param name="direction">The normalized direction of the ray.</param>
        /// <param name="hitInfo">The result hit information. Valid only when method returns true.</param>
        /// <param name="maxDistance">The maximum distance the ray should check for collisions.</param>
        /// <returns>True if ray hits an object, otherwise false.</returns>
        public bool RayCast(Vector3 origin, Vector3 direction, out RayCastHit hitInfo, float maxDistance = float.MaxValue)
        {
            return Internal_RayCast3(unmanagedPtr, ref origin, ref direction, out hitInfo, maxDistance);
        }

        /// <summary>
        /// Gets a point on the collider that is closest to a given location.
        /// Can be used to find a hit location or position to apply explosion force or any other special effects.
        /// </summary>
        /// <param name="position">The position to find the closest point to it.</param>
        /// <returns>The result point on the collider that is closest to the specified location.</returns>
        public Vector3 ClosestPoint(Vector3 position)
        {
            Vector3 result;
            Internal_ClosestPoint(unmanagedPtr, ref position, out result);
            return result;
        }

        #region Internal Calls

#if !UNIT_TEST_COMPILANT
        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_RayCast1(IntPtr obj, ref Vector3 origin, ref Vector3 direction, float maxDistance);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_RayCast2(IntPtr obj, ref Vector3 origin, ref Vector3 direction, out float resultHitDistance, float maxDistance);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_RayCast3(IntPtr obj, ref Vector3 origin, ref Vector3 direction, out RayCastHit hitInfo, float maxDistance);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_ClosestPoint(IntPtr obj, ref Vector3 position, out Vector3 result);
#endif

        #endregion
    }
}
