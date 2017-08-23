////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
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
        private object _valueToSet;

        /// <summary>
        /// Gets the custom editor style.
        /// </summary>
        /// <value>
        /// The style.
        /// </value>
        public virtual DisplayStyle Style => DisplayStyle.Group;
        
        /// <summary>
        /// Gets a value indicating whether single object is selected.
        /// </summary>
        /// <value>
        ///   <c>true</c> if single object is selected; otherwise, <c>false</c>.
        /// </value>
        public bool IsSingleObject => _values.IsSingleObject;

        /// <summary>
        /// Gets a value indicating whether selected objects are diffrent values.
        /// </summary>
        /// <value>
        ///   <c>true</c> if selected objects are diffrent values; otherwise, <c>false</c>.
        /// </value>
        public bool HasDiffrentValues => _values.HasDiffrentValues;

        /// <summary>
        /// Gets a value indicating whether selected objects are diffrent types.
        /// </summary>
        /// <value>
        ///   <c>true</c> if selected objects are diffrent types; otherwise, <c>false</c>.
        /// </value>
        public bool HasDiffrentTypes => _values.HasDiffrentTypes;

        /// <summary>
        /// Gets the values types array (without duplicates).
        /// </summary>
        /// <value>
        /// The values types.
        /// </value>
        public Type[] ValuesTypes => _values.ValuesTypes;

        /// <summary>
        /// Gets the values.
        /// </summary>
        /// <value>
        /// The values.
        /// </value>
        public ValueContainer Values => _values;

        /// <summary>
        /// Gets the presenter control. It's the top most part of the Custom Editors layout.
        /// </summary>
        /// <value>
        /// The presenter.
        /// </value>
        public CustomEditorPresenter Presenter => _presenter;

        /// <summary>
        /// Gets a value indicating whether setting value is blocked (during refresh).
        /// </summary>
        /// <value>
        ///   <c>true</c> if setting value is blocked; otherwise, <c>false</c>.
        /// </value>
        protected bool IsSetBlocked => _isSetBlocked;

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
            if (parentScrollV != -1)
                ((Panel)_presenter.Panel.Parent).VScrollBar.Value = parentScrollV;
        }
        
        /// <summary>
        /// Initializes this editor.
        /// </summary>
        /// <param name="layout">The layout builder.</param>
        public abstract void Initialize(LayoutElementsContainer layout);

        internal void Cleanup()
        {
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
        }

        internal void RefreshRoot()
        {
            // Update itself
            Refresh();

            // Update children
            for (int i = 0; i < _children.Count; i++)
                _children[i].RefreshInternal();
        }

        internal void RefreshInternal()
        {
            // Check if need to update value
            if (_hasValueDirty)
            {
                // Cleanup (won't retry update in case of exception)
                object val = _valueToSet;
                _hasValueDirty = false;
                _valueToSet = null;

                // Assign value
                _values.Set(_parent.Values, val);

                // Propagate values up if parent is not a ref type
                var obj = _parent;
                while (obj._parent != null && obj.Values.Type.IsValueType)
                {
                    obj.Values.Set(obj._parent.Values);
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
            for (int i = 0; i < _children.Count; i++)
                _children[i].RefreshInternal();
        }

        /// <summary>
        /// Refreshes this editor.
        /// </summary>
        public virtual void Refresh()
        {
        }
        
        /// <summary>
        /// Sets the object value. Actual update is performed during editor refresh in sync.
        /// </summary>
        /// <param name="value">The value.</param>
        protected void SetValue(object value)
        {
            if (_isSetBlocked)
                return;

            _hasValueDirty = true;
            _valueToSet = value;
            _presenter.OnDirty();
        }
    }
}
