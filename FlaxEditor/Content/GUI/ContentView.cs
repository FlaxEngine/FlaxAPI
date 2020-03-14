// Copyright (c) 2012-2020 Wojciech Figat. All rights reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using FlaxEditor.Options;
using FlaxEditor.Windows;
using FlaxEngine;
using FlaxEngine.GUI;

namespace FlaxEditor.Content.GUI
{
    /// <summary>
    /// Main control for <see cref="ContentWindow"/> used to present collection of <see cref="ContentItem"/>.
    /// </summary>
    /// <seealso cref="FlaxEngine.GUI.ContainerControl" />
    /// <seealso cref="FlaxEditor.Content.IContentItemOwner" />
    public partial class ContentView : ContainerControl, IContentItemOwner
    {
        private readonly List<ContentItem> _items = new List<ContentItem>(256);
        private readonly List<ContentItem> _selection = new List<ContentItem>(16);

        private float _viewScale = 1.0f;

        #region External Events

        /// <summary>
        /// Called when user wants to open the item.
        /// </summary>
        public event Action<ContentItem> OnOpen;

        /// <summary>
        /// Called when user wants to rename the item.
        /// </summary>
        public event Action<ContentItem> OnRename;

        /// <summary>
        /// Called when user wants to delete the item.
        /// </summary>
        public event Action<List<ContentItem>> OnDelete;

        /// <summary>
        /// Called when user wants to paste the files/folders.
        /// </summary>
        public event Action<string[]> OnPaste;

        /// <summary>
        /// Called when user wants to duplicate the item(s).
        /// </summary>
        public event Action<List<ContentItem>> OnDuplicate;

        /// <summary>
        /// Called when user wants to navigate backward.
        /// </summary>
        public event Action OnNavigateBack;

        #endregion

        /// <summary>
        /// Gets the items.
        /// </summary>
        public List<ContentItem> Items => _items;

        /// <summary>
        /// Gets the items count.
        /// </summary>
        public int ItemsCount => _items.Count;

        /// <summary>
        /// Gets the selected items.
        /// </summary>
        public List<ContentItem> Selection => _selection;

        /// <summary>
        /// Gets the selected count.
        /// </summary>
        public int SelectedCount => _selection.Count;

        /// <summary>
        /// Gets a value indicating whether any item is selected.
        /// </summary>
        public bool HasSelection => _selection.Count > 0;

        /// <summary>
        /// Gets or sets the view scale.
        /// </summary>
        public float ViewScale
        {
            get => _viewScale;
            set
            {
                value = Mathf.Clamp(value, 0.3f, 3.0f);
                if (!Mathf.NearEqual(value, _viewScale))
                {
                    _viewScale = value;
                    PerformLayout();
                }
            }
        }

        /// <summary>
        /// Flag is used to indicate if user is searching for items. Used to show a proper message to the user.
        /// </summary>
        public bool IsSearching;

        /// <summary>
        /// The input actions collection to processed during user input.
        /// </summary>
        public readonly InputActionsContainer InputActions;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContentView"/> class.
        /// </summary>
        public ContentView()
        {
            DockStyle = DockStyle.Top;
            IsScrollable = true;

            // Setup input actions
            InputActions = new InputActionsContainer(new[]
            {
                new InputActionsContainer.Binding(options => options.Delete, () =>
                {
                    if (HasSelection)
                        OnDelete?.Invoke(_selection);
                }),
                new InputActionsContainer.Binding(options => options.SelectAll, SelectAll),
                new InputActionsContainer.Binding(options => options.Rename, () =>
                {
                    if (HasSelection && _selection[0].CanRename)
                    {
                        if (_selection.Count > 1)
                            Select(_selection[0]);
                        OnRename?.Invoke(_selection[0]);
                    }
                }),
                new InputActionsContainer.Binding(options => options.Copy, Copy),
                new InputActionsContainer.Binding(options => options.Paste, Paste),
                new InputActionsContainer.Binding(options => options.Duplicate, Duplicate),
            });
        }

