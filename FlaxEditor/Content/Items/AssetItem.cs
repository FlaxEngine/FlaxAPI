// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System;
using FlaxEngine;

namespace FlaxEditor.Content
{
    /// <summary>
    /// Asset item object.
    /// </summary>
    /// <seealso cref="FlaxEditor.Content.ContentItem" />
    public abstract class AssetItem : ContentItem
    {
        /// <summary>
        /// Gets the asset unique identifier.
        /// </summary>
        public Guid ID { get; protected set; }

        /// <summary>
        /// Gets the asset type identifier.
        /// </summary>
        public string TypeName { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AssetItem"/> class.
        /// </summary>
        /// <param name="path">The asset path.</param>
        /// <param name="typeName">The asset type name.</param>
        /// <param name="id">The asset identifier.</param>
        protected AssetItem(string path, string typeName, ref Guid id)
        : base(path)
        {
            TypeName = typeName;
            ID = id;
        }

        /// <inheritdoc />
        public override ContentDomain ItemDomain => ContentDomain.Other;

        /// <inheritdoc />
        public override ContentItemType ItemType => ContentItemType.Asset;

        /// <inheritdoc />
        protected override bool DrawShadow => true;

        /// <inheritdoc />
        public override ContentItem Find(Guid id)
        {
            return id == ID ? this : null;
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return Path + ":" + ID;
        }
    }
}
