////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

namespace FlaxEditor.CustomEditors
{
    /// <summary>
    /// Base class for all custom editors used to present object(s) properties. Allows to extend game objects editing with more logic and customization.
    /// </summary>
    public abstract class CustomEditor
    {
        private ValueContainer _values;

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
        /// Gets the values.
        /// </summary>
        /// <value>
        /// The values.
        /// </value>
        public ValueContainer Values => _values;

        internal virtual void Initialize(LayoutElementsContainer layout, ValueContainer values)
        {
            _values = values;

            Initialize(layout);
            Refresh();
        }

        /// <summary>
        /// Initializes this editor.
        /// </summary>
        /// <param name="layout">The layout builder.</param>
        public abstract void Initialize(LayoutElementsContainer layout);

        /// <summary>
        /// Refreshes this editor.
        /// </summary>
        public abstract void Refresh();
    }
}
