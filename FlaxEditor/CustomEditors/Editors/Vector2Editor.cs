////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using FlaxEditor.CustomEditors.Elements;
using FlaxEngine;
using FlaxEngine.GUI;

namespace FlaxEditor.CustomEditors.Editors
{
    /// <summary>
    /// Default implementation of the inspector used to edit Vector2 value type properties.
    /// </summary>
    [CustomEditor(typeof(Vector2)), DefaultEditor]
    public sealed class Vector2Editor : CustomEditor
    {
        private FloatValueElement xElement;
        private FloatValueElement yElement;

        /// <inheritdoc />
        public override bool IsInline => true;

        /// <inheritdoc />
        public override void Initialize(LayoutElementsContainer layout)
        {
            if (Values == null)
                return;

            var grid = layout.Custom<UniformGridPanel>();
            grid.Custom.Height = TextBox.DefaultHeight;
            grid.Custom.SlotsHorizontally = 2;
            grid.Custom.SlotsVertically = 1;

            xElement = grid.FloatValue();
            xElement.FloatValue.ValueChanged += OnValueChanged;

            yElement = grid.FloatValue();
            yElement.FloatValue.ValueChanged += OnValueChanged;
        }

        private void OnValueChanged()
        {
            SetValue(new Vector2(
                xElement.FloatValue.Value,
                yElement.FloatValue.Value));
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
                var value = (Vector2)Values[0];
                xElement.FloatValue.Value = value.X;
                yElement.FloatValue.Value = value.Y;
            }
        }
    }
}
