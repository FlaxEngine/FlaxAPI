// This code was auto-generated. Do not modify it.

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace FlaxEngine
{
    /// <summary>
    /// A base class for all colliders.
    /// </summary>
    /// <seealso cref="Actor" />
    /// <seealso cref="PhysicsColliderActor" />
    [Tooltip("A base class for all colliders.")]
    public abstract unsafe partial class Collider : PhysicsColliderActor
    {
        /// <inheritdoc />
        protected Collider() : base()
        {
        }

        /// <summary>
        /// The physical material used to define the collider physical properties.
        /// </summary>
        [EditorOrder(2), DefaultValue(null), AssetReference(typeof(PhysicalMaterial), true), EditorDisplay("Collider")]
        [Tooltip("The physical material used to define the collider physical properties.")]
        public JsonAsset Material
        {
            get { return Internal_GetMaterial(unmanagedPtr); }
            set { Internal_SetMaterial(unmanagedPtr, FlaxEngine.Object.GetUnmanagedPtr(value)); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern JsonAsset Internal_GetMaterial(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetMaterial(IntPtr obj, IntPtr value);

        /// <summary>
        /// Gets or sets the 'IsTrigger' flag.
        /// </summary>
        /// <remarks>
        /// A trigger doesn't register a collision with an incoming Rigidbody. Instead, it sends OnTriggerEnter, OnTriggerExit and OnTriggerStay message when a rigidbody enters or exits the trigger volume.
        /// </remarks>
        [EditorOrder(0), DefaultValue(false), EditorDisplay("Collider")]
        [Tooltip("The 'IsTrigger' flag.")]
        public bool IsTrigger
        {
            get { return Internal_GetIsTrigger(unmanagedPtr); }
            set { Internal_SetIsTrigger(unmanagedPtr, value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_GetIsTrigger(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetIsTrigger(IntPtr obj, bool value);

        /// <summary>
        /// Gets or sets the center of the collider, measured in the object's local space.
        /// </summary>
        [EditorOrder(10), DefaultValue(typeof(Vector3), "0,0,0"), EditorDisplay("Collider")]
        [Tooltip("The center of the collider, measured in the object's local space.")]
        public Vector3 Center
        {
            get { Internal_GetCenter(unmanagedPtr, out var resultAsRef); return resultAsRef; }
            set { Internal_SetCenter(unmanagedPtr, ref value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_GetCenter(IntPtr obj, out Vector3 resultAsRef);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetCenter(IntPtr obj, ref Vector3 value);

        /// <summary>
        /// Gets or sets the contact offset.
        /// </summary>
        /// <remarks>
        /// Colliders whose distance is less than the sum of their ContactOffset values will generate contacts. The contact offset must be positive. Contact offset allows the collision detection system to predictively enforce the contact constraint even when the objects are slightly separated.
        /// </remarks>
        [EditorOrder(1), DefaultValue(10.0f), Limit(0, 100), EditorDisplay("Collider")]
        [Tooltip("The contact offset.")]
        public float ContactOffset
        {
            get { return Internal_GetContactOffset(unmanagedPtr); }
            set { Internal_SetContactOffset(unmanagedPtr, value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern float Internal_GetContactOffset(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetContactOffset(IntPtr obj, float value);

        /// <summary>
        /// Determines whether this collider is attached to the body.
        /// </summary>
        [Tooltip("Determines whether this collider is attached to the body.")]
        public bool IsAttached
        {
            get { return Internal_IsAttached(unmanagedPtr); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_IsAttached(IntPtr obj);

        /// <summary>
        /// Performs a raycast against this collider shape.
        /// </summary>
        /// <param name="origin">The origin of the ray.</param>
        /// <param name="direction">The normalized direction of the ray.</param>
        /// <param name="resultHitDistance">The raycast result hit position distance from the ray origin. Valid only if raycast hits anything.</param>
        /// <param name="maxDistance">The maximum distance the ray should check for collisions.</param>
        /// <returns>True if ray hits an object, otherwise false.</returns>
        public bool RayCast(Vector3 origin, Vector3 direction, out float resultHitDistance, float maxDistance = float.MaxValue)
        {
            return Internal_RayCast(unmanagedPtr, ref origin, ref direction, out resultHitDistance, maxDistance);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_RayCast(IntPtr obj, ref Vector3 origin, ref Vector3 direction, out float resultHitDistance, float maxDistance);

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
            return Internal_RayCast1(unmanagedPtr, ref origin, ref direction, out hitInfo, maxDistance);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_RayCast1(IntPtr obj, ref Vector3 origin, ref Vector3 direction, out RayCastHit hitInfo, float maxDistance);

        /// <summary>
        /// Gets a point on the collider that is closest to a given location. Can be used to find a hit location or position to apply explosion force or any other special effects.
        /// </summary>
        /// <param name="position">The position to find the closest point to it.</param>
        /// <param name="result">The result point on the collider that is closest to the specified location.</param>
        public void ClosestPoint(Vector3 position, out Vector3 result)
        {
            Internal_ClosestPoint(unmanagedPtr, ref position, out result);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_ClosestPoint(IntPtr obj, ref Vector3 position, out Vector3 result);

        /// <summary>
        /// Computes minimum translational distance between two geometry objects.
        /// Translating the first collider by direction * distance will separate the colliders apart if the function returned true. Otherwise, direction and distance are not defined.
        /// The one of the colliders has to be BoxCollider, SphereCollider CapsuleCollider or a convex MeshCollider. The other one can be any type.
        /// If objects do not overlap, the function can not compute the distance and returns false.
        /// </summary>
        /// <param name="colliderA">The first collider.</param>
        /// <param name="colliderB">The second collider.</param>
        /// <param name="direction">The computed direction along which the translation required to separate the colliders apart is minimal. Valid only if function returns true.</param>
        /// <param name="distance">The penetration distance along direction that is required to separate the colliders apart. Valid only if function returns true.</param>
        /// <returns>True if the distance has successfully been computed, i.e. if objects do overlap, otherwise false.</returns>
        public static bool ComputePenetration(Collider colliderA, Collider colliderB, out Vector3 direction, out float distance)
        {
            return Internal_ComputePenetration(FlaxEngine.Object.GetUnmanagedPtr(colliderA), FlaxEngine.Object.GetUnmanagedPtr(colliderB), out direction, out distance);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_ComputePenetration(IntPtr colliderA, IntPtr colliderB, out Vector3 direction, out float distance);
    }
}
