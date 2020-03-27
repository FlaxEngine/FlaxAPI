// Copyright (c) 2012-2020 Wojciech Figat. All rights reserved.

namespace FlaxEngine
{
    partial class RigidBody
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
    }
}
