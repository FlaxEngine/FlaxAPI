// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

namespace FlaxEditor.Content
{
    /// <summary>
    /// Content item for the auxiliary files.
    /// </summary>
    /// <seealso cref="FlaxEditor.Content.ContentItem" />
    public class FileItem : ContentItem
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FileItem"/> class.
        /// </summary>
        /// <param name="path">The path to the file.</param>
        public FileItem(string path)
        : base(path)
        {
        }

        /// <inheritdoc />
        public override ContentItemType ItemType => ContentItemType.Other;

        /// <inheritdoc />
        public override string DefaultThumbnailName => "Document64";
    }
}
