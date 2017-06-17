////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System;
using FlaxEngine;

namespace FlaxEditor.Content
{
    /// <summary>
    /// Base class for all binary asset proxy objects used to manage <see cref="BinaryAssetItem"/>.
    /// </summary>
    /// <seealso cref="FlaxEditor.Content.AssetProxy" />
    public abstract class BinaryAssetProxy : AssetProxy
    {
        /// <summary>
        /// The binary asset files extension.
        /// </summary>
        public static readonly string Extension = "flax";
        
        // Check if proxy supports given asset type name id
        //virtual bool IsTypeNameID(uint32 typeNameID) const = 0;

        /*public override bool IsProxyFor(ContentItem item)
        {
            return item != null && item.IsAsset && IsTypeNameID(static_cast<AssetElement*>(el)->GetTypeID());
        }
        */
        /// <inheritdoc />
        public override string FileExtension => Extension;
    }
}
