namespace CelelejEngine
{
    /// <summary>
    ///   Representation of RGBA colors in 32 bit format.
    /// </summary>
    public struct Color32
    {
        /// <summary>
        ///   Red component of the color.
        /// </summary>
        public byte R;

        /// <summary>
        ///   Green component of the color.
        /// </summary>
        public byte G;

        /// <summary>
        ///   Blue component of the color.
        /// </summary>
        public byte B;

        /// <summary>
        ///   Alpha component of the color.
        /// </summary>
        public byte A;

        /// <summary>
        ///   Constructs a new Color32 with given r, g, b, a components.
        /// </summary>
        /// <param name="r"></param>
        /// <param name="g"></param>
        /// <param name="b"></param>
        /// <param name="a"></param>
        public Color32(byte r, byte g, byte b, byte a)
        {
            R = r;
            G = g;
            B = b;
            A = a;
        }

        /// <summary>
        ///   Linearly interpolates between colors a and b by t.
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
        ///   Linearly interpolates between colors a and b by t.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="t"></param>
        public static Color32 LerpUnclamped(Color32 a, Color32 b, float t)
        {
            return new Color32((byte)(a.R + (b.R - a.R) * t), (byte)(a.G + (b.G - a.G) * t), (byte)(a.B + (b.B - a.B) * t), (byte)(a.A + (b.A - a.A) * t));
        }

        public static implicit operator Color32(Color c)
        {
            return new Color32((byte)(Mathf.Clamp01(c.R) * 255f), (byte)(Mathf.Clamp01(c.G) * 255f), (byte)(Mathf.Clamp01(c.B) * 255f), (byte)(Mathf.Clamp01(c.A) * 255f));
        }

        public static implicit operator Color(Color32 c)
        {
            return new Color(c.R / 255f, c.G / 255f, c.B / 255f, c.A / 255f);
        }

        /// <summary>
        ///   Returns a nicely formatted string of this color.
        /// </summary>
        public override string ToString()
        {
            return string.Format("RGBA({0}, {1}, {2}, {3})", R, G, B, A);
        }

        /// <summary>
        ///   Returns a nicely formatted string of this color.
        /// </summary>
        /// <param name="format"></param>
        public string ToString(string format)
        {
            return string.Format("RGBA({0}, {1}, {2}, {3})", R.ToString(format), G.ToString(format), B.ToString(format), A.ToString(format));
        }
    }
}
