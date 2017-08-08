////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

namespace FlaxEditor.Content.Import
{
    /// <summary>
    /// Texture asset import entry.
    /// </summary>
    /// <seealso cref="FlaxEditor.Content.Import.FileEntry" />
    public class TextureFileEntry : FileEntry
    {
        // TODO: add import texture settings

        /// <summary>
        /// Initializes a new instance of the <see cref="TextureFileEntry"/> class.
        /// </summary>
        /// <param name="url">The source file url.</param>
        public TextureFileEntry(string url)
            : base(url)
        {
            // TODO: prepare import options based on file name
        }

        /// <inheritdoc />
        public override bool HasSettings => true;
    }
}
