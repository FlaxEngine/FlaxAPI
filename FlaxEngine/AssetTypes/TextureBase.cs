// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System;
using System.Runtime.CompilerServices;
using FlaxEngine.Rendering;

namespace FlaxEngine
{
    /// <summary>
    /// Base class for <see cref="Texture"/>, <see cref="SpriteAtlas"/>, <see cref="IESProfile"/> and other assets that contain texture.
    /// </summary>
    /// <seealso cref="FlaxEngine.BinaryAsset" />
    public abstract class TextureBase : BinaryAsset
    {
        /// <summary>
        /// Gets the texture data format.
        /// </summary>
        [UnmanagedCall]
        public PixelFormat Format
        {
#if UNIT_TEST_COMPILANT
			get; set;
#else
            get { return Internal_GetFormat(unmanagedPtr); }
#endif
        }

        /// <summary>
        /// Gets the total width of the texture. Actual resident size may be different due to dynamic content streaming. Returns 0 if texture is not loaded.
        /// </summary>
        [UnmanagedCall]
        public int Width
        {
#if UNIT_TEST_COMPILANT
			get; set;
#else
            get { return Internal_GetWidth(unmanagedPtr); }
#endif
        }

        /// <summary>
        /// Gets the total height of the texture. Actual resident size may be different due to dynamic content streaming. Returns 0 if texture is not loaded.
        /// </summary>
        [UnmanagedCall]
        public int Height
        {
#if UNIT_TEST_COMPILANT
			get; set;
#else
            get { return Internal_GetHeight(unmanagedPtr); }
#endif
        }

        /// <summary>
        /// Gets the total size of the texture. Actual resident size may be different due to dynamic content streaming.
        /// </summary>
        [UnmanagedCall]
        public Vector2 Size
        {
#if UNIT_TEST_COMPILANT
			get; set;
#else
            get
            {
                Vector2 resultAsRef;
                Internal_GetSize(unmanagedPtr, out resultAsRef);
                return resultAsRef;
            }
#endif
        }

        /// <summary>
        /// Gets the total array size of the texture.
        /// </summary>
        [UnmanagedCall]
        public int ArraySize
        {
#if UNIT_TEST_COMPILANT
			get; set;
#else
            get { return Internal_GetArraySize(unmanagedPtr); }
#endif
        }

        /// <summary>
        /// Gets the total mip levels count of the texture. Actual resident mipmaps count may be different due to dynamic content streaming.
        /// </summary>
        [UnmanagedCall]
        public int MipLevels
        {
#if UNIT_TEST_COMPILANT
			get; set;
#else
            get { return Internal_GetMipLevels(unmanagedPtr); }
#endif
        }

        /// <summary>
        /// Gets the current mip levels count of the texture that are on GPU ready to use.
        /// </summary>
        [UnmanagedCall]
        public int ResidentMipLevels
        {
#if UNIT_TEST_COMPILANT
			get; set;
#else
            get { return Internal_GetResidentMipLevels(unmanagedPtr); }
#endif
        }

        /// <summary>
        /// Gets the amount of the memory used by this resource.
        /// Exact value may differ due to memory alignment and resource allocation policy.
        /// </summary>
        [UnmanagedCall]
        public ulong CurrentMemoryUsage
        {
#if UNIT_TEST_COMPILANT
			get; set;
#else
            get { return Internal_GetCurrentMemoryUsage(unmanagedPtr); }
#endif
        }

        /// <summary>
        /// Gets the total memory usage that texture may have in use (if loaded to the maximum quality).
        /// Exact value may differ due to memory alignment and resource allocation policy.
        /// </summary>
        [UnmanagedCall]
        public ulong TotalMemoryUsage
        {
#if UNIT_TEST_COMPILANT
			get; set;
#else
            get { return Internal_GetTotalMemoryUsage(unmanagedPtr); }
#endif
        }

        #region Internal Calls

#if !UNIT_TEST_COMPILANT
        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern PixelFormat Internal_GetFormat(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern int Internal_GetWidth(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern int Internal_GetHeight(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_GetSize(IntPtr obj, out Vector2 resultAsRef);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern int Internal_GetArraySize(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern int Internal_GetMipLevels(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern int Internal_GetResidentMipLevels(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern ulong Internal_GetCurrentMemoryUsage(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern ulong Internal_GetTotalMemoryUsage(IntPtr obj);
#endif

        #endregion
    }
}
