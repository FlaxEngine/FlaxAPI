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
    /// Default implementation of the inspector used to edit Vector3 value type properties.
    /// </summary>
    [CustomEditor(typeof(Vector3)), DefaultEditor]
    public class Vector3Editor : CustomEditor
    {
        /// <summary>
        /// The X component element.
        /// </summary>
        protected FloatValueElement XElement;

        /// <summary>
        /// The Y component element.
        /// </summary>
        protected FloatValueElement YElement;

        /// <summary>
        /// The Z component element.
        /// </summary>
        protected FloatValueElement ZElement;

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

            LimitAttribute limit = null;
            if (Values.Info != null)
            {
                var attributes = Values.Info.GetCustomAttributes(true);
                limit = (LimitAttribute)attributes.FirstOrDefault(x => x is LimitAttribute);
            }

            XElement = grid.FloatValue();
            XElement.SetLimits(limit);
            XElement.FloatValue.ValueChanged += OnValueChanged;

            YElement = grid.FloatValue();
            YElement.SetLimits(limit);
            YElement.FloatValue.ValueChanged += OnValueChanged;

            ZElement = grid.FloatValue();
            ZElement.SetLimits(limit);
            ZElement.FloatValue.ValueChanged += OnValueChanged;
        }

        private void OnValueChanged()
        {
            if (IsSetBlocked)
                return;

            SetValue(new Vector3(
                XElement.FloatValue.Value,
                YElement.FloatValue.Value,
                ZElement.FloatValue.Value));
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
                var value = (Vector3)Values[0];
                XElement.FloatValue.Value = value.X;
                YElement.FloatValue.Value = value.Y;
                ZElement.FloatValue.Value = value.Z;
            }
        }
    }
}
