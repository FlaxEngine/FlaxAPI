// This code was auto-generated. Do not modify it.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace FlaxEngine
{
    /// <summary>
    /// Graphics rendering backend system types.
    /// </summary>
    [Tooltip("Graphics rendering backend system types.")]
    public enum RendererType
    {
        /// <summary>
        /// Unknown type
        /// </summary>
        [Tooltip("Unknown type")]
        Unknown = 0,

        /// <summary>
        /// DirectX 10
        /// </summary>
        [Tooltip("DirectX 10")]
        DirectX10 = 1,

        /// <summary>
        /// DirectX 10.1
        /// </summary>
        [Tooltip("DirectX 10.1")]
        DirectX10_1 = 2,

        /// <summary>
        /// DirectX 11
        /// </summary>
        [Tooltip("DirectX 11")]
        DirectX11 = 3,

        /// <summary>
        /// DirectX 12
        /// </summary>
        [Tooltip("DirectX 12")]
        DirectX12 = 4,

        /// <summary>
        /// OpenGL 4.1
        /// </summary>
        [Tooltip("OpenGL 4.1")]
        OpenGL4_1 = 5,

        /// <summary>
        /// OpenGL 4.4
        /// </summary>
        [Tooltip("OpenGL 4.4")]
        OpenGL4_4 = 6,

        /// <summary>
        /// OpenGL ES 3
        /// </summary>
        [Tooltip("OpenGL ES 3")]
        OpenGLES3 = 7,

        /// <summary>
        /// OpenGL ES 3.1
        /// </summary>
        [Tooltip("OpenGL ES 3.1")]
        OpenGLES3_1 = 8,

        /// <summary>
        /// Null backend
        /// </summary>
        [Tooltip("Null backend")]
        Null = 9,

        /// <summary>
        /// Vulkan
        /// </summary>
        [Tooltip("Vulkan")]
        Vulkan = 10,

        /// <summary>
        /// PlayStation 4
        /// </summary>
        [Tooltip("PlayStation 4")]
        PS4 = 11,

        /// <summary>
        /// The count of items in the RendererType enum.
        /// </summary>
        [HideInEditor]
        [Tooltip("The count of items in the RendererType enum.")]
        MAX,
    }
}

namespace FlaxEngine
{
    /// <summary>
    /// Shader profile types define the version and type of the shading language used by the graphics backend.
    /// </summary>
    [Tooltip("Shader profile types define the version and type of the shading language used by the graphics backend.")]
    public enum ShaderProfile
    {
        /// <summary>
        /// Unknown
        /// </summary>
        [Tooltip("Unknown")]
        Unknown = 0,

        /// <summary>
        /// DirectX (Shader Model 4 compatible)
        /// </summary>
        [Tooltip("DirectX (Shader Model 4 compatible)")]
        DirectX_SM4 = 1,

        /// <summary>
        /// DirectX (Shader Model 5 compatible)
        /// </summary>
        [Tooltip("DirectX (Shader Model 5 compatible)")]
        DirectX_SM5 = 2,

        /// <summary>
        /// GLSL 410
        /// </summary>
        [Tooltip("GLSL 410")]
        GLSL_410 = 3,

        /// <summary>
        /// GLSL 440
        /// </summary>
        [Tooltip("GLSL 440")]
        GLSL_440 = 4,

        /// <summary>
        /// Vulkan (Shader Model 5 compatible)
        /// </summary>
        [Tooltip("Vulkan (Shader Model 5 compatible)")]
        Vulkan_SM5 = 5,

        /// <summary>
        /// PlayStation 4
        /// </summary>
        [Tooltip("PlayStation 4")]
        PS4 = 6,

        /// <summary>
        /// The count of items in the ShaderProfile enum.
        /// </summary>
        [HideInEditor]
        [Tooltip("The count of items in the ShaderProfile enum.")]
        MAX,
    }
}

namespace FlaxEngine
{
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
        [Tooltip("The features set defined by the core capabilities of OpenGL ES2.")]
        ES2 = 0,

