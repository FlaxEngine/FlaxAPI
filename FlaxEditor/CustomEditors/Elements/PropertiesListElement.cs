////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System.Collections.Generic;
using FlaxEngine.GUI;

namespace FlaxEditor.CustomEditors.Elements
{
    /// <summary>
    /// <see cref="CustomEditor"/> properties list element.
    /// </summary>
    /// <seealso cref="LayoutElementsContainer"/>
    public class PropertiesListElement : LayoutElementsContainer
    {
        /// <summary>
        /// <see cref="CustomEditor"/> properties list control.
        /// </summary>
        /// <seealso cref="FlaxEngine.GUI.VerticalPanel" />
        public class PropertiesList : VerticalPanel
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="PropertiesList"/> class.
            /// </summary>
            public PropertiesList()
            {
                LeftMargin = 50;
            }

            // TODO: change leftMargin using mouse
            // TODO: draw splitter
            // TODO: sync splitter for whole presenter
            // TODO: draw editors names
            // TODO: show editor tooltips
            // TODO: if name is too long to show in leftMargin space -> use tooltip to show it
        }

        /// <summary>
        /// The child editors added to this elements container.
        /// </summary>
        protected readonly List<CustomEditor> _editors = new List<CustomEditor>();

        /// <summary>
        /// The list.
        /// </summary>
        public readonly PropertiesList Properties = new PropertiesList();

        /// <inheritdoc />
        public override ContainerControl ContainerControl => Properties;

        /// <inheritdoc />
        protected override void OnAddEditor(CustomEditor editor)
        {
            base.OnAddEditor(editor);

            _editors.Add(editor);
        }
    }
}
