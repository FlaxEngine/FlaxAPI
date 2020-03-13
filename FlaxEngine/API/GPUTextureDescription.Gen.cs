// This code was auto-generated. Do not modify it.

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace FlaxEngine
{
    /// <summary>
    /// GPU texture usage flags.
    /// </summary>
    [Flags]
    [Tooltip("GPU texture usage flags.")]
    public enum GPUTextureFlags
    {
        /// <summary>
        /// No texture flags.
        /// </summary>
        [Tooltip("No texture flags.")]
        None = 0x0000,

        /// <summary>
        /// Create a texture that can be bound as a shader resource.
        /// </summary>
        [Tooltip("Create a texture that can be bound as a shader resource.")]
        ShaderResource = 0x0001,

        /// <summary>
        /// Create a texture that can be bound as a render target.
        /// </summary>
        [Tooltip("Create a texture that can be bound as a render target.")]
        RenderTarget = 0x0002,

        /// <summary>
        /// Create a texture can be bound as an unordered access buffer.
        /// </summary>
        [Tooltip("Create a texture can be bound as an unordered access buffer.")]
        UnorderedAccess = 0x0004,

        /// <summary>
        /// Create a texture can be bound as a depth stencil buffer.
        /// </summary>
        [Tooltip("Create a texture can be bound as a depth stencil buffer.")]
        DepthStencil = 0x0008,

        /// <summary>
        /// Create texture views per texture mip map (valid only for Texture2D with ShaderResource or RenderTarget flag).
        /// </summary>
        [Tooltip("Create texture views per texture mip map (valid only for Texture2D with ShaderResource or RenderTarget flag).")]
        PerMipViews = 0x0010,

        /// <summary>
        /// Create texture views per texture slice map (valid only for Texture3D with ShaderResource or RenderTarget flag).
        /// </summary>
        [Tooltip("Create texture views per texture slice map (valid only for Texture3D with ShaderResource or RenderTarget flag).")]
        PerSliceViews = 0x0020,

        /// <summary>
        /// Create read-only view for depth-stencil buffer. Valid only if texture uses depth-stencil and the graphics device supports it.
        /// </summary>
        [Tooltip("Create read-only view for depth-stencil buffer. Valid only if texture uses depth-stencil and the graphics device supports it.")]
        ReadOnlyDepthView = 0x0040,

        /// <summary>
        /// Create a texture that can be used as a native window swap chain backbuffer surface.
        /// </summary>
        [Tooltip("Create a texture that can be used as a native window swap chain backbuffer surface.")]
        BackBuffer = 0x0080,
    }
}