        /// <summary>
        /// The features set defined by the core capabilities of OpenGL ES3.
        /// </summary>
        [Tooltip("The features set defined by the core capabilities of OpenGL ES3.")]
        ES3 = 1,

        /// <summary>
        /// The features set defined by the core capabilities of OpenGL ES3.1.
        /// </summary>
        [Tooltip("The features set defined by the core capabilities of OpenGL ES3.1.")]
        ES3_1 = 2,

        /// <summary>
        /// The features set defined by the core capabilities of DirectX 10 Shader Model 4.
        /// </summary>
        [Tooltip("The features set defined by the core capabilities of DirectX 10 Shader Model 4.")]
        SM4 = 3,

        /// <summary>
        /// The features set defined by the core capabilities of DirectX 11 Shader Model 5.
        /// </summary>
        [Tooltip("The features set defined by the core capabilities of DirectX 11 Shader Model 5.")]
        SM5 = 4,

        /// <summary>
        /// The count of items in the FeatureLevel enum.
        /// </summary>
        [HideInEditor]
        [Tooltip("The count of items in the FeatureLevel enum.")]
        MAX,
    }
}

namespace FlaxEngine
{
    /// <summary>
    /// Multisample count level.
    /// </summary>
    [Tooltip("Multisample count level.")]
    public enum MSAALevel : int
    {
        /// <summary>
        /// Disabled multisampling.
        /// </summary>
        [Tooltip("Disabled multisampling.")]
        None = 1,

        /// <summary>
        /// Two samples per pixel.
        /// </summary>
        [Tooltip("Two samples per pixel.")]
        X2 = 2,

        /// <summary>
        /// Four samples per pixel.
        /// </summary>
        [Tooltip("Four samples per pixel.")]
        X4 = 4,

        /// <summary>
        /// Eight samples per pixel.
        /// </summary>
        [Tooltip("Eight samples per pixel.")]
        X8 = 8,
    }
}

namespace FlaxEngine
{
    /// <summary>
    /// Shadows casting modes by visual elements.
    /// </summary>
    [Flags]
    [Tooltip("Shadows casting modes by visual elements.")]
    public enum ShadowsCastingMode
    {
        /// <summary>
        /// Never render shadows.
        /// </summary>
        [Tooltip("Never render shadows.")]
        None = 0,

        /// <summary>
        /// Render shadows only in static views (env probes, lightmaps, etc.).
        /// </summary>
        [Tooltip("Render shadows only in static views (env probes, lightmaps, etc.).")]
        StaticOnly = 1,

        /// <summary>
        /// Render shadows only in dynamic views (game, editor, etc.).
        /// </summary>
        [Tooltip("Render shadows only in dynamic views (game, editor, etc.).")]
        DynamicOnly = 2,

        /// <summary>
        /// Always render shadows.
        /// </summary>
        [Tooltip("Always render shadows.")]
        All = StaticOnly | DynamicOnly,
    }
}

namespace FlaxEngine
{
    /// <summary>
    /// Identifies expected GPU resource use during rendering. The usage directly reflects whether a resource is accessible by the CPU and/or the GPU.
    /// </summary>
    [Tooltip("Identifies expected GPU resource use during rendering. The usage directly reflects whether a resource is accessible by the CPU and/or the GPU.")]
    public enum GPUResourceUsage
    {
        /// <summary>
        /// A resource that requires read and write access by the GPU.
        /// This is likely to be the most common usage choice.
        /// Memory will be used on device only, so fast access from the device is preferred.
        /// It usually means device-local GPU (video) memory.
        /// </summary>
        /// <remarks>
        /// Usage:
        /// - Resources written and read by device, e.g. images used as render targets.
        /// - Resources transferred from host once (immutable) or infrequently and read by
        ///   device multiple times, e.g. textures to be sampled, vertex buffers, constant
        ///   buffers, and majority of other types of resources used on GPU.
        /// </remarks>
        Default = 0,

