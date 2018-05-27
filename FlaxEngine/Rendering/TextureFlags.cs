// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System;

namespace FlaxEngine.Rendering
{
    /// <summary>
    /// GPU texture usage flags.
    /// </summary>
    [Flags]
    public enum TextureFlags
    {
        /// <summary>
        /// No texture flags.
        /// </summary>
        None = 0x0000,

        /// <summary>
        /// Create a texture that can be bound as a shader resource.
        /// </summary>
        ShaderResource = 0x0001,

        /// <summary>
        /// Create a texture that can be bound as a render target.
        /// </summary>
        RenderTarget = 0x0002,

        /// <summary>
        /// Create a texture can be bound as an unordered access buffer.
        /// </summary>
        UnorderedAccess = 0x0004,

        /// <summary>
        /// Create a texture can be bound as a depth stencil buffer.
        /// </summary>
        DepthStencil = 0x0008,

        /// <summary>
        /// Create render target handles per texture mip map (valid only for Texture2D with ShaderResource or RenderTarget flag).
        /// </summary>
        PerMipHandles = 0x0010,

        /// <summary>
        /// Create render target handles per texture array/volume slice (valid only for Texture2D and Texture3D with ShaderResource or RenderTarget flag).
        /// </summary>
        PerSliceHandles = 0x0020,
    }
}
