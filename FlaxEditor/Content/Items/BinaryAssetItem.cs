////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System;
using FlaxEngine;

namespace FlaxEditor.Content
{
    /// <summary>
    /// Represents binary asset item.
    /// </summary>
    /// <seealso cref="FlaxEditor.Content.AssetItem" />
    public class BinaryAssetItem : AssetItem
    {
        /// <summary>
        /// Gets the asset type identifier.
        /// </summary>
        /// <value>
        /// The asset type identifier.
        /// </value>
        public int TypeID { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BinaryAssetItem"/> class.
        /// </summary>
        /// <param name="path">The asset path.</param>
        /// <param name="id">The asset identifier.</param>
        /// <param name="typeId">The asset type identifier.</param>
        /// <param name="domain">The asset domain.</param>
        public BinaryAssetItem(string path, Guid id, int typeId, ContentDomain domain)
            : base(path, id)
        {
            TypeID = typeId;
            ItemDomain = domain;
        }

        /// <summary>
        /// Gets the asset import path.
        /// </summary>
        /// <param name="importPath">The import path.</param>
        /// <returns>True if fails, otherwise false.</returns>
        public bool GetImportPath(out string importPath)
        {
            var asset = FlaxEngine.Content.LoadAsync(ID);
            if (asset is BinaryAsset binaryAsset)
            {
                // Get meta from loaded asset
                importPath = binaryAsset.ImportPath;
                return string.IsNullOrEmpty(importPath);
            }

            importPath = string.Empty;
            return true;
        }

        /// <inheritdoc />
        public override ContentDomain ItemDomain { get; }
    }
}
