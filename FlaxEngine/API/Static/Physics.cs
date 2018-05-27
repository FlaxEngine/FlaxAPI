// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System.Runtime.CompilerServices;

namespace FlaxEngine
{
    public static partial class Physics
    {
        /// <summary>
        /// Performs a raycast against objects in the scene.
        /// </summary>
        /// <param name="origin">The origin of the ray.</param>
        /// <param name="direction">The normalized direction of the ray.</param>
        /// <param name="maxDistance">The maximum distance the ray should check for collisions.</param>
        /// <param name="layerMask">The layer mask used to filter the results.</param>
        /// <param name="hitTriggers">If set to <c>true</c> triggers will be hit, otherwise will skip them.</param>
        /// <returns>True if ray hits an matching object, otherwise false.</returns>
        public static bool RayCast(Vector3 origin, Vector3 direction, float maxDistance = float.MaxValue, int layerMask = int.MaxValue, bool hitTriggers = true)
        {
            return Internal_RayCast1(ref origin, ref direction, maxDistance, layerMask, hitTriggers);
        }

        /// <summary>
        /// Performs a raycast against objects in the scene, returns results in a RaycastHit structure.
        /// </summary>
        /// <param name="origin">The origin of the ray.</param>
        /// <param name="direction">The normalized direction of the ray.</param>
        /// <param name="hitInfo">The result hit information. Valid only when method returns true.</param>
        /// <param name="maxDistance">The maximum distance the ray should check for collisions.</param>
        /// <param name="layerMask">The layer mask used to filter the results.</param>
        /// <param name="hitTriggers">If set to <c>true</c> triggers will be hit, otherwise will skip them.</param>
        /// <returns>True if ray hits an matching object, otherwise false.</returns>
        public static bool RayCast(Vector3 origin, Vector3 direction, out RayCastHit hitInfo, float maxDistance = float.MaxValue, int layerMask = int.MaxValue, bool hitTriggers = true)
        {
            return Internal_RayCast2(ref origin, ref direction, out hitInfo, maxDistance, layerMask, hitTriggers);
        }

        /// <summary>
        /// Performs a raycast against objects in the scene, returns results in a RaycastHit structure.
        /// </summary>
        /// <param name="origin">The origin of the ray.</param>
        /// <param name="direction">The normalized direction of the ray.</param>
        /// <param name="maxDistance">The maximum distance the ray should check for collisions.</param>
        /// <param name="layerMask">The layer mask used to filter the results.</param>
        /// <param name="hitTriggers">If set to <c>true</c> triggers will be hit, otherwise will skip them.</param>
        /// <returns>The result hits.</returns>
        public static RayCastHit[] RayCastAll(Vector3 origin, Vector3 direction, float maxDistance = float.MaxValue, int layerMask = int.MaxValue, bool hitTriggers = true)
        {
            return Internal_RayCast3(ref origin, ref direction, maxDistance, layerMask, hitTriggers);
        }

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
        public static bool BoxCast(Vector3 center, Vector3 halfExtents, Vector3 direction, Quaternion rotation, float maxDistance = float.MaxValue, int layerMask = int.MaxValue, bool hitTriggers = true)
        {
            return Internal_BoxCast1(ref center, ref halfExtents, ref direction, ref rotation, maxDistance, layerMask, hitTriggers);
        }

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
        public static bool BoxCast(Vector3 center, Vector3 halfExtents, Vector3 direction, out RayCastHit hitInfo, Quaternion rotation, float maxDistance = float.MaxValue, int layerMask = int.MaxValue, bool hitTriggers = true)
        {
            return Internal_BoxCast2(ref center, ref halfExtents, ref direction, out hitInfo, ref rotation, maxDistance, layerMask, hitTriggers);
        }

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
        /// <returns>The result hits.</returns>
        public static RayCastHit[] BoxCastAll(Vector3 center, Vector3 halfExtents, Vector3 direction, Quaternion rotation, float maxDistance = float.MaxValue, int layerMask = int.MaxValue, bool hitTriggers = true)
        {
            return Internal_BoxCast3(ref center, ref halfExtents, ref direction, ref rotation, maxDistance, layerMask, hitTriggers);
        }

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
        public static bool SphereCast(Vector3 center, float radius, Vector3 direction, float maxDistance = float.MaxValue, int layerMask = int.MaxValue, bool hitTriggers = true)
        {
            return Internal_SphereCast1(ref center, radius, ref direction, maxDistance, layerMask, hitTriggers);
        }

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
        public static bool SphereCast(Vector3 center, float radius, Vector3 direction, out RayCastHit hitInfo, float maxDistance = float.MaxValue, int layerMask = int.MaxValue, bool hitTriggers = true)
        {
            return Internal_SphereCast2(ref center, radius, ref direction, out hitInfo, maxDistance, layerMask, hitTriggers);
        }

        /// <summary>
        /// Performs a sweep test against objects in the scene using a sphere geometry.
        /// </summary>
        /// <param name="center">The sphere center.</param>
        /// <param name="radius">The radius of the sphere.</param>
        /// <param name="direction">The normalized direction in which cast a sphere.</param>
        /// <param name="maxDistance">The maximum distance the ray should check for collisions.</param>
        /// <param name="layerMask">The layer mask used to filter the results.</param>
        /// <param name="hitTriggers">If set to <c>true</c> triggers will be hit, otherwise will skip them.</param>
        /// <returns>The result hits.</returns>
        public static RayCastHit[] SphereCastAll(Vector3 center, float radius, Vector3 direction, float maxDistance = float.MaxValue, int layerMask = int.MaxValue, bool hitTriggers = true)
        {
            return Internal_SphereCast3(ref center, radius, ref direction, maxDistance, layerMask, hitTriggers);
        }

