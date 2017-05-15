// Flax Engine scripting API

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
        /// Check if can edit assets in this state
        /// </summary>
        /// <returns>True if can edit content, otherwise false</returns>
        public virtual bool CanEditContent => true;

        /// <summary>
        /// Check if can edit scene in this state
        /// </summary>
        /// <returns>True if can edit scene, otherwise false</returns>
        public virtual bool CanEditScene => false;

        /// <summary>
        /// Check if can use toolbox in this state
        /// </summary>
        /// <returns>True if can use toolbox, otherwise false</returns>
        public virtual bool CanUseToolbox => false;

        /// <summary>
        /// Check if can use undo/redo in this state
        /// </summary>
        /// <returns>True if can use undo/redo, otherwise false</returns>
        public virtual bool CanUseUndoRedo => false;

        /// <summary>
        /// Check if can change scene in this state
        /// </summary>
        /// <returns>True if can change scene, otherwise false</returns>
        public virtual bool CanChangeScene => false;

        /// <summary>
        /// Check if can enter play mode in this state
        /// </summary>
        /// <returns>True if can enter play mode, otherwise false</returns>
        public virtual bool CanEnterPlayMode => false;

        /// <summary>
        /// Check if can enter recompile scripts in this state
        /// </summary>
        /// <returns>True if can recompile scripts, otherwise false</returns>
        public virtual bool CanReloadScripts => false;

        /// <summary>
        /// Check if static is valid for Editor UI calls and other stuff
        /// </summary>
        /// <returns>True if state is valid for Editor UI calls and other stuff, otherwise false</returns>
        public virtual bool IsEditorReady => true;

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
