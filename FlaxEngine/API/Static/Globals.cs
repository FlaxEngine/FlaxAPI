// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System;

namespace FlaxEngine
{
    public static partial class Globals
    {
        #region Paths

        /// <summary>
        /// Gets the Main engine directory path.
        /// </summary>
        public static string StartupPath { get; private set; }

        /// <summary>
        /// Gets the Temporary folder path.
        /// </summary>
        public static string TemporaryFolder { get; private set; }

        /// <summary>
        /// Gets the project directory path.
        /// </summary>
        public static string ProjectFolder { get; private set; }

        /// <summary>
        /// Gets the Engine private data folder path.
        /// </summary>
        public static string EngineFolder { get; private set; }

        /// <summary>
        /// Gets the development files folder path.
        /// Valid only in development builds.
        /// </summary>
        public static string DevelopmentFolder { get; private set; }

        /// <summary>
        /// Gets the Editor private data folder path.
        /// Valid only in Editor.
        /// </summary>
        public static string EditorFolder { get; private set; }

        /// <summary>
        /// Gets the external tools folder path.
        /// </summary>
        public static string ToolsFolder { get; private set; }

        /// <summary>
        /// Gets the content directory path.
        /// </summary>
        public static string ContentFolder { get; private set; }

        /// <summary>
        /// Gets the game source code directory path.
        /// </summary>
        public static string SourceFolder { get; private set; }

        /// <summary>
        /// Gets the project specific cache folder path.
        /// </summary>
        public static string ProjectCacheFolder { get; private set; }

        /// <summary>
        /// Gets the Mono library folder path.
        /// </summary>
        public static string MonoPath { get; private set; }

        #endregion

        internal static void Init()
        {
            // Initialize paths
            var paths = GetPaths();
            if (paths == null || paths.Length != 11)
                throw new InvalidOperationException("Invalid Globals.");
            StartupPath = paths[0];
            TemporaryFolder = paths[1];
            ProjectFolder = paths[2];
            EngineFolder = paths[3];
            DevelopmentFolder = paths[4];
            EditorFolder = paths[5];
            ToolsFolder = paths[6];
            ContentFolder = paths[7];
            SourceFolder = paths[8];
            ProjectCacheFolder = paths[9];
            MonoPath = paths[10];
        }
    }
}
