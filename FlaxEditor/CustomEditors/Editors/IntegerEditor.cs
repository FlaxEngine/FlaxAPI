////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2018 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System.Linq;
using FlaxEditor.CustomEditors.Elements;
using FlaxEngine;

namespace FlaxEditor.CustomEditors.Editors
{
    /// <summary>
    /// Default implementation of the inspector used to edit inteager value type properties.
    /// </summary>
    [CustomEditor(typeof(int)), DefaultEditor]
    public sealed class IntegerEditor : CustomEditor
    {
        private IIntegerValueEditor element;

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
					// Use int value editor with limit
					var intValue = layout.IntegerValue();
			        intValue.SetLimits((LimitAttribute)limit);
			        intValue.IntValue.ValueChanged += OnValueChanged;
			        intValue.IntValue.SlidingEnd += ClearToken;
			        element = intValue;
		        }
	        }
	        if (element == null)
	        {
		        // Use int value editor
		        var intValue = layout.IntegerValue();
		        intValue.IntValue.ValueChanged += OnValueChanged;
		        intValue.IntValue.SlidingEnd += ClearToken;
		        element = intValue;
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
            if (HasDiffrentValues)
            {
                // TODO: support different values for ValueBox<T>
            }
            else
            {
                element.Value = (int)Values[0];
            }
        }
    }
}
