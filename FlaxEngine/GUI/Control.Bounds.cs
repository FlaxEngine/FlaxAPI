////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

namespace FlaxEngine.GUI
{
    // Duplicate of Rectangle API for easier lookup
    public partial class Control
    {
        /// <summary>
        /// Gets or sets X coordinate of the upper-left corner of the control relative to the upper-left corner of its container
        /// </summary>
        public float X
        {
            get => _bounds.X;
            set
            {
                if (!Mathf.NearEqual(_bounds.X, value))
                    SetLocationInternal(new Vector2(value, _bounds.Y));
            }
        }

        /// <summary>
        /// Gets or sets Y coordinate of the upper-left corner of the control relative to the upper-left corner of its container
        /// </summary>
        public float Y
        {
            get => _bounds.Y;
            set
            {
                if (!Mathf.NearEqual(_bounds.Y, value))
                    SetLocationInternal(new Vector2(_bounds.X, value));
            }
        }

        /// <summary>
        /// Gets or sets coordinates of the upper-left corner of the control relative to the upper-left corner of its container
        /// </summary>
        public Vector2 Location
        {
            get => _bounds.Location;
            set
            {
                if (!_bounds.Location.Equals(ref value))
                    SetLocationInternal(value);
            }
        }

        /// <summary>
        /// Gets or sets width of the control
        /// </summary>
        public float Width
        {
            get => _bounds.Width;
            set
            {
                if (!Mathf.NearEqual(_bounds.Width, value))
                    SetSizeInternal(new Vector2(value, _bounds.Height));
            }
        }

        /// <summary>
        /// Gets or sets height of the control
        /// </summary>
        public float Height
        {
            get => _bounds.Height;
            set
            {
                if (!Mathf.NearEqual(_bounds.Height, value))
                    SetSizeInternal(new Vector2(_bounds.Width, value));
            }
        }

        /// <summary>
        /// Gets or sets control's size
        /// </summary>
        public Vector2 Size
        {
            get => _bounds.Size;
            set
            {
                if (!_bounds.Size.Equals(ref value))
                    SetSizeInternal(value);
            }
        }

        /// <summary>
        /// Gets Y coordinate of the top edge of the control relative to the upper-left corner of its container
        /// </summary>
        public float Top => Bounds.Top;

        /// <summary>
        /// Gets Y coordinate of the bottom edge of the control relative to the upper-left corner of its container
        /// </summary>
        public float Bottom => Bounds.Bottom;

        /// <summary>
        /// Gets X coordinate of the left edge of the control relative to the upper-left corner of its container
        /// </summary>
        public float Left => Bounds.Left;

        /// <summary>
        /// Gets X coordinate of the right edge of the control relative to the upper-left corner of its container
        /// </summary>
        public float Right => Bounds.Right;

        /// <summary>
        /// Gets position of the upper left corner of the control relative to the upper-left corner of its container
        /// </summary>
        public Vector2 UpperLeft => Bounds.UpperLeft;

        /// <summary>
        /// Gets position of the upper right corner of the control relative to the upper-left corner of its container
        /// </summary>
        public Vector2 UpperRight => Bounds.UpperRight;

        /// <summary>
        /// Gets position of the bottom right corner of the control relative to the upper-left corner of its container
        /// </summary>
        public Vector2 BottomRight => Bounds.BottomRight;

        /// <summary>
        /// Gets position of the bottom left of the control relative to the upper-left corner of its container
        /// </summary>
        public Vector2 BottomLeft => Bounds.BottomLeft;

        /// <summary>
        /// Gets center position of the control relative to the upper-left corner of its container
        /// </summary>
        public Vector2 Center => Bounds.Center;

        /// <summary>
        /// Gets or sets control's bounds retangle
        /// </summary>
        public Rectangle Bounds
        {
            get => _bounds;
            set
            {
                if (!_bounds.Equals(ref value))
                {
                    SetLocationInternal(_bounds.Location);
                    SetSizeInternal(_bounds.Size);
                }
            }
        }
    }
}
