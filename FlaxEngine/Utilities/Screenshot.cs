// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System;
using System.Runtime.CompilerServices;
using FlaxEngine.Rendering;

namespace FlaxEngine.Utilities
{
    /// <summary>
    /// Utility class used to capture screenshots.
    /// </summary>
    public static class Screenshot
    {
        /// <summary>
        /// Captures the specified render target contents and saves it to the file.
        /// Remember that downloading data from the GPU may take a while so screenshot may be taken one or more frames later due to latency.
        /// </summary>
        /// <param name="target">The target render target to capture it's contents.</param>
        /// <param name="path">The custom file location. Use null or empty to use default one.</param>
        [UnmanagedCall]
        public static void Capture(RenderTarget target, string path = null)
        {
            if (target == null)
                throw new ArgumentNullException();

            Internal_Capture1(target.unmanagedPtr, path);
        }

        /// <summary>
        /// Captures the specified render task backbuffer contents and saves it to the file.
        /// Remember that downloading data from the GPU may take a while so screenshot may be taken one or more frames later due to latency.
        /// </summary>
        /// <param name="target">The target task to capture it's backbuffer.</param>
        /// <param name="path">The custom file location. Use null or empty to use default one.</param>
        [UnmanagedCall]
        public static void Capture(SceneRenderTask target = null, string path = null)
        {
            Internal_Capture2(Object.GetUnmanagedPtr(target), path);
        }

        #region Internal Calls

#if !UNIT_TEST_COMPILANT
        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_Capture1(IntPtr targetObj, string pathObj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_Capture2(IntPtr targetObj, string pathObj);
#endif

        #endregion
    }
}
