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
    public class IESProfileProxy : BinaryAssetProxy
    {
        /// <inheritdoc />
        public override string Name => "IES Profile";

        /// <inheritdoc />
        public override bool AcceptsTypeID(int typeID)
        {
            return typeID == IESProfile.TypeID;
        }

        /// <inheritdoc />
        public override bool CanReimport(ContentItem item)
        {
            return true;
        }

        /// <inheritdoc />
        public override EditorWindow Open(Editor editor, ContentItem item)
        {
            throw new NotImplementedException();// TODO: ies profile window
        }

        /// <inheritdoc />
        public override Color AccentColor => Color.FromRGB(0x695C7F);

        /// <inheritdoc />
        public override ContentDomain Domain => IESProfile.Domain;
    }
}
