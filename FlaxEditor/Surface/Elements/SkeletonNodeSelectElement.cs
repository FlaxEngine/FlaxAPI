// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System;
using System.Text;
using FlaxEngine;

namespace FlaxEditor.Surface.Elements
{
    /// <summary>
    /// The Visject surface node element used to pick a skeleton node with a combo box.
    /// </summary>
    public class SkeletonNodeSelectElement : ComboBoxElement
    {
        /// <inheritdoc />
        public SkeletonNodeSelectElement(SurfaceNode parentNode, NodeElementArchetype archetype)
        : base(parentNode, archetype)
        {
            _isAutoSelect = true;

            UpdateComboBox();

            // Select saved value
            _selectedIndices.Clear();
            if (Archetype.ValueIndex != -1)
            {
                var selectedIndex = (int)ParentNode.Values[Archetype.ValueIndex];
                if (selectedIndex > -1 && selectedIndex < _items.Count)
                    SelectedIndex = selectedIndex;
            }
        }

        /// <summary>
        /// Updates the Combo Box items list from the current skeleton nodes hierarchy.
        /// </summary>
        protected void UpdateComboBox()
        {
            // Prepare
            var selectedBone = SelectedItem;
            _items.Clear();

            // Get the skeleton
            var surfaceParam = Surface.GetParameter(Windows.Assets.AnimationGraphWindow.BaseModelId);
            var skeleton = surfaceParam != null ? FlaxEngine.Content.Load<SkinnedModel>((Guid)surfaceParam.Value) : null;
            if (skeleton == null || !skeleton.IsLoaded)
            {
                SelectedIndex = -1;
                return;
            }

            // Update the items
            var bones = skeleton.Skeleton;
            _items.Capacity = Mathf.Max(_items.Capacity, bones.Length);
            StringBuilder sb = new StringBuilder(64);
            for (int i = 0; i < bones.Length; i++)
            {
                sb.Clear();
                int parent = bones[i].ParentIndex;
                while (parent != -1)
                {
                    sb.Append(" ");
                    parent = bones[parent].ParentIndex;
                }

                sb.Append("> ");
                sb.Append(bones[i].Name);

                _items.Add(sb.ToString());
            }

            // Restore the selected bone
            SelectedItem = selectedBone;
        }
    }
}
