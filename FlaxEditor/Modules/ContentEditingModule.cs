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
            window = proxy.Open(Editor, item);
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

        /// <summary>
        /// Reimports the specified asset.
        /// </summary>
        /// <param name="asset">The asset.</param>
        public void Reimport(BinaryAssetItem asset)
        {
            throw new NotImplementedException("Reimporting binary assets");
        }

        /// <summary>
        /// Clones the asset to the temporary folder.
        /// </summary>
        /// <param name="item">The item to clone.</param>
        /// <param name="resultPath">The result path.</param>
        /// <returns>True if failed, otherwise false.</returns>
        public bool FastTempAssetClone(AssetItem item, out string resultPath)
        {
            var extension = System.IO.Path.GetExtension(item.Path);
            var id = Guid.NewGuid();
            resultPath = System.IO.Path.Combine(Globals.TemporaryFolder, id.ToString("N")) + extension;
            
            if (CloneAssetFile(resultPath, item.Path, id))
                return true;

            return false;
        }

        /// <summary>
        /// Duplicates the asset file and changes it's ID.
        /// </summary>
        /// <param name="dstPath">The destination file path.</param>
        /// <param name="srcPath">The source file path.</param>
        /// <param name="dstId">The destination asset identifier.</param>
        /// <returns>True if cannot perform that operation, otherwise false.</returns>
        public bool CloneAssetFile(string dstPath, string srcPath, Guid dstId)
        {
            // Use internal call
            return Editor.Internal_CloneAssetFile(dstPath, srcPath, ref dstId);
        }
    }
}
