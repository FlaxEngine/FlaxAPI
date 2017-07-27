////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System;

namespace FlaxEngine
{
    // TODO expand with insert after and before

    /// <summary>
    /// Allows to declare order of the item in the editor.
    /// </summary>
    /// <seealso cref="System.Attribute" />
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Delegate | AttributeTargets.Event | AttributeTargets.Method)]
    public sealed class EditorIndexAttribute : Attribute
    {
        /// <summary>
        /// Requested index to perform layout on. Used to order the items.
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        /// Override display index in visual tree for provided model.
        /// </summary>
        /// <remarks>
        /// Current index is resolved runtime, and can change if custom editor class has changed.
        /// </remarks>
        /// <param name="index">The order index.</param>
        public EditorIndexAttribute(int index)
        {
            Index = index;
        }
    }
}
