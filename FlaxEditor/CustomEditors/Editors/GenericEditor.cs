////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System.Collections.Generic;

namespace FlaxEditor.CustomEditors.Editors
{
    /// <summary>
    /// Default implementation of the inspector used when no specified inspector is provided for the type. Inspector 
    /// displays GUI for all the inspectable fields in the object.
    /// </summary>
    public sealed class GenericEditor : CustomEditor
    {
        private readonly List<CustomEditor> children = new List<CustomEditor>();

        /// <inheritdoc />
        public override void Initialize(LayoutElementsContainer layout)
        {
            // TODO: for structures get all public fields
            // TODO: for objects get all public properties
            // TODO: support attribues
            // TODO: spawn custom editors for every editable thing
            // TODO; use shared properties/fields across all selected objects values
            
            // test code for building editor layout
            layout.Button("My button");
            var group = layout.Group("Super Group");
            group.Button("Inner button 1");
            group.Button("Inner button 2");
            group.Button("Inner button 3");
        }

        /// <inheritdoc />
        public override void Refresh()
        {
            for (int i = 0; i < children.Count; i++)
                children[i].Refresh();
        }
    }
}
