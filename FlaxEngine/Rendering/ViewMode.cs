// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

namespace FlaxEngine.Rendering
{
    /// <summary>
    /// Describes frame rendering modes.
    /// </summary>
    public enum ViewMode
    {
        /// <summary>
        /// Full rendering
        /// </summary>
        Default = 0,

        /// <summary>
        /// Without post-process pass
        /// </summary>
        NoPostFx = 1,

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
        /// Draw Light buffer
        /// </summary>
        LightBuffer = 12,

        /// <summary>
        /// Draw reflections buffer
        /// </summary>
        Reflections = 13,

        /// <summary>
        /// Draw scene objects in wireframe mode
        /// </summary>
        Wireframe = 14,

        /// <summary>
        /// Draw motion vectors debug view
        /// </summary>
        MotionVectors = 15,

        /// <summary>
        /// Draw materials subsurface color debug view
        /// </summary>
        SubsurfaceColor = 16,
    }
}
