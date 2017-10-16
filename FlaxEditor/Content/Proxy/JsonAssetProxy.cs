////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

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
            return new JsonAssetItem(path, id, TypeName);
        }
    }
}
