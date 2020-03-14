// Copyright (c) 2012-2020 Wojciech Figat. All rights reserved.

namespace FlaxEditor.Content.Import
{
    /// <summary>
    /// The content item import request data container.
    /// </summary>
    public struct Request
    {
        /// <summary>
        /// The input item path (folder or file).
        /// </summary>
        public string InputPath;

        /// <summary>
        /// The output path (folder or file).
        /// </summary>
        public string OutputPath;

        /// <summary>
        /// Flag set to true for binary assets handled by the engine internally.
        /// </summary>
        public bool IsBinaryAsset;

        /// <summary>
        /// Flag used to skip showing import settings dialog to used. Can be used for importing assets from code by plugins.
        /// </summary>
        public bool SkipSettingsDialog;

        /// <summary>
        /// The custom settings object.
        /// </summary>
        public object Settings;
    }
}
