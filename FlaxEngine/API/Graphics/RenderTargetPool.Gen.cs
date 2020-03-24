// This code was auto-generated. Do not modify it.

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace FlaxEngine
{
    /// <summary>
    /// Utility for pooling render target resources with reusing and sharing resources during rendering.
    /// </summary>
    [Tooltip("Utility for pooling render target resources with reusing and sharing resources during rendering.")]
    public static unsafe partial class RenderTargetPool
    {
        /// <summary>
        /// Gets a temporary render target.
        /// </summary>
        /// <param name="desc">The texture description.</param>
        /// <returns>The allocated render target or reused one.</returns>
        public static GPUTexture Get(ref GPUTextureDescription desc)
        {
            return Internal_Get(ref desc);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern GPUTexture Internal_Get(ref GPUTextureDescription desc);

        /// <summary>
        /// Releases a temporary render target.
        /// </summary>
        /// <param name="rt">The reference to temporary target to release.</param>
        public static void Release(GPUTexture rt)
        {
            Internal_Release(FlaxEngine.Object.GetUnmanagedPtr(rt));
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_Release(IntPtr rt);
    }
}
