// Flax Engine scripting API

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlaxEngine.GUI.Docking;

namespace FlaxEditor.Modules
{
    /// <summary>
    /// Manages Editor UI. Especially main window UI.
    /// </summary>
    /// <seealso cref="FlaxEditor.Modules.EditorModule" />
    public sealed class UIModule : EditorModule
    {
        /// <summary>
        /// The master dock panel for all Editor windows.
        /// </summary>
        public readonly MasterDockPanel MasterPanel = new MasterDockPanel();

        internal UIModule(Editor editor)
            : base(editor)
        {
        }
    }
}
