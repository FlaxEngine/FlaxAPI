////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using FlaxEditor.CustomEditors.Elements;
using FlaxEditor.CustomEditors.GUI;
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
        /// Helper flag that is set to true if this container is in root presenter area, otherwise it's one of child groups.
        /// It's used to collapse all chil groups and open the root ones by auto.
        /// </summary>
        internal bool isRootGroup = true;

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
            if (!isRootGroup)
            {
                element.Panel.Close(false);
            }
            element.isRootGroup = false;
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
        /// Adds new custom element with name label.
        /// </summary>
        /// <param name="name">The property name.</param>
        /// <typeparam name="T">The custom control.</typeparam>
        /// <returns>The created element.</returns>
        public CustomElement<T> Custom<T>(string name)
            where T : Control, new()
        {
            var property = AddPropertyItem(name);
            return property.Custom<T>();
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
        /// Adds new custom elements container with name label.
        /// </summary>
        /// <param name="name">The property name.</param>
        /// <typeparam name="T">The custom control.</typeparam>
        /// <returns>The created element.</returns>
        public CustomElementsContainer<T> CustomContainer<T>(string name)
            where T : ContainerControl, new()
        {
            var property = AddPropertyItem(name);
            return property.CustomContainer<T>();
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
        /// Adds new check box element with name label.
        /// </summary>
        /// <param name="name">The property name.</param>
        /// <returns>The created element.</returns>
        public CheckBoxElement Checkbox(string name)
        {
            var property = AddPropertyItem(name);
            return property.Checkbox();
        }

        /// <summary>
        /// Adds new label element.
        /// </summary>
        /// <param name="text">The label text.</param>
        /// <returns>The created element.</returns>
        public LabelElement Label(string text)
        {
            LabelElement element = new LabelElement();
            element.Label.Text = text;
            OnAddElement(element);
            return element;
        }

        /// <summary>
        /// Adds new label element with name label.
        /// </summary>
        /// <param name="name">The property name.</param>
        /// <param name="text">The label text.</param>
        /// <returns>The created element.</returns>
        public LabelElement Label(string name, string text)
        {
            var property = AddPropertyItem(name);
            return property.Label(text);
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
        /// Adds new float value element with name label.
        /// </summary>
        /// <param name="name">The property name.</param>
        /// <returns>The created element.</returns>
        public FloatValueElement FloatValue(string name)
        {
            var property = AddPropertyItem(name);
            return property.FloatValue();
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
        /// Adds new integer value element with name label.
        /// </summary>
        /// <param name="name">The property name.</param>
        /// <returns>The created element.</returns>
        public IntegerValueElement IntegerValue(string name)
        {
            var property = AddPropertyItem(name);
            return property.IntegerValue();
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
        /// Adds new enum value element with name label.
        /// </summary>
        /// <param name="name">The property name.</param>
        /// <param name="type">The enum type.</param>
        /// <param name="cusstomBuildEntriesDelegate">The custom entries layout builder. Allows to hide existing or add diffrent enum values to editor.</param>
        /// <returns>The created element.</returns>
        public EnumElement Enum(string name, Type type, EnumElement.BuildEntriesDelegate cusstomBuildEntriesDelegate = null)
        {
            var property = AddPropertyItem(name);
            return property.Enum(type, cusstomBuildEntriesDelegate);
        }

        /// <summary>
        /// Adds object(s) editor. Selects proper <see cref="CustomEditor"/> based on overrides.
        /// </summary>
        /// <param name="values">The values.</param>
        /// <param name="overrideEditor">The custom editor to use. If null will detect it by auto.</param>
        /// <returns>The created element.</returns>
        public CustomEditor Object(ValueContainer values, CustomEditor overrideEditor = null)
        {
            if(values == null)
                throw new ArgumentNullException();

            var editor = CustomEditorsUtil.CreateEditor(values, overrideEditor);

            OnAddEditor(editor);
            editor.Initialize(CustomEditor.CurrentCustomEditor.Presenter, this, values);

            return editor;
        }

        /// <summary>
        /// Adds object(s) editor with name label. Selects proper <see cref="CustomEditor"/> based on overrides.
        /// </summary>
        /// <param name="name">The property name.</param>
        /// <param name="values">The values.</param>
        /// <param name="overrideEditor">The custom editor to use. If null will detect it by auto.</param>
        /// <returns>The created element.</returns>
        public CustomEditor Object(string name, ValueContainer values, CustomEditor overrideEditor = null)
        {
            var property = AddPropertyItem(name);
            return property.Object(values, overrideEditor);
        }

        /// <summary>
        /// Adds object property editor. Selects proper <see cref="CustomEditor"/> based on overrides.
        /// </summary>
        /// <param name="name">The property name.</param>
        /// <param name="values">The values.</param>
        /// <param name="overrideEditor">The custom editor to use. If null will detect it by auto.</param>
        /// <returns>The created element.</returns>
        public CustomEditor Property(string name, ValueContainer values, CustomEditor overrideEditor = null)
        {
            var editor = CustomEditorsUtil.CreateEditor(values, overrideEditor);
            var style = editor.Style;

            if(style == DisplayStyle.InlineIntoParent)
            {
                return Object(values, editor);
            }
            
            if (style == DisplayStyle.Group)
            {
                var group = Group(name, true);
                group.Panel.Close(false);
                return group.Object(values, editor);
            }

            var property = AddPropertyItem(name);
            return property.Object(values, editor);
        }

        /// <summary>
        /// Adds object property editor. Selects proper <see cref="CustomEditor"/> based on overrides.
        /// </summary>
        /// <param name="label">The property label.</param>
        /// <param name="values">The values.</param>
        /// <param name="overrideEditor">The custom editor to use. If null will detect it by auto.</param>
        /// <returns>The created element.</returns>
        public CustomEditor Property(PropertyNameLabel label, ValueContainer values, CustomEditor overrideEditor = null)
        {
            var editor = CustomEditorsUtil.CreateEditor(values, overrideEditor);
            var style = editor.Style;

            if(style == DisplayStyle.InlineIntoParent)
            {
                return Object(values, editor);
            }
            
            if (style == DisplayStyle.Group)
            {
                var group = Group(label.Name, true);
                group.Panel.Close(false);
                return group.Object(values, editor);
            }

            var property = AddPropertyItem(label);
            return property.Object(values, editor);
        }

        private PropertiesListElement AddPropertyItem()
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
            return element;
        }

        /// <summary>
        /// Adds the <see cref="PropertiesListElement"/> to the current layou or reuses the previous one. Used to inject properties.
        /// </summary>
        /// <param name="name">The property label name.</param>
        /// <returns>The element.</returns>
        public PropertiesListElement AddPropertyItem(string name)
        {
            PropertiesListElement element = AddPropertyItem();
            element.OnAddProperty(name);
            return element;
        }

        /// <summary>
        /// Adds the <see cref="PropertiesListElement"/> to the current layou or reuses the previous one. Used to inject properties.
        /// </summary>
        /// <param name="label">The property label.</param>
        /// <returns>The element.</returns>
        public PropertiesListElement AddPropertyItem(PropertyNameLabel label)
        {
            if(label == null)
                throw new ArgumentNullException();

            PropertiesListElement element = AddPropertyItem();
            element.OnAddProperty(label);
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

        /// <summary>
        /// Clears the layout.
        /// </summary>
        public virtual void ClearLayout()
        {
            Children.Clear();
        }

        /// <inheritdoc />
        public override Control Control => ContainerControl;
    }
}
