// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System;
using System.Collections.Generic;
using FlaxEditor.Content;
using FlaxEngine;
using FlaxEngine.GUI;

namespace FlaxEditor.GUI.Drag
{
    /// <summary>
    /// Helper class for handling <see cref="AssetItem"/> drag and drop.
    /// </summary>
    /// <seealso cref="AssetItem" />
    public sealed class DragAssets : DragHelper<AssetItem, DragEventArgs>
    {
        /// <summary>
        /// The default prefix for drag data used for <see cref="ContentItem"/>.
        /// </summary>
        public const string DragPrefix = DragItems.DragPrefix;

        /// <summary>
        /// Creates a new DragHelper
        /// </summary>
        /// <param name="validateFunction">The validation function</param>
        public DragAssets(Func<AssetItem, bool> validateFunction) : base(validateFunction)
        {
        }

        /// <summary>
        /// Gets the drag data (finds asset item).
        /// </summary>
        /// <param name="asset">The asset.</param>
        /// <returns>The data.</returns>
        public DragData ToDragData(Asset asset) => GetDragData(asset);

        /// <inheritdoc/>
        public override DragData ToDragData(AssetItem item) => GetDragData(item);

        /// <inheritdoc/>
        public override DragData ToDragData(IEnumerable<AssetItem> items) => GetDragData(items);


        public static DragData GetDragData(Asset asset)
        {
            return DragItems.GetDragData(Editor.Instance.ContentDatabase.Find(asset.ID));
        }
        public static DragData GetDragData(AssetItem item)
        {
            return DragItems.GetDragData(item);
        }

        public static DragData GetDragData(IEnumerable<AssetItem> items)
        {
            return DragItems.GetDragData(items);
        }

        /// <summary>
        /// Tries to parse the drag data to extract <see cref="AssetItem"/> collection.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns>Gathered objects or empty array if cannot get any valid.</returns>
        public override IEnumerable<AssetItem> FromDragData(DragData data)
        {
            if (data is DragDataText dataText)
            {
                if (dataText.Text.StartsWith(DragPrefix))
                {
                    // Remove prefix and parse splitted names
                    var paths = dataText.Text.Remove(0, DragPrefix.Length).Split('\n');
                    var results = new List<AssetItem>(paths.Length);
                    for (int i = 0; i < paths.Length; i++)
                    {
                        // Find element
                        var obj = Editor.Instance.ContentDatabase.Find(paths[i]) as AssetItem;

                        // Check it
                        if (obj != null)
                            results.Add(obj);
                    }

                    return results.ToArray();
                }
            }
            return new AssetItem[0];
        }

        public override void DragDrop(DragEventArgs dragEventArgs, IEnumerable<AssetItem> item)
        {

        }
    }
}
