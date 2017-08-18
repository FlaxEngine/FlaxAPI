////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using FlaxEditor.CustomEditors.Elements;
using FlaxEngine;

namespace FlaxEditor.CustomEditors.Editors
{
    /// <summary>
    /// Default implementation of the inspector used to edit string properties.
    /// </summary>
    [CustomEditor(typeof(string)), DefaultEditor]
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

            Debug.Log("-----> text changed to " + textBox.Text);
        }

        /// <inheritdoc />
        public override void Refresh()
        {
            if (HasDiffrentValues)
            {
                textBox.TextBox.Text = string.Empty;
                textBox.TextBox.WatermarkText = "Diffrent Values";
            }
            else
            {
                textBox.TextBox.Text = (string)Values[0];
                textBox.TextBox.WatermarkText = string.Empty;
            }
        }
    }
}
