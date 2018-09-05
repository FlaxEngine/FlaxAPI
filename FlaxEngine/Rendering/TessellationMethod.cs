// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

namespace FlaxEngine.Rendering
{
    /// <summary>
    /// Describes the different tessellation methods supported by the graphics system.
    /// </summary>
    public enum TessellationMethod
    {
        /// <summary>
        /// No tessellation.
        /// </summary>
        None = 0,

        /// <summary>
        /// Flat tessellation. Also known as dicing tessellation.
        /// </summary>
        Flat = 1,

        /// <summary>
        /// Point normal tessellation.
        /// </summary>
        PointNormal = 2,

        /// <summary>
        /// Geometric version of Phong normal interpolation, not applied on normals but on the vertex positions.
        /// </summary>
        Phong = 3,
    }
}
