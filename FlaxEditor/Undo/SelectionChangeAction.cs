////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using FlaxEditor.History;
using FlaxEditor.SceneGraph;

namespace FlaxEditor
{
    /// <summary>
    /// Objects selection change action.
    /// </summary>
    /// <seealso cref="IUndoAction" />
    public class SelectionChangeAction : IUndoAction
    {
        private readonly ISceneTreeNode[] _before;
        private readonly ISceneTreeNode[] _after;

        internal SelectionChangeAction(ISceneTreeNode[] before, ISceneTreeNode[] after)
        {
            _before = before;
            _after = after;
        }
        
        /// <inheritdoc />
        public string ActionString => "Selection change";

        /// <inheritdoc />
        public void Do()
        {
            Editor.Instance.SceneEditing.OnSelectionUndo(_after);
        }

        /// <inheritdoc />
        public void Undo()
        {
            Editor.Instance.SceneEditing.OnSelectionUndo(_before);
        }
    }
}