        /// <summary>
        /// Clears the items in the view.
        /// </summary>
        public void ClearItems()
        {
            // Lock layout
            var wasLayoutLocked = IsLayoutLocked;
            IsLayoutLocked = true;

            // Deselect items first
            ClearSelection();

            // Remove references and unlink items
            for (int i = 0; i < _items.Count; i++)
            {
                _items[i].Parent = null;
                _items[i].RemoveReference(this);
            }
            _items.Clear();

            // Unload and perform UI layout
            IsLayoutLocked = wasLayoutLocked;
            PerformLayout();
        }

        /// <summary>
        /// Shows the items collection in the view.
        /// </summary>
        /// <param name="items">The items to show.</param>
        /// <param name="additive">If set to <c>true</c> items will be added to the current selection. Otherwise selection will be cleared before.</param>
        public void ShowItems(List<ContentItem> items, bool additive = false)
        {
            if (items == null)
                throw new ArgumentNullException();

            // Check if show nothing or not change view
            if (items.Count == 0)
            {
                // Deselect items if need to
                if (!additive)
                    ClearItems();
                return;
            }

            // Lock layout
            var wasLayoutLocked = IsLayoutLocked;
            IsLayoutLocked = true;

            // Deselect items if need to
            if (!additive)
                ClearItems();

            // Add references and link items
            _items.AddRange(items);
            for (int i = 0; i < items.Count; i++)
            {
                items[i].Parent = this;
                items[i].AddReference(this);
            }

            // Sort items
            _children.Sort();

            // Unload and perform UI layout
            IsLayoutLocked = wasLayoutLocked;
            PerformLayout();
        }

        /// <summary>
        /// Determines whether the specified item is selected.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>
        ///   <c>true</c> if the specified item is selected; otherwise, <c>false</c>.
        /// </returns>
        public bool IsSelected(ContentItem item)
        {
            return _selection.Contains(item);
        }

        /// <summary>
        /// Clears the selected items collection.
        /// </summary>
        public void ClearSelection()
        {
            if (_selection.Count == 0)
                return;

            _selection.Clear();
        }

        /// <summary>
        /// Selects the specified items.
        /// </summary>
        /// <param name="items">The items.</param>
        /// <param name="additive">If set to <c>true</c> items will be added to the current selection. Otherwise selection will be cleared before.</param>
        public void Select(List<ContentItem> items, bool additive = false)
        {
            if (items == null)
                throw new ArgumentNullException();

            // Check if nothing to select
            if (items.Count == 0)
            {
                // Deselect items if need to
                if (!additive)
                    ClearSelection();
                return;
            }

            // Lock layout
            var wasLayoutLocked = IsLayoutLocked;
            IsLayoutLocked = true;

            // Select items
            if (additive)
            {
                for (int i = 0; i < items.Count; i++)
                {
                    if (!_selection.Contains(items[i]))
                        _selection.Add(items[i]);
                }
            }
            else
            {
                _selection.Clear();
                _selection.AddRange(items);
            }

            // Unload and perform UI layout
            IsLayoutLocked = wasLayoutLocked;
            PerformLayout();
        }

        /// <summary>
        /// Selects the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="additive">If set to <c>true</c> item will be added to the current selection. Otherwise selection will be cleared before.</param>
        public void Select(ContentItem item, bool additive = false)
        {
            if (item == null)
                throw new ArgumentNullException();

            // Lock layout
            var wasLayoutLocked = IsLayoutLocked;
            IsLayoutLocked = true;

            // Select item
            if (additive)
            {
                if (!_selection.Contains(item))
                    _selection.Add(item);
            }
            else
            {
                _selection.Clear();
                _selection.Add(item);
            }

            // Unload and perform UI layout
            IsLayoutLocked = wasLayoutLocked;
            PerformLayout();
        }

