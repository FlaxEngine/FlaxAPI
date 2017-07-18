////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;

namespace FlaxEngine.GUI
{
    /// <summary>
    /// Combo box control allows to choose one item from the provided collection of options.
    /// </summary>
    /// <seealso cref="FlaxEngine.GUI.Control" />
    public class ComboBox : Control
    {
        private const float DefaultHeight = 18.0f;

        protected readonly List<string> _items = new List<string>();
        protected ContextMenu _popupMenu;
        protected bool _mouseDown, _blockPopup;
        protected int _seletedIndex = -1;

        /// <summary>
        /// True if sort items before showing the list, otherwise present them in the unchanged order.
        /// </summary>
        public bool Sorted { get; set; }

        /// <summary>
        /// Event fired when selected index gets changed.
        /// </summary>
        public event Action<ComboBox> OnSelectedIndexChanged;

        /// <summary>
        /// Gets a value indicating whether this popup menu is opened.
        /// </summary>
        /// <value>
        ///   <c>true</c> if popup is opened; otherwise, <c>false</c>.
        /// </value>
        public bool IsPopupOpened => _popupMenu != null && _popupMenu.IsOpened;

        /// <summary>
        /// Gets or sets the selected item (returns <see cref="string.Empty"/> if no item is being selected).
        /// </summary>
        /// <value>
        /// The selected item.
        /// </value>
        public string SelectedItem
        {
            get => _seletedIndex != -1 ? _items[_seletedIndex] : string.Empty;
            set => SelectedIndex = _items.IndexOf(value);
        }

        /// <summary>
        /// Gets or sets the index of the selected.
        /// </summary>
        /// <value>
        /// The index of the selected.
        /// </value>
        public int SelectedIndex
        {
            get => _seletedIndex;
            set
            {
                // Clamp index
                value = Mathf.Clamp(value, value, _items.Count - 1);

                // Check if index will change
                if (value != _seletedIndex)
                {
                    // Select
                    _seletedIndex = value;
                    SelectedIndexChanged();
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ComboBox"/> class.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="width">The width.</param>
        public ComboBox(float x, float y, float width = 120.0f)
            : base(true, x, y, width, DefaultHeight)
        {
        }

        /// <summary>
        /// Clears the items.
        /// </summary>
        public void ClearItems()
        {
            _items.Clear();
        }

        /// <summary>
        /// Adds the item.
        /// </summary>
        /// <param name="item">The item.</param>
        public void AddItem(string item)
        {
            _items.Add(item);
        }

        /// <summary>
        /// Adds the items.
        /// </summary>
        /// <param name="items">The items.</param>
        public void AddItems(IEnumerable<string> items)
        {
            _items.AddRange(items);
        }

        /// <summary>
        /// Sets the items.
        /// </summary>
        /// <param name="items">The items.</param>
        public void SetItems(IEnumerable<string> items)
        {
            SelectedIndex = -1;
            _items.Clear();
            _items.AddRange(items);
        }

        /// <summary>
        /// Called when selected item index gets changed.
        /// </summary>
        protected virtual void SelectedIndexChanged()
        {
            OnSelectedIndexChanged?.Invoke(this);
        }

        /// <summary>
        /// Creates the popup menu.
        /// </summary>
        protected virtual ContextMenu CreatePopup()
        {
            var popup = new ContextMenu();

            // Bind events
            popup.OnVisibleChanged += cm =>
            {
                var win = ParentWindow;
                _blockPopup = win != null && new Rectangle(Vector2.Zero, Size).Contains(PointFromWindow(win.MousePosition));
                if (!_blockPopup)
                    Focus();
            };
            popup.OnButtonClicked += (id, cm) =>
            {
                SelectedIndex = id;
                cm.Hide();
            };

            return popup;
        }

        /// <inheritdoc />
        public override void OnDestroy()
        {
            if (_popupMenu != null)
            {
                _popupMenu.Hide();
                _popupMenu.Dispose();
                _popupMenu = null;
            }

            base.OnDestroy();
        }

        /// <inheritdoc />
        public override void Draw()
        {
            base.Draw();

            // Cache data
            var style = Style.Current;
            var clientRect = new Rectangle(Vector2.Zero, Size);
            float margin = clientRect.Height * 0.2f;
            float boxSize = clientRect.Height - margin * 2;
            float arrowOpacity = IsMouseOver ? 1.0f : 0.6f;
            bool isOpened = IsPopupOpened;

            // Background
            Color backgroundColor = style.BackgroundNormal;
            Color borderColor = style.BorderNormal;
            if (!Enabled)
            {
                backgroundColor *= 0.4f;
                arrowOpacity = 0.4f;
            }
            else if (IsMouseOver || _mouseDown || isOpened)
            {
                borderColor = style.BorderSelected;
            }
            Render2D.FillRectangle(clientRect, backgroundColor);
            Render2D.DrawRectangle(clientRect, borderColor);

            // Check if has selected item
            if (_seletedIndex != -1)
            {
                // Draw text of the selected item
                float textScale = Height / DefaultHeight;
                Render2D.DrawText(style.FontMedium, _items[_seletedIndex], new Rectangle(margin, 0, clientRect.Width - boxSize, clientRect.Height), Enabled ? style.Foreground : style.ForegroundDisabled, TextAlignment.Near, TextAlignment.Center, TextWrapping.NoWrap, 1.0f, textScale);
            }

            // Arrow
            Render2D.DrawSprite(style.ArrowDown, new Rectangle(clientRect.Width - margin - boxSize, margin, boxSize, boxSize), isOpened ? style.BackgroundSelected : (Color.White * arrowOpacity));
        }

        /// <inheritdoc />
        public override void OnLostFocus()
        {
            base.OnLostFocus();

            // Clear flags
            _mouseDown = false;
            _blockPopup = false;
        }

        /// <inheritdoc />
        public override void OnMouseLeave()
        {
            // Clear flags
            _mouseDown = false;
            _blockPopup = false;

            base.OnMouseLeave();
        }

        /// <inheritdoc />
        public override bool OnMouseDown(Vector2 location, MouseButtons buttons)
        {
            // Check mosue buttons
            if (buttons == MouseButtons.Left)
            {
                // Set flag
                _mouseDown = true;
            }

            return base.OnMouseDown(location, buttons);
        }

        /// <inheritdoc />
        public override bool OnMouseUp(Vector2 location, MouseButtons buttons)
        {
            // Check flags
            if (_mouseDown && !_blockPopup)
            {
                // Clear flag
                _mouseDown = false;

                // Ensure to have valid menu
                if (_popupMenu == null)
                    _popupMenu = CreatePopup();

                // Check if menu hs been already shown
                if (_popupMenu.Visible)
                {
                    // Hide
                    _popupMenu.Hide();
                    return true;
                }

                // Check if has any items
                if (_items.Count > 0)
                {
                    // Setup items list
                    _popupMenu.DisposeChildren();
                    if (Sorted)
                        _items.Sort();
                    for (int i = 0; i < _items.Count; i++)
                    {
                        _popupMenu.AddButton(i, _items[i]);
                    }

                    // Show dropdown list
                    _popupMenu.MinimumWidth = Width;
                    _popupMenu.Show(this, new Vector2(1, Height));
                }
            }
            else
            {
                _blockPopup = false;
            }

            return true;
        }
    }
}
