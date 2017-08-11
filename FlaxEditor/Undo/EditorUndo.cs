////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System;

namespace FlaxEditor
{
    /// <summary>
    /// Implementation of <see cref="Undo"/> customized for the <see cref="Editor"/>.
    /// </summary>
    /// <seealso cref="FlaxEditor.Undo" />
    public class EditorUndo : Undo
    {
        private readonly Editor _editor;

        internal EditorUndo(Editor editor)
        {
            _editor = editor;
        }

        /// <inheritdoc />
        public override bool Enabled
        {
            get => _editor.StateMachine.CurrentState.CanUseUndoRedo;
            set => throw new AccessViolationException("Cannot change enabled state of the editor main undo.");
        }
    }
}
