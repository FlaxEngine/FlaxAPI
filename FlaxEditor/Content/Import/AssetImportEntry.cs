// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

namespace FlaxEditor.Content.Import
{
    /// <summary>
    /// Asset import entry.
    /// </summary>
    public class AssetImportEntry : ImportFileEntry
    {
        /// <inheritdoc />
        public AssetImportEntry(string url, string resultUrl)
            : base(url, resultUrl)
        {
        }

        /// <inheritdoc />
        public override bool Import()
        {
            // Use engine backend
            return Editor.Import(SourceUrl, ResultUrl);
        }
    }
}
