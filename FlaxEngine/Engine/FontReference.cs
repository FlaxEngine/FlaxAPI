// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

namespace FlaxEngine
{
    /// <summary>
    /// Font reference that defines the font asset and font size to use.
    /// </summary>
    public class FontReference
    {
        [NoSerialize]
        private FontAsset _font;

        [NoSerialize]
        private int _size;

        [NoSerialize]
        private Font _cachedFont;

        /// <summary>
        /// Initializes a new instance of the <see cref="FontReference"/> class.
        /// </summary>
        public FontReference()
        {
            _font = null;
            _size = 30;
            _cachedFont = null;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FontReference"/> struct.
        /// </summary>
        /// <param name="font">The font.</param>
        /// <param name="size">The font size.</param>
        public FontReference(FontAsset font, int size)
        {
            _font = font;
            _size = size;
            _cachedFont = null;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FontReference"/> struct.
        /// </summary>
        /// <param name="font">The font.</param>
        public FontReference(Font font)
        {
            _font = font?.Asset;
            _size = font?.Size ?? 30;
            _cachedFont = font;
        }

        /// <summary>
        /// The font asset.
        /// </summary>
        [EditorOrder(0), Tooltip("The font asset to use as characters source.")]
        public FontAsset Font
        {
            get => _font;
            set
            {
                if (_font != value)
                {
                    _font = value;

                    if (_cachedFont)
                        _cachedFont = null;
                }
            }
        }

        /// <summary>
        /// The size of the font characters.
        /// </summary>
        [EditorOrder(10), Limit(1, 500, 0.1f), Tooltip("The size of the font characters.")]
        public int Size
        {
            get => _size;
            set
            {
                if (_size != value)
                {
                    _size = value;

                    if (_cachedFont)
                        _cachedFont = null;
                }
            }
        }

        /// <summary>
        /// Gets the font object described by the structure.
        /// </summary>
        /// <returns>Th font or null if descriptor is invalid.</returns>
        public Font GetFont()
        {
            return _cachedFont ?? (_cachedFont = _font?.CreateFont(_size));
        }
    }
}
