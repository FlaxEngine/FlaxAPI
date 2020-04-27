// Copyright (c) 2012-2020 Wojciech Figat. All rights reserved.

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
        /// The type of the asset (the same as <see cref="AssetItem.TypeName"/> but cached as type reference).
        /// </summary>
        public readonly Type Type;

        /// <summary>
        /// Initializes a new instance of the <see cref="BinaryAssetItem"/> class.
        /// </summary>
        /// <param name="path">The asset path.</param>
        /// <param name="id">The asset identifier.</param>
        /// <param name="typeName">The asset type name identifier.</param>
        public BinaryAssetItem(string path, Guid id, string typeName)
        : base(path, typeName, ref id)
        {
            Type = Utilities.Utils.GetType(typeName);

            if (typeof(TextureBase).IsAssignableFrom(Type))
                SearchFilter = ContentItemSearchFilter.Texture;
            else if (typeof(MaterialBase).IsAssignableFrom(Type))
                SearchFilter = ContentItemSearchFilter.Material;
            else if (typeof(ModelBase).IsAssignableFrom(Type))
                SearchFilter = ContentItemSearchFilter.Model;
            else if (typeof(Prefab).IsAssignableFrom(Type))
                SearchFilter = ContentItemSearchFilter.Prefab;
            else if (typeof(SceneAsset).IsAssignableFrom(Type))
                SearchFilter = ContentItemSearchFilter.Scene;
            else if (typeof(AudioClip).IsAssignableFrom(Type))
                SearchFilter = ContentItemSearchFilter.Audio;
            else if (typeof(Animation).IsAssignableFrom(Type))
                SearchFilter = ContentItemSearchFilter.Animation;
            else if (typeof(ParticleEmitter).IsAssignableFrom(Type) || typeof(ParticleSystem).IsAssignableFrom(Type))
                SearchFilter = ContentItemSearchFilter.Particles;
            else
                SearchFilter = ContentItemSearchFilter.Other;
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
        public override ContentItemSearchFilter SearchFilter { get; }

        /// <inheritdoc />
        public override bool IsOfType(Type type)
        {
            return type.IsAssignableFrom(Type);
        }
    }
}
