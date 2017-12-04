// Flax Engine scripting API

using System;
using FlaxEditor.Utilities;
using FlaxEngine;
using FlaxEngine.Utilities;

namespace FlaxEditor.States
{
    /// <summary>
    /// In this state editor is simulating game.
    /// </summary>
    /// <seealso cref="FlaxEditor.States.EditorState" />
    public sealed class PlayingState : EditorState
    {
        private readonly DuplicateScenes _duplicateScenes = new DuplicateScenes();

        /// <summary>
        /// Gets a value indicating whether any scene was dirty before entering the play mode.
        /// </summary>
        public bool WasDirty => _duplicateScenes.WasDirty;

        /// <inheritdoc />
        public override bool CanEditScene => true;

        /// <inheritdoc />
        public override bool CanEnterPlayMode => true;

        /// <summary>
        /// Gets or sets a value indicating whether game logic is paused.
        /// </summary>
        public bool IsPaused
        {
            get => !SceneManager.IsGameLogicRunning;
            set
            {
                if (!IsActive)
                    throw new InvalidOperationException();

                SceneManager.IsGameLogicRunning = !value;
            }
        }

        internal PlayingState(Editor editor)
            : base(editor)
        {
        }

        /// <inheritdoc />
        public override void OnEnter()
        {
            Input.ScanGamepads();

            // Remove references to the scene objects
            Editor.Scene.ClearRefsToSceneObjects();

            // Duplicate editor scene for simulation
            _duplicateScenes.GatherSceneData();

            // Fire events
            Editor.OnPlayBegin();
            IsPaused = false;
        }

        /// <inheritdoc />
        public override void OnExit(State nextState)
        {
            IsPaused = true;

            // Remove references to the scene objects
            Editor.Scene.ClearRefsToSceneObjects();

            // Restore editor scene
            _duplicateScenes.RestoreSceneData();

            // Restore game settings and state for editor environment
            Time.TimeScale = 1.0f;
            Physics.AutoSimulation = true;
            Screen.CursorVisible = true;
            var win = Editor.Windows.GameWin?.ParentWindow;
            if (win != null)
                win.Cursor = CursorType.Default;

            // Fire events
            Editor.OnPlayEnd();
            IsPaused = true;
        }
    }
}
