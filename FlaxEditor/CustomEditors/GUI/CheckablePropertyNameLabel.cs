////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2018 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System;
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
        
        /// <summary>
        /// Event fired when 'checked' state gets changed.
        /// </summary>
        public event Action<CheckablePropertyNameLabel> CheckChanged;

        /// <inheritdoc />
        public CheckablePropertyNameLabel(string name)
            : base(name)
        {
            CheckBox = new CheckBox(2, 2)
            {
                Checked = true,
                Size = new Vector2(14),
                Parent = this
            };
            CheckBox.CheckChanged += UpdateStyle;
            Margin = new Margin(CheckBox.Right + 4, 0, 0, 0);
        }

        /// <summary>
        /// Updates the label style.
        /// </summary>
        protected void UpdateStyle()
        {
            CheckChanged?.Invoke(this);
            TextColor = CheckBox.Checked ? Color.White : new Color(0.6f);
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
