////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System;
using FlaxEditor.Windows;
using FlaxEngine;

namespace FlaxEditor.Content
{
    /// <summary>
    /// A <see cref="MaterialInstance"/> asset proxy object.
    /// </summary>
    /// <seealso cref="FlaxEditor.Content.BinaryAssetProxy" />
    public class MaterialInstanceProxy : BinaryAssetProxy
    {
        /// <inheritdoc />
        public override string Name => "Material Instance";

        /// <inheritdoc />
        public override bool AcceptsTypeID(int typeID)
        {
            return typeID == MaterialInstance.TypeID;
        }

        /// <inheritdoc />
        public override EditorWindow Open(ContentItem item)
        {
            throw new NotImplementedException();// TODO: material instance window
        }

        /// <inheritdoc />
        public override Color AccentColor => Color.FromRGB(0x2c3e50);

        /// <inheritdoc />
        public override ContentDomain Domain => MaterialInstance.Domain;
    }
}
