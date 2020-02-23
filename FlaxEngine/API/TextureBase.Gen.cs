// This code was auto-generated. Do not modify it.

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace FlaxEngine
{
    /// <summary>
    /// Base class for <see cref="Texture"/>, <see cref="SpriteAtlas"/>, <see cref="IESProfile"/> and other assets that can contain texture data.
    /// </summary>
    /// <seealso cref="FlaxEngine.BinaryAsset" />
    [Tooltip("Base class for <see cref=\"Texture\"/>, <see cref=\"SpriteAtlas\"/>, <see cref=\"IESProfile\"/> and other assets that can contain texture data.")]
    public abstract partial class TextureBase : BinaryAsset
    {
        /// <inheritdoc />
        protected TextureBase() : base()
        {
        }

        /// <summary>
        /// Gets the native pointer to the underlying resource. It's a low-level platform-specific handle.
        /// </summary>
        [Tooltip("The native pointer to the underlying resource. It's a low-level platform-specific handle.")]
        public IntPtr NativePtr
        {
            get { return Internal_GetNativePtr(unmanagedPtr); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern IntPtr Internal_GetNativePtr(IntPtr obj);

        /// <summary>
        /// Gets the texture data format.
        /// </summary>
        [Tooltip("The texture data format.")]
        public PixelFormat Format
        {
            get { return Internal_Format(unmanagedPtr); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern PixelFormat Internal_Format(IntPtr obj);

        /// <summary>
        /// Gets the total width of the texture. Actual resident size may be different due to dynamic content streaming. Returns 0 if texture is not loaded.
        /// </summary>
        [Tooltip("The total width of the texture. Actual resident size may be different due to dynamic content streaming. Returns 0 if texture is not loaded.")]
        public int Width
        {
            get { return Internal_Width(unmanagedPtr); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern int Internal_Width(IntPtr obj);

        /// <summary>
        /// Gets the total height of the texture. Actual resident size may be different due to dynamic content streaming. Returns 0 if texture is not loaded.
        /// </summary>
        [Tooltip("The total height of the texture. Actual resident size may be different due to dynamic content streaming. Returns 0 if texture is not loaded.")]
        public int Height
        {
            get { return Internal_Height(unmanagedPtr); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern int Internal_Height(IntPtr obj);

        /// <summary>
        /// Gets the total size of the texture. Actual resident size may be different due to dynamic content streaming. Returns Vector2.Zero if texture is not loaded.
        /// </summary>
        [Tooltip("The total size of the texture. Actual resident size may be different due to dynamic content streaming. Returns Vector2::Zero if texture is not loaded.")]
        public Vector2 Size
        {
            get { Internal_Size(unmanagedPtr, out var resultAsRef); return resultAsRef; }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_Size(IntPtr obj, out Vector2 resultAsRef);

        /// <summary>
        /// Gets the total array size of the texture.
        /// </summary>
        [Tooltip("The total array size of the texture.")]
        public int ArraySize
        {
            get { return Internal_GetArraySize(unmanagedPtr); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern int Internal_GetArraySize(IntPtr obj);

        /// <summary>
        /// Gets the total mip levels count of the texture. Actual resident mipmaps count may be different due to dynamic content streaming.
        /// </summary>
        [Tooltip("The total mip levels count of the texture. Actual resident mipmaps count may be different due to dynamic content streaming.")]
        public int MipLevels
        {
            get { return Internal_GetMipLevels(unmanagedPtr); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern int Internal_GetMipLevels(IntPtr obj);

        /// <summary>
        /// Gets the current mip levels count of the texture that are on GPU ready to use.
        /// </summary>
        [Tooltip("The current mip levels count of the texture that are on GPU ready to use.")]
        public int ResidentMipLevels
        {
            get { return Internal_GetResidentMipLevels(unmanagedPtr); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern int Internal_GetResidentMipLevels(IntPtr obj);

        /// <summary>
        /// Gets the amount of the memory used by this resource. Exact value may differ due to memory alignment and resource allocation policy.
        /// </summary>
        [Tooltip("The amount of the memory used by this resource. Exact value may differ due to memory alignment and resource allocation policy.")]
        public ulong CurrentMemoryUsage
        {
            get { return Internal_GetCurrentMemoryUsage(unmanagedPtr); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern ulong Internal_GetCurrentMemoryUsage(IntPtr obj);

        /// <summary>
        /// Gets the total memory usage that texture may have in use (if loaded to the maximum quality). Exact value may differ due to memory alignment and resource allocation policy.
        /// </summary>
        [Tooltip("The total memory usage that texture may have in use (if loaded to the maximum quality). Exact value may differ due to memory alignment and resource allocation policy.")]
        public ulong TotalMemoryUsage
        {
            get { return Internal_GetTotalMemoryUsage(unmanagedPtr); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern ulong Internal_GetTotalMemoryUsage(IntPtr obj);

        /// <summary>
        /// Gets the mip data.
        /// </summary>
        /// <param name="mipIndex">The mip index (zero-based).</param>
        /// <param name="rowPitch">The data row pitch (in bytes).</param>
        /// <param name="slicePitch">The data slice pitch (in bytes).</param>
        /// <returns>The mip-map data or empty if failed to get it.</returns>
        public byte[] GetMipData(int mipIndex, out int rowPitch, out int slicePitch)
        {
            return Internal_GetMipData(unmanagedPtr, mipIndex, out rowPitch, out slicePitch);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern byte[] Internal_GetMipData(IntPtr obj, int mipIndex, out int rowPitch, out int slicePitch);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_Init(IntPtr obj, IntPtr ptr);
    }
}
