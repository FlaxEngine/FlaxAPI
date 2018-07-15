// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System;
using FlaxEditor.Windows;
using FlaxEditor.Windows.Assets;
using FlaxEngine;

namespace FlaxEditor.Content
{
    /// <summary>
    /// Base class for all Json asset proxy objects used to manage <see cref="JsonAssetItem"/>.
    /// </summary>
    /// <seealso cref="FlaxEditor.Content.AssetProxy" />
    public abstract class JsonAssetBaseProxy : AssetProxy
    {
    }

    /// <summary>
    /// Json assets proxy.
    /// </summary>
    /// <seealso cref="FlaxEditor.Content.JsonAssetBaseProxy" />
    public abstract class JsonAssetProxy : JsonAssetBaseProxy
    {
        /// <summary>
        /// The json files extension.
        /// </summary>
        public static readonly string Extension = "json";

        /// <inheritdoc />
        public override string Name => "Json";

        /// <inheritdoc />
        public override ContentDomain Domain => ContentDomain.Document;

        /// <inheritdoc />
        public override string FileExtension => Extension;

        /// <inheritdoc />
        public override EditorWindow Open(Editor editor, ContentItem item)
        {
            return new JsonAssetWindow(editor, (JsonAssetItem)item);
        }

        /// <inheritdoc />
        public override bool IsProxyFor(ContentItem item)
        {
            return item is JsonAssetItem;
        }

        /// <inheritdoc />
        public override Color AccentColor => Color.FromRGB(0xd14f67);

        /// <inheritdoc />
        public override bool AcceptsAsset(string typeName, string path)
        {
            return typeName == TypeName && base.AcceptsAsset(typeName, path);
        }

        /// <inheritdoc />
        public override AssetItem ConstructItem(string path, string typeName, ref Guid id)
        {
            return new JsonAssetItem(path, id, typeName);
        }
    }

    /// <summary>
    /// Generic Json assets proxy (supports all json assets that don't have dedicated proxy).
    /// </summary>
    /// <seealso cref="FlaxEditor.Content.JsonAssetBaseProxy" />
    public sealed class GenericJsonAssetProxy : JsonAssetProxy
    {
        /// <inheritdoc />
        public override string TypeName => typeof(JsonAsset).FullName;

        /// <inheritdoc />
        public override bool AcceptsAsset(string typeName, string path)
        {
            return path.EndsWith(FileExtension, StringComparison.OrdinalIgnoreCase);
        }
    }

    /// <summary>
    /// Content proxy for a json assets of the given type that can be spawned in the editor.
    /// </summary>
    /// <seealso cref="FlaxEditor.Content.JsonAssetProxy" />
    public sealed class SpawnableJsonAssetProxy<T> : JsonAssetProxy where T : new()
    {
        /// <inheritdoc />
        public override string Name { get; } = CustomEditors.CustomEditorsUtil.GetPropertyNameUI(typeof(T).Name);

        /// <inheritdoc />
        public override bool CanCreate(ContentFolder targetLocation)
        {
            return targetLocation.CanHaveAssets;
        }

        /// <inheritdoc />
        public override void Create(string outputPath, object arg)
        {
            Editor.SaveJsonAsset(outputPath, new T());
        }

        /// <inheritdoc />
        public override string TypeName { get; } = typeof(T).FullName;
    }
}
