using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlaxEditor.CustomEditors;
using FlaxEditor.CustomEditors.Elements;
using FlaxEditor.GUI;
using FlaxEngine;
using FlaxEngine.GUI;

namespace FlaxEditor.Options
{
    /// <summary>
    /// Theme options data container object.
    /// </summary>
    [CustomEditor(typeof(Editor<ThemeOptions>))]
    public sealed class ThemeOptions
    {
        internal class StyleOptionsEditor : CustomEditor
        {
            private ComboBoxElement _combobox;

            /// <inheritdoc />
            public override DisplayStyle Style => DisplayStyle.Inline;

            /// <inheritdoc />
            public override void Initialize(LayoutElementsContainer layout)
            {
                _combobox = layout.ComboBox();
                ReloadOptions(_combobox.ComboBox);
                _combobox.ComboBox.SelectedIndexChanged += OnComboBoxSelectedIndexChanged;
                _combobox.ComboBox.PopupShowing += ReloadOptions;
            }

            private void ReloadOptions(ComboBox obj)
            {
                var themeOptions = (ThemeOptions)ParentEditor.Values[0];
                var options = new string[themeOptions.Styles.Count + 1];
                options[0] = "Default";

                int i = 0;
                foreach (var styleName in themeOptions.Styles.Keys)
                {
                    options[i + 1] = styleName;
                    i++;
                }
                _combobox.ComboBox.SetItems(options);
                _combobox.ComboBox.SelectedItem = (string)Values[0];
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
        /// Currently selected style
        /// </summary>
        [CustomEditor(typeof(StyleOptionsEditor))]
        [Tooltip("Restart Editor to apply style")]
        public string SelectedStyle = "Default";

        /// <summary>
        /// All available styles
        /// </summary>
        public Dictionary<string, Style> Styles { get; set; } = new Dictionary<string, Style>();
    }
}
