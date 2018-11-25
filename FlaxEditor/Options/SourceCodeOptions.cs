// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System.ComponentModel;
using FlaxEditor.CustomEditors;
using FlaxEditor.CustomEditors.Elements;
using FlaxEngine;
using FlaxEngine.GUI;

namespace FlaxEditor.Options
{
    /// <summary>
    /// Source code options data container.
    /// </summary>
    [CustomEditor(typeof(Editor<SourceCodeOptions>))]
    public sealed class SourceCodeOptions
    {
        /// <summary>
        /// Editor for the editing of the editable SourceCodeEditor property.
        /// </summary>
        /// <seealso cref="FlaxEditor.CustomEditors.CustomEditor" />
        internal class SourceCodeEditorEditor : CustomEditor
        {
            private ComboBoxElement _combobox;

            /// <inheritdoc />
            public override DisplayStyle Style => DisplayStyle.Inline;

            /// <inheritdoc />
            public override void Initialize(LayoutElementsContainer layout)
            {
                var editors = Editor.Instance.CodeEditing.Editors;
                var options = new string[editors.Count + 1];
                options[0] = "None";
                for (int i = 0; i < editors.Count; i++)
                {
                    options[i + 1] = editors[i].Name;
                }

                _combobox = layout.ComboBox();
                _combobox.ComboBox.SetItems(options);
                _combobox.ComboBox.SelectedItem = (string)Values[0];
                _combobox.ComboBox.SelectedIndexChanged += OnComboBoxSelectedIndexChanged;
            }

            private void OnComboBoxSelectedIndexChanged(ComboBox combobox)
            {
                SetValue(combobox.SelectedItem);
            }

            /// <inheritdoc />
            public override void Refresh()
            {
                _combobox.ComboBox.SelectedItem = (string)Values[0];

                base.Refresh();
            }
        }

        /// <summary>
        /// Gets or sets the source code editing IDE to use for project and source files accessing.
        /// </summary>
        [DefaultValue("Default"), CustomEditor(typeof(SourceCodeEditorEditor))]
        [EditorDisplay("Accessor"), EditorOrder(100), Tooltip("The source code editing IDE to use for project and source files accessing.")]
        public string SourceCodeEditor { get; set; } = "Default";
    }
}