        /// <summary>
        /// Selects all the items.
        /// </summary>
        public void SelectAll()
        {
            // Lock layout
            var wasLayoutLocked = IsLayoutLocked;
            IsLayoutLocked = true;

            // Select items
            _selection.Clear();
            _selection.AddRange(_items);

            // Unload and perform UI layout
            IsLayoutLocked = wasLayoutLocked;
            PerformLayout();
        }

        /// <summary>
        /// Deselects the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        public void Deselect(ContentItem item)
        {
            if (item == null)
                throw new ArgumentNullException();

            // Lock layout
            var wasLayoutLocked = IsLayoutLocked;
            IsLayoutLocked = true;

            // Deselect item
            if (_selection.Contains(item))
                _selection.Remove(item);

            // Unload and perform UI layout
            IsLayoutLocked = wasLayoutLocked;
            PerformLayout();
        }

        /// <summary>
        /// Duplicates the selected items.
        /// </summary>
        public void Duplicate()
        {
            OnDuplicate?.Invoke(_selection);
        }

        /// <summary>
        /// Copies the selected items (to the system clipboard).
        /// </summary>
        public void Copy()
        {
            if (_selection.Count == 0)
                return;

            var files = _selection.ConvertAll(x => x.Path).ToArray();
            Platform.ClipboardFiles = files;
        }

        /// <summary>
        /// Returns true if user can paste data to the view (copied any files before).
        /// </summary>
        /// <returns>True if can paste files.</returns>
        public bool CanPaste()
        {
            var files = Platform.ClipboardFiles;
            return files != null && files.Length > 0;
        }

        /// <summary>
        /// Pastes the copied items (from the system clipboard).
        /// </summary>
        public void Paste()
        {
            var files = Platform.ClipboardFiles;
            if (files == null || files.Length == 0)
                return;

            OnPaste?.Invoke(files);
        }

        /// <summary>
        /// Gives focus and selects the first item in the view.
        /// </summary>
        public void SelectFirstItem()
        {
            if (_items.Count > 0)
            {
                _items[0].Focus();
                Select(_items[0]);
            }
            else
            {
                Focus();
            }
        }

        /// <summary>
        /// Refreshes thumbnails of all items in the <see cref="ContentView"/>.
        /// </summary>
        public void RefreshThumbnails()
        {
            for (int i = 0; i < _items.Count; i++)
                _items[i].RefreshThumbnail();
        }

        #region Internal events

        /// <summary>
        /// Called when user clicks on an item.
        /// </summary>
        /// <param name="item">The item.</param>
        public void OnItemClick(ContentItem item)
        {
            bool isSelected = _selection.Contains(item);

            // Add/remove from selection
            if (Root.GetKey(Keys.Control))
            {
                if (isSelected)
                    Deselect(item);
                else
                    Select(item, true);
            }
            // Range select
            else if (Root.GetKey(Keys.Shift))
            {
                int min = _selection.Min(x => x.IndexInParent);
                int max = _selection.Max(x => x.IndexInParent);
                min = Mathf.Min(min, item.IndexInParent);
                max = Mathf.Max(max, item.IndexInParent);
                var selection = new List<ContentItem>(_selection);
                for (int i = min; i <= max; i++)
                {
                    if (_children[i] is ContentItem cc && !selection.Contains(cc))
                    {
                        selection.Add(cc);
                    }
                }
                Select(selection);
            }
            // Select
            else
            {
                Select(item);
            }
        }

        /// <summary>
        /// Called when user wants to rename item.
        /// </summary>
        /// <param name="item">The item.</param>
        public void OnItemDoubleClickName(ContentItem item)
        {
            OnRename?.Invoke(item);
        }

        /// <summary>
        /// Called when user wants to open item.
        /// </summary>
        /// <param name="item">The item.</param>
        public void OnItemDoubleClick(ContentItem item)
        {
            OnOpen?.Invoke(item);
        }

        #endregion

        #region IContentItemOwner

        /// <inheritdoc />
        void IContentItemOwner.OnItemDeleted(ContentItem item)
        {
            _selection.Remove(item);
            _items.Remove(item);
        }

        /// <inheritdoc />
        void IContentItemOwner.OnItemRenamed(ContentItem item)
        {
        }

