// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System;
using FlaxEditor.SceneGraph;

namespace FlaxEditor
{
    /// <summary>
    /// Objects selection change action.
    /// </summary>
    /// <seealso cref="IUndoAction" />
    public class SelectionChangeAction : UndoActionBase<SelectionChangeAction.DataStorage>
    {
        /// <summary>
        /// The undo data.
        /// </summary>
        [Serializable]
        public struct DataStorage
        {
            /// <summary>
            /// The 'before' selection.
            /// </summary>
            public SceneGraphNode[] Before;

            /// <summary>
            /// The 'after' selection.
            /// </summary>
            public SceneGraphNode[] After;
        }

        internal SelectionChangeAction(SceneGraphNode[] before, SceneGraphNode[] after)
        {
            Data = new DataStorage
            {
                Before = before,
                After = after,
            };
        }

        /// <inheritdoc />
        public override string ActionString => "Selection change";

        /// <inheritdoc />
        public override void Do()
        {
            var data = Data;
            Editor.Instance.SceneEditing.OnSelectionUndo(data.After);
        }

        /// <inheritdoc />
        public override void Undo()
        {
            var data = Data;
            Editor.Instance.SceneEditing.OnSelectionUndo(data.Before);
        }
    }
}