        /// <summary>
        /// A resource that is accessible by both the GPU (read only) and the CPU (write only).
        /// A dynamic resource is a good choice for a resource that will be updated by the CPU at least once per frame.
        /// Dynamic buffers or textures are usually used to upload data to GPU and use it within a single frame.
        /// </summary>
        /// <remarks>
        /// Usage:
        /// - Resources written frequently by CPU (dynamic), read by device.
        ///   E.g. textures, vertex buffers, uniform buffers updated every frame or every draw call.
        /// </remarks>
        Dynamic = 1,

        /// <summary>
        /// A resource that supports data transfer (copy) from the CPU to the GPU.
        /// It usually means CPU (system) memory. Resources created in this pool may still be accessible to the device, but access to them can be slow.
        /// </summary>
        /// <remarks>
        /// Usage:
        /// - Staging copy of resources used as transfer source.
        /// </remarks>
        StagingUpload = 2,

        /// <summary>
        /// A resource that supports data transfer (copy) from the GPU to the CPU.
        /// </summary>
        /// <remarks>
        /// Usage:
        /// - Resources written by device, read by host - results of some computations, e.g. screen capture, average scene luminance for HDR tone mapping.
        /// - Any resources read or accessed randomly on host, e.g. CPU-side copy of vertex buffer used as source of transfer, but also used for collision detection.
        /// </remarks>
        [Tooltip("A resource that supports data transfer (copy) from the GPU to the CPU.")]
        StagingReadback = 3,
    }
}

namespace FlaxEngine
{
    /// <summary>
    /// Describes how a mapped GPU resource will be accessed.
    /// </summary>
    [Flags]
    [Tooltip("Describes how a mapped GPU resource will be accessed.")]
    public enum GPUResourceMapMode
    {
        /// <summary>
        /// The resource is mapped for reading.
        /// </summary>
        [Tooltip("The resource is mapped for reading.")]
        Read = 0x01,

        /// <summary>
        /// The resource is mapped for writing.
        /// </summary>
        [Tooltip("The resource is mapped for writing.")]
        Write = 0x02,

        /// <summary>
        /// The resource is mapped for reading and writing.
        /// </summary>
        [Tooltip("The resource is mapped for reading and writing.")]
        ReadWrite = Read | Write,
    }
}

namespace FlaxEngine
{
    /// <summary>
    /// Primitives types.
    /// </summary>
    [Tooltip("Primitives types.")]
    public enum PrimitiveTopologyType
    {
        Undefined = 0,

        Point = 1,

        Line = 2,

        Triangle = 3,
    }
}

namespace FlaxEngine
{
    /// <summary>
    /// Primitives culling mode.
    /// </summary>
    [Tooltip("Primitives culling mode.")]
    public enum CullMode : byte
    {
        /// <summary>
        /// Cull back-facing primitives only.
        /// </summary>
        [Tooltip("Cull back-facing primitives only.")]
        Normal = 0,

        /// <summary>
        /// Cull front-facing primitives only.
        /// </summary>
        [Tooltip("Cull front-facing primitives only.")]
        Inverted = 1,

        /// <summary>
        /// Disable face culling.
        /// </summary>
        [Tooltip("Disable face culling.")]
        TwoSided = 2,
    }
}

namespace FlaxEngine
{
    /// <summary>
    /// Rendering quality levels.
    /// </summary>
    [Tooltip("Rendering quality levels.")]
    public enum Quality : byte
    {
        /// <summary>
        /// The low quality.
        /// </summary>
        [Tooltip("The low quality.")]
        Low = 0,

        /// <summary>
        /// The medium quality.
        /// </summary>
        [Tooltip("The medium quality.")]
        Medium = 1,

        /// <summary>
        /// The high quality.
        /// </summary>
        [Tooltip("The high quality.")]
        High = 2,

        /// <summary>
        /// The ultra, mega, fantastic quality!
        /// </summary>
        [Tooltip("The ultra, mega, fantastic quality!")]
        Ultra = 3,

