// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

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
        public bool IsPlayMode => CurrentState == PlayingState;

        /// <summary>
        /// Checks if editor is in editing mode
        /// </summary>
        /// <returns>True if editor is in edit mode, otherwise false</returns>
        public bool IsEditMode => CurrentState == EditingSceneState;

        /// <summary>
        /// Editor loading state.
        /// </summary>
        public readonly LoadingState LoadingState;

        /// <summary>
        /// Editor closing state.
        /// </summary>
        public readonly ClosingState ClosingState;

        /// <summary>
        /// Editor editing scene state.
        /// </summary>
        public readonly EditingSceneState EditingSceneState;

        /// <summary>
        /// Editor changing scenes state.
        /// </summary>
        public readonly ChangingScenesState ChangingScenesState;

        /// <summary>
        /// Editor playing state.
        /// </summary>
        public readonly PlayingState PlayingState;

        /// <summary>
        /// Editor reloading scripts state.
        /// </summary>
        public readonly ReloadingScriptsState ReloadingScriptsState;

        /// <summary>
        /// Editor building lighting state.
        /// </summary>
        public readonly BuildingLightingState BuildingLightingState;

        internal EditorStateMachine(Editor editor)
        {
            // Register all in-build states
            AddState(LoadingState = new LoadingState(editor));
            AddState(ClosingState = new ClosingState(editor));
            AddState(EditingSceneState = new EditingSceneState(editor));
            AddState(ChangingScenesState = new ChangingScenesState(editor));
            AddState(PlayingState = new PlayingState(editor));
            AddState(ReloadingScriptsState = new ReloadingScriptsState(editor));
            AddState(BuildingLightingState = new BuildingLightingState(editor));

            // Set initial state
            GoToState(LoadingState);
        }

        internal void Update()
        {
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
            Editor.Log($"Changing editor state from {currentState} to {nextState}");

            base.SwitchState(nextState);
        }
    }
}
