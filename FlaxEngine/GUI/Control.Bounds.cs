// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

namespace FlaxEngine.GUI
{
    // Duplicate of Rectangle API for easier lookup
    public partial class Control
    {
        /// <summary>
        /// Gets or sets X coordinate of the upper-left corner of the control relative to the upper-left corner of its container
        /// </summary>
        [HideInEditor, NoSerialize]
        public float X
        {
            get => _bounds.X;
            set
            {
                if (!Mathf.NearEqual(_bounds.X, value))
                {
                    var location = new Vector2(value, _bounds.Y);
                    SetLocationInternal(ref location);
                }
            }
        }

        /// <summary>
        /// Gets or sets Y coordinate of the upper-left corner of the control relative to the upper-left corner of its container
        /// </summary>
        [HideInEditor, NoSerialize]
        public float Y
        {
            get => _bounds.Y;
            set
            {
                if (!Mathf.NearEqual(_bounds.Y, value))
                {
                    var location = new Vector2(_bounds.X, value);
                    SetLocationInternal(ref location);
                }
            }
        }

        /// <summary>
        /// Gets or sets coordinates of the upper-left corner of the control relative to the upper-left corner of its container
        /// </summary>
        [NoSerialize]
        [ExpandGroups, EditorDisplay("Transform"), EditorOrder(1000), Tooltip("The location of the upper-left corner of the control relative to he upper-left corner of its container.")]
        public Vector2 Location
        {
            get => _bounds.Location;
            set
            {
                if (!_bounds.Location.Equals(ref value))
                    SetLocationInternal(ref value);
            }
        }

        /// <summary>
        /// Gets or sets width of the control
        /// </summary>
        [HideInEditor, NoSerialize]
        public float Width
        {
            get => _bounds.Width;
            set
            {
                if (!Mathf.NearEqual(_bounds.Width, value))
                {
                    var size = new Vector2(value, _bounds.Height);
                    SetSizeInternal(ref size);
                }
            }
        }

        /// <summary>
        /// Gets or sets height of the control
        /// </summary>
        [HideInEditor, NoSerialize]
        public float Height
        {
            get => _bounds.Height;
            set
            {
                if (!Mathf.NearEqual(_bounds.Height, value))
                {
                    var size = new Vector2(_bounds.Width, value);
                    SetSizeInternal(ref size);
                }
            }
        }

        /// <summary>
        /// Gets or sets control's size
        /// </summary>
        [EditorDisplay("Transform"), EditorOrder(1010), Tooltip("The size of the control bounds.")]
        public Vector2 Size
        {
            get => _bounds.Size;
            set
            {
                if (!_bounds.Size.Equals(ref value))
                    SetSizeInternal(ref value);
            }
        }

        /// <summary>
        /// Gets Y coordinate of the top edge of the control relative to the upper-left corner of its container
        /// </summary>
        public float Top => _bounds.Top;

        /// <summary>
        /// Gets Y coordinate of the bottom edge of the control relative to the upper-left corner of its container
        /// </summary>
        public float Bottom => _bounds.Bottom;

        /// <summary>
        /// Gets X coordinate of the left edge of the control relative to the upper-left corner of its container
        /// </summary>
        public float Left => _bounds.Left;

        /// <summary>
        /// Gets X coordinate of the right edge of the control relative to the upper-left corner of its container
        /// </summary>
        public float Right => _bounds.Right;

        /// <summary>
        /// Gets position of the upper left corner of the control relative to the upper-left corner of its container
        /// </summary>
        public Vector2 UpperLeft => _bounds.UpperLeft;

        /// <summary>
        /// Gets position of the upper right corner of the control relative to the upper-left corner of its container
        /// </summary>
        public Vector2 UpperRight => _bounds.UpperRight;

        /// <summary>
        /// Gets position of the bottom right corner of the control relative to the upper-left corner of its container
        /// </summary>
        public Vector2 BottomRight => _bounds.BottomRight;

        /// <summary>
        /// Gets position of the bottom left of the control relative to the upper-left corner of its container
        /// </summary>
        public Vector2 BottomLeft => _bounds.BottomLeft;

        /// <summary>
        /// Gets center position of the control relative to the upper-left corner of its container
        /// </summary>
        public Vector2 Center => _bounds.Center;

        /// <summary>
        /// Gets or sets control's bounds rectangle
        /// </summary>
        [HideInEditor, NoSerialize]
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
        [EditorDisplay("Transform"), Limit(float.MinValue, float.MaxValue, 0.1f), EditorOrder(1020), Tooltip("The control scale parameter.")]
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
        /// Gets or sets the normalized pivot location (used to transform control around it). Point (0,0) is upper left corner, (0.5,0.5) is center, (1,1) is bottom left corner.
        /// </summary>
        [EditorDisplay("Transform"), Limit(0.0f, 1.0f, 0.1f), EditorOrder(1030), Tooltip("The control rotation pivot location in normalized control size. Point (0,0) is upper left corner, (0.5,0.5) is center, (1,1) is bottom left corner.")]
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
        /// Gets or sets the shear. Defined in degrees.
        /// </summary>
        [EditorDisplay("Transform"), EditorOrder(1040), Tooltip("The shear transform angles (x, y). Defined in degrees.")]
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
        [EditorDisplay("Transform"), EditorOrder(1050), Tooltip("The control rotation angle (in degrees).")]
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
            // Actual pivot and negative pivot
            Vector2 v1, v2;
            Vector2.Multiply(ref _pivot, ref _bounds.Size, out v1);
            Vector2.Negate(ref v1, out v2);
            Vector2.Add(ref v1, ref _bounds.Location, out v1);

            // ------ Matrix3x3 based version:

            /*
            // Negative pivot
            Matrix3x3 m1, m2;
            Matrix3x3.Translation2D(ref v2, out m1);

            // Scale
            Matrix3x3.Scaling(_scale.X, _scale.Y, 1, out m2);
            Matrix3x3.Multiply(ref m1, ref m2, out m1);

            // Shear
            Matrix3x3.Shear(ref _shear, out m2);
            Matrix3x3.Multiply(ref m1, ref m2, out m1);

            // Rotation
            Matrix3x3.RotationZ(_rotation * Mathf.DegreesToRadians, out m2);
            Matrix3x3.Multiply(ref m1, ref m2, out m1);

            // Pivot + Location
            Matrix3x3.Translation2D(ref v1, out m2);
            Matrix3x3.Multiply(ref m1, ref m2, out _cachedTransform);
            */

            // ------ Matrix2x2 based version:

            // 2D transformation
            Matrix2x2 m1, m2;
            Matrix2x2.Scale(ref _scale, out m1);
            Matrix2x2.Shear(ref _shear, out m2);
            Matrix2x2.Multiply(ref m1, ref m2, out m1);
            Matrix2x2.Rotation(Mathf.DegreesToRadians * _rotation, out m2);
            Matrix2x2.Multiply(ref m1, ref m2, out m1);

            // Mix all the stuff
            Matrix3x3 m3;
            Matrix3x3.Translation2D(ref v2, out m3);
            Matrix3x3 m4 = (Matrix3x3)m1;
            Matrix3x3.Multiply(ref m3, ref m4, out m3);
            Matrix3x3.Translation2D(ref v1, out m4);
            Matrix3x3.Multiply(ref m3, ref m4, out _cachedTransform);

            // Cache inverted transform
            Matrix3x3.Invert(ref _cachedTransform, out _cachedTransformInv);
        }
    }
}
