////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System.Collections.Generic;
using System.Reflection;

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
            if (Values == null)
                return;

            // TODO: for structures get all public fields
            // TODO: for objects get all public properties
            // TODO: support attribues
            // TODO: spawn custom editors for every editable thing
            // TODO; use shared properties/fields across all selected objects values

            // Faster path for single object selected
            if (IsSingleObject)
            {
                var type = Values[0].GetType();

                //layout.Button("Type " + type.Name);

                if (type.IsClass)
                {
                    layout.Button("Type " + type.Name);
                    layout.Space(10);

                    // TODO: promote children to other base class like CustomEditorContainer ?

                    var properties = type.GetProperties();
                    for (int i = 0; i < properties.Length; i++)
                    {
                        var p = properties[i];

                        layout.Button("Property " + p.Name);

                        //var pValues = new ValueContainer() { p.GetValue(Values[0]) };

                        //var child = layout.Object(pValues);
                        //children.Add(child);
                    }
                }
                else
                {
                    layout.Button("No class type: " + type.Name);
                }
            }
            else
            {
                layout.Button("More than object selected");
            }

            /*// test code for building editor layout
            layout.Button("My button");
            var group = layout.Group("Super Group");
            group.Button("Inner button 1");
            group.Space(10);
            group.Button("Inner button 2");
            group.Space(10);
            group.Button("Inner button 3");
            group.Space(10);
            group.Button("Inner button 4");
            */
        }

        /// <inheritdoc />
        public override void Refresh()
        {
            for (int i = 0; i < children.Count; i++)
                children[i].Refresh();
        }
    }
}
