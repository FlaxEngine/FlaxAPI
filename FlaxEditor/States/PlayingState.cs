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

        /// <inheritdoc />
        public override bool CanEditScene => true;

        /// <inheritdoc />
        public override bool CanEnterPlayMode => true;

        /// <summary>
        /// Gets or sets a value indicating whether game logic is paused.
        /// </summary>
        /// <value>
        ///   <c>true</c> if game logic is paused; otherwise, <c>false</c>.
        /// </value>
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
            // Remove references to the scene objects
            Editor.Scene.ClearRefsToSceneObjects();

            // Restore editor scene
            _duplicateScenes.RestoreSceneData();

            // Fire events
            Editor.OnPlayEnd();
        }
    }
}
