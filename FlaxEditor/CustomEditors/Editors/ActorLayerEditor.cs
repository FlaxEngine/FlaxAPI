////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using FlaxEditor.Content.Settings;
using FlaxEditor.CustomEditors.Elements;
using FlaxEngine.GUI;

namespace FlaxEditor.CustomEditors.Editors
{
    /// <summary>
    /// Custom editor for picking actor layer. Instead of choosing bit mask or layer index it shows a combo box with simple layer picking by name.
    /// </summary>
    public sealed class ActorLayerEditor : CustomEditor
    {
        private ComboBoxElement element;

        /// <inheritdoc />
        public override DisplayStyle Style => DisplayStyle.Inline;

        /// <inheritdoc />
        public override void Initialize(LayoutElementsContainer layout)
        {
            element = layout.ComboBox();
            element.ComboBox.SelectedIndexChanged += OnSelectedIndexChanged;

            // Set layer names
            element.ComboBox.SetItems(LayersAndTagsSettings.GetCurrentLayers());
        }

        private void OnSelectedIndexChanged(ComboBox comboBox)
        {
            int value = comboBox.SelectedIndex;
            if (value == -1)
                value = 0;
            SetValue(value);
        }

        /// <inheritdoc />
        public override void Refresh()
        {
            if (HasDiffrentValues)
            {
                // TODO: support different values on many actor selected
            }
            else
            {
                element.ComboBox.SelectedIndex = (int)Values[0];
            }
        }
    }
}
