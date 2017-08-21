////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using FlaxEngine;

namespace FlaxEditor.Content.Import
{
    /// <summary>
    /// Texture format types.
    /// </summary>
    public enum TextureFormatType : byte
    {
        Unknown = 0,
        ColorRGB = 1,
        ColorRGBA = 2,
        NormalMap = 3,
        GrayScale = 4,
        HdrRGBA = 5,
        HdrRGB = 6
    }

    /// <summary>
    /// Proxy object to present texture import settings in <see cref="ImportFilesDialog"/>.
    /// </summary>
    public class TextureImportSettings
    {
        /// <summary>
        /// A custom version of <see cref="TextureFormatType"/> for GUI.
        /// </summary>
        public enum CustomTextureFormatType
        {
            ColorRGB = 1,
            ColorRGBA = 2,
            NormalMap = 3,
            GrayScale = 4,
            HdrRGBA = 5,
            HdrRGB = 6
        }

        /// <summary>
        /// A custom set of max texture import sizes.
        /// </summary>
        public enum CustomMaxSizeType
        {
            _32 = 32,
            _64 = 64,
            _128 = 128,
            _256 = 256,
            _512 = 512,
            _1024 = 1024,
            _2048 = 2048,
            _4096 = 4096,
            _8192 = 8192
        }

        /// <summary>
        /// Texture format type
        /// </summary>
        [EditorOrder(0), Tooltip("Texture import format type")]
        public CustomTextureFormatType Type { get; set; } = CustomTextureFormatType.ColorRGB;

        /// <summary>
        /// True if texture should be improted as a texture atlas resource
        /// </summary>
        [EditorOrder(10), Tooltip("True if texture should be improted as a texture atlas (with sprites)")]
        public bool IsAtlas { get; set; }

        /// <summary>
        /// True if disable dynamic texture streaming
        /// </summary>
        [EditorOrder(20), Tooltip("True if disable dynamic texture streaming")]
        public bool NeverStream { get; set; }

        /// <summary>
        /// Enables/disables texture data compression.
        /// </summary>
        [EditorOrder(30), Tooltip("True if comrpess texture data")]
        public bool Compress { get; set; } = true;

        /// <summary>
        /// True if texture channels have independent data
        /// </summary>
        [EditorOrder(40), Tooltip("True if texture channels have independent data (for compression methods)")]
        public bool IndependentChannels { get; set; }

        /// <summary>
        /// True if texture contains sRGB format and engine should keep that data format
        /// </summary>
        [EditorOrder(50), Tooltip("True if texture contains colors in sRGB format data")]
        public bool IsSRGB { get; set; }

        /// <summary>
        /// True if generate mip maps chain for the texture.
        /// </summary>
        [EditorOrder(60), Tooltip("True if generate mip maps chain for the texture")]
        public bool GenerateMipMaps { get; set; }  = true;

        /// <summary>
        /// The import texture scale.
        /// </summary>
        [EditorOrder(70), Tooltip("Texture scale. Default is 1.")]
        public float Scale { get; set; } = 1.0f;

        /// <summary>
        /// Maximum size of the texture (for both width and height).
        /// Higher resolution textures will be resized during importing process.
        /// </summary>
        [EditorOrder(80), Tooltip("Maximum texture size (will be resized if need to)")]
        public CustomMaxSizeType MaxSize { get; set; } = CustomMaxSizeType._8192;
    }

    /// <summary>
    /// Texture asset import entry.
    /// </summary>
    /// <seealso cref="FlaxEditor.Content.Import.AssetFileEntry" />
    public class TextureFileEntry : AssetFileEntry
    {
        private TextureImportSettings _settings = new TextureImportSettings();

        /// <summary>
        /// Initializes a new instance of the <see cref="TextureFileEntry"/> class.
        /// </summary>
        /// <param name="url">The source file url.</param>
        /// <param name="resultUrl">The result file url.</param>
        public TextureFileEntry(string url, string resultUrl)
            : base(url, resultUrl)
        {
            // TODO: prepare import options based on file name
        }

        /// <inheritdoc />
        public override bool HasSettings => true;

        /// <inheritdoc />
        public override object Settings => _settings;
    }
}