        /// <summary>
        /// Checks whether the given box overlaps with other colliders or not.
        /// </summary>
        /// <param name="center">The box center.</param>
        /// <param name="halfExtents">The half size of the box in each direction.</param>
        /// <param name="rotation">The box rotation.</param>
        /// <param name="layerMask">The layer mask used to filter the results.</param>
        /// <param name="hitTriggers">If set to <c>true</c> triggers will be hit, otherwise will skip them.</param>
        /// <returns>True if box overlaps any matching object, otherwise false.</returns>
        public static bool CheckBox(Vector3 center, Vector3 halfExtents, Quaternion rotation, int layerMask = int.MaxValue, bool hitTriggers = true)
        {
            return Internal_CheckBox(ref center, ref halfExtents, ref rotation, layerMask, hitTriggers);
        }

        /// <summary>
        /// Checks whether the given sphere overlaps with other colliders or not.
        /// </summary>
        /// <param name="center">The sphere center.</param>
        /// <param name="radius">The radius of the sphere.</param>
        /// <param name="layerMask">The layer mask used to filter the results.</param>
        /// <param name="hitTriggers">If set to <c>true</c> triggers will be hit, otherwise will skip them.</param>
        /// <returns>True if sphere overlaps any matching object, otherwise false.</returns>
        public static bool CheckSphere(Vector3 center, float radius, int layerMask = int.MaxValue, bool hitTriggers = true)
        {
            return Internal_CheckSphere(ref center, radius, layerMask, hitTriggers);
        }

        /// <summary>
        /// Finds all colliders touching or inside of the given box.
        /// </summary>
        /// <param name="center">The box center.</param>
        /// <param name="halfExtents">The half size of the box in each direction.</param>
        /// <param name="rotation">The box rotation.</param>
        /// <param name="layerMask">The layer mask used to filter the results.</param>
        /// <param name="hitTriggers">If set to <c>true</c> triggers will be hit, otherwise will skip them.</param>
        /// <returns>The result colliders that overlap with the given box.</returns>
        public static Collider[] OverlapBox(Vector3 center, Vector3 halfExtents, Quaternion rotation, int layerMask = int.MaxValue, bool hitTriggers = true)
        {
            return Internal_OverlapBox(ref center, ref halfExtents, ref rotation, layerMask, hitTriggers);
        }

        /// <summary>
        /// Finds all colliders touching or inside of the given sphere.
        /// </summary>
        /// <param name="center">The sphere center.</param>
        /// <param name="radius">The radius of the sphere.</param>
        /// <param name="layerMask">The layer mask used to filter the results.</param>
        /// <param name="hitTriggers">If set to <c>true</c> triggers will be hit, otherwise will skip them.</param>
        /// <returns>The result colliders that overlap with the given sphere.</returns>
        public static Collider[] OverlapSphere(Vector3 center, float radius, int layerMask = int.MaxValue, bool hitTriggers = true)
        {
            return Internal_OverlapSphere(ref center, radius, layerMask, hitTriggers);
        }

        #region Internal Calls

#if !UNIT_TEST_COMPILANT
        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_RayCast1(ref Vector3 origin, ref Vector3 direction, float maxDistance, int layerMask, bool hitTriggers);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_RayCast2(ref Vector3 origin, ref Vector3 direction, out RayCastHit hitInfo, float maxDistance, int layerMask, bool hitTriggers);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern RayCastHit[] Internal_RayCast3(ref Vector3 origin, ref Vector3 direction, float maxDistance, int layerMask, bool hitTriggers);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_BoxCast1(ref Vector3 center, ref Vector3 halfExtents, ref Vector3 direction, ref Quaternion rotation, float maxDistance, int layerMask, bool hitTriggers);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_BoxCast2(ref Vector3 center, ref Vector3 halfExtents, ref Vector3 direction, out RayCastHit hitInfo, ref Quaternion rotation, float maxDistance, int layerMask, bool hitTriggers);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern RayCastHit[] Internal_BoxCast3(ref Vector3 center, ref Vector3 halfExtents, ref Vector3 direction, ref Quaternion rotation, float maxDistance, int layerMask, bool hitTriggers);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_SphereCast1(ref Vector3 center, float radius, ref Vector3 direction, float maxDistance, int layerMask, bool hitTriggers);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_SphereCast2(ref Vector3 center, float radius, ref Vector3 direction, out RayCastHit hitInfo, float maxDistance, int layerMask, bool hitTriggers);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern RayCastHit[] Internal_SphereCast3(ref Vector3 center, float radius, ref Vector3 direction, float maxDistance, int layerMask, bool hitTriggers);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_CheckBox(ref Vector3 center, ref Vector3 halfExtents, ref Quaternion rotation, int layerMask, bool hitTriggers);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_CheckSphere(ref Vector3 center, float radius, int layerMask, bool hitTriggers);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern Collider[] Internal_OverlapBox(ref Vector3 center, ref Vector3 halfExtents, ref Quaternion rotation, int layerMask, bool hitTriggers);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern Collider[] Internal_OverlapSphere(ref Vector3 center, float radius, int layerMask, bool hitTriggers);
#endif

        #endregion
    }
}
