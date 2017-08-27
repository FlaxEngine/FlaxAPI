////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Runtime.InteropServices;

namespace FlaxEngine.Rendering
{

    /// <summary>
    /// Material Domain Type
    /// </summary>
    public enum MaterialDomain : byte
    {
        /// <summary>
        /// The surface material.
        /// </summary>
        Surface = 0,

        /// <summary>
        /// The post process material.
        /// </summary>
        PostProcess = 1
    }

    /// <summary>
    /// Material Blending Mode
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
        /// The unlit material. Emissive channel is used as a output color. Can perform custom lighting operations or just glow.
        /// </summary>
        Unlit = 2,
    }

    /// <summary>
    /// Material Transparent Lighting Mode
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
    /// Material usage flags
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
        /// The flags.
        /// </summary>
        public MaterialFlags Flags;

        /// <summary>
        /// The transparent lighting mode.
        /// </summary>
        public MaterialTransparentLighting TransparentLighting;

        /// <summary>
        /// The post fx material rendering location.
        /// </summary>
        public MaterialPostFxLocation PostFxLocation;

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

        /// <inheritdoc />
        public bool Equals(MaterialInfo other)
        {
            return Domain == other.Domain
                   && BlendMode == other.BlendMode
                   && Flags == other.Flags
                   && TransparentLighting == other.TransparentLighting
                   && PostFxLocation == other.PostFxLocation;
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            return obj is MaterialInfo && Equals((MaterialInfo)obj);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (int)Domain;
                hashCode = (hashCode * 397) ^ (int)BlendMode;
                hashCode = (hashCode * 397) ^ (int)Flags;
                hashCode = (hashCode * 397) ^ (int)TransparentLighting;
                hashCode = (hashCode * 397) ^ (int)PostFxLocation;
                return hashCode;
            }
        }
    }
}
