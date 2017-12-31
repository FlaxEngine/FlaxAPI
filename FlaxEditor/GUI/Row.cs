////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using FlaxEngine;
using FlaxEngine.GUI;

namespace FlaxEditor.GUI
{
    /// <summary>
    /// Single row control for <see cref="Table"/>.
    /// </summary>
    /// <seealso cref="FlaxEngine.GUI.Control" />
    public class Row : Control
    {
        private Table _table;

        /// <summary>
        /// Gets the parent table that owns this row.
        /// </summary>
        public Table Table => _table;

        /// <summary>
        /// Gets or sets the cell values.
        /// </summary>
        public object[] Values { get; set; }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="Row"/> class.
        /// </summary>
        /// <param name="height">The height.</param>
        public Row(float height = 16)
            : base(0, 0, 100, height)
        {
        }

        /// <inheritdoc />
        public override void Draw()
        {
            base.Draw();

            var style = Style.Current;

            if (IsMouseOver)
            {
                Render2D.FillRectangle(new Rectangle(Vector2.Zero, Size), style.BackgroundHighlighted * 0.1f, true);
            }

            if (Values != null && _table?.Columns != null)
            {
                float x = 0;
                int end = Mathf.Min(Values.Length, _table.Columns.Length);
                for (int i = 0; i < end; i++)
                {
                    var column = _table.Columns[i];
                    var value = Values[i];
                    var width = _table.GetColumnWidth(i);

                    string text;
                    if (value == null)
                        text = string.Empty;
                    else if (column.FormatValue != null)
                        text = column.FormatValue(value);
                    else
                        text = value.ToString();

                    var rect = new Rectangle(x, 0, width, Height);

                    Render2D.DrawText(style.FontMedium, text, rect, Color.White, TextAlignment.Far, TextAlignment.Center);

                    x += width;
                }
            }
        }

        /// <inheritdoc />
        protected override void OnParentChangedInternal()
        {
            base.OnParentChangedInternal();

            _table = Parent as Table;
        }
    }
}
