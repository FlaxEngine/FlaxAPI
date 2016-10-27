// Celelej Game Engine scripting API

using System.Runtime.InteropServices;

namespace CelelejEngine
{
    /// <summary>
    /// Define a Rectangle.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct Rectangle// : IEquatable<Rectangle>
    {
        // TODO: finish this type

        public float X;

        public float Y;

        public float Width;

        public float Height;

        public Rectangle(float x, float y, float width, float height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        public Rectangle(Vector2 location, Vector2 size)
        {
            X = location.X;
            Y = location.Y;
            Width = size.X;
            Height = size.Y;
        }
    }
}
