// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

namespace FlaxEngine
{
    /// <summary>
    /// Basic types of the content assets base types.
    /// </summary>
    public enum ContentDomain
    {
        /// <summary>
        /// The invalid.
        /// </summary>
        Invalid,

        /// <summary>
        /// The texture.
        /// </summary>
        Texture,

        /// <summary>
        /// The cube texture.
        /// </summary>
        CubeTexture,

        /// <summary>
        /// The material.
        /// </summary>
        Material,

        /// <summary>
        /// The model.
        /// </summary>
        Model,

        /// <summary>
        /// The prefab.
        /// </summary>
        Prefab,

        /// <summary>
        /// The document.
        /// </summary>
        Document,

        /// <summary>
        /// The other.
        /// </summary>
        Other,

        /// <summary>
        /// The shader.
        /// </summary>
        Shader,

        /// <summary>
        /// The font.
        /// </summary>
        Font,

        /// <summary>
        /// The IES profile.
        /// </summary>
        IESProfile,

        /// <summary>
        /// The scene.
        /// </summary>
        Scene,

        /// <summary>
        /// The audio.
        /// </summary>
        Audio,

        /// <summary>
        /// The animation.
        /// </summary>
        Animation,

        /// <summary>
        /// The skeleton bones masking.
        /// </summary>
        SkeletonMask,
    }

    /// <summary>
    /// Assets objects base class.
    /// </summary>
    public partial class Asset
    {
        /// <inheritdoc />
        public override string ToString()
        {
            return $"{Path} ({GetType().Name})";
        }
    }
}
