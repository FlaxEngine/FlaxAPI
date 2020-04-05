// This code was auto-generated. Do not modify it.

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace FlaxEngine
{
    /// <summary>
    /// The main engine class.
    /// </summary>
    [Tooltip("The main engine class.")]
    public static unsafe partial class Engine
    {
        /// <summary>
        /// Gets the amount of frames rendered during last second known as Frames Per Second. User scripts updates or fixed updates for physics may run at a different frequency than scene rendering. Use this property to get an accurate amount of frames rendered during the last second.
        /// </summary>
        [Tooltip("The amount of frames rendered during last second known as Frames Per Second. User scripts updates or fixed updates for physics may run at a different frequency than scene rendering. Use this property to get an accurate amount of frames rendered during the last second.")]
        public static int FramesPerSecond
        {
            get { return Internal_GetFramesPerSecond(); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern int Internal_GetFramesPerSecond();

        /// <summary>
        /// Gets the application command line arguments.
        /// </summary>
        [Tooltip("The application command line arguments.")]
        public static string CommandLine
        {
            get { return Internal_GetCommandLine(); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern string Internal_GetCommandLine();

        /// <summary>
        /// Requests normal engine exit.
        /// </summary>
        /// <param name="exitCode">The exit code.</param>
        public static void RequestExit(int exitCode = 0)
        {
            Internal_RequestExit(exitCode);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_RequestExit(int exitCode);

        /// <summary>
        /// Gets the custom game settings asset referenced by the given key.
        /// </summary>
        /// <param name="key">The settings key.</param>
        /// <returns>The returned asset. Returns null if key is invalid, cannot load asset or data is missing.</returns>
        public static JsonAsset GetCustomSettings(string key)
        {
            return Internal_GetCustomSettings(key);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern JsonAsset Internal_GetCustomSettings(string key);
    }
}
