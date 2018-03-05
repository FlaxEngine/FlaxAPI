////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2018 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using FlaxEditor.Content;
using FlaxEngine.GUI;

namespace FlaxEditor.GUI.Drag
{
    /// <summary>
    /// Helper class for handling <see cref="ScriptItem"/> drag and drop.
    /// </summary>
    /// <seealso cref="ScriptItem" />
    public sealed class DragScriptItems : DragHelper<ScriptItem>
    {
        /// <summary>
        /// The default prefix for drag data used for <see cref="ContentItem"/>.
        /// </summary>
        public const string DragPrefix = DragItems.DragPrefix;

        /// <inheritdoc />
        protected override void GetherObjects(DragDataText data, Func<ScriptItem, bool> validateFunc)
        {
            var items = ParseData(data);
            for (int i = 0; i < items.Length; i++)
            {
                if (validateFunc(items[i]))
                    Objects.Add(items[i]);
            }
        }

        /// <summary>
        /// Tries to parse the drag data to extract <see cref="ScriptItem"/> collection.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns>Gathered objects or empty array if cannot get any valid.</returns>
        public static ScriptItem[] ParseData(DragDataText data)
        {
            if (data.Text.StartsWith(DragPrefix))
            {
                // Remove prefix and parse splited names
                var paths = data.Text.Remove(0, DragPrefix.Length).Split('\n');
                var results = new List<ScriptItem>(paths.Length);
                for (int i = 0; i < paths.Length; i++)
                {
                    // Find element
                    var obj = Editor.Instance.ContentDatabase.FindScript(paths[i]);

                    // Check it
                    if (obj != null)
                        results.Add(obj);
                }

                return results.ToArray();
            }

            return new ScriptItem[0];
        }

        /// <summary>
        /// Gets the drag data.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>The data.</returns>
        public static DragDataText GetDragData(ScriptItem item)
        {
            return DragItems.GetDragData(item);
        }

        /// <summary>
        /// Gets the drag data.
        /// </summary>
        /// <param name="items">The items.</param>
        /// <returns>The data.</returns>
        public static DragDataText GetDragData(IEnumerable<ScriptItem> items)
        {
            return DragItems.GetDragData(items);
        }
    }
}
