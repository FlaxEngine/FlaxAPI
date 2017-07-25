////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System.Collections.Generic;
using FlaxEngine.GUI;

namespace FlaxEditor.CustomEditors
{
    /// <summary>
    /// Represents single element of the Custom Editor layout.
    /// </summary>
    public abstract class LayoutElement
    {
        /// <summary>
        /// Gets the control represented by this element.
        /// </summary>
        /// <value>
        /// The control.
        /// </value>
        public abstract Control Control { get; }

        /// <summary>
        /// Builds the layout.
        /// </summary>
        public virtual void BuildLayout()
        {
        }

        /// <summary>
        /// Updates the specified element with selected objects values.
        /// </summary>
        /// <param name="selection">The selection.</param>
        public virtual void Update(List<object> selection)
        {
        }

        /// <summary>
        /// Releases resources.
        /// </summary>
        public virtual void Dispose()
        {
            Control.Dispose();
        }
    }
}
