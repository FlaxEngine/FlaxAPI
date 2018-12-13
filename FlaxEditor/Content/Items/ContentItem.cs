// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System;
using System.Collections.Generic;
using FlaxEditor.Content.GUI;
using FlaxEditor.GUI.Drag;
using FlaxEngine;
using FlaxEngine.Assertions;
using FlaxEngine.GUI;

namespace FlaxEditor.Content
{
    /// <summary>
    /// Content item types.
    /// </summary>
    public enum ContentItemType
    {
        /// <summary>
        /// The binary or text asset.
        /// </summary>
        Asset,

        /// <summary>
        /// The directory.
        /// </summary>
        Folder,

        /// <summary>
        /// The script file.
        /// </summary>
        Script,

        /// <summary>
        /// The scene file.
        /// </summary>
        Scene,

        /// <summary>
        /// The other type.
        /// </summary>
        Other
    }

    /// <summary>
    /// Content item filter types used for searching.
    /// </summary>
    public enum ContentItemSearchFilter
    {
        /// <summary>
        /// The model.
        /// </summary>
        Model,

        /// <summary>
        /// The skinned model.
        /// </summary>
        SkinnedModel,

        /// <summary>
        /// The material.
        /// </summary>
        Material,

        /// <summary>
        /// The texture.
        /// </summary>
        Texture,

        /// <summary>
        /// The scene.
        /// </summary>
        Scene,

        /// <summary>
        /// The prefab.
        /// </summary>
        Prefab,

        /// <summary>
        /// The script.
        /// </summary>
        Script,

        /// <summary>
        /// The audio.
        /// </summary>
        Audio,

        /// <summary>
        /// The animation.
        /// </summary>
        Animation,

        /// <summary>
        /// The json.
        /// </summary>
        Json,

        /// <summary>
        /// The other.
        /// </summary>
        Other,
    }

    /// <summary>
    /// Interface for objects that can reference the content items in order to receive events from them.
    /// </summary>
    public interface IContentItemOwner
    {
        /// <summary>
        /// Called when referenced item gets deleted (asset unloaded, file deleted, etc.).
        /// Item should not be used after that.
        /// </summary>
        /// <param name="item">The item.</param>
        void OnItemDeleted(ContentItem item);

        /// <summary>
        /// Called when referenced item gets renamed (filename change, path change, etc.)
        /// </summary>
        /// <param name="item">The item.</param>
        void OnItemRenamed(ContentItem item);

        /// <summary>
        /// Called when item gets reimported or reloaded.
        /// </summary>
        /// <param name="item">The item.</param>
        void OnItemReimported(ContentItem item);

        /// <summary>
        /// Called when referenced item gets disposed (editor closing, database internal changes, etc.).
        /// Item should not be used after that.
        /// </summary>
        /// <param name="item">The item.</param>
        void OnItemDispose(ContentItem item);
    }

    /// <summary>
    /// Base class for all content items.
    /// Item parent GUI control is always <see cref="ContentView"/> or null if not in a view.
    /// </summary>
    /// <seealso cref="FlaxEngine.GUI.Control" />
    public abstract class ContentItem : Control
    {
        /// <summary>
        /// The default margin size.
        /// </summary>
        public const int DefaultMarginSize = 4;

        /// <summary>
        /// The default text height.
        /// </summary>
        public const int DefaultTextHeight = 42;

        /// <summary>
        /// The default thumbnail size.
        /// </summary>
        public const int DefaultThumbnailSize = PreviewsCache.AssetIconSize;

        /// <summary>
        /// The default width.
        /// </summary>
        public const int DefaultWidth = (DefaultThumbnailSize + 2 * DefaultMarginSize);

        /// <summary>
        /// The default height.
        /// </summary>
        public const int DefaultHeight = (DefaultThumbnailSize + 2 * DefaultMarginSize + DefaultTextHeight);

        private ContentFolder _parentFolder;

        private bool _isMouseDown;
        private Vector2 _mouseDownStartPos;
        private readonly List<IContentItemOwner> _references = new List<IContentItemOwner>(4);

        private Sprite _thumbnail;
        private Sprite _shadowIcon;

        /// <summary>
        /// Gets the item domain.
        /// </summary>
        public virtual ContentDomain ItemDomain => ContentDomain.Invalid;

        /// <summary>
        /// Gets the type of the item.
        /// </summary>
        public abstract ContentItemType ItemType { get; }

