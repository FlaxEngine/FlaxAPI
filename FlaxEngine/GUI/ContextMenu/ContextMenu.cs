////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

namespace FlaxEngine.GUI
{
    /// <summary>
    /// Ebent for context menu buttons click actions handling.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <param name="contextMenu">The context menu.</param>
    public delegate void OnContextMenuButtonClicked(int id, ContextMenu contextMenu);

    /// <summary>
    /// Popup menu control.
    /// </summary>
    /// <seealso cref="FlaxEngine.GUI.ContextMenuBase" />
    public class ContextMenu : ContextMenuBase
    {
        /// <summary>
        /// Gets or sets the minimum width.
        /// </summary>
        /// <value>
        /// The minimum width.
        /// </value>
        public float MinimumWidth { get; set; }

        /// <summary>
        /// Event fired when any button in this popup menu gets clicked.
        /// </summary>
        public event OnContextMenuButtonClicked OnButtonClicked;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContextMenu"/> class.
        /// </summary>
        public ContextMenu()
        {
            MinimumWidth = 10;
        }

        /// <summary>
        /// Adds the button.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="text">The text.</param>
        /// <returns>Created context menu item control.</returns>
        public ContextMenuButton AddButton(int id, string text)
        {
            var item = new ContextMenuButton(this, id, text);
            item.Parent = this;
            return item;
        }

        /// <summary>
        /// Adds the button.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="text">The text.</param>
        /// <param name="shortkeys">The shortkeys.</param>
        /// <returns>Created context menu item control.</returns>
        public ContextMenuButton AddButton(int id, string text, string shortkeys)
        {
            var item = new ContextMenuButton(this, id, text, shortkeys);
            item.Parent = this;
            return item;
        }

        /// <summary>
        /// Adds the child menu.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns>Created context menu item control.</returns>
        public ContextMenuChildMenu AddChildMenu(string text)
        {
            var item = new ContextMenuChildMenu(this, text);
            item.Parent = this;
            return item;
        }

        /// <summary>
        /// Adds the separator.
        /// </summary>
        /// <returns>Created context menu item control.</returns>
        public ContextMenuSeparator AddSeparator()
        {
            var item = new ContextMenuSeparator(this);
            item.Parent = this;
            return item;
        }

        /// <summary>
        /// Tries to find button with the given ID.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Button or null if cannot find it.</returns>
        public ContextMenuButton GetButton(int id)
        {
            // TODO: could we provide some fast access cache for using ids?

            for (int i = 0; i < _children.Count; i++)
            {
                if (_children[i] is ContextMenuButton button && button.ID == id)
                    return button;
            }

            return null;
        }

        internal void OnButtonClicked_Internal(ContextMenuButton button)
        {
            OnButtonClicked?.Invoke(button.ID, this);
        }

        /// <inheritdoc />
        public override bool ContainsPoint(ref Vector2 location)
        {
            if (base.ContainsPoint(ref location))
                return true;

            Vector2 cLocation = location - Location;
            for (int i = 0; i < _children.Count; i++)
            {
                if (_children[i].ContainsPoint(ref cLocation))
                    return true;
            }

            return false;
        }

        /// <inheritdoc />
        protected override void PerformLayoutSelf()
        {
            var prevSize = Size;

            // Calculate size of the context menu
            float maxWidth = 0;
            float height = DefaultItemsMargin * 2;
            for (int i = 0; i < _children.Count; i++)
            {
                var c = _children[i];
                if (c.Visible)
                {
                    Vector2 itemSize = c.Size;
                    height += itemSize.Y + DefaultItemsMargin;
                    maxWidth = Mathf.Max(maxWidth, itemSize.X);
                }
            }
            maxWidth = Mathf.Max(maxWidth + 20, MinimumWidth);

            // Resize container
            Size = new Vector2(Mathf.Ceil(maxWidth), Mathf.Ceil(height));

            // Arrange controls
            float y = DefaultItemsMargin;
            float x = DefaultItemsMargin + DefaultItemsLeftMargin;
            float width = Width - x - DefaultItemsMargin;
            for (int i = 0; i < _children.Count; i++)
            {
                if (_children[i] is ContextMenuItem item && item.Visible)
                {
                    var hight = item.Height;
                    item.Bounds = new Rectangle(x, y, width, hight);
                    y += hight + DefaultItemsMargin;
                }
            }

            // Check if is visible size get changed
            if (Visible && prevSize != Size)
            {
                // Update window dimensions
                UpdateWindowSize();
            }
        }
    }
}
