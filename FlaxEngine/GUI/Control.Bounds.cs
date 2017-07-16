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
                Location = value.Location;
                Size = value.Size;
            }
        }

        /// <summary>
        /// Gets or sets the scale.
        /// </summary>
        /// <value>
        /// The scale.
        /// </value>
        public Vector2 Scale
        {
            get => _scale;
            set
            {
                if (!_scale.Equals(ref value))
                    SetScaleInternal(ref value);
            }
        }

        /// <summary>
        /// Gets or sets the normalized pivot location (used to transform control around it).
        /// </summary>
        /// <value>
        /// The pivot.
        /// </value>
        public Vector2 Pivot
        {
            get => _pivot;
            set
            {
                if (!_pivot.Equals(ref value))
                    SetPivotInternal(ref value);
            }
        }

        /// <summary>
        /// Gets or sets the shear.
        /// </summary>
        /// <value>
        /// The shear.
        /// </value>
        public Vector2 Shear
        {
            get => _shear;
            set
            {
                if (!_shear.Equals(ref value))
                    SetShearInternal(ref value);
            }
        }

        /// <summary>
        /// Gets or sets the rotation angle (in degrees).
        /// </summary>
        /// <value>
        /// The rotation.
        /// </value>
        public float Rotation
        {
            get => _rotation;
            set
            {
                if (!Mathf.NearEqual(_rotation, value))
                    SetRotationInternal(value);
            }
        }

        /// <summary>
        /// Updates the control transform.
        /// </summary>
        protected void UpdateTransform()
        {
            // Transformation matrix building:
            // - calculate 2D transformation (scale, rotation and shear)
            // - calculate actual pivot location (pivot * size)
            // - apply negative pivot offset
            // - apply 2D transform
            // - apply pivot offset
            // - apply location offset

            // 2D transformation
            Matrix2x2 m1, m2;
            Matrix2x2.Scale(ref _scale, out m1);
            Matrix2x2.Shear(ref _shear, out m2);
            Matrix2x2.Multiply(ref m1, ref m2, out m1);
            Matrix2x2.Rotation(Mathf.DegreesToRadians * _rotation, out m2);
            Matrix2x2.Multiply(ref m1, ref m2, out m1);

            // Actual pivot and negative pivot
            Vector2 v1, v2;
            Vector2.Multiply(ref _pivot, ref _bounds.Location, out v1);
            Vector2.Negate(ref v1, out v2);
            Vector2.Add(ref v1, ref _bounds.Location, out v1);

            // Mix all the stuff
            Matrix3x3 m3;
            Matrix3x3.Translation2D(ref v2, out m3);
            Matrix3x3 m4 = (Matrix3x3)m1;
            Matrix3x3.Multiply(ref m3, ref m4, out m3);
            Matrix3x3.Translation2D(ref v1, out m4);
            Matrix3x3.Multiply(ref m3, ref m4, out _cachedTransform);

            // TODO: Temp test code:
            var pivot = _pivot * Size;
            var mmm = Matrix.Translation(-pivot.X, -pivot.Y, 1) * Matrix.RotationZ(_rotation * Mathf.DegreesToRadians) * Matrix.Translation(pivot.X + X, pivot.Y + Y, 1);
            _cachedTransform = new Matrix3x3(
                mmm.M11, mmm.M12, mmm.M13,
                mmm.M21, mmm.M22, mmm.M23,
                mmm.M41, mmm.M42, 1.0f
                );
        }
    }
}
