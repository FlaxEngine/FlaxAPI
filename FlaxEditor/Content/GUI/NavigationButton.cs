// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using FlaxEditor.GUI.Drag;
using FlaxEngine;
using FlaxEngine.GUI;

namespace FlaxEditor.Content.GUI
{
    /// <summary>
    /// Content window navigation button.
    /// </summary>
    /// <seealso cref="FlaxEngine.GUI.Button" />
    public class NavigationButton : Button
    {
        private DragItems _dragOverItems;
        private bool _validDragOver;

        /// <summary>
        /// The default margin (horizontal).
        /// </summary>
        public const float DefaultMargin = 6.0f;

        /// <summary>
        /// Gets the target node.
        /// </summary>
        public ContentTreeNode TargetNode { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="NavigationButton"/> class.
        /// </summary>
        /// <param name="targetNode">The target node.</param>
        /// <param name="x">The x position.</param>
        /// <param name="y">The y position.</param>
        /// <param name="height">The height.</param>
        public NavigationButton(ContentTreeNode targetNode, float x, float y, float height)
        : base(x, y, 2 * DefaultMargin)
        {
            TargetNode = targetNode;
            Height = height;
            Text = targetNode.NavButtonLabel + "/";
        }

        /// <inheritdoc />
        public override void Draw()
        {
            // Cache data
            var style = Style.Current;
            var clientRect = new Rectangle(Vector2.Zero, Size);
            var textRect = new Rectangle(4, 0, clientRect.Width - 4, clientRect.Height);

            // Draw background
            if (IsDragOver && _validDragOver)
            {
                Render2D.FillRectangle(clientRect, Style.Current.BackgroundSelected * 0.6f);
            }
            else if (_mouseDown)
            {
                Render2D.FillRectangle(clientRect, style.BackgroundSelected);
            }
            else if (IsMouseOver)
            {
                Render2D.FillRectangle(clientRect, style.BackgroundHighlighted);
            }

            // Draw text
            Render2D.DrawText(style.FontMedium, Text, textRect, style.Foreground, TextAlignment.Near, TextAlignment.Center);
        }

        /// <inheritdoc />
        public override void PerformLayout(bool force = false)
        {
            var style = Style.Current;

            if (style.FontMedium)
            {
                Width = style.FontMedium.MeasureText(Text).X + 2 * DefaultMargin;
            }
        }

        /// <inheritdoc />
        protected override void OnClick()
        {
            // Navigate
            Editor.Instance.Windows.ContentWin.Navigate(TargetNode);

            base.OnClick();
        }


        private DragDropEffect GetDragEffect(DragData data)
        {
            if (data is DragDataFiles)
            {
                if (TargetNode.CanHaveAssets)
                    return DragDropEffect.Copy;
            }
            else
            {
                if (_dragOverItems.HasValidDrag)
                    return DragDropEffect.Move;
            }

            return DragDropEffect.None;
        }

        private bool ValidateDragItem(ContentItem item)
        {
            // Reject itself and any parent
            return item != TargetNode.Folder && !item.Find(TargetNode.Folder) && !TargetNode.IsRoot;
        }

        /// <inheritdoc />
        public override DragDropEffect OnDragEnter(ref Vector2 location, DragData data)
        {
            base.OnDragEnter(ref location, data);

            if (_dragOverItems == null)
                _dragOverItems = new DragItems(ValidateDragItem);

            _dragOverItems.OnDragEnter(data);
            var result = GetDragEffect(data);
            _validDragOver = result != DragDropEffect.None;
            return result;
        }

        /// <inheritdoc />
        public override DragDropEffect OnDragMove(ref Vector2 location, DragData data)
        {
            base.OnDragMove(ref location, data);

            return GetDragEffect(data);
        }

        /// <inheritdoc />
        public override void OnDragLeave()
        {
            base.OnDragLeave();

            _dragOverItems.OnDragLeave();
            _validDragOver = false;
        }

        /// <inheritdoc />
        public override DragDropEffect OnDragDrop(ref Vector2 location, DragData data)
        {
            var result = DragDropEffect.None;
            base.OnDragDrop(ref location, data);

            // Check if drop element or files
            if (data is DragDataFiles files)
            {
                // Import files
                Editor.Instance.ContentImporting.Import(files.Files, TargetNode.Folder);
                result = DragDropEffect.Copy;
            }
            else if (_dragOverItems.HasValidDrag)
            {
                // Move items
                Editor.Instance.ContentDatabase.Move(_dragOverItems.Objects, TargetNode.Folder);
                result = DragDropEffect.Move;
            }

            _dragOverItems.OnDragDrop();
            _validDragOver = false;

            return result;
        }
    }
}
