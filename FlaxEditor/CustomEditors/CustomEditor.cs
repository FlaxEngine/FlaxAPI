// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System;
using System.Collections.Generic;
using System.Reflection;
using FlaxEditor.CustomEditors.GUI;
using FlaxEngine;
using FlaxEngine.GUI;

namespace FlaxEditor.CustomEditors
{
    /// <summary>
    /// Custom editor layout style modes.
    /// </summary>
    public enum DisplayStyle
    {
        /// <summary>
        /// Creates a separate group for the editor (drop down element). This is a default value.
        /// </summary>
        Group,

        /// <summary>
        /// Inlines editor contents into the property area without creating a drop-down group.
        /// </summary>
        Inline,

        /// <summary>
        /// Inlines editor contents into the parent editor layout. Won;t use property with label name.
        /// </summary>
        InlineIntoParent,
    }

    /// <summary>
    /// Base class for all custom editors used to present object(s) properties. Allows to extend game objects editing with more logic and customization.
    /// </summary>
    public abstract class CustomEditor
    {
        private LayoutElementsContainer _layout;
        private CustomEditorPresenter _presenter;
        private CustomEditor _parent;
        private readonly List<CustomEditor> _children = new List<CustomEditor>();
        private ValueContainer _values;
        private bool _isSetBlocked;
        private bool _hasValueDirty;
        private bool _rebuildOnRefresh;
        private object _valueToSet;

        /// <summary>
        /// Gets the custom editor style.
        /// </summary>
        public virtual DisplayStyle Style => DisplayStyle.Group;

        /// <summary>
        /// Gets a value indicating whether single object is selected.
        /// </summary>
        public bool IsSingleObject => _values.IsSingleObject;

        /// <summary>
        /// Gets a value indicating whether selected objects are different values.
        /// </summary>
        public bool HasDifferentValues => _values.HasDifferentValues;

        /// <summary>
        /// Gets a value indicating whether selected objects are different types.
        /// </summary>
        public bool HasDifferentTypes => _values.HasDifferentTypes;

        /// <summary>
        /// Gets the values types array (without duplicates).
        /// </summary>
        public Type[] ValuesTypes => _values.ValuesTypes;

        /// <summary>
        /// Gets the values.
        /// </summary>
        public ValueContainer Values => _values;

        /// <summary>
        /// Gets the parent editor.
        /// </summary>
        public CustomEditor ParentEditor => _parent;

        /// <summary>
        /// Gets the children editors (readonly).
        /// </summary>
        public List<CustomEditor> ChildrenEditors => _children;

        /// <summary>
        /// Gets the presenter control. It's the top most part of the Custom Editors layout.
        /// </summary>
        public CustomEditorPresenter Presenter => _presenter;

        /// <summary>
        /// Gets a value indicating whether setting value is blocked (during refresh).
        /// </summary>
        protected bool IsSetBlocked => _isSetBlocked;

        /// <summary>
        /// The linked label used to show this custom editor. Can be null if not used (eg. editor is inlined or is usign a very customized UI layout).
        /// </summary>
        protected PropertyNameLabel LinkedLabel;

        internal virtual void Initialize(CustomEditorPresenter presenter, LayoutElementsContainer layout, ValueContainer values)
        {
            _layout = layout;
            _presenter = presenter;
            _values = values;

            var prev = CurrentCustomEditor;
            CurrentCustomEditor = this;

            _isSetBlocked = true;
            Initialize(layout);
            Refresh();
            _isSetBlocked = false;

            CurrentCustomEditor = prev;
        }

        internal static CustomEditor CurrentCustomEditor;

        internal void OnChildCreated(CustomEditor child)
        {
            // Link
            _children.Add(child);
            child._parent = this;
        }

