////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System;
using FlaxEditor.Content;
using FlaxEditor.GUI.Drag;
using FlaxEngine;
using FlaxEngine.GUI;

namespace FlaxEditor.Surface.Elements
{
    /// <summary>
    /// Assets picking control.
    /// </summary>
    /// <seealso cref="FlaxEditor.Surface.SurfaceNodeElementControl" />
    public class AssetSelect : SurfaceNodeElementControl, IContentItemOwner
    {
        private const float IconSize = 64;
        private const float ButtonsOffset = 2;
        private const float ButtonsSize = 12;

        private AssetItem _selected;
        private ContentDomain _domain;

        private bool _isMosueDown;
        private Vector2 _mosueDownPos;
        private Vector2 _mousePos;
        private readonly DragAssets _dragOverElement = new DragAssets();

        /// <summary>
        /// Gets or sets the selected item.
        /// </summary>
        /// <value>
        /// The selected item.
        /// </value>
        public AssetItem Selected
        {
            get => _selected;
            set
            {
                // Check if value won't change
                if (value == _selected)
                    return;

                // Remove reference to the previous
                _selected?.RemoveReference(this);

                // Change value
                _selected = value;
                ParentNode.Values[Archetype.ValueIndex] = _selected?.ID ?? Guid.Empty;

                // Add reference to the new
                _selected?.AddReference(this);

                // Fire events
                ParentNode.Surface.MarkAsEdited();
            }
        }

        /// <inheritdoc />
        public AssetSelect(SurfaceNode parentNode, NodeElementArchetype archetype)
            : base(parentNode, archetype, archetype.ActualPosition, new Vector2(IconSize + ButtonsOffset + ButtonsSize, IconSize), true)
        {
            _domain = (ContentDomain)archetype.BoxID;
            _mousePos = Vector2.Minimum;

            // Link item
            _selected = Editor.Instance.ContentDatabase.Find((Guid)parentNode.Values[archetype.ValueIndex]) as AssetItem;
            _selected?.AddReference(this);
        }

        private void DoDrag()
        {
            // Do the drag drop operation if has selected element
            if (_selected != null && new Rectangle(Vector2.Zero, Size).Contains(ref _mosueDownPos))
            {
                DoDragDrop(DragAssets.GetDragData(_selected));
            }
        }

        /// <inheritdoc />
        public void OnItemDeleted(ContentItem item)
        {
            // Deselect item
            Selected = null;
        }

        /// <inheritdoc />
        public void OnItemRenamed(ContentItem item)
        {
        }

        /// <inheritdoc />
        public void OnItemDispose(ContentItem item)
        {
            // Deselect item
            Selected = null;
        }

        /// <inheritdoc />
        public override void Draw()
        {
            // Cache data
            var style = Style.Current;
            float buttonSize = ButtonsSize;
            var itemRect = new Rectangle(0, 0, Height, Height);
            var button1Rect = new Rectangle(Width + ButtonsOffset - buttonSize, 0, buttonSize, buttonSize);
            var button2Rect = new Rectangle(button1Rect.X, button1Rect.Bottom, buttonSize, buttonSize);

            // Check if has item selected
            if (_selected != null)
            {
                // Draw item preview
                _selected.DrawThumbnail(ref itemRect);

                // Draw find button
                Render2D.DrawSprite(style.Search, button1Rect, new Color(button1Rect.Contains(_mousePos) ? 1.0f : 0.7f));

                // Draw remove button
                Render2D.DrawSprite(style.Cross, button2Rect, new Color(button2Rect.Contains(_mousePos) ? 1.0f : 0.7f));
            }
            else
            {
                // No element selected
                Render2D.FillRectangle(itemRect, new Color(0.2f));
                Render2D.DrawText(style.FontMedium, "No\nasset\nselected", itemRect, Color.Wheat, TextAlignment.Center, TextAlignment.Center, TextWrapping.NoWrap, 1.0f, Height / IconSize);
            }

            // Check if drag is over
            if (IsDragOver && _dragOverElement.HasValidDrag)
                Render2D.FillRectangle(itemRect, style.BackgroundSelected * 0.4f, true);
        }

