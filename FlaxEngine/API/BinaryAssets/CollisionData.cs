////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

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

    public partial class CollisionData
	{
	    /// <summary>
	    /// The asset type content domain.
	    /// </summary>
	    public const ContentDomain Domain = ContentDomain.Other;
    }
}
