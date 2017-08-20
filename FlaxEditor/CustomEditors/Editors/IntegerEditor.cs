////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

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
        private IntegerValueElement element;

        /// <inheritdoc />
        public override bool IsInline => true;

        /// <inheritdoc />
        public override void Initialize(LayoutElementsContainer layout)
        {
            if (Values == null)
                return;

            element = layout.IntegerValue();
            element.IntValue.ValueChanged += () => SetValue(element.IntValue.Value);
        }

        /// <inheritdoc />
        public override void Refresh()
        {
            if (HasDiffrentValues)
            {
                // TOOD: support different values for ValueBox<T>
            }
            else
            {
                element.IntValue.Value = (int)Values[0];
            }
        }
    }
}
