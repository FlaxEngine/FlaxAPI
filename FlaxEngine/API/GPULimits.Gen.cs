// This code was auto-generated. Do not modify it.

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace FlaxEngine
{
    /// <summary>
    /// Which resources are supported for a given format and given device.
    /// </summary>
    [Flags]
    [Tooltip("Which resources are supported for a given format and given device.")]
    public enum FormatSupport : int
    {
        /// <summary>
        /// No features supported.
        /// </summary>
        None = 0,

        /// <summary>
        /// Buffer resources supported.
        /// </summary>
        Buffer = 1,

        /// <summary>
        /// Vertex buffers supported.
        /// </summary>
        InputAssemblyVertexBuffer = 2,

        /// <summary>
        /// Index buffers supported.
        /// </summary>
        InputAssemblyIndexBuffer = 4,

        /// <summary>
        /// Streaming output buffers supported.
        /// </summary>
        StreamOutputBuffer = 8,

        /// <summary>
        /// 1D texture resources supported.
        /// </summary>
        Texture1D = 16,

        /// <summary>
        /// 2D texture resources supported.
        /// </summary>
        Texture2D = 32,

        /// <summary>
        /// 3D texture resources supported.
        /// </summary>
        Texture3D = 64,

        /// <summary>
        /// Cube texture resources supported.
        /// </summary>
        TextureCube = 128,

        /// <summary>
        /// The shader Load function for texture objects is supported.
        /// </summary>
        ShaderLoad = 256,

        /// <summary>
        /// The shader Sample function for texture objects is supported.
        /// </summary>
        ShaderSample = 512,

        /// <summary>
        /// The shader SampleCmp and SampleCmpLevelZero functions for texture objects are supported.
        /// </summary>
        ShaderSampleComparison = 1024,

        /// <summary>
        /// Unused.
        /// </summary>
        ShaderSampleMonoText = 2048,

        /// <summary>
        /// Mipmaps are supported.
        /// </summary>
        Mip = 4096,

        /// <summary>
        /// Automatic generation of mipmaps is supported.
        /// </summary>
        MipAutogen = 8192,

        /// <summary>
        /// Render targets are supported.
        /// </summary>
        RenderTarget = 16384,

        /// <summary>
        /// Blend operations supported.
        /// </summary>
        Blendable = 32768,

        /// <summary>
        /// Depth stencils supported.
        /// </summary>
        DepthStencil = 65536,

        /// <summary>
        /// CPU locking supported.
        /// </summary>
        CpuLockable = 131072,

        /// <summary>
        /// Multisample antialiasing (MSAA) resolve operations are supported.
        /// </summary>
        MultisampleResolve = 262144,

        /// <summary>
        /// Format can be displayed on screen.
        /// </summary>
        Display = 524288,

        /// <summary>
        /// Format can't be cast to another format.
        /// </summary>
        CastWithinBitLayout = 1048576,

        /// <summary>
        /// Format can be used as a multi-sampled render target.
        /// </summary>
        MultisampleRenderTarget = 2097152,

        /// <summary>
        /// Format can be used as a multi-sampled texture and read into a shader with the shader Load function.
        /// </summary>
        MultisampleLoad = 4194304,

        /// <summary>
        /// Format can be used with the shader gather function.
        /// </summary>
        ShaderGather = 8388608,

        /// <summary>
        /// Format supports casting when the resource is a back buffer.
        /// </summary>
        BackBufferCast = 16777216,

        /// <summary>
        /// Format can be used for an unordered access view.
        /// </summary>
        TypedUnorderedAccessView = 33554432,

        /// <summary>
        /// Format can be used with the shader gather with comparison function.
        /// </summary>
        ShaderGatherComparison = 67108864,

        /// <summary>
        /// Format can be used with the decoder output.
        /// </summary>
        DecoderOutput = 134217728,

        /// <summary>
        /// Format can be used with the video processor output.
        /// </summary>
        VideoProcessorOutput = 268435456,

        /// <summary>
        /// Format can be used with the video processor input.
        /// </summary>
        VideoProcessorInput = 536870912,

        /// <summary>
        /// Format can be used with the video encoder.
        /// </summary>
        VideoEncoder = 1073741824,
    }
}

