using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlaxEditor.CustomEditors.Elements;
using FlaxEditor.GUI;
using FlaxEngine;
using FlaxEngine.GUI;

namespace FlaxEditor.CustomEditors.Editors
{  /// <summary>
   /// Default implementation of the inspector used to edit styles.
   /// </summary>
    [CustomEditor(typeof(Style)), DefaultEditor]
    public class StyleEditor : CustomEditor
    {
        private CustomElement<StyleValueEditor> element;

        /// <inheritdoc />
        public override DisplayStyle Style => DisplayStyle.Inline;

        /// <summary>
        /// Initializes this editor.
        /// </summary>
        /// <param name="layout">The layout builder.</param>
        public override void Initialize(LayoutElementsContainer layout)
        {
            Style style = (Style)this.Values[0];

            element = layout.Custom<StyleValueEditor>();
            element.CustomControl.Value = style;
            element.CustomControl.ValueChanged += OnValueChanged;
            element.CustomControl.DockStyle = DockStyle.None;
        }

        private void OnValueChanged()
        {
            SetValue(element.CustomControl.Value);
        }

        /// <inheritdoc />
        public override void Refresh()
        {
            base.Refresh();

            if (HasDifferentValues)
            {
            }
            else
            {
                element.CustomControl.Value = (Style)Values[0];
            }
        }
    }
}
