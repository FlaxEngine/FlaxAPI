using System;

namespace FlaxEngine
{
    /// <summary>
    /// Representation of RGBA colors.
    /// </summary>
    [Serializable]
    public partial struct Color
    {
        /// <summary>
        /// Red component of the color.
        /// </summary>
        public float R;

        /// <summary>
        /// Green component of the color.
        /// </summary>
        public float G;

        /// <summary>
        /// Blue component of the color.
        /// </summary>
        public float B;

        /// <summary>
        /// Alpha component of the color.
        /// </summary>
        public float A;

        /// <summary>
        /// Gets or sets the component at the specified index.
        /// </summary>
        /// <value>The value of the red, green, blue, and alpha components, depending on the index.</value>
        /// <param name="index">The index of the component to access. Use 0 for the alpha component, 1 for the red component, 2 for the green component, and 3 for the blue component.</param>
        /// <returns>The value of the component at the specified index.</returns>
        /// <exception cref="System.IndexOutOfRangeException">Thrown when the <paramref name="index"/> is out of the range [0, 3].</exception>
        public float this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0:
                    {
                        return R;
                    }
                    case 1:
                    {
                        return G;
                    }
                    case 2:
                    {
                        return B;
                    }
                    case 3:
                    {
                        return A;
                    }
                }
                throw new IndexOutOfRangeException("Invalid Color index!");
            }
            set
            {
                switch (index)
                {
                    case 0:
                    {
                        R = value;
                        break;
                    }
                    case 1:
                    {
                        G = value;
                        break;
                    }
                    case 2:
                    {
                        B = value;
                        break;
                    }
                    case 3:
                    {
                        A = value;
                        break;
                    }
                    default:
                    {
                        throw new IndexOutOfRangeException("Invalid Color index!");
                    }
                }
            }
        }

        /// <summary>
        /// Returns the minimum color component value: Min(r,g,b).
        /// </summary>
        public float MinColorComponent => Mathf.Min(Mathf.Min(R, G), B);

        /// <summary>
        /// Returns the maximum color component value: Max(r,g,b).
        /// </summary>
        public float MaxColorComponent => Mathf.Max(Mathf.Max(R, G), B);

        /// <summary>
        /// Constructs a new Color with given r,g,b,a component.
        /// </summary>
        /// <param name="rgba">RGBA component.</param>
        public Color(float rgba)
        {
            R = rgba;
            G = rgba;
            B = rgba;
            A = rgba;
        }

        /// <summary>
        /// Constructs a new Color with given r,g,b,a components.
        /// </summary>
        /// <param name="r">Red component.</param>
        /// <param name="g">Green component.</param>
        /// <param name="b">Blue component.</param>
        /// <param name="a">Alpha component.</param>
        public Color(float r, float g, float b, float a)
        {
            R = r;
            G = g;
            B = b;
            A = a;
        }

        /// <summary>
        /// Constructs a new Color with given r,g,b,a components.
        /// </summary>
        /// <param name="r">Red component.</param>
        /// <param name="g">Green component.</param>
        /// <param name="b">Blue component.</param>
        /// <param name="a">Alpha component.</param>
        public Color(byte r, byte g, byte b, byte a)
        {
            R = r / 255.0f;
            G = g / 255.0f;
            B = b / 255.0f;
            A = a / 255.0f;
        }

        /// <summary>
        /// Constructs a new Color with given r,g,b components and sets a to 1.
        /// </summary>
        /// <param name="r">Red component.</param>
        /// <param name="g">Green component.</param>
        /// <param name="b">Blue component.</param>
        public Color(float r, float g, float b)
        {
            R = r;
            G = g;
            B = b;
            A = 1f;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Color"/> struct.
        /// </summary>
        /// <param name="values">The values to assign to the red, green, blue, and alpha components of the color. This must be an array with four elements.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="values"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="values"/> contains more or less than four elements.</exception>
        public Color(float[] values)
        {
            if (values == null)
                throw new ArgumentNullException(nameof(values));
            if (values.Length != 4)
                throw new ArgumentOutOfRangeException(nameof(values), "There must be four and only four input values for Color.");

            R = values[0];
            G = values[1];
            B = values[2];
            A = values[3];
        }

        internal Color AlphaMultiplied(float multiplier)
        {
            return new Color(R, G, B, A * multiplier);
        }

        /// <inheritdoc />
        public override bool Equals(object other)
        {
            if (!(other is Color))
                return false;
            var color = (Color)other;
            return R.Equals(color.R) && G.Equals(color.G) && B.Equals(color.B) && A.Equals(color.A);
        }

        /// <summary>
        /// Determines whether the specified <see cref="Color"/> is equal to this instance.
        /// </summary>
        /// <param name="other">The <see cref="Color"/> to compare with this instance.</param>
        /// <returns>
        /// <c>true</c> if the specified <see cref="Color"/> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public bool Equals(Color other)
        {
            return R.Equals(other.R) && G.Equals(other.G) && B.Equals(other.B) && A.Equals(other.A);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = R.GetHashCode();
                hashCode = (hashCode * 397) ^ G.GetHashCode();
                hashCode = (hashCode * 397) ^ B.GetHashCode();
                hashCode = (hashCode * 397) ^ A.GetHashCode();
                return hashCode;
            }
        }

        /// <summary>
        /// Converts the color from a packed BGRA integer.
        /// </summary>
        /// <param name="color">A packed integer containing all four color components in BGRA order</param>
        /// <returns>A color.</returns>
        public static Color FromBgra(int color)
        {
            return new Color((byte)((color >> 16) & 255), (byte)((color >> 8) & 255), (byte)(color & 255), (byte)((color >> 24) & 255));
        }

        /// <summary>
        /// Converts the color from a packed BGRA integer.
        /// </summary>
        /// <param name="color">A packed integer containing all four color components in BGRA order</param>
        /// <returns>A color.</returns>
        public static Color FromBgra(uint color)
        {
            return FromBgra(unchecked((int)color));
        }

        /// <summary>
        /// Converts the color into a packed integer.
        /// </summary>
        /// <returns>A packed integer containing all four color components.</returns>
        public int ToBgra()
        {
            uint a = (uint)(A * 255.0f) & 255;
            uint r = (uint)(R * 255.0f) & 255;
            uint g = (uint)(G * 255.0f) & 255;
            uint b = (uint)(B * 255.0f) & 255;

            uint value = b;
            value |= g << 8;
            value |= r << 16;
            value |= a << 24;

            return (int)value;
        }

        /// <summary>
        /// Converts the color into a packed integer.
        /// </summary>
        /// <returns>A packed integer containing all four color components.</returns>
        public void ToBgra(out byte r, out byte g, out byte b, out byte a)
        {
            a = (byte)(A * 255.0f);
            r = (byte)(R * 255.0f);
            g = (byte)(G * 255.0f);
            b = (byte)(B * 255.0f);
        }

        /// <summary>
        /// Converts the color into a packed integer.
        /// </summary>
        /// <returns>A packed integer containing all four color components.</returns>
        public int ToRgba()
        {
            uint a = (uint)(A * 255.0f) & 255;
            uint r = (uint)(R * 255.0f) & 255;
            uint g = (uint)(G * 255.0f) & 255;
            uint b = (uint)(B * 255.0f) & 255;

            uint value = r;
            value |= g << 8;
            value |= b << 16;
            value |= a << 24;

            return (int)value;
        }

        /// <summary>
        /// Converts the color into a three component vector.
        /// </summary>
        /// <returns>A three component vector containing the red, green, and blue components of the color.</returns>
        public Vector3 ToVector3()
        {
            return new Vector3(R, G, B);
        }

        /// <summary>
        /// Converts the color into a four component vector.
        /// </summary>
        /// <returns>A four component vector containing all four color components.</returns>
        public Vector4 ToVector4()
        {
            return new Vector4(R, G, B, A);
        }

        /// <summary>
        /// Creates an array containing the elements of the color.
        /// </summary>
        /// <returns>A four-element array containing the components of the color.</returns>
        public float[] ToArray()
        {
            return new[] {R, G, B, A};
        }

        /// <summary>
        /// Converts this color from linear space to sRGB space.
        /// </summary>
        /// <returns>A color3 in sRGB space.</returns>
        public Color ToSRgb()
        {
            return new Color(Mathf.LinearToSRgb(R), Mathf.LinearToSRgb(G), Mathf.LinearToSRgb(B), A);
        }

        /// <summary>
        /// Converts this color from sRGB space to linear space.
        /// </summary>
        /// <returns>A Color in linear space.</returns>
        public Color ToLinear()
        {
            return new Color(Mathf.SRgbToLinear(R), Mathf.SRgbToLinear(G), Mathf.SRgbToLinear(B), A);
        }

        /// <summary>
        /// Creates an RGB colour from HSV input.
        /// </summary>
        /// <param name="h">Hue [0..1].</param>
        /// <param name="s">Saturation [0..1].</param>
        /// <param name="v">Value [0..1].</param>
        /// <returns>
        /// An opaque colour with HSV matching the input.
        /// </returns>
        public static Color HSVToRGB(float h, float s, float v)
        {
            return HSVToRGB(h, s, v, true);
        }

        /// <summary>
        /// Creates an RGB colour from HSV input.
        /// </summary>
        /// <param name="H">Hue [0..1].</param>
        /// <param name="S">Saturation [0..1].</param>
        /// <param name="V">Value [0..1].</param>
        /// <param name="hdr">Output HDR colours. If true, the returned colour will not be clamped to [0..1].</param>
        /// <returns>
        /// An opaque colour with HSV matching the input.
        /// </returns>
        public static Color HSVToRGB(float H, float S, float V, bool hdr)
        {
            Color v = White;
            if (S == 0f)
            {
                v.R = V;
                v.G = V;
                v.B = V;
            }
            else if (V != 0f)
            {
                v.R = 0f;
                v.G = 0f;
                v.B = 0f;
                float s = S;
                float single = V;
                float h = H * 6f;
                var num = (int)Mathf.Floor(h);
                float single1 = h - num;
                float single2 = single * (1f - s);
                float single3 = single * (1f - s * single1);
                float single4 = single * (1f - s * (1f - single1));
                switch (num)
                {
                    case -1:
                    {
                        v.R = single;
                        v.G = single2;
                        v.B = single3;
                        break;
                    }
                    case 0:
                    {
                        v.R = single;
                        v.G = single4;
                        v.B = single2;
                        break;
                    }
                    case 1:
                    {
                        v.R = single3;
                        v.G = single;
                        v.B = single2;
                        break;
                    }
                    case 2:
                    {
                        v.R = single2;
                        v.G = single;
                        v.B = single4;
                        break;
                    }
                    case 3:
                    {
                        v.R = single2;
                        v.G = single3;
                        v.B = single;
                        break;
                    }
                    case 4:
                    {
                        v.R = single4;
                        v.G = single2;
                        v.B = single;
                        break;
                    }
                    case 5:
                    {
                        v.R = single;
                        v.G = single2;
                        v.B = single3;
                        break;
                    }
                    case 6:
                    {
                        v.R = single;
                        v.G = single4;
                        v.B = single2;
                        break;
                    }
                }
                if (!hdr)
                {
                    v.R = Mathf.Clamp(v.R, 0f, 1f);
                    v.G = Mathf.Clamp(v.G, 0f, 1f);
                    v.B = Mathf.Clamp(v.B, 0f, 1f);
                }
            }
            else
            {
                v.R = 0f;
                v.G = 0f;
                v.B = 0f;
            }
            return v;
        }

        /// <summary>
        /// Linearly interpolates between colors a and b by t.
        /// </summary>
        /// <param name="a">Color a</param>
        /// <param name="b">Color b</param>
        /// <param name="t">Float for combining a and b</param>
        public static Color Lerp(Color a, Color b, float t)
        {
            t = Mathf.Clamp01(t);
            return new Color(a.R + (b.R - a.R) * t, a.G + (b.G - a.G) * t, a.B + (b.B - a.B) * t, a.A + (b.A - a.A) * t);
        }

        /// <summary>
        /// Linearly interpolates between colors a and b by t.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="t"></param>
        public static Color LerpUnclamped(Color a, Color b, float t)
        {
            return new Color(a.R + (b.R - a.R) * t, a.G + (b.G - a.G) * t, a.B + (b.B - a.B) * t, a.A + (b.A - a.A) * t);
        }

        public static Color operator +(Color a, Color b)
        {
            return new Color(a.R + b.R, a.G + b.G, a.B + b.B, a.A + b.A);
        }

        public static Color operator /(Color a, float b)
        {
            return new Color(a.R / b, a.G / b, a.B / b, a.A / b);
        }

        public static bool operator ==(Color lhs, Color rhs)
        {
            return Equals(lhs, rhs);
        }

        public static implicit operator Vector3(Color c)
        {
            return new Vector3(c.R, c.G, c.B);
        }

        public static implicit operator Vector4(Color c)
        {
            return new Vector4(c.R, c.G, c.B, c.A);
        }

        public static implicit operator Color(Vector4 v)
        {
            return new Color(v.X, v.Y, v.Z, v.W);
        }

        public static implicit operator Color(Vector3 v)
        {
            return new Color(v.X, v.Y, v.Z);
        }

        public static bool operator !=(Color lhs, Color rhs)
        {
            return !(lhs == rhs);
        }

        public static Color operator *(Color a, Color b)
        {
            return new Color(a.R * b.R, a.G * b.G, a.B * b.B, a.A * b.A);
        }

        public static Color operator *(Color a, float b)
        {
            return new Color(a.R * b, a.G * b, a.B * b, a.A * b);
        }

        public static Color operator *(float b, Color a)
        {
            return new Color(a.R * b, a.G * b, a.B * b, a.A * b);
        }

        public static Color operator -(Color a, Color b)
        {
            return new Color(a.R - b.R, a.G - b.G, a.B - b.B, a.A - b.A);
        }

        internal Color RGBMultiplied(float multiplier)
        {
            return new Color(R * multiplier, G * multiplier, B * multiplier, A);
        }

        internal Color RGBMultiplied(Color multiplier)
        {
            return new Color(R * multiplier.R, G * multiplier.G, B * multiplier.B, A);
        }

        public static void RGBToHSV(Color rgbColor, out float h, out float s, out float v)
        {
            if ((rgbColor.B > rgbColor.G) && (rgbColor.B > rgbColor.R))
                RGBToHSVHelper(4f, rgbColor.B, rgbColor.R, rgbColor.G, out h, out s, out v);
            else if (rgbColor.G <= rgbColor.R)
                RGBToHSVHelper(0f, rgbColor.R, rgbColor.G, rgbColor.B, out h, out s, out v);
            else
                RGBToHSVHelper(2f, rgbColor.G, rgbColor.B, rgbColor.R, out h, out s, out v);
        }

        private static void RGBToHSVHelper(float offset, float dominantcolor, float colorone, float colortwo, out float h, out float s, out float v)
        {
            v = dominantcolor;
            if (v == 0f)
            {
                s = 0f;
                h = 0f;
            }
            else
            {
                var single = 0f;
                single = colorone <= colortwo ? colorone : colortwo;
                float vv = v - single;
                if (vv == 0f)
                {
                    s = 0f;
                    h = offset + (colorone - colortwo);
                }
                else
                {
                    s = vv / v;
                    h = offset + (colorone - colortwo) / vv;
                }
                h = h / 6f;
                if (h < 0f)
                    h = h + 1f;
            }
        }

        /// <summary>
        /// Adjusts the contrast of a color.
        /// </summary>
        /// <param name="value">The color whose contrast is to be adjusted.</param>
        /// <param name="contrast">The amount by which to adjust the contrast.</param>
        /// <param name="result">When the method completes, contains the adjusted color.</param>
        public static void AdjustContrast(ref Color value, float contrast, out Color result)
        {
            result.A = value.A;
            result.R = 0.5f + contrast * (value.R - 0.5f);
            result.G = 0.5f + contrast * (value.G - 0.5f);
            result.B = 0.5f + contrast * (value.B - 0.5f);
        }

        /// <summary>
        /// Adjusts the contrast of a color.
        /// </summary>
        /// <param name="value">The color whose contrast is to be adjusted.</param>
        /// <param name="contrast">The amount by which to adjust the contrast.</param>
        /// <returns>The adjusted color.</returns>
        public static Color AdjustContrast(Color value, float contrast)
        {
            return new Color(
                0.5f + contrast * (value.R - 0.5f),
                0.5f + contrast * (value.G - 0.5f),
                0.5f + contrast * (value.B - 0.5f),
                value.A);
        }

        /// <summary>
        /// Adjusts the saturation of a color.
        /// </summary>
        /// <param name="value">The color whose saturation is to be adjusted.</param>
        /// <param name="saturation">The amount by which to adjust the saturation.</param>
        /// <param name="result">When the method completes, contains the adjusted color.</param>
        public static void AdjustSaturation(ref Color value, float saturation, out Color result)
        {
            float grey = value.R * 0.2125f + value.G * 0.7154f + value.B * 0.0721f;

            result.A = value.A;
            result.R = grey + saturation * (value.R - grey);
            result.G = grey + saturation * (value.G - grey);
            result.B = grey + saturation * (value.B - grey);
        }

        /// <summary>
        /// Adjusts the saturation of a color.
        /// </summary>
        /// <param name="value">The color whose saturation is to be adjusted.</param>
        /// <param name="saturation">The amount by which to adjust the saturation.</param>
        /// <returns>The adjusted color.</returns>
        public static Color AdjustSaturation(Color value, float saturation)
        {
            float grey = value.R * 0.2125f + value.G * 0.7154f + value.B * 0.0721f;

            return new Color(
                grey + saturation * (value.R - grey),
                grey + saturation * (value.G - grey),
                grey + saturation * (value.B - grey),
                value.A);
        }

        /// <summary>
        /// Premultiplies the color components by the alpha value.
        /// </summary>
        /// <param name="value">The color to premultiply.</param>
        /// <returns>A color with premultiplied alpha.</returns>
        public static Color PremultiplyAlpha(Color value)
        {
            return new Color(value.R * value.A, value.G * value.A, value.B * value.A, value.A);
        }

        /// <summary>
        /// Returns a nicely formatted string of this color.
        /// </summary>
        public override string ToString()
        {
            return string.Format("RGBA({0:F3}, {1:F3}, {2:F3}, {3:F3})", R, G, B, A);
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
