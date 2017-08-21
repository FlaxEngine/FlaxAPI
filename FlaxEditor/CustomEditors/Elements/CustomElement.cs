////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using FlaxEngine.GUI;

namespace FlaxEditor.CustomEditors.Elements
{
    /// <summary>
    /// The custom layout element.
    /// </summary>
    /// <seealso cref="FlaxEditor.CustomEditors.LayoutElementsContainer" />
    public class CustomElement<T> : LayoutElementsContainer
        where T : ContainerControl, new()
    {
        /// <summary>
        /// The custom control.
        /// </summary>
        public readonly T Custom = new T();

        /// <inheritdoc />
        public override ContainerControl ContainerControl => Custom;
    }
}
