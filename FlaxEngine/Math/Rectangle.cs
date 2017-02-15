// Flax Engine scripting API

using System;
using System.Runtime.InteropServices;

namespace FlaxEngine
{
    /// <summary>
    /// Define a Rectangle.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct Rectangle : IEquatable<Rectangle>
    {
        /// <summary>
        /// Rectangle location (coordinates of the upper-left corner)
        /// </summary>
        public Vector2 Location;

        /// <summary>
        /// Rectangle size
        /// </summary>
        public Vector2 Size;

        /// <summary>
        /// Gets or sets X coordinate of the left edge of the rectangle
        /// </summary>
        public float X
        {
            get { return Location.X; }
            set { Location.X = value; }
        }

        /// <summary>
        /// Gets or sets Y coordinate of the left edge of the rectangle
        /// </summary>
        public float Y
        {
            get { return Location.Y; }
            set { Location.Y = value; }
        }

        /// <summary>
        /// Gets or sets width of the rectangle
        /// </summary>
        public float Width
        {
            get { return Size.X; }
            set { Size.X = value; }
        }

        /// <summary>
        /// Gets or sets height of the rectangle
        /// </summary>
        public float Height
        {
            get { return Size.Y; }
            set { Size.Y = value; }
        }

        /// <summary>
        /// Gets Y coordinate of the top edge of the rectangle
        /// </summary>
        public float Top
        {
            get { return Location.Y; }
        }

        /// <summary>
        /// Gets Y coordinate of the bottom edge of the rectangle
        /// </summary>
        public float Bottom
        {
            get { return Location.Y + Size.Y; }
        }

        /// <summary>
        /// Gets X coordinate of the left edge of the rectangle
        /// </summary>
        public float Left
        {
            get { return Location.X; }
        }

        /// <summary>
        /// Gets X coordinate of the right edge of the rectangle
        /// </summary>
        public float Right
        {
            get { return Location.X + Size.X; }
        }

        /// <summary>
        /// Gets position of the upper left corner of the rectangle
        /// </summary>
        public Vector2 UpperLeft
        {
            get { return Location; }
        }

        /// <summary>
        /// Gets position of the upper right corner of the rectangle
        /// </summary>
        public Vector2 UpperRight
        {
            get { return new Vector2(Location.X + Size.X, Location.Y); }
        }

        /// <summary>
        /// Gets position of the bottom right corner of the rectangle
        /// </summary>
        public Vector2 BottomRight
        {
            get { return Location + Size; }
        }

        /// <summary>
        /// Gets position of the bottom left corner of the rectangle
        /// </summary>
        public Vector2 BottomLeft
        {
            get { return new Vector2(Location.X, Location.Y + Size.Y); }
        }

        /// <summary>
        /// Gets center position of the rectangle
        /// </summary>
        public Vector2 Center
        {
            get { return Location + Size * 0.5f; }
        }

        /// <summary>
        /// Init
        /// </summary>
        /// <param name="x">X coordinate</param>
        /// <param name="y">Y coordinate</param>
        /// <param name="width">Width</param>
        /// <param name="height">Height</param>
        public Rectangle(float x, float y, float width, float height)
        {
            Location = new Vector2(x, y);
            Size = new Vector2(width, height);
        }

        /// <summary>
        /// Init
        /// </summary>
        /// <param name="location">Location of the upper left corner</param>
        /// <param name="size">Size</param>
        public Rectangle(Vector2 location, Vector2 size)
        {
            Location = location;
            Size = size;
        }

        /// <summary>
        /// Checks if rectangle contains given point
        /// </summary>
        /// <param name="location">Point location to check</param>
        /// <returns>True if point is inside rectangle's area</returns>
        public bool Contains(Vector2 location)
        {
            return (location.X >= Location.X && location.Y >= Location.Y) && (location.X <= Location.X + Size.X && location.Y <= Location.Y + Size.Y);
        }

        /// <summary>
        /// Checks if rectangle contains given point
        /// </summary>
        /// <param name="location">Point location to check</param>
        /// <returns>True if point is inside rectangle's area</returns>
        public bool Contains(ref Vector2 location)
        {
            return (location.X >= Location.X && location.Y >= Location.Y) && (location.X <= Location.X + Size.X && location.Y <= Location.Y + Size.Y);
        }

        /// <summary>
        /// Determines whether this rectangle entirely contains a specified rectangle
        /// </summary>
        /// <param name="value">The rectangle to evaluate</param>
        /// <returns>True if this rectangle entirely contains the specified rectangle, or false if not</returns>
        public bool Contains(Rectangle value)
        {
            return (Location.X <= value.Location.X) && (value.Right <= Right) && (Location.Y <= value.Location.Y) && (value.Bottom <= Bottom);
        }

        /// <summary>
        /// Determines whether a specified rectangle intersects with this rectangle
        /// </summary>
        /// <param name="value">The rectangle to evaluate</param>
        /// <returns>True if the specified rectangle intersects with this one, therwise false</returns>
        public bool Intersects(Rectangle value)
        {
            return (value.Location.X < Right) && (Location.X < value.Right) && (value.Location.Y < Bottom) && (Location.Y < value.Bottom);
        }

        /// <summary>
        /// Offset rectangle position
        /// </summary>
        /// <param name="x">X coordinate offset</param>
        /// <param name="y">Y coordinate offset</param>
        public void Offset(float x, float y)
        {
            X += x;
            Y += y;
        }

        /// <summary>
        /// Offset rectangle position
        /// </summary>
        /// <param name="offset">X and Y coordinate offset</param>
        public void Offset(Vector2 offset)
        {
            Location += offset;
        }

        /// <summary>
        /// Make offseted rectangle
        /// </summary>
        /// <param name="offset">X and Y coordinate offset</param>
        /// <returns>Offseted rectangle</returns>
        public Rectangle MakeOffseted(Vector2 offset)
        {
            return new Rectangle(Location + offset, Size);
        }

        /// <summary>
        /// Expand rectangle area in all directions by given amout
        /// </summary>
        /// <param name="toExpand">Amount of units to expand a rectangle</param>
        public void Expand(float toExpand)
        {
            Location -= toExpand * 0.5f;
            Size += toExpand;
        }

        /// <summary>
        /// Make expanded rectangle area in all directions by given amout
        /// </summary>
        /// <param name="toExpand">Amount of units to expand a rectangle</param>
        /// <returns>Expanded rectangle</returns>
        public Rectangle MakeExpanded(float toExpand)
        {
            return new Rectangle(Location - toExpand * 0.5f, Size + toExpand);
        }

        /// <summary>
        /// Scale rectangle area in all directions by given amout
        /// </summary>
        /// <param name="scale">Scale value to expand a rectangle</param>
        public void Scale(float scale)
        {
            Vector2 toExpand = Size * (scale - 1.0f) * 0.5f;
            Location -= toExpand * 0.5f;
            Size += toExpand;
        }

        /// <summary>
        /// Make scaled rectangle area in all directions by given amout
        /// </summary>
        /// <param name="scale">Scale value to expand a rectangle</param>
        /// <returns>Scaled rectangle</returns>
        public Rectangle MakeScaled(float scale)
        {
            Vector2 toExpand = Size * (scale - 1.0f) * 0.5f;
            return new Rectangle(Location - toExpand * 0.5f, Size + toExpand);
        }

        /// <summary>
        /// Calculates a rectangle that contains the union of a and b rectangles
        /// </summary>
        /// <param name="a">First rectangle</param>
        /// <param name="b">Second rectangle</param>
        /// <returns>Rectangle that contains both a and b rectangles</returns>
        public static Rectangle Union(Rectangle a, Rectangle b)
        {
            float left = Mathf.Min(a.Left, b.Left);
            float right = Mathf.Max(a.Right, b.Right);
            float top = Mathf.Min(a.Top, b.Top);
            float bottom = Mathf.Max(a.Bottom, b.Bottom);
            return new Rectangle(left, top, Mathf.Max(right - left, 0.0f), Mathf.Max(bottom - top, 0.0f));
        }

        /// <summary>
        /// Calculates a rectangle that contains the shared part of a and b rectangles
        /// </summary>
        /// <param name="a">First rectangle</param>
        /// <param name="b">Second rectangle</param>
        /// <returns>Rectangle that contains shared part of a and b rectangles</returns>
        public static Rectangle Shared(Rectangle a, Rectangle b)
        {
            float left = Mathf.Max(a.Left, b.Left);
            float right = Mathf.Min(a.Right, b.Right);
            float top = Mathf.Max(a.Top, b.Top);
            float bottom = Mathf.Min(a.Bottom, b.Bottom);
            return new Rectangle(left, top, Mathf.Max(right - left, 0.0f), Mathf.Max(bottom - top, 0.0f));
        }

        /// <summary>
        /// Creates rectangle from two points
        /// </summary>
        /// <param name="p1">First point</param>
        /// <param name="p2">Second point</param>
        /// <returns>Rectangle that contains both p1 and p2</returns>
        public static Rectangle FromPoints(Vector2 p1, Vector2 p2)
        {
            Vector2 upperLeft, rightBottom;
            Vector2.Min(ref p1, ref p2, out upperLeft);
            Vector2.Max(ref p1, ref p2, out rightBottom);
            return new Rectangle(upperLeft, Vector2.Max(rightBottom - upperLeft, Vector2.Zero));
        }

        #region Operators

        public static Rectangle operator+(Rectangle rectangle, Vector2 offset)
        {
            return new Rectangle(rectangle.Location + offset, rectangle.Size);
        }

        public static Rectangle operator-(Rectangle rectangle, Vector2 offset)
        {
            return new Rectangle(rectangle.Location - offset, rectangle.Size);
        }

        #endregion

        /// <inheritdoc />
        public bool Equals(Rectangle other)
        {
            return Location.Equals(other.Location) && Size.Equals(other.Size);
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            return obj is Rectangle && Equals((Rectangle)obj);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            unchecked
            {
                return (Location.GetHashCode() * 397) ^ Size.GetHashCode();
            }
        }
    }
}
