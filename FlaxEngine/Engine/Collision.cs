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
        private Actor _actorA;
        private Actor _actorB;
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
        /// The first collider (this instance). It may be null if this actor is not the <see cref="Collider"/> (eg. <see cref="Terrain"/>).
        /// </summary>
        public Collider ThisCollider => _actorA as Collider;

        /// <summary>
        /// The second collider (other instance). It may be null if this actor is not the <see cref="Collider"/> (eg. <see cref="Terrain"/>).
        /// </summary>
        public Collider OtherCollider => _actorB as Collider;

        /// <summary>
        /// The first collider (this instance).
        /// </summary>
        public Actor ThisActor => _actorA;

        /// <summary>
        /// The second collider (other instance).
        /// </summary>
        public Actor OtherActor => _actorB;

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
        internal static int _dataUsed;

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
            fixed (byte* dataPtr = data)
            {
                using (var memoryStream = new MemoryStream(data, false))
                using (var stream = new BinaryReader(memoryStream))
                {
                    int version = stream.ReadInt32();
                    if (version != 1)
                        return null;
                    int collisionsCount = stream.ReadInt32();

                    if (_data == null || _data.Length < collisionsCount * 2)
                        _data = new Collision[(int)(collisionsCount * 2 * 1.5f)];
                    _dataUsed = collisionsCount * 2;

                    int index = 0;
                    for (int i = 0; i < collisionsCount; i++)
                    {
                        var ptr = dataPtr + memoryStream.Position;
                        CollisionData* collisionData = ((CollisionData*)ptr) + i;

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

        internal static void Internal_SendCollisions(int newStart, int newCount, int removedStart, int removedCount)
        {
            Collision c;

            for (int i = 0; i < newCount;)
            {
                c = _data[newStart + i++];
                c.ThisCollider?.OnCollisionEnter(c);
                c = _data[newStart + i++];
                c.ThisCollider?.OnCollisionEnter(c);
            }

            for (int i = 0; i < removedCount;)
            {
                c = _data[removedStart + i++];
                c.ThisCollider?.OnCollisionExit(c);
                c = _data[removedStart + i++];
                c.ThisCollider?.OnCollisionExit(c);
            }

            for (int i = 0; i < _dataUsed; i++)
                _pool.Add(_data[i]);
            Array.Clear(_data, 0, _dataUsed);
            _dataUsed = 0;
        }

        internal unsafe void CopyFrom(CollisionData* data)
        {
            _impulse = data->Impulse;
            _velocityA = data->VelocityA;
            _velocityB = data->VelocityB;
            _actorA = Object.Find<Actor>(ref data->ActorA);
            _actorB = Object.Find<Actor>(ref data->ActorB);

            Assert.AreEqual(data->ContactsCount, _contacts.Length);

            ContactPointData* ptr = &data->Contacts0;
            for (int i = 0; i < data->ContactsCount; i++)
            {
                _contacts[i] = new ContactPoint(ref ptr[i], ref data->ActorA, ref data->ActorB);
            }
        }

        internal void CopyFrom(Collision data)
        {
            _impulse = data._impulse;
            _velocityA = data._velocityA;
            _velocityB = data._velocityB;
            _actorA = data._actorA;
            _actorB = data._actorB;

            for (int i = 0; i < _contacts.Length; i++)
                _contacts[i] = data._contacts[i];
        }

        internal void SwapObjects()
        {
            var tmp1 = _velocityA;
            _velocityA = _velocityB;
            _velocityB = tmp1;

            var tmp2 = _actorA;
            _actorA = _actorB;
            _actorB = tmp2;

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
            public Guid ActorA;
            public Guid ActorB;
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
