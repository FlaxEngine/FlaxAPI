////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System.Collections.Generic;

namespace FlaxEditor.CustomEditors
{
    /// <summary>
    /// Synchronizes objects modifications and records undo operations.
    /// Allows to override undo action target objects for the part of the custome ditors hierarchy.
    /// </summary>
    /// <seealso cref="FlaxEditor.CustomEditors.CustomEditor" />
    public class SyncPointEditor : CustomEditor
    {
        /// <summary>
        /// The 'is dirty' flag.
        /// </summary>
        protected bool _isDirty;
        
        /// <summary>
        /// Gets the undo objects used to record undo operation changes.
        /// </summary>
        public virtual IEnumerable<object> UndoObjects => Presenter.GetUndoObjects(Presenter);

        /// <summary>
        /// Gets the undo.
        /// </summary>
        public virtual Undo Undo => Presenter.Undo;
        
        /// <inheritdoc />
        public override void Initialize(LayoutElementsContainer layout)
        {
            _isDirty = false;
        }

        internal override void RefreshInternal()
        {
            bool isDirty = _isDirty;
            _isDirty = false;

            // If any UI control has been modified we should try to record selected objects change
            if (isDirty && Undo != null)
            {
                using (new UndoMultiBlock(Undo, UndoObjects, "Edit object(s)"))
                    Refresh();
            }
            else
            {
                Refresh();
            }

            if (isDirty)
                OnModified();
        }

        /// <inheritdoc />
        public override void Refresh()
        {
            RefreshRoot();
        }

        /// <summary>
        /// Called when data gets modified by the custom editors.
        /// </summary>
        protected virtual void OnModified()
        {
        }
        
        /// <inheritdoc />
        protected override bool OnDirty(CustomEditor editor, object value)
        {
            // Mark as modified and don't pass event futher
            _isDirty = true;
            return false;
        }
    }
}
