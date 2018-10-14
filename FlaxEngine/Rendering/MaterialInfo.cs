// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System;
using System.Runtime.InteropServices;

namespace FlaxEngine.Rendering
{
    /// <summary>
    /// Material domain type. Material domain defines the target usage of the material shader.
    /// </summary>
    public enum MaterialDomain : byte
    {
        /// <summary>
        /// The surface material. Can be used to render the scene geometry including models and skinned models.
        /// </summary>
        Surface = 0,

        /// <summary>
        /// The post process material. Can be used to perform custom post-processing of the rendered frame.
        /// </summary>
        PostProcess = 1,

        /// <summary>
        /// The deferred decal material. Can be used to apply custom overlay or surface modifications to the object surfaces in the world.
        /// </summary>
        Decal = 2,

        /// <summary>
        /// The GUI shader. Can be used to draw custom control interface elements or to add custom effects to the GUI.
        /// </summary>
        GUI = 3,

        /// <summary>
        /// The terrain shader. Can be used only with landscape chunks geometry that use optimized vertex data and support multi-layered blending.
        /// </summary>
        Terrain = 4,
    }

    /// <summary>
    /// Material blending modes.
    /// </summary>
    public enum MaterialBlendMode : byte
    {
        /// <summary>
        /// The opaque material. Used during GBuffer pass rendering.
        /// </summary>
        Opaque = 0,

        /// <summary>
        /// The transparent material. Used during Forward pass rendering.
        /// </summary>
        Transparent = 1,

        /// <summary>
        /// The additive blend. Material color is used to add to color of the objects behind the surface. Used during Forward pass rendering.
        /// </summary>
        Additive = 2,

        /// <summary>
        /// The multiply blend. Material color is used to multiply color of the objects behind the surface. Used during Forward pass rendering.
        /// </summary>
        Multiply = 3,
    }

    /// <summary>
    /// Material shading models. Defines how material inputs and properties are combined to result the final surface color.
    /// </summary>
    public enum MaterialShadingModel : byte
    {
        /// <summary>
        /// The unlit material. Emissive channel is used as an output color. Can perform custom lighting operations or just glow. Won't be affected by the lighting pipeline.
        /// </summary>
        Unlit = 0,

        /// <summary>
        /// The default lit material. The most common choice for the material surfaces.
        /// </summary>
        Lit = 1,

        /// <summary>
        /// The subsurface material. Intended for materials like vax or skin that need light scattering to transport simulation through the object.
        /// </summary>
        Subsurface = 2,
    }

    /// <summary>
    /// Material transparent lighting modes.
    /// </summary>
    public enum MaterialTransparentLighting : byte
    {
        /// <summary>
        /// Shading is disabled.
        /// </summary>
        None = 0,

        /// <summary>
        /// Shading is performed per pixel for single directional light.
        /// </summary>
        SingleDirectionalPerPixel = 1
    }

    /// <summary>
    /// Material usage flags.
    /// </summary>
    [Flags]
    public enum MaterialFlags : uint
    {
        /// <summary>
        /// The none.
        /// </summary>
        None = 0,

        /// <summary>
        /// Material is using mask to discard some pixels.
        /// Masked materials are using full vertex buffer during shadow maps and depth pass rendering (need UVs).
        /// </summary>
        UseMask = 1 << 0,

        /// <summary>
        /// The two sided material. No triangle normal culling is performed.
        /// </summary>
        TwoSided = 1 << 1,

        /// <summary>
        /// The wireframe material.
        /// </summary>
        Wireframe = 1 << 2,

        /// <summary>
        /// The material is using emissive light.
        /// </summary>
        UseEmissive = 1 << 3,

        /// <summary>
        /// The transparent materials option. Disable depth test (material ignores depth).
        /// </summary>
        TransparentDisableDepthTest = 1 << 4,

        /// <summary>
        /// The transparent materials option. Disable fog.
        /// </summary>
        TransparentDisableFog = 1 << 5,

        /// <summary>
        /// The transparent materials option. Disable reflections.
        /// </summary>
        TransparentDisableReflections = 1 << 6,

        /// <summary>
        /// The transparent materials option. Disable depth buffer write (won't modify depth buffer value after rendering).
        /// </summary>
        DisableDepthWrite = 1 << 7,

