// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System;

namespace FlaxEngine.Rendering
{
    /// <summary>
    /// Extensions to <see cref="PixelFormat"/>.
    /// </summary>
    public static class PixelFormatExtensions
    {
        private const int MaxPixelFormats = 256;
        private static int[] _sizeOfInBits = new int[MaxPixelFormats];

        private static int GetIndex(PixelFormat format)
        {
            // DirectX official pixel formats (0..115 use 0..127 in the arrays)
            // Custom pixel formats (1024..1151? use 128..255 in the array)
            //if ((int)format >= 1024)
            //	return (int)format - 1024 + 128;

            return (int)format;
        }

        private static void InitFormat(PixelFormat[] formats, int bitCount)
        {
            for (int i = 0; i < formats.Length; i++)
                _sizeOfInBits[GetIndex(formats[i])] = bitCount;
        }

        /// <summary>
        /// Initializes the static <see cref="PixelFormatExtensions"/> class.
        /// </summary>
        static PixelFormatExtensions()
        {
            PixelFormat[] formats1 =
            {
                PixelFormat.R1_UNorm
            };
            InitFormat(formats1, 1);

            PixelFormat[] formats2 =
            {
                PixelFormat.A8_UNorm,
                PixelFormat.R8_SInt,
                PixelFormat.R8_SNorm,
                PixelFormat.R8_Typeless,
                PixelFormat.R8_UInt,
                PixelFormat.R8_UNorm
            };
            InitFormat(formats2, 8);

            PixelFormat[] formats3 =
            {
                PixelFormat.B5G5R5A1_UNorm,
                PixelFormat.B5G6R5_UNorm,
                PixelFormat.D16_UNorm,
                PixelFormat.R16_Float,
                PixelFormat.R16_SInt,
                PixelFormat.R16_SNorm,
                PixelFormat.R16_Typeless,
                PixelFormat.R16_UInt,
                PixelFormat.R16_UNorm,
                PixelFormat.R8G8_SInt,
                PixelFormat.R8G8_SNorm,
                PixelFormat.R8G8_Typeless,
                PixelFormat.R8G8_UInt,
                PixelFormat.R8G8_UNorm
            };
            InitFormat(formats3, 16);

            PixelFormat[] formats4 =
            {
                PixelFormat.B8G8R8X8_Typeless,
                PixelFormat.B8G8R8X8_UNorm,
                PixelFormat.B8G8R8X8_UNorm_sRGB,
                PixelFormat.D24_UNorm_S8_UInt,
                PixelFormat.D32_Float,
                PixelFormat.D32_Float_S8X24_UInt,
                PixelFormat.G8R8_G8B8_UNorm,
                PixelFormat.R10G10B10_Xr_Bias_A2_UNorm,
                PixelFormat.R10G10B10A2_Typeless,
                PixelFormat.R10G10B10A2_UInt,
                PixelFormat.R10G10B10A2_UNorm,
                PixelFormat.R11G11B10_Float,
                PixelFormat.R16G16_Float,
                PixelFormat.R16G16_SInt,
                PixelFormat.R16G16_SNorm,
                PixelFormat.R16G16_Typeless,
                PixelFormat.R16G16_UInt,
                PixelFormat.R16G16_UNorm,
                PixelFormat.R24_UNorm_X8_Typeless,
                PixelFormat.R24G8_Typeless,
                PixelFormat.R32_Float,
                PixelFormat.R32_Float_X8X24_Typeless,
                PixelFormat.R32_SInt,
                PixelFormat.R32_Typeless,
                PixelFormat.R32_UInt,
                PixelFormat.R8G8_B8G8_UNorm,
                PixelFormat.R8G8B8A8_SInt,
                PixelFormat.R8G8B8A8_SNorm,
                PixelFormat.R8G8B8A8_Typeless,
                PixelFormat.R8G8B8A8_UInt,
                PixelFormat.R8G8B8A8_UNorm,
                PixelFormat.R8G8B8A8_UNorm_sRGB,
                PixelFormat.B8G8R8A8_Typeless,
                PixelFormat.B8G8R8A8_UNorm,
                PixelFormat.B8G8R8A8_UNorm_sRGB,
                PixelFormat.R9G9B9E5_SharedExp,
                PixelFormat.X24_Typeless_G8_UInt,
                PixelFormat.X32_Typeless_G8X24_UInt,
            };
            InitFormat(formats4, 32);

            PixelFormat[] formats5 =
            {
                PixelFormat.R16G16B16A16_Float,
                PixelFormat.R16G16B16A16_SInt,
                PixelFormat.R16G16B16A16_SNorm,
                PixelFormat.R16G16B16A16_Typeless,
                PixelFormat.R16G16B16A16_UInt,
                PixelFormat.R16G16B16A16_UNorm,
                PixelFormat.R32G32_Float,
                PixelFormat.R32G32_SInt,
                PixelFormat.R32G32_Typeless,
                PixelFormat.R32G32_UInt,
                PixelFormat.R32G8X24_Typeless,
            };
            InitFormat(formats5, 64);

            PixelFormat[] formats6 =
            {
                PixelFormat.R32G32B32_Float,
                PixelFormat.R32G32B32_SInt,
                PixelFormat.R32G32B32_Typeless,
                PixelFormat.R32G32B32_UInt,
            };
            InitFormat(formats6, 96);

            PixelFormat[] formats7 =
            {
                PixelFormat.R32G32B32A32_Float,
                PixelFormat.R32G32B32A32_SInt,
                PixelFormat.R32G32B32A32_Typeless,
                PixelFormat.R32G32B32A32_UInt,
            };
            InitFormat(formats7, 128);

            PixelFormat[] formats8 =
            {
                PixelFormat.BC1_Typeless,
                PixelFormat.BC1_UNorm,
                PixelFormat.BC1_UNorm_sRGB,
                PixelFormat.BC4_SNorm,
                PixelFormat.BC4_Typeless,
                PixelFormat.BC4_UNorm,
            };
            InitFormat(formats8, 4);

            PixelFormat[] formats9 =
            {
                PixelFormat.BC2_Typeless,
                PixelFormat.BC2_UNorm,
                PixelFormat.BC2_UNorm_sRGB,
                PixelFormat.BC3_Typeless,
                PixelFormat.BC3_UNorm,
                PixelFormat.BC3_UNorm_sRGB,
                PixelFormat.BC5_SNorm,
                PixelFormat.BC5_Typeless,
                PixelFormat.BC5_UNorm,
                PixelFormat.BC6H_Sf16,
                PixelFormat.BC6H_Typeless,
                PixelFormat.BC6H_Uf16,
                PixelFormat.BC7_Typeless,
                PixelFormat.BC7_UNorm,
                PixelFormat.BC7_UNorm_sRGB,
            };
            InitFormat(formats9, 8);
        }

