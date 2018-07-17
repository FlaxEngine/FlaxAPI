// ////////////////////////////////////////////////////////////////////////////////////
// // Copyright (c) 2012-2017 Flax Engine. All rights reserved.
// ////////////////////////////////////////////////////////////////////////////////////

using System;

namespace FlaxEditor.Workspace.Items.Definitions.Abstract
{
    /// <summary>
    /// Creates definition a base definition for a single item in workspace hierarchy
    /// </summary>
    public abstract class DefinitionItem
    {
        /// <summary>
        /// Indent of defined item
        /// </summary>
        public int Indent;

        /// <summary>
        /// Generate new item with nulll parent and set item indent
        /// </summary>
        /// <param name="indent"></param>
        protected DefinitionItem(int indent)
        {
            Indent = indent;
        }

        /// <summary>
        /// Parent of defined item
        /// </summary>
        protected DefinitionItem Parent { get; private set; }

        /// <summary>
        /// Set parent for this definition item
        /// </summary>
        /// <param name="parent"></param>
        internal void SetParent(DefinitionItem parent)
        {
            if (Parent != null)
            {
                throw new InvalidOperationException("Parent already assigned");
            }

            Parent = parent;
        }

        /// <summary>
        /// Visit this item using provided visitor
        /// </summary>
        /// <param name="visitor">Implementation of visitor used to visit all nodes within structure</param>
        public abstract void Visit(DefinitionItemVisitor visitor);
    }
}
