// Copyright (c) 2012-2020 Wojciech Figat. All rights reserved.

namespace FlaxEngine
{
    /// <summary>
    /// Defines the dimension of a texture object.
    /// </summary>
    public enum TextureDimensions
    {
        /// <summary>
        /// The texture (2d).
        /// </summary>
        [Tooltip("The texture (2d).")]
        Texture,

        /// <summary>
        /// The volume texture (3d texture).
        /// </summary>
        [Tooltip("The volume texture (3d texture).")]
        VolumeTexture,

        /// <summary>
        /// The cube texture (2d texture array of 6 items).
        /// </summary>
        [Tooltip("The cube texture (2d texture array of 6 items).")]
        CubeTexture,
    }
}
