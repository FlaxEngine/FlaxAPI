// Flax Engine scripting API

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlaxEngine.GUI.Docking
{
    public class DockWindow : Panel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DockWindow"/> class.
        /// </summary>
        /// <param name="masterPanel">The master docking panel.</param>
        /// <param name="hideOnClose">True if hide window on closing, otherwise it will be destroyed.</param>
        /// <param name="scrollBars">The scroll bars.</param>
        public DockWindow(MasterDockPanel masterPanel, bool hideOnClose, ScrollBars scrollBars)
            : base(scrollBars)
        {
            todo: finish his
        }
    }
}