        /// <summary>
        /// Rebuilds the editor layout. Cleans the whole UI with child elements/editors and then builds new hierarchy. Call it only when necessary.
        /// </summary>
        public void RebuildLayout()
        {
            var values = _values;
            var presenter = _presenter;
            var layout = _layout;
            var control = layout.ContainerControl;
            var parent = _parent;
            var parentScrollV = (_presenter.Panel.Parent as Panel)?.VScrollBar?.Value ?? -1;

            control.IsLayoutLocked = true;
            control.DisposeChildren();

            layout.ClearLayout();
            Cleanup();

            _parent = parent;
            Initialize(presenter, layout, values);

            control.UnlockChildrenRecursive();
            control.PerformLayout();

            // Restore scroll value
            if (parentScrollV > -1 && _presenter != null && _presenter.Panel.Parent is Panel panel && panel.VScrollBar != null)
                panel.VScrollBar.Value = parentScrollV;
        }

        /// <summary>
        /// Rebuilds the editor layout on editor refresh. Postponed after dirty value gets synced. Call it after <see cref="SetValue"/> to update editor after actual value assign.
        /// </summary>
        public void RebuildLayoutOnRefresh()
        {
            _rebuildOnRefresh = true;
        }

        /// <summary>
        /// Initializes this editor.
        /// </summary>
        /// <param name="layout">The layout builder.</param>
        public abstract void Initialize(LayoutElementsContainer layout);

        /// <summary>
        /// Deinitializes this editor (unbind events and cleanup).
        /// </summary>
        protected virtual void Deinitialize()
        {
        }

        /// <summary>
        /// Cleanups this editor resources and child editors.
        /// </summary>
        internal void Cleanup()
        {
            Deinitialize();

            for (int i = 0; i < _children.Count; i++)
            {
                _children[i].Cleanup();
            }

            _children.Clear();
            _presenter = null;
            _parent = null;
            _hasValueDirty = false;
            _valueToSet = null;
            _values = null;
            _isSetBlocked = false;
            _rebuildOnRefresh = false;
            LinkedLabel = null;
        }

        internal void RefreshRoot()
        {
            for (int i = 0; i < _children.Count; i++)
                _children[i].RefreshRootChild();
        }

        internal void RefreshRootChild()
        {
            Refresh();

            for (int i = 0; i < _children.Count; i++)
                _children[i].RefreshInternal();
        }

        internal virtual void RefreshInternal()
        {
            if (_values == null)
                return;

            // Check if need to update value
            if (_hasValueDirty)
            {
                // Cleanup (won't retry update in case of exception)
                object val = _valueToSet;
                _hasValueDirty = false;
                _valueToSet = null;

                // Assign value
                _values.Set(_parent.Values, val);

                // Propagate values up (eg. when member of structure gets modified, also structure should be updated as a part of the other object)
                var obj = _parent;
                while (obj._parent != null)
                {
                    obj._parent.SyncChildValues(obj.Values);
                    obj = obj._parent;
                }
            }
            else
            {
                // Update values
                _values.Refresh(_parent.Values);
            }

            // Update itself
            _isSetBlocked = true;
            Refresh();
            _isSetBlocked = false;

            // Update children
            try
            {
                for (int i = 0; i < _children.Count; i++)
                    _children[i].RefreshInternal();
            }
            catch (TargetException ex)
            {
                // This happens when one of the edited objects changes its type.
                // Eg. from Label to TextBox, while both types were assigned to the same field of type Control.
                // It's valid, just rebuild the child editors and log the warning to keep it tracking.
                Editor.LogWarning("Exception while updating the child editors");
                Editor.LogWarning(ex);
                RebuildLayout();
            }

            // Rebuild if flag is set
            if (_rebuildOnRefresh)
            {
                _rebuildOnRefresh = false;
                RebuildLayout();
            }
        }

        /// <summary>
        /// Refreshes this editor.
        /// </summary>
        public virtual void Refresh()
        {
            if (LinkedLabel != null)
            {
                // Prefab value diff
                if (Values.HasReferenceValue)
                {
                    var style = FlaxEngine.GUI.Style.Current;
                    LinkedLabel.HighlightStripColor = CanRevertReferenceValue ? style.BackgroundSelected * 0.8f : Color.Transparent;
                }
            }
        }