        /// <summary>
        /// Calculates the size of a <see cref="PixelFormat"/> in bytes.
        /// </summary>
        /// <param name="format">The Pixel format.</param>
        /// <returns>size of in bytes</returns>
        public static int SizeInBytes(this PixelFormat format)
        {
            return SizeInBits(format) / 8;
        }

        /// <summary>
        /// Calculates the size of a <see cref="PixelFormat"/> in bits.
        /// </summary>
        /// <param name="format">The pixel format.</param>
        /// <returns>The size in bits</returns>
        public static int SizeInBits(this PixelFormat format)
        {
            return _sizeOfInBits[GetIndex(format)];
        }

        /// <summary>
        /// Calculate the size of the alpha channel in bits depending on the pixel format.
        /// </summary>
        /// <param name="format">The pixel format</param>
        /// <returns>The size in bits</returns>
        public static int AlphaSizeInBits(this PixelFormat format)
        {
            switch (format)
            {
            case PixelFormat.R32G32B32A32_Typeless:
            case PixelFormat.R32G32B32A32_Float:
            case PixelFormat.R32G32B32A32_UInt:
            case PixelFormat.R32G32B32A32_SInt:
                return 32;

            case PixelFormat.R16G16B16A16_Typeless:
            case PixelFormat.R16G16B16A16_Float:
            case PixelFormat.R16G16B16A16_UNorm:
            case PixelFormat.R16G16B16A16_UInt:
            case PixelFormat.R16G16B16A16_SNorm:
            case PixelFormat.R16G16B16A16_SInt:
                return 16;

            case PixelFormat.R10G10B10A2_Typeless:
            case PixelFormat.R10G10B10A2_UNorm:
            case PixelFormat.R10G10B10A2_UInt:
            case PixelFormat.R10G10B10_Xr_Bias_A2_UNorm:
                return 2;

            case PixelFormat.R8G8B8A8_Typeless:
            case PixelFormat.R8G8B8A8_UNorm:
            case PixelFormat.R8G8B8A8_UNorm_sRGB:
            case PixelFormat.R8G8B8A8_UInt:
            case PixelFormat.R8G8B8A8_SNorm:
            case PixelFormat.R8G8B8A8_SInt:
            case PixelFormat.B8G8R8A8_UNorm:
            case PixelFormat.B8G8R8A8_Typeless:
            case PixelFormat.B8G8R8A8_UNorm_sRGB:
            case PixelFormat.A8_UNorm:
                return 8;

            case PixelFormat.B5G5R5A1_UNorm:
                return 1;

            case PixelFormat.BC1_Typeless:
            case PixelFormat.BC1_UNorm:
            case PixelFormat.BC1_UNorm_sRGB:
                return 1; // or 0

            case PixelFormat.BC2_Typeless:
            case PixelFormat.BC2_UNorm:
            case PixelFormat.BC2_UNorm_sRGB:
                return 4;

            case PixelFormat.BC3_Typeless:
            case PixelFormat.BC3_UNorm:
            case PixelFormat.BC3_UNorm_sRGB:
                return 8;

            case PixelFormat.BC7_Typeless:
            case PixelFormat.BC7_UNorm:
            case PixelFormat.BC7_UNorm_sRGB:
                return 8; // or 0

            default: return 0;
            }
        }

