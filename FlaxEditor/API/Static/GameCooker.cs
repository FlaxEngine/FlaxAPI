// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using FlaxEngine;

namespace FlaxEditor
{
    /// <summary>
    /// Game building options. Used as flags.
    /// </summary>
    [Flags]
    public enum BuildOptions
    {
        /// <summary>
        /// No special options declared.
        /// </summary>
        None = 0,

        /// <summary>
        /// The debug build mode (opposite to release mode).
        /// </summary>
        Debug = 1,

        /// <summary>
        /// Shows the output directory folder on building end.
        /// </summary>
        ShowOutput = 1 << 1,
    }

    /// <summary>
    /// Game build target platform.
    /// </summary>
    public enum BuildPlatform
    {
        /// <summary>
        /// Windows x86 (32-bit architecture)
        /// </summary>
        Windows32 = 1,

        /// <summary>
        /// Windows x64 (64-bit architecture)
        /// </summary>
        Windows64 = 2,

        /// <summary>
        /// Universal Windows Platform (UWP) (x86 architecture)
        /// </summary>
        WindowsStoreX86 = 3,

        /// <summary>
        /// Universal Windows Platform (UWP) (x64 architecture)
        /// </summary>
        WindowsStoreX64 = 4,

        /// <summary>
        /// Xbox One (x64 architecture)
        /// </summary>
        XboxOne = 5,
    }

    /// <summary>
    /// The build mode.
    /// </summary>
    public enum BuildMode
    {
        /// <summary>
        /// The release configuration.
        /// </summary>
        Release = 0,

        /// <summary>
        /// The debug configuration.
        /// </summary>
        Debug = 1,
    }

    public static partial class GameCooker
    {
        /// <summary>
        /// Build options data.
        /// </summary>
        public struct Options
        {
            /// <summary>
            /// The platform.
            /// </summary>
            public BuildPlatform Platform;

            /// <summary>
            /// The options.
            /// </summary>
            public BuildOptions Flags;

            /// <summary>
            /// The output path (normalized, absolute).
            /// </summary>
            public string OutputPath;

            /// <summary>
            /// The custom defines.
            /// </summary>
            public string[] Defines;
        }

        private static Options _lastOptions;

        /// <summary>
        /// Starts building game for the specified platform.
        /// </summary>
        /// <param name="platform">The target platform.</param>
        /// <param name="options">The build options.</param>
        /// <param name="outputPath">The output path (output directory).</param>
        /// <param name="defines">Scripts compilation define symbols (macros).</param>
        public static void Build(BuildPlatform platform, BuildOptions options, string outputPath, string[] defines = null)
        {
            if (IsRunning)
                throw new InvalidOperationException("Cannot start build while already running.");
            if (string.IsNullOrEmpty(outputPath))
                throw new ArgumentNullException(nameof(outputPath));

            // Cache options (reuse them for build step events)
            _lastOptions.Platform = platform;
            _lastOptions.Flags = options;
            _lastOptions.OutputPath = StringUtils.ConvertRelativePathToAbsolute(Globals.ProjectFolder, StringUtils.NormalizePath(outputPath));
            _lastOptions.Defines = defines;

            Internal_Build(platform, options, outputPath, defines);
        }

        /// <summary>
        /// Building event type.
        /// </summary>
        public enum EventType
        {
            /// <summary>
            /// The build started.
            /// </summary>
            BuildStarted = 0,

            /// <summary>
            /// The build failed.
            /// </summary>
            BuildFailed = 1,

            /// <summary>
            /// The build done.
            /// </summary>
            BuildDone = 2,
        }

        /// <summary>
        /// Game building event delegate.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="options">The build options (read only).</param>
        public delegate void BuildEventDelegate(EventType type, ref Options options);

        /// <summary>
        /// Game building progress reporting delegate type.
        /// </summary>
        /// <param name="info">The information text.</param>
        /// <param name="totalProgress">The total progress percentage (normalized to 0-1).</param>
        public delegate void BuildProgressDelegate(string info, float totalProgress);

        /// <summary>
        /// Occurs when building event rises.
        /// </summary>
        public static event BuildEventDelegate Event;

        /// <summary>
        /// Occurs when building game progress fires.
        /// </summary>
        public static event BuildProgressDelegate Progress;

        /// <summary>
        /// Gets the type of the platform from the game build platform type.
        /// </summary>
        /// <param name="buildPlatform">The build platform.</param>
        /// <returns>The run-type platform type.</returns>
        public static PlatformType GetPlatformType(BuildPlatform buildPlatform)
        {
            switch (buildPlatform)
            {
            case BuildPlatform.Windows32:
            case BuildPlatform.Windows64: return PlatformType.Windows;
            case BuildPlatform.WindowsStoreX86:
            case BuildPlatform.WindowsStoreX64: return PlatformType.WindowsStore;
            case BuildPlatform.XboxOne: return PlatformType.XboxOne;
            default: throw new ArgumentOutOfRangeException(nameof(buildPlatform), buildPlatform, null);
            }
        }

        internal static void Internal_OnEvent(EventType type)
        {
            Event?.Invoke(type, ref _lastOptions);
        }

        internal static void Internal_OnProgress(string info, float totalProgress)
        {
            Progress?.Invoke(info, totalProgress);
        }

        internal static void Internal_CanDeployPlugin(BuildPlatform buildPlatform, Assembly assembly, out bool result)
        {
            // Find plugin (game only plugins are used to deploy)
            var plugin = PluginManager.GamePlugins.FirstOrDefault(x => x.GetType().Assembly == assembly);
            if (plugin == null)
            {
                Debug.Write(LogType.Log, "No loaded game plugins from assembly " + assembly.FullName);
                result = true;
                return;
            }
            var desc = plugin.Description;

            // Check if plugins supports the given platform
            var platform = GetPlatformType(buildPlatform);
            if (desc.SupportedPlatforms != null && !desc.SupportedPlatforms.Contains(platform))
            {
                Debug.Write(LogType.Log, "Skip game plugin from assembly " + assembly.FullName);
                result = false;
                return;
            }

            Debug.Write(LogType.Log, "Use game plugin from assembly " + assembly.FullName);
            result = true;
        }

        #region Internal Calls

#if !UNIT_TEST_COMPILANT
        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_Build(BuildPlatform platform, BuildOptions options, string outputPath, string[] defines);
#endif

        #endregion
    }
}
