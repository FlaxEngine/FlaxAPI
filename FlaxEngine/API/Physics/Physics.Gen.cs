// This code was auto-generated. Do not modify it.

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace FlaxEngine
{
    /// <summary>
    /// Raycast hit result data.
    /// </summary>
    [Tooltip("Raycast hit result data.")]
    [StructLayout(LayoutKind.Sequential)]
    public unsafe partial struct RayCastHit
    {
        /// <summary>
        /// The collider that was hit.
        /// </summary>
        [Tooltip("The collider that was hit.")]
        public PhysicsColliderActor Collider;

        /// <summary>
        /// The normal of the surface the ray hit.
        /// </summary>
        [Tooltip("The normal of the surface the ray hit.")]
        public Vector3 Normal;

        /// <summary>
        /// The distance from the ray's origin to the hit location.
        /// </summary>
        [Tooltip("The distance from the ray's origin to the hit location.")]
        public float Distance;

        /// <summary>
        /// The point in the world space where ray hit the collider.
        /// </summary>
        [Tooltip("The point in the world space where ray hit the collider.")]
        public Vector3 Point;

        /// <summary>
        /// The barycentric coordinates of hit point, for triangle mesh and height field.
        /// </summary>
        [Tooltip("The barycentric coordinates of hit point, for triangle mesh and height field.")]
        public Vector2 UV;
    }
}

