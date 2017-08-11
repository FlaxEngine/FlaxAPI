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
    public class SelectionChangeAction : IUndoAction
    {
        // Note: we use SceneTreeNode ids because they may be removed so we have to find them on action rollback
        private readonly Guid[] _before;
        private readonly Guid[] _after;

        internal SelectionChangeAction(SceneTreeNode[] before, SceneTreeNode[] after)
        {
            _before = new Guid[before.Length];
            for (int i = 0; i < before.Length; i++)
                _before[i] = before[i].ID;

            _after = new Guid[after.Length];
            for (int i = 0; i < after.Length; i++)
                _after[i] = after[i].ID;
        }

        /// <inheritdoc />
        public string ActionString => "Selection change";

        /// <inheritdoc />
        public void Do()
        {
            //Editor.Instance.SceneEditing.OnSelectionUndo(_after);
        }

        /// <inheritdoc />
        public void Undo()
        {
            //Editor.Instance.SceneEditing.OnSelectionUndo(_before);
        }
    }
}
