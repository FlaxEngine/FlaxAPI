// Copyright (c) 2012-2019 Wojciech Figat. All rights reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using FlaxEngine;
using FlaxEngine.GUI;

namespace FlaxEditor.GUI.ContextMenu
{
    /// <summary>
    /// Popup menu control.
    /// </summary>
    /// <seealso cref="ContextMenuBase" />
    [HideInEditor]
    public class ContextMenu : ContextMenuBase
    {
        /// <summary>
        /// The items container.
        /// </summary>
        /// <seealso cref="FlaxEngine.GUI.Panel" />
        [HideInEditor]
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
            protected override void Arrange()
            {
                base.Arrange();

                // Arrange controls
                Margin margin = _menu._itemsMargin;
                float y = margin.Top;
                float x = margin.Left;
                float width = Width - margin.Width;
                for (int i = 0; i < _children.Count; i++)
                {
                    if (_children[i] is ContextMenuItem item && item.Visible)
                    {
                        var height = item.Height;
                        item.Bounds = new Rectangle(x, y, width, height);
                        y += height + margin.Height;
                    }
                }
            }
        }

        /// <summary>
        /// The items area margin.
        /// </summary>
        protected Margin _itemsAreaMargin = new Margin(0, 0, 3, 3);

        /// <summary>
        /// The items margin.
        /// </summary>
        protected Margin _itemsMargin = new Margin(16, 0, 2, 0);

        /// <summary>
        /// The items panel.
        /// </summary>
        protected ItemsPanel _panel;

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
        /// Event fired when user clicks on the button.
        /// </summary>
        public event Action<ContextMenuButton> ButtonClicked;

        /// <summary>
        /// Gets the context menu items container control.
        /// </summary>
        public ContainerControl ItemsContainer => _panel;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContextMenu"/> class.
        /// </summary>
        public ContextMenu()
        {
            MinimumWidth = 10;
            MaximumItemsInViewCount = 20;

            _panel = new ItemsPanel(this)
            {
                ClipChildren = false,
                Parent = this,
            };
        }

        /// <summary>
        /// Removes all the added items (buttons, separators, etc.).
        /// </summary>
        public void DisposeAllItems()
        {
            for (int i = _panel.ChildrenCount - 1; _panel.ChildrenCount > 0 && i >= 0; i--)
            {
                if (_panel.Children[i] is ContextMenuItem)
                    _panel.Children[i].Dispose();
            }
        }

        /// <summary>
        /// Adds the button.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns>Created context menu item control.</returns>
        public ContextMenuButton AddButton(string text)
        {
            var item = new ContextMenuButton(this, text);
            item.Parent = _panel;
            return item;
        }

        /// <summary>
        /// Adds the button.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="shortKeys">The short keys.</param>
        /// <returns>Created context menu item control.</returns>
        public ContextMenuButton AddButton(string text, string shortKeys)
        {
            var item = new ContextMenuButton(this, text, shortKeys);
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
            var item = new ContextMenuButton(this, text);
            item.Parent = _panel;
            item.Clicked += clicked;
            return item;
        }

        /// <summary>
        /// Adds the button.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="clicked">On button clicked event.</param>
        /// <returns>Created context menu item control.</returns>
        public ContextMenuButton AddButton(string text, Action<ContextMenuButton> clicked)
        {
            var item = new ContextMenuButton(this, text);
            item.Parent = _panel;
            item.ButtonClicked += clicked;
            return item;
        }

        /// <summary>
        /// Adds the button.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="shortKeys">The shortKeys.</param>
        /// <param name="clicked">On button clicked event.</param>
        /// <returns>Created context menu item control.</returns>
        public ContextMenuButton AddButton(string text, string shortKeys, Action clicked)
        {
            var item = new ContextMenuButton(this, text, shortKeys);
            item.Parent = _panel;
            item.Clicked += clicked;
            return item;
        }

        /// <summary>
        /// Gets the child menu (with that name).
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns>Created context menu item control or null if missing.</returns>
        public ContextMenuChildMenu GetChildMenu(string text)
        {
            for (int i = 0; i < _panel.ChildrenCount; i++)
            {
                if (_panel.Children[i] is ContextMenuChildMenu menu && menu.Text == text)
                    return menu;
            }

            return null;
        }

        /// <summary>
        /// Adds the child menu or gets it if already created (with that name).
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns>Created context menu item control.</returns>
        public ContextMenuChildMenu GetOrAddChildMenu(string text)
        {
            var item = GetChildMenu(text);
            if (item == null)
            {
                item = new ContextMenuChildMenu(this, text);
                item.Parent = _panel;
            }

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
        /// Called when button get clicked.
        /// </summary>
        /// <param name="button">The button.</param>
        public virtual void OnButtonClicked(ContextMenuButton button)
        {
            ButtonClicked?.Invoke(button);
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
