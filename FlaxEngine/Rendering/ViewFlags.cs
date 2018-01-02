////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2018 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

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
        /// Shows/hides dynamic actors
        /// </summary>
        DynamicActors = 1 << 0,

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
        /// Shows/hides tone mapping effct
        /// </summary>
        ToneMapping = 1 << 15,

        /// <summary>
        /// Shows/hides eye adaptation effct
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
        /// Shows/hides Constructive Solid Geometry
        /// </summary>
        CSG = 1 << 19,

        /// <summary>
        /// Shows/hides deph of field effect
        /// </summary>
        DepthOfField = 1 << 20,

        /// <summary>
        /// Shows/hides physics debug shapes.
        /// </summary>
        PhysicsDebug = 1 << 21,




        /// <summary>
        /// Default flags for Game
        /// </summary>
        DefaultGame = DynamicActors | Reflections | CSG | DepthOfField
                      | SSR | AO | GI | DirectionalLights | PointLights | SpotLights | SkyLights | Shadows | SpecularLight
                      | AntiAliasing | CustomPostProcess | Bloom | ToneMapping | EyeAdaptation | CameraArtifacts | LensFlares,

        /// <summary>
        /// Default flags for Editor
        /// </summary>
        DefaultEditor = DynamicActors | Reflections | CSG
                        | SSR | AO | GI | DirectionalLights | PointLights | SpotLights | SkyLights | Shadows | SpecularLight
                        | AntiAliasing | CustomPostProcess | Bloom | ToneMapping | EyeAdaptation | CameraArtifacts | LensFlares | EditorSprites,

        /// <summary>
        /// Default flags for Material Previews generating
        /// </summary>
        DefaultMaterialPreview = DynamicActors | Reflections
                                 | GI | DirectionalLights | PointLights | SpotLights | SkyLights | SpecularLight
                                 | AntiAliasing | Bloom | ToneMapping | EyeAdaptation | CameraArtifacts | LensFlares,

        /// <summary>
        /// Default flags for Material Previews generating
        /// </summary>
        DefaultModelPreview = DynamicActors | Reflections
                              | GI | DirectionalLights | PointLights | SpotLights | SkyLights | SpecularLight
                              | AntiAliasing | Bloom | ToneMapping | EyeAdaptation | CameraArtifacts | LensFlares,
    }
}
