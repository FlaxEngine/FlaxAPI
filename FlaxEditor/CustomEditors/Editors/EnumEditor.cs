// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System;
using System.Linq;
using FlaxEditor.CustomEditors.Elements;
using FlaxEngine;

namespace FlaxEditor.CustomEditors.Editors
{
    /// <summary>
    /// Default implementation of the inspector used to edit float value type properties.
    /// </summary>
    [CustomEditor(typeof(Enum)), DefaultEditor]
    public class EnumEditor : CustomEditor
    {
        /// <summary>
        /// The enum element.
        /// </summary>
        protected EnumElement element;

        /// <inheritdoc />
        public override DisplayStyle Style => DisplayStyle.Inline;

        /// <inheritdoc />
        public override void Initialize(LayoutElementsContainer layout)
        {
            var mode = EnumDisplayAttribute.FormatMode.Default;
            var attributes = Values.GetAttributes();
            if (attributes?.FirstOrDefault(x => x is EnumDisplayAttribute) is EnumDisplayAttribute enumDisplay)
            {
                mode = enumDisplay.Mode;
            }

            if (HasDifferentTypes)
            {
                // No support for different enum types
            }
            else
            {
                element = layout.Enum(Values[0].GetType(), null, mode);
                element.ValueChanged += OnValueChanged;
            }
        }

        /// <summary>
        /// Called when value get changed. Allows to override default value setter logic.
        /// </summary>
        protected virtual void OnValueChanged()
        {
            SetValue(element.EnumTypeValue);
        }

        /// <inheritdoc />
        public override void Refresh()
        {
            base.Refresh();

            if (HasDifferentValues)
            {
                // No support for different enum values
            }
            else
            {
                element.EnumTypeValue = Values[0];
            }
        }
    }
}
