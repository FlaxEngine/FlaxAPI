// Copyright (c) 2012-2019 Wojciech Figat. All rights reserved.

using System;
using System.Runtime.InteropServices;

namespace FlaxEngine
{
    /// <summary>
    /// A common description for all GPU textures.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct GPUTextureDescription : IEquatable<GPUTextureDescription>
    {
        /// <summary>
        /// The dimensions of the texture.
        /// </summary>
        public TextureDimensions Dimensions;

        /// <summary>
        /// Texture width (in texels).
        /// </summary>
        public int Width;

        /// <summary>
        /// Texture height (in texels).
        /// </summary>
        public int Height;

        /// <summary>
        /// Texture depth (in texels) for Volume Textures.
        /// </summary>
        public int Depth;

        /// <summary>
        /// Number of textures in array for Texture Arrays.
        /// </summary>
        public int ArraySize;

        /// <summary>	
        /// The maximum number of mipmap levels in the texture. Use 1 for a multisampled texture; or 0 to generate a full set of subtextures.
        /// </summary>	
        public int MipLevels;

        /// <summary>	
        /// Texture format (see <strong><see cref="PixelFormat"/></strong>).
        /// </summary>	
        public PixelFormat Format;

        /// <summary>	
        /// Structure that specifies multisampling parameters for the texture.
        /// </summary>	
        public MSAALevel MultiSampleLevel;

        /// <summary>	
        /// Flags (see <strong><see cref="GPUTextureFlags"/></strong>) for binding to pipeline stages. The flags can be combined by a logical OR.
        /// </summary>
        public GPUTextureFlags Flags;

        /// <summary>	
        /// Value that identifies how the texture is to be read from and written to. The most common value is <see cref="GPUResourceUsage.Default"/>; see <strong><see cref="GPUResourceUsage"/></strong> for all possible values.
        /// </summary>
        public GPUResourceUsage Usage;

        /// <summary>
        /// Default clear color for render targets
        /// </summary>
        public Color DefaultClearColor;

        /// <summary>
        /// Gets a value indicating whether this instance is a render target.
        /// </summary>
        public bool IsRenderTarget => (Flags & GPUTextureFlags.RenderTarget) != 0;

        /// <summary>
        /// Gets a value indicating whether this instance is a depth stencil.
        /// </summary>
        public bool IsDepthStencil => (Flags & GPUTextureFlags.DepthStencil) != 0;

        /// <summary>
        /// Gets a value indicating whether this instance is a shader resource.
        /// </summary>
        public bool IsShaderResource => (Flags & GPUTextureFlags.ShaderResource) != 0;

        /// <summary>
        /// Gets a value indicating whether this instance is a unordered access.
        /// </summary>
        public bool IsUnorderedAccess => (Flags & GPUTextureFlags.UnorderedAccess) != 0;

        /// <summary>
        /// Gets a value indicating whether this instance has per mip level handles.
        /// </summary>
        public bool HasPerMipViews => (Flags & GPUTextureFlags.PerMipViews) != 0;

        /// <summary>
        /// Gets a value indicating whether this instance has per slice views.
        /// </summary>
        public bool HasPerSliceViews => (Flags & GPUTextureFlags.PerSliceViews) != 0;

        /// <summary>
        /// Gets a value indicating whether this instance is a multi sample texture.
        /// </summary>
        public bool IsMultiSample => MultiSampleLevel > MSAALevel.None;

        /// <summary>
        /// Gets a value indicating whether this instance is a cubemap texture.
        /// </summary>
        public bool IsCubeMap => Dimensions == TextureDimensions.CubeTexture;

        /// <summary>
        /// Gets a value indicating whether this instance is a volume texture.
        /// </summary>
        public bool IsVolume => Dimensions == TextureDimensions.VolumeTexture;

        /// <summary>
        /// Gets a value indicating whether this instance is an array texture.
        /// </summary>
        public bool IsArray => ArraySize != 1;

        private static int CalculateMipMapCount(int requestedLevel, int width)
        {
            if (requestedLevel == 0)
                requestedLevel = 14; // GPU_MAX_TEXTURE_MIP_LEVELS

            int maxMipMap = 1;
            while (width > 1)
            {
                if (width > 1)
                    width >>= 1;
                maxMipMap++;
            }

            return Mathf.Min(requestedLevel, maxMipMap);
        }

        /// <summary>
        /// Clears description.
        /// </summary>
        public void Clear()
        {
            this = new GPUTextureDescription();
            MultiSampleLevel = MSAALevel.None;
        }

        /// <summary>
        /// Creates a new 1D <see cref="GPUTextureDescription" /> with a single level of mipmap.
        /// </summary>
        /// <param name="width">The width.</param>
        /// <param name="format">Describes the format to use.</param>
        /// <param name="textureFlags">true if the texture needs to support unordered read write.</param>
        /// <returns>A new instance of 1D <see cref="GPUTextureDescription" /> class.</returns>
        /// <remarks>The first dimension of mipMapTextures describes the number of array (Texture1D Array), second dimension is the mipmap, the third is the texture data for a particular mipmap.</remarks>
        public static GPUTextureDescription New1D(int width, PixelFormat format, GPUTextureFlags textureFlags = GPUTextureFlags.ShaderResource)
        {
            return New1D(width, format, textureFlags, 1, 1);
        }

        /// <summary>
        /// Creates a new 1D <see cref="GPUTextureDescription" /> with a single mipmap.
        /// </summary>
        /// <param name="width">The width.</param>
        /// <param name="format">Describes the format to use.</param>
        /// <param name="textureFlags">true if the texture needs to support unordered read write.</param>
        /// <param name="arraySize">Size of the texture 2D array, default to 1.</param>
        /// <returns>A new instance of 1D <see cref="GPUTextureDescription" /> class.</returns>
        public static GPUTextureDescription New1D(int width, PixelFormat format, GPUTextureFlags textureFlags = GPUTextureFlags.ShaderResource, int arraySize = 1)
        {
            return New1D(width, format, textureFlags, 1, arraySize);
        }

        /// <summary>
        /// Creates a new 1D <see cref="GPUTextureDescription" />.
        /// </summary>
        /// <param name="width">The width.</param>
        /// <param name="mipCount">Number of mipmaps for the texture. Default is 1. Use 0 to allocate full mip chain.</param>
        /// <param name="format">Describes the format to use.</param>
        /// <param name="textureFlags">true if the texture needs to support unordered read write.</param>
        /// <param name="arraySize">Size of the texture 2D array, default to 1.</param>
        /// <returns>A new instance of 1D <see cref="GPUTextureDescription" /> class.</returns>
        public static GPUTextureDescription New1D(int width, int mipCount, PixelFormat format, GPUTextureFlags textureFlags = GPUTextureFlags.ShaderResource, int arraySize = 1)
        {
            return New1D(width, format, textureFlags, mipCount, arraySize);
        }

        /// <summary>
        /// Creates a new 1D <see cref="GPUTextureDescription" />.
        /// </summary>
        /// <param name="width">The width.</param>
        /// <param name="format">Describes the format to use.</param>
        /// <param name="textureFlags">true if the texture needs to support unordered read write.</param>
        /// <param name="mipCount">Number of mipmaps for the texture. Default is 1. Use 0 to allocate full mip chain.</param>
        /// <param name="arraySize">Size of the texture 2D array, default to 1.</param>
        /// <returns>A new instance of 1D <see cref="GPUTextureDescription" /> class.</returns>
        public static GPUTextureDescription New1D(int width, PixelFormat format, GPUTextureFlags textureFlags, int mipCount, int arraySize)
        {
            GPUTextureDescription desc;
            desc.Dimensions = TextureDimensions.Texture;
            desc.Width = width;
            desc.Height = 1;
            desc.Depth = 1;
            desc.ArraySize = arraySize;
            desc.MipLevels = CalculateMipMapCount(mipCount, width);
            desc.Format = format;
            desc.MultiSampleLevel = MSAALevel.None;
            desc.Flags = textureFlags;
            desc.Usage = GPUResourceUsage.Default;
            desc.DefaultClearColor = Color.Black;
            return desc;
        }

        /// <summary>
        /// Creates a new <see cref="GPUTextureDescription" /> with a single mipmap.
        /// </summary>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="format">Describes the format to use.</param>
        /// <param name="textureFlags">true if the texture needs to support unordered read write.</param>
        /// <param name="arraySize">Size of the texture 2D array, default to 1.</param>
        /// <returns>A new instance of <see cref="GPUTextureDescription" /> class.</returns>
        public static GPUTextureDescription New2D(int width, int height, PixelFormat format, GPUTextureFlags textureFlags = GPUTextureFlags.ShaderResource, int arraySize = 1)
        {
            return New2D(width, height, 1, format, textureFlags, arraySize);
        }

        /// <summary>
        /// Creates a new <see cref="GPUTextureDescription" />.
        /// </summary>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="mipCount">Number of mipmaps for the texture. Default is 1. Use 0 to allocate full mip chain.</param>
        /// <param name="format">Describes the format to use.</param>
        /// <param name="textureFlags">true if the texture needs to support unordered read write.</param>
        /// <param name="arraySize">Size of the texture 2D array, default to 1.</param>
        /// <param name="msaaLevel">The MSAA Level.</param>
        /// <returns>A new instance of <see cref="GPUTextureDescription" /> class.</returns>
        public static GPUTextureDescription New2D(int width, int height, int mipCount, PixelFormat format, GPUTextureFlags textureFlags = GPUTextureFlags.ShaderResource, int arraySize = 1, MSAALevel msaaLevel = MSAALevel.None)
        {
            return New2D(width, height, format, textureFlags, mipCount, arraySize, msaaLevel);
        }

        /// <summary>
        /// Creates a new <see cref="GPUTextureDescription" />.
        /// </summary>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="format">Describes the format to use.</param>
        /// <param name="textureFlags">true if the texture needs to support unordered read write.</param>
        /// <param name="mipCount">Number of mipmaps for the texture. Default is 1. Use 0 to allocate full mip chain.</param>
        /// <param name="arraySize">Size of the texture 2D array, default to 1.</param>
        /// <param name="msaaLevel">The MSAA Level.</param>
        /// <returns>A new instance of <see cref="GPUTextureDescription" /> class.</returns>
        public static GPUTextureDescription New2D(int width, int height, PixelFormat format, GPUTextureFlags textureFlags, int mipCount, int arraySize, MSAALevel msaaLevel = MSAALevel.None)
        {
            GPUTextureDescription desc;
            desc.Dimensions = TextureDimensions.Texture;
            desc.Width = width;
            desc.Height = height;
            desc.Depth = 1;
            desc.ArraySize = arraySize;
            desc.MipLevels = CalculateMipMapCount(mipCount, Mathf.Max(width, height));
            desc.Format = format;
            desc.MultiSampleLevel = msaaLevel;
            desc.Flags = textureFlags;
            desc.DefaultClearColor = Color.Black;
            desc.Usage = GPUResourceUsage.Default;
            return desc;
        }

        /// <summary>
        /// Creates a new <see cref="GPUTextureDescription" /> with a single mipmap.
        /// </summary>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="depth">The depth.</param>
        /// <param name="format">Describes the format to use.</param>
        /// <param name="textureFlags">true if the texture needs to support unordered read write.</param>
        /// <returns>A new instance of <see cref="GPUTextureDescription" /> class.</returns>
        public static GPUTextureDescription New3D(int width, int height, int depth, PixelFormat format, GPUTextureFlags textureFlags = GPUTextureFlags.ShaderResource)
        {
            return New3D(width, height, depth, 1, format, textureFlags);
        }

        /// <summary>
        /// Creates a new <see cref="GPUTextureDescription" />.
        /// </summary>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="depth">The depth.</param>
        /// <param name="mipCount">Number of mipmaps for the texture. Default is 1. Use 0 to allocate full mip chain.</param>
        /// <param name="format">Describes the format to use.</param>
        /// <param name="textureFlags">true if the texture needs to support unordered read write.</param>
        /// <returns>A new instance of <see cref="GPUTextureDescription" /> class.</returns>
        public static GPUTextureDescription New3D(int width, int height, int depth, int mipCount, PixelFormat format, GPUTextureFlags textureFlags = GPUTextureFlags.ShaderResource)
        {
            return New3D(width, height, depth, format, textureFlags, mipCount);
        }

        /// <summary>
        /// Creates a new <see cref="GPUTextureDescription" />.
        /// </summary>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="depth">The depth.</param>
        /// <param name="format">Describes the format to use.</param>
        /// <param name="textureFlags">true if the texture needs to support unordered read write.</param>
        /// <param name="mipCount">Number of mipmaps for the texture. Default is 1. Use 0 to allocate full mip chain.</param>
        /// <returns>A new instance of <see cref="GPUTextureDescription" /> class.</returns>
        public static GPUTextureDescription New3D(int width, int height, int depth, PixelFormat format, GPUTextureFlags textureFlags, int mipCount)
        {
            GPUTextureDescription desc;
            desc.Dimensions = TextureDimensions.VolumeTexture;
            desc.Width = width;
            desc.Height = height;
            desc.Depth = depth;
            desc.ArraySize = 1;
            desc.MipLevels = CalculateMipMapCount(mipCount, Mathf.Max(width, height, depth));
            desc.Format = format;
            desc.MultiSampleLevel = MSAALevel.None;
            desc.Flags = textureFlags;
            desc.DefaultClearColor = Color.Black;
            desc.Usage = GPUResourceUsage.Default;
            return desc;
        }

        /// <summary>
        /// Creates a new Cube <see cref="GPUTextureDescription" />.
        /// </summary>
        /// <param name="size">The size (in pixels) of the top-level faces of the cube texture.</param>
        /// <param name="format">Describes the format to use.</param>
        /// <param name="textureFlags">The texture flags.</param>
        /// <returns>A new instance of <see cref="GPUTextureDescription" /> class.</returns>
        public static GPUTextureDescription NewCube(int size, PixelFormat format, GPUTextureFlags textureFlags = GPUTextureFlags.ShaderResource)
        {
            return NewCube(size, 1, format, textureFlags);
        }

        /// <summary>
        /// Creates a new Cube <see cref="GPUTextureDescription"/>.
        /// </summary>
        /// <param name="size">The size (in pixels) of the top-level faces of the cube texture.</param>
        /// <param name="mipCount">Number of mipmaps for the texture. Default is 1. Use 0 to allocate full mip chain.</param>
        /// <param name="format">Describes the format to use.</param>
        /// <param name="textureFlags">The texture flags.</param>
        /// <returns>A new instance of <see cref="GPUTextureDescription"/> class.</returns>
        public static GPUTextureDescription NewCube(int size, int mipCount, PixelFormat format, GPUTextureFlags textureFlags = GPUTextureFlags.ShaderResource)
        {
            return NewCube(size, format, textureFlags, mipCount);
        }

        /// <summary>
        /// Creates a new Cube <see cref="GPUTextureDescription"/>.
        /// </summary>
        /// <param name="size">The size (in pixels) of the top-level faces of the cube texture.</param>
        /// <param name="format">Describes the format to use.</param>
        /// <param name="textureFlags">The texture flags.</param>
        /// <param name="mipCount">Number of mipmaps for the texture. Default is 1. Use 0 to allocate full mip chain.</param>
        /// <returns>A new instance of <see cref="GPUTextureDescription"/> class.</returns>
        public static GPUTextureDescription NewCube(int size, PixelFormat format, GPUTextureFlags textureFlags, int mipCount)
        {
            var desc = New2D(size, size, format, textureFlags, mipCount, 6);
            desc.Dimensions = TextureDimensions.CubeTexture;
            return desc;
        }

        /// <summary>
        /// Gets the staging description for this instance.
        /// </summary>
        /// <returns>A staging texture description</returns>
        public GPUTextureDescription ToStagingUpload()
        {
            var copy = this;
            copy.Flags = GPUTextureFlags.None;
            copy.Usage = GPUResourceUsage.StagingUpload;
            return copy;
        }

        /// <summary>
        /// Gets the staging description for this instance.
        /// </summary>
        /// <returns>A staging texture description</returns>
        public GPUTextureDescription ToStagingReadback()
        {
            var copy = this;
            copy.Flags = GPUTextureFlags.None;
            copy.Usage = GPUResourceUsage.StagingReadback;
            return copy;
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return string.Format("Size: {0}x{1}x{2}[{3}], Type: {4}, Mips: {5}, Format: {6}, MSAA: {7}, Flags: {8}, Usage: {9}",
                                 Width,
                                 Height,
                                 Depth,
                                 ArraySize,
                                 Dimensions,
                                 MipLevels,
                                 Format,
                                 MultiSampleLevel,
                                 Flags,
                                 Usage);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (int)Dimensions;
                hashCode = (hashCode * 397) ^ Width;
                hashCode = (hashCode * 397) ^ Height;
                hashCode = (hashCode * 397) ^ Depth;
                hashCode = (hashCode * 397) ^ ArraySize;
                hashCode = (hashCode * 397) ^ MipLevels;
                hashCode = (hashCode * 397) ^ (int)Format;
                hashCode = (hashCode * 397) ^ (int)MultiSampleLevel;
                hashCode = (hashCode * 397) ^ (int)Flags;
                hashCode = (hashCode * 397) ^ (int)Usage;
                hashCode = (hashCode * 397) ^ DefaultClearColor.GetHashCode();
                return hashCode;
            }
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            return obj is GPUTextureDescription other && Equals(other);
        }

        /// <inheritdoc />
        public bool Equals(GPUTextureDescription other)
        {
            return Dimensions == other.Dimensions &&
                   Width == other.Width &&
                   Height == other.Height &&
                   Depth == other.Depth &&
                   ArraySize == other.ArraySize &&
                   MipLevels == other.MipLevels &&
                   Format == other.Format &&
                   MultiSampleLevel == other.MultiSampleLevel &&
                   Flags == other.Flags &&
                   Usage == other.Usage &&
                   DefaultClearColor.Equals(ref other.DefaultClearColor);
        }
    };
}
