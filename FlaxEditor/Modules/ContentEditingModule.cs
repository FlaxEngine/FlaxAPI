// Flax Engine scripting API

using System;
using FlaxEditor.Content;
using FlaxEditor.Windows;
using FlaxEngine;
using FlaxEngine.GUI.Docking;

namespace FlaxEditor.Modules
{
    /// <summary>
    /// Opening/editing asset windows module.
    /// </summary>
    /// <seealso cref="FlaxEditor.Modules.EditorModule" />
    public sealed class ContentEditingModule : EditorModule
    {
        internal ContentEditingModule(Editor editor)
            : base(editor)
        {
        }

        /// <summary>
        /// Opens the specified item in dedicated editor window.
        /// </summary>
        /// <param name="item">The content item.</param>
        /// <param name="disableAutoShow">True if disable automatic window showing. Used during workspace layout loading to deserialize it faster.</param>
        /// <returns>Opened window or null if cannot open item.</returns>
        public EditorWindow Open(ContentItem item, bool disableAutoShow = false)
        {
            if (item == null)
                throw new ArgumentNullException();

            // Check if any window is already editing this item
            var window = Editor.Windows.FindEditor(item);
            if (window != null)
            {
                return window;
            }

            // Find proxy object
            var proxy = Editor.ContentDatabase.GetProxy(item);
            if (proxy == null)
            {
                // Error
                Debug.Log("Missing content proxy object for " + item);
                return null;
            }

            // Open
            window = proxy.Open(item);
            if (window != null && !disableAutoShow)
            {
                // Check if there is a floating window that has the same size
                Vector2 defaultSize = window.DefaultSize;
                foreach (var win in Editor.UI.MasterPanel.FloatingPanels)
                {
                    // Check if size is similar
                    if (Vector2.Abs(win.Size - defaultSize).LengthSquared < 100)
                    {
                        // Dock
                        window.Show(DockState.DockFill, win);
                        window.Focus();
                        return window;
                    }
                }

                // Show floating
                window.ShowFloating(defaultSize);
            }

            return window;
        }
    }
}
