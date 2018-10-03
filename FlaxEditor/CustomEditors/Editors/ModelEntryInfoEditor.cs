// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using FlaxEditor.CustomEditors.Elements;
using FlaxEngine;

namespace FlaxEditor.CustomEditors.Editors
{
    /// <summary>
    /// Default implementation of the inspector used to edit ModelEntryInfo value type properties.
    /// </summary>
    [CustomEditor(typeof(ModelEntryInfo)), DefaultEditor]
    public sealed class ModelEntryInfoEditor : GenericEditor
    {
        private GroupElement _group;
        private bool _updateName;

        /// <inheritdoc />
        public override void Initialize(LayoutElementsContainer layout)
        {
            _updateName = true;
            var group = layout.Group("Entry");
            _group = group;

            base.Initialize(group);
        }

        /// <inheritdoc />
        public override void Refresh()
        {
            if (_updateName)
            {
                TryToGetName();
            }

            base.Refresh();
        }

        private void TryToGetName()
        {
            if (_group != null &&
                Values.Count > 0 &&
                Values[0] is ModelEntryInfo entry
                && ParentEditor?.ParentEditor != null
                && ParentEditor.ParentEditor.Values.Count > 0)
            {
                if (ParentEditor.ParentEditor.Values[0] is StaticModel staticModel)
                {
                    var model = staticModel.Model;
                    if (model && model.IsLoaded)
                    {
                        _group.Panel.HeaderText = "Entry " + model.MaterialSlots[entry.Index].Name;
                        _updateName = false;
                    }
                }
                else if (ParentEditor.ParentEditor.Values[0] is AnimatedModel animatedModel)
                {
                    var model = animatedModel.SkinnedModel;
                    if (model && model.IsLoaded)
                    {
                        _group.Panel.HeaderText = "Entry " + model.MaterialSlots[entry.Index].Name;
                        _updateName = false;
                    }
                }
            }
        }
    }
}
