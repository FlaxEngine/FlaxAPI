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

        internal int DefaultIndex;

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
            // Remove filter
            for (int i = 0; i < _children.Count; i++)
            {
                if (_children[i] is VisjectCMItem item)
                    item.UpdateFilter(null);
            }

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

        public float SortChildrenWithStartBox(Box startBox)
        {
            if (startBox == null)
            {
                SortChildren();
                return -1;
            }

            // Calculate the scores
            Tuple<float, Control>[] scores = CalculateScores(_children, startBox);

            Array.Sort(scores, (a, b) =>
            {
                // Sort by score (highest to lowest)
                int sortValue = -1 * a.Item1.CompareTo(b.Item1);
                if (sortValue == 0)
                {
                    // Otherwise, sort them the usual way
                    sortValue = a.Item2.CompareTo(b.Item2);
                }
                return sortValue;
            });

            for (int i = 0; i < _children.Count; i++)
            {
                if (scores[i].Item2 is VisjectCMItem item)
                {
                    item.Starred = scores[i].Item1 > 0;
                }
                _children[i] = scores[i].Item2;
            }
            PerformLayout();

            return scores[0].Item1; // Return the highest score
        }

        private Tuple<float, Control>[] CalculateScores(List<Control> children, Box startBox)
        {
            Tuple<float, Control>[] score = new Tuple<float, Control>[children.Count];

            for (int i = 0; i < children.Count; i++)
            {
                score[i] = new Tuple<float, Control>(0, children[i]);
                if (startBox != null)
                {
                    if (children[i] is VisjectCMItem item)
                    {
                        if (CanConnectTo(startBox, item.NodeArchetype))
                        {
                            score[i] = new Tuple<float, Control>(1, children[i]);
                        }
                    }
                }
            }

            return score;
        }

        private bool CanConnectTo(Box startBox, NodeArchetype nodeArchetype)
        {
            for (int i = 0; i < nodeArchetype.Elements.Length; i++)
            {
                if (nodeArchetype.Elements[i].Type == NodeElementType.Input &&
                    //(startBox.CurrentType & nodeArchetype.Elements[i].ConnectionsType) != 0))
                    startBox.CanUseType(nodeArchetype.Elements[i].ConnectionsType))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
