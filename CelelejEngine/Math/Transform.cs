// Celelej Game Engine scripting API

using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace CelelejEngine
{
    /// <summary>
    /// Represents a three dimensional mathematical transformation.
    /// </summary>
    [Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct Transform : IEquatable<Transform>, IFormattable
    {
        private static readonly string _formatString = "Translation:{0} Orientation:{1} Scale:{2}";

        /// <summary>
        /// The size of the <see cref="Transform" /> type, in bytes
        /// </summary>
        public static readonly int SizeInBytes = Marshal.SizeOf(typeof(Transform));

        /// <summary>
        /// A identity <see cref="Transform" /> with all default values
        /// </summary>
        public static readonly Vector3 Identity;

        /// <summary>
        /// Translation vector of the transform
        /// </summary>
        public Vector3 Translation;

        /// <summary>
        /// Rotation of the transform
        /// </summary>
        public Quaternion Orientation;

        /// <summary>
        /// Scale vector of the transform
        /// </summary>
        public Vector3 Scale;

        /// <summary>
        /// Init
        /// </summary>
        /// <param name="position">Position in 3D space</param>
        public Transform(Vector3 position)
        {
            Translation = position;
            Orientation = Quaternion.Identity;
            Scale = Vector3.One;
        }

        /// <summary>
        /// Init
        /// </summary>
        /// <param name="position">Position in 3D space</param>
        /// <param name="rotation">Rotation in 3D space</param>
        public Transform(Vector3 position, Quaternion rotation)
        {
            Translation = position;
            Orientation = rotation;
            Scale = Vector3.One;
        }

        /// <summary>
        /// Init
        /// </summary>
        /// <param name="position">Position in 3D space</param>
        /// <param name="rotation">Rotation in 3D space</param>
        /// <param name="scale">Transform scale</param>
        public Transform(Vector3 position, Quaternion rotation, Vector3 scale)
        {
            Translation = position;
            Orientation = rotation;
            Scale = scale;
        }

        /// <summary>
        /// Gets a value indicting whether this transform is identity
        /// </summary>
        public bool IsIdentity
        {
            get { return Equals(Identity); }
        }

        /// <summary>
        /// Tests for equality between two objects.
        /// </summary>
        /// <param name="left">The first value to compare.</param>
        /// <param name="right">The second value to compare.</param>
        /// <returns>
        /// <c>true</c> if <paramref name="left" /> has the same value as <paramref name="right" />; otherwise,
        /// <c>false</c>.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(Transform left, Transform right)
        {
            return left.Equals(ref right);
        }

        /// <summary>
        /// Tests for inequality between two objects.
        /// </summary>
        /// <param name="left">The first value to compare.</param>
        /// <param name="right">The second value to compare.</param>
        /// <returns>
        /// <c>true</c> if <paramref name="left" /> has a different value than <paramref name="right" />; otherwise,
        /// <c>false</c>.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(Transform left, Transform right)
        {
            return !left.Equals(ref right);
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format(CultureInfo.CurrentCulture, _formatString, Translation, Orientation, Scale);
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public string ToString(string format)
        {
            if (format == null)
                return ToString();

            return string.Format(CultureInfo.CurrentCulture, _formatString, 
                Translation.ToString(format, CultureInfo.CurrentCulture),
                Orientation.ToString(format, CultureInfo.CurrentCulture), 
                Scale.ToString(format, CultureInfo.CurrentCulture));
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <param name="formatProvider">The format provider.</param>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public string ToString(IFormatProvider formatProvider)
        {
            return string.Format(formatProvider, _formatString, Translation, Orientation, Scale);
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="formatProvider">The format provider.</param>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public string ToString(string format, IFormatProvider formatProvider)
        {
            if (format == null)
                return ToString(formatProvider);

            return string.Format(formatProvider, _formatString,
                Translation.ToString(format, formatProvider),
                Orientation.ToString(format, formatProvider),
                Scale.ToString(format, formatProvider));
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.
        /// </returns>
        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = Translation.GetHashCode();
                hashCode = (hashCode * 397) ^ Orientation.GetHashCode();
                hashCode = (hashCode * 397) ^ Scale.GetHashCode();
                return hashCode;
            }
        }

        /// <summary>
        /// Determines whether the specified <see cref="Transform" /> is equal to this instance.
        /// </summary>
        /// <param name="other">The <see cref="Transform" /> to compare with this instance.</param>
        /// <returns>
        /// <c>true</c> if the specified <see cref="Transform" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(ref Transform other)
        {
            return Translation == other.Translation
                && Orientation == other.Orientation
                && Scale == other.Scale;
        }

        /// <summary>
        /// Determines whether the specified <see cref="Transform" /> is equal to this instance.
        /// </summary>
        /// <param name="other">The <see cref="Transform" /> to compare with this instance.</param>
        /// <returns>
        /// <c>true</c> if the specified <see cref="Transform" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(Transform other)
        {
            return Equals(ref other);
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object" /> is equal to this instance.
        /// </summary>
        /// <param name="value">The <see cref="System.Object" /> to compare with this instance.</param>
        /// <returns>
        /// <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object value)
        {
            if (!(value is Transform))
                return false;

            var strongValue = (Transform)value;
            return Equals(ref strongValue);
        }
    }
}
