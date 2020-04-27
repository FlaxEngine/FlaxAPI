// Copyright (c) 2012-2020 Wojciech Figat. All rights reserved.

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

    partial class Collider
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

            var rigidbody = AttachedRigidBody;
            if (rigidbody)
                rigidbody.OnCollisionEnter(collision);
        }

        internal void OnCollisionExit(Collision collision)
        {
            CollisionExit?.Invoke(collision);

            var rigidbody = AttachedRigidBody;
            if (rigidbody)
                rigidbody.OnCollisionExit(collision);
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

            var rigidbody = AttachedRigidBody;
            if (rigidbody)
                rigidbody.OnTriggerEnter(collider);
        }

        internal void OnTriggerExit(Collider collider)
        {
            TriggerExit?.Invoke(collider);

            var rigidbody = AttachedRigidBody;
            if (rigidbody)
                rigidbody.OnTriggerExit(collider);
        }
    }
}
