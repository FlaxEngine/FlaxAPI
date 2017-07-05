////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System;
using FlaxEngine;

namespace FlaxEditor.Content
{
    /// <summary>
    /// Base class for all asset proxy objects used to manage <see cref="AssetItem"/>.
    /// </summary>
    /// <seealso cref="FlaxEditor.Content.ContentProxy" />
    public abstract class AssetProxy : ContentProxy
    {
        /// <summary>
        /// Gets the assets domain.
        /// </summary>
        /// <value>
        /// The domain.
        /// </value>
        public abstract ContentDomain Domain { get; }

        /// <inheritdoc />
        public override bool IsAsset => true;

        /// <summary>
        /// Checks if this proxy supports the given asset type id at the given path.
        /// </summary>
        /// <param name="typeID">The asset type identifier.</param>
        /// <param name="path">The asset path.</param>
        /// <returns>True if proxy supports assets of the given type id and path.</returns>
        public virtual bool AcceptsAsset(int typeID, string path)
        {
            return path.EndsWith(FileExtension, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Constructs the item for the asset.
        /// </summary>
        /// <param name="path">The asset path.</param>
        /// <param name="typeId">The asset type identifier.</param>
        /// <param name="id">The asset identifier.</param>
        /// <returns>Created item.</returns>
        public abstract AssetItem ConstructItem(string path, int typeId, ref Guid id);
    }
}
