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
    public sealed class DragItems : DragHelper<ContentItem, DragEventArgs>
    {
        /// <summary>
        /// The default prefix for drag data used for <see cref="ContentItem"/>.
        /// </summary>
        public const string DragPrefix = "ASSET!?";

        /// <summary>
        /// Creates a new DragHelper
        /// </summary>
        /// <param name="validateFunction">The validation function</param>
        public DragItems(Func<ContentItem, bool> validateFunction) : base(validateFunction)
        {
        }

        public DragData ToDragData(string path) => GetDragData(path);

        /// <inheritdoc/>
        public override DragData ToDragData(ContentItem item) => GetDragData(item);

        /// <inheritdoc/>
        public override DragData ToDragData(IEnumerable<ContentItem> items) => GetDragData(items);

        public static DragData GetDragData(string path)
        {
            if (path == null)
                throw new ArgumentNullException();

            return new DragDataText(DragPrefix + path);
        }

        public static DragDataText GetDragData(ContentItem item)
        {
            if (item == null)
                throw new ArgumentNullException();

            return new DragDataText(DragPrefix + item.Path);
        }

        public static DragData GetDragData(IEnumerable<ContentItem> items)
        {
            if (items == null)
                throw new ArgumentNullException();

            string text = DragPrefix;
            foreach (var item in items)
                text += item.Path + '\n';
            return new DragDataText(text);
        }

        /// <inheritdoc/>
        public override IEnumerable<ContentItem> FromDragData(DragData data)
        {
            if (data is DragDataText dataText)
            {
                if (dataText.Text.StartsWith(DragPrefix))
                {
                    // Remove prefix and parse splitted names
                    var paths = dataText.Text.Remove(0, DragPrefix.Length).Split('\n');
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
            }
            return new ContentItem[0];
        }

        /// <inheritdoc/>
        public override void DragDrop(DragEventArgs dragEventArgs, IEnumerable<ContentItem> item)
        {

        }
    }
}