namespace FlaxEngine
{
    /// <summary>
    /// The features exposed for a particular format.
    /// </summary>
    [Tooltip("The features exposed for a particular format.")]
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct FormatFeatures
    {
        /// <summary>
        /// Gets the maximum MSAA sample count for a particular <see cref="PixelFormat"/>.
        /// </summary>
        [Tooltip("The maximum MSAA sample count for a particular <see cref=\"PixelFormat\"/>.")]
        public MSAALevel MSAALevelMax;

        /// <summary>
        /// Support of a given format on the installed video device.
        /// </summary>
        [Tooltip("Support of a given format on the installed video device.")]
        public FormatSupport Support;
    }
}

namespace FlaxEngine
{
    /// <summary>
    /// Graphics Device limits and constraints descriptor.
    /// </summary>
    [Tooltip("Graphics Device limits and constraints descriptor.")]
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct GPULimits
    {
        /// <summary>
        /// True if device supports Compute shaders.
        /// </summary>
        [Tooltip("True if device supports Compute shaders.")]
        public bool IsComputeSupported;

        /// <summary>
        /// True if device supports Tessellation shaders (domain and hull shaders).
        /// </summary>
        [Tooltip("True if device supports Tessellation shaders (domain and hull shaders).")]
        public bool IsTessellationSupported;

        /// <summary>
        /// True if device supports Geometry shaders.
        /// </summary>
        [Tooltip("True if device supports Geometry shaders.")]
        public bool IsGeometryShadersSupported;

        /// <summary>
        /// True if device supports hardware geometry instancing.
        /// </summary>
        [Tooltip("True if device supports hardware geometry instancing.")]
        public bool IsInstancingSupported;

        /// <summary>
        /// True if device supports rendering to volume textures using Geometry shaders.
        /// </summary>
        [Tooltip("True if device supports rendering to volume textures using Geometry shaders.")]
        public bool IsVolumeTextureRenderingSupported;

        /// <summary>
        /// True if device supports indirect drawing (append buffers with counters and pixel shader write to UAV).
        /// </summary>
        [Tooltip("True if device supports indirect drawing (append buffers with counters and pixel shader write to UAV).")]
        public bool IsDrawIndirectSupported;

        /// <summary>
        /// True if device supports separate render target blending states.
        /// </summary>
        [Tooltip("True if device supports separate render target blending states.")]
        public bool IsSupportingSeparateRTBlendState;

        /// <summary>
        /// True if device supports depth buffer texture as a shader resource view.
        /// </summary>
        [Tooltip("True if device supports depth buffer texture as a shader resource view.")]
        public bool HasDepthAsSRV;

        /// <summary>
        /// True if device supports depth buffer texture as a readonly depth buffer (can be sampled in the shader while performing depth-test).
        /// </summary>
        [Tooltip("True if device supports depth buffer texture as a readonly depth buffer (can be sampled in the shader while performing depth-test).")]
        public bool HasReadOnlyDepth;

        /// <summary>
        /// True if device supports multisampled depth buffer texture as a shader resource view.
        /// </summary>
        [Tooltip("True if device supports multisampled depth buffer texture as a shader resource view.")]
        public bool HasMultisampleDepthAsSRV;

        /// <summary>
        /// The maximum amount of texture mip levels.
        /// </summary>
        [Tooltip("The maximum amount of texture mip levels.")]
        public int MaximumMipLevelsCount;

        /// <summary>
        /// The maximum size of the 1D texture.
        /// </summary>
        [Tooltip("The maximum size of the 1D texture.")]
        public int MaximumTexture1DSize;

        /// <summary>
        /// The maximum length of 1D textures array.
        /// </summary>
        [Tooltip("The maximum length of 1D textures array.")]
        public int MaximumTexture1DArraySize;

        /// <summary>
        /// The maximum size of the 2D texture.
        /// </summary>
        [Tooltip("The maximum size of the 2D texture.")]
        public int MaximumTexture2DSize;

        /// <summary>
        /// The maximum length of 2D textures array.
        /// </summary>
        [Tooltip("The maximum length of 2D textures array.")]
        public int MaximumTexture2DArraySize;

        /// <summary>
        /// The maximum size of the 3D texture.
        /// </summary>
        [Tooltip("The maximum size of the 3D texture.")]
        public int MaximumTexture3DSize;

        /// <summary>
        /// The maximum size of the cube texture (both width and height).
        /// </summary>
        [Tooltip("The maximum size of the cube texture (both width and height).")]
        public int MaximumTextureCubeSize;
    }
}
