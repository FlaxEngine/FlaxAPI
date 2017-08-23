////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using FlaxEditor.CustomEditors.Elements;
using FlaxEngine;
using FlaxEngine.GUI;

namespace FlaxEditor.CustomEditors.Editors
{
    /// <summary>
    /// Default implementation of the inspector used to edit Quaternion value type properties.
    /// </summary>
    [CustomEditor(typeof(Quaternion)), DefaultEditor]
    public sealed class QuaternionEditor : CustomEditor
    {
        private FloatValueElement xElement;
        private FloatValueElement yElement;
        private FloatValueElement zElement;

        /// <inheritdoc />
        public override DisplayStyle Style => DisplayStyle.Inline;

        /// <inheritdoc />
        public override void Initialize(LayoutElementsContainer layout)
        {
            var grid = layout.CustomContainer<UniformGridPanel>();
            var gridControl = grid.CustomControl;
            gridControl.Height = TextBox.DefaultHeight;
            gridControl.SlotsHorizontally = 3;
            gridControl.SlotsVertically = 1;

            xElement = grid.FloatValue();
            xElement.FloatValue.ValueChanged += OnValueChanged;

            yElement = grid.FloatValue();
            yElement.FloatValue.ValueChanged += OnValueChanged;

            zElement = grid.FloatValue();
            zElement.FloatValue.ValueChanged += OnValueChanged;
        }

        private void OnValueChanged()
        {
            if (IsSetBlocked)
                return;

            float x = xElement.FloatValue.Value;
            float y = yElement.FloatValue.Value;
            float z = zElement.FloatValue.Value;
            Quaternion quat;
            Quaternion.Euler(x, y, z, out quat);
            SetValue(quat);
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
                var value = (Quaternion)Values[0];
                var euler = value.EulerAngles;
                xElement.FloatValue.Value = euler.X;
                yElement.FloatValue.Value = euler.Y;
                zElement.FloatValue.Value = euler.Z;
            }
        }
    }
}
