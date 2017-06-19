////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Linq;
using FlaxEditor.Windows;
using FlaxEngine.Assertions;
using FlaxEngine.GUI;

namespace FlaxEditor.Content.GUI
{
    /// <summary>
    /// Main control for <see cref="ContentWindow"/> used to present collection of <see cref="ContentItem"/>.
    /// </summary>
    /// <seealso cref="FlaxEngine.GUI.ContainerControl" />
    /// <seealso cref="FlaxEditor.Content.IContentItemOwner" />
    public class ContentView : ContainerControl, IContentItemOwner
    {
        private readonly List<ContentItem> _items = new List<ContentItem>(256);
        private readonly List<ContentItem> _selection = new List<ContentItem>(16);

        private float _scale = 1.0f;

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
        /// Called when user wants to duplicate the item.
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
        /// <value>
        /// The items.
        /// </value>
        public List<ContentItem> Items => _items;

        /// <summary>
        /// Gets the items count.
        /// </summary>
        /// <value>
        /// The items count.
        /// </value>
        public int ItemsCount => _items.Count;

        /// <summary>
        /// Gets the selected items.
        /// </summary>
        /// <value>
        /// The selection.
        /// </value>
        public List<ContentItem> Selection => _selection;

        /// <summary>
        /// Gets the selected count.
        /// </summary>
        /// <value>
        /// The selected count.
        /// </value>
        public int SelectedCount => _selection.Count;

        /// <summary>
        /// Gets a value indicating whether any item is selected.
        /// </summary>
        /// <value>
        ///   <c>true</c> if any item is selected; otherwise, <c>false</c>.
        /// </value>
        public bool HasSelection => _selection.Count > 0;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContentView"/> class.
        /// </summary>
        public ContentView()
            : base(true)
        {
            DockStyle = DockStyle.Top;
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
            if(items == null)
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

            // Sort itmes
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

            // Deselect items if need to
            if(!additive)
                _selection.Clear();

            // Select items
            Assert.IsTrue(items.TrueForAll(x => _items.Contains(x)));
            _selection.AddRange(items);

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

            // Deselect items if need to
            if (!additive)
                _selection.Clear();

            // Select item
            Assert.IsTrue(_items.Contains(item));
            _selection.Add(item);

            // Unload and perform UI layout
            IsLayoutLocked = wasLayoutLocked;
            PerformLayout();
        }

        /// <summary>
        /// Refreshes thumbnails of all itmes in the <see cref="ContentView"/>.
        /// </summary>
        public void RefreshPreviews()
        {
            for (int i = 0; i < _items.Count; i++)
                _items[i].RefreshPreview();
        }

        #region Internal events

        /// <summary>
        /// Called when user clicks on an item.
        /// </summary>
        /// <param name="item">The item.</param>
        public void OnItemClick(ContentItem item)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Called when user wants to rename item.
        /// </summary>
        /// <param name="item">The item.</param>
        public void OnItemDoubleClickName(ContentItem item)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Called when user wants to open item.
        /// </summary>
        /// <param name="item">The item.</param>
        public void OnItemDoubleClick(ContentItem item)
        {
            throw new NotImplementedException();
        }

        #endregion

        /// <inheritdoc />
        void IContentItemOwner.OnItemDeleted(ContentItem item)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        void IContentItemOwner.OnItemRenamed(ContentItem item)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        void IContentItemOwner.OnItemDispose(ContentItem item)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public override bool IsScrollable => true;

        /// <inheritdoc />
        public override void OnDestroy()
        {
            // Ensure to unlink all items
            ClearItems();

            base.OnDestroy();
        }
    }
}
