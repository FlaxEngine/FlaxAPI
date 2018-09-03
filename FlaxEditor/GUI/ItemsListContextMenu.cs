// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System;
using System.Collections.Generic;
using FlaxEditor.Utilities;
using FlaxEngine;
using FlaxEngine.GUI;

namespace FlaxEditor.GUI
{
    /// <summary>
    /// The custom context menu that shows a list of items and supports searching by name and query results highlighting.
    /// </summary>
    /// <seealso cref="ContextMenuBase" />
    public class ItemsListContextMenu : ContextMenuBase
    {
        /// <summary>
        /// The single list item control.
        /// </summary>
        /// <seealso cref="FlaxEngine.GUI.Control" />
        public class Item : Control
        {
            /// <summary>
            /// The is mouse down flag.
            /// </summary>
            protected bool _isMouseDown;

            /// <summary>
            /// The search query highlights.
            /// </summary>
            protected List<Rectangle> _highlights;

            /// <summary>
            /// Gets or sets the name.
            /// </summary>
            public string Name { get; set; }

            /// <summary>
            /// Initializes a new instance of the <see cref="Item"/> class.
            /// </summary>
            public Item()
            : base(0, 0, 120, 12)
            {
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="Item"/> class.
            /// </summary>
            /// <param name="name">The item name.</param>
            /// <param name="tag">The item tag object.</param>
            public Item(string name, object tag = null)
            : base(0, 0, 120, 12)
            {
                Name = name;
                Tag = tag;
            }

            /// <summary>
            /// Updates the filter.
            /// </summary>
            /// <param name="filterText">The filter text.</param>
            public void UpdateFilter(string filterText)
            {
                if (string.IsNullOrWhiteSpace(filterText))
                {
                    // Clear filter
                    _highlights?.Clear();
                    Visible = true;
                }
                else
                {
                    QueryFilterHelper.Range[] ranges;
                    if (QueryFilterHelper.Match(filterText, Name, out ranges))
                    {
                        // Update highlights
                        if (_highlights == null)
                            _highlights = new List<Rectangle>(ranges.Length);
                        else
                            _highlights.Clear();
                        var style = Style.Current;
                        var font = style.FontSmall;
                        for (int i = 0; i < ranges.Length; i++)
                        {
                            var start = font.GetCharPosition(Name, ranges[i].StartIndex);
                            var end = font.GetCharPosition(Name, ranges[i].EndIndex);
                            _highlights.Add(new Rectangle(start.X + 2, 0, end.X - start.X, Height));
                        }

                        Visible = true;
                    }
                    else
                    {
                        // Hide
                        _highlights?.Clear();
                        Visible = false;
                    }
                }
            }

            /// <summary>
            /// Gets the text rectangle.
            /// </summary>
            /// <param name="rect">The output rectangle.</param>
            protected virtual void GetTextRect(out Rectangle rect)
            {
                rect = new Rectangle(2, 0, Width - 4, Height);
            }

            /// <inheritdoc />
            public override void Draw()
            {
                var style = Style.Current;
                GetTextRect(out var textRect);

                // Overlay
                if (IsMouseOver)
                    Render2D.FillRectangle(new Rectangle(Vector2.Zero, Size), style.BackgroundHighlighted);

                // Draw all highlights
                if (_highlights != null)
                {
                    var color = style.ProgressNormal * 0.6f;
                    for (int i = 0; i < _highlights.Count; i++)
                    {
                        var rect = _highlights[i];
                        rect.Location += textRect.Location;
                        rect.Height = textRect.Height;
                        Render2D.FillRectangle(rect, color);
                    }
                }

                // Draw name
                Render2D.DrawText(style.FontSmall, Name, textRect, Enabled ? style.Foreground : style.ForegroundDisabled, TextAlignment.Near, TextAlignment.Center);
            }

            /// <inheritdoc />
            public override bool OnMouseDown(Vector2 location, MouseButton buttons)
            {
                if (buttons == MouseButton.Left)
                {
                    _isMouseDown = true;
                }

                return base.OnMouseDown(location, buttons);
            }

            /// <inheritdoc />
            public override bool OnMouseUp(Vector2 location, MouseButton buttons)
            {
                if (buttons == MouseButton.Left && _isMouseDown)
                {
                    _isMouseDown = false;
                    // More parents...
                    ((ItemsListContextMenu)Parent.Parent.Parent).OnClickItem(this);
                }

                return base.OnMouseUp(location, buttons);
            }

            /// <inheritdoc />
            public override void OnMouseLeave()
            {
                _isMouseDown = false;

                base.OnMouseLeave();
            }

            /// <inheritdoc />
            public override int Compare(Control other)
            {
                if (other is Item otherItem)
                    return String.Compare(Name, otherItem.Name, StringComparison.Ordinal);
                return base.Compare(other);
            }
        }

