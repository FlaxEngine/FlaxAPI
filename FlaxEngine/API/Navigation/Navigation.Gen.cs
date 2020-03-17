// This code was auto-generated. Do not modify it.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace FlaxEngine
{
    /// <summary>
    /// The result information for navigation mesh queries.
    /// </summary>
    [Tooltip("The result information for navigation mesh queries.")]
    [StructLayout(LayoutKind.Sequential)]
    public unsafe partial struct NavMeshHit
    {
        /// <summary>
        /// The hit point position.
        /// </summary>
        [Tooltip("The hit point position.")]
        public Vector3 Position;

        /// <summary>
        /// The distance to hit point (from the query origin).
        /// </summary>
        [Tooltip("The distance to hit point (from the query origin).")]
        public float Distance;

        /// <summary>
        /// The hit point normal vector.
        /// </summary>
        [Tooltip("The hit point normal vector.")]
        public Vector3 Normal;
    }
}

namespace FlaxEngine
{
    /// <summary>
    /// The navigation service used for path finding and agents navigation system.
    /// </summary>
    [Tooltip("The navigation service used for path finding and agents navigation system.")]
    public static unsafe partial class Navigation
    {
        /// <summary>
        /// Returns true if navigation system is during navmesh building (any request is valid or async task active).
        /// </summary>
        [Tooltip("Returns true if navigation system is during navmesh building (any request is valid or async task active).")]
        public static bool IsBuildingNavMesh
        {
            get { return Internal_IsBuildingNavMesh(); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_IsBuildingNavMesh();

        /// <summary>
        /// Gets the navmesh building progress (normalized to range 0-1).
        /// </summary>
        [Tooltip("The navmesh building progress (normalized to range 0-1).")]
        public static float NavMeshBuildingProgress
        {
            get { return Internal_GetNavMeshBuildingProgress(); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern float Internal_GetNavMeshBuildingProgress();

        /// <summary>
        /// Finds the distance from the specified start position to the nearest polygon wall.
        /// </summary>
        /// <param name="startPosition">The start position.</param>
        /// <param name="hitInfo">The result hit information. Valid only when query succeed.</param>
        /// <param name="maxDistance">The maximum distance to search for wall (search radius).</param>
        /// <returns>True if ray hits an matching object, otherwise false.</returns>
        public static bool FindDistanceToWall(Vector3 startPosition, out NavMeshHit hitInfo, float maxDistance = float.MaxValue)
        {
            return Internal_FindDistanceToWall(ref startPosition, out hitInfo, maxDistance);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_FindDistanceToWall(ref Vector3 startPosition, out NavMeshHit hitInfo, float maxDistance);

        /// <summary>
        /// Finds the path between the two positions presented as a list of waypoints stored in the corners array.
        /// </summary>
        /// <param name="startPosition">The start position.</param>
        /// <param name="endPosition">The end position.</param>
        /// <param name="resultPath">The result path.</param>
        /// <returns>True if found valid path between given two points (it may be partial), otherwise false if failed.</returns>
        public static bool FindPath(Vector3 startPosition, Vector3 endPosition, out Vector3[] resultPath)
        {
            return Internal_FindPath(ref startPosition, ref endPosition, out resultPath, typeof(Vector3));
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_FindPath(ref Vector3 startPosition, ref Vector3 endPosition, out Vector3[] resultPath, System.Type resultArrayItemType0);

        /// <summary>
        /// Projects the point to nav mesh surface (finds the nearest polygon).
        /// </summary>
        /// <param name="point">The source point.</param>
        /// <param name="result">The result position on the navmesh (valid only if method returns true).</param>
        /// <returns>True if found valid location on the navmesh, otherwise false.</returns>
        public static bool ProjectPoint(Vector3 point, out Vector3 result)
        {
            return Internal_ProjectPoint(ref point, out result);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_ProjectPoint(ref Vector3 point, out Vector3 result);

        /// <summary>
        /// Casts a 'walkability' ray along the surface of the navigation mesh from the start position toward the end position.
        /// </summary>
        /// <param name="startPosition">The start position.</param>
        /// <param name="endPosition">The end position.</param>
        /// <param name="hitInfo">The result hit information. Valid only when query succeed.</param>
        /// <returns>True if ray hits an matching object, otherwise false.</returns>
        public static bool RayCast(Vector3 startPosition, Vector3 endPosition, out NavMeshHit hitInfo)
        {
            return Internal_RayCast(ref startPosition, ref endPosition, out hitInfo);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_RayCast(ref Vector3 startPosition, ref Vector3 endPosition, out NavMeshHit hitInfo);

        /// <summary>
        /// Builds the Nav Mesh for the given scene (discards all its tiles).
        /// </summary>
        /// <remarks>
        /// Requests are enqueued till the next game scripts update. Actual navmesh building in done via Thread Pool tasks in a background to prevent game thread stalls.
        /// </remarks>
        /// <param name="scene">The scene.</param>
        /// <param name="timeoutMs">The timeout to wait before building Nav Mesh (in milliseconds).</param>
        public static void BuildNavMesh(Scene scene, float timeoutMs = 50)
        {
            Internal_BuildNavMesh(FlaxEngine.Object.GetUnmanagedPtr(scene), timeoutMs);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_BuildNavMesh(IntPtr scene, float timeoutMs);

        /// <summary>
        /// Builds the Nav Mesh for the given scene (builds only the tiles overlapping the given bounding box).
        /// </summary>
        /// <remarks>
        /// Requests are enqueued till the next game scripts update. Actual navmesh building in done via Thread Pool tasks in a background to prevent game thread stalls.
        /// </remarks>
        /// <param name="scene">The scene.</param>
        /// <param name="dirtyBounds">The bounds in world-space to build overlapping tiles.</param>
        /// <param name="timeoutMs">The timeout to wait before building Nav Mesh (in milliseconds).</param>
        public static void BuildNavMesh(Scene scene, BoundingBox dirtyBounds, float timeoutMs = 50)
        {
            Internal_BuildNavMesh1(FlaxEngine.Object.GetUnmanagedPtr(scene), ref dirtyBounds, timeoutMs);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_BuildNavMesh1(IntPtr scene, ref BoundingBox dirtyBounds, float timeoutMs);
    }
}