        /// <inheritdoc />
        public override void OnDestroy()
        {
            _selected?.RemoveReference(this);
            _selected = null;

            base.OnDestroy();
        }

        /// <inheritdoc />
        public override void OnMouseLeave()
        {
            _mousePos = Vector2.Minimum;

            // Check if start drag drop
            if (_isMosueDown)
            {
                // Clear flag
                _isMosueDown = false;

                // Do the drag
                DoDrag();
            }

            base.OnMouseLeave();
        }

        /// <inheritdoc />
        public override void OnMouseEnter(Vector2 location)
        {
            _mousePos = location;
            _mosueDownPos = Vector2.Minimum;

            base.OnMouseEnter(location);
        }

        /// <inheritdoc />
        public override void OnMouseMove(Vector2 location)
        {
            _mousePos = location;

            // Check if start drag drop
            if (_isMosueDown && Vector2.Distance(location, _mosueDownPos) > 10.0f)
            {
                // Clear flag
                _isMosueDown = false;

                // Do the drag
                DoDrag();
            }

            base.OnMouseMove(location);
        }

        /// <inheritdoc />
        public override bool OnMouseUp(Vector2 location, MouseButtons buttons)
        {
            // Buttons logic
            float buttonSize = ButtonsSize;
            var button1Rect = new Rectangle(Width + ButtonsOffset - buttonSize, 0, buttonSize, buttonSize);
            var button2Rect = new Rectangle(button1Rect.X, button1Rect.Bottom, buttonSize, buttonSize);
            if (_selected != null)
            {
                if (button1Rect.Contains(location))
                {
                    // Select asset
                    Editor.Instance.Windows.ContentWin.Select(_selected);
                }
                else if (button2Rect.Contains(location))
                {
                    // Deselect asset
                    Selected = null;
                }
            }

            // Handled
            return true;
        }

        /// <inheritdoc />
        public override bool OnMouseDown(Vector2 location, MouseButtons buttons)
        {
            // Set flag
            if (buttons == MouseButtons.Left)
            {
                _isMosueDown = true;
                _mosueDownPos = location;
            }

            // Handled
            return true;
        }

        /// <inheritdoc />
        public override bool OnMouseDoubleClick(Vector2 location, MouseButtons buttons)
        {
            // Focus
            Focus();

            // Check if has element selected
            if (_selected != null)
            {
                // Open it
                Editor.Instance.ContentEditing.Open(_selected);
            }

            // Handled
            return true;
        }

        /// <inheritdoc />
        public override DragDropEffect OnDragEnter(ref Vector2 location, DragData data)
        {
            base.OnDragEnter(ref location, data);

            // Check if drop asset
            if (_dragOverElement.OnDragEnter(data, x => x.ItemDomain == _domain))
            {
            }

            return _dragOverElement.Effect;
        }

        /// <inheritdoc />
        public override DragDropEffect OnDragMove(ref Vector2 location, DragData data)
        {
            base.OnDragMove(ref location, data);

            return _dragOverElement.Effect;
        }

        /// <inheritdoc />
        public override void OnDragLeave()
        {
            // Clear cache
            _dragOverElement.OnDragLeave();

            base.OnDragLeave();
        }

        /// <inheritdoc />
        public override DragDropEffect OnDragDrop(ref Vector2 location, DragData data)
        {
            base.OnDragDrop(ref location, data);

            if (_dragOverElement.HasValidDrag)
            {
                // Select element
                Selected = _dragOverElement.Objects[0];
            }

            // Clear cache
            _dragOverElement.OnDragDrop();

            return DragDropEffect.Move;
        }
    }
}
