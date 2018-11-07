// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System;
using System.Collections.Generic;
using FlaxEditor.Surface.Elements;
using FlaxEngine.GUI;

namespace FlaxEditor.Surface.ContextMenu
{
    /// <summary>
    /// Drop panel for group of <see cref="VisjectCMItem"/>. It represents <see cref="GroupArchetype"/>.
    /// </summary>
    /// <seealso cref="FlaxEngine.GUI.DropPanel" />
    public sealed class VisjectCMGroup : DropPanel
    {
        /// <summary>
        /// The context menu.
        /// </summary>
        public readonly VisjectCM ContextMenu;

        /// <summary>
        /// The archetype.
        /// </summary>
        public readonly GroupArchetype Archetype;

        // A bit of a hack to make sure that the default group order is preserved
        internal int DefaultIndex;

        /// <summary>
        /// A computed score for the context menu order
        /// </summary>
        public float SortScore { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="VisjectCMGroup"/> class.
        /// </summary>
        /// <param name="cm">The context menu.</param>
        /// <param name="archetype">The group archetype.</param>
        public VisjectCMGroup(VisjectCM cm, GroupArchetype archetype)
        {
            HeaderText = archetype.Name;
            ContextMenu = cm;
            Archetype = archetype;
        }

        /// <summary>
        /// Resets the view.
        /// </summary>
        public void ResetView()
        {
            SortScore = 0;
            // Remove filter
            for (int i = 0; i < _children.Count; i++)
            {
                if (_children[i] is VisjectCMItem item)
                {
                    item.UpdateFilter(null);
                    item.UpdateScore(null);
                }
            }
            SortChildren();
            if (ContextMenu.ShowExpanded)
                Open(false);
            else
                Close(false);
            Visible = true;
        }

        /// <summary>
        /// Updates the filter.
        /// </summary>
        /// <param name="filterText">The filter text.</param>
        public void UpdateFilter(string filterText)
        {
            // Update items
            bool isAnyVisible = false;
            for (int i = 0; i < _children.Count; i++)
            {
                if (_children[i] is VisjectCMItem item)
                {
                    item.UpdateFilter(filterText);
                    isAnyVisible |= item.Visible;
                }
            }

            // Update itself
            if (isAnyVisible)
            {
                Open(false);
                Visible = true;
            }
            else
            {
                // Hide group if none of the items matched the filter
                Visible = false;
            }
        }

        /// <summary>
        /// Updates the sorting of the <see cref="VisjectCMItem"/>s of this <see cref="VisjectCMGroup"/>
        /// Also updates the <see cref="SortScore"/>
        /// </summary>
        /// <param name="selectedBox">The currently user-selected box</param>
        public void UpdateItemSort(Box selectedBox)
        {
            SortScore = 0;
            for (int i = 0; i < _children.Count; i++)
            {
                if (_children[i] is VisjectCMItem item)
                {
                    item.UpdateScore(selectedBox);

                    if (item.SortScore > SortScore)
                    {
                        SortScore = item.SortScore;
                    }
                }
            }

            if (selectedBox == null)
            {
                SortScore = 0;
            }

            SortChildren();
        }

        /// <inheritdoc/>
        public override int Compare(Control other)
        {
            if (other is VisjectCMGroup otherGroup)
            {
                int order = -1 * SortScore.CompareTo(otherGroup.SortScore);
                if (order == 0)
                {
                    order = DefaultIndex.CompareTo(otherGroup.DefaultIndex);
                }
                return order;
            }
            return base.Compare(other);
        }
    }
}
