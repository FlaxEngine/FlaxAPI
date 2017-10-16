////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System;
using FlaxEngine;

namespace FlaxEditor.Content
{
    /// <summary>
    /// Asset item stored in a Json format file.
    /// </summary>
    /// <seealso cref="FlaxEditor.Content.AssetItem" />
    public class JsonAssetItem : AssetItem
    {
        /// <summary>
        /// Gets the name of the asset data type.
        /// </summary>
        public string DataTypeName { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="JsonAssetItem"/> class.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="id">The identifier.</param>
        /// <param name="typeName">Name of the resource type.</param>
        public JsonAssetItem(string path, Guid id, string typeName)
            : base(path, id)
        {
            DataTypeName = typeName;
        }

        /// <inheritdoc />
        public override ContentDomain ItemDomain => ContentDomain.Document;

        /// <inheritdoc />
        public override string DefaultThumbnailName => "Document64";

        /// <inheritdoc />
        protected override bool DrawShadow => false;
    }
}
