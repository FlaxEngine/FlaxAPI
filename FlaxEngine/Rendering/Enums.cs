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
        /// Null backend
        /// </summary>
        Null = 9,
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
    }
}
