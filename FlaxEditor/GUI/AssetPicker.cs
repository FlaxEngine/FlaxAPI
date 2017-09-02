////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System;
using FlaxEditor.Content;
using FlaxEditor.GUI.Drag;
using FlaxEngine;
using FlaxEngine.GUI;

namespace FlaxEditor.GUI
{
    /// <summary>
    /// Assets picking control.
    /// </summary>
    /// <seealso cref="Control" />
    /// <seealso cref="IContentItemOwner" />
    public class AssetPicker : Control, IContentItemOwner
    {
        private const float DefaultIconSize = 64;
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
        public AssetItem SelectedItem
        {
            get => _selected;
            set
            {
                // Check if value won't change
                if (value == _selected)
                    return;
                if (value != null && value.ItemDomain != _domain)
                    throw new ArgumentException("Invalid asset domain.");

                // Change value
                _selected?.RemoveReference(this);
                _selected = value;
                _selected?.AddReference(this);
                
                // Update tooltip
                TooltipText = _selected?.NamePath;
                
                OnSelectedItemChanged();
            }
        }

        /// <summary>
        /// Gets or sets the selected asset identifier.
        /// </summary>
        /// <value>
        /// The selected asset identifier.
        /// </value>
        public Guid SelectedID
        {
            get => _selected?.ID ?? Guid.Empty;
            set
            {
                if (value != SelectedID)
                {
                    SelectedItem = Editor.Instance.ContentDatabase.Find(value) as AssetItem;
                }
            }
        }

        /// <summary>
        /// Gets or sets the selected asset object.
        /// </summary>
        /// <value>
        /// The selected assetobject .
        /// </value>
        public Asset SelectedAsset
        {
            get => _selected != null ? FlaxEngine.Content.Load(_selected.ID) : null;
            set
            {
                if (value == null)
                {
                    SelectedItem = null;
                }
                else if (value.ID != SelectedID)
                {
                    SelectedItem = Editor.Instance.ContentDatabase.Find(value.ID) as AssetItem;
                }
            }
        }

        /// <summary>
        /// Gets or sets the assets domain that this picker acepts.
        /// </summary>
        /// <value>
        /// The domain.
        /// </value>
        public ContentDomain Domain
        {
            get => _domain;
            set
            {
                if (_domain != value)
                {
                    _domain = value;

                    if (_selected != null && _selected.ItemDomain != _domain)
                        SelectedItem = null;
                }
            }
        }

        /// <summary>
        /// Occurs when selected item gets changed.
        /// </summary>
        public event Action SelectedItemChanged;

        /// <summary>
        /// Initializes a new instance of the <see cref="AssetPicker"/> class.
        /// </summary>
        public AssetPicker()
            : this(ContentDomain.Invalid, Vector2.Zero)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AssetPicker"/> class.
        /// </summary>
        /// <param name="contentDomain">The assets content domain.</param>
        public AssetPicker(ContentDomain contentDomain)
            : this(contentDomain, Vector2.Zero)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AssetPicker"/> class.
        /// </summary>
        /// <param name="contentDomain">The assets content domain.</param>
        /// <param name="location">The control location.</param>
        public AssetPicker(ContentDomain contentDomain, Vector2 location)
            : base(true, location, new Vector2(DefaultIconSize + ButtonsOffset + ButtonsSize, DefaultIconSize))
        {
            _domain = contentDomain;
            _mousePos = Vector2.Minimum;
        }
        
        /// <summary>
        /// Called when selected item gets changed.
        /// </summary>
        protected virtual void OnSelectedItemChanged()
        {
            SelectedItemChanged?.Invoke();
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
            SelectedItem = null;
        }

        /// <inheritdoc />
        public void OnItemRenamed(ContentItem item)
        {
        }

        /// <inheritdoc />
        public void OnItemDispose(ContentItem item)
        {
            // Deselect item
            SelectedItem = null;
        }

        private Rectangle IconRect => new Rectangle(0, 0, Height, Height);

        private Rectangle Button1Rect => new Rectangle(Height + ButtonsOffset, 0, ButtonsSize, ButtonsSize);

        private Rectangle Button2Rect => new Rectangle(Height + ButtonsOffset, ButtonsSize, ButtonsSize, ButtonsSize);

        /// <inheritdoc />
        public override void Draw()
        {
            // Cache data
            var style = Style.Current;
            var iconRect = IconRect;
            var button1Rect = Button1Rect;
            var button2Rect = Button2Rect;

            // Check if has item selected
            if (_selected != null)
            {
                // Draw item preview
                _selected.DrawThumbnail(ref iconRect);

                // Draw find button
                Render2D.DrawSprite(style.Search, button1Rect, new Color(button1Rect.Contains(_mousePos) ? 1.0f : 0.7f));

                // Draw remove button
                Render2D.DrawSprite(style.Cross, button2Rect, new Color(button2Rect.Contains(_mousePos) ? 1.0f : 0.7f));

                // Draw name
                float sizeForTextLeft = Width - button1Rect.Right;
                if (sizeForTextLeft > 30)
                {
                    Render2D.DrawText(
                        style.FontSmall,
                        _selected.ShortName,
                        new Rectangle(button1Rect.Right + 2, 0, sizeForTextLeft, ButtonsSize),
                        Color.White,
                        TextAlignment.Near,
                        TextAlignment.Center);
                }
            }
            else
            {
                // No element selected
                Render2D.FillRectangle(iconRect, new Color(0.2f));
                Render2D.DrawText(style.FontMedium, "No asset\nselected", iconRect, Color.Wheat, TextAlignment.Center, TextAlignment.Center, TextWrapping.NoWrap, 1.0f, Height / DefaultIconSize);
            }

            // Check if drag is over
            if (IsDragOver && _dragOverElement.HasValidDrag)
                Render2D.FillRectangle(iconRect, style.BackgroundSelected * 0.4f, true);
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
            if (buttons == MouseButtons.Left)
            {
                _isMosueDown = false;
            }

            // Buttons logic
            if (_selected != null)
            {
                if (Button1Rect.Contains(location))
                {
                    // Select asset
                    Editor.Instance.Windows.ContentWin.Select(_selected);
                }
                else if (Button2Rect.Contains(location))
                {
                    // Deselect asset
                    SelectedItem = null;
                }
            }

            // Handled
            return true;
        }

        /// <inheritdoc />
        public override bool OnMouseDown(Vector2 location, MouseButtons buttons)
        {
            // Set flag for dragging asset
            if (buttons == MouseButtons.Left && IconRect.Contains(location))
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
            if (_selected != null && IconRect.Contains(location))
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
                SelectedItem = _dragOverElement.Objects[0];
            }

            // Clear cache
            _dragOverElement.OnDragDrop();

            return DragDropEffect.Move;
        }
    }
}