        /// <summary>
        /// The transparent materials option. Disable distortion.
        /// </summary>
        TransparentDisableDistortion = 1 << 8,

        /// <summary>
        /// The material is using world position offset (it may be animated inside a shader).
        /// </summary>
        UsePositionOffset = 1 << 9,

        /// <summary>
        /// The material is using vertex colors. The render will try to feed the pipeline with a proper buffer so material can gather valid data.
        /// </summary>
        UseVertexColor = 1 << 10,

        /// <summary>
        /// The material is using per-pixel normal mapping.
        /// </summary>
        UseNormal = 1 << 11,

        /// <summary>
        /// The material is using position displacement (in domain shader).
        /// </summary>
        UseDisplacement = 1 << 12,

        /// <summary>
        /// The flag used to indicate that material input normal vector is defined in the world space rather than tangent space.
        /// </summary>
        InputWorldSpaceNormal = 1 << 13,

        /// <summary>
        /// The flag used to indicate that material uses dithered model LOD transition for smoother LODs switching.
        /// </summary>
        UseDitheredLODTransition = 1 << 14,
    }

    /// <summary>
    /// Post Fx material rendering locations.
    /// </summary>
    public enum MaterialPostFxLocation : byte
    {
        /// <summary>
        /// The after post processing pass using LDR input frame.
        /// </summary>
        AfterPostProcessingPass = 0,

        /// <summary>
        /// The before post processing pass using HDR input frame.
        /// </summary>
        BeforePostProcessingPass = 1,

        /// <summary>
        /// The before forward pass but after GBuffer with HDR input frame.
        /// </summary>
        BeforeForwardPass = 2,

        /// <summary>
        /// The after custom post effects.
        /// </summary>
        AfterCustomPostEffects = 3,

        /// <summary>
        /// The 'before' Reflections pass. After the Light pass. Can be used to implement a custom light types that accumulate lighting to the light buffer.
        /// </summary>
        BeforeReflectionsPass = 4,

        /// <summary>
        /// The 'after' AA filter pass. Rendering is done to the output backbuffer.
        /// </summary>
        AfterAntiAliasingPass = 5,
    }

    /// <summary>
    /// Decal material blending modes.
    /// </summary>
    public enum MaterialDecalBlendingMode : byte
    {
        /// <summary>
        /// Decal will be fully blended with the material surface.
        /// </summary>
        Translucent = 0,

        /// <summary>
        /// Decal color will be blended with the material surface color (using multiplication).
        /// </summary>
        Stain = 1,

        /// <summary>
        /// Decal will blend the normal vector only.
        /// </summary>
        Normal = 2,

        /// <summary>
        /// Decal will apply the emissive light only.
        /// </summary>
        Emissive = 3,
    }

    /// <summary>
    /// Material input scene textures. Special inputs from the graphics pipeline.
    /// </summary>
    public enum MaterialSceneTextures
    {
        /// <summary>
        /// The scene color.
        /// </summary>
        SceneColor = 0,

        /// <summary>
        /// The scene depth.
        /// </summary>
        SceneDepth = 1,

        /// <summary>
        /// The material diffuse color.
        /// </summary>
        DiffuseColor = 2,

        /// <summary>
        /// The material specular color.
        /// </summary>
        SpecularColor = 3,

        /// <summary>
        /// The material world space normal.
        /// </summary>
        WorldNormal = 4,

        /// <summary>
        /// The ambient occlusion.
        /// </summary>
        AmbientOcclusion = 5,

        /// <summary>
        /// The material metalness value.
        /// </summary>
        Metalness = 6,

        /// <summary>
        /// The material roughness value.
        /// </summary>
        Roughness = 7,

        /// <summary>
        /// The material specular value.
        /// </summary>
        Specular = 8,

        /// <summary>
        /// The material color.
        /// </summary>
        BaseColor = 9,

        /// <summary>
        /// The material shading model.
        /// </summary>
        ShadingModel = 10,
    }

