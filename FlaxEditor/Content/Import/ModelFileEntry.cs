////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

namespace FlaxEditor.Content.Import
{
    /// <summary>
    /// Model asset import entry.
    /// </summary>
    /// <seealso cref="FlaxEditor.Content.Import.FileEntry" />
    public class ModelFileEntry : FileEntry
    {
        // TODO: add import model settings

        /// <summary>
        /// Initializes a new instance of the <see cref="ModelFileEntry"/> class.
        /// </summary>
        /// <param name="url">The source file url.</param>
        public ModelFileEntry(string url)
            : base(url)
        {
        }

        /// <inheritdoc />
        public override bool HasSettings => true;
    }
}