        internal void LinkLabel(PropertyNameLabel label)
        {
            LinkedLabel = label;
        }

        private void RevertDiff(CustomEditor editor)
        {
            // Skip if no change detected
            if (!editor.Values.IsReferenceValueModified)
                return;

            if (editor.ChildrenEditors.Count == 0)
            {
                editor.SetValueToReference();
            }
            else
            {
                for (int i = 0; i < editor.ChildrenEditors.Count; i++)
                {
                    RevertDiff(editor.ChildrenEditors[i]);
                }
            }
        }

        /// <summary>
        /// Gets a value indicating whether this editor can revert the value to reference value.
        /// </summary>
        public bool CanRevertReferenceValue
        {
            get
            {
                if (!Values.IsReferenceValueModified)
                    return false;

                // Skip array items (show diff only on a bottom level properties and fields)
                if (ParentEditor != null && ParentEditor.Values.Type != null && ParentEditor.Values.Type.IsArray)
                    return false;

                return true;
            }
        }

        /// <summary>
        /// Reverts the property value to reference value (if has). Handles undo.
        /// </summary>
        public void RevertToReferenceValue()
        {
            if (!Values.HasReferenceValue)
                return;

            Editor.Log("Reverting object changes to prefab");

            RevertDiff(this);
        }

        /// <summary>
        /// Updates the reference value assigned to the editor's values container. Sends the event down the custom editors hierarchy to propagate the change.
        /// </summary>
        /// <remarks>
        /// Has no effect on editors that don't have reference value assigned.
        /// </remarks>
        public void RefreshReferenceValue()
        {
            if (!Values.HasReferenceValue)
                return;

            if (ParentEditor?.Values?.HasReferenceValue ?? false)
            {
                Values.RefreshReferenceValue(ParentEditor.Values.ReferenceValue);
            }

            for (int i = 0; i < ChildrenEditors.Count; i++)
            {
                ChildrenEditors[i].RefreshReferenceValue();
            }
        }

        /// <summary>
        /// Sets the editor value to the reference value (if assigned).
        /// </summary>
        public void SetValueToReference()
        {
            SetValue(Values.ReferenceValue);
        }

        /// <summary>
        /// Sets the object value. Actual update is performed during editor refresh in sync.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="token">The source editor token used by the value setter to support batching Undo actions (eg. for sliders or color pickers).</param>
        protected void SetValue(object value, object token = null)
        {
            if (_isSetBlocked)
                return;

            if (OnDirty(this, value, token))
            {
                _hasValueDirty = true;
                _valueToSet = value;
            }
        }

        /// <summary>
        /// Synchronizes the child values container with this editor values.
        /// </summary>
        /// <param name="values">The values to synchronize.</param>
        protected virtual void SyncChildValues(ValueContainer values)
        {
            values.Set(Values);
        }

        /// <summary>
        /// Called when custom editor gets dirty (UI value has been modified).
        /// Allows to filter the event, block it or handle in a custom way.
        /// </summary>
        /// <param name="editor">The editor.</param>
        /// <param name="value">The value.</param>
        /// <param name="token">The source editor token used by the value setter to support batching Undo actions (eg. for sliders or color pickers).</param>
        /// <returns>True if allow to handle this event, otherwise false.</returns>
        protected virtual bool OnDirty(CustomEditor editor, object value, object token = null)
        {
            ParentEditor.OnDirty(editor, value, token);
            return true;
        }

        /// <summary>
        /// Clears the token assigned with <see cref="OnDirty"/> parameter. Called on merged undo action end (eg. users stops using slider).
        /// </summary>
        protected virtual void ClearToken()
        {
            ParentEditor.ClearToken();
        }
    }
}
