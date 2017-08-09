////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.IO;

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
    }
}