    /// <summary>
    /// Structure with basic information about the material surface.
    /// It describes how material is reacting on light and which graphical features of it requires to render.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct MaterialInfo
    {
        /// <summary>
        /// The domain.
        /// </summary>
        public MaterialDomain Domain;

        /// <summary>
        /// The blend mode.
        /// </summary>
        public MaterialBlendMode BlendMode;

        /// <summary>
        /// The shading mode.
        /// </summary>
        public MaterialShadingModel ShadingModel;

        /// <summary>
        /// The flags.
        /// </summary>
        public MaterialFlags Flags;

        /// <summary>
        /// The transparent lighting mode.
        /// </summary>
        public MaterialTransparentLighting TransparentLighting;

        /// <summary>
        /// The decal material blending mode.
        /// </summary>
        public MaterialDecalBlendingMode DecalBlendingMode;

        /// <summary>
        /// The post fx material rendering location.
        /// </summary>
        public MaterialPostFxLocation PostFxLocation;

        /// <summary>
        /// The mask threshold.
        /// </summary>
        public float MaskThreshold;

        /// <summary>
        /// The opacity threshold.
        /// </summary>
        public float OpacityThreshold;

        /// <summary>
        /// The tessellation mode.
        /// </summary>
        public TessellationMethod TessellationMode;

        /// <summary>
        /// The maximum tessellation factor (used only if material uses tessellation).
        /// </summary>
        public int MaxTessellationFactor;

        /// <summary>
        /// Creates the default <see cref="MaterialInfo"/>.
        /// </summary>
        /// <returns>The result.</returns>
        public static MaterialInfo CreateDefault()
        {
            return new MaterialInfo
            {
                Domain = MaterialDomain.Surface,
                BlendMode = MaterialBlendMode.Opaque,
                ShadingModel = MaterialShadingModel.Lit,
                Flags = MaterialFlags.None,
                TransparentLighting = MaterialTransparentLighting.None,
                DecalBlendingMode = MaterialDecalBlendingMode.Translucent,
                PostFxLocation = MaterialPostFxLocation.AfterPostProcessingPass,
                MaskThreshold = 0.3f,
                OpacityThreshold = 0.004f,
                TessellationMode = TessellationMethod.None,
                MaxTessellationFactor = 15,
            };
        }

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="a">The a.</param>
        /// <param name="b">The b.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator ==(MaterialInfo a, MaterialInfo b)
        {
            return a.Equals(b);
        }

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="a">The a.</param>
        /// <param name="b">The b.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator !=(MaterialInfo a, MaterialInfo b)
        {
            return !a.Equals(b);
        }

        /// <summary>
        /// Compares with the other material info and returns true if both values are equal.
        /// </summary>
        /// <param name="other">The other info.</param>
        /// <returns>True if both objects are equal, otherwise false.</returns>
        public bool Equals(MaterialInfo other)
        {
            return Domain == other.Domain
                   && BlendMode == other.BlendMode
                   && ShadingModel == other.ShadingModel
                   && Flags == other.Flags
                   && TransparentLighting == other.TransparentLighting
                   && DecalBlendingMode == other.DecalBlendingMode
                   && PostFxLocation == other.PostFxLocation
                   && Mathf.NearEqual(MaskThreshold, other.MaskThreshold)
                   && Mathf.NearEqual(OpacityThreshold, other.OpacityThreshold)
                   && TessellationMode == other.TessellationMode
                   && MaxTessellationFactor == other.MaxTessellationFactor;
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            return obj is MaterialInfo info && Equals(info);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (int)Domain;
                hashCode = (hashCode * 397) ^ (int)BlendMode;
                hashCode = (hashCode * 397) ^ (int)ShadingModel;
                hashCode = (hashCode * 397) ^ (int)Flags;
                hashCode = (hashCode * 397) ^ (int)TransparentLighting;
                hashCode = (hashCode * 397) ^ (int)PostFxLocation;
                hashCode = (hashCode * 397) ^ (int)DecalBlendingMode;
                hashCode = (hashCode * 397) ^ (int)(MaskThreshold * 1000.0f);
                hashCode = (hashCode * 397) ^ (int)(OpacityThreshold * 1000.0f);
                hashCode = (hashCode * 397) ^ (int)TessellationMode;
                hashCode = (hashCode * 397) ^ MaxTessellationFactor;
                return hashCode;
            }
        }
    }
}
