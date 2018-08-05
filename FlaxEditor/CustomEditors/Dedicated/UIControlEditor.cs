// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System;
using FlaxEditor.CustomEditors.Editors;
using FlaxEditor.GUI;
using FlaxEngine;
using FlaxEngine.GUI;

namespace FlaxEditor.CustomEditors.Dedicated
{
    /// <summary>
    /// Dedicated custom editor for <see cref="UIControl.Control"/> object.
    /// </summary>
    /// <seealso cref="FlaxEditor.CustomEditors.Editors.GenericEditor" />
    public sealed class UIControlControlEditor : GenericEditor
    {
        /// <inheritdoc />
        public override void Initialize(LayoutElementsContainer layout)
        {
            // Set control type button
            var space = layout.Space(20);
            float setTypeButtonWidth = 60.0f;
            var setTypeButton = new Button((layout.ContainerControl.Width - setTypeButtonWidth) / 2, 1, setTypeButtonWidth, 18)
            {
                TooltipText = "Sets the control to the given type",
                AnchorStyle = AnchorStyle.UpperCenter,
                Text = "Set Type",
                Parent = space.Spacer
            };
            setTypeButton.ButtonClicked += OnSetTypeButtonClicked;

            // Don't show editor if any control is invalid
            if (Values.HasNull)
            {
                var label = layout.Label("Select control type to create", TextAlignment.Center);
                label.Label.Enabled = false;
                return;
            }

            // Add control type helper label
            {
                var type = Values[0].GetType();
                var label = layout.AddPropertyItem("Type", "The type of the created control.");
                label.Label(type.FullName);
            }

            // Show control properties
            base.Initialize(layout);
        }

        private void OnSetTypeButtonClicked(Button button)
        {
            var controlTypes = Editor.Instance.CodeEditing.GetControlTypes();
            if (controlTypes.Count == 0)
                return;

            // Show context menu with list of controls to add
            var cm = new ItemsListContextMenu(180);
            for (int i = 0; i < controlTypes.Count; i++)
            {
                var controlType = controlTypes[i];
                cm.ItemsPanel.AddChild(new ItemsListContextMenu.Item(controlType.Name, controlType)
                {
                    TooltipText = controlType.FullName,
                });
            }

            cm.ItemClicked += controlType => SetType((Type)controlType.Tag);
            cm.SortChildren();
            cm.Show(button.Parent, button.BottomLeft);
        }

        private void SetType(Type controlType)
        {
            var uiControls = ParentEditor.Values;
            if (Presenter.Undo != null)
            {
                using (new UndoMultiBlock(Presenter.Undo, uiControls, "Set Control Type"))
                {
                    for (int i = 0; i < uiControls.Count; i++)
                    {
                        var uiControl = (UIControl)uiControls[i];
                        uiControl.Control = (Control)Activator.CreateInstance(controlType);
                    }
                }
            }
            else
            {
                for (int i = 0; i < uiControls.Count; i++)
                {
                    var uiControl = (UIControl)uiControls[i];
                    uiControl.Control = (Control)Activator.CreateInstance(controlType);
                }
            }

            ParentEditor.RebuildLayout();
        }
    }
}
