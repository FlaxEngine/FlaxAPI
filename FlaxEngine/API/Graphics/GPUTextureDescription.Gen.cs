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

namespace FlaxEngine
{
    /// <summary>
    /// Defines the dimension of a texture object.
    /// </summary>
    [Tooltip("Defines the dimension of a texture object.")]
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

namespace FlaxEngine
{
    /// <summary>
    /// A common description for all GPU textures.
    /// </summary>
    [Tooltip("A common description for all GPU textures.")]
    [StructLayout(LayoutKind.Sequential)]
    public unsafe partial struct GPUTextureDescription
    {
        /// <summary>
        /// The dimensions of the texture.
        /// </summary>
        [Tooltip("The dimensions of the texture.")]
        public TextureDimensions Dimensions;

        /// <summary>
        /// Texture width (in texels).
        /// </summary>
        [Tooltip("Texture width (in texels).")]
        public int Width;

        /// <summary>
        /// Texture height (in texels).
        /// </summary>
        [Tooltip("Texture height (in texels).")]
        public int Height;

        /// <summary>
        /// Texture depth (in texels) for Volume Textures.
        /// </summary>
        [Tooltip("Texture depth (in texels) for Volume Textures.")]
        public int Depth;

        /// <summary>
        /// Number of textures in array for Texture Arrays.
        /// </summary>
        [Tooltip("Number of textures in array for Texture Arrays.")]
        public int ArraySize;

        /// <summary>
        /// The maximum number of mipmap levels in the texture. Use 1 for a multisampled texture; or 0 to generate a full set of subtextures.
        /// </summary>
        [Tooltip("The maximum number of mipmap levels in the texture. Use 1 for a multisampled texture; or 0 to generate a full set of subtextures.")]
        public int MipLevels;

        /// <summary>
        /// Texture format (see <strong><see cref="PixelFormat"/></strong>).
        /// </summary>
        [Tooltip("Texture format (see <strong><see cref=\"PixelFormat\"/></strong>).")]
        public PixelFormat Format;

        /// <summary>
        /// Structure that specifies multisampling parameters for the texture.
        /// </summary>
        [Tooltip("Structure that specifies multisampling parameters for the texture.")]
        public MSAALevel MultiSampleLevel;

        /// <summary>
        /// Flags (see <strong><see cref="GPUTextureFlags"/></strong>) for binding to pipeline stages. The flags can be combined by a logical OR.
        /// </summary>
        [Tooltip("Flags (see <strong><see cref=\"GPUTextureFlags\"/></strong>) for binding to pipeline stages. The flags can be combined by a logical OR.")]
        public GPUTextureFlags Flags;

        /// <summary>
        /// Value that identifies how the texture is to be read from and written to. The most common value is <see cref="GPUResourceUsage.Default"/>; see <strong><see cref="GPUResourceUsage"/></strong> for all possible values.
        /// </summary>
        [Tooltip("Value that identifies how the texture is to be read from and written to. The most common value is <see cref=\"GPUResourceUsage.Default\"/>; see <strong><see cref=\"GPUResourceUsage\"/></strong> for all possible values.")]
        public GPUResourceUsage Usage;

        /// <summary>
        /// Default clear color for render targets
        /// </summary>
        [Tooltip("Default clear color for render targets")]
        public Color DefaultClearColor;
    }
}
