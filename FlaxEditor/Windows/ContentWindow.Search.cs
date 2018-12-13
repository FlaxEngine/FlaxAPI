// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System.Collections.Generic;
using FlaxEditor.Content;
using FlaxEditor.Utilities;
using FlaxEngine;
using FlaxEngine.GUI;

namespace FlaxEditor.Windows
{
    public sealed partial class ContentWindow
    {
        private class SearchFilterComboBox : ComboBox
        {
            public SearchFilterComboBox(float x, float y, float width)
            : base(x, y, width)
            {
            }

            /// <inheritdoc />
            public override void Draw()
            {
                // Cache data
                var clientRect = new Rectangle(Vector2.Zero, Size);
                float margin = clientRect.Height * 0.2f;
                float boxSize = clientRect.Height - margin * 2;
                bool isOpened = IsPopupOpened;
                bool enabled = EnabledInHierarchy;
                Color backgroundColor = BackgroundColor;
                Color borderColor = BorderColor;
                Color arrowColor = ArrowColor;
                if (!enabled)
                {
                    backgroundColor *= 0.5f;
                    arrowColor *= 0.7f;
                }
                else if (isOpened || _mouseDown)
                {
                    backgroundColor = BackgroundColorSelected;
                    borderColor = BorderColorSelected;
                    arrowColor = ArrowColorSelected;
                }
                else if (IsMouseOver)
                {
                    backgroundColor = BackgroundColorHighlighted;
                    borderColor = BorderColorHighlighted;
                    arrowColor = ArrowColorHighlighted;
                }

                // Background
                Render2D.FillRectangle(clientRect, backgroundColor);
                Render2D.DrawRectangle(clientRect, borderColor);

                // Draw text
                float textScale = Height / DefaultHeight;
                var textRect = new Rectangle(margin, 0, clientRect.Width - boxSize - 2.0f * margin, clientRect.Height);
                Render2D.PushClip(textRect);
                var textColor = TextColor;
                Render2D.DrawText(Font.GetFont(), "Filters", textRect, enabled ? textColor : textColor * 0.5f, TextAlignment.Near, TextAlignment.Center, TextWrapping.NoWrap, 1.0f, textScale);
                Render2D.PopClip();

                // Arrow
                ArrowImage?.Draw(new Rectangle(clientRect.Width - margin - boxSize, margin, boxSize, boxSize), arrowColor);
            }
        }

        private void OnFoldersSearchBoxTextChanged()
        {
            // Skip events during setup or init stuff
            if (IsLayoutLocked)
                return;

            var root = _root;
            root.LockChildrenRecursive();

            // Update tree
            var query = _foldersSearchBox.Text;
            root.UpdateFilter(query);

            root.UnlockChildrenRecursive();
            PerformLayout();
            PerformLayout();
        }

        /// <summary>
        /// Clears the items searching query text and filters.
        /// </summary>
        public void ClearItemsSearch()
        {
            // Skip if already cleared
            if (_itemsSearchBox.TextLength == 0 && !_itemsFilterBox.HasSelection)
                return;

            IsLayoutLocked = true;

            _itemsSearchBox.Clear();
            _itemsFilterBox.SelectedIndex = -1;

            IsLayoutLocked = false;

            UpdateItemsSearch();
        }

        private void UpdateItemsSearch()
        {
            // Skip events during setup or init stuff
            if (IsLayoutLocked)
                return;

            // Check if clear filters
            if (_itemsSearchBox.TextLength == 0 && !_itemsFilterBox.HasSelection)
            {
                RefreshView();
                return;
            }

            // Apply filter
            var items = new List<ContentItem>(8);
            var query = _itemsSearchBox.Text;
            var filters = new bool[_itemsFilterBox.Items.Count];
            if (_itemsFilterBox.HasSelection)
            {
                // Update filters flags
                for (int i = 0; i < filters.Length; i++)
                {
                    filters[i] = _itemsFilterBox.Selection.Contains(i);
                }
            }
            else
            {
                // No filters
                for (int i = 0; i < filters.Length; i++)
                {
                    filters[i] = true;
                }
            }
            if (string.IsNullOrWhiteSpace(query))
            {
                if (SelectedNode == _root)
                {
                    // Special case for root folder
                    for (int i = 0; i < _root.ChildrenCount; i++)
                    {
                        if (_root.GetChild(i) is ContentTreeNode node)
                            UpdateItemsSearchFilter(node.Folder, items, filters);
                    }
                }
                else
                {
                    UpdateItemsSearchFilter(CurrentViewFolder, items, filters);
                }
            }
            else
            {
                if (SelectedNode == _root)
                {
                    // Special case for root folder
                    for (int i = 0; i < _root.ChildrenCount; i++)
                    {
                        if (_root.GetChild(i) is ContentTreeNode node)
                            UpdateItemsSearchFilter(node.Folder, items, filters, query);
                    }
                }
                else
                {
                    UpdateItemsSearchFilter(CurrentViewFolder, items, filters, query);
                }
            }
            _view.IsSearching = true;
            _view.ShowItems(items);
        }

        private void UpdateItemsSearchFilter(ContentFolder folder, List<ContentItem> items, bool[] filters)
        {
            for (int i = 0; i < folder.Children.Count; i++)
            {
                var child = folder.Children[i];

                if (child is ContentFolder childFolder)
                {
                    UpdateItemsSearchFilter(childFolder, items, filters);
                }
                else
                {
                    if (filters[(int)child.SearchFilter])
                    {
                        items.Add(child);
                    }
                }
            }
        }

        private void UpdateItemsSearchFilter(ContentFolder folder, List<ContentItem> items, bool[] filters, string filterText)
        {
            for (int i = 0; i < folder.Children.Count; i++)
            {
                var child = folder.Children[i];

                if (child is ContentFolder childFolder)
                {
                    UpdateItemsSearchFilter(childFolder, items, filters, filterText);
                }
                else
                {
                    if (filters[(int)child.SearchFilter] && QueryFilterHelper.Match(filterText, child.ShortName))
                    {
                        items.Add(child);
                    }
                }
            }
        }
    }
}
