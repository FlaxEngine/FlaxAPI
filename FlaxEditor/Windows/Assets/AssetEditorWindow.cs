// Flax Engine scripting API

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlaxEditor.Content;
using FlaxEngine.GUI;

namespace FlaxEditor.Windows.Assets
{
    /// <summary>
    /// Base class for assets editing/viewing windows.
    /// </summary>
    /// <seealso cref="FlaxEditor.Windows.EditorWindow" />
    public abstract class AssetEditorWindow : EditorWindow, IEditable
    {
        protected AssetItem _item;

        /// <summary>
        /// Gets the item.
        /// </summary>
        /// <value>
        /// The item.
        /// </value>
        public AssetItem Item => _item;

        /// <summary>
        /// Initializes a new instance of the <see cref="AssetEditorWindow"/> class.
        /// </summary>
        /// <param name="editor">The editor.</param>
        /// <param name="item">The item.</param>
        protected AssetEditorWindow(Editor editor, AssetItem item)
            : base(editor, false, ScrollBars.None)
        {
            _item = item;
        }

        /// <summary>
        /// Tries to save asset changes if it has been edited.
        /// </summary>
        public abstract void Save();

        #region IEditable Implementation

        private bool _isEdited;

        /// <inheritdoc />
        public event Action OnEdited;

        /// <inheritdoc />
        public bool IsEdited
        {
            get => _isEdited;
            protected set
            {
                if (value)
                    MarkAsEdited();
                else
                    ClearEditedFlag();
            }
        }

        /// <inheritdoc />
        public void MarkAsEdited()
        {
            // Check if state will change
            if (_isEdited == false)
            {
                // Set flag
                _isEdited = true;

                // Call events
                OnEditedState();
                OnEdited?.Invoke();
                OnEditedStateChanged();
            }
        }

        /// <summary>
        /// Clears the edited flag.
        /// </summary>
        protected void ClearEditedFlag()
        {
            // Check if state will change
            if (_isEdited)
            {
                // Clear flag
                _isEdited = false;

                // Call event
                OnEditedStateChanged();
            }
        }

        /// <summary>
        /// Action fired when object gets edited.
        /// </summary>
        protected virtual void OnEditedState()
        {
        }

        /// <summary>
        /// Action firecd when object edited state gets changed.
        /// </summary>
        protected virtual void OnEditedStateChanged()
        {
        }

        #endregion

        /// <inheritdoc />
        public override bool IsEditingItem(ContentItem item)
        {
            return item == _item;
        }
    }
}
