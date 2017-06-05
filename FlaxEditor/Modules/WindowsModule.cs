// Flax Engine scripting API

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlaxEditor.Windows;

namespace FlaxEditor.Modules
{
    /// <summary>
    /// Manages Editor windows and popups.
    /// </summary>
    /// <seealso cref="FlaxEditor.Modules.EditorModule" />
    public sealed class WindowsModule : EditorModule
    {
        /// <summary>
        /// List with all created editor windows.
        /// </summary>
        public readonly List<EditorWindow> Windows = new List<EditorWindow>(32);

        internal WindowsModule(Editor editor)
            : base(editor)
        {
            // Init windows module first
            InitOrder = -100;
        }
    }
}
