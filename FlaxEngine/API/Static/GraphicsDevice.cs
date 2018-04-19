// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System.Runtime.InteropServices;

namespace FlaxEngine.Rendering
{
    public static partial class GraphicsDevice
    {
        /// <summary>
        /// Graphics device limits description.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct DeviceLimits
        {
            /// <summary>
            /// True if device supports Compute shaders.
            /// </summary>
            public bool IsComputeSupported;

            /// <summary>
            /// True if device supports separate render target blending states.
            /// </summary>
            public bool IsSupportingSeparateRTBlendState;

            /// <summary>
            /// True if device supports depth buffer texture as a shader resource view.
            /// </summary>
            public bool HasDepthAsSRV;

            /// <summary>
            /// True if device supports multisampled depth buffer texture as a shader resource view.
            /// </summary>
            public bool HasMultisampleDepthAsSRV;

            /// <summary>
            /// The maximum amount of texture mip levels.
            /// </summary>
            public int MaximumMipLevelsCount;

            /// <summary>
            /// The maximum size of the 1D texture.
            /// </summary>
            public int MaximumTexture1DSize;

            /// <summary>
            /// The maximum length of 1D textures array.
            /// </summary>
            public int MaximumTexture1DArraySize;

            /// <summary>
            /// The maximum size of the 2D texture.
            /// </summary>
            public int MaximumTexture2DSize;

            /// <summary>
            /// The maximum length of 2D textures array.
            /// </summary>
            public int MaximumTexture2DArraySize;

            /// <summary>
            /// The maximum size of the 3D texture.
            /// </summary>
            public int MaximumTexture3DSize;

            /// <summary>
            /// The maximum size of the cube texture (both width and height).
            /// </summary>
            public int MaximumTextureCubeSize;
        }
    }
}