        /// <summary>
        /// The count of items in the Quality enum.
        /// </summary>
        [HideInEditor]
        [Tooltip("The count of items in the Quality enum.")]
        MAX,
    }
}

namespace FlaxEngine
{
    /// <summary>
    /// Post Fx material rendering locations.
    /// </summary>
    [Tooltip("Post Fx material rendering locations.")]
    public enum MaterialPostFxLocation : byte
    {
        /// <summary>
        /// The after post processing pass using LDR input frame.
        /// </summary>
        [Tooltip("The after post processing pass using LDR input frame.")]
        AfterPostProcessingPass = 0,

        /// <summary>
        /// The before post processing pass using HDR input frame.
        /// </summary>
        [Tooltip("The before post processing pass using HDR input frame.")]
        BeforePostProcessingPass = 1,

        /// <summary>
        /// The before forward pass but after GBuffer with HDR input frame.
        /// </summary>
        [Tooltip("The before forward pass but after GBuffer with HDR input frame.")]
        BeforeForwardPass = 2,

        /// <summary>
        /// The after custom post effects.
        /// </summary>
        [Tooltip("The after custom post effects.")]
        AfterCustomPostEffects = 3,

        /// <summary>
        /// The 'before' Reflections pass. After the Light pass. Can be used to implement a custom light types that accumulate lighting to the light buffer.
        /// </summary>
        [Tooltip("The 'before' Reflections pass. After the Light pass. Can be used to implement a custom light types that accumulate lighting to the light buffer.")]
        BeforeReflectionsPass = 4,

        /// <summary>
        /// The 'after' AA filter pass. Rendering is done to the output backbuffer.
        /// </summary>
        [Tooltip("The 'after' AA filter pass. Rendering is done to the output backbuffer.")]
        AfterAntiAliasingPass = 5,
    }
}

namespace FlaxEngine
{
    /// <summary>
    /// The Post Process effect rendering location within the rendering pipeline.
    /// </summary>
    [Tooltip("The Post Process effect rendering location within the rendering pipeline.")]
    public enum PostProcessEffectLocation
    {
        /// <summary>
        /// The default location after the in-build PostFx pass (bloom, color grading, etc.) but before anti-aliasing effect.
        /// </summary>
        [Tooltip("The default location after the in-build PostFx pass (bloom, color grading, etc.) but before anti-aliasing effect.")]
        Default = 0,

        /// <summary>
        ///The 'before' in-build PostFx pass (bloom, color grading, etc.). After Forward Pass (transparency) and fog effects.
        /// </summary>
        [Tooltip("he 'before' in-build PostFx pass (bloom, color grading, etc.). After Forward Pass (transparency) and fog effects.")]
        BeforePostProcessingPass = 1,

        /// <summary>
        /// The 'before' Forward pass (transparency) and fog effects. After the Light pass and Reflections pass.
        /// </summary>
        [Tooltip("The 'before' Forward pass (transparency) and fog effects. After the Light pass and Reflections pass.")]
        BeforeForwardPass = 2,

        /// <summary>
        /// The 'before' Reflections pass. After the Light pass. Can be used to implement a custom light types that accumulate lighting to the light buffer.
        /// </summary>
        [Tooltip("The 'before' Reflections pass. After the Light pass. Can be used to implement a custom light types that accumulate lighting to the light buffer.")]
        BeforeReflectionsPass = 3,

        /// <summary>
        /// The 'after' AA filter pass. Rendering is done to the output backbuffer.
        /// </summary>
        [Tooltip("The 'after' AA filter pass. Rendering is done to the output backbuffer.")]
        AfterAntiAliasingPass = 4,
    }
}

namespace FlaxEngine
{
    /// <summary>
    /// The objects drawing pass types. Used as a flags for objects drawing masking.
    /// </summary>
    [Flags]
    [Tooltip("The objects drawing pass types. Used as a flags for objects drawing masking.")]
    public enum DrawPass : int
    {
        /// <summary>
        /// The none.
        /// </summary>
        [Tooltip("The none.")]
        None = 0,

