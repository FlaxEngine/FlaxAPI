////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

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
            // Sort controls from up to down
            float y = _topMargin;
            float w = Width - _leftMargin - _rightMargin;
            for (int i = 0; i < _children.Count; i++)
            {
                Control c = _children[i];
                if (c.Visible)
                {
                    var h = c.Height;
                    c.Bounds = new Rectangle(_leftMargin + _offset.X, y + _spacing + _offset.Y, w, h);
                    y = c.Bottom;
                }
            }
            y += _bottomMargin;

            // Update size
            Height = y;
        }
    }
}
