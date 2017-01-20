// Flax Engine scripting API

using System;
using System.Runtime.CompilerServices;

namespace FlaxEngine
{
    public partial class Actor
    {
        // TODO: set direction
        // TODO: Instantiate from prefab
        // TODO: Destroy
        // TODO: TransformDirection, TranformPoint
        // TODO: InverseTransformDirection, InverseTransformPoint
        // TODO: LootAt, Translate, Rotate

        /// <summary>
        /// Returns true if object is fully static on the scene
        /// </summary>
        [UnmanagedCall]
        public bool IsStatic
        {
#if UNIT_TEST_COMPILANT
			get; set;
#else
            get { return Internal_GetStaticFlags(unmanagedPtr) == StaticFlags.FullyStatic; }
#endif
        }

        
        /// <summary>
        /// The rotation as Euler angles in degrees.
        /// The x, y, and z angles represent a rotation z degrees around the z axis, x degrees around the x axis, and y degrees around the y axis (in that order).
        /// Angles order (xyz): pitch, yaw and roll.
        /// </summary>
        [UnmanagedCall]
        public Vector3 EulerAngles
        {
#if UNIT_TEST_COMPILANT
			get { return Orientation.EulerAngles; }
            set
            {
                Quaternion orientation;
                Quaternion.Euler(ref value, out orientation);
                Orientation = orientation;
            }
#else
            get { return Internal_GetOrientation(unmanagedPtr).EulerAngles; }
            set
            {
                Quaternion orientation;
                Quaternion.Euler(ref value, out orientation);
                Internal_SetOrientation(unmanagedPtr, ref orientation);
            }
#endif

        }

        /// <summary>
        /// The local rotation as Euler angles in degrees.
        /// The x, y, and z angles represent a rotation z degrees around the z axis, x degrees around the x axis, and y degrees around the y axis (in that order).
        /// Angles order (xyz): pitch, yaw and roll.
        /// </summary>
        [UnmanagedCall]
        public Vector3 LocaEulerAngles
        {
#if UNIT_TEST_COMPILANT
			get { return LocalOrientation.EulerAngles; }
            set
            {
                Quaternion orientation;
                Quaternion.Euler(ref value, out orientation);
                LocalOrientation = orientation;
            }
#else
            get { return Internal_GetLocalOrientation(unmanagedPtr).EulerAngles; }
            set
            {
                Quaternion orientation;
                Quaternion.Euler(ref value, out orientation);
                Internal_SetLocalOrientation(unmanagedPtr, ref orientation);
            }
#endif
        }

        /// <summary>
        /// Gets actor direction vector
        /// </summary>
        public Vector3 Direction
        {
            get { return Vector3.ForwardLH * Orientation; }
        }

        /// <summary>
        /// Resets actor local transform
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ResetLocalTransform()
        {
            LocalTransform = Transform.Identity;
        }

        /// <summary>
        /// Returns true if actor has parent
        /// </summary>
        [UnmanagedCall]
        public bool HasParent
        {
#if UNIT_TEST_COMPILANT
			get; set;
#else
            get { return Internal_GetParent(unmanagedPtr) != null; }
#endif
        }

        /// <summary>
        /// Returns true if actor has any children
        /// </summary>
        [UnmanagedCall]
        public bool HasChildren
        {
#if UNIT_TEST_COMPILANT
			get; set;
#else
            get { return Internal_GetChildCount(unmanagedPtr) > 0; }
#endif
        }

        /// <summary>
        /// Returns true if actor object has child actor with given name
        /// </summary>
        /// <param name="name">Actor name</param>
        /// <returns></returns>
#if UNIT_TEST_COMPILANT
		[Obsolete("Unit tests, don't support methods calls.")]
#endif
        [UnmanagedCall]
        public bool HasChild(string name)
        {
#if UNIT_TEST_COMPILANT
			throw new NotImplementedException("Unit tests, don't support methods calls. Only properties can be get or set.");
#else
            return Internal_GetChildWithName(unmanagedPtr, name) != null;
#endif

        }

        /// <summary>
        /// Sets actor parent to this object
        /// </summary>
        /// <param name="actor">Actor to link</param>
        /// <param name="worldPositionStays">Should actor world positions remain the same after parent change?</param>
#if UNIT_TEST_COMPILANT
		[Obsolete("Unit tests, don't support methods calls.")]
#endif
        [UnmanagedCall]
        public void AddChild(Actor actor, bool worldPositionStays = true)
        {
#if UNIT_TEST_COMPILANT
			throw new NotImplementedException("Unit tests, don't support methods calls. Only properties can be get or set.");
#else
            actor?.SetParent(this);
#endif
        }

        /// <inheritdoc />
        [UnmanagedCall]
        public override string ToString()
        {
            return $"{GetName} ({GetType().Name})";
        }
    }
}
