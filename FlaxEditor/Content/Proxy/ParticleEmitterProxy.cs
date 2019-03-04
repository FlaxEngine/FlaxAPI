// Copyright (c) 2012-2019 Wojciech Figat. All rights reserved.

using System;
using FlaxEditor.Windows;
using FlaxEngine;

namespace FlaxEditor.Content
{
    /// <summary>
    /// A <see cref="ParticleEmitter"/> asset proxy object.
    /// </summary>
    /// <seealso cref="FlaxEditor.Content.BinaryAssetProxy" />
    public class ParticleEmitterProxy : BinaryAssetProxy
    {
        /// <inheritdoc />
        public override string Name => "Particle Emitter";

        /// <inheritdoc />
        public override EditorWindow Open(Editor editor, ContentItem item)
        {
            throw new NotImplementedException();
            //return new ParticleEmitterWindow(editor, item as AssetItem);
        }

        /// <inheritdoc />
        public override Color AccentColor => Color.FromRGB(0xFF79D2B0);

        /// <inheritdoc />
        public override ContentDomain Domain => ContentDomain.Other;

        /// <inheritdoc />
        public override Type AssetType => typeof(ParticleEmitter);

        /// <inheritdoc />
        public override bool CanCreate(ContentFolder targetLocation)
        {
            return targetLocation.CanHaveAssets;
        }

        /// <inheritdoc />
        public override void Create(string outputPath, object arg)
        {
            if (Editor.CreateAsset(Editor.NewAssetType.ParticleEmitter, outputPath))
                throw new Exception("Failed to create new asset.");
        }
    }
}
