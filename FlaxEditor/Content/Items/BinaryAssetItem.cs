// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

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
        /// Initializes a new instance of the <see cref="BinaryAssetItem"/> class.
        /// </summary>
        /// <param name="path">The asset path.</param>
        /// <param name="id">The asset identifier.</param>
        /// <param name="typeName">The asset type name identifier.</param>
        /// <param name="domain">The asset domain.</param>
        public BinaryAssetItem(string path, Guid id, string typeName, ContentDomain domain)
        : base(path, typeName, ref id)
        {
            ItemDomain = domain;

            switch (domain)
            {
            case ContentDomain.Texture:
            case ContentDomain.CubeTexture:
                SearchFilter = ContentItemSearchFilter.Texture;
                break;
            case ContentDomain.Material:
                SearchFilter = ContentItemSearchFilter.Material;
                break;
            case ContentDomain.Model:
                SearchFilter = ContentItemSearchFilter.Model;
                break;
            case ContentDomain.Prefab:
                SearchFilter = ContentItemSearchFilter.Prefab;
                break;
            case ContentDomain.Scene:
                SearchFilter = ContentItemSearchFilter.Scene;
                break;
            case ContentDomain.Audio:
                SearchFilter = ContentItemSearchFilter.Audio;
                break;
            case ContentDomain.Animation:
                SearchFilter = ContentItemSearchFilter.Animation;
                break;
            default:
                SearchFilter = ContentItemSearchFilter.Other;
                break;
            }
        }

        /// <summary>
        /// Gets the asset import path.
        /// </summary>
        /// <param name="importPath">The import path.</param>
        /// <returns>True if fails, otherwise false.</returns>
        public bool GetImportPath(out string importPath)
        {
            // TODO: add internal call to content backend with fast import asset metadata gather (without asset loading)

            var asset = FlaxEngine.Content.LoadAsync(ID);
            if (asset is BinaryAsset binaryAsset)
            {
                binaryAsset.WaitForLoaded(100);

                // Get meta from loaded asset
                importPath = binaryAsset.ImportPath;
                return string.IsNullOrEmpty(importPath);
            }

            importPath = string.Empty;
            return true;
        }

        internal void OnReimport(ref Guid id)
        {
            ID = id;
            OnReimport();
        }

        /// <inheritdoc />
        public override ContentDomain ItemDomain { get; }

        /// <inheritdoc />
        public override ContentItemSearchFilter SearchFilter { get; }
    }
}
