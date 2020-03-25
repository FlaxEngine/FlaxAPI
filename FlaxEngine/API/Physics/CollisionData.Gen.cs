// This code was auto-generated. Do not modify it.

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace FlaxEngine
{
    /// <summary>
    /// A <see cref="CollisionData"/> storage data type.
    /// </summary>
    [Tooltip("A <see cref=\"CollisionData\"/> storage data type.")]
    public enum CollisionDataType
    {
        /// <summary>
        /// Nothing.
        /// </summary>
        [Tooltip("Nothing.")]
        None = 0,

        /// <summary>
        /// A convex polyhedron represented as a set of vertices and polygonal faces. The number of vertices and faces of a convex mesh is limited to 255.
        /// </summary>
        [Tooltip("A convex polyhedron represented as a set of vertices and polygonal faces. The number of vertices and faces of a convex mesh is limited to 255.")]
        ConvexMesh = 1,

        /// <summary>
        /// A collision triangle mesh consists of a collection of vertices and the triangle indices.
        /// </summary>
        [Tooltip("A collision triangle mesh consists of a collection of vertices and the triangle indices.")]
        TriangleMesh = 2,
    }
}

namespace FlaxEngine
{
    /// <summary>
    /// Set of flags used to generate model convex mesh. Allows to customize process.
    /// </summary>
    [Flags]
    [Tooltip("Set of flags used to generate model convex mesh. Allows to customize process.")]
    public enum ConvexMeshGenerationFlags
    {
        /// <summary>
        /// Nothing.
        /// </summary>
        [Tooltip("Nothing.")]
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
}

namespace FlaxEngine
{
    /// <summary>
    /// The collision data asset cooking options.
    /// </summary>
    [Tooltip("The collision data asset cooking options.")]
    [StructLayout(LayoutKind.Sequential)]
    public unsafe partial struct CollisionDataOptions
    {
        /// <summary>
        /// The data type.
        /// </summary>
        [Tooltip("The data type.")]
        public CollisionDataType Type;

        /// <summary>
        /// The source model asset id.
        /// </summary>
        [Tooltip("The source model asset id.")]
        public Guid Model;

        /// <summary>
        /// The source model LOD index.
        /// </summary>
        [Tooltip("The source model LOD index.")]
        public int ModelLodIndex;

        /// <summary>
        /// The cooked collision bounds.
        /// </summary>
        [Tooltip("The cooked collision bounds.")]
        public BoundingBox Box;

        /// <summary>
        /// The convex generation flags.
        /// </summary>
        [Tooltip("The convex generation flags.")]
        public ConvexMeshGenerationFlags ConvexFlags;

        /// <summary>
        /// The convex vertices limit (maximum amount).
        /// </summary>
        [Tooltip("The convex vertices limit (maximum amount).")]
        public int ConvexVertexLimit;
    }
}

namespace FlaxEngine
{
    /// <summary>
    /// Represents a physics mesh that can be used with a MeshCollider. Physics mesh can be a generic triangle mesh or a convex mesh.
    /// </summary>
    [Tooltip("Represents a physics mesh that can be used with a MeshCollider. Physics mesh can be a generic triangle mesh or a convex mesh.")]
    public unsafe partial class CollisionData : BinaryAsset
    {
        /// <inheritdoc />
        protected CollisionData() : base()
        {
        }

        /// <summary>
        /// Gets the options.
        /// </summary>
        [Tooltip("The options.")]
        public CollisionDataOptions Options
        {
            get { Internal_GetOptions(unmanagedPtr, out var resultAsRef); return resultAsRef; }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_GetOptions(IntPtr obj, out CollisionDataOptions resultAsRef);

        /// <summary>
        /// Cooks the mesh collision data and updates the virtual asset. action cannot be performed on a main thread.
        /// </summary>
        /// <remarks>
        /// Can be used only for virtual assets (see <see cref="Asset.IsVirtual"/> and <see cref="Content.CreateVirtualAsset{T}"/>).
        /// </remarks>
        /// <param name="type">The collision data type.</param>
        /// <param name="model">The source model.</param>
        /// <param name="modelLodIndex">The source model LOD index.</param>
        /// <param name="convexFlags">The convex mesh generation flags.</param>
        /// <param name="convexVertexLimit">The convex mesh vertex limit. Use values in range [8;255]</param>
        public bool CookCollision(CollisionDataType type, Model model, int modelLodIndex = 0, ConvexMeshGenerationFlags convexFlags = ConvexMeshGenerationFlags.None, int convexVertexLimit = 255)
        {
            return Internal_CookCollision(unmanagedPtr, type, FlaxEngine.Object.GetUnmanagedPtr(model), modelLodIndex, convexFlags, convexVertexLimit);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_CookCollision(IntPtr obj, CollisionDataType type, IntPtr model, int modelLodIndex, ConvexMeshGenerationFlags convexFlags, int convexVertexLimit);

        /// <summary>
        /// Cooks the mesh collision data and updates the virtual asset. action cannot be performed on a main thread.
        /// </summary>
        /// <remarks>
        /// Can be used only for virtual assets (see <see cref="Asset.IsVirtual"/> and <see cref="Content.CreateVirtualAsset{T}"/>).
        /// </remarks>
        /// <param name="type">The collision data type.</param>
        /// <param name="vertices">The source geometry vertex buffer with vertices positions. Cannot be empty.</param>
        /// <param name="triangles">The source data index buffer (triangles list). Uses 32-bit stride buffer. Cannot be empty. Length must be multiple of 3 (as 3 vertices build a triangle).</param>
        /// <param name="convexFlags">The convex mesh generation flags.</param>
        /// <param name="convexVertexLimit">The convex mesh vertex limit. Use values in range [8;255]</param>
        public bool CookCollision(CollisionDataType type, Vector3[] vertices, uint[] triangles, ConvexMeshGenerationFlags convexFlags = ConvexMeshGenerationFlags.None, int convexVertexLimit = 255)
        {
            return Internal_CookCollision1(unmanagedPtr, type, vertices, triangles, convexFlags, convexVertexLimit);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_CookCollision1(IntPtr obj, CollisionDataType type, Vector3[] vertices, uint[] triangles, ConvexMeshGenerationFlags convexFlags, int convexVertexLimit);

        /// <summary>
        /// Extracts the collision data geometry into list of triangles.
        /// </summary>
        /// <param name="vertexBuffer">The output vertex buffer.</param>
        /// <param name="indexBuffer">The output index buffer.</param>
        public void ExtractGeometry(out Vector3[] vertexBuffer, out int[] indexBuffer)
        {
            Internal_ExtractGeometry(unmanagedPtr, out vertexBuffer, out indexBuffer, typeof(Vector3));
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_ExtractGeometry(IntPtr obj, out Vector3[] vertexBuffer, out int[] indexBuffer, System.Type resultArrayItemType0);
    }
}
