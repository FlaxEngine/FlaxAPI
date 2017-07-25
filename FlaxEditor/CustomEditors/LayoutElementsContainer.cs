////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System.Collections.Generic;
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
        private readonly Queue<LayoutElement> toReuse = new Queue<LayoutElement>();

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
        
        private void RemoveReusable()
        {
            while (toReuse.Count > 0)
            {
                var c = toReuse.Dequeue();
                c.Dispose();
            }
        }

        /// <summary>
        /// Adds new group element.
        /// </summary>
        /// <param name="title">The title.</param>
        /// <returns>The created element.</returns>
        public GroupElement Group(string title)
        {
            // Try to resuse element or create new one
            GroupElement element;
            if (toReuse.Count > 0 && toReuse.Dequeue() is GroupElement e)
            {
                element = e;
            }
            else
            {
                RemoveReusable();
                element = new GroupElement();
            }

            // Link it
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
            // Try to resuse element or create new one
            ButtonElement element;
            if (toReuse.Count > 0 && toReuse.Dequeue() is ButtonElement e)
            {
                element = e;
            }
            else
            {
                RemoveReusable();
                element = new ButtonElement();
            }

            // Link it
            element.Init(title);
            element.Control.Parent = ContainerControl;
            Children.Add(element);
            return element;
        }

        /// <inheritdoc />
        public override Control Control => ContainerControl;

        /// <inheritdoc />
        public override void BuildLayout()
        {
            for (int i = 0; i < Children.Count; i++)
            {
                var child = Children[i];
                toReuse.Enqueue(child);
                child.Control.Parent = null;
            }
            Children.Clear();

            // TODO: create elements here
            base.BuildLayout();

            RemoveReusable();
        }

        /// <inheritdoc />
        public override void Update(List<object> selection)
        {
            base.Update(selection);

            for (int i = 0; i < Children.Count; i++)
            {
                Children[i].Update(selection);
            }
        }

        /// <inheritdoc />
        public override void Dispose()
        {
            RemoveReusable();

            base.Dispose();
        }
    }
}
