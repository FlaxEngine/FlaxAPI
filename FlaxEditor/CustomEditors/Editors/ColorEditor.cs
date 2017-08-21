////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using FlaxEditor.CustomEditors.Elements;
using FlaxEngine;
using FlaxEngine.GUI;

namespace FlaxEditor.CustomEditors.Editors
{
    /// <summary>
    /// Default implementation of the inspector used to edit Color value type properties.
    /// </summary>
    [CustomEditor(typeof(Color)), DefaultEditor]
    public sealed class ColorEditor : CustomEditor
    {
        private CustomElement<ColorValueBox> element;

        /// <inheritdoc />
        public override bool IsInline => true;

        /// <inheritdoc />
        public override void Initialize(LayoutElementsContainer layout)
        {
            if (Values == null)
                return;

            element = layout.Custom<ColorValueBox>();
            element.CustomControl.ValueChanged += () => SetValue(element.CustomControl.Value);
        }

        /// <inheritdoc />
        public override void Refresh()
        {
            if (HasDiffrentValues)
            {
                // TODO: support different values on ColorValueBox
            }
            else
            {
                element.CustomControl.Value = (Color)Values[0];
            }
        }
    }
}
