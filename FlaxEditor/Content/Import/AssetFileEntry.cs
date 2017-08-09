////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

namespace FlaxEditor.Content.Import
{
    /// <summary>
    /// Asset import entry.
    /// </summary>
    public class AssetFileEntry : FileEntry
    {
        /// <inheritdoc />
        public AssetFileEntry(string url, string resultUrl)
            : base(url, resultUrl)
        {
        }

        /// <inheritdoc />
        public override bool Import()
        {
            // Use engine backend
            return Editor.Import(Url, ResultUrl);
        }
    }
}
