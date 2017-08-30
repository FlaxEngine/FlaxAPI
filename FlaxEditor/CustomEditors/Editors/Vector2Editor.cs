////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System.Linq;
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
        public override DisplayStyle Style => DisplayStyle.Inline;

        /// <inheritdoc />
        public override void Initialize(LayoutElementsContainer layout)
        {
            var grid = layout.CustomContainer<UniformGridPanel>();
            var gridControl = grid.CustomControl;
            gridControl.Height = TextBox.DefaultHeight;
            gridControl.SlotsHorizontally = 2;
            gridControl.SlotsVertically = 1;

            LimitAttribute limit = null;
            if (Values.Info != null)
            {
                var attributes = Values.Info.GetCustomAttributes(true);
                limit = (LimitAttribute)attributes.FirstOrDefault(x => x is LimitAttribute);
            }

            xElement = grid.FloatValue();
            xElement.SetLimits(limit);
            xElement.FloatValue.ValueChanged += OnValueChanged;

            yElement = grid.FloatValue();
            yElement.SetLimits(limit);
            yElement.FloatValue.ValueChanged += OnValueChanged;
        }

        private void OnValueChanged()
        {
            if (IsSetBlocked)
                return;

            SetValue(new Vector2(
                xElement.FloatValue.Value,
                yElement.FloatValue.Value));
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
                var value = (Vector2)Values[0];
                xElement.FloatValue.Value = value.X;
                yElement.FloatValue.Value = value.Y;
            }
        }
    }
}