        /// <summary>
        /// The hardware depth rendering to the depth buffer (used for shadow maps rendering).
        /// </summary>
        [Tooltip("The hardware depth rendering to the depth buffer (used for shadow maps rendering).")]
        Depth = 1,

        /// <summary>
        /// The base pass rendering to the GBuffer (for opaque materials).
        /// </summary>
        [EditorDisplay(name: "GBuffer")]
        [Tooltip("The base pass rendering to the GBuffer (for opaque materials).")]
        GBuffer = 1 << 1,

        /// <summary>
        /// The forward pass rendering (for transparent materials).
        /// </summary>
        [Tooltip("The forward pass rendering (for transparent materials).")]
        Forward = 1 << 2,

        /// <summary>
        /// The transparent objects distortion vectors rendering (with blending).
        /// </summary>
        [Tooltip("The transparent objects distortion vectors rendering (with blending).")]
        Distortion = 1 << 3,

        /// <summary>
        /// The motion vectors (velocity) rendering pass (for movable objects).
        /// </summary>
        [Tooltip("The motion vectors (velocity) rendering pass (for movable objects).")]
        MotionVectors = 1 << 4,

        /// <summary>
        /// The default set of draw passes for the scene objects.
        /// </summary>
        [HideInEditor]
        [Tooltip("The default set of draw passes for the scene objects.")]
        Default = Depth | GBuffer | Forward | Distortion | MotionVectors,

        /// <summary>
        /// The all draw passes combined into a single mask.
        /// </summary>
        [HideInEditor]
        [Tooltip("The all draw passes combined into a single mask.")]
        All = Depth | GBuffer | Forward | Distortion | MotionVectors,
    }
}

namespace FlaxEngine
{
    /// <summary>
    /// Describes frame rendering modes.
    /// </summary>
    [Tooltip("Describes frame rendering modes.")]
    public enum ViewMode
    {
        /// <summary>
        /// Full rendering
        /// </summary>
        [Tooltip("Full rendering")]
        Default = 0,

        /// <summary>
        /// Without post-process pass
        /// </summary>
        [Tooltip("Without post-process pass")]
        NoPostFx = 1,

        /// <summary>
        /// Draw Diffuse
        /// </summary>
        [Tooltip("Draw Diffuse")]
        Diffuse = 2,

        /// <summary>
        /// Draw Normals
        /// </summary>
        [Tooltip("Draw Normals")]
        Normals = 3,

        /// <summary>
        /// Draw Emissive
        /// </summary>
        [Tooltip("Draw Emissive")]
        Emissive = 4,

        /// <summary>
        /// Draw Depth
        /// </summary>
        [Tooltip("Draw Depth")]
        Depth = 5,

        /// <summary>
        /// Draw Ambient Occlusion
        /// </summary>
        [Tooltip("Draw Ambient Occlusion")]
        AmbientOcclusion = 6,

        /// <summary>
        /// Draw Material's Metalness
        /// </summary>
        [Tooltip("Draw Material's Metalness")]
        Metalness = 7,

        /// <summary>
        /// Draw Material's Roughness
        /// </summary>
        [Tooltip("Draw Material's Roughness")]
        Roughness = 8,

        /// <summary>
        /// Draw Material's Specular
        /// </summary>
        [Tooltip("Draw Material's Specular")]
        Specular = 9,

        /// <summary>
        /// Draw Material's Specular Color
        /// </summary>
        [Tooltip("Draw Material's Specular Color")]
        SpecularColor = 10,

        /// <summary>
        /// Draw Shading Model
        /// </summary>
        [Tooltip("Draw Shading Model")]
        ShadingModel = 11,

        /// <summary>
        /// Draw Lights buffer
        /// </summary>
        [Tooltip("Draw Lights buffer")]
        LightBuffer = 12,

