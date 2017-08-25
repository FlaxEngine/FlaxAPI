////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using FlaxEngine;
using FlaxEngine.GUI;

namespace FlaxEditor.CustomEditors.GUI
{
    /// <summary>
    /// Custom property name label that contains a checkbox used to enable/disable a property.
    /// </summary>
    /// <seealso cref="FlaxEditor.CustomEditors.GUI.PropertyNameLabel" />
    public class CheckablePropertyNameLabel : PropertyNameLabel
    {
        /// <summary>
        /// The check box.
        /// </summary>
        public readonly CheckBox CheckBox;

        /// <inheritdoc />
        public CheckablePropertyNameLabel(string name)
            : base(name)
        {
            CheckBox = new CheckBox(2, 2)
            {
                Size = new Vector2(14),
                Parent = this
            };
            Margin = new Margin(CheckBox.Right + 4, 0, 0, 0);
        }

        /// <inheritdoc />
        protected override void PerformLayoutSelf()
        {
            base.PerformLayoutSelf();
            
            // Center checkbox
            CheckBox.Y = (Height - CheckBox.Height) / 2;
        }
    }
}
