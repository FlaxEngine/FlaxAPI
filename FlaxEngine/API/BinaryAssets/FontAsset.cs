// Copyright (c) 2012-2020 Wojciech Figat. All rights reserved.

using System;
using System.Runtime.InteropServices;

namespace FlaxEngine
{
    /// <summary>
    /// The font hinting used when rendering characters.
    /// </summary>
    public enum FontHinting : byte
    {
        /// <summary>
        /// Use the default hinting specified in the font.
        /// </summary>
        [Tooltip("Use the default hinting specified in the font.")]
        Default,

        /// <summary>
        /// Force the use of an automatic hinting algorithm (over the font's native hinter).
        /// </summary>
        [Tooltip("Force the use of an automatic hinting algorithm (over the font's native hinter).")]
        Auto,

        /// <summary>
        /// Force the use of an automatic light hinting algorithm, optimized for non-monochrome displays.
        /// </summary>
        [Tooltip("Force the use of an automatic light hinting algorithm, optimized for non-monochrome displays.")]
        AutoLight,

        /// <summary>
        /// Force the use of an automatic hinting algorithm optimized for monochrome displays.
        /// </summary>
        [Tooltip("Force the use of an automatic hinting algorithm optimized for monochrome displays.")]
        Monochrome,

        /// <summary>
        /// Do not use hinting. This generally generates 'blurrier' bitmap glyphs when the glyph are rendered in any of the anti-aliased modes.
        /// </summary>
        [Tooltip("Do not use hinting. This generally generates 'blurrier' bitmap glyphs when the glyph are rendered in any of the anti-aliased modes.")]
        None,
    }

    /// <summary>
    /// The font flags used when rendering characters.
    /// </summary>
    [Flags]
    public enum FontFlags : byte
    {
        /// <summary>
        /// No options.
        /// </summary>
        None = 0,

        /// <summary>
        /// Enables using anti-aliasing for font characters. Otherwise font will use monochrome data.
        /// </summary>
        [Tooltip("Enables using anti-aliasing for font characters. Otherwise font will use monochrome data.")]
        AntiAliasing = 1,

        /// <summary>
        /// Enables artificial embolden effect.
        /// </summary>
        [Tooltip("Enables artificial embolden effect.")]
        Bold = 2,

        /// <summary>
        /// Enables slant effect, emulating italic style.
        /// </summary>
        [Tooltip("Enables slant effect, emulating italic style.")]
        Italic = 4,
    }

    public partial class FontAsset
    {
        /// <summary>
        /// The font asset options.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct FontOptions
        {
            /// <summary>
            /// The hinting.
            /// </summary>
            public FontHinting Hinting;

            /// <summary>
            /// The flags.
            /// </summary>
            public FontFlags Flags;

            /// <summary>
            /// Tests for equality between two objects.
            /// </summary>
            /// <param name="other">The other object to compare.</param>
            /// <returns><c>true</c> if this object has the same value as <paramref name="other" />; otherwise, <c>false</c> </returns>
            public bool Equals(FontOptions other)
            {
                return Hinting == other.Hinting && Flags == other.Flags;
            }

            /// <inheritdoc />
            public override bool Equals(object obj)
            {
                return obj is FontOptions other && Equals(other);
            }

            /// <inheritdoc />
            public override int GetHashCode()
            {
                unchecked
                {
                    return ((int)Hinting * 397) ^ (int)Flags;
                }
            }

            /// <summary>
            /// Tests for equality between two objects.
            /// </summary>
            /// <param name="left">The first value to compare.</param>
            /// <param name="right">The second value to compare.</param>
            /// <returns><c>true</c> if <paramref name="left" /> has the same value as <paramref name="right" />; otherwise, <c>false</c>.</returns>
            public static bool operator ==(FontOptions left, FontOptions right)
            {
                return left.Hinting == right.Hinting && left.Flags == right.Flags;
            }

            /// <summary>
            /// Tests for inequality between two objects.
            /// </summary>
            /// <param name="left">The first value to compare.</param>
            /// <param name="right">The second value to compare.</param>
            /// <returns><c>true</c> if <paramref name="left" /> has a different value than <paramref name="right" />; otherwise,<c>false</c>.</returns>
            public static bool operator !=(FontOptions left, FontOptions right)
            {
                return left.Hinting != right.Hinting || left.Flags != right.Flags;
            }
        };
    }
}
