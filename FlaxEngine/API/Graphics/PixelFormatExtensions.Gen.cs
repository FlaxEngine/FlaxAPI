// This code was auto-generated. Do not modify it.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace FlaxEngine
{
    /// <summary>
    /// Extensions to <see cref="PixelFormat"/>.
    /// </summary>
    [Tooltip("Extensions to <see cref=\"PixelFormat\"/>.")]
    public static unsafe partial class PixelFormatExtensions
    {
        /// <summary>
        /// Calculates the size of a <see cref="PixelFormat"/> in bytes.
        /// </summary>
        /// <param name="format">The Pixel format.</param>
        /// <returns>size of in bytes</returns>
        public static int SizeInBytes(PixelFormat format)
        {
            return Internal_SizeInBytes(format);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern int Internal_SizeInBytes(PixelFormat format);

        /// <summary>
        /// Calculates the size of a <see cref="PixelFormat"/> in bits.
        /// </summary>
        /// <param name="format">The pixel format.</param>
        /// <returns>The size in bits</returns>
        public static int SizeInBits(PixelFormat format)
        {
            return Internal_SizeInBits(format);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern int Internal_SizeInBits(PixelFormat format);

        /// <summary>
        /// Calculate the size of the alpha channel in bits depending on the pixel format.
        /// </summary>
        /// <param name="format">The pixel format</param>
        /// <returns>The size in bits</returns>
        public static int AlphaSizeInBits(PixelFormat format)
        {
            return Internal_AlphaSizeInBits(format);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern int Internal_AlphaSizeInBits(PixelFormat format);

        /// <summary>
        /// Determines whether the specified <see cref="PixelFormat"/> contains alpha channel.
        /// </summary>
        /// <param name="format">The Pixel Format.</param>
        /// <returns><c>true</c> if the specified <see cref="PixelFormat"/> has alpha; otherwise, <c>false</c>.</returns>
        public static bool HasAlpha(PixelFormat format)
        {
            return Internal_HasAlpha(format);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_HasAlpha(PixelFormat format);

        /// <summary>
        /// Determines whether the specified <see cref="PixelFormat"/> is depth stencil.
        /// </summary>
        /// <param name="format">The Pixel Format.</param>
        /// <returns><c>true</c> if the specified <see cref="PixelFormat"/> is depth stencil; otherwise, <c>false</c>.</returns>
        public static bool IsDepthStencil(PixelFormat format)
        {
            return Internal_IsDepthStencil(format);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_IsDepthStencil(PixelFormat format);

        /// <summary>
        /// Determines whether the specified <see cref="PixelFormat"/> has stencil bits.
        /// </summary>
        /// <param name="format">The Pixel Format.</param>
        /// <returns><c>true</c> if the specified <see cref="PixelFormat"/> has stencil bits; otherwise, <c>false</c>.</returns>
        public static bool HasStencil(PixelFormat format)
        {
            return Internal_HasStencil(format);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_HasStencil(PixelFormat format);

        /// <summary>
        /// Determines whether the specified <see cref="PixelFormat"/> is Typeless.
        /// </summary>
        /// <param name="format">The <see cref="PixelFormat"/>.</param>
        /// <param name="partialTypeless">Enable/disable partially typeless formats.</param>
        /// <returns><c>true</c> if the specified <see cref="PixelFormat"/> is Typeless; otherwise, <c>false</c>.</returns>
        public static bool IsTypeless(PixelFormat format, bool partialTypeless)
        {
            return Internal_IsTypeless(format, partialTypeless);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_IsTypeless(PixelFormat format, bool partialTypeless);

        /// <summary>
        /// Returns true if the <see cref="PixelFormat"/> is valid.
        /// </summary>
        /// <param name="format">A format to validate</param>
        /// <returns>True if the <see cref="PixelFormat"/> is valid.</returns>
        public static bool IsValid(PixelFormat format)
        {
            return Internal_IsValid(format);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_IsValid(PixelFormat format);

        /// <summary>
        /// Returns true if the <see cref="PixelFormat"/> is a compressed format.
        /// </summary>
        /// <param name="format">The format to check for compressed.</param>
        /// <returns>True if the <see cref="PixelFormat"/> is a compressed format</returns>
        public static bool IsCompressed(PixelFormat format)
        {
            return Internal_IsCompressed(format);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_IsCompressed(PixelFormat format);

        /// <summary>
        /// Determines whether the specified <see cref="PixelFormat"/> is packed.
        /// </summary>
        /// <param name="format">The Pixel Format.</param>
        /// <returns><c>true</c> if the specified <see cref="PixelFormat"/> is packed; otherwise, <c>false</c>.</returns>
        public static bool IsPacked(PixelFormat format)
        {
            return Internal_IsPacked(format);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_IsPacked(PixelFormat format);

        /// <summary>
        /// Determines whether the specified <see cref="PixelFormat"/> is planar.
        /// </summary>
        /// <param name="format">The Pixel Format.</param>
        /// <returns><c>true</c> if the specified <see cref="PixelFormat"/> is planar; otherwise, <c>false</c>.</returns>
        public static bool IsPlanar(PixelFormat format)
        {
            return Internal_IsPlanar(format);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_IsPlanar(PixelFormat format);

        /// <summary>
        /// Determines whether the specified <see cref="PixelFormat"/> is video.
        /// </summary>
        /// <param name="format">The <see cref="PixelFormat"/>.</param>
        /// <returns><c>true</c> if the specified <see cref="PixelFormat"/> is video; otherwise, <c>false</c>.</returns>
        public static bool IsVideo(PixelFormat format)
        {
            return Internal_IsVideo(format);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_IsVideo(PixelFormat format);

        /// <summary>
        /// Determines whether the specified <see cref="PixelFormat"/> is a sRGB format.
        /// </summary>
        /// <param name="format">The <see cref="PixelFormat"/>.</param>
        /// <returns><c>true</c> if the specified <see cref="PixelFormat"/> is a sRGB format; otherwise, <c>false</c>.</returns>
        public static bool IsSRGB(PixelFormat format)
        {
            return Internal_IsSRGB(format);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_IsSRGB(PixelFormat format);

        /// <summary>
        /// Determines whether the specified <see cref="PixelFormat"/> is HDR (either 16 or 32bits Float)
        /// </summary>
        /// <param name="format">The format.</param>
        /// <returns><c>true</c> if the specified pixel format is HDR (Floating poInt); otherwise, <c>false</c>.</returns>
        public static bool IsHDR(PixelFormat format)
        {
            return Internal_IsHDR(format);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_IsHDR(PixelFormat format);

        /// <summary>
        /// Determines whether the specified format is in RGBA order.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <returns><c>true</c> if the specified format is in RGBA order; otherwise, <c>false</c>.</returns>
        public static bool IsRgbAOrder(PixelFormat format)
        {
            return Internal_IsRgbAOrder(format);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_IsRgbAOrder(PixelFormat format);

        /// <summary>
        /// Determines whether the specified format is in BGRA order.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <returns><c>true</c> if the specified format is in BGRA order; otherwise, <c>false</c>.</returns>
        public static bool IsBGRAOrder(PixelFormat format)
        {
            return Internal_IsBGRAOrder(format);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_IsBGRAOrder(PixelFormat format);

        /// <summary>
        /// Determines whether the specified format contains normalized data. It indicates that values stored in an integer format are to be mapped to the range [-1,1] (for signed values) or [0,1] (for unsigned values) when they are accessed and converted to floating point.
        /// </summary>
        /// <param name="format">The <see cref="PixelFormat"/>.</param>
        /// <returns>True if given format contains normalized data type, otherwise false.</returns>
        public static bool IsNormalized(PixelFormat format)
        {
            return Internal_IsNormalized(format);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_IsNormalized(PixelFormat format);

        /// <summary>
        /// Determines whether the specified format is integer data type (signed or unsigned).
        /// </summary>
        /// <param name="format">The <see cref="PixelFormat"/>.</param>
        /// <returns>True if given format contains integer data type (signed or unsigned), otherwise false.</returns>
        public static bool IsInteger(PixelFormat format)
        {
            return Internal_IsInteger(format);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_IsInteger(PixelFormat format);

        /// <summary>
        /// Computes the scanline count (number of scanlines).
        /// </summary>
        /// <param name="format">The <see cref="PixelFormat"/>.</param>
        /// <param name="height">The height.</param>
        /// <returns>The scanline count.</returns>
        public static int ComputeScanlineCount(PixelFormat format, int height)
        {
            return Internal_ComputeScanlineCount(format, height);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern int Internal_ComputeScanlineCount(PixelFormat format, int height);

        /// <summary>
        /// Computes the format components count (number of R, G, B, A channels).
        /// </summary>
        /// <param name="format">The <see cref="PixelFormat"/>.</param>
        /// <returns>The components count.</returns>
        public static int ComputeComponentsCount(PixelFormat format)
        {
            return Internal_ComputeComponentsCount(format);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern int Internal_ComputeComponentsCount(PixelFormat format);

        /// <summary>
        /// Finds the equivalent sRGB format to the provided format.
        /// </summary>
        /// <param name="format">The non sRGB format.</param>
        /// <returns>The equivalent sRGB format if any, the provided format else.</returns>
        public static PixelFormat TosRGB(PixelFormat format)
        {
            return Internal_TosRGB(format);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern PixelFormat Internal_TosRGB(PixelFormat format);

        /// <summary>
        /// Finds the equivalent non sRGB format to the provided sRGB format.
        /// </summary>
        /// <param name="format">The non sRGB format.</param>
        /// <returns>The equivalent non sRGB format if any, the provided format else.</returns>
        public static PixelFormat ToNonsRGB(PixelFormat format)
        {
            return Internal_ToNonsRGB(format);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern PixelFormat Internal_ToNonsRGB(PixelFormat format);

        /// <summary>
        /// Converts the format to typeless.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <returns>The typeless format.</returns>
        public static PixelFormat MakeTypeless(PixelFormat format)
        {
            return Internal_MakeTypeless(format);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern PixelFormat Internal_MakeTypeless(PixelFormat format);

        /// <summary>
        /// Converts the typeless format to float.
        /// </summary>
        /// <param name="format">The typeless format.</param>
        /// <returns>The float format.</returns>
        public static PixelFormat MakeTypelessFloat(PixelFormat format)
        {
            return Internal_MakeTypelessFloat(format);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern PixelFormat Internal_MakeTypelessFloat(PixelFormat format);

        /// <summary>
        /// Converts the typeless format to unorm.
        /// </summary>
        /// <param name="format">The typeless format.</param>
        /// <returns>The unorm format.</returns>
        public static PixelFormat MakeTypelessUNorm(PixelFormat format)
        {
            return Internal_MakeTypelessUNorm(format);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern PixelFormat Internal_MakeTypelessUNorm(PixelFormat format);
    }
}
