// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System;
using System.Collections.Generic;
using FlaxEditor.Content;
using FlaxEngine.GUI;

namespace FlaxEditor.GUI.Drag
{
    public sealed class DragScriptItems : DragScriptItems<DragEventArgs>
    {
        public DragScriptItems(Func<ScriptItem, bool> validateFunction) : base(validateFunction)
        {
        }
    }

    /// <summary>
    /// Helper class for handling <see cref="ScriptItem"/> drag and drop.
    /// </summary>
    /// <seealso cref="ScriptItem" />
    public class DragScriptItems<U> : DragHelper<ScriptItem, U> where U : DragEventArgs
    {
        /// <summary>
        /// The default prefix for drag data used for <see cref="ContentItem"/>.
        /// </summary>
        public const string DragPrefix = DragItems<DragEventArgs>.DragPrefix;

        /// <summary>
        /// Creates a new DragHelper
        /// </summary>
        /// <param name="validateFunction">The validation function</param>
        public DragScriptItems(Func<ScriptItem, bool> validateFunction) : base(validateFunction)
        {
        }

        /// <inheritdoc/>
        public override DragData ToDragData(ScriptItem item) => GetDragData(item);

        /// <inheritdoc/>
        public override DragData ToDragData(IEnumerable<ScriptItem> items) => GetDragData(items);

        public static DragData GetDragData(ScriptItem item)
        {
            return DragItems<DragEventArgs>.GetDragData(item);
        }

        public static DragData GetDragData(IEnumerable<ScriptItem> items)
        {
            return DragItems<DragEventArgs>.GetDragData(items);
        }

        public override IEnumerable<ScriptItem> FromDragData(DragData data)
        {
            if (data is DragDataText dataText)
            {
                if (dataText.Text.StartsWith(DragPrefix))
                {
                    // Remove prefix and parse splitted names
                    var paths = dataText.Text.Remove(0, DragPrefix.Length).Split('\n');
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
            }
            return new ScriptItem[0];
        }

        public override void DragDrop(U dragEventArgs, IEnumerable<ScriptItem> item)
        {

        }
    }
}
