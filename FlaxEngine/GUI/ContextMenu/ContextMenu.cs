////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Linq;

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
        /// The items container.
        /// </summary>
        /// <seealso cref="FlaxEngine.GUI.Panel" />
        protected class ItemsPanel : Panel
        {
            private readonly ContextMenu _menu;

            /// <summary>
            /// Initializes a new instance of the <see cref="ItemsPanel"/> class.
            /// </summary>
            /// <param name="menu">The menu.</param>
            public ItemsPanel(ContextMenu menu)
                : base(ScrollBars.Vertical)
            {
                _menu = menu;
            }

            /// <inheritdoc />
            protected override void Arrage()
            {
                base.Arrage();

                // Arrange controls
                Margin margin = _menu._itemsMargin;
                float y = margin.Top;
                float x = margin.Left;
                float width = Width - margin.Width;
                for (int i = 0; i < _children.Count; i++)
                {
                    if (_children[i] is ContextMenuItem item && item.Visible)
                    {
                        var hight = item.Height;
                        item.Bounds = new Rectangle(x, y, width, hight);
                        y += hight + margin.Height;
                    }
                }
            }
        }

        /// <summary>
        /// The items area margin.
        /// </summary>
        protected Margin _itemsAreaMargin = new Margin(0, 0, 2, 2);

        /// <summary>
        /// The items margin.
        /// </summary>
        protected Margin _itemsMargin = new Margin(16, 0, 2, 0);

        /// <summary>
        /// The items panel.
        /// </summary>
        protected ItemsPanel _panel;

        /// <summary>
        /// Event fired when any button in this popup menu gets clicked.
        /// </summary>
        public event OnContextMenuButtonClicked OnButtonClicked;

        /// <summary>
        /// Gets or sets the items area margin (items container area margin).
        /// </summary>
        public Margin ItemsAreaMargin
        {
            get => _itemsAreaMargin;
            set
            {
                _itemsAreaMargin = value;
                PerformLayout();
            }
        }

        /// <summary>
        /// Gets or sets the items margin.
        /// </summary>
        public Margin ItemsMargin
        {
            get => _itemsMargin;
            set
            {
                _itemsMargin = value;
                PerformLayout();
            }
        }

        /// <summary>
        /// Gets or sets the minimum popup width.
        /// </summary>
        public float MinimumWidth { get; set; }

        /// <summary>
        /// Gets or sets the maximum amount of items in the view. If popup has more items to show it uses a additional scroll panel.
        /// </summary>
        public int MaximumItemsInViewCount { get; set; }

        /// <summary>
        /// Gets the items (readonly).
        /// </summary>
        public IEnumerable<ContextMenuItem> Items => _panel.Children.OfType<ContextMenuItem>();

        /// <summary>
        /// Initializes a new instance of the <see cref="ContextMenu"/> class.
        /// </summary>
        public ContextMenu()
        {
            MinimumWidth = 10;
            MaximumItemsInViewCount = 1000000;

            _panel = new ItemsPanel(this);
            _panel.Parent = this;
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
            item.Parent = _panel;
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
            item.Parent = _panel;
            return item;
        }

        /// <summary>
        /// Adds the button.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="clicked">On button clicked event.</param>
        /// <returns>Created context menu item control.</returns>
        public ContextMenuButton AddButton(string text, Action clicked)
        {
            var item = new ContextMenuButton(this, -1, text);
            item.Parent = _panel;
            item.Clicked += clicked;
            return item;
        }

        /// <summary>
        /// Adds the button.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="shortkeys">The shortkeys.</param>
        /// <param name="clicked">On button clicked event.</param>
        /// <returns>Created context menu item control.</returns>
        public ContextMenuButton AddButton(string text, string shortkeys, Action clicked)
        {
            var item = new ContextMenuButton(this, -1, text, shortkeys);
            item.Parent = _panel;
            item.Clicked += clicked;
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
            item.Parent = _panel;
            return item;
        }

        /// <summary>
        /// Adds the separator.
        /// </summary>
        /// <returns>Created context menu item control.</returns>
        public ContextMenuSeparator AddSeparator()
        {
            var item = new ContextMenuSeparator(this);
            item.Parent = _panel;
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

            for (int i = 0; i < _panel.Children.Count; i++)
            {
                if (_panel.Children[i] is ContextMenuButton button && button.ID == id)
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
            for (int i = 0; i < _panel.Children.Count; i++)
            {
                if (_panel.Children[i].ContainsPoint(ref cLocation))
                    return true;
            }

            return false;
        }

        /// <inheritdoc />
        protected override void PerformLayoutSelf()
        {
            var prevSize = Size;
            
            // Calculate size of the context menu (items only)
            float maxWidth = 0;
            float height = _itemsAreaMargin.Height;
            int itemsLeft = MaximumItemsInViewCount;
            for (int i = 0; i < _panel.Children.Count; i++)
            {
                if (_panel.Children[i] is ContextMenuItem item && item.Visible)
                {
                    if (itemsLeft > 0)
                    {
                        height += item.Height + _itemsMargin.Height;
                        itemsLeft--;
                    }
                    maxWidth = Mathf.Max(maxWidth, item.MinimumWidth);
                }
            }
            maxWidth = Mathf.Max(maxWidth + 20, MinimumWidth);

            // Resize container
            Size = new Vector2(Mathf.Ceil(maxWidth), Mathf.Ceil(height));

            // Arrange items view panel
            var panelBounds = new Rectangle(Vector2.Zero, Size);
            _itemsAreaMargin.ShrinkRectangle(ref panelBounds);
            _panel.Bounds = panelBounds;

            // Check if is visible size get changed
            if (Visible && prevSize != Size)
            {
                // Update window dimensions
                UpdateWindowSize();
            }
        }
    }
}
