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
        private ValueContainer _values;
        private readonly List<CustomEditor> _children = new List<CustomEditor>();

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
                    if (_values[0] != _values[1])
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

        internal virtual void Initialize(LayoutElementsContainer layout, ValueContainer values)
        {
            _values = values;

            var prev = CurrentCustomEditor;
            CurrentCustomEditor = this;

            Initialize(layout);
            Refresh();

            CurrentCustomEditor = prev;
        }

        internal static CustomEditor CurrentCustomEditor;

        internal void OnChildCreated(CustomEditor child)
        {
            _children.Add(child);
        }

        /// <summary>
        /// Initializes this editor.
        /// </summary>
        /// <param name="layout">The layout builder.</param>
        public abstract void Initialize(LayoutElementsContainer layout);

        /// <summary>
        /// Refreshes this editor.
        /// </summary>
        public virtual void Refresh()
        {
            // Update children
            for (int i = 0; i < _children.Count; i++)
                _children[i].Refresh();
        }
    }
}
