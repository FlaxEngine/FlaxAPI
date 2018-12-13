// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

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
            if (_itemsSearchBox.TextLength == 0)
                return;

            IsLayoutLocked = true;

            _itemsSearchBox.Clear();
            _itemsFilterBox.SelectedIndex = -1;

            IsLayoutLocked = false;
        }

        private void UpdateItemsSearch()
        {
            // Skip events during setup or init stuff
            if (IsLayoutLocked)
                return;

            // TODO: implement it
        }
    }
}
