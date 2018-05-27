// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System.Linq;
using FlaxEditor.CustomEditors.Elements;
using FlaxEngine;

namespace FlaxEditor.CustomEditors.Editors
{
    /// <summary>
    /// Default implementation of the inspector used to edit float value type properties.
    /// </summary>
    [CustomEditor(typeof(float)), DefaultEditor]
    public sealed class FloatEditor : CustomEditor
    {
        private IFloatValueEditor element;

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
                var range = attributes.FirstOrDefault(x => x is RangeAttribute);
                if (range != null)
                {
                    // Use slider
                    var slider = layout.Slider();
                    slider.SetLimits((RangeAttribute)range);
                    slider.Slider.ValueChanged += OnValueChanged;
                    slider.Slider.SlidingEnd += ClearToken;
                    element = slider;
                }
                var limit = attributes.FirstOrDefault(x => x is LimitAttribute);
                if (limit != null)
                {
                    // Use float value editor with limit
                    var floatValue = layout.FloatValue();
                    floatValue.SetLimits((LimitAttribute)limit);
                    floatValue.FloatValue.ValueChanged += OnValueChanged;
                    floatValue.FloatValue.SlidingEnd += ClearToken;
                    element = floatValue;
                }
            }
            if (element == null)
            {
                // Use float value editor
                var floatValue = layout.FloatValue();
                floatValue.FloatValue.ValueChanged += OnValueChanged;
                floatValue.FloatValue.SlidingEnd += ClearToken;
                element = floatValue;
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
                element.Value = (float)Values[0];
            }
        }
    }
}
