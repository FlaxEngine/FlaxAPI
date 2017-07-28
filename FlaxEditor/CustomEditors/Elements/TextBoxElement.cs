////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using FlaxEngine.GUI;

namespace FlaxEditor.CustomEditors.Elements
{
    /// <summary>
    /// The textbox element.
    /// </summary>
    /// <seealso cref="FlaxEditor.CustomEditors.LayoutElement" />
    public class TextBoxElement : LayoutElement
    {
        /// <summary>
        /// The text box.
        /// </summary>
        public readonly TextBox TextBox;

        /// <summary>
        /// Initializes a new instance of the <see cref="TextBoxElement"/> class.
        /// </summary>
        /// <param name="isMultiline">Enable/disable multiline text input support</param>
        public TextBoxElement(bool isMultiline = false)
        {
            TextBox = new TextBox(isMultiline, 0, 0);
        }

        /// <inheritdoc />
        public override Control Control => TextBox;
    }
}
