// This code was auto-generated. Do not modify it.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace FlaxEngine
{
    /// <summary>
    /// Material domain type. Material domain defines the target usage of the material shader.
    /// </summary>
    [Tooltip("Material domain type. Material domain defines the target usage of the material shader.")]
    public enum MaterialDomain : byte
    {
        /// <summary>
        /// The surface material. Can be used to render the scene geometry including models and skinned models.
        /// </summary>
        [Tooltip("The surface material. Can be used to render the scene geometry including models and skinned models.")]
        Surface = 0,

        /// <summary>
        /// The post process material. Can be used to perform custom post-processing of the rendered frame.
        /// </summary>
        [Tooltip("The post process material. Can be used to perform custom post-processing of the rendered frame.")]
        PostProcess = 1,

        /// <summary>
        /// The deferred decal material. Can be used to apply custom overlay or surface modifications to the object surfaces in the world.
        /// </summary>
        [Tooltip("The deferred decal material. Can be used to apply custom overlay or surface modifications to the object surfaces in the world.")]
        Decal = 2,

        /// <summary>
        /// The GUI shader. Can be used to draw custom control interface elements or to add custom effects to the GUI.
        /// </summary>
        [Tooltip("The GUI shader. Can be used to draw custom control interface elements or to add custom effects to the GUI.")]
        GUI = 3,

        /// <summary>
        /// The terrain shader. Can be used only with landscape chunks geometry that use optimized vertex data and support multi-layered blending.
        /// </summary>
        [Tooltip("The terrain shader. Can be used only with landscape chunks geometry that use optimized vertex data and support multi-layered blending.")]
        Terrain = 4,

        /// <summary>
        /// The particle shader. Can be used only with particles geometry (sprites, trails and ribbons). Supports reading particle data on a GPU.
        /// </summary>
        [Tooltip("The particle shader. Can be used only with particles geometry (sprites, trails and ribbons). Supports reading particle data on a GPU.")]
        Particle = 5,
    }
}

namespace FlaxEngine
{
    /// <summary>
    /// Material blending modes.
    /// </summary>
    [Tooltip("Material blending modes.")]
    public enum MaterialBlendMode : byte
    {
        /// <summary>
        /// The opaque material. Used during GBuffer pass rendering.
        /// </summary>
        [Tooltip("The opaque material. Used during GBuffer pass rendering.")]
        Opaque = 0,

        /// <summary>
        /// The transparent material. Used during Forward pass rendering.
        /// </summary>
        [Tooltip("The transparent material. Used during Forward pass rendering.")]
        Transparent = 1,

        /// <summary>
        /// The additive blend. Material color is used to add to color of the objects behind the surface. Used during Forward pass rendering.
        /// </summary>
        [Tooltip("The additive blend. Material color is used to add to color of the objects behind the surface. Used during Forward pass rendering.")]
        Additive = 2,

        /// <summary>
        /// The multiply blend. Material color is used to multiply color of the objects behind the surface. Used during Forward pass rendering.
        /// </summary>
        [Tooltip("The multiply blend. Material color is used to multiply color of the objects behind the surface. Used during Forward pass rendering.")]
        Multiply = 3,
    }
}

namespace FlaxEngine
{
    /// <summary>
    /// Material shading modes. Defines how material inputs and properties are combined to result the final surface color.
    /// </summary>
    [Tooltip("Material shading modes. Defines how material inputs and properties are combined to result the final surface color.")]
    public enum MaterialShadingModel : byte
    {
        /// <summary>
        /// The unlit material. Emissive channel is used as an output color. Can perform custom lighting operations or just glow. Won't be affected by the lighting pipeline.
        /// </summary>
        [Tooltip("The unlit material. Emissive channel is used as an output color. Can perform custom lighting operations or just glow. Won't be affected by the lighting pipeline.")]
        Unlit = 0,

        /// <summary>
        /// The default lit material. The most common choice for the material surfaces.
        /// </summary>
        [Tooltip("The default lit material. The most common choice for the material surfaces.")]
        Lit = 1,

        /// <summary>
        /// The subsurface material. Intended for materials like vax or skin that need light scattering to transport simulation through the object.
        /// </summary>
        [Tooltip("The subsurface material. Intended for materials like vax or skin that need light scattering to transport simulation through the object.")]
        Subsurface = 2,
    }
}

namespace FlaxEngine
{
    /// <summary>
    /// Material features flags.
    /// </summary>
    [Flags]
    [Tooltip("Material features flags.")]
    public enum MaterialFeaturesFlags : uint
    {
        /// <summary>
        /// No flags.
        /// </summary>
        [Tooltip("No flags.")]
        None = 0,

