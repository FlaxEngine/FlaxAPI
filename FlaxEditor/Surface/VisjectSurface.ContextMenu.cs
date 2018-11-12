// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System.Linq;
using FlaxEditor.Surface.ContextMenu;
using FlaxEditor.Surface.Elements;
using FlaxEngine;
using FlaxEngine.GUI;

namespace FlaxEditor.Surface
{
    public partial class VisjectSurface
    {
        private ContextMenuButton _cmCopyButton;
        private ContextMenuButton _cmPasteButton;
        private ContextMenuButton _cmDuplicateButton;
        private ContextMenuButton _cmCutButton;
        private ContextMenuButton _cmDeleteButton;
        private ContextMenuButton _cmRemoveNodeConnectionsButton;
        private ContextMenuButton _cmRemoveBoxConnectionsButton;

        /// <summary>
        /// Sets the primary menu for the Visject nodes spawning. Can be overriden per surface or surface context. Set to null to restore the default menu.
        /// </summary>
        /// <param name="menu">The menu to override with (use null if restore the default value).</param>
        protected void SetPrimaryMenu(VisjectCM menu)
        {
            menu = menu ?? _cmPrimaryMenu;

            if (menu == _activeVisjectCM)
                return;

            if (_activeVisjectCM != null)
            {
                _activeVisjectCM.OnItemClicked -= OnPrimaryMenuButtonClick;
                _activeVisjectCM.VisibleChanged -= OnPrimaryMenuVisibleChanged;
            }

            _activeVisjectCM = menu;

            if (_activeVisjectCM != null)
            {
                _activeVisjectCM.OnItemClicked += OnPrimaryMenuButtonClick;
                _activeVisjectCM.VisibleChanged += OnPrimaryMenuVisibleChanged;
            }
        }

        /// <summary>
        /// Shows the primary menu.
        /// </summary>
        /// <param name="location">The location in the Surface Space.</param>
        public void ShowPrimaryMenu(Vector2 location)
        {
            // Offset added in case the user doesn't like the box and wants to quickly get rid of it by clicking
            location += new Vector2(5);
            _activeVisjectCM.Show(this, location, _connectionInstigator as Box);
            _cmStartPos = location;
        }

        /// <summary>
        /// Shows the secondary context menu.
        /// </summary>
        /// <param name="location">The location in the Surface Space.</param>
        public void ShowSecondaryCM(Vector2 location)
        {
            var selection = SelectedNodes;
            if (selection.Count == 0)
                return;

            // Update context menu buttons
            _cmPasteButton.Enabled = CanPaste();
            _cmCutButton.Enabled = selection.All(node => (node.Archetype.Flags & NodeFlags.NoRemove) == 0);
            _cmDeleteButton.Enabled = _cmCutButton.Enabled;
            var boxUnderMouse = GetChildAtRecursive(location) as Box;
            _cmRemoveBoxConnectionsButton.Enabled = boxUnderMouse != null && boxUnderMouse.HasAnyConnection;
            _cmRemoveBoxConnectionsButton.Tag = boxUnderMouse;

            // Show secondary context menu
            _cmStartPos = location;
            _cmSecondaryMenu.Tag = selection;
            _cmSecondaryMenu.Show(this, location);
        }

        private void OnPrimaryMenuVisibleChanged(Control primaryMenu)
        {
            if (!primaryMenu.Visible)
            {
                _connectionInstigator = null;
            }
        }

        /// <summary>
        /// Handles Visject CM item click event by spawning the selected item.
        /// </summary>
        /// <param name="visjectCmItem">The item.</param>
        /// <param name="selectedBox">The selected box.</param>
        protected void OnPrimaryMenuButtonClick(VisjectCMItem visjectCmItem, Box selectedBox)
        {
            var node = Context.SpawnNode(
                visjectCmItem.GroupArchetype,
                visjectCmItem.NodeArchetype,
                _rootControl.PointFromParent(ref _cmStartPos),
                visjectCmItem.Data
            );

            // And, if the user is patiently waiting for his box to get connected to the newly created one fulfill his wish!
            if (selectedBox is Box startBox)
            {
                _connectionInstigator = startBox;
                Box alternativeBox = null;
                foreach (var box in node.GetBoxes().Where(box => box.IsOutput != selectedBox.IsOutput))
                {
                    if ((selectedBox.CurrentType & box.CurrentType) != 0)
                    {
                        ConnectingEnd(box);
                        return;
                    }

                    if (alternativeBox == null && selectedBox.CanUseType(box.CurrentType))
                    {
                        alternativeBox = box;
                    }
                }

                if (alternativeBox != null)
                {
                    ConnectingEnd(alternativeBox);
                }
                else
                {
                    ConnectingEnd(null);
                }
            }

            // Disable intelligent connecting for now
            /*
            var toBeDeselected = new System.Collections.Generic.List<SurfaceNode>();

            using (var outputBoxes = Selection
                                     .OrderBy(n => n.Top)
                                     .SelectMany(n => n.GetBoxes())
                                     .Where(b => b.IsOutput && !b.HasAnyConnection)
                                     .GetEnumerator())
            {
                // For each input box (I'm assuming that they are sorted properly)
                foreach (var inputBox in node.GetBoxes().Where(box => !box.IsOutput))
                {
                    Box connectWith = null;

                    // Find the next decent output box and connect them
                    while (connectWith == null && outputBoxes.MoveNext())
                    {
                        var outputBox = outputBoxes.Current;
                        bool connectAnyways = true;

                        // Can I rely on the box indices?
                        // If it's a constant node, it needs some special handling (either connect the first box or the other ones, never both)
                        if (outputBox.ParentNode.GroupArchetype.Name == "Constants")
                        {
                            // Don't always connect this sort of box
                            connectAnyways = false;

                            // If it's the first box, everything is fine?
                            if (outputBox.ID == 0)
                            {
                                // Everything is fine
                                // If this one doesn't have any alternatives, I can just connect it regardless of the consequences
                                if (outputBox.ParentNode.Elements.Count(e => e is Box) <= 1)
                                {
                                    connectAnyways = true;
                                }
                            }
                            // It's an alternative box
                            else
                            {
                                // The first one is already connected => skip this one!
                                if (outputBox.ParentNode.GetBox(0).HasAnyConnection)
                                {
                                    continue;
                                }
                                else
                                {
                                    // It's an actual alternative
                                }
                            }
                        }

                        // If they can easily be connected, just do it ✔
                        if ((inputBox.CurrentType & outputBox.CurrentType) != 0)
                        {
                            connectWith = outputBox;
                        }
                        else if (connectAnyways && inputBox.CanUseType(outputBox.CurrentType))
                        {
                            connectWith = outputBox;
                        }
                    }
                    if (connectWith != null)
                    {
                        // Connect them
                        connectWith.CreateConnection(inputBox);
                        toBeDeselected.Add(connectWith.ParentNode);
                    }
                }
            }

            foreach (var toDeselect in toBeDeselected)
            {
                Deselect(toDeselect);
            }

            AddToSelection(node);
            */
        }
    }
}