        /// <inheritdoc />
        void IContentItemOwner.OnItemReimported(ContentItem item)
        {
        }

        /// <inheritdoc />
        void IContentItemOwner.OnItemDispose(ContentItem item)
        {
            _selection.Remove(item);
            _items.Remove(item);
        }

        #endregion

        /// <inheritdoc />
        public override void Draw()
        {
            base.Draw();

            var style = Style.Current;

            // Check if drag is over
            if (IsDragOver && _validDragOver)
            {
                Render2D.FillRectangle(new Rectangle(Vector2.Zero, Size), style.BackgroundSelected * 0.4f);
            }

            // Check if it's an empty thing
            if (_items.Count == 0)
            {
                Render2D.DrawText(style.FontSmall, IsSearching ? "No results" : "Empty", new Rectangle(Vector2.Zero, Size), style.ForegroundDisabled, TextAlignment.Center, TextAlignment.Center);
            }
        }

        /// <inheritdoc />
        public override bool OnMouseWheel(Vector2 location, float delta)
        {
            // Check if pressing control key
            if (Root.GetKey(Keys.Control))
            {
                // Zoom
                ViewScale += delta * 0.05f;

                // Handled
                return true;
            }

            return base.OnMouseWheel(location, delta);
        }

        /// <inheritdoc />
        public override bool OnKeyDown(Keys key)
        {
            // Navigate backward
            if (key == Keys.Backspace)
            {
                OnNavigateBack?.Invoke();
                return true;
            }

            if (InputActions.Process(Editor.Instance, this, key))
                return true;

            // Check if sth is selected
            if (HasSelection)
            {
                // Open
                if (key == Keys.Return && _selection.Count == 1)
                {
                    OnOpen?.Invoke(_selection[0]);
                    return true;
                }

                // Movement with arrows
                {
                    var root = _selection[0];
                    Vector2 size = root.Size;
                    Vector2 offset = Vector2.Minimum;
                    ContentItem item = null;
                    if (key == Keys.ArrowUp)
                    {
                        offset = new Vector2(0, -size.Y);
                    }
                    else if (key == Keys.ArrowDown)
                    {
                        offset = new Vector2(0, size.Y);
                    }
                    else if (key == Keys.ArrowRight)
                    {
                        offset = new Vector2(size.X, 0);
                    }
                    else if (key == Keys.ArrowLeft)
                    {
                        offset = new Vector2(-size.X, 0);
                    }
                    if (offset != Vector2.Minimum)
                    {
                        item = GetChildAt(root.Location + size / 2 + offset) as ContentItem;
                    }
                    if (item != null)
                    {
                        OnItemClick(item);
                        return true;
                    }
                }
            }

            return base.OnKeyDown(key);
        }

        /// <inheritdoc />
        protected override void PerformLayoutSelf()
        {
            // Calculate items size
            float width = Width;
            float defaultItemsWidth = ContentItem.DefaultWidth * _viewScale;
            int itemsToFit = Mathf.FloorToInt(width / defaultItemsWidth);
            float itemsWidth = width / Mathf.Max(itemsToFit, 1);
            float itemsHeight = itemsWidth / defaultItemsWidth * (ContentItem.DefaultHeight * _viewScale);

            // Arrange controls
            float x = 0, y = 0;
            for (int i = 0; i < _children.Count; i++)
            {
                var c = _children[i];

                c.Bounds = new Rectangle(x, y, itemsWidth, itemsHeight);

                x += itemsWidth;
                if (x + itemsWidth > width)
                {
                    x = 0;
                    y += itemsHeight;
                }
            }
            if (x > 0)
                y += itemsHeight;

            // Set maximum size and fit the parent container
            if (HasParent)
                y = Mathf.Max(y, Parent.Height);
            Height = y;
        }

        /// <inheritdoc />
        public override void OnDestroy()
        {
            // Ensure to unlink all items
            ClearItems();

            base.OnDestroy();
        }
    }
}