        /// <summary>
        /// Determines whether the specified <see cref="PixelFormat"/> contains alpha channel.
        /// </summary>
        /// <param name="format">The Pixel Format.</param>
        /// <returns><c>true</c> if the specified <see cref="PixelFormat"/> has alpha; otherwise, <c>false</c>.</returns>
        public static bool HasAlpha(this PixelFormat format)
        {
            return AlphaSizeInBits(format) != 0;
        }

        /// <summary>
        /// Determines whether the specified <see cref="PixelFormat"/> is depth stencil.
        /// </summary>
        /// <param name="format">The Pixel Format.</param>
        /// <returns><c>true</c> if the specified <see cref="PixelFormat"/> is depth stencil; otherwise, <c>false</c>.</returns>
        public static bool IsDepthStencil(this PixelFormat format)
        {
            switch (format)
            {
            case PixelFormat.R32G8X24_Typeless:
            case PixelFormat.D32_Float_S8X24_UInt:
            case PixelFormat.R32_Float_X8X24_Typeless:
            case PixelFormat.X32_Typeless_G8X24_UInt:
            case PixelFormat.D32_Float:
            case PixelFormat.R24G8_Typeless:
            case PixelFormat.D24_UNorm_S8_UInt:
            case PixelFormat.R24_UNorm_X8_Typeless:
            case PixelFormat.X24_Typeless_G8_UInt:
            case PixelFormat.D16_UNorm:
                return true;

            default:
                return false;
            }
        }

        /// <summary>
        /// Determines whether the specified <see cref="PixelFormat"/> has stencil bits.
        /// </summary>
        /// <param name="format">The Pixel Format.</param>
        /// <returns><c>true</c> if the specified <see cref="PixelFormat"/> has stencil bits; otherwise, <c>false</c>.</returns>
        public static bool HasStencil(this PixelFormat format)
        {
            switch (format)
            {
            case PixelFormat.D24_UNorm_S8_UInt:
                return true;

            default:
                return false;
            }
        }

