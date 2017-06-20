////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System;

namespace FlaxEditor.Content
{
    /// <summary>
    /// Asset item stored in a Json format file.
    /// </summary>
    /// <seealso cref="FlaxEditor.Content.AssetItem" />
    public class JsonAssetItem : AssetItem
    {
        /// <summary>
        /// Gets the name of the asset type.
        /// </summary>
        /// <value>
        /// The name of the type.
        /// </value>
        public string TypeName { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="JsonAssetItem"/> class.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="id">The identifier.</param>
        /// <param name="typeName">Name of the resource type.</param>
        public JsonAssetItem(string path, Guid id, string typeName)
            : base(path, id)
        {
            TypeName = typeName;
        }

        /// <inheritdoc />
        public override string DefaultThumbnailName => "Document64";
    }
}
