////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

namespace FlaxEngine.Rendering
{
    /// <summary>
    /// Describies frame rendering modes.
    /// </summary>
    public enum ViewMode
    {
        /// <summary>
        /// Full rendering
        /// </summary>
        Default = 0,

        /// <summary>
        /// Fast rendering. Without post-process
        /// </summary>
        Fast = 1,

        /// <summary>
        /// Draw Diffuse
        /// </summary>
        Diffuse = 2,

        /// <summary>
        /// Draw Normals
        /// </summary>
        Normals = 3,

        /// <summary>
        /// Draw Emissive
        /// </summary>
        Emissive = 4,

        /// <summary>
        /// Draw Depth
        /// </summary>
        Depth = 5,

        /// <summary>
        /// Draw Ambient Occlusion
        /// </summary>
        AmbientOcclusion = 6,

        /// <summary>
        /// Draw Material's Metalness
        /// </summary>
        Metalness = 7,

        /// <summary>
        /// Draw Material's Roughness
        /// </summary>
        Roughness = 8,

        /// <summary>
        /// Draw Material's Specular
        /// </summary>
        Specular = 9,

        /// <summary>
        /// Draw Material's Specular Color
        /// </summary>
        SpecularColor = 10,

        /// <summary>
        /// Draw Shading Model
        /// </summary>
        ShadingModel = 11,

        /// <summary>
        /// Draw Lights buffer
        /// </summary>
        LightsBuffer = 12,

        /// <summary>
        /// Draw reflections buffer
        /// </summary>
        Reflections = 13,
    }
}
