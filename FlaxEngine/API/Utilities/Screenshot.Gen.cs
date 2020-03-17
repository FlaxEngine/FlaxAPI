// This code was auto-generated. Do not modify it.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace FlaxEngine
{
    /// <summary>
    /// The utility class for capturing game screenshots.
    /// </summary>
    [Tooltip("The utility class for capturing game screenshots.")]
    public static unsafe partial class Screenshot
    {
        /// <summary>
        /// Captures the specified render target contents and saves it to the file.
        /// Remember that downloading data from the GPU may take a while so screenshot may be taken one or more frames later due to latency.
        /// </summary>
        /// <param name="target">The target render target to capture it's contents.</param>
        /// <param name="path">The custom file location. Use null or empty to use default one.</param>
        public static void Capture(GPUTexture target, string path = null)
        {
            Internal_Capture(FlaxEngine.Object.GetUnmanagedPtr(target), path);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_Capture(IntPtr target, string path);

        /// <summary>
        /// Captures the specified render task backbuffer contents and saves it to the file.
        /// Remember that downloading data from the GPU may take a while so screenshot may be taken one or more frames later due to latency.
        /// </summary>
        /// <param name="target">The target task to capture it's backbuffer.</param>
        /// <param name="path">The custom file location. Use null or empty to use default one.</param>
        public static void Capture(SceneRenderTask target = null, string path = null)
        {
            Internal_Capture1(FlaxEngine.Object.GetUnmanagedPtr(target), path);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_Capture1(IntPtr target, string path);

        /// <summary>
        /// Captures the main render task backbuffer contents and saves it to the file.
        /// Remember that downloading data from the GPU may take a while so screenshot may be taken one or more frames later due to latency.
        /// </summary>
        /// <param name="path">The custom file location. Use null or empty to use default one.</param>
        public static void Capture(string path = null)
        {
            Internal_Capture2(path);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_Capture2(string path);
    }
}
