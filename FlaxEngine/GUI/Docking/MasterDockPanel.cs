// Flax Engine scripting API

using System;
using System.Collections.Generic;

namespace FlaxEngine.GUI.Docking
{
    /// <summary>
    /// Master Dock Panel control used as a root control for dockable windows workspace.
    /// </summary>
    /// <seealso cref="FlaxEngine.GUI.Docking.DockPanel" />
    public class MasterDockPanel : DockPanel
    {
        /// <summary>
        /// Array with all created dock windows for that master panel.
        /// </summary>
        public readonly List<DockWindow> Windows = new List<DockWindow>(32);

        /// <summary>
        /// Array with all floating windows for that master panel.
        /// </summary>
        public readonly List<FloatWindowDockPanel> FloatingPanels = new List<FloatWindowDockPanel>(4);

        /// <summary>
        /// Gets the visible windows count.
        /// </summary>
        /// <value>
        /// The visible windows count.
        /// </value>
        public int VisibleWindowsCount
        {
            get
            {
                int result = 0;
                for (int i = 0; i < Windows.Count; i++)
                {
                    if (Windows[i].Visible)
                        result++;
                }
                return result;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MasterDockPanel"/> class.
        /// </summary>
        public MasterDockPanel()
            : base(null)
        {
        }

        /// <summary>
        /// Saves whole docking layout to the file.
        /// </summary>
        /// <param name="path">Destination file path</param>
        /// <param name="mainWindow">Main window</param>
        /// <returns>True if cannot save data, otherwise false</returns>
        public bool SaveLayout(string path, Window mainWindow)
        {
            throw new NotImplementedException("Finish saving layout to xml file");
        }

        /// <summary>
        /// Loads whole docking layout from the file.
        /// </summary>
        /// <param name="path">Source file path</param>
        /// <param name="setupWindow">Function that creates windows. String argument contains serialized window typename.</param>
        /// <param name="mainWindow">Main window</param>
        /// <returns>True if cannot load data, otherwise false</returns>
        public bool LoadLayout(string path, Func<MasterDockPanel, string, DockWindow> setupWindow, Window mainWindow)
        {
            throw new NotImplementedException("Finish loading layout from xml file");
        }

        /// <summary>
        /// Resets windows layout.
        /// </summary>
        public void ResetLayout()
        {
            // Close all windows
            for (int i = Windows.Count - 1; i >= 0 && Windows.Count > 0; i--)
                Windows[i].Close();

            // Ensure that has no docked windows
            while (_childPanels.Count > 0)
                _childPanels[0].Dispose();

            // Delete reaming controls (except tabs proxy)
            if (_tabsProxy != null)
                _tabsProxy.Parent = null;
            DisposeChildren();
            if (_tabsProxy != null)
                _tabsProxy.Parent = this;
        }

        /// <summary>
        /// Performs hit test over dock panel.
        /// </summary>
        /// <param name="position">Screen space position to test.</param>
        /// <param name="excluded">Floating window to ommit during searching (and all docked to that one).</param>
        /// <returns>Dock panel that has been hitted or null if nothing found.</returns>
        public DockPanel HitTest(Vector2 position, FloatWindowDockPanel excluded)
        {
            // Check all floating windows
            // TODO: gather windows order and take it into account when performing test
            for (int i = 0; i < FloatingPanels.Count; i++)
            {
                var win = FloatingPanels[i];
                if (win.Visible && win != excluded)
                {
                    var result = win.HitTest(ref position);
                    if (result != null)
                        return result;
                }
            }

            // Base
            return base.HitTest(ref position);
        }

        internal void linkWindow(DockWindow window)
        {
            // Add to the windows list
            Windows.Add(window);
        }

        internal void unlinkWindow(DockWindow window)
        {
            // Call event to the window
            window.OnUnlinkInternal();

            // Prevent this action during disposing (we don't want to modify Windows list)
            if (IsDisposing)
                return;

            // Remove from the windows list
            Windows.Remove(window);
        }
        
        /// <inheritdoc />
        public override bool IsMaster => true;

        /// <inheritdoc />
        public override void OnDestroy()
        {
            base.OnDestroy();

            Windows.Clear();
            FloatingPanels.Clear();
        }

        /// <inheritdoc />
        public override DockState TryGetDockState(ref float splitterValue)
        {
            return DockState.Unknown;
        }
    }
}
