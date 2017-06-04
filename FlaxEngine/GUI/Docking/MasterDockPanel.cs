// Flax Engine scripting API

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
        {
        }
    }
}
