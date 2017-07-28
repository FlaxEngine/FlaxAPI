////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System.Collections.Generic;
using FlaxEditor.CustomEditors.Editors;
using FlaxEditor.CustomEditors.Elements;
using FlaxEngine.GUI;

namespace FlaxEditor.CustomEditors
{
    /// <summary>
    /// Represents a container control for <see cref="LayoutElement"/>. Can contain child elements.
    /// </summary>
    /// <seealso cref="FlaxEditor.CustomEditors.LayoutElement" />
    public abstract class LayoutElementsContainer : LayoutElement
    {
        /// <summary>
        /// The children.
        /// </summary>
        public readonly List<LayoutElement> Children = new List<LayoutElement>();

        /// <summary>
        /// Gets the control represented by this element.
        /// </summary>
        /// <value>
        /// The control.
        /// </value>
        public abstract ContainerControl ContainerControl { get; }

        /// <summary>
        /// Adds new group element.
        /// </summary>
        /// <param name="title">The title.</param>
        /// <returns>The created element.</returns>
        public GroupElement Group(string title)
        {
            GroupElement element = new GroupElement();
            element.Init(title);
            element.Control.Parent = ContainerControl;
            Children.Add(element);
            return element;
        }

        /// <summary>
        /// Adds new button element.
        /// </summary>
        /// <param name="title">The title.</param>
        /// <returns>The created element.</returns>
        public ButtonElement Button(string title)
        {
            ButtonElement element = new ButtonElement();
            element.Init(title);
            element.Control.Parent = ContainerControl;
            Children.Add(element);
            return element;
        }

        /// <summary>
        /// Adds new space.
        /// </summary>
        /// <param name="height">The height.</param>
        /// <returns>The created element.</returns>
        public SpaceElement Space(float height)
        {
            SpaceElement element = new SpaceElement();
            element.Init(height);
            element.Control.Parent = ContainerControl;
            Children.Add(element);
            return element;
        }

        /// <summary>
        /// Adds new text box element.
        /// </summary>
        /// <param name="isMultiline">Enable/disable multiline text input support</param>
        /// <returns>The created element.</returns>
        public TextBoxElement TextBox(bool isMultiline)
        {
            TextBoxElement element = new TextBoxElement(isMultiline);
            element.Control.Parent = ContainerControl;
            Children.Add(element);
            return element;
        }

        /// <summary>
        /// Adds object(s) editor. Selects proper <see cref="CustomEditor"/> based on overrides.
        /// </summary>
        /// <param name="values">The values.</param>
        /// <returns>The created element.</returns>
        public CustomEditor Object(ValueContainer values)
        {
            // TODO: select proper editor (check attribues, global overrides, type overrides, etc.)

            var editor = new GenericEditor();
            editor.Initialize(this, values);
            return editor;
        }

        /// <inheritdoc />
        public override Control Control => ContainerControl;
    }
}
