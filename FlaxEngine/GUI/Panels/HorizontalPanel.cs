// Copyright (c) 2012-2019 Wojciech Figat. All rights reserved.

namespace FlaxEngine.GUI
{
    /// <summary>
    /// This panel arranges child controls horizontally.
    /// </summary>
    /// <seealso cref="FlaxEngine.GUI.PanelWithMargins" />
    public class HorizontalPanel : PanelWithMargins
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HorizontalPanel"/> class.
        /// </summary>
        public HorizontalPanel()
        {
        }

        /// <inheritdoc />
        protected override void PerformLayoutSelf()
        {
            // Sort controls from left to right
            float x = _margin.Left;
            float h = Height - _margin.Height;
            bool hasAnyItem = false;
            for (int i = 0; i < _children.Count; i++)
            {
                Control c = _children[i];
                if (c.Visible)
                {
                    var w = c.Width;
                    c.Bounds = new Rectangle(x + _offset.X, _margin.Top + _offset.Y, h, w);
                    x = c.Right + _spacing;
                    hasAnyItem = true;
                }
            }
            if (hasAnyItem)
                x -= _spacing;
            x += _margin.Right;

            // Update size
            if (_autoSize)
                Width = x;
        }
    }
}
