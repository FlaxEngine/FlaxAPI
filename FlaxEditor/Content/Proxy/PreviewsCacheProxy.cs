////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using FlaxEditor.Windows;
using FlaxEditor.Windows.Assets;
using FlaxEngine;

namespace FlaxEditor.Content
{
    /// <summary>
    /// A <see cref="PreviewsCache"/> asset proxy object.
    /// </summary>
    /// <seealso cref="FlaxEditor.Content.BinaryAssetProxy" />
    public class PreviewsCacheProxy : BinaryAssetProxy
    {
        /// <inheritdoc />
        public override string Name => "Previews Cache";

        /// <inheritdoc />
        public override bool AcceptsTypeID(int typeID)
        {
            return typeID == PreviewsCache.TypeID;
        }

        /// <inheritdoc />
        public override EditorWindow Open(Editor editor, ContentItem item)
        {
            return new PreviewsCacheWindow(editor, (AssetItem)item);
        }

        /// <inheritdoc />
        public override Color AccentColor => Color.FromRGB(0x80FFAE);

        /// <inheritdoc />
        public override ContentDomain Domain => PreviewsCache.Domain;
    }
}
