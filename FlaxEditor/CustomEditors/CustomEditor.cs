////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Linq;
using FlaxEditor.CustomEditors.Elements;

namespace FlaxEditor.CustomEditors
{
    /// <summary>
    /// Base class for all custom editors used to present object(s) properties. Allows to extend game objects editing with more logic and customization.
    /// </summary>
    public abstract class CustomEditor
    {
        private CustomEditorPresenter _presenter;
        private CustomEditor _parent;
        private readonly List<CustomEditor> _children = new List<CustomEditor>();
        private ValueContainer _values;
        private bool _isSetBlocked;
        private bool _hasValueDirty;
        private object _valueToSet;

        /// <summary>
        /// Helper value used by the <see cref="PropertiesListElement.PropertiesList"/> to draw property name.
        /// </summary>
        internal string PropertyName;
        
        /// <summary>
        /// Helper value used by the <see cref="PropertiesListElement.PropertiesList"/> to draw property names in a proper area.
        /// </summary>
        internal int PropertyFirstChildControlIndex;

        /// <summary>
        /// Gets a value indicating whether inline editor contents into the property value, otherwise will use expandable group area.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this editor is inline into properties; otherwise, <c>false</c>.
        /// </value>
        public virtual bool IsInline => false;

        /// <summary>
        /// Gets a value indicating whether single object is selected.
        /// </summary>
        /// <value>
        ///   <c>true</c> if single object is selected; otherwise, <c>false</c>.
        /// </value>
        public bool IsSingleObject => _values.Count == 1;

        /// <summary>
        /// Gets a value indicating whether selected objects are diffrent values.
        /// </summary>
        /// <value>
        ///   <c>true</c> if selected objects are diffrent values; otherwise, <c>false</c>.
        /// </value>
        public bool HasDiffrentValues
        {
            get
            {
                for (int i = 1; i < _values.Count; i++)
                {
                    if (!Equals(_values[0], _values[i]))
                        return true;
                }
                return false;
            }
        }

        /// <summary>
        /// Gets a value indicating whether selected objects are diffrent types.
        /// </summary>
        /// <value>
        ///   <c>true</c> if selected objects are diffrent types; otherwise, <c>false</c>.
        /// </value>
        public bool HasDiffrentTypes
        {
            get
            {
                if (_values.Count < 2)
                    return false;
                var theFirstType = _values[0].GetType();
                for (int i = 1; i < _values.Count; i++)
                {
                    if (theFirstType != _values[1].GetType())
                        return true;
                }
                return false;
            }
        }

        /// <summary>
        /// Gets the values types array (without duplicates).
        /// </summary>
        /// <value>
        /// The values types.
        /// </value>
        public Type[] ValuesTypes
        {
            get
            {
                if (_values == null)
                    return new Type[0];
                if (_values.Count == 1)
                    return new []{_values[0].GetType()};
                return _values.ConvertAll(x => x.GetType()).Distinct().ToArray();
            }
        }

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

        internal virtual void Initialize(CustomEditorPresenter presenter, LayoutElementsContainer layout, ValueContainer values)
        {
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
