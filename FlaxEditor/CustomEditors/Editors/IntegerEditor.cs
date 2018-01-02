////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2018 Flax Engine. All rights reserved.
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
        public override DisplayStyle Style => DisplayStyle.Inline;

        /// <inheritdoc />
        public override void Initialize(LayoutElementsContainer layout)
        {
            element = layout.IntegerValue();
            element.SetLimits(Values.Info);
            element.IntValue.ValueChanged += () => SetValue(element.IntValue.Value);
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
                element.IntValue.Value = (int)Values[0];
            }
        }
    }
}