        /// <summary>
        /// The wireframe material.
        /// </summary>
        [Tooltip("The wireframe material.")]
        Wireframe = 1 << 1,

        /// <summary>
        /// The depth test is disabled (material ignores depth).
        /// </summary>
        [Tooltip("The depth test is disabled (material ignores depth).")]
        DisableDepthTest = 1 << 2,

        /// <summary>
        /// Disable depth buffer write (won't modify depth buffer value during rendering).
        /// </summary>
        [Tooltip("Disable depth buffer write (won't modify depth buffer value during rendering).")]
        DisableDepthWrite = 1 << 3,

        /// <summary>
        /// The flag used to indicate that material input normal vector is defined in the world space rather than tangent space.
        /// </summary>
        [Tooltip("The flag used to indicate that material input normal vector is defined in the world space rather than tangent space.")]
        InputWorldSpaceNormal = 1 << 4,

        /// <summary>
        /// The flag used to indicate that material uses dithered model LOD transition for smoother LODs switching.
        /// </summary>
        [Tooltip("The flag used to indicate that material uses dithered model LOD transition for smoother LODs switching.")]
        DitheredLODTransition = 1 << 5,

        /// <summary>
        /// The flag used to disable fog. The Forward Pass materials option.
        /// </summary>
        [Tooltip("The flag used to disable fog. The Forward Pass materials option.")]
        DisableFog = 1 << 6,

        /// <summary>
        /// The flag used to disable reflections. The Forward Pass materials option.
        /// </summary>
        [Tooltip("The flag used to disable reflections. The Forward Pass materials option.")]
        DisableReflections = 1 << 7,

        /// <summary>
        /// The flag used to disable distortion effect (light refraction). The Forward Pass materials option.
        /// </summary>
        [Tooltip("The flag used to disable distortion effect (light refraction). The Forward Pass materials option.")]
        DisableDistortion = 1 << 8,
    }
}

namespace FlaxEngine
{
    /// <summary>
    /// Material features usage flags. Detected by the material generator to help graphics pipeline optimize rendering of material shaders.
    /// </summary>
    [Flags]
    [Tooltip("Material features usage flags. Detected by the material generator to help graphics pipeline optimize rendering of material shaders.")]
    public enum MaterialUsageFlags : uint
    {
        /// <summary>
        /// No flags.
        /// </summary>
        [Tooltip("No flags.")]
        None = 0,

        /// <summary>
        /// Material is using mask to discard some pixels. Masked materials are using full vertex buffer during shadow maps and depth pass rendering (need UVs).
        /// </summary>
        [Tooltip("Material is using mask to discard some pixels. Masked materials are using full vertex buffer during shadow maps and depth pass rendering (need UVs).")]
        UseMask = 1 << 0,

        /// <summary>
        /// The material is using emissive light.
        /// </summary>
        [Tooltip("The material is using emissive light.")]
        UseEmissive = 1 << 1,

        /// <summary>
        /// The material is using world position offset (it may be animated inside a shader).
        /// </summary>
        [Tooltip("The material is using world position offset (it may be animated inside a shader).")]
        UsePositionOffset = 1 << 2,

        /// <summary>
        /// The material is using vertex colors. The render will try to feed the pipeline with a proper buffer so material can gather valid data.
        /// </summary>
        [Tooltip("The material is using vertex colors. The render will try to feed the pipeline with a proper buffer so material can gather valid data.")]
        UseVertexColor = 1 << 3,

        /// <summary>
        /// The material is using per-pixel normal mapping.
        /// </summary>
        [Tooltip("The material is using per-pixel normal mapping.")]
        UseNormal = 1 << 4,

        /// <summary>
        /// The material is using position displacement (in domain shader).
        /// </summary>
        [Tooltip("The material is using position displacement (in domain shader).")]
        UseDisplacement = 1 << 5,

        /// <summary>
        /// The flag used to indicate that material uses refraction feature.
        /// </summary>
        [Tooltip("The flag used to indicate that material uses refraction feature.")]
        UseRefraction = 1 << 6,
    }
}

namespace FlaxEngine
{
    /// <summary>
    /// Decal material blending modes.
    /// </summary>
    [Tooltip("Decal material blending modes.")]
    public enum MaterialDecalBlendingMode : byte
    {
        /// <summary>
        /// Decal will be fully blended with the material surface.
        /// </summary>
        [Tooltip("Decal will be fully blended with the material surface.")]
        Translucent = 0,

        /// <summary>
        /// Decal color will be blended with the material surface color (using multiplication).
        /// </summary>
        [Tooltip("Decal color will be blended with the material surface color (using multiplication).")]
        Stain = 1,

