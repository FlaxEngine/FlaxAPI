// Copyright (c) 2012-2020 Wojciech Figat. All rights reserved.

using System;

namespace FlaxEngine
{
    /// <summary>
    /// Which resources are supported for a given format and given device.
    /// </summary>
    [Flags]
    public enum FormatSupport
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
