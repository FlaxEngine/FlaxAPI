// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System;
using System.Runtime.InteropServices;

namespace FlaxEngine
{
    /// <summary>
    /// Representation of RGBA colors in 32 bit format.
    /// </summary>
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct Color32
    {
        /// <summary>
        /// The size of the <see cref="Color32" /> type, in bytes.
        /// </summary>
        public static readonly int SizeInBytes = Marshal.SizeOf(typeof(Color32));

        /// <summary>
        /// The transparent color.
        /// </summary>
        public static readonly Color32 Transparent = new Color32(0, 0, 0, 0);

        /// <summary>
        /// The black color.
        /// </summary>
        public static readonly Color32 Black = new Color32(0, 0, 0, 255);

        /// <summary>
        /// The white color.
        /// </summary>
        public static readonly Color32 White = new Color32(255, 255, 255, 255);

        /// <summary>
        /// Red component of the color.
        /// </summary>
        public byte R;

        /// <summary>
        /// Green component of the color.
        /// </summary>
        public byte G;

        /// <summary>
        /// Blue component of the color.
        /// </summary>
        public byte B;

        /// <summary>
        /// Alpha component of the color.
        /// </summary>
        public byte A;

        /// <summary>
        /// Constructs a new Color32 with given r, g, b, a components.
        /// </summary>
        /// <param name="r">The red component value.</param>
        /// <param name="g">The green component value.</param>
        /// <param name="b">The blue component value.</param>
        /// <param name="a">The alpha component value.</param>
        public Color32(byte r, byte g, byte b, byte a)
        {
            R = r;
            G = g;
            B = b;
            A = a;
        }

        /// <summary>
        /// Linearly interpolates between colors a and b by t.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="t"></param>
        public static Color32 Lerp(Color32 a, Color32 b, float t)
        {
            t = Mathf.Clamp01(t);
            return new Color32((byte)(a.R + (b.R - a.R) * t), (byte)(a.G + (b.G - a.G) * t), (byte)(a.B + (b.B - a.B) * t), (byte)(a.A + (b.A - a.A) * t));
        }

        /// <summary>
        /// Linearly interpolates between colors a and b by t.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="t"></param>
        public static Color32 LerpUnclamped(Color32 a, Color32 b, float t)
        {
            return new Color32((byte)(a.R + (b.R - a.R) * t), (byte)(a.G + (b.G - a.G) * t), (byte)(a.B + (b.B - a.B) * t), (byte)(a.A + (b.A - a.A) * t));
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="Color"/> to <see cref="Color32"/>.
        /// </summary>
        /// <param name="c">The color.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator Color32(Color c)
        {
            return new Color32((byte)(Mathf.Clamp01(c.R) * 255f), (byte)(Mathf.Clamp01(c.G) * 255f), (byte)(Mathf.Clamp01(c.B) * 255f), (byte)(Mathf.Clamp01(c.A) * 255f));
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="Color32"/> to <see cref="Color"/>.
        /// </summary>
        /// <param name="c">The color.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator Color(Color32 c)
        {
            return new Color(c.R / 255f, c.G / 255f, c.B / 255f, c.A / 255f);
        }

        /// <summary>
        /// Performs an explicit conversion from <see cref="Color32"/> to <see cref="Vector4"/>.
        /// </summary>
        /// <param name="c">The color.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static explicit operator Vector4(Color32 c)
        {
            return new Vector4(c.R / 255f, c.G / 255f, c.B / 255f, c.A / 255f);
        }

        /// <summary>
        /// Returns a nicely formatted string of this color.
        /// </summary>
        public override string ToString()
        {
            return string.Format("RGBA({0}, {1}, {2}, {3})", R, G, B, A);
        }

        /// <summary>
        /// Returns a nicely formatted string of this color.
        /// </summary>
        /// <param name="format"></param>
        public string ToString(string format)
        {
            return string.Format("RGBA({0}, {1}, {2}, {3})", R.ToString(format), G.ToString(format), B.ToString(format), A.ToString(format));
        }
    }
}
