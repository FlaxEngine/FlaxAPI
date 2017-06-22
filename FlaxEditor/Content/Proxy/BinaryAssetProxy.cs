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
        
        /// <summary>
        /// Checks if this proxy supports the given asset type id.
        /// </summary>
        /// <param name="typeID">The asset type identifier.</param>
        /// <returns>True if proxy supports assets of the given type id.</returns>
        public abstract bool AcceptsTypeID(int typeID);
        
        /// <inheritdoc />
        public override bool IsProxyFor(ContentItem item)
        {
            return item is BinaryAssetItem binaryAssetItem && AcceptsTypeID(binaryAssetItem.TypeID);
        }

        /// <inheritdoc />
        public override string FileExtension => Extension;
    }
}
