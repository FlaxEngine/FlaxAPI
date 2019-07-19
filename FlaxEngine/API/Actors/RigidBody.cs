// Copyright (c) 2012-2019 Wojciech Figat. All rights reserved.

using System;
using System.Runtime.CompilerServices;

namespace FlaxEngine
{
    public sealed partial class RigidBody
    {
        /// <summary>
        /// Occurs when a collision start gets registered for one of the colliders attached to this rigidbody (it collides with something).
        /// </summary>
        public event CollisionDelegate CollisionEnter;

        /// <summary>
        /// Occurs when a collision end gets registered for one of the colliders attached to this rigidbody (it ends colliding with something).
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
        /// Occurs when a trigger touching start gets registered for one of the colliders attached to this rigidbody (the other collider enters it and triggers the event).
        /// </summary>
        public event TriggerDelegate TriggerEnter;

        /// <summary>
        /// Occurs when a trigger touching end gets registered for one of the colliders attached to this rigidbody (the other collider enters it and triggers the event).
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
        /// Gets a point on one of the colliders attached to the attached that is closest to a given location. 
        /// Can be used to find a hit location or position to apply explosion force or any other special effects.
        /// </summary>
        /// <param name="position">The position to find the closest point to it.</param>
        /// <returns>The result point on the rigidbody shape that is closest to the specified location.</returns>
        public Vector3 ClosestPoint(Vector3 position)
        {
            Vector3 result;
            Internal_ClosestPoint(unmanagedPtr, ref position, out result);
            return result;
        }

        #region Internal Calls

#if !UNIT_TEST_COMPILANT
        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_ClosestPoint(IntPtr obj, ref Vector3 position, out Vector3 result);
#endif

        #endregion
    }
}