        /// <summary>
        /// Draw reflections buffer
        /// </summary>
        [Tooltip("Draw reflections buffer")]
        Reflections = 13,

        /// <summary>
        /// Draw scene objects in wireframe mode
        /// </summary>
        [Tooltip("Draw scene objects in wireframe mode")]
        Wireframe = 14,

        /// <summary>
        /// Draw motion vectors debug view
        /// </summary>
        [Tooltip("Draw motion vectors debug view")]
        MotionVectors = 15,

        /// <summary>
        /// Draw materials subsurface color debug view
        /// </summary>
        [Tooltip("Draw materials subsurface color debug view")]
        SubsurfaceColor = 16,

        /// <summary>
        /// Draw materials colors with ambient occlusion
        /// </summary>
        [Tooltip("Draw materials colors with ambient occlusion")]
        Unlit = 17,
    }
}

namespace FlaxEngine
{
    /// <summary>
    /// Frame rendering flags used to switch between graphics features.
    /// </summary>
    [Flags]
    [Tooltip("Frame rendering flags used to switch between graphics features.")]
    public enum ViewFlags : long
    {
        /// <summary>
        /// Nothing.
        /// </summary>
        [Tooltip("Nothing.")]
        None = 0,

        /// <summary>
        /// Shows/hides the debug shapes rendered using Debug Draw.
        /// </summary>
        [Tooltip("Shows/hides the debug shapes rendered using Debug Draw.")]
        DebugDraw = 1,

        /// <summary>
        /// Shows/hides Editor sprites
        /// </summary>
        [Tooltip("Shows/hides Editor sprites")]
        EditorSprites = 1 << 1,

        /// <summary>
        /// Shows/hides reflections
        /// </summary>
        [Tooltip("Shows/hides reflections")]
        Reflections = 1 << 2,

        /// <summary>
        /// Shows/hides Screen Space Reflections
        /// </summary>
        [Tooltip("Shows/hides Screen Space Reflections")]
        SSR = 1 << 3,

        /// <summary>
        /// Shows/hides Ambient Occlusion effect
        /// </summary>
        [Tooltip("Shows/hides Ambient Occlusion effect")]
        AO = 1 << 4,

        /// <summary>
        /// Shows/hides Global Illumination effect
        /// </summary>
        [Tooltip("Shows/hides Global Illumination effect")]
        GI = 1 << 5,

        /// <summary>
        /// Shows/hides directional lights
        /// </summary>
        [Tooltip("Shows/hides directional lights")]
        DirectionalLights = 1 << 6,

        /// <summary>
        /// Shows/hides point lights
        /// </summary>
        [Tooltip("Shows/hides point lights")]
        PointLights = 1 << 7,

        /// <summary>
        /// Shows/hides spot lights
        /// </summary>
        [Tooltip("Shows/hides spot lights")]
        SpotLights = 1 << 8,

        /// <summary>
        /// Shows/hides sky lights
        /// </summary>
        [Tooltip("Shows/hides sky lights")]
        SkyLights = 1 << 9,

        /// <summary>
        /// Shows/hides shadows
        /// </summary>
        [Tooltip("Shows/hides shadows")]
        Shadows = 1 << 10,

        /// <summary>
        /// Shows/hides specular light rendering
        /// </summary>
        [Tooltip("Shows/hides specular light rendering")]
        SpecularLight = 1 << 11,

        /// <summary>
        /// Shows/hides Anti-Aliasing
        /// </summary>
        [Tooltip("Shows/hides Anti-Aliasing")]
        AntiAliasing = 1 << 12,

        /// <summary>
        /// Shows/hides custom Post-Process effects
        /// </summary>
        [Tooltip("Shows/hides custom Post-Process effects")]
        CustomPostProcess = 1 << 13,

        /// <summary>
        /// Shows/hides bloom effect
        /// </summary>
        [Tooltip("Shows/hides bloom effect")]
        Bloom = 1 << 14,

