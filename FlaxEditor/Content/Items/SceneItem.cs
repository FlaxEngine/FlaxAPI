////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlaxEngine;

namespace FlaxEditor.Content
{
    /// <summary>
    /// Content item that contains <see cref="FlaxEngine.Scene"/> data.
    /// </summary>
    /// <seealso cref="FlaxEditor.Content.JsonAssetItem" />
    public sealed class SceneItem : JsonAssetItem
    {
        /// <summary>
        /// The scene asset typename.
        /// </summary>
        public const string SceneAssetTypename = "FlaxEngine.SceneAsset";

        /// <summary>
        /// Initializes a new instance of the <see cref="SceneItem"/> class.
        /// </summary>
        /// <param name="path">The asset path.</param>
        /// <param name="id">The asset identifier.</param>
        public SceneItem(string path, Guid id)
            : base(path, id, SceneAssetTypename)
        {
        }

        /// <inheritdoc />
        public override ContentDomain ItemDomain => ContentDomain.Scene;

        /// <inheritdoc />
        public override ContentItemType ItemType => ContentItemType.Scene;

        /// <inheritdoc />
        public override string DefaultPreviewName => "Scene64";
    }
}
