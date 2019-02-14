// Copyright (c) 2012-2019 Wojciech Figat. All rights reserved.

// ReSharper disable InconsistentNaming

namespace FlaxEngine.Rendering
{
    /// <summary>
    /// Graphics rendering backend system types.
    /// </summary>
    public enum RendererType
    {
        /// <summary>
        /// Unknown type
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// DirectX 10
        /// </summary>
        DirectX10 = 1,

        /// <summary>
        /// DirectX 10.1
        /// </summary>
        DirectX10_1 = 2,

        /// <summary>
        /// DirectX 11
        /// </summary>
        DirectX11 = 3,

        /// <summary>
        /// DirectX 12
        /// </summary>
        DirectX12 = 4,

        /// <summary>
        /// OpenGL 4.1
        /// </summary>
        OpenGL4_1 = 5,

        /// <summary>
        /// OpenGL 4.4
        /// </summary>
        OpenGL4_4 = 6,

        /// <summary>
        /// OpenGL ES 3
        /// </summary>
        OpenGLES3 = 7,

        /// <summary>
        /// OpenGL ES 3.1
        /// </summary>
        OpenGLES3_1 = 8,

        /// <summary>
        /// Null backend
        /// </summary>
        Null = 9,

        /// <summary>
        /// Vulkan
        /// </summary>
        Vulkan = 10,
    }

    /// <summary>
    /// Shader profile types define the version and type of the shading language used by the graphics backend.
    /// </summary>
    public enum ShaderProfile
    {
        /// <summary>
        /// Unknown
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// Shader Model 4.0
        /// </summary>
        ShaderModel4 = 1,

        /// <summary>
        /// Shader Model 5.0
        /// </summary>
        ShaderModel5 = 2,

        /// <summary>
        /// GLSL 410
        /// </summary>
        GLSL_410 = 3,

        /// <summary>
        /// GLSL 440
        /// </summary>
        GLSL_440 = 4,

        /// <summary>
        /// Vulkan (Shader Model 5 compatible)
        /// </summary>
        Vulkan_SM5 = 5,
    }
}
