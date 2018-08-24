// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
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
        /// The maximum size for the texture resources (supported by engine, the target platform can be lower capabilities).
        /// </summary>
        public const int MaxTextureSize = 8192;

        /// <summary>
        /// The maximum amount of the mip levels for the texture resources (supported by engine, the target platform can be lower capabilities).
        /// </summary>
        public const int MaxMipLevels = 14;

        /// <summary>
        /// The maximum array size for the texture resources (supported by engine, the target platform can be lower capabilities).
        /// </summary>
        public const int MaxArraySize = 512;

        /// <summary>
        /// Gets the native pointer to the underlying resource. It's a low-level platform-specific handle.
        /// </summary>
        [UnmanagedCall]
        public IntPtr NativePtr
        {
#if UNIT_TEST_COMPILANT
			get; set;
#else
            get { return Internal_GetNativePtr(unmanagedPtr); }
#endif
        }

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

        /// <summary>
        /// Gets the texture mip level data (raw bytes).
        /// </summary>
        /// <param name="mipIndex">The zero-based index of the mip map to get (zero is the top most mip map). See <see cref="MipLevels"/>.</param>
        /// <param name="rowPitch">The data row pitch (in bytes).</param>
        /// <param name="slicePitch">The data slice pitch (in bytes).</param>
        /// <returns>The mip map raw bytes data or null if loading failed.</returns>
#if UNIT_TEST_COMPILANT
        [Obsolete("Unit tests, don't support methods calls.")]
