////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System.Collections.Generic;
using System.Reflection;
using FlaxEditor.CustomEditors.Elements;
using FlaxEngine.Assertions;
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
            OnAddElement(element);
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
            OnAddElement(element);
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
            OnAddElement(element);
            return element;
        }

        /// <summary>
        /// Adds new text box element.
        /// </summary>
        /// <param name="isMultiline">Enable/disable multiline text input support</param>
        /// <returns>The created element.</returns>
        public TextBoxElement TextBox(bool isMultiline = false)
        {
            TextBoxElement element = new TextBoxElement(isMultiline);
            OnAddElement(element);
            return element;
        }

        /// <summary>
        /// Adds new check box element.
        /// </summary>
        /// <returns>The created element.</returns>
        public CheckBoxElement Checkbox()
        {
            CheckBoxElement element = new CheckBoxElement();
            OnAddElement(element);
            return element;
        }

        /// <summary>
        /// Adds new float value element.
        /// </summary>
        /// <returns>The created element.</returns>
        public FloatValueElement FloatValue()
        {
            FloatValueElement element = new FloatValueElement();
            OnAddElement(element);
            return element;
        }

        /// <summary>
        /// Adds new integer value element.
        /// </summary>
        /// <returns>The created element.</returns>
        public IntegerValueElement IntegerValue()
        {
            IntegerValueElement element = new IntegerValueElement();
            OnAddElement(element);
            return element;
        }

        /// <summary>
        /// Adds object(s) editor. Selects proper <see cref="CustomEditor"/> based on overrides.
        /// </summary>
        /// <param name="member">The member.</param>
        /// <param name="values">The values.</param>
        /// <param name="overrideEditor">The custom editor to use. If null will detect it by auto.</param>
        /// <returns>The created element.</returns>
        public CustomEditor Object(MemberInfo member, ValueContainer values, CustomEditor overrideEditor = null)
        {
            var editor = overrideEditor ?? CustomEditorsUtil.CreateEditor(member);

            OnAddEditor(editor);
            editor.Initialize(this, values);

            return editor;
        }
        
        /// <summary>
        /// Adds object property editor. Selects proper <see cref="CustomEditor"/> based on overrides.
        /// </summary>
        /// <param name="name">The property name.</param>
        /// <param name="member">The member.</param>
        /// <param name="values">The values.</param>
        /// <param name="overrideEditor">The custom editor to use. If null will detect it by auto.</param>
        /// <returns>The created element.</returns>
        public CustomEditor Property(string name, MemberInfo member, ValueContainer values, CustomEditor overrideEditor = null)
        {
            var editor = overrideEditor ?? CustomEditorsUtil.CreateEditor(member);
            
            if (!editor.IsInline)
            {
                var group = Group(name);
                return group.Object(member, values, editor);
            }

            // TODO: reuse previous element
            
            PropertiesListElement element;

            element = new PropertiesListElement();
            OnAddElement(element);
            var obj = element.Object(member, values, editor);
            obj.PropertyName = name;
            return obj;
        }

        /// <summary>
        /// Called when element is added to the layout.
        /// </summary>
        /// <param name="element">The element.</param>
        protected virtual void OnAddElement(LayoutElement element)
        {
            element.Control.Parent = ContainerControl;
            Children.Add(element);
        }

        /// <summary>
        /// Called when editor is added.
        /// </summary>
        /// <param name="editor">The editor.</param>
        protected virtual void OnAddEditor(CustomEditor editor)
        {
            // This could be passed by the calling code but it's easier to hide it from the user
            // Note: we need that custom editor to link generated editor into the parent
            var customEditor = CustomEditor.CurrentCustomEditor;
            Assert.IsNotNull(customEditor);
            customEditor.OnChildCreated(editor);
        }

        /// <inheritdoc />
        public override Control Control => ContainerControl;
    }
}
