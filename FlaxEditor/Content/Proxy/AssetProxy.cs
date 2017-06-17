////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

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
    }
}
