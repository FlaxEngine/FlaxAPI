// Copyright (c) 2012-2019 Wojciech Figat. All rights reserved.

namespace FlaxEngine.GUI
{
    /// <summary>
    /// This panel arranges child controls vertically.
    /// </summary>
    /// <seealso cref="FlaxEngine.GUI.PanelWithMargins" />
    public class VerticalPanel : PanelWithMargins
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VerticalPanel"/> class.
        /// </summary>
        public VerticalPanel()
        {
        }

        /// <inheritdoc />
        protected override void PerformLayoutSelf()
        {
            // Sort controls from top to bottom
            float y = _margin.Top;
            float w = Width - _margin.Width;
            bool hasAnyItem = false;
            for (int i = 0; i < _children.Count; i++)
            {
                Control c = _children[i];
                if (c.Visible)
                {
                    var h = c.Height;
                    c.Bounds = new Rectangle(_margin.Left + _offset.X, y + _offset.Y, w, h);
                    y = c.Bottom + _spacing;
                    hasAnyItem = true;
                }
            }
            if (hasAnyItem)
                y -= _spacing;
            y += _margin.Bottom;

            // Update size
            if (_autoSize)
                Height = y;
        }
    }
}
