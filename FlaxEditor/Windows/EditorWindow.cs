// Flax Engine scripting API

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlaxEngine.GUI;
using FlaxEngine.GUI.Docking;

namespace FlaxEditor.Windows
{
    /// <summary>
    ///  Base class for all windows in Editor.
    /// </summary>
    /// <seealso cref="FlaxEngine.GUI.Docking.DockWindow" />
    public class EditorWindow : DockWindow
    {
        /// <summary>
        /// Gets the editor object.
        /// </summary>
        public readonly Editor Editor;

        /// <summary>
        /// Initializes a new instance of the <see cref="EditorWindow"/> class.
        /// </summary>
        /// <param name="editor">The editor.</param>
        /// <param name="hideOnClose">True if hide window on closing, otherwise it will be destroyed.</param>
        /// <param name="scrollBars">The scroll bars.</param>
        protected EditorWindow(Editor editor, bool hideOnClose, ScrollBars scrollBars)
            : base(editor.UI.MasterPanel, hideOnClose, scrollBars)
        {
            Editor = editor;

            // Register
            Editor.Windows.Windows.Add(this);
        }

        /// <inheritdoc />
        public override void OnDestroy()
        {
            // Unregister
            Editor.Windows.Windows.Remove(this);

            base.OnDestroy();
        }
    }
}
