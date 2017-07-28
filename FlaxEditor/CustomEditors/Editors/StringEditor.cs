////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System;
using FlaxEditor.CustomEditors.Elements;

namespace FlaxEditor.CustomEditors.Editors
{
    /// <summary>
    /// Default implementation of the inspector used to edit string properties.
    /// </summary>
    public sealed class StringEditor : CustomEditor
    {
        private TextBoxElement textBox;

        /// <inheritdoc />
        public override void Initialize(LayoutElementsContainer layout)
        {
            if (Values == null)
                return;

            textBox = layout.TextBox(false);
            textBox.TextBox.EditEnd += OnTextChanged;
        }

        private void OnTextChanged()
        {
            // TODO: block events during values refresh

            // TODO: update values, send mark dirty event or sth
        }

        /// <inheritdoc />
        public override void Refresh()
        {
            var hasDiffrentValue = HasDiffrentValues;
            // TODO: handle diffrent values better
            textBox.TextBox.Text = hasDiffrentValue ? "Diffrent Values" : Values[0] as string;
        }
    }
}
