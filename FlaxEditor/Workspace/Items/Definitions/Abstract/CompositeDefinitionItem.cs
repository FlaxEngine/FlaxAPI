// ////////////////////////////////////////////////////////////////////////////////////
// // Copyright (c) 2012-2017 Flax Engine. All rights reserved.
// ////////////////////////////////////////////////////////////////////////////////////

using System.Collections.Generic;

namespace FlaxEditor.Workspace.Items.Definitions.Abstract
{
    /// <summary>
    /// Adds additonal funcionalities to <see cref="DefinitionItem" /> that expands it to list like behaviour. Composite
    /// Definition Item can have multiple childs
    /// </summary>
    public abstract class CompositeDefinitionItem : DefinitionItem
    {
        /// <summary>
        /// Initialize new <see cref="CompositeDefinitionItem" /> with list of definiton items and indent of given node
        /// </summary>
        /// <param name="items">List of defined <see cref="DefinitionItem" /> that should be child items</param>
        /// <param name="indent"></param>
        protected CompositeDefinitionItem(IList<DefinitionItem> items, int indent)
        : base(indent)
        {
            ItemsList = items;
            foreach (var definitionItem in items)
            {
                definitionItem.SetParent(this);
            }
        }

        /// <summary>
        /// Enumerable of all <see cref="DefinitionItem" /> currently hold by this <see cref="CompositeDefinitionItem" />
        /// </summary>
        public IEnumerable<DefinitionItem> Items => ItemsList;

        /// <summary>
        /// All <see cref="DefinitionItem" /> currently hold by this <see cref="CompositeDefinitionItem" />
        /// </summary>
        protected IList<DefinitionItem> ItemsList { get; }

        /// <summary>
        /// Add new <see cref="DefinitionItem" /> to the <see cref="ItemsList" />
        /// </summary>
        /// <param name="item">Defined <see cref="DefinitionItem" /> that should be child item</param>
        protected void Add(DefinitionItem item)
        {
            ItemsList.Add(item);
            item.SetParent(this);
        }

        /// <summary>
        /// Remove existing <see cref="DefinitionItem" /> from the <see cref="ItemsList" />
        /// </summary>
        /// <param name="item">Defined <see cref="DefinitionItem" /> that should be removed</param>
        protected void Remove(DefinitionItem item)
        {
            ItemsList.Remove(item);
        }

        /// <summary>
        /// Insert before an existing <see cref="DefinitionItem" /> from to the <see cref="ItemsList" />
        /// </summary>
        /// <param name="item">Defined <see cref="DefinitionItem" /> that should be child item</param>
        protected void InsertBefore(DefinitionItem item, DefinitionItem anchor)
        {
            ItemsList.Insert(ItemsList.IndexOf(anchor), item);
            item.SetParent(this);
        }

        /// <summary>
        /// Insert after an existing <see cref="DefinitionItem" /> from to the <see cref="ItemsList" />
        /// </summary>
        /// <param name="item">Defined <see cref="DefinitionItem" /> that should be child item</param>
        protected void InsertAfter(DefinitionItem item, DefinitionItem anchor)
        {
            ItemsList.Insert(ItemsList.IndexOf(anchor) + 1, item);
            item.SetParent(this);
        }
    }
}
