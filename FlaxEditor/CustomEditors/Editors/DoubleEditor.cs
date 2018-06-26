// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System.Linq;
using FlaxEditor.CustomEditors.Elements;
using FlaxEngine;

namespace FlaxEditor.CustomEditors.Editors
{
    /// <summary>
    /// Default implementation of the inspector used to edit double value type properties.
    /// </summary>
    [CustomEditor(typeof(double)), DefaultEditor]
    public sealed class DoubleEditor : CustomEditor
    {
        private DoubleValueElement element;

        /// <inheritdoc />
        public override DisplayStyle Style => DisplayStyle.Inline;

        /// <inheritdoc />
        public override void Initialize(LayoutElementsContainer layout)
        {
            element = null;

            // Try get limit attribute for value min/max range setting and slider speed
            if (Values.Info != null)
            {
                var attributes = Values.Info.GetCustomAttributes(true);
                var limit = attributes.FirstOrDefault(x => x is LimitAttribute);
                if (limit != null)
                {
                    // Use double value editor with limit
                    var doubleValue = layout.DoubleValue();
                    doubleValue.SetLimits((LimitAttribute)limit);
                    doubleValue.DoubleValue.ValueChanged += OnValueChanged;
                    doubleValue.DoubleValue.SlidingEnd += ClearToken;
                    element = doubleValue;
                }
            }
            if (element == null)
            {
                // Use double value editor
                var doubleValue = layout.DoubleValue();
                doubleValue.DoubleValue.ValueChanged += OnValueChanged;
                doubleValue.DoubleValue.SlidingEnd += ClearToken;
                element = doubleValue;
            }
        }

        private void OnValueChanged()
        {
            var isSliding = element.IsSliding;
            var token = isSliding ? this : null;
            SetValue(element.Value, token);
        }

        /// <inheritdoc />
        public override void Refresh()
        {
            if (HasDifferentValues)
            {
                // TODO: support different values for ValueBox<T>
            }
            else
            {
                element.Value = (double)Values[0];
            }
        }
    }
}
