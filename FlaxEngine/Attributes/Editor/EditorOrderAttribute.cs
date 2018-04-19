// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System;

namespace FlaxEngine
{
    // TODO expand with insert after and before

    /// <summary>
    /// Allows to declare order of the item in the editor.
    /// </summary>
    /// <seealso cref="System.Attribute" />
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Delegate | AttributeTargets.Event | AttributeTargets.Method)]
    public sealed class EditorOrderAttribute : Attribute
    {
        /// <summary>
        /// Requested order to perform layout on. Used to order the items.
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        /// Override display order in visual tree for provided model.
        /// </summary>
        /// <remarks>
        /// Current order is resolved runtime, and can change if custom editor class has changed.
        /// </remarks>
        /// <param name="order">The order order.</param>
        public EditorOrderAttribute(int order)
        {
            Order = order;
        }
    }
}