        /// <summary>
        /// Determines whether the specified <see cref="PixelFormat"/> is Typeless.
        /// </summary>
        /// <param name="format">The <see cref="PixelFormat"/>.</param>
        /// <param name="partialTypeless">Enable/disable prtially typeless formats.</param>
        /// <returns><c>true</c> if the specified <see cref="PixelFormat"/> is Typeless; otherwise, <c>false</c>.</returns>
        public static bool IsTypeless(this PixelFormat format, bool partialTypeless)
        {
            switch (format)
            {
            case PixelFormat.R32G32B32A32_Typeless:
            case PixelFormat.R32G32B32_Typeless:
            case PixelFormat.R16G16B16A16_Typeless:
            case PixelFormat.R32G32_Typeless:
            case PixelFormat.R32G8X24_Typeless:
            case PixelFormat.R10G10B10A2_Typeless:
            case PixelFormat.R8G8B8A8_Typeless:
            case PixelFormat.R16G16_Typeless:
            case PixelFormat.R32_Typeless:
            case PixelFormat.R24G8_Typeless:
            case PixelFormat.R8G8_Typeless:
            case PixelFormat.R16_Typeless:
            case PixelFormat.R8_Typeless:
            case PixelFormat.BC1_Typeless:
            case PixelFormat.BC2_Typeless:
            case PixelFormat.BC3_Typeless:
            case PixelFormat.BC4_Typeless:
            case PixelFormat.BC5_Typeless:
            case PixelFormat.B8G8R8A8_Typeless:
            case PixelFormat.B8G8R8X8_Typeless:
            case PixelFormat.BC6H_Typeless:
            case PixelFormat.BC7_Typeless:
                return true;

            case PixelFormat.R32_Float_X8X24_Typeless:
            case PixelFormat.X32_Typeless_G8X24_UInt:
            case PixelFormat.R24_UNorm_X8_Typeless:
            case PixelFormat.X24_Typeless_G8_UInt:
                return partialTypeless;

            default:
                return false;
            }
        }

        /// <summary>
        /// Returns true if the <see cref="PixelFormat"/> is valid.
        /// </summary>
        /// <param name="format">A format to validate</param>
        /// <returns>True if the <see cref="PixelFormat"/> is valid.</returns>
        public static bool IsValid(this PixelFormat format)
        {
            return ((int)(format) >= 1 && (int)(format) <= 115);
        }

        /// <summary>
        /// Returns true if the <see cref="PixelFormat"/> is a compressed format.
        /// </summary>
        /// <param name="format">The format to check for compressed.</param>
        /// <returns>True if the <see cref="PixelFormat"/> is a compressed format</returns>
        public static bool IsCompressed(this PixelFormat format)
        {
            switch (format)
            {
            case PixelFormat.BC1_Typeless:
            case PixelFormat.BC1_UNorm:
            case PixelFormat.BC1_UNorm_sRGB:
            case PixelFormat.BC2_Typeless:
            case PixelFormat.BC2_UNorm:
            case PixelFormat.BC2_UNorm_sRGB:
            case PixelFormat.BC3_Typeless:
            case PixelFormat.BC3_UNorm:
            case PixelFormat.BC3_UNorm_sRGB:
            case PixelFormat.BC4_Typeless:
            case PixelFormat.BC4_UNorm:
            case PixelFormat.BC4_SNorm:
            case PixelFormat.BC5_Typeless:
            case PixelFormat.BC5_UNorm:
            case PixelFormat.BC5_SNorm:
            case PixelFormat.BC6H_Typeless:
            case PixelFormat.BC6H_Uf16:
            case PixelFormat.BC6H_Sf16:
            case PixelFormat.BC7_Typeless:
            case PixelFormat.BC7_UNorm:
            case PixelFormat.BC7_UNorm_sRGB:
                return true;

            default:
                return false;
            }
        }

        /// <summary>
        /// Determines whether the specified <see cref="PixelFormat"/> is packed.
        /// </summary>
        /// <param name="format">The Pixel Format.</param>
        /// <returns><c>true</c> if the specified <see cref="PixelFormat"/> is packed; otherwise, <c>false</c>.</returns>
        public static bool IsPacked(this PixelFormat format)
        {
            return ((format == PixelFormat.R8G8_B8G8_UNorm) || (format == PixelFormat.G8R8_G8B8_UNorm));
        }

        /// <summary>
        /// Determines whether the specified <see cref="PixelFormat"/> is planar.
        /// </summary>
        /// <param name="format">The Pixel Format.</param>
        /// <returns><c>true</c> if the specified <see cref="PixelFormat"/> is planar; otherwise, <c>false</c>.</returns>
        public static bool IsPlanar(this PixelFormat format)
        {
            return false;
        }

