////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System;
using FlaxEditor.Windows;
using FlaxEngine;

namespace FlaxEditor.Content
{
    /// <summary>
    /// A <see cref="CubeTexture"/> asset proxy object.
    /// </summary>
    /// <seealso cref="FlaxEditor.Content.BinaryAssetProxy" />
    public class CubeTextureProxy : BinaryAssetProxy
    {
        /// <inheritdoc />
        public override string Name => "Cube Texture";

        /// <inheritdoc />
        public override bool AcceptsTypeID(int typeID)
        {
            return typeID == CubeTexture.TypeID;
        }

        /// <inheritdoc />
        public override EditorWindow Open(Editor editor, ContentItem item)
        {
            throw new NotImplementedException();// TODO: cube texture window
        }

        /// <inheritdoc />
        public override Color AccentColor => Color.FromRGB(0x3498db);

        /// <inheritdoc />
        public override ContentDomain Domain => CubeTexture.Domain;
    }
}
