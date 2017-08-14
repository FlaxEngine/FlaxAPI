////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

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
            public SceneTreeNode[] Before;

            /// <summary>
            /// The 'after' selection.
            /// </summary>
            public SceneTreeNode[] After;
        }

        internal SelectionChangeAction(SceneTreeNode[] before, SceneTreeNode[] after)
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
