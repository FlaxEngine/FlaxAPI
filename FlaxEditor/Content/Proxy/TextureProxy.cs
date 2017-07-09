////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System;
using FlaxEditor.Windows;
using FlaxEngine;

namespace FlaxEditor.Content
{
    /// <summary>
    /// A <see cref="Texture"/> asset proxy object.
    /// </summary>
    /// <seealso cref="FlaxEditor.Content.BinaryAssetProxy" />
    public sealed class TextureProxy : BinaryAssetProxy
    {
        /// <inheritdoc />
        public override string Name => "Texture";

        /// <inheritdoc />
        public override bool AcceptsTypeID(int typeID)
        {
            return typeID == Texture.TypeID;
        }

        /// <inheritdoc />
        public override EditorWindow Open(Editor editor, ContentItem item)
        {
            throw new NotImplementedException();// TODO: texture window
        }

        /// <inheritdoc />
        public override Color AccentColor => Color.FromRGB(0x25B84C);

        /// <inheritdoc />
        public override ContentDomain Domain => Texture.Domain;
    }
}
