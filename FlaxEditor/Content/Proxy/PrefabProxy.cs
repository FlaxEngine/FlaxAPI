// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System;
using FlaxEditor.Windows;
using FlaxEngine;

namespace FlaxEditor.Content
{
    /// <summary>
    /// Content proxy for <see cref="PrefabItem"/>.
    /// </summary>
    /// <seealso cref="FlaxEditor.Content.JsonAssetBaseProxy" />
    public sealed class PrefabProxy : JsonAssetBaseProxy
    {
        /// <summary>
        /// The prefab files extension.
        /// </summary>
        public static readonly string Extension = "prefab";

        /// <summary>
        /// The prefab asset data typename.
        /// </summary>
        public static readonly string AssetTypename = typeof(Prefab).FullName;

        /// <inheritdoc />
        public override string Name => "Prefab";

        /// <inheritdoc />
        public override ContentDomain Domain => ContentDomain.Prefab;

        /// <inheritdoc />
        public override string FileExtension => Extension;

        /// <inheritdoc />
        public override EditorWindow Open(Editor editor, ContentItem item)
        {
            throw new NotImplementedException("TODO: opening and editing prefabs");
        }

        /// <inheritdoc />
        public override bool IsProxyFor(ContentItem item)
        {
            return item is PrefabItem;
        }

        /// <inheritdoc />
        public override Color AccentColor => Color.FromRGB(0x7eef21);

        /// <inheritdoc />
        public override string TypeName => AssetTypename;

        /// <inheritdoc />
        public override AssetItem ConstructItem(string path, string typeName, ref Guid id)
        {
            return new PrefabItem(path, id);
        }
    }
}
