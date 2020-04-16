// This code was auto-generated. Do not modify it.

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace FlaxEngine
{
    /// <summary>
    /// Global engine variables container.
    /// </summary>
    [Tooltip("Global engine variables container.")]
    public static unsafe partial class Globals
    {
        /// <summary>
        /// Main engine directory path.
        /// </summary>
        [Tooltip("Main engine directory path.")]
        public static string StartupPath
        {
            get { return Internal_GetStartupPath(); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern string Internal_GetStartupPath();

        /// <summary>
        /// Temporary folder path.
        /// </summary>
        [Tooltip("Temporary folder path.")]
        public static string TemporaryFolder
        {
            get { return Internal_GetTemporaryFolder(); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern string Internal_GetTemporaryFolder();

        /// <summary>
        /// Directory that contains project
        /// </summary>
        [Tooltip("Directory that contains project")]
        public static string ProjectFolder
        {
            get { return Internal_GetProjectFolder(); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern string Internal_GetProjectFolder();

        /// <summary>
        /// Folder with Engine's private data
        /// </summary>
        [Tooltip("Folder with Engine's private data")]
        public static string EngineFolder
        {
            get { return Internal_GetEngineFolder(); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern string Internal_GetEngineFolder();

        /// <summary>
        /// The product local data directory.
        /// </summary>
        [Tooltip("The product local data directory.")]
        public static string ProductLocalFolder
        {
            get { return Internal_GetProductLocalFolder(); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern string Internal_GetProductLocalFolder();

        /// <summary>
        /// The engine executable file location.
        /// </summary>
        [Tooltip("The engine executable file location.")]
        public static string BinFolder
        {
            get { return Internal_GetBinFolder(); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern string Internal_GetBinFolder();

        /// <summary>
        /// Folder with Editor files
        /// </summary>
        [Tooltip("Folder with Editor files")]
        public static string EditorFolder
        {
            get { return Internal_GetEditorFolder(); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern string Internal_GetEditorFolder();

        /// <summary>
        /// Game source code directory path
        /// </summary>
        [Tooltip("Game source code directory path")]
        public static string SourceFolder
        {
            get { return Internal_GetSourceFolder(); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern string Internal_GetSourceFolder();

        /// <summary>
        /// Content directory path
        /// </summary>
        [Tooltip("Content directory path")]
        public static string ContentFolder
        {
            get { return Internal_GetContentFolder(); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern string Internal_GetContentFolder();

        /// <summary>
        /// Project specific cache folder path
        /// </summary>
        [Tooltip("Project specific cache folder path")]
        public static string ProjectCacheFolder
        {
            get { return Internal_GetProjectCacheFolder(); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern string Internal_GetProjectCacheFolder();

        /// <summary>
        /// Mono library folder path
        /// </summary>
        [Tooltip("Mono library folder path")]
        public static string MonoPath
        {
            get { return Internal_GetMonoPath(); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern string Internal_GetMonoPath();

        /// <summary>
        /// Main Engine thread id
        /// </summary>
        [Tooltip("Main Engine thread id")]
        public static ulong MainThreadID
        {
            get { return Internal_GetMainThreadID(); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern ulong Internal_GetMainThreadID();

        /// <summary>
        /// The full engine version.
        /// </summary>
        [Tooltip("The full engine version.")]
        public static string EngineVersion
        {
            get { return Internal_GetEngineVersion(); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern string Internal_GetEngineVersion();

        /// <summary>
        /// The engine build version.
        /// </summary>
        [Tooltip("The engine build version.")]
        public static int EngineBuildNumber
        {
            get { return Internal_GetEngineBuildNumber(); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern int Internal_GetEngineBuildNumber();

        /// <summary>
        /// The short name of the product (can be `Flax Editor` or name of the game e.g. `My Space Shooter`).
        /// </summary>
        [Tooltip("The short name of the product (can be `Flax Editor` or name of the game e.g. `My Space Shooter`).")]
        public static string ProductName
        {
            get { return Internal_GetProductName(); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern string Internal_GetProductName();

        /// <summary>
        /// The company name (short name used for app data directory).
        /// </summary>
        [Tooltip("The company name (short name used for app data directory).")]
        public static string CompanyName
        {
            get { return Internal_GetCompanyName(); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern string Internal_GetCompanyName();
    }
}