        private readonly TextBox _searchBox;
        private bool _waitingForInput;

        /// <summary>
        /// Event fired when any item in this popup menu gets clicked.
        /// </summary>
        public event Action<Item> ItemClicked;

        /// <summary>
        /// The panel control where you should add your items.
        /// </summary>
        public readonly VerticalPanel ItemsPanel;

        /// <summary>
        /// Initializes a new instance of the <see cref="ItemsListContextMenu"/> class.
        /// </summary>
        /// <param name="width">The control width.</param>
        /// <param name="height">The control height.</param>
        public ItemsListContextMenu(float width = 320, float height = 220)
        {
            // Context menu dimensions
            Size = new Vector2(width, height);

            // Search box
            _searchBox = new TextBox(false, 1, 1)
            {
                Width = Width - 3,
                WatermarkText = "Search...",
                Parent = this
            };
            _searchBox.TextChanged += OnSearchFilterChanged;

            // Create first panel (for scrollbar)
            var panel1 = new Panel(ScrollBars.Vertical)
            {
                Bounds = new Rectangle(0, _searchBox.Bottom + 1, Width, Height - _searchBox.Bottom - 2),
                Parent = this
            };

            // Create second panel (for items arrangement)
            ItemsPanel = new VerticalPanel
            {
                DockStyle = DockStyle.Top,
                IsScrollable = true,
                Parent = panel1
            };
        }

        private void OnSearchFilterChanged()
        {
            // Skip events during setup or init stuff
            if (IsLayoutLocked)
                return;

            // Update items
            var items = ItemsPanel.Children;
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i] is Item item)
                    item.UpdateFilter(_searchBox.Text);
            }

            PerformLayout();
            _searchBox.Focus();
        }

        /// <summary>
        /// Called when user clicks on an item.
        /// </summary>
        /// <param name="item">The item.</param>
        public void OnClickItem(Item item)
        {
            Hide();
            ItemClicked?.Invoke(item);
        }

        /// <summary>
        /// Resets the view.
        /// </summary>
        public void ResetView()
        {
            bool wasLayoutLocked = IsLayoutLocked;
            IsLayoutLocked = true;

            var items = ItemsPanel.Children;
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i] is Item item)
                    item.UpdateFilter(null);
            }

            _searchBox.Clear();
            IsLayoutLocked = wasLayoutLocked;
            PerformLayout();
        }

        /// <inheritdoc />
        protected override void OnShow()
        {
            // Prepare
            ResetView();
            Focus();
            _waitingForInput = true;

            base.OnShow();
        }

        /// <inheritdoc />
        public override void Hide()
        {
            Focus(null);

            base.Hide();
        }

        /// <inheritdoc />
        public override bool OnKeyDown(Keys key)
        {
            if (key == Keys.Escape)
            {
                Hide();
                return true;
            }

            if (_waitingForInput)
            {
                _waitingForInput = false;
                _searchBox.Focus();
                return _searchBox.OnKeyDown(key);
            }

            return base.OnKeyDown(key);
        }
    }
}
