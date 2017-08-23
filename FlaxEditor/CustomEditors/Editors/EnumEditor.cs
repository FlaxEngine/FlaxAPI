////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System;
using FlaxEditor.CustomEditors.Elements;
using FlaxEngine;

namespace FlaxEditor.CustomEditors.Editors
{
    /// <summary>
    /// Default implementation of the inspector used to edit float value type properties.
    /// </summary>
    [CustomEditor(typeof(Enum)), DefaultEditor]
    public sealed class EnumEditor : CustomEditor
    {
        private EnumElement element;

        /// <inheritdoc />
        public override bool IsInline => true;

        /// <inheritdoc />
        public override void Initialize(LayoutElementsContainer layout)
        {
            if (HasDiffrentTypes)
            {
                // No support for diffrent enum types
            }
            else
            {
                element = layout.Enum(Values[0].GetType());
                element.ValueChanged += () => SetValue(element.EnumTypeValue);
            }
        }

        /// <inheritdoc />
        public override void Refresh()
        {
            if (HasDiffrentValues)
            {
                // No support for diffrent enum values
            }
            else
            {
                element.EnumTypeValue = Values[0];
            }
        }
    }
}
