////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using FlaxEditor.CustomEditors.Elements;
using FlaxEngine;
using FlaxEngine.GUI;

namespace FlaxEditor.CustomEditors.Editors
{
    /// <summary>
    /// Default implementation of the inspector used to edit Vector4 value type properties.
    /// </summary>
    [CustomEditor(typeof(Vector4)), DefaultEditor]
    public sealed class Vector4Editor : CustomEditor
    {
        private FloatValueElement xElement;
        private FloatValueElement yElement;
        private FloatValueElement zElement;
        private FloatValueElement wElement;

        /// <inheritdoc />
        public override bool IsInline => true;

        /// <inheritdoc />
        public override void Initialize(LayoutElementsContainer layout)
        {
            var grid = layout.CustomContainer<UniformGridPanel>();
            var gridControl = grid.CustomControl;
            gridControl.Height = TextBox.DefaultHeight;
            gridControl.SlotsHorizontally = 4;
            gridControl.SlotsVertically = 1;

            xElement = grid.FloatValue();
            xElement.FloatValue.ValueChanged += OnValueChanged;

            yElement = grid.FloatValue();
            yElement.FloatValue.ValueChanged += OnValueChanged;

            zElement = grid.FloatValue();
            zElement.FloatValue.ValueChanged += OnValueChanged;

            wElement = grid.FloatValue();
            wElement.FloatValue.ValueChanged += OnValueChanged;
        }

        private void OnValueChanged()
        {
            if (IsSetBlocked)
                return;

            SetValue(new Vector4(
                xElement.FloatValue.Value,
                yElement.FloatValue.Value,
                zElement.FloatValue.Value,
                wElement.FloatValue.Value));
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
                var value = (Vector4)Values[0];
                xElement.FloatValue.Value = value.X;
                yElement.FloatValue.Value = value.Y;
                zElement.FloatValue.Value = value.Z;
                wElement.FloatValue.Value = value.W;
            }
        }
    }
}
