// Copyright (c) 2012-2019 Wojciech Figat. All rights reserved.

using System;

namespace FlaxEngine
{
    /// <summary>
    /// GPU texture usage flags.
    /// </summary>
    [Flags]
    public enum GPUTextureFlags
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
        /// Create texture views per texture mip map (valid only for Texture2D with ShaderResource or RenderTarget flag).
        /// </summary>
        PerMipViews = 0x0010,

        /// <summary>
        /// Create texture views per texture slice map (valid only for Texture3D with ShaderResource or RenderTarget flag).
        /// </summary>
        PerSliceViews = 0x0020,

        /// <summary>
        /// Create read-only view for depth-stencil buffer. Valid only if texture uses depth-stencil and the graphics device supports it.
        /// </summary>
        ReadOnlyDepthView = 0x0040,
    }
}