        /// <summary>
        /// Gets the type of the item searching filter to use.
        /// </summary>
        public abstract ContentItemSearchFilter SearchFilter { get; }

        /// <summary>
        /// Gets a value indicating whether this instance is asset.
        /// </summary>
        public bool IsAsset => ItemType == ContentItemType.Asset;

        /// <summary>
        /// Gets a value indicating whether this instance is folder.
        /// </summary>
        public bool IsFolder => ItemType == ContentItemType.Folder;

        /// <summary>
        /// Gets a value indicating whether this instance can have children.
        /// </summary>
        public bool CanHaveChildren => ItemType == ContentItemType.Folder;

        /// <summary>
        /// Determines whether this item can be renamed.
        /// </summary>
        public virtual bool CanRename => true;

        /// <summary>
        /// Gets a value indicating whether this item can be dragged and dropped.
        /// </summary>
        public virtual bool CanDrag => true;

        /// <summary>
        /// Gets a value indicating whether this <see cref="ContentItem"/> exists on drive.
        /// </summary>
        public virtual bool Exists => System.IO.File.Exists(Path);

        /// <summary>
        /// Gets the parent folder.
        /// </summary>
        public ContentFolder ParentFolder
        {
            get => _parentFolder;
            set
            {
                if (_parentFolder == value)
                    return;

                // Remove from old
                _parentFolder?.Children.Remove(this);

                // Link
                _parentFolder = value;

                // Add to new
                _parentFolder?.Children.Add(this);

                OnParentFolderChanged();
            }
        }

        /// <summary>
        /// Gets the path to the item.
        /// </summary>
        public string Path { get; private set; }

        /// <summary>
        /// Gets the item short name (filename without extension).
        /// </summary>
        public string ShortName { get; private set; }

        /// <summary>
        /// Gets the asset name relative to the project root folder (without asset file extension)
        /// </summary>
        public string NamePath
        {
            get
            {
                string result = Path;
                if (result.StartsWith(Globals.ProjectFolder))
                {
                    result = result.Substring(Globals.ProjectFolder.Length + 1);
                }
                return StringUtils.GetPathWithoutExtension(result);
            }
        }

        /// <summary>
        /// Gets the default name of the content item thumbnail.
        /// Returns null if not used.
        /// </summary>
        public virtual Sprite DefaultThumbnail => Sprite.Invalid;

        /// <summary>
        /// Gets a value indicating whether this item has default thumbnail.
        /// </summary>
        public bool HasDefaultThumbnail => DefaultThumbnail.IsValid;

