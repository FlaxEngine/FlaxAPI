// Flax Engine scripting API

using FlaxEngine;
using FlaxEngine.Rendering;

namespace FlaxEditor
{
    /// <summary>
    /// Asset type used by the editor to store generated asset previews.
    /// </summary>
    /// <seealso cref="FlaxEngine.Asset" />
    public sealed class PreviewsCache : Asset
    {
        /// <summary>
        /// The default asset previews icon size (both width and height since it's a square).
        /// </summary>
        public const int AssetIconSize = 64;

        /// <summary>
        /// The default assets previews atlas size
        /// </summary>
        public const int AssetIconsAtlasSize = 1024;

        /// <summary>
        /// The default assets previews atlas margin (between icons)
        /// </summary>
        public const int AssetIconsAtlasMargin = 4;

        /// <summary>
        /// The default format of previews atlas.
        /// </summary>
        public const PixelFormat AssetIconsAtlasFormat = PixelFormat.R8G8B8A8_UNorm;

        /// <summary>
        /// The previews cache asset type unique ID.
        /// </summary>
        public const int TypeID = 11;

        /// <summary>
        /// The asset type content domain.
        /// </summary>
        public const ContentDomain Domain = ContentDomain.Texture;
    }
}
