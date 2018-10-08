// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System;

namespace FlaxEngine.Rendering
{
    /// <summary>
    /// Frame rendering flags used to switch between graphics features.
    /// </summary>
    [Flags]
    public enum ViewFlags : long
    {
        /// <summary>
        /// Nothing.
        /// </summary>
        None = 0,

        //DynamicActors = 1 << 0,

        /// <summary>
        /// Shows/hides Editor sprites
        /// </summary>
        EditorSprites = 1 << 1,

        /// <summary>
        /// Shows/hides reflections
        /// </summary>
        Reflections = 1 << 2,

        /// <summary>
        /// Shows/hides Screen Space Reflections
        /// </summary>
        SSR = 1 << 3,

        /// <summary>
        /// Shows/hides Ambient Occlusion effect
        /// </summary>
        AO = 1 << 4,

        /// <summary>
        /// Shows/hides Global Illumination effect
        /// </summary>
        GI = 1 << 5,

        /// <summary>
        /// Shows/hides directional lights
        /// </summary>
        DirectionalLights = 1 << 6,

        /// <summary>
        /// Shows/hides point lights
        /// </summary>
        PointLights = 1 << 7,

        /// <summary>
        /// Shows/hides spot lights
        /// </summary>
        SpotLights = 1 << 8,

        /// <summary>
        /// Shows/hides sky lights
        /// </summary>
        SkyLights = 1 << 9,

        /// <summary>
        /// Shows/hides shadows
        /// </summary>
        Shadows = 1 << 10,

        /// <summary>
        /// Shows/hides specular light rendering
        /// </summary>
        SpecularLight = 1 << 11,

        /// <summary>
        /// Shows/hides Anti-Aliasing
        /// </summary>
        AntiAliasing = 1 << 12,

        /// <summary>
        /// Shows/hides custom Post-Process effects
        /// </summary>
        CustomPostProcess = 1 << 13,

        /// <summary>
        /// Shows/hides bloom effect
        /// </summary>
        Bloom = 1 << 14,

        /// <summary>
        /// Shows/hides tone mapping effect
        /// </summary>
        ToneMapping = 1 << 15,

        /// <summary>
        /// Shows/hides eye adaptation effect
        /// </summary>
        EyeAdaptation = 1 << 16,

        /// <summary>
        /// Shows/hides camera artifacts
        /// </summary>
        CameraArtifacts = 1 << 17,

        /// <summary>
        /// Shows/hides lens flares
        /// </summary>
        LensFlares = 1 << 18,

        /// <summary>
        /// Shows/hides deferred decals.
        /// </summary>
        Decals = 1 << 19,

        /// <summary>
        /// Shows/hides depth of field effect
        /// </summary>
        DepthOfField = 1 << 20,

        /// <summary>
        /// Shows/hides physics debug shapes.
        /// </summary>
        PhysicsDebug = 1 << 21,

        /// <summary>
        /// Shows/hides fogging effects.
        /// </summary>
        Fog = 1 << 22,

        /// <summary>
        /// Shows/hides the motion blur effect.
        /// </summary>
        MotionBlur = 1 << 23,

        /// <summary>
        /// Default flags for Game
        /// </summary>
        DefaultGame = Reflections | DepthOfField | Fog | Decals | MotionBlur
                      | SSR | AO | GI | DirectionalLights | PointLights | SpotLights | SkyLights | Shadows | SpecularLight
                      | AntiAliasing | CustomPostProcess | Bloom | ToneMapping | EyeAdaptation | CameraArtifacts | LensFlares,

        /// <summary>
        /// Default flags for Editor
        /// </summary>
        DefaultEditor = Reflections | Fog | Decals
                        | SSR | AO | GI | DirectionalLights | PointLights | SpotLights | SkyLights | Shadows | SpecularLight
                        | AntiAliasing | CustomPostProcess | Bloom | ToneMapping | EyeAdaptation | CameraArtifacts | LensFlares | EditorSprites,

        /// <summary>
        /// Default flags for materials/models previews generating
        /// </summary>
        DefaultAssetPreview = Reflections | Decals
                              | GI | DirectionalLights | PointLights | SpotLights | SkyLights | SpecularLight
                              | AntiAliasing | Bloom | ToneMapping | EyeAdaptation | CameraArtifacts | LensFlares,
    }
}
