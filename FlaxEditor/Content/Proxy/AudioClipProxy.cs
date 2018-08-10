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
    /// A <see cref="AudioClip"/> asset proxy object.
    /// </summary>
    /// <seealso cref="FlaxEditor.Content.BinaryAssetProxy" />
    public class AudioClipProxy : BinaryAssetProxy
    {
        /// <inheritdoc />
        public override string Name => "Audio Clip";

        /// <inheritdoc />
        public override bool CanReimport(ContentItem item)
        {
            return true;
        }

        /// <inheritdoc />
        public override EditorWindow Open(Editor editor, ContentItem item)
        {
            return new AudioClipWindow(editor, (AssetItem)item);
        }

        /// <inheritdoc />
        public override Color AccentColor => Color.FromRGB(0xB3452B);

        /// <inheritdoc />
        public override ContentDomain Domain => ContentDomain.Audio;

        /// <inheritdoc />
        public override Type AssetType => typeof(AudioClip);

        /// <inheritdoc />
        public override void OnThumbnailDrawBegin(ThumbnailRequest request, ContainerControl guiRoot, GPUContext context)
        {
            guiRoot.AddChild(new Label
            {
                Text = "Audio",
                Size = guiRoot.Size,
                Wrapping = TextWrapping.WrapWords
            });
        }
    }
}
