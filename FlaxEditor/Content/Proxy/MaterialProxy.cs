////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System;
using FlaxEditor.Windows;
using FlaxEngine;

namespace FlaxEditor.Content
{
    /// <summary>
    /// A <see cref="Material"/> asset proxy object.
    /// </summary>
    /// <seealso cref="FlaxEditor.Content.BinaryAssetProxy" />
    public class MaterialProxy : BinaryAssetProxy
    {
        /// <inheritdoc />
        public override string Name => "Material";

        /// <inheritdoc />
        public override bool AcceptsTypeID(int typeID)
        {
            return typeID == Material.TypeID;
        }

        /// <inheritdoc />
        public override EditorWindow Open(ContentItem item)
        {
            throw new NotImplementedException();// TODO: material window
        }

        /// <inheritdoc />
        public override Color AccentColor => Color.FromRGB(0x16a085);

        /// <inheritdoc />
        public override ContentDomain Domain => Material.Domain;
    }
}
