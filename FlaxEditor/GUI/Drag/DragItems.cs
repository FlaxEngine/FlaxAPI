// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System;
using System.Collections.Generic;
using FlaxEditor.Content;
using FlaxEngine.GUI;

namespace FlaxEditor.GUI.Drag
{
    /// <summary>
    /// Helper class for handling <see cref="ContentItem"/> drag and drop.
    /// </summary>
    /// <seealso cref="ContentItem" />
    public sealed class DragItems : DragHelper<ContentItem>
    {
        /// <summary>
        /// The default prefix for drag data used for <see cref="ContentItem"/>.
        /// </summary>
        public const string DragPrefix = "ASSET!?";

        /// <inheritdoc />
        protected override void GetherObjects(DragDataText data, Func<ContentItem, bool> validateFunc)
        {
            var items = ParseData(data);
            for (int i = 0; i < items.Length; i++)
            {
                if (validateFunc(items[i]))
                    Objects.Add(items[i]);
            }
        }

        /// <summary>
        /// Tries to parse the drag data to extract <see cref="ContentItem"/> collection.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns>Gathered objects or empty array if cannot get any valid.</returns>
        public static ContentItem[] ParseData(DragDataText data)
        {
            if (data.Text.StartsWith(DragPrefix))
            {
                // Remove prefix and parse splited names
                var paths = data.Text.Remove(0, DragPrefix.Length).Split('\n');
                var results = new List<ContentItem>(paths.Length);
                for (int i = 0; i < paths.Length; i++)
                {
                    // Find element
                    var obj = Editor.Instance.ContentDatabase.Find(paths[i]);

                    // Check it
                    if (obj != null)
                        results.Add(obj);
                }

                return results.ToArray();
            }

            return new ContentItem[0];
        }

        /// <summary>
        /// Gets the drag data.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>The data.</returns>
        public static DragDataText GetDragData(ContentItem item)
        {
            if (item == null)
                throw new ArgumentNullException();

            return new DragDataText(DragPrefix + item.Path);
        }

        /// <summary>
        /// Gets the drag data.
        /// </summary>
        /// <param name="path">The full content item path.</param>
        /// <returns>The data.</returns>
        public static DragDataText GetDragData(string path)
        {
            if (path == null)
                throw new ArgumentNullException();

            return new DragDataText(DragPrefix + path);
        }

        /// <summary>
        /// Gets the drag data.
        /// </summary>
        /// <param name="items">The items.</param>
        /// <returns>The data.</returns>
        public static DragDataText GetDragData(IEnumerable<ContentItem> items)
        {
            if (items == null)
                throw new ArgumentNullException();

            string text = DragPrefix;
            foreach (var item in items)
                text += item.Path + '\n';
            return new DragDataText(text);
        }
    }
}