        /// <summary>
        /// Shows/hides tone mapping effect
        /// </summary>
        [Tooltip("Shows/hides tone mapping effect")]
        ToneMapping = 1 << 15,

        /// <summary>
        /// Shows/hides eye adaptation effect
        /// </summary>
        [Tooltip("Shows/hides eye adaptation effect")]
        EyeAdaptation = 1 << 16,

        /// <summary>
        /// Shows/hides camera artifacts
        /// </summary>
        [Tooltip("Shows/hides camera artifacts")]
        CameraArtifacts = 1 << 17,

        /// <summary>
        /// Shows/hides lens flares
        /// </summary>
        [Tooltip("Shows/hides lens flares")]
        LensFlares = 1 << 18,

        /// <summary>
        /// Shows/hides deferred decals.
        /// </summary>
        [Tooltip("Shows/hides deferred decals.")]
        Decals = 1 << 19,

        /// <summary>
        /// Shows/hides depth of field effect
        /// </summary>
        [Tooltip("Shows/hides depth of field effect")]
        DepthOfField = 1 << 20,

        /// <summary>
        /// Shows/hides physics debug shapes.
        /// </summary>
        [Tooltip("Shows/hides physics debug shapes.")]
        PhysicsDebug = 1 << 21,

        /// <summary>
        /// Shows/hides fogging effects.
        /// </summary>
        [Tooltip("Shows/hides fogging effects.")]
        Fog = 1 << 22,

        /// <summary>
        /// Shows/hides the motion blur effect.
        /// </summary>
        [Tooltip("Shows/hides the motion blur effect.")]
        MotionBlur = 1 << 23,

        /// <summary>
        /// Default flags for Game.
        /// </summary>
        [Tooltip("Default flags for Game.")]
        DefaultGame = Reflections | DepthOfField | Fog | Decals | MotionBlur | SSR | AO | GI | DirectionalLights | PointLights | SpotLights | SkyLights | Shadows | SpecularLight | AntiAliasing | CustomPostProcess | Bloom | ToneMapping | EyeAdaptation | CameraArtifacts | LensFlares,

        /// <summary>
        /// Default flags for Editor.
        /// </summary>
        [Tooltip("Default flags for Editor.")]
        DefaultEditor = Reflections | Fog | Decals | DebugDraw | SSR | AO | GI | DirectionalLights | PointLights | SpotLights | SkyLights | Shadows | SpecularLight | AntiAliasing | CustomPostProcess | Bloom | ToneMapping | EyeAdaptation | CameraArtifacts | LensFlares | EditorSprites,

        /// <summary>
        /// Default flags for materials/models previews generating.
        /// </summary>
        [Tooltip("Default flags for materials/models previews generating.")]
        DefaultAssetPreview = Reflections | Decals | GI | DirectionalLights | PointLights | SpotLights | SkyLights | SpecularLight | AntiAliasing | Bloom | ToneMapping | EyeAdaptation | CameraArtifacts | LensFlares,
    }
}

namespace FlaxEngine
{
    /// <summary>
    /// Describes the different tessellation methods supported by the graphics system.
    /// </summary>
    [Tooltip("Describes the different tessellation methods supported by the graphics system.")]
    public enum TessellationMethod
    {
        /// <summary>
        /// No tessellation.
        /// </summary>
        [Tooltip("No tessellation.")]
        None = 0,

        /// <summary>
        /// Flat tessellation. Also known as dicing tessellation.
        /// </summary>
        [Tooltip("Flat tessellation. Also known as dicing tessellation.")]
        Flat = 1,

        /// <summary>
        /// Point normal tessellation.
        /// </summary>
        [Tooltip("Point normal tessellation.")]
        PointNormal = 2,

        /// <summary>
        /// Geometric version of Phong normal interpolation, not applied on normals but on the vertex positions.
        /// </summary>
        [Tooltip("Geometric version of Phong normal interpolation, not applied on normals but on the vertex positions.")]
        Phong = 3,
    }
}
