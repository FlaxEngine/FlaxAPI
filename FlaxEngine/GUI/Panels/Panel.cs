// Flax Engine scripting API

using System;

namespace FlaxEngine.GUI
{
    /// <summary>
    /// Panel UI control.
    /// </summary>
    /// <seealso cref="FlaxEngine.GUI.ContainerControl" />
    public class Panel : ContainerControl
    {
        /// <summary>
        /// The scroll right corner. Used to scroll contents of the panel control.
        /// </summary>
        protected Vector2 _scrollRightCorner;

        /// <summary>
        /// The vertical scroll bar.
        /// </summary>
        public readonly VScrollBar VScrollBar;

        /// <summary>
        /// The horizontal scroll bar.
        /// </summary>
        public readonly HScrollBar HScrollBar;

        /// <summary>
        /// Gets the scrolling right corner.
        /// </summary>
        /// <value>
        /// The scrolling right corner.
        /// </value>
        public Vector2 ScrollRightCorner
        {
            get { return _scrollRightCorner; }
            internal set { _scrollRightCorner = value; }
        }

        /// <summary>
        /// Gets the view bottom.
        /// </summary>
        /// <value>
        /// The view bottom.
        /// </value>
        public Vector2 ViewBottom => Size + _viewOffset;

        /// <summary>
        /// Initializes a new instance of the <see cref="Panel"/> class.
        /// </summary>
        /// <param name="scrollBars">The scroll bars.</param>
        public Panel(ScrollBars scrollBars)
            : base(false)
        {
            // Create scroll bars
            if ((scrollBars & ScrollBars.Vertical) == ScrollBars.Vertical)
            {
                // Create vertical sroll bar
                VScrollBar = new VScrollBar(Width - ScrollBar.DefaultSize, Height);
                VScrollBar.Parent = this;
            }
            if ((scrollBars & ScrollBars.Horizontal) == ScrollBars.Horizontal)
            {
                // Create vertical sroll bar
                HScrollBar = new HScrollBar(Height - ScrollBar.DefaultSize, Width);
                HScrollBar.Parent = this;
            }
        }

        /// <summary>
        /// Scrolls the view to the given control area.
        /// </summary>
        /// <param name="c">The control.</param>
        public void ScrollViewTo(Control c)
        {
            if(c == null)
                throw new ArgumentNullException();

            Vector2 location = c.Location;
            Vector2 size = c.Size;
            while (c.HasParent && c.Parent != this)
            {
                c = c.Parent;
                location = c.PointToParent(location);
            }

            if (c.HasParent)
            {
                ScrollViewTo(new Rectangle(location, size));
            }
        }

        /// <summary>
        /// Scrolls the view to the given location.
        /// </summary>
        /// <param name="location">The location.</param>
        public void ScrollViewTo(Vector2 location)
        {
            ScrollViewTo(new Rectangle(location, Vector2.Zero));
        }

        /// <summary>
        /// Scrolls the view to the given area.
        /// </summary>
        /// <param name="bounds">The bounds.</param>
        public void ScrollViewTo(Rectangle bounds)
        {
            bool wasLocked = IsLayoutLocked;
            IsLayoutLocked = true;

            if (HScrollBar != null && HScrollBar.Visible)
                HScrollBar.ScrollViewTo(bounds.Left, bounds.Right);
            if (VScrollBar != null && VScrollBar.Visible)
                VScrollBar.ScrollViewTo(bounds.Top, bounds.Bottom);

            IsLayoutLocked = wasLocked;
            PerformLayout();
        }

        internal void setViewOffset(Orientation orientation, float value)
        {
            if (orientation == Orientation.Vertical)
                _viewOffset.Y = -value;
            else
                _viewOffset.X = -value;
        }
    }
}
