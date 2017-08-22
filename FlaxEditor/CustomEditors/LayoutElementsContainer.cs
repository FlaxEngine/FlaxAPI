////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Reflection;
using FlaxEditor.CustomEditors.Elements;
using FlaxEngine;
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
        /// <param name="useTransparentHeader">True if use drop down icon and transparent group header, otherwise use normal style.</param>
        /// <returns>The created element.</returns>
        public GroupElement Group(string title, bool useTransparentHeader = false)
        {
            GroupElement element = new GroupElement();
            element.Init(title);
            if (useTransparentHeader)
            {
                element.Panel.EnableDropDownIcon = true;
                element.Panel.HeaderColor = element.Panel.HeaderColorMouseOver = Color.Transparent;
            }
            OnAddElement(element);
            return element;
        }

        /// <summary>
        /// Adds new button element.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns>The created element.</returns>
        public ButtonElement Button(string text)
        {
            ButtonElement element = new ButtonElement();
            element.Init(text);
            OnAddElement(element);
            return element;
        }

        /// <summary>
        /// Adds new custom element.
        /// </summary>
        /// <typeparam name="T">The custom control.</typeparam>
        /// <returns>The created element.</returns>
        public CustomElement<T> Custom<T>()
            where T : Control, new()
        {
            var element = new CustomElement<T>();
            OnAddElement(element);
            return element;
        }

        /// <summary>
        /// Adds new custom elements container.
        /// </summary>
        /// <typeparam name="T">The custom control.</typeparam>
        /// <returns>The created element.</returns>
        public CustomElementsContainer<T> CustomContainer<T>()
            where T : ContainerControl, new()
        {
            var element = new CustomElementsContainer<T>();
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
        /// Adds new enum value element.
        /// </summary>
        /// <param name="type">The enum type.</param>
        /// <param name="cusstomBuildEntriesDelegate">The custom entries layout builder. Allows to hide existing or add diffrent enum values to editor.</param>
        /// <returns>The created element.</returns>
        public EnumElement Enum(Type type, EnumElement.BuildEntriesDelegate cusstomBuildEntriesDelegate = null)
        {
            EnumElement element = new EnumElement(type, cusstomBuildEntriesDelegate);
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
            editor.Initialize(CustomEditor.CurrentCustomEditor.Presenter, this, values);

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
                var group = Group(name, true);
                group.Panel.Close(false);
                return group.Object(member, values, editor);
            }

            var element = AddPropertyItem(name);
            return element.Object(member, values, editor);
        }

        /// <summary>
        /// Adds the <see cref="PropertiesListElement"/> to the current layou or reuses the previous one. Used to inject properties.
        /// </summary>
        /// <returns>The element.</returns>
        protected PropertiesListElement AddPropertyItem(string name)
        {
            // Try to reuse previous control
            PropertiesListElement element;
            if (Children.Count > 0 && Children[Children.Count - 1] is PropertiesListElement propertiesListElement)
            {
                element = propertiesListElement;
            }
            else
            {
                element = new PropertiesListElement();
                OnAddElement(element);
            }

            element.OnAddProperty(name);

            return element;
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