        /// <summary>
        /// Gets or sets the item thumbnail. Warning, thumbnail may not be available if item has no references (<see cref="ReferencesCount"/>).
        /// </summary>
        public Sprite Thumbnail
        {
            get => _thumbnail;
            set => _thumbnail = value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContentItem"/> class.
        /// </summary>
        /// <param name="path">The path to the item.</param>
        protected ContentItem(string path)
        : base(0, 0, DefaultWidth, DefaultHeight)
        {
            // Set path
            Path = path;
            ShortName = System.IO.Path.GetFileNameWithoutExtension(path);
        }

        /// <summary>
        /// Updates the item path. Use with caution or even don't use it. It's dangerous.
        /// </summary>
        /// <param name="value">The new path.</param>
        internal virtual void UpdatePath(string value)
        {
            Assert.AreNotEqual(Path, value);

            // Set path
            Path = StringUtils.NormalizePath(value);
            ShortName = System.IO.Path.GetFileNameWithoutExtension(value);

            // Fire event
            for (int i = 0; i < _references.Count; i++)
            {
                _references[i].OnItemRenamed(this);
            }
        }

        /// <summary>
        /// Refreshes the item thumbnail.
        /// </summary>
        public virtual void RefreshThumbnail()
        {
            // Skip if item has default thumbnail
            if (HasDefaultThumbnail)
                return;

            var thumbnails = Editor.Instance.Thumbnails;

            // Delete old thumbnail and remove it from the cache
            thumbnails.DeletePreview(this);

            // Request new one (if need to)
            if (_references.Count > 0)
            {
                thumbnails.RequestPreview(this);
            }
        }

        /// <summary>
        /// Tries to find the item at the specified path.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>Found item or null if missing.</returns>
        public virtual ContentItem Find(string path)
        {
            return Path == path ? this : null;
        }

        /// <summary>
        /// Tries to find a specified item in the assets tree.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>True if has been found, otherwise false.</returns>
        public virtual bool Find(ContentItem item)
        {
            return this == item;
        }

        /// <summary>
        /// Tries to find the item with the specified id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>Found item or null if missing.</returns>
        public virtual ContentItem Find(Guid id)
        {
            return null;
        }

        /// <summary>
        /// Tries to find script with the given name.
        /// </summary>
        /// <param name="scriptName">Name of the script.</param>
        /// <returns>Found script or null if missing.</returns>
        public virtual ScriptItem FindScriptWitScriptName(string scriptName)
        {
            return null;
        }

        /// <summary>
        /// Gets a value indicating whether draw item shadow.
        /// </summary>
        /// <value>
        ///   <c>true</c> if draw shadow; otherwise, <c>false</c>.
        /// </value>
        protected virtual bool DrawShadow => false;

        /// <summary>
        /// Gets the local space rectangle for element name text area.
        /// </summary>
        /// <value>
        /// The text rectangle.
        /// </value>
        public Rectangle TextRectangle
        {
            get
            {
                float width = Width;
                float textRectHeight = DefaultTextHeight * width / DefaultWidth;
                return new Rectangle(0, Height - textRectHeight, width, textRectHeight);
            }
        }

        /// <summary>
        /// Draws the item thumbnail.
        /// </summary>
        /// <param name="rectangle">The thumbnail rectangle.</param>
        public void DrawThumbnail(ref Rectangle rectangle)
        {
            // Draw shadow
            if (DrawShadow)
            {
                const float thumbnailInShadowSize = 50.0f;
                var shadowRect = rectangle.MakeExpanded((DefaultThumbnailSize - thumbnailInShadowSize) * rectangle.Width / DefaultThumbnailSize * 1.3f);
                if (!_shadowIcon.IsValid)
                    _shadowIcon = Editor.Instance.Icons.AssetShadow;
                Render2D.DrawSprite(_shadowIcon, shadowRect);
            }

            // Draw thumbnail
            if (_thumbnail.IsValid)
                Render2D.DrawSprite(_thumbnail, rectangle);
            else
                Render2D.FillRectangle(rectangle, Color.Black);
        }

        /// <summary>
        /// Gets the amount of references to that item.
        /// </summary>
        /// <value>
        /// The references count.
        /// </value>
        public int ReferencesCount => _references.Count;

        /// <summary>
        /// Adds the reference to the item.
        /// </summary>
        /// <param name="obj">The object.</param>
        public void AddReference(IContentItemOwner obj)
        {
            Assert.IsNotNull(obj);
            Assert.IsFalse(_references.Contains(obj));

            _references.Add(obj);

            // Check if need to generate preview
            if (_references.Count == 1 && !_thumbnail.IsValid)
            {
                RequestThumbnail();
            }
        }

        /// <summary>
        /// Removes the reference from the item.
        /// </summary>
        /// <param name="obj">The object.</param>
        public void RemoveReference(IContentItemOwner obj)
        {
            if (_references.Remove(obj))
            {
                // Check if need to release the preview
                if (_references.Count == 0 && _thumbnail.IsValid)
                {
                    ReleaseThumbnail();
                }
            }
        }

        /// <summary>
        /// Called when content item gets removed (by the user or externally).
        /// </summary>
        public virtual void OnDelete()
        {
            // Fire event
            while (_references.Count > 0)
            {
                var reference = _references[0];
                reference.OnItemDeleted(this);
                RemoveReference(reference);
            }

            // Release thumbnail
            if (_thumbnail.IsValid)
            {
                ReleaseThumbnail();
            }
        }

        /// <summary>
        /// Called when item parent folder gets changed.
        /// </summary>
        protected virtual void OnParentFolderChanged()
        {
        }

        /// <summary>
        /// Requests the thumbnail.
        /// </summary>
        protected void RequestThumbnail()
        {
            Editor.Instance.Thumbnails.RequestPreview(this);
        }

        /// <summary>
        /// Releases the thumbnail.
        /// </summary>
        protected void ReleaseThumbnail()
        {
            // Simply unlink sprite
            _thumbnail = Sprite.Invalid;
        }

        /// <summary>
        /// Called when item gets reimported or reloaded.
        /// </summary>
        protected virtual void OnReimport()
        {
            for (int i = 0; i < _references.Count; i++)
                _references[i].OnItemReimported(this);
            RefreshThumbnail();
        }

        /// <summary>
        /// Does the drag and drop operation with this asset.
        /// </summary>
        protected virtual void DoDrag()
        {
            if (!CanDrag)
                return;

            DragData data;

            // Check if is selected
            if (Parent is ContentView view && view.IsSelected(this))
            {
                // Drag selected item
                data = DragItems.GetDragData(view.Selection);
            }
            else
            {
                // Drag single item
                data = DragItems.GetDragData(this);
            }

            // Start drag operation
            DoDragDrop(data);
        }

        /// <inheritdoc />
        public override void Draw()
        {
            // Cache data
            var width = Width;
            var height = Height;
            var style = Style.Current;
            var view = Parent as ContentView;
            bool isSelected = view.IsSelected(this);
            var clientRect = new Rectangle(0, 0, width, height);
            float thumbnailSize = width - 2 * DefaultMarginSize;
            var thumbnailRect = new Rectangle(DefaultMarginSize, DefaultMarginSize, thumbnailSize, thumbnailSize);
            var textRect = TextRectangle;

            // Draw background
            if (isSelected)
                Render2D.FillRectangle(clientRect, Parent.ContainsFocus ? style.BackgroundSelected : style.LightBackground);
            else if (IsMouseOver)
                Render2D.FillRectangle(clientRect, style.BackgroundHighlighted);

            // Draw preview
            DrawThumbnail(ref thumbnailRect);

            // Draw short name
            Render2D.PushClip(ref textRect);
            Render2D.DrawText(style.FontMedium, ShortName, textRect, style.Foreground, TextAlignment.Center, TextAlignment.Center, TextWrapping.WrapWords, 0.75f, 0.95f);
            Render2D.PopClip();
        }

        /// <inheritdoc />
        public override bool OnMouseDown(Vector2 location, MouseButton buttons)
        {
            Focus();

            if (buttons == MouseButton.Left)
            {
                // Cache data
                _isMouseDown = true;
                _mouseDownStartPos = location;
            }

            return true;
        }

        /// <inheritdoc />
        public override bool OnMouseUp(Vector2 location, MouseButton buttons)
        {
            if (buttons == MouseButton.Left)
            {
                // Clear flag
                _isMouseDown = false;

                // Fire event
                (Parent as ContentView).OnItemClick(this);
            }

            return base.OnMouseUp(location, buttons);
        }

        /// <inheritdoc />
        public override bool OnMouseDoubleClick(Vector2 location, MouseButton buttons)
        {
            Focus();

            // Check if clicked on name area (and can be renamed)
            if (CanRename && TextRectangle.Contains(ref location))
            {
                // Rename
                (Parent as ContentView).OnItemDoubleClickName(this);
            }
            else
            {
                // Open
                (Parent as ContentView).OnItemDoubleClick(this);
            }

            return true;
        }

        /// <inheritdoc />
        public override void OnMouseMove(Vector2 location)
        {
            // Check if start drag and drop
            if (_isMouseDown && Vector2.Distance(_mouseDownStartPos, location) > 10.0f)
            {
                // Clear flag
                _isMouseDown = false;

                // Start drag drop
                DoDrag();
            }
        }

        /// <inheritdoc />
        public override void OnMouseLeave()
        {
            // Check if start drag and drop
            if (_isMouseDown)
            {
                // Clear flag
                _isMouseDown = false;

                // Start drag drop
                DoDrag();
            }

            base.OnMouseLeave();
        }

        /// <inheritdoc />
        public override int Compare(Control other)
        {
            if (other is ContentItem otherItem)
            {
                if (otherItem.IsFolder)
                    return 1;
                return string.Compare(ShortName, otherItem.ShortName, StringComparison.InvariantCulture);
            }

            return base.Compare(other);
        }

        /// <inheritdoc />
        public override void OnDestroy()
        {
            // Fire event
            while (_references.Count > 0)
            {
                var reference = _references[0];
                reference.OnItemDispose(this);
                RemoveReference(reference);
            }

            // Release thumbnail
            if (_thumbnail.IsValid)
            {
                ReleaseThumbnail();
            }

            base.OnDestroy();
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return Path;
        }
    }
}
