// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using FlaxEngine.Assertions;

// ReSharper disable InconsistentNaming

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

        private Collision(int contactsCount)
        {
            _contacts = new ContactPoint[contactsCount];
        }

        internal static List<Collision> _pool = new List<Collision>();
        internal static Collision[] _data;

        internal static Collision GetCollision(int contactsCount)
        {
            for (int i = _pool.Count - 1; i >= 0; i--)
            {
                Collision result = _pool[i];
                if (result.Contacts.Length == contactsCount)
                {
                    _pool.RemoveAt(i);
                    return result;
                }
            }
            return new Collision(contactsCount);
        }

        internal static unsafe Collision[] Internal_ExtractCollisions(byte[] data)
        {
            // Return used collisions to pool
            if (_data != null)
            {
                _pool.AddRange(_data);
                _data = null;
            }

            //CollisionData collisionData = new CollisionData();
            fixed (byte* dataPtr = data)
            {
                using (var memoryStream = new MemoryStream(data, false))
                using (var stream = new BinaryReader(memoryStream))
                {
                    int version = stream.ReadInt32();
                    if (version != 1)
                        return null;
                    int collisionsCount = stream.ReadInt32();

                    int index = 0;
                    _data = new Collision[collisionsCount * 2];
                    for (int i = 0; i < collisionsCount; i++)
                    {
                        var ptr = dataPtr + memoryStream.Position;
                        CollisionData* collisionData = (CollisionData*)ptr;

                        var c1 = GetCollision(collisionData->ContactsCount);
                        var c2 = GetCollision(collisionData->ContactsCount);

                        c1.CopyFrom(collisionData);
                        c2.CopyFrom(c1);
                        c2.SwapObjects();

                        _data[index++] = c1;
                        _data[index++] = c2;
                    }
                }
            }

            return _data;
        }

        internal unsafe void CopyFrom(CollisionData* data)
        {
            _impulse = data->Impulse;
            _velocityA = data->VelocityA;
            _velocityB = data->VelocityB;
            _colliderA = Object.Find<Collider>(ref data->ColliderA);
            _colliderB = Object.Find<Collider>(ref data->ColliderB);

            Assert.AreEqual(data->ContactsCount, _contacts.Length);

            ContactPointData* ptr = &data->Contacts0;
            for (int i = 0; i < data->ContactsCount; i++)
            {
                _contacts[i] = new ContactPoint(ref ptr[i], ref data->ColliderA, ref data->ColliderB);
            }
        }

        internal void CopyFrom(Collision data)
        {
            _impulse = data._impulse;
            _velocityA = data._velocityA;
            _velocityB = data._velocityB;
            _colliderA = data._colliderA;
            _colliderB = data._colliderB;
            _contacts = (ContactPoint[])data._contacts.Clone();
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

        [StructLayout(LayoutKind.Sequential)]
        internal struct ContactPointData
        {
            public Vector3 Point;
            public float Separation;
            public Vector3 Normal;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct CollisionData
        {
            public Guid ColliderA;
            public Guid ColliderB;
            public Vector3 Impulse;
            public Vector3 VelocityA;
            public Vector3 VelocityB;
            public int ContactsCount;

            public ContactPointData Contacts0;
            public ContactPointData Contacts1;
            public ContactPointData Contacts2;
            public ContactPointData Contacts3;
            public ContactPointData Contacts4;
            public ContactPointData Contacts5;
            public ContactPointData Contacts6;
            public ContactPointData Contacts7;
        }
    }
}
