// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System;
using FlaxEditor.Content.Thumbnails;
using FlaxEditor.Windows;
using FlaxEditor.Windows.Assets;
using FlaxEngine;
using FlaxEngine.GUI;
using FlaxEngine.Rendering;

namespace FlaxEditor.Content
{
    /// <summary>
    /// A <see cref="FontAsset"/> asset proxy object.
    /// </summary>
    /// <seealso cref="FlaxEditor.Content.BinaryAssetProxy" />
    public class FontProxy : BinaryAssetProxy
    {
        /// <inheritdoc />
        public override string Name => "Font";

        /// <inheritdoc />
        public override bool CanReimport(ContentItem item)
        {
            return true;
        }

        /// <inheritdoc />
        public override EditorWindow Open(Editor editor, ContentItem item)
        {
            return new FontAssetWindow(editor, (AssetItem)item);
        }

        /// <inheritdoc />
        public override Color AccentColor => Color.FromRGB(0x2D74B2);

        /// <inheritdoc />
        public override ContentDomain Domain => ContentDomain.Font;

        /// <inheritdoc />
        public override Type AssetType => typeof(FontAsset);

        /// <inheritdoc />
        public override void OnThumbnailDrawBegin(ThumbnailRequest request, ContainerControl guiRoot, GPUContext context)
        {
            var asset = FlaxEngine.Content.Load<FontAsset>(request.Item.Path);
            guiRoot.AddChild(new Label
            {
                Text = asset.FamilyName,
                Size = guiRoot.Size,
                Wrapping = TextWrapping.WrapWords
            });
        }
    }
}
