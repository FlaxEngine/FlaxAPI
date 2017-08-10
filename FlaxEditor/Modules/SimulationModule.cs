// Flax Engine scripting API

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        }

        /// <summary>
        /// Requests stop playing in editor.
        /// </summary>
        public void RequestStopPlay()
        {

        }

        /// <summary>
        /// Requests pause in playing.
        /// </summary>
        public void RequestPausePlay()
        {

        }

        /// <summary>
        /// Request resume in playing.
        /// </summary>
        public void RequestResumePlay()
        {

        }

        /// <summary>
        /// Requests playing single frame in advance.
        /// </summary>
        public void RequestPlayOneFrame()
        {

        }
    }
}
