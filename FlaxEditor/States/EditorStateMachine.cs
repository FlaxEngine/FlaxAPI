// Flax Engine scripting API

using System.Collections.Generic;
using FlaxEngine.Utilities;

namespace FlaxEditor.States
{
    /// <summary>
    /// Flax Editor states machine.
    /// </summary>
    /// <seealso cref="FlaxEngine.Utilities.StateMachine" />
    public sealed class EditorStateMachine : StateMachine
    {
        private readonly Queue<EditorState> _pendingStates = new Queue<EditorState>();

        /// <summary>
        /// Gets the current state.
        /// </summary>
        /// <value>
        /// The current state.
        /// </value>
        public new EditorState CurrentState => currentState as EditorState;

        /// <summary>
        /// Checks if editor is in playing mode
        /// </summary>
        /// <returns>True if editor is in play mode, otherwise false</returns>
        //bool IsPlayMode => CurrentState == PlayingState;

        /// <summary>
        /// Checks if editor is in editing mode
        /// </summary>
        /// <returns>True if editor is in edit mode, otherwise false</returns>
        //bool IsEditMode => CurrentState == EditingSceneState;

        /// <summary>
        /// Editor loading state.
        /// </summary>
        public LoadingState LoadingState = new LoadingState();

        internal EditorStateMachine()
        {
            // Register all in-build states
            AddState(LoadingState);

            // Set initial state
            GoToState(LoadingState);
        }

        public void Update()
        {
            //Debug.Assert();// TODO: test main thread

            // Changing states
            while (_pendingStates.Count > 0)
            {
                GoToState(_pendingStates.Dequeue());
            }

            // State update
            CurrentState?.Update();
        }

        /// <inheritdoc />
        public override void GoToState(State state)
        {
            //if (IsInMainThread())
            {
                // Change state
                base.GoToState(state);
            }
            /*else
            {
                LOG_EDITOR(Info, 131, ToString(stateType));

                // Enqueue state change
                _pendingStates.Enqueue(state);
            }*/
        }

        /// <inheritdoc />
        protected override void SwitchState(State nextState)
        {
            var prev = (EditorState)currentState;
            var next = (EditorState)nextState;

            // Send info
            //LOG_EDITOR(Info, 117, ToString(next->GetType()));

            // Base
            base.SwitchState(nextState);

            // Fire events
            /*OnStateChanged(prev ? prev->GetType() : EditorStates::Loading, next ? next->GetType() : EditorStates::Loading);
            if (_currentState && CurrentState()->IsEditorReady())
            {
                // TODO: let CEditor bind to OnStateCangedEvent instead of that nesting!

                // Update UI
                CUIModule->UpdateToolstrip();
                CUIModule->UpdateStatusStripColor();
            }*/
        }
    }
}
