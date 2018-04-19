// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System.Linq;
using FlaxEditor.CustomEditors.Elements;
using FlaxEditor.Surface.Elements;
using FlaxEngine;

namespace FlaxEditor.CustomEditors.Editors
{
    /// <summary>
    /// Default implementation of the inspector used to edit string properties.
    /// </summary>
    [CustomEditor(typeof(string)), DefaultEditor]
    public sealed class StringEditor : CustomEditor
    {
        private TextBoxElement element;

        /// <inheritdoc />
        public override DisplayStyle Style => DisplayStyle.Inline;

	    /// <inheritdoc />
	    public override void Initialize(LayoutElementsContainer layout)
	    {
		    bool isMultiLine = false;

		    if (Values.Info != null)
		    {
			    var attributes = Values.Info.GetCustomAttributes(true);
			    var multiLine = attributes.FirstOrDefault(x => x is MultilineTextAttribute);
			    if (multiLine != null)
			    {
				    isMultiLine = true;
			    }
		    }

		    element = layout.TextBox(isMultiLine);
		    element.TextBox.EditEnd += () => SetValue(element.Text);
	    }

	    /// <inheritdoc />
        public override void Refresh()
        {
            if (HasDiffrentValues)
            {
                element.TextBox.Text = string.Empty;
                element.TextBox.WatermarkText = "Different Values";
            }
            else
            {
                element.TextBox.Text = (string)Values[0];
                element.TextBox.WatermarkText = string.Empty;
            }
        }
    }
}

