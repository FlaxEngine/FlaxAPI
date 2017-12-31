////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Runtime.CompilerServices;
using FlaxEngine;
using FlaxEngine.GUI;

namespace FlaxEditor.GUI
{
    /// <summary>
    /// Table control with columns and rows.
    /// </summary>
    /// <seealso cref="FlaxEngine.GUI.ContainerControl" />
    public class Table : ContainerControl
    {
        private float _headerHeight = 20;
        private ColumnDefinition[] _columns;
        private float[] _splits;

        /// <summary>
        /// Gets or sets the height of the table column headers.
        /// </summary>
        public float HeaderHeight
        {
            get => _headerHeight;
            set
            {
                value = Mathf.Max(value, 1);
                if (!Mathf.NearEqual(value, _headerHeight))
                {
                    _headerHeight = value;
                    PerformLayout();
                }
            }
        }

        /// <summary>
        /// Gets or sets the columns description.
        /// </summary>
        public ColumnDefinition[] Columns
        {
            get => _columns;
            set
            {
                _columns = value;

                if (_columns == null)
                {
                    // Unlink splits
                    _splits = null;
                }
                else
                {
                    // Set the default values for the splits
                    _splits = new float[_columns.Length];
                    float split = 1.0f / _columns.Length;
                    for (int i = 0; i < _splits.Length; i++)
                        _splits[i] = split;
                }

                PerformLayout();
            }
        }

        /// <summary>
        /// The column split values. Specified per column as normalized value to range [0;1]. Actual column width is calculated by multiplication of split value and table width.
        /// </summary>
        public float[] Splits
        {
            get => _splits;
            set
            {
                if (_columns == null)
                    return;
                if (value == null || value.Length != _columns.Length)
                    throw new ArgumentException();

                _splits = value;
                PerformLayout();
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Table"/> class.
        /// </summary>
        public Table()
        {
            _performChildrenLayoutFirst = true;
        }

        /// <summary>
        /// Gets the actual column width (in pixels).
        /// </summary>
        /// <param name="columnIndex">Zero-based index of the column.</param>
        /// <returns>The column width in pixels.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public float GetColumnWidth(int columnIndex)
        {
            return _splits[columnIndex] * Width;
        }

        /// <inheritdoc />
        public override void Draw()
        {
            base.Draw();

            if (_columns != null && _splits != null)
            {
                Rectangle rect = new Rectangle(0, 0, 0, _headerHeight);
                for (int i = 0; i < _columns.Length; i++)
                {
                    rect.Width = GetColumnWidth(i);
                    DrawColumn(ref rect, i);
                    rect.X += rect.Width;
                }
            }
        }

        /// <summary>
        /// Draws the column.
        /// </summary>
        /// <param name="rect">The the header area rectangle.</param>
        /// <param name="columnIndex">The zero-based index of the column.</param>
        protected virtual void DrawColumn(ref Rectangle rect, int columnIndex)
        {
            var column = _columns[columnIndex];

            if (column.TitleBackgroundColor.A > 0)
                Render2D.FillRectangle(rect, column.TitleBackgroundColor, true);

            var font = column.TitleFont ?? Style.Current.FontMedium;
            Render2D.DrawText(font, column.Title, rect, column.TitleColor, TextAlignment.Center, TextAlignment.Center);
        }

        /// <inheritdoc />
        protected override void PerformLayoutSelf()
        {
            base.PerformLayoutSelf();

            // Arrange rows
            float y = _headerHeight;
            for (int i = 0; i < Children.Count; i++)
            {
                var c = Children[i];
                var bounds = c.Bounds;
                bounds.Width = Width;
                bounds.Y = y;
                c.Bounds = bounds;
                y += bounds.Height + 1;
            }

            Height = y;
        }
    }
}
