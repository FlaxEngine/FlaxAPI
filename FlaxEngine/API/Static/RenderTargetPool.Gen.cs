// Copyright (c) 2012-2019 Wojciech Figat. All rights reserved.
// This code was generated by a tool. Changes to this file may cause
// incorrect behavior and will be lost if the code is regenerated.

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace FlaxEngine
{
    /// <summary>
    /// Utility for pooling render target resources with reusing and sharing resources during rendering.
    /// </summary>
    public static partial class RenderTargetPool
    {
        /// <summary>
        /// Gets a temporary render target.
        /// </summary>
        /// <param name="desc">The texture description.</param>
        /// <returns>The allocated render target or reused one.</returns>
#if UNIT_TEST_COMPILANT
        [Obsolete("Unit tests, don't support methods calls.")]
#endif
        [UnmanagedCall]
        public static GPUTexture Get(ref GPUTextureDescription desc)
        {
#if UNIT_TEST_COMPILANT
            throw new NotImplementedException("Unit tests, don't support methods calls. Only properties can be get or set.");
#else
            return Internal_Get(ref desc);
#endif
        }

        /// <summary>
        /// Releases a temporary render target.
        /// </summary>
        /// <param name="rt">The reference to temporary target to release.</param>
#if UNIT_TEST_COMPILANT
        [Obsolete("Unit tests, don't support methods calls.")]
#endif
        [UnmanagedCall]
        public static void Release(GPUTexture rt)
        {
#if UNIT_TEST_COMPILANT
            throw new NotImplementedException("Unit tests, don't support methods calls. Only properties can be get or set.");
#else
            Internal_Release(FlaxEngine.Object.GetUnmanagedPtr(rt));
#endif
        }

        #region Internal Calls

#if !UNIT_TEST_COMPILANT
        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern GPUTexture Internal_Get(ref GPUTextureDescription desc);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_Release(IntPtr rt);
#endif

        #endregion
    }
}