        /// <summary>
        /// Decal will blend the normal vector only.
        /// </summary>
        [Tooltip("Decal will blend the normal vector only.")]
        Normal = 2,

        /// <summary>
        /// Decal will apply the emissive light only.
        /// </summary>
        [Tooltip("Decal will apply the emissive light only.")]
        Emissive = 3,
    }
}

namespace FlaxEngine
{
    /// <summary>
    /// Material input scene textures. Special inputs from the graphics pipeline.
    /// </summary>
    [Tooltip("Material input scene textures. Special inputs from the graphics pipeline.")]
    public enum MaterialSceneTextures
    {
        /// <summary>
        /// The scene color.
        /// </summary>
        [Tooltip("The scene color.")]
        SceneColor = 0,

        /// <summary>
        /// The scene depth.
        /// </summary>
        [Tooltip("The scene depth.")]
        SceneDepth = 1,

        /// <summary>
        /// The material diffuse color.
        /// </summary>
        [Tooltip("The material diffuse color.")]
        DiffuseColor = 2,

        /// <summary>
        /// The material specular color.
        /// </summary>
        [Tooltip("The material specular color.")]
        SpecularColor = 3,

        /// <summary>
        /// The material world space normal.
        /// </summary>
        [Tooltip("The material world space normal.")]
        WorldNormal = 4,

        /// <summary>
        /// The ambient occlusion.
        /// </summary>
        [Tooltip("The ambient occlusion.")]
        AmbientOcclusion = 5,

        /// <summary>
        /// The material metalness value.
        /// </summary>
        [Tooltip("The material metalness value.")]
        Metalness = 6,

        /// <summary>
        /// The material roughness value.
        /// </summary>
        [Tooltip("The material roughness value.")]
        Roughness = 7,

        /// <summary>
        /// The material specular value.
        /// </summary>
        [Tooltip("The material specular value.")]
        Specular = 8,

        /// <summary>
        /// The material color.
        /// </summary>
        [Tooltip("The material color.")]
        BaseColor = 9,

        /// <summary>
        /// The material shading mode.
        /// </summary>
        [Tooltip("The material shading mode.")]
        ShadingModel = 10,
    }
}

namespace FlaxEngine
{
    /// <summary>
    /// Structure with basic information about the material surface. It describes how material is reacting on light and which graphical features of it requires to render.
    /// </summary>
    [Tooltip("Structure with basic information about the material surface. It describes how material is reacting on light and which graphical features of it requires to render.")]
    [StructLayout(LayoutKind.Sequential)]
    public unsafe partial struct MaterialInfo
    {
        /// <summary>
        /// The material shader domain.
        /// </summary>
        [Tooltip("The material shader domain.")]
        public MaterialDomain Domain;

        /// <summary>
        /// The blending mode for rendering.
        /// </summary>
        [Tooltip("The blending mode for rendering.")]
        public MaterialBlendMode BlendMode;

        /// <summary>
        /// The shading mode for lighting.
        /// </summary>
        [Tooltip("The shading mode for lighting.")]
        public MaterialShadingModel ShadingModel;

        /// <summary>
        /// The usage flags.
        /// </summary>
        [Tooltip("The usage flags.")]
        public MaterialUsageFlags UsageFlags;

        /// <summary>
        /// The features usage flags.
        /// </summary>
        [Tooltip("The features usage flags.")]
        public MaterialFeaturesFlags FeaturesFlags;

        /// <summary>
        /// The decal material blending mode.
        /// </summary>
        [Tooltip("The decal material blending mode.")]
        public MaterialDecalBlendingMode DecalBlendingMode;

        /// <summary>
        /// The post fx material rendering location.
        /// </summary>
        [Tooltip("The post fx material rendering location.")]
        public MaterialPostFxLocation PostFxLocation;

        /// <summary>
        /// The primitives culling mode.
        /// </summary>
        [Tooltip("The primitives culling mode.")]
        public CullMode CullMode;

        /// <summary>
        /// The mask threshold.
        /// </summary>
        [Tooltip("The mask threshold.")]
        public float MaskThreshold;

        /// <summary>
        /// The opacity threshold.
        /// </summary>
        [Tooltip("The opacity threshold.")]
        public float OpacityThreshold;

        /// <summary>
        /// The tessellation mode.
        /// </summary>
        [Tooltip("The tessellation mode.")]
        public TessellationMethod TessellationMode;

        /// <summary>
        /// The maximum tessellation factor (used only if material uses tessellation).
        /// </summary>
        [Tooltip("The maximum tessellation factor (used only if material uses tessellation).")]
        public int MaxTessellationFactor;
    }
}
