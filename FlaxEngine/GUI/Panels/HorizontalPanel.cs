////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

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
            float x = _leftMargin;
            float h = Height - _topMargin - _bottomMargin;
            for (int i = 0; i < _children.Count; i++)
            {
                Control c = _children[i];
                if (c.Visible)
                {
                    var w = c.Width;
                    c.Bounds = new Rectangle(x + _spacing, _topMargin, h, w);
                    x = c.Bottom;
                }
            }
            x += _rightMargin;

            // Update size
            Width = x;
        }
    }
}
