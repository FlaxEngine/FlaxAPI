////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using FlaxEditor.Content.GUI;
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
        /// Called when referenced item gets disposed (editor closing, database inetrnal changes, etc.).
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
        public const int DefaultMarginSize = 4;
        public const int DefaultTextHeight = 42;
        public const int DefaultIconSize = PreviewsCache.AssetIconSize;
        public const int DefaultWidth = (DefaultIconSize + 2 * DefaultMarginSize);
        public const int DefaultHeight = (DefaultIconSize + 2 * DefaultMarginSize + DefaultTextHeight);

        private ContentFolder _parentFolder;

        protected bool _isMouseDown;
        protected Vector2 _mouseDownStartPos;
        protected readonly List<IContentItemOwner> _references = new List<IContentItemOwner>(4);

        protected Sprite _icon;
        protected Sprite _shadow;

        /// <summary>
        /// Gets the item domain.
        /// </summary>
        /// <value>
        /// The item domain.
        /// </value>
        public virtual ContentDomain ItemDomain => ContentDomain.Invalid;

        /// <summary>
        /// Gets the type of the item.
        /// </summary>
        /// <value>
        /// The type of the item.
        /// </value>
        public abstract ContentItemType ItemType { get; }

        /// <summary>
        /// Gets a value indicating whether this instance is asset.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is asset; otherwise, <c>false</c>.
        /// </value>
        public bool IsAsset => ItemType == ContentItemType.Asset;

        /// <summary>
        /// Gets a value indicating whether this instance is folder.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is folder; otherwise, <c>false</c>.
        /// </value>
        public bool IsFolder => ItemType == ContentItemType.Folder;

        /// <summary>
        /// Gets a value indicating whether this instance can have children.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance can have children; otherwise, <c>false</c>.
        /// </value>
        public bool CanHaveChildren => ItemType == ContentItemType.Folder;

        /// <summary>
        /// Determines whether this item can be renamed.
        /// </summary>
        /// <returns>True if this item can be renamed, otherwise false.</returns>
        public virtual bool CanRename => true;

        /// <summary>
        /// Gets the parent folder.
        /// </summary>
        /// <value>
        /// The parent folder.
        /// </value>
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
            }
        }

        /// <summary>
        /// Gets the path to the item.
        /// </summary>
        /// <value>
        /// The item path.
        /// </value>
        public string Path { get; private set; }

        /// <summary>
        /// Gets the item short name (filename without extension).
        /// </summary>
        /// <value>
        /// The item short name.
        /// </value>
        public string ShortName { get; private set; }

        /// <summary>
        /// Gets the asset name relative to the project root folder (without asset file extension)
        /// </summary>
        /// <value>
        /// The name path.
        /// </value>
        public string NamePath
        {
            get
            {
                throw new NotImplementedException();
                /*string result = Path;
                if (result.StartsWith(Globals::Paths::ProjectFolder))
                {
                    result = result.Substring(Globals::Paths::ProjectFolder.Length() + 1);
                }
                return StringUtils::GetPathWithoutExtension(result);*/
            }
        }

        /// <summary>
        /// Gets the default name of the content item icon.
        /// Returns null if not used.
        /// </summary>
        /// <value>
        /// The default name of the preview.
        /// </value>
        public virtual string DefaultPreviewName => null;

        /// <summary>
        /// Gets or sets the icon. Warning, icon may not be available if item has no references (<see cref="ReferencesCount"/>).
        /// </summary>
        /// <value>
        /// The icon.
        /// </value>
        public Sprite Icon
        {
            get => _icon;
            set => _icon = value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContentItem"/> class.
        /// </summary>
        /// <param name="path">The path to the item.</param>
        protected ContentItem(string path)
            : base(true, 0, 0, DefaultWidth, DefaultHeight)
        {
            // Set path
            Path = path;
            ShortName = System.IO.Path.GetFileNameWithoutExtension(path);
        }

        /// <summary>
        /// Updates the item path. Use with caution or even don't use it. It's dangerous.
        /// </summary>
        /// <param name="value">The new path.</param>
        public void UpdatePath(string value)
        {
            Assert.AreNotEqual(Path, value);

            // Set path
            Path = value;
            ShortName = System.IO.Path.GetFileNameWithoutExtension(value);

            // Fire event
            for (int i = 0; i < _references.Count; i++)
            {
                _references[i].OnItemRenamed(this);
            }
        }

        /// <summary>
        /// Refreshes the item preview.
        /// </summary>
        public virtual void RefreshPreview()
        {
            // Skip if item has default preview
            if (DefaultPreviewName != null)
                return;

            throw new NotImplementedException();

            /*auto manager = CWindowsModule->ContentWin->GetPreviewManager();

            // Delete preview and remove it from cache
            manager->DeletePreview(this);

            // Request icon
            manager->LoadPreview(this);*/
        }

        /// <summary>
        /// Trues to find the item at the specified path.
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
        /// Trues to find the item with the specified id.
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
                float textRectHeight = DefaultTextHeight * Width / DefaultWidth;
                return new Rectangle(2, Height - textRectHeight, Width - 4, textRectHeight);
            }
        }

        /// <summary>
        /// Draws the item icon.
        /// </summary>
        /// <param name="iconRect">The icon rectangle.</param>
        public void DrawIcon(ref Rectangle iconRect)
        {
            // Draw shadow
            /*if (DrawShadow)
            {
                const float iconInShadowSize = 50.0f;
                var shadowRect = iconRect.MakeExpanded((DefaultIconSize - iconInShadowSize) * iconRect.Width / DefaultIconSize * 1.3f);
                if (!_shadow.IsValid)
                    _shadow = CUIModule->GetIcon(TEXT("AssetShadow"));
                Render2D.DrawSprite(_shadow, shadowRect);
            }

            // Draw icon
            if (_icon.IsValid)
                Render2D.DrawSprite(_icon, iconRect);
            else*/
                Render2D.FillRectangle(iconRect, Color.Black);
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
            if (_references.Count > 0 && !_icon.IsValid)
            {
                RequestIcon();
            }
        }

        /// <summary>
        /// Removes the reference from the item.
        /// </summary>
        /// <param name="obj">The object.</param>
        public void RemoveReference(IContentItemOwner obj)
        {
            Assert.IsTrue(_references.Contains(obj));

            _references.Remove(obj);

            // Check if need to release the preview
            if (_references.Count == 0 && _icon.IsValid)
            {
                ReleaseIcon();
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

            // Release icon
            if (_icon.IsValid)
            {
                ReleaseIcon();
            }
        }

        /// <summary>
        /// Requests the icon.
        /// </summary>
        protected void RequestIcon()
        {
            // TODO: call previews manager
            //CWindowsModule->ContentWin->GetPreviewManager()->LoadPreview(this);
        }

        /// <summary>
        /// Releases the icon.
        /// </summary>
        protected void ReleaseIcon()
        {
            // TODO: call previews manager
            //CWindowsModule->ContentWin->GetPreviewManager()->ReleasePreview(this);
        }

        /// <summary>
        /// Does the drag and drop operation with this asset.
        /// </summary>
        protected void DoDrag()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public override void Draw()
        {
            /*// Cache data
            var width = Width;
            var height = Height;
            var style = Style.Current;
            var view = Parent as ContentView;
            bool isSelected = view.IsSelected(this);
            var clientRect = new Rectangle(0, 0, width, height);
            float iconSize = width - 2 * DefaultMarginSize;
            var iconRect = new Rectangle(DefaultMarginSize, DefaultMarginSize, iconSize, iconSize);
            var textRect = TextRectangle;

            // Draw background
            if (isSelected)
                Render2D.FillRectangle(clientRect, Parent.ContainsFocus ? style.BackgroundSelected : style.LightBackground);
            else if (IsMouseOver)
                Render2D.FillRectangle(clientRect, style.BackgroundHighlighted);

            // Draw preview
            DrawIcon(ref iconRect);

            // Draw short name
            Render2D.DrawText(style.FontMedium, ShortName, textRect, style.Foreground, TextAlignment.Center, TextAlignment.Center, TextWrapping.WrapWords, 0.75f, 0.95f);*/

            Render2D.FillRectangle(new Rectangle(0, 0, Width, Height), Color.Purple);
        }

        /// <inheritdoc />
        public override bool OnMouseDown(Vector2 location, MouseButtons buttons)
        {
            Focus();

            if (buttons == MouseButtons.Left)
            {
                // Cache data
                _isMouseDown = true;
                _mouseDownStartPos = location;
            }

            return true;
        }

        /// <inheritdoc />
        public override bool OnMouseUp(Vector2 location, MouseButtons buttons)
        {
            if (buttons == MouseButtons.Left)
            {
                // Clear flag
                _isMouseDown = false;

                // Fire event
                (Parent as ContentView).OnItemClick(this);
            }

            return base.OnMouseUp(location, buttons);
        }

        /// <inheritdoc />
        public override bool OnMouseDoubleClick(Vector2 location, MouseButtons buttons)
        {
            Focus();

            // Check if clicked on name area (and can be renamed)
            if (CanRename && TextRectangle.Contains(location))
            {
                // Rename
                (Parent as ContentView).OnRename(this);
            }
            else
            {
                // Open
                (Parent as ContentView).OnOpen(this);
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
        public override void OnDestroy()
        {
            // Fire event
            while (_references.Count > 0)
            {
                var reference = _references[0];
                reference.OnItemDispose(this);
                RemoveReference(reference);
            }

            // Release icon
            if (_icon.IsValid)
            {
                ReleaseIcon();
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
