// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

namespace FlaxEngine.Rendering
{
    /// <summary>
    /// Rendering pass types.
    /// </summary>
    public enum RenderPass
    {
        /// <summary>
        /// Basic pass used to fill GBuffer with data of solid materials.
        /// </summary>
        GBufferFill,

        /// <summary>
        /// Forward rendering pass.
        /// </summary>
        ForwardPass,

        /// <summary>
        /// Render hardware depth (raw depth buffer).
        /// </summary>
        DepthHW,

        /// <summary>
        /// Output transparent materials distortion vectors (with blending).
        /// </summary>
        TransparentDistortion,

        /// <summary>
        /// Motion vectors rendering pass (for dynamic objects).
        /// </summary>
        MotionVectors,
    }
}
