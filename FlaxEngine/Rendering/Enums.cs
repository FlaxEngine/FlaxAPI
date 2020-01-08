// Copyright (c) 2012-2020 Wojciech Figat. All rights reserved.

// ReSharper disable InconsistentNaming

namespace FlaxEngine
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
        /// DirectX (Shader Model 4 compatible)
        /// </summary>
        DirectX_SM4 = 1,

        /// <summary>
        /// DirectX (Shader Model 5 compatible)
        /// </summary>
        DirectX_SM5 = 2,

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

        /// <summary>
        /// PlayStation 4
        /// </summary>
        PS4 = 6,
    }

    /// <summary>
    /// Graphics feature levels indicates what level of support can be relied upon. 
    /// They are named after the graphics API to indicate the minimum level of the features set to support. 
    /// Feature levels are ordered from the lowest to the most high-end so feature level enum can be used to switch between feature levels (e.g. don't use geometry shader if not supported).
    /// </summary>
    public enum FeatureLevel
    {
        /// <summary>
        /// The features set defined by the core capabilities of OpenGL ES2.
        /// </summary>
        ES2 = 0,

        /// <summary>
        /// The features set defined by the core capabilities of OpenGL ES3.
        /// </summary>
        ES3 = 1,

        /// <summary>
        /// The features set defined by the core capabilities of OpenGL ES3.1.
        /// </summary>
        ES3_1 = 2,

        /// <summary>
        /// The features set defined by the core capabilities of DirectX 10 Shader Model 4.
        /// </summary>
        SM4 = 3,

        /// <summary>
        /// The features set defined by the core capabilities of DirectX 11 Shader Model 5.
        /// </summary>
        SM5 = 4,
    }
}
