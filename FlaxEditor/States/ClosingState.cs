// Flax Engine scripting API

namespace FlaxEditor.States
{
    /// <summary>
    /// In this state editor is performing closing actions and will shutdown. This is last state and cannot leave it.
    /// </summary>
    /// <seealso cref="FlaxEditor.States.EditorState" />
    public sealed class ClosingState : EditorState
    {
        /// <inheritdoc />
        public override bool CanEditContent => false;

        /// <inheritdoc />
        public override bool IsEditorReady => false;

        /// <inheritdoc />
        public override bool CanEnter()
        {
            // Disable exit from Closing state
            return false;
        }
    }
}
