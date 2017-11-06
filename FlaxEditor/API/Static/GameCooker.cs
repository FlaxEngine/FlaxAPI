////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Runtime.CompilerServices;

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

    public static partial class GameCooker
    {
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
        /// Occurs when building event rises.
        /// </summary>
        public static event Action<EventType> OnEvent;

        internal static void Internal_OnEvent(EventType type)
        {
            OnEvent?.Invoke(type);
        }

        #region Internal Calls

#if !UNIT_TEST_COMPILANT
        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_Build(BuildPlatform platform, BuildOptions options, string outputPath, string[] defines);
#endif

        #endregion
    }
}
