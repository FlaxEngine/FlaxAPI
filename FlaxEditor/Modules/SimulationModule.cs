// Flax Engine scripting API

using System;
using FlaxEditor.Scripting;
using FlaxEditor.States;
using FlaxEngine;

namespace FlaxEditor.Modules
{
    /// <summary>
    /// Manages play in-editor feature (game simulation).
    /// </summary>
    /// <seealso cref="FlaxEditor.Modules.EditorModule" />
    public sealed class SimulationModule : EditorModule
    {
        private bool _isPlayModeRequested;
        private bool _isPlayModeStopRequested;
        private bool _wasEditorWinFocusedOnPlay;

        internal SimulationModule(Editor editor)
            : base(editor)
        {
        }

        /// <summary>
        /// Checks if play mode should start only with single frame update and then enter step mode.
        /// </summary>
        /// <returns>True if start with only 1 frame update and the nenter step mode, otherwise false</returns>
        public bool ShouldPlayModeStartWithStep => Editor.UI.IsPauseButtonChecked;

        /// <summary>
        /// Returns true if play mode has been requested.
        /// </summary>
        /// <returns>True if play mode has been requested, otherwise false</returns>
        public bool IsPlayModeRequested => _isPlayModeRequested;

        /// <summary>
        /// Requests start playing in editor.
        /// </summary>
        public void RequestStartPlay()
        {
            // Check if is in edit mode
            if (Editor.StateMachine.IsEditMode)
            {
                Editor.Log("[PlayMode] Start");

                // Request to be compiled
                ScriptsBuilder.CheckForCompile();

                // Set flag
                _isPlayModeRequested = true;

                // Update
                Editor.UI.UpdateToolstrip();
            }
        }

        /// <summary>
        /// Requests stop playing in editor.
        /// </summary>
        public void RequestStopPlay()
        {
            // Check if is in play mode
            if (Editor.StateMachine.IsPlayMode)
            {
                Editor.Log("[PlayMode] Stop");

                // Set flag
                _isPlayModeStopRequested = true;

                // Update
                Editor.UI.UpdateToolstrip();
            }
        }

        /// <summary>
        /// Requests pause in playing.
        /// </summary>
        public void RequestPausePlay()
        {
            // Check if is in play mode and isn't paused
            if (Editor.StateMachine.IsPlayMode && !Editor.StateMachine.PlayingState.IsPaused)
            {
                Editor.Log("[PlayMode] Pause");

                // Pause
                Editor.StateMachine.PlayingState.IsPaused = true;

                // Update
                Editor.UI.UpdateToolstrip();
            }
        }

        /// <summary>
        /// Request resume in playing.
        /// </summary>
        public void RequestResumePlay()
        {
            // Check if is in play mode and is paused
            if (Editor.StateMachine.IsPlayMode && Editor.StateMachine.PlayingState.IsPaused)
            {
                Editor.Log("[PlayMode] Resume");

                // Resume
                Editor.StateMachine.PlayingState.IsPaused = false;

                // Update
                Editor.UI.UpdateToolstrip();
            }
        }

        /// <summary>
        /// Requests playing single frame in advance.
        /// </summary>
        public void RequestPlayOneFrame()
        {
            // Check if is in play mode and is paused
            if (Editor.StateMachine.IsPlayMode && Editor.StateMachine.PlayingState.IsPaused)
            {
                Editor.Log("[PlayMode] Step one frame");

                // TODO: step one frame using playing state internal logic
                throw new NotImplementedException("Step one frame in playmode");

                // Update
                Editor.UI.UpdateToolstrip();
            }
        }

        internal void OnPlayModeEnter()
        {
            Editor.Windows.FlashMainWindow();

            var gameWin = Editor.Windows.GameWin;
            if (gameWin != null)
            {
                _wasEditorWinFocusedOnPlay = gameWin.ContainsFocus;
                gameWin.FocusOrShow();
            }
            else
            {
                _wasEditorWinFocusedOnPlay = false;
            }

            Editor.Log("[PlayMode] Enter");
        }

        internal void OnPlayModeExit()
        {
            var gameWin = Editor.Windows.GameWin;
            if (gameWin != null && _wasEditorWinFocusedOnPlay)
            {
                gameWin.FocusOrShow();
            }

            Editor.UI.UncheckPauseButton();

            Editor.Log("[PlayMode] Exit");
        }

        /// <inheritdoc />
        public override void OnUpdate()
        {
            // Input
            if (Input.GetKeyDown(KeyCode.F5))
            {
                if (Editor.StateMachine.IsPlayMode)
                {
                    // Stop
                    RequestStopPlay();
                }
                else
                {
                    // Play
                    RequestStartPlay();
                }
            }
            else if (Input.GetKeyDown(KeyCode.F11))
            {
                // Step
                RequestPlayOneFrame();
            }

            // Check if can enter playing in editor mode
            if (Editor.StateMachine.CurrentState.CanEnterPlayMode)
            {
                // Check if play mode has been requested
                if (_isPlayModeRequested)
                {
                    // Check if editor has been compiled and scripting reloaded (there is no pending reload action)
                    if (ScriptsBuilder.IsReady && !SceneManager.IsAnyAsyncActionPending)
                    {
                        // Clear flag
                        _isPlayModeRequested = false;

                        // Enter play mode
                        Editor.StateMachine.GoToState<PlayingState>();

                        // Check if move just by one frame
                        if (ShouldPlayModeStartWithStep)
                        {
                            RequestPausePlay();
                        }
                    }
                }
                // Check if play mode exit has been requested
                else if (_isPlayModeStopRequested)
                {
                    // Clear flag
                    _isPlayModeStopRequested = false;

                    // Exit play mode
                    Editor.StateMachine.GoToState<EditingSceneState>();
                }
            }
            else
            {
                // Clear flags
                _isPlayModeRequested = false;
                _isPlayModeStopRequested = false;
            }
        }
    }
}
