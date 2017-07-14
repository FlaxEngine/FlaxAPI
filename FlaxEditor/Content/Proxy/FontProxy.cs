////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System;
using FlaxEditor.Content.Thumbnails;
using FlaxEditor.Modules;
using FlaxEditor.Windows;
using FlaxEngine;
using FlaxEngine.GUI;
using FlaxEngine.Rendering;

namespace FlaxEditor.Content
{
    /// <summary>
    /// A <see cref="Texture"/> asset proxy object.
    /// </summary>
    /// <seealso cref="FlaxEditor.Content.BinaryAssetProxy" />
    public class FontProxy : BinaryAssetProxy
    {
        /// <inheritdoc />
        public override string Name => "Font";

        /// <inheritdoc />
        public override bool AcceptsTypeID(int typeID)
        {
            return typeID == FontAsset.TypeID;
        }

        /// <inheritdoc />
        public override EditorWindow Open(Editor editor, ContentItem item)
        {
            throw new NotImplementedException();// TODO: font window
        }

        /// <inheritdoc />
        public override Color AccentColor => Color.FromRGB(0x2D74B2);

        /// <inheritdoc />
        public override ContentDomain Domain => FontAsset.Domain;
        
        /// <inheritdoc />
        public override void OnThumbnailDrawBegin(ThumbnailRequest request, ContainerControl guiRoot, GPUContext context)
        {
            var asset = FlaxEngine.Content.Load<FontAsset>(request.Item.Path);
            guiRoot.AddChild(new Label(false, Vector2.Zero, guiRoot.Size)
            {
                Text = asset.FamilyName,
                Wrapping = TextWrapping.WrapWords
            });
        }
    }
}
