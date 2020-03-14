// Copyright (c) 2012-2020 Wojciech Figat. All rights reserved.

namespace FlaxEngine
{
    /// <summary>
    /// Describes the different tessellation methods supported by the graphics system.
    /// </summary>
    public enum TessellationMethod
    {
        /// <summary>
        /// No tessellation.
        /// </summary>
        [Tooltip("No tessellation.")]
        None = 0,

        /// <summary>
        /// Flat tessellation. Also known as dicing tessellation.
        /// </summary>
        [Tooltip("Flat tessellation. Also known as dicing tessellation.")]
        Flat = 1,

        /// <summary>
        /// Point normal tessellation.
        /// </summary>
        [Tooltip("Point normal tessellation.")]
        PointNormal = 2,

        /// <summary>
        /// Geometric version of Phong normal interpolation, not applied on normals but on the vertex positions.
        /// </summary>
        [Tooltip("Geometric version of Phong normal interpolation, not applied on normals but on the vertex positions.")]
        Phong = 3,
    }
}
