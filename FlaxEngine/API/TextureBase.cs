// Copyright (c) 2012-2020 Wojciech Figat. All rights reserved.

using System;
using System.Runtime.InteropServices;

namespace FlaxEngine
{
    partial class TextureBase
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
        public unsafe void Init(ref InitData initData)
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

            // Convert data to internal storage (don't allocate memory but pin the managed arrays)
            InternalInitData t = new InternalInitData
            {
                Format = initData.Format,
                Width = initData.Width,
                Height = initData.Height,
                ArraySize = initData.ArraySize,
                MipLevels = initData.Mips.Length,
            };
            fixed (byte* data13 = initData.Mips[13].Data)
            fixed (byte* data12 = initData.Mips[11].Data)
            fixed (byte* data11 = initData.Mips[11].Data)
            fixed (byte* data10 = initData.Mips[10].Data)
            fixed (byte* data09 = initData.Mips[9].Data)
            fixed (byte* data08 = initData.Mips[8].Data)
            fixed (byte* data07 = initData.Mips[7].Data)
            fixed (byte* data06 = initData.Mips[6].Data)
            fixed (byte* data05 = initData.Mips[5].Data)
            fixed (byte* data04 = initData.Mips[4].Data)
            fixed (byte* data03 = initData.Mips[3].Data)
            fixed (byte* data02 = initData.Mips[2].Data)
            fixed (byte* data01 = initData.Mips[1].Data)
            fixed (byte* data00 = initData.Mips[0].Data)
            {
                t.Data13 = new IntPtr(data13);
                t.Data12 = new IntPtr(data12);
                t.Data11 = new IntPtr(data11);
                t.Data10 = new IntPtr(data10);
                t.Data09 = new IntPtr(data09);
                t.Data08 = new IntPtr(data08);
                t.Data07 = new IntPtr(data07);
                t.Data06 = new IntPtr(data06);
                t.Data05 = new IntPtr(data05);
                t.Data04 = new IntPtr(data04);
                t.Data03 = new IntPtr(data03);
                t.Data02 = new IntPtr(data02);
                t.Data01 = new IntPtr(data01);
                t.Data00 = new IntPtr(data00);

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
                if (Internal_Init(unmanagedPtr, new IntPtr(&t)))
                    throw new FlaxException("Failed to init texture data.");
            }
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

            public IntPtr Data00;
            public IntPtr Data01;
            public IntPtr Data02;
            public IntPtr Data03;
            public IntPtr Data04;
            public IntPtr Data05;
            public IntPtr Data06;
            public IntPtr Data07;
            public IntPtr Data08;
            public IntPtr Data09;
            public IntPtr Data10;
            public IntPtr Data11;
            public IntPtr Data12;
            public IntPtr Data13;
        }
    }
}
