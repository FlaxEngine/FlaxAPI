// Flax Engine scripting API

using System;

namespace FlaxEngine.GUI.Docking
{
    /// <summary>
    /// Floating Window Dock Panel control.
    /// </summary>
    /// <seealso cref="FlaxEngine.GUI.Docking.DockPanel" />
    public class FloatWindowDockPanel : DockPanel
    {
        protected readonly MasterDockPanel _masterPanel;
        protected readonly Window _window;

        /// <summary>
        /// Gets the master panel.
        /// </summary>
        /// <value>
        /// The master panel.
        /// </value>
        public MasterDockPanel MasterPanel => _masterPanel;

        /// <summary>
        /// Gets the window.
        /// </summary>
        /// <value>
        /// The window.
        /// </value>
        public Window Window => _window;

        /// <summary>
        /// Initializes a new instance of the <see cref="FloatWindowDockPanel"/> class.
        /// </summary>
        /// <param name="masterPanel">The master panel.</param>
        /// <param name="window">The window.</param>
        FloatWindowDockPanel(MasterDockPanel masterPanel, Window window)
            : base(null)
        {
            _masterPanel = masterPanel;
            _window = window;

            // Link
            _masterPanel.FloatingPanels.Add(this);
            throw new NotImplementedException();
            //Control::SetParent(window->GUI);
            //window->OnClosing.Bind < CFloatWindowDockPanel, &CFloatWindowDockPanel::onClosing > (this);
            //window->OnLButtonHit.Bind < CFloatWindowDockPanel, &CFloatWindowDockPanel::onLButtonHit > (this);
        }
    }
}
