////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System;
using FlaxEditor.Windows;
using FlaxEngine;

namespace FlaxEditor.Content
{
    /// <summary>
    /// A <see cref="SpriteAtlas"/> asset proxy object.
    /// </summary>
    /// <seealso cref="FlaxEditor.Content.BinaryAssetProxy" />
    public class SpriteAtlasProxy : BinaryAssetProxy
    {
        /// <inheritdoc />
        public override string Name => "Sprite Atlas";

        /// <inheritdoc />
        public override bool AcceptsTypeID(int typeID)
        {
            return typeID == SpriteAtlas.TypeID;
        }

        /// <inheritdoc />
        public override EditorWindow Open(Editor editor, ContentItem item)
        {
            throw new NotImplementedException();// TODO: sprite atlas window
        }

        /// <inheritdoc />
        public override Color AccentColor => Color.FromRGB(0x5C7F69);

        /// <inheritdoc />
        public override ContentDomain Domain => SpriteAtlas.Domain;
    }
}
