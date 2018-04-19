// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System;
using System.Collections.Generic;
using FlaxEditor.CustomEditors.Editors;
using FlaxEditor.CustomEditors.GUI;
using FlaxEngine;
using FlaxEngine.Rendering;

namespace FlaxEditor.CustomEditors.Dedicated
{
    /// <summary>
    /// <see cref="PostProcessSettings"/> editor.
    /// </summary>
    [CustomEditor(typeof(PostProcessSettings)), DefaultEditor]
    public class PostProcessSettingsEditor : GenericEditor
    {
        private readonly List<CheckablePropertyNameLabel> _labels = new List<CheckablePropertyNameLabel>(64);

        /// <inheritdoc />
        public override DisplayStyle Style => DisplayStyle.InlineIntoParent;

        /// <inheritdoc />
        protected override List<ItemInfo> GetItemsForType(Type type)
        {
            // Show structure properties
            return GetItemsForType(type, true, false);
        }

        /// <inheritdoc />
        protected override void SpawnProperty(LayoutElementsContainer itemLayout, ValueContainer itemValues, ItemInfo item)
        {
            var order = item.Order.Order;

            // Skip for PosFx Materials
            if (order == 10000)
            {
                base.SpawnProperty(itemLayout, itemValues, item);
                return;
            }

            // Add labels with a check box
            var label = new CheckablePropertyNameLabel(item.DisplayName);
            label.CheckBox.Tag = order;
            label.CheckChanged += CheckBoxOnCheckChanged;
            _labels.Add(label);
            itemLayout.Property(label, itemValues, item.OverrideEditor, item.TooltipText);
        }

        private void CheckBoxOnCheckChanged(CheckablePropertyNameLabel label)
        {
            if (IsSetBlocked)
                return;

            var order = (int)label.CheckBox.Tag;
            var value = (PostProcessSettings)Values[0];
            value.data.SetFlag(order, label.CheckBox.Checked);
            value.isDataDirty = true;
            SetValue(value);
        }

        /// <inheritdoc />
        public override void Refresh()
        {
            base.Refresh();

            // Update all labels
            var value = (PostProcessSettings)Values[0];
            for (int i = 0; i < _labels.Count; i++)
            {
                var order = (int)_labels[i].CheckBox.Tag;
                _labels[i].CheckBox.Checked = value.data.GetFlag(order);
            }
        }

        /// <inheritdoc />
        public override void Initialize(LayoutElementsContainer layout)
        {
            _labels.Clear();

            base.Initialize(layout);
        }
    }
}
