// Flax Engine scripting API

using FlaxEditor.Scripting;
using FlaxEditor.States;
using FlaxEditor.Windows;
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
        private bool _stepFrame;
        private bool _updateOrFixedUpdateWasCalled;
        private EditorWindow _enterPlayFocusedWindow;

        internal SimulationModule(Editor editor)
            : base(editor)
        {
			FlaxEngine.Scripting.FixedUpdate += OnFixedUpdate;
        }

	    /// <summary>
        /// Checks if play mode should start only with single frame update and then enter step mode.
        /// </summary>
        public bool ShouldPlayModeStartWithStep => Editor.UI.IsPauseButtonChecked;

        /// <summary>
        /// Returns true if play mode has been requested.
        /// </summary>
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
        /// Requests the playing start or stop in editor.
        /// </summary>
        public void RequestPlayOrStopPlay()
        {
            // Check if is in play mode
            if (Editor.StateMachine.IsPlayMode)
                RequestStopPlay();
            else
                RequestStartPlay();
        }

		/// <summary>
		/// Requests the playing mode resume or pause if already running.
		/// </summary>
		public void RequestResumeOrPause()
	    {
		    // Check if is in pause state
		    if (Editor.StateMachine.PlayingState.IsPaused)
			    Editor.Simulation.RequestResumePlay();

		    else
			    Editor.Simulation.RequestPausePlay();
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

                // Set flag
                _stepFrame = true;
	            _updateOrFixedUpdateWasCalled = false;
				Editor.StateMachine.PlayingState.IsPaused = false;

                // Update
                Editor.UI.UpdateToolstrip();
            }
        }

        /// <inheritdoc />
        public override void OnPlayBegin()
        {
            Editor.Windows.FlashMainWindow();

            // Pick focused window to restore it
            var gameWin = Editor.Windows.GameWin;
            var editWin = Editor.Windows.EditWin;
            if (editWin != null && editWin.IsSelected)
                _enterPlayFocusedWindow = editWin;
            else if (gameWin != null && gameWin.IsSelected)
                _enterPlayFocusedWindow = gameWin;

            // Show Game widow if hidden
            if (gameWin != null)
            {
                if (!gameWin.IsDocked)
                {
                    gameWin.ShowFloating();
                }
                else if (!gameWin.IsSelected)
                {
                    gameWin.SelectTab(false);
                }
            }

            Editor.Log("[PlayMode] Enter");
        }

        /// <inheritdoc />
        public override void OnPlayEnd()
        {
            // Restore focused window before play mode
            if (_enterPlayFocusedWindow != null)
            {
                _enterPlayFocusedWindow.FocusOrShow();
                _enterPlayFocusedWindow = null;
            }

            Editor.UI.UncheckPauseButton();

            Editor.Log("[PlayMode] Exit");
        }

        /// <inheritdoc />
        public override void OnUpdate()
        {
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
                // Check if step one frame
                else if (_stepFrame)
	            {
		            if (_updateOrFixedUpdateWasCalled)
		            {
			            // Clear flag and pause
			            _stepFrame = false;
			            Editor.StateMachine.PlayingState.IsPaused = true;
			            Editor.UI.UpdateToolstrip();
		            }
		            else
		            {
						// Fixed update may not be called but don't allow to call pdate for more than 2 times during single step
			            _updateOrFixedUpdateWasCalled = true;
		            }
	            }
            }
            else
            {
                // Clear flags
                _isPlayModeRequested = false;
                _isPlayModeStopRequested = false;
                _stepFrame = false;
	            _updateOrFixedUpdateWasCalled = false;
            }
        }

	    private void OnFixedUpdate()
	    {
			// Rise the flag so play mode step end will be called after physics update (user see objects movement)
		    _updateOrFixedUpdateWasCalled = true;
	    }
	}
}
