////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    };

    /// <summary>
    /// Base class for all content items.
    /// </summary>
    /// <seealso cref="FlaxEngine.GUI.Control" />
    public abstract class ContentItem : Control
    {
        public const int DefaultMarginSize = 4;
        public const int DefaultTextHeight = 42;
        public const int DefaultIconSize = PreviewsCache.AssetIconSize;
        public const int DefaultWidth = (DefaultIconSize + 2 * DefaultMarginSize);
        public const int DefaultHeight = (DefaultIconSize + 2 * DefaultMarginSize + DefaultTextHeight);

        protected bool _isMouseDown;
        protected Vector2 _mouseDownStartPos;
        protected readonly List<IContentItemOwner> _references = new List<IContentItemOwner>(4);

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
        /// Gets the parent folder.
        /// </summary>
        /// <value>
        /// The parent folder.
        /// </value>
        public ContentFolder ParentFolder { get; private set; }

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
        /// Gets the amount of references to that item.
        /// </summary>
        /// <value>
        /// The references count.
        /// </value>
        public int ReferencesCount => _references.Count;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContentItem"/> class.
        /// </summary>
        /// <param name="path">The path to the item.</param>
        protected ContentItem(string path)
            : base(true, 0, 0, DefaultWidth, DefaultHeight)
        {
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
    }
}