#endif
        [UnmanagedCall]
        public byte[] GetMipData(int mipIndex, out int rowPitch, out int slicePitch)
        {
#if UNIT_TEST_COMPILANT
            throw new NotImplementedException("Unit tests, don't support methods calls. Only properties can be get or set.");
#else
            return Internal_GetMipData(unmanagedPtr, mipIndex, out rowPitch, out slicePitch);
#endif
        }

        /// <summary>
        /// The texture data initialization container.
        /// </summary>
        public struct InitData
        {
            /// <summary>
            /// The format of the pixels.
            /// </summary>
            public PixelFormat Format;

            /// <summary>
            /// The width (in pixels).
            /// </summary>
            public int Width;

            /// <summary>
            /// The height (in pixels).
            /// </summary>
            public int Height;

            /// <summary>
            /// The array size (slices count).
            /// </summary>
            public int ArraySize;

            /// <summary>
            /// The mips levels data.
            /// </summary>
            public MipData[] Mips;

            /// <summary>
            /// Returns true if init data is valid.
            /// </summary>
            public bool IsValid => Format != PixelFormat.Unknown &&
                                   Mathf.IsInRange(Width, 1, MaxTextureSize) &&
                                   Mathf.IsInRange(Height, 1, MaxTextureSize) &&
                                   Mathf.IsInRange(ArraySize, 1, MaxArraySize) &&
                                   Mips != null &&
                                   Mathf.IsInRange(Mips.Length, 1, MaxMipLevels);

            /// <summary>
            /// The mip data container.
            /// </summary>
            public struct MipData
            {
                /// <summary>
                /// The texture data. Use <see cref="RowPitch"/> and <see cref="SlicePitch"/> to define the storage format.
                /// </summary>
                public byte[] Data;

                /// <summary>
                /// The data container image row pitch (in bytes).
                /// </summary>
                public int RowPitch;

                /// <summary>
                /// The data container image slice pitch (in bytes).
                /// </summary>
                public int SlicePitch;
            }
        }

        /// <summary>
        /// Initializes the texture storage container with the given data. Valid only for virtual assets. Can be used in both Editor and at runtime in a build game.
        /// It does not perform any data streaming or uploading to the GPU. Only the texture resource is being initialized and the data is copied to be streamed later.
        /// </summary>
        /// <remarks>
        /// Can be used only for virtual assets (see <see cref="Asset.IsVirtual"/> and <see cref="Content.CreateVirtualAsset{T}"/>).
        /// </remarks>
        /// <param name="initData">The texture init data.</param>
        public unsafe void Init(InitData initData)
        {
            // Validate input
            if (!IsVirtual)
                throw new InvalidOperationException("Only virtual textures can be modified at runtime.");
            if (!initData.IsValid)
                throw new ArgumentException("Invalid texture init data.");
            for (int i = 0; i < initData.Mips.Length; i++)
            {
                if (initData.Mips[i].Data == null ||
                    initData.Mips[i].Data.Length < initData.Mips[i].SlicePitch * initData.ArraySize)
                    throw new ArgumentException("Invalid texture mip init data.");
            }

            // Convert data to internal storage (don't allocate memory)
            InternalInitData t = new InternalInitData();
            t.Format = initData.Format;
            t.Width = initData.Width;
            t.Height = initData.Height;
            t.ArraySize = initData.ArraySize;
            t.MipLevels = initData.Mips.Length;
            if (t.MipLevels > 13)
                t.Data13 = initData.Mips[13].Data;
            if (t.MipLevels > 12)
                t.Data12 = initData.Mips[12].Data;
            if (t.MipLevels > 11)
                t.Data11 = initData.Mips[11].Data;
            if (t.MipLevels > 10)
                t.Data10 = initData.Mips[10].Data;
            if (t.MipLevels > 9)
                t.Data09 = initData.Mips[9].Data;
            if (t.MipLevels > 8)
                t.Data08 = initData.Mips[8].Data;
            if (t.MipLevels > 7)
                t.Data07 = initData.Mips[7].Data;
            if (t.MipLevels > 6)
                t.Data06 = initData.Mips[6].Data;
            if (t.MipLevels > 5)
                t.Data05 = initData.Mips[5].Data;
            if (t.MipLevels > 4)
                t.Data04 = initData.Mips[4].Data;
            if (t.MipLevels > 3)
                t.Data03 = initData.Mips[3].Data;
            if (t.MipLevels > 2)
                t.Data02 = initData.Mips[2].Data;
            if (t.MipLevels > 1)
                t.Data01 = initData.Mips[1].Data;
            if (t.MipLevels > 0)
                t.Data00 = initData.Mips[0].Data;
            int* rowPitches = &t.Data00RowPitch;
            for (int i = 0; i < t.MipLevels; i++)
            {
                rowPitches[i] = initData.Mips[i].RowPitch;
            }
            int* slicePitches = &t.Data00SlicePitch;
            for (int i = 0; i < t.MipLevels; i++)
            {
                slicePitches[i] = initData.Mips[i].SlicePitch;
            }

            // Call backend
            if (Internal_Init(unmanagedPtr, ref t))
                throw new FlaxException("Failed to init texture data.");
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct InternalInitData
        {
            public PixelFormat Format;
            public int Width;
            public int Height;
            public int ArraySize;
            public int MipLevels;

            public int Data00RowPitch;
            public int Data01RowPitch;
            public int Data02RowPitch;
            public int Data03RowPitch;
            public int Data04RowPitch;
            public int Data05RowPitch;
            public int Data06RowPitch;
            public int Data07RowPitch;
            public int Data08RowPitch;
            public int Data09RowPitch;
            public int Data10RowPitch;
            public int Data11RowPitch;
            public int Data12RowPitch;
            public int Data13RowPitch;

            public int Data00SlicePitch;
            public int Data01SlicePitch;
            public int Data02SlicePitch;
            public int Data03SlicePitch;
            public int Data04SlicePitch;
            public int Data05SlicePitch;
            public int Data06SlicePitch;
            public int Data07SlicePitch;
            public int Data08SlicePitch;
            public int Data09SlicePitch;
            public int Data10SlicePitch;
            public int Data11SlicePitch;
            public int Data12SlicePitch;
            public int Data13SlicePitch;

            public byte[] Data00;
            public byte[] Data01;
            public byte[] Data02;
            public byte[] Data03;
            public byte[] Data04;
            public byte[] Data05;
            public byte[] Data06;
            public byte[] Data07;
            public byte[] Data08;
            public byte[] Data09;
            public byte[] Data10;
            public byte[] Data11;
            public byte[] Data12;
            public byte[] Data13;
        }

        #region Internal Calls

#if !UNIT_TEST_COMPILANT
        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern IntPtr Internal_GetNativePtr(IntPtr obj);

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

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern byte[] Internal_GetMipData(IntPtr obj, int mipIndex, out int rowPitch, out int slicePitch);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_Init(IntPtr obj, ref InternalInitData intiData);
#endif

        #endregion
    }
}
