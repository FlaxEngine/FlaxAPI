// Copyright (c) 2012-2019 Wojciech Figat. All rights reserved.

using FlaxEngine.Utilities;

namespace FlaxEditor.States
{
    /// <summary>
    /// Base class for all Editor states.
    /// </summary>
    /// <seealso cref="FlaxEngine.Utilities.State" />
    public abstract class EditorState : State
    {
        /// <summary>
        /// Gets the editor state machine.
        /// </summary>
        public new EditorStateMachine StateMachine => owner as EditorStateMachine;

        /// <summary>
        /// Gets the editor object.
        /// </summary>
        public readonly Editor Editor;

        /// <summary>
        /// Checks if can edit assets in this state.
        /// </summary>
        public virtual bool CanEditContent => true;

        /// <summary>
        /// Checks if can edit scene in this state
        /// </summary>
        public virtual bool CanEditScene => false;

        /// <summary>
        /// Checks if can use toolbox in this state.
        /// </summary>
        public virtual bool CanUseToolbox => false;

        /// <summary>
        /// Checks if can use undo/redo in this state.
        /// </summary>
        public virtual bool CanUseUndoRedo => false;

        /// <summary>
        /// Checks if can change scene in this state.
        /// </summary>
        public virtual bool CanChangeScene => false;

        /// <summary>
        /// Checks if can enter play mode in this state.
        /// </summary>
        public virtual bool CanEnterPlayMode => false;

        /// <summary>
        /// Checks if can enter recompile scripts in this state.
        /// </summary>
        public virtual bool CanReloadScripts => false;

        /// <summary>
        /// Checks if static is valid for Editor UI calls and other stuff.
        /// </summary>
        public virtual bool IsEditorReady => true;

        /// <summary>
        /// Gets the state status message for the UI. Returns null if use the default value.
        /// </summary>
        public virtual string Status => null;

        /// <summary>
        /// Initializes a new instance of the <see cref="EditorState"/> class.
        /// </summary>
        /// <param name="editor">The editor.</param>
        protected EditorState(Editor editor)
        {
            Editor = editor;
        }

        /// <summary>
        /// Update state. Called every Engine tick.
        /// </summary>
        public virtual void Update()
        {
        }

        /// <inheritdoc />
        public override bool CanExit(State nextState)
        {
            // TODO: add blocking changing states based on this state properties
            return base.CanExit(nextState);
        }
    }
}
