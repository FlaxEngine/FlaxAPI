////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System.Collections;

namespace FlaxEngine
{
    /// <summary>
    /// Contains a collision information passed to the OnCollisionEnter/OnCollisionStay/OnCollisionExit events.
    /// </summary>
    public sealed class Collision : IEnumerable
    {
        private Vector3 _impulse;
        private Vector3 _velocityA;
        private Vector3 _velocityB;
        private Collider _colliderA;
        private Collider _colliderB;
        private ContactPoint[] _contacts;

        /// <summary>
        /// The total impulse applied to this contact pair to resolve the collision.
        /// </summary>
        /// <remarks>
        /// The total impulse is obtained by summing up impulses applied at all contact points in this collision pair.
        /// </remarks>
        public Vector3 Impulse => _impulse;

        /// <summary>
        /// The linear velocity of the first colliding object (this instance).
        /// </summary>
        public Vector3 ThisVelocity => _velocityA;

        /// <summary>
        /// The linear velocity of the second colliding object (other instance).
        /// </summary>
        public Vector3 OtherVelocity => _velocityB;

        /// <summary>
        /// The relative linear velocity of the two colliding objects.
        /// </summary>
        /// <remarks>
        /// Can be used to detect stronger collisions.
        /// </remarks>
        public Vector3 RelativeVelocity
        {
            get
            {
                Vector3 result;
                Vector3.Subtract(ref _velocityA, ref _velocityB, out result);
                return result;
            }
        }

        /// <summary>
        /// The first collider (this instance).
        /// </summary>
        public Collider ThisCollider => _colliderA;

        /// <summary>
        /// The second collider (other instance).
        /// </summary>
        public Collider OtherCollider => _colliderB;

        /// <summary>
        /// The contacts locations.
        /// </summary>
        public ContactPoint[] Contacts => _contacts;

        /// <summary>
        /// Gets the contact points enumerator
        /// </summary>
        /// <returns>The contact points enumerator.</returns>
        public IEnumerator GetEnumerator()
        {
            return _contacts.GetEnumerator();
        }

        internal void SwapObjects()
        {
            var tmp1 = _velocityA;
            _velocityA = _velocityB;
            _velocityB = tmp1;

            var tmp2 = _colliderA;
            _colliderA = _colliderB;
            _colliderB = tmp2;

            for (int i = 0; i < _contacts.Length; i++)
                _contacts[i].SwapObjects();
        }
    }
}