        /// <summary>
        /// Determines whether the specified <see cref="PixelFormat"/> is video.
        /// </summary>
        /// <param name="format">The <see cref="PixelFormat"/>.</param>
        /// <returns><c>true</c> if the specified <see cref="PixelFormat"/> is video; otherwise, <c>false</c>.</returns>
        public static bool IsVideo(this PixelFormat format)
        {
            return false;
        }

        /// <summary>
        /// Determines whether the specified <see cref="PixelFormat"/> is a sRGB format.
        /// </summary>
        /// <param name="format">The <see cref="PixelFormat"/>.</param>
        /// <returns><c>true</c> if the specified <see cref="PixelFormat"/> is a sRGB format; otherwise, <c>false</c>.</returns>
        public static bool IsSRGB(this PixelFormat format)
        {
            switch (format)
            {
            case PixelFormat.R8G8B8A8_UNorm_sRGB:
            case PixelFormat.BC1_UNorm_sRGB:
            case PixelFormat.BC2_UNorm_sRGB:
            case PixelFormat.BC3_UNorm_sRGB:
            case PixelFormat.B8G8R8A8_UNorm_sRGB:
            case PixelFormat.B8G8R8X8_UNorm_sRGB:
            case PixelFormat.BC7_UNorm_sRGB:
                return true;

            default:
                return false;
            }
        }

        /// <summary>
        /// Determines whether the specified <see cref="PixelFormat"/> is HDR (either 16 or 32bits Float)
        /// </summary>
        /// <param name="format">The format.</param>
        /// <returns><c>true</c> if the specified pixel format is HDR (Floating poInt); otherwise, <c>false</c>.</returns>
        public static bool IsHDR(this PixelFormat format)
        {
            switch (format)
            {
            case PixelFormat.R16G16B16A16_Float:
            case PixelFormat.R32G32B32A32_Float:
            case PixelFormat.R16G16_Float:
            case PixelFormat.R16_Float:
            case PixelFormat.BC6H_Sf16:
            case PixelFormat.BC6H_Uf16:
                return true;

            default: return false;
            }
        }

        /// <summary>
        /// Determines whether the specified format is in RGBA order.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <returns>
        ///   <c>true</c> if the specified format is in RGBA order; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsRgbAOrder(this PixelFormat format)
        {
            switch (format)
            {
            case PixelFormat.R32G32B32A32_Typeless:
            case PixelFormat.R32G32B32A32_Float:
            case PixelFormat.R32G32B32A32_UInt:
            case PixelFormat.R32G32B32A32_SInt:
            case PixelFormat.R32G32B32_Typeless:
            case PixelFormat.R32G32B32_Float:
            case PixelFormat.R32G32B32_UInt:
            case PixelFormat.R32G32B32_SInt:
            case PixelFormat.R16G16B16A16_Typeless:
            case PixelFormat.R16G16B16A16_Float:
            case PixelFormat.R16G16B16A16_UNorm:
            case PixelFormat.R16G16B16A16_UInt:
            case PixelFormat.R16G16B16A16_SNorm:
            case PixelFormat.R16G16B16A16_SInt:
            case PixelFormat.R32G32_Typeless:
            case PixelFormat.R32G32_Float:
            case PixelFormat.R32G32_UInt:
            case PixelFormat.R32G32_SInt:
            case PixelFormat.R32G8X24_Typeless:
            case PixelFormat.R10G10B10A2_Typeless:
            case PixelFormat.R10G10B10A2_UNorm:
            case PixelFormat.R10G10B10A2_UInt:
            case PixelFormat.R11G11B10_Float:
            case PixelFormat.R8G8B8A8_Typeless:
            case PixelFormat.R8G8B8A8_UNorm:
            case PixelFormat.R8G8B8A8_UNorm_sRGB:
            case PixelFormat.R8G8B8A8_UInt:
            case PixelFormat.R8G8B8A8_SNorm:
            case PixelFormat.R8G8B8A8_SInt:
                return true;

            default:
                return false;
            }
        }

        /// <summary>
        /// Determines whether the specified format is in BGRA order.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <returns>
        ///   <c>true</c> if the specified format is in BGRA order; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsBGRAOrder(this PixelFormat format)
        {
            switch (format)
            {
            case PixelFormat.B8G8R8A8_UNorm:
            case PixelFormat.B8G8R8X8_UNorm:
            case PixelFormat.B8G8R8A8_Typeless:
            case PixelFormat.B8G8R8A8_UNorm_sRGB:
            case PixelFormat.B8G8R8X8_Typeless:
            case PixelFormat.B8G8R8X8_UNorm_sRGB:
                return true;

            default:
                return false;
            }
        }

