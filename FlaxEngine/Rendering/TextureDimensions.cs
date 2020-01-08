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
        Texture,

        /// <summary>
        /// The volume texture (3d texture).
        /// </summary>
        VolumeTexture,

        /// <summary>
        /// The cube texture (2d texture array of 6 items).
        /// </summary>
        CubeTexture,
    }
}
