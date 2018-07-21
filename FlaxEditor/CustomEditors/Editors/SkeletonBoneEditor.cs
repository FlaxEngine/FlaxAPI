// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using FlaxEditor.CustomEditors.Elements;
using FlaxEngine;
using FlaxEngine.GUI;

namespace FlaxEditor.CustomEditors.Editors
{
    /// <summary>
    /// Custom editor for picking skeleton bone. Instead of choosing bone index or entering bone text it shows a combo box with simple tag picking by name.
    /// </summary>
    public sealed class SkeletonBoneEditor : CustomEditor
    {
        private ComboBoxElement element;

        /// <inheritdoc />
        public override DisplayStyle Style => DisplayStyle.Inline;

        /// <inheritdoc />
        public override void Initialize(LayoutElementsContainer layout)
        {
            element = layout.ComboBox();
            element.ComboBox.SelectedIndexChanged += OnSelectedIndexChanged;

            // Set bone names
            if (ParentEditor != null
                && ParentEditor.Values.Count == 1 && ParentEditor.Values[0] is BoneSocket boneSocket
                && boneSocket.Parent is AnimatedModel animatedModel && animatedModel.SkinnedModel
                && !animatedModel.SkinnedModel.WaitForLoaded())
            {
                var skeleton = animatedModel.SkinnedModel.Skeleton;
                for (int i = 0; i < skeleton.Length; i++)
                    element.ComboBox.AddItem(skeleton[i].Name);
            }
        }

        private void OnSelectedIndexChanged(ComboBox comboBox)
        {
            string value = comboBox.SelectedItem;
            SetValue(value);
        }

        /// <inheritdoc />
        public override void Refresh()
        {
            base.Refresh();

            if (HasDifferentValues)
            {
                // TODO: support different values on many actor selected
            }
            else
            {
                string value = (string)Values[0];
                element.ComboBox.SelectedItem = value;
            }
        }
    }
}
