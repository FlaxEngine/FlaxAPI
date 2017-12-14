////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Runtime.CompilerServices;

namespace FlaxEngine
{
    /// <summary>
    /// A <see cref="CollisionData"/> storage data type.
    /// </summary>
    public enum CollisionDataType
    {
        /// <summary>
        /// Nothing.
        /// </summary>
        None = 0,

        /// <summary>
        /// A convex polyhedron represented as a set of vertices and polygonal faces. The number of vertices and faces of a convex mesh is limited to 255.
        /// </summary>
        ConvexMesh = 1,

        /// <summary>
        /// A collision triangle mesh consists of a collection of vertices and the triangle indices.
        /// </summary>
        TriangleMesh = 2,
    }

    /// <summary>
    /// Set of flags used to generate model convex mesh. Allows to customize process.
    /// </summary>
    public enum ConvexMeshGenerationFlags
    {
        /// <summary>
        /// Nothing.
        /// </summary>
        None = 0,

        /// <summary>
        /// Disables the convex mesh validation to speed-up hull creation. 
        /// Creating a convex mesh with invalid input data without prior validation
        /// may result in undefined behavior.
        /// </summary>
        SkipValidation = 1,

        /// <summary>
        /// Enables plane shifting vertex limit algorithm. 
        ///
        /// Plane shifting is an alternative algorithm for the case when the computed hull has more vertices
        /// than the specified vertex limit. 
        /// 
        /// The default algorithm computes the full hull, and an OBB around the input vertices. This OBB is then sliced
        /// with the hull planes until the vertex limit is reached. The default algorithm requires the vertex limit
        /// to be set to at least 8, and typically produces results that are much better quality than are produced
        /// by plane shifting. 
        /// 
        /// When plane shifting is enabled, the hull computation stops when vertex limit is reached.The hull planes
        /// are then shifted to contain all input vertices, and the new plane intersection points are then used to
        /// generate the final hull with the given vertex limit.Plane shifting may produce sharp edges to vertices
        /// very far away from the input cloud, and does not guarantee that all input vertices are inside the resulting
        /// hull. However, it can be used with a vertex limit as low as 4.
        /// </summary>
        UsePlaneShifting = 2,

        /// <summary>
        /// Inertia tensor computation is faster using SIMD code, but the precision is lower, which may result 
        /// in incorrect inertia for very thin hulls.
        /// </summary>
        UseFastInteriaComputation = 4,

        /// <summary>
        /// Convex hull input vertices are shifted to be around origin to provide better computation stability.
        /// It is recommended to provide input vertices around the origin, otherwise use this flag to improve
        /// numerical stability.
        /// </summary>
        ShiftVertices = 8,
    }

    public partial class CollisionData
    {
        /// <summary>
        /// The asset type content domain.
        /// </summary>
        public const ContentDomain Domain = ContentDomain.Other;

        /// <summary>
        /// Gets the set of options used to generate a collision data mesh.
        /// </summary>
        /// <param name="modelLodIndex">Index of the model LOD index used to generate a collision data (value provided during data cooking, may be higher than actual source model LODs collection size).</param>
        /// <param name="convexFlags">The convex mesh generation flags.</param>
        /// <param name="convexVertexLimit">The convex mesh vertex limit.</param>
        public void GetCookOptions(out int modelLodIndex, out ConvexMeshGenerationFlags convexFlags, out int convexVertexLimit)
        {
            Internal_GetCookOptions(unmanagedPtr, out modelLodIndex, out convexFlags, out convexVertexLimit);
        }

#if !UNIT_TEST_COMPILANT
        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_GetCookOptions(IntPtr obj, out int modelLodIndex, out ConvexMeshGenerationFlags convexFlags, out int convexVertexLimit);
#endif
    }
}
