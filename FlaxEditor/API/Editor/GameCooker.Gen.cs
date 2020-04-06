// This code was auto-generated. Do not modify it.

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using FlaxEngine;

namespace FlaxEditor
{
    /// <summary>
    /// Game building service. Processes project files and outputs build game for a target platform.
    /// </summary>
    [Tooltip("Game building service. Processes project files and outputs build game for a target platform.")]
    public static unsafe partial class GameCooker
    {
        /// <summary>
        /// Determines whether game building is running.
        /// </summary>
        [Tooltip("Determines whether game building is running.")]
        public static bool IsRunning
        {
            get { return Internal_IsRunning(); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_IsRunning();

        /// <summary>
        /// Determines whether building cancel has been requested.
        /// </summary>
        [Tooltip("Determines whether building cancel has been requested.")]
        public static bool IsCancelRequested
        {
            get { return Internal_IsCancelRequested(); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_IsCancelRequested();

        /// <summary>
        /// Starts building game for the specified platform.
        /// </summary>
        /// <param name="platform">The target platform.</param>
        /// <param name="configuration">The build configuration.</param>
        /// <param name="outputPath">The output path (output directory).</param>
        /// <param name="options">The build options.</param>
        /// <param name="defines">Scripts compilation define symbols (macros).</param>
        public static void Build(BuildPlatform platform, BuildConfiguration configuration, string outputPath, BuildOptions options, string[] defines)
        {
            Internal_Build(platform, configuration, outputPath, options, defines);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_Build(BuildPlatform platform, BuildConfiguration configuration, string outputPath, BuildOptions options, string[] defines);

        /// <summary>
        /// Sends a cancel event to the game building service.
        /// </summary>
        /// <param name="waitForEnd">If set to <c>true</c> wait for the stopped building end.</param>
        public static void Cancel(bool waitForEnd = false)
        {
            Internal_Cancel(waitForEnd);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_Cancel(bool waitForEnd);
    }
}