namespace FlaxEngine
{
    /// <summary>
    /// Physics simulation system.
    /// </summary>
    [Tooltip("Physics simulation system.")]
    public static unsafe partial class Physics
    {
        /// <summary>
        /// The automatic simulation feature. True if perform physics simulation after on fixed update by auto, otherwise user should do it.
        /// </summary>
        [Tooltip("The automatic simulation feature. True if perform physics simulation after on fixed update by auto, otherwise user should do it.")]
        public static bool AutoSimulation
        {
            get { return Internal_GetAutoSimulation(); }
            set { Internal_SetAutoSimulation(value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_GetAutoSimulation();

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetAutoSimulation(bool value);

        /// <summary>
        /// Gets or sets the current gravity force.
        /// </summary>
        [Tooltip("The current gravity force.")]
        public static Vector3 Gravity
        {
            get { Internal_GetGravity(out var resultAsRef); return resultAsRef; }
            set { Internal_SetGravity(ref value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_GetGravity(out Vector3 resultAsRef);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetGravity(ref Vector3 value);

        /// <summary>
        /// Gets or sets the CCD feature enable flag.
        /// </summary>
        [Tooltip("The CCD feature enable flag.")]
        public static bool EnableCCD
        {
            get { return Internal_GetEnableCCD(); }
            set { Internal_SetEnableCCD(value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_GetEnableCCD();

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetEnableCCD(bool value);

        /// <summary>
        /// Gets or sets the minimum relative velocity required for an object to bounce.
        /// </summary>
        [Tooltip("The minimum relative velocity required for an object to bounce.")]
        public static float BounceThresholdVelocity
        {
            get { return Internal_GetBounceThresholdVelocity(); }
            set { Internal_SetBounceThresholdVelocity(value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern float Internal_GetBounceThresholdVelocity();

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetBounceThresholdVelocity(float value);

        /// <summary>
        /// Checks if physical simulation is running
        /// </summary>
        [Tooltip("Checks if physical simulation is running")]
        public static bool IsDuringSimulation
        {
            get { return Internal_IsDuringSimulation(); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_IsDuringSimulation();

        /// <summary>
        /// Performs a raycast against objects in the scene.
        /// </summary>
        /// <param name="origin">The origin of the ray.</param>
        /// <param name="direction">The normalized direction of the ray.</param>
        /// <param name="maxDistance">The maximum distance the ray should check for collisions.</param>
        /// <param name="layerMask">The layer mask used to filter the results.</param>
        /// <param name="hitTriggers">If set to <c>true</c> triggers will be hit, otherwise will skip them.</param>
        /// <returns>True if ray hits an matching object, otherwise false.</returns>
        public static bool RayCast(Vector3 origin, Vector3 direction, float maxDistance = float.MaxValue, uint layerMask = uint.MaxValue, bool hitTriggers = true)
        {
            return Internal_RayCast(ref origin, ref direction, maxDistance, layerMask, hitTriggers);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_RayCast(ref Vector3 origin, ref Vector3 direction, float maxDistance, uint layerMask, bool hitTriggers);

        /// <summary>
        /// Performs a raycast against objects in the scene, returns results in a RayCastHit structure.
        /// </summary>
        /// <param name="origin">The origin of the ray.</param>
        /// <param name="direction">The normalized direction of the ray.</param>
        /// <param name="hitInfo">The result hit information. Valid only when method returns true.</param>
        /// <param name="maxDistance">The maximum distance the ray should check for collisions.</param>
        /// <param name="layerMask">The layer mask used to filter the results.</param>
        /// <param name="hitTriggers">If set to <c>true</c> triggers will be hit, otherwise will skip them.</param>
        /// <returns>True if ray hits an matching object, otherwise false.</returns>
        public static bool RayCast(Vector3 origin, Vector3 direction, out RayCastHit hitInfo, float maxDistance = float.MaxValue, uint layerMask = uint.MaxValue, bool hitTriggers = true)
        {
            return Internal_RayCast1(ref origin, ref direction, out hitInfo, maxDistance, layerMask, hitTriggers);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_RayCast1(ref Vector3 origin, ref Vector3 direction, out RayCastHit hitInfo, float maxDistance, uint layerMask, bool hitTriggers);

        /// <summary>
        /// Performs a raycast against objects in the scene, returns results in a RayCastHit structure.
        /// </summary>
        /// <param name="origin">The origin of the ray.</param>
        /// <param name="direction">The normalized direction of the ray.</param>
        /// <param name="results">The result hits. Valid only when method returns true.</param>
        /// <param name="maxDistance">The maximum distance the ray should check for collisions.</param>
        /// <param name="layerMask">The layer mask used to filter the results.</param>
        /// <param name="hitTriggers">If set to <c>true</c> triggers will be hit, otherwise will skip them.</param>
        /// <returns>True if ray hits an matching object, otherwise false.</returns>
        public static bool RayCastAll(Vector3 origin, Vector3 direction, out RayCastHit[] results, float maxDistance = float.MaxValue, uint layerMask = uint.MaxValue, bool hitTriggers = true)
        {
            return Internal_RayCastAll(ref origin, ref direction, out results, maxDistance, layerMask, hitTriggers, typeof(RayCastHit));
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_RayCastAll(ref Vector3 origin, ref Vector3 direction, out RayCastHit[] results, float maxDistance, uint layerMask, bool hitTriggers, System.Type resultArrayItemType0);

        /// <summary>
        /// Performs a sweep test against objects in the scene using a box geometry.
        /// </summary>
        /// <param name="center">The box center.</param>
        /// <param name="halfExtents">The half size of the box in each direction.</param>
        /// <param name="direction">The normalized direction in which cast a box.</param>
        /// <param name="rotation">The box rotation.</param>
        /// <param name="maxDistance">The maximum distance the ray should check for collisions.</param>
        /// <param name="layerMask">The layer mask used to filter the results.</param>
        /// <param name="hitTriggers">If set to <c>true</c> triggers will be hit, otherwise will skip them.</param>
        /// <returns>True if box hits an matching object, otherwise false.</returns>
        public static bool BoxCast(Vector3 center, Vector3 halfExtents, Vector3 direction, Quaternion rotation, float maxDistance = float.MaxValue, uint layerMask = uint.MaxValue, bool hitTriggers = true)
        {
            return Internal_BoxCast(ref center, ref halfExtents, ref direction, ref rotation, maxDistance, layerMask, hitTriggers);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_BoxCast(ref Vector3 center, ref Vector3 halfExtents, ref Vector3 direction, ref Quaternion rotation, float maxDistance, uint layerMask, bool hitTriggers);

        /// <summary>
        /// Performs a sweep test against objects in the scene using a box geometry.
        /// </summary>
        /// <param name="center">The box center.</param>
        /// <param name="halfExtents">The half size of the box in each direction.</param>
        /// <param name="direction">The normalized direction in which cast a box.</param>
        /// <param name="hitInfo">The result hit information. Valid only when method returns true.</param>
        /// <param name="rotation">The box rotation.</param>
        /// <param name="maxDistance">The maximum distance the ray should check for collisions.</param>
        /// <param name="layerMask">The layer mask used to filter the results.</param>
        /// <param name="hitTriggers">If set to <c>true</c> triggers will be hit, otherwise will skip them.</param>
        /// <returns>True if box hits an matching object, otherwise false.</returns>
        public static bool BoxCast(Vector3 center, Vector3 halfExtents, Vector3 direction, out RayCastHit hitInfo, Quaternion rotation, float maxDistance = float.MaxValue, uint layerMask = uint.MaxValue, bool hitTriggers = true)
        {
            return Internal_BoxCast1(ref center, ref halfExtents, ref direction, out hitInfo, ref rotation, maxDistance, layerMask, hitTriggers);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_BoxCast1(ref Vector3 center, ref Vector3 halfExtents, ref Vector3 direction, out RayCastHit hitInfo, ref Quaternion rotation, float maxDistance, uint layerMask, bool hitTriggers);

        /// <summary>
        /// Performs a sweep test against objects in the scene using a box geometry.
        /// </summary>
        /// <param name="center">The box center.</param>
        /// <param name="halfExtents">The half size of the box in each direction.</param>
        /// <param name="direction">The normalized direction in which cast a box.</param>
        /// <param name="results">The result hits. Valid only when method returns true.</param>
        /// <param name="rotation">The box rotation.</param>
        /// <param name="maxDistance">The maximum distance the ray should check for collisions.</param>
        /// <param name="layerMask">The layer mask used to filter the results.</param>
        /// <param name="hitTriggers">If set to <c>true</c> triggers will be hit, otherwise will skip them.</param>
        /// <returns>True if box hits an matching object, otherwise false.</returns>
        public static bool BoxCastAll(Vector3 center, Vector3 halfExtents, Vector3 direction, out RayCastHit[] results, Quaternion rotation, float maxDistance = float.MaxValue, uint layerMask = uint.MaxValue, bool hitTriggers = true)
        {
            return Internal_BoxCastAll(ref center, ref halfExtents, ref direction, out results, ref rotation, maxDistance, layerMask, hitTriggers, typeof(RayCastHit));
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_BoxCastAll(ref Vector3 center, ref Vector3 halfExtents, ref Vector3 direction, out RayCastHit[] results, ref Quaternion rotation, float maxDistance, uint layerMask, bool hitTriggers, System.Type resultArrayItemType0);

        /// <summary>
        /// Performs a sweep test against objects in the scene using a sphere geometry.
        /// </summary>
        /// <param name="center">The sphere center.</param>
        /// <param name="radius">The radius of the sphere.</param>
        /// <param name="direction">The normalized direction in which cast a sphere.</param>
        /// <param name="maxDistance">The maximum distance the ray should check for collisions.</param>
        /// <param name="layerMask">The layer mask used to filter the results.</param>
        /// <param name="hitTriggers">If set to <c>true</c> triggers will be hit, otherwise will skip them.</param>
        /// <returns>True if sphere hits an matching object, otherwise false.</returns>
        public static bool SphereCast(Vector3 center, float radius, Vector3 direction, float maxDistance = float.MaxValue, uint layerMask = uint.MaxValue, bool hitTriggers = true)
        {
            return Internal_SphereCast(ref center, radius, ref direction, maxDistance, layerMask, hitTriggers);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_SphereCast(ref Vector3 center, float radius, ref Vector3 direction, float maxDistance, uint layerMask, bool hitTriggers);

        /// <summary>
        /// Performs a sweep test against objects in the scene using a sphere geometry.
        /// </summary>
        /// <param name="center">The sphere center.</param>
        /// <param name="radius">The radius of the sphere.</param>
        /// <param name="direction">The normalized direction in which cast a sphere.</param>
        /// <param name="hitInfo">The result hit information. Valid only when method returns true.</param>
        /// <param name="maxDistance">The maximum distance the ray should check for collisions.</param>
        /// <param name="layerMask">The layer mask used to filter the results.</param>
        /// <param name="hitTriggers">If set to <c>true</c> triggers will be hit, otherwise will skip them.</param>
        /// <returns>True if sphere hits an matching object, otherwise false.</returns>
        public static bool SphereCast(Vector3 center, float radius, Vector3 direction, out RayCastHit hitInfo, float maxDistance = float.MaxValue, uint layerMask = uint.MaxValue, bool hitTriggers = true)
        {
            return Internal_SphereCast1(ref center, radius, ref direction, out hitInfo, maxDistance, layerMask, hitTriggers);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_SphereCast1(ref Vector3 center, float radius, ref Vector3 direction, out RayCastHit hitInfo, float maxDistance, uint layerMask, bool hitTriggers);

        /// <summary>
        /// Performs a sweep test against objects in the scene using a sphere geometry.
        /// </summary>
        /// <param name="center">The sphere center.</param>
        /// <param name="radius">The radius of the sphere.</param>
        /// <param name="direction">The normalized direction in which cast a sphere.</param>
        /// <param name="results">The result hits. Valid only when method returns true.</param>
        /// <param name="maxDistance">The maximum distance the ray should check for collisions.</param>
        /// <param name="layerMask">The layer mask used to filter the results.</param>
        /// <param name="hitTriggers">If set to <c>true</c> triggers will be hit, otherwise will skip them.</param>
        /// <returns>True if sphere hits an matching object, otherwise false.</returns>
        public static bool SphereCastAll(Vector3 center, float radius, Vector3 direction, out RayCastHit[] results, float maxDistance = float.MaxValue, uint layerMask = uint.MaxValue, bool hitTriggers = true)
        {
            return Internal_SphereCastAll(ref center, radius, ref direction, out results, maxDistance, layerMask, hitTriggers, typeof(RayCastHit));
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_SphereCastAll(ref Vector3 center, float radius, ref Vector3 direction, out RayCastHit[] results, float maxDistance, uint layerMask, bool hitTriggers, System.Type resultArrayItemType0);

        /// <summary>
        /// Checks whether the given box overlaps with other colliders or not.
        /// </summary>
        /// <param name="center">The box center.</param>
        /// <param name="halfExtents">The half size of the box in each direction.</param>
        /// <param name="rotation">The box rotation.</param>
        /// <param name="layerMask">The layer mask used to filter the results.</param>
        /// <param name="hitTriggers">If set to <c>true</c> triggers will be hit, otherwise will skip them.</param>
        /// <returns>True if box overlaps any matching object, otherwise false.</returns>
        public static bool CheckBox(Vector3 center, Vector3 halfExtents, Quaternion rotation, uint layerMask = uint.MaxValue, bool hitTriggers = true)
        {
            return Internal_CheckBox(ref center, ref halfExtents, ref rotation, layerMask, hitTriggers);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_CheckBox(ref Vector3 center, ref Vector3 halfExtents, ref Quaternion rotation, uint layerMask, bool hitTriggers);

        /// <summary>
        /// Checks whether the given sphere overlaps with other colliders or not.
        /// </summary>
        /// <param name="center">The sphere center.</param>
        /// <param name="radius">The radius of the sphere.</param>
        /// <param name="layerMask">The layer mask used to filter the results.</param>
        /// <param name="hitTriggers">If set to <c>true</c> triggers will be hit, otherwise will skip them.</param>
        /// <returns>True if sphere overlaps any matching object, otherwise false.</returns>
        public static bool CheckSphere(Vector3 center, float radius, uint layerMask = uint.MaxValue, bool hitTriggers = true)
        {
            return Internal_CheckSphere(ref center, radius, layerMask, hitTriggers);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_CheckSphere(ref Vector3 center, float radius, uint layerMask, bool hitTriggers);

        /// <summary>
        /// Finds all colliders touching or inside of the given box.
        /// </summary>
        /// <param name="center">The box center.</param>
        /// <param name="halfExtents">The half size of the box in each direction.</param>
        /// <param name="rotation">The box rotation.</param>
        /// <param name="results">The result colliders that overlap with the given box. Valid only when method returns true.</param>
        /// <param name="layerMask">The layer mask used to filter the results.</param>
        /// <param name="hitTriggers">If set to <c>true</c> triggers will be hit, otherwise will skip them.</param>
        /// <returns>True if box overlaps any matching object, otherwise false.</returns>
        public static bool OverlapBox(Vector3 center, Vector3 halfExtents, out Collider[] results, Quaternion rotation, uint layerMask = uint.MaxValue, bool hitTriggers = true)
        {
            return Internal_OverlapBox(ref center, ref halfExtents, out results, ref rotation, layerMask, hitTriggers, typeof(Collider));
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_OverlapBox(ref Vector3 center, ref Vector3 halfExtents, out Collider[] results, ref Quaternion rotation, uint layerMask, bool hitTriggers, System.Type resultArrayItemType0);

        /// <summary>
        /// Finds all colliders touching or inside of the given sphere.
        /// </summary>
        /// <param name="center">The sphere center.</param>
        /// <param name="radius">The radius of the sphere.</param>
        /// <param name="results">The result colliders that overlap with the given sphere. Valid only when method returns true.</param>
        /// <param name="layerMask">The layer mask used to filter the results.</param>
        /// <param name="hitTriggers">If set to <c>true</c> triggers will be hit, otherwise will skip them.</param>
        /// <returns>True if sphere overlaps any matching object, otherwise false.</returns>
        public static bool OverlapSphere(Vector3 center, float radius, out Collider[] results, uint layerMask = uint.MaxValue, bool hitTriggers = true)
        {
            return Internal_OverlapSphere(ref center, radius, out results, layerMask, hitTriggers, typeof(Collider));
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_OverlapSphere(ref Vector3 center, float radius, out Collider[] results, uint layerMask, bool hitTriggers, System.Type resultArrayItemType0);

        /// <summary>
        /// Finds all colliders touching or inside of the given box.
        /// </summary>
        /// <param name="center">The box center.</param>
        /// <param name="halfExtents">The half size of the box in each direction.</param>
        /// <param name="rotation">The box rotation.</param>
        /// <param name="results">The result colliders that overlap with the given box. Valid only when method returns true.</param>
        /// <param name="layerMask">The layer mask used to filter the results.</param>
        /// <param name="hitTriggers">If set to <c>true</c> triggers will be hit, otherwise will skip them.</param>
        /// <returns>True if box overlaps any matching object, otherwise false.</returns>
        public static bool OverlapBox(Vector3 center, Vector3 halfExtents, out PhysicsColliderActor[] results, Quaternion rotation, uint layerMask = uint.MaxValue, bool hitTriggers = true)
        {
            return Internal_OverlapBox1(ref center, ref halfExtents, out results, ref rotation, layerMask, hitTriggers, typeof(PhysicsColliderActor));
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_OverlapBox1(ref Vector3 center, ref Vector3 halfExtents, out PhysicsColliderActor[] results, ref Quaternion rotation, uint layerMask, bool hitTriggers, System.Type resultArrayItemType0);

        /// <summary>
        /// Finds all colliders touching or inside of the given sphere.
        /// </summary>
        /// <param name="center">The sphere center.</param>
        /// <param name="radius">The radius of the sphere.</param>
        /// <param name="results">The result colliders that overlap with the given sphere. Valid only when method returns true.</param>
        /// <param name="layerMask">The layer mask used to filter the results.</param>
        /// <param name="hitTriggers">If set to <c>true</c> triggers will be hit, otherwise will skip them.</param>
        /// <returns>True if sphere overlaps any matching object, otherwise false.</returns>
        public static bool OverlapSphere(Vector3 center, float radius, out PhysicsColliderActor[] results, uint layerMask = uint.MaxValue, bool hitTriggers = true)
        {
            return Internal_OverlapSphere1(ref center, radius, out results, layerMask, hitTriggers, typeof(PhysicsColliderActor));
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_OverlapSphere1(ref Vector3 center, float radius, out PhysicsColliderActor[] results, uint layerMask, bool hitTriggers, System.Type resultArrayItemType0);

        /// <summary>
        /// Called during main engine loop to start physic simulation. Use CollectResults after.
        /// </summary>
        /// <param name="dt">The delta time (in seconds).</param>
        public static void Simulate(float dt)
        {
            Internal_Simulate(dt);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_Simulate(float dt);

        /// <summary>
        /// Called during main engine loop to collect physic simulation results and apply them as well as fire collision events.
        /// </summary>
        public static void CollectResults()
        {
            Internal_CollectResults();
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_CollectResults();
    }
}
