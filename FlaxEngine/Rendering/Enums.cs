// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

// ReSharper disable InconsistentNaming

namespace FlaxEngine.Rendering
{
    /// <summary>
    /// Graphics rendering backend system types.
    /// </summary>
    public enum RendererType
    {
        /// <summary>
        /// The unknown type
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// The DirectX 10
        /// </summary>
        DirectX10 = 1,

        /// <summary>
        /// The DirectX 10.1
        /// </summary>
        DirectX10_1 = 2,

        /// <summary>
        /// The DirectX 11
        /// </summary>
        DirectX11 = 3,

        /// <summary>
        /// The DirectX 12
        /// </summary>
        DirectX12 = 4,
    }

    /// <summary>
    /// The shader profile types.
    /// </summary>
    public enum ShaderProfile
    {
        /// <summary>
        /// The unknown
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// The Shader Model 4.0
        /// </summary>
        ShaderModel4 = 1,

        /// <summary>
        /// The Shader Model 5.0
        /// </summary>
        ShaderModel5 = 2,
    }
}