        /// <summary>
        /// Computes the scanline count (number of scanlines).
        /// </summary>
        /// <param name="format">The <see cref="PixelFormat"/>.</param>
        /// <param name="height">The height.</param>
        /// <returns>The scanline count.</returns>
        public static int ComputeScanlineCount(this PixelFormat format, int height)
        {
            switch (format)
            {
            case PixelFormat.BC1_Typeless:
            case PixelFormat.BC1_UNorm:
            case PixelFormat.BC1_UNorm_sRGB:
            case PixelFormat.BC2_Typeless:
            case PixelFormat.BC2_UNorm:
            case PixelFormat.BC2_UNorm_sRGB:
            case PixelFormat.BC3_Typeless:
            case PixelFormat.BC3_UNorm:
            case PixelFormat.BC3_UNorm_sRGB:
            case PixelFormat.BC4_Typeless:
            case PixelFormat.BC4_UNorm:
            case PixelFormat.BC4_SNorm:
            case PixelFormat.BC5_Typeless:
            case PixelFormat.BC5_UNorm:
            case PixelFormat.BC5_SNorm:
            case PixelFormat.BC6H_Typeless:
            case PixelFormat.BC6H_Uf16:
            case PixelFormat.BC6H_Sf16:
            case PixelFormat.BC7_Typeless:
            case PixelFormat.BC7_UNorm:
            case PixelFormat.BC7_UNorm_sRGB:
                return Math.Max(1, (height + 3) / 4);

            default:
                return height;
            }
        }

        /// <summary>
        /// Find the equivalent sRGB format to the provided format.
        /// </summary>
        /// <param name="format">The non sRGB format.</param>
        /// <returns>The equivalent sRGB format if any, the provided format else.</returns>
        public static PixelFormat TosRGB(this PixelFormat format)
        {
            switch (format)
            {
            case PixelFormat.R8G8B8A8_UNorm:
                return PixelFormat.R8G8B8A8_UNorm_sRGB;

            case PixelFormat.BC1_UNorm:
                return PixelFormat.BC1_UNorm_sRGB;

            case PixelFormat.BC2_UNorm:
                return PixelFormat.BC2_UNorm_sRGB;

            case PixelFormat.BC3_UNorm:
                return PixelFormat.BC3_UNorm_sRGB;

            case PixelFormat.B8G8R8A8_UNorm:
                return PixelFormat.B8G8R8A8_UNorm_sRGB;

            case PixelFormat.B8G8R8X8_UNorm:
                return PixelFormat.B8G8R8X8_UNorm_sRGB;

            case PixelFormat.BC7_UNorm:
                return PixelFormat.BC7_UNorm_sRGB;

            default:
                return format;
            }
        }

        /// <summary>
        /// Find the equivalent non sRGB format to the provided sRGB format.
        /// </summary>
        /// <param name="format">The non sRGB format.</param>
        /// <returns>The equivalent non sRGB format if any, the provided format else.</returns>
        public static PixelFormat ToNonsRGB(this PixelFormat format)
        {
            switch (format)
            {
            case PixelFormat.R8G8B8A8_UNorm_sRGB:
                return PixelFormat.R8G8B8A8_UNorm;

            case PixelFormat.BC1_UNorm_sRGB:
                return PixelFormat.BC1_UNorm;

            case PixelFormat.BC2_UNorm_sRGB:
                return PixelFormat.BC2_UNorm;

            case PixelFormat.BC3_UNorm_sRGB:
                return PixelFormat.BC3_UNorm;

            case PixelFormat.B8G8R8A8_UNorm_sRGB:
                return PixelFormat.B8G8R8A8_UNorm;

            case PixelFormat.B8G8R8X8_UNorm_sRGB:
                return PixelFormat.B8G8R8X8_UNorm;

            case PixelFormat.BC7_UNorm_sRGB:
                return PixelFormat.BC7_UNorm;

            default:
                return format;
            }
        }
    }
}
