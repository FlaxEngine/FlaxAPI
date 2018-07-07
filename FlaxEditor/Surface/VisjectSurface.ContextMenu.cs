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
        /// Shows the primary menu.
        /// </summary>
        /// <param name="location">The location in the Surface Space.</param>
        public void ShowPrimaryMenu(Vector2 location)
        {
            _cmPrimaryMenu.Show(this, location);
        }

        /// <summary>
        /// Shows the secondary context menu.
        /// </summary>
        /// <param name="location">The location in the Surface Space.</param>
        public void ShowSecondaryCM(Vector2 location)
        {
            var selection = Selection;

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

        private void OnPrimaryMenuButtonClick(VisjectCMItem visjectCmItem)
        {
            var node = SpawnNode(visjectCmItem.GroupArchetype, visjectCmItem.NodeArchetype, _surface.PointFromParent(_cmStartPos));
            //if (node.GetBoxes().ConvertAll(b => b.IsOutput).Aggregate((a, b) => a && b) && HasSelection) //TODO: No way! You're not getting away with this ***
            // {
            //    node.Location += new Vector2(-node.Width - 40, 90 * Selection.Count);
            //}

            //TODO: Refactor this 

            var toBeDeselected = new System.Collections.Generic.List<SurfaceNode>();

            var outputBoxes = Selection
                                    .OrderBy(n => n.Top)
                                    .SelectMany(n =>
                                    {
                                        if (n.GroupArchetype.Name == "Constants") //TODO: No to hardcoding!
                                        {
                                            return Enumerable.Repeat(n.GetBoxes().First(), 1);
                                        }
                                        else
                                        {
                                            return n.GetBoxes();
                                        }
                                    })
                                    .Where(b => b.IsOutput && !b.HasAnyConnection);


            //I'm assuming that they are sorted properly
            using (var inputBoxes = node
                                    .GetBoxes()
                                    .Where(box => !box.IsOutput)
                                    .GetEnumerator())
            {
                inputBoxes.MoveNext();
                foreach (var outputBox in outputBoxes)
                {
                    if (inputBoxes.Current == null) break;

                    //If the type matches
                    if ((inputBoxes.Current.CurrentType & outputBox.CurrentType) != 0)
                    {
                        //Connect them
                        inputBoxes.Current.CreateConnection(outputBox);
                        toBeDeselected.Add(outputBox.ParentNode);
                        if (!inputBoxes.MoveNext()) break;
                    }

                    else
                    {
                        bool hasAlternatives = outputBox
                                                              .ParentNode
                                                              .GetBoxes()
                                                              .Where(b => b.IsOutput && !b.HasAnyConnection)
                                                              .Skip(1).Count() > 0;
                        //If the box has some alternatives
                        if (outputBox.ParentNode.GroupArchetype.Name == "Constants" && hasAlternatives)
                        {
                            foreach (var alternativeBox in outputBox
                                                              .ParentNode
                                                              .GetBoxes()
                                                              .Where(b => b.IsOutput && !b.HasAnyConnection)
                                                              .Skip(1))
                            {
                                if ((inputBoxes.Current.CurrentType & alternativeBox.CurrentType) != 0)
                                {
                                    inputBoxes.Current.CreateConnection(alternativeBox);
                                    toBeDeselected.Add(alternativeBox.ParentNode);
                                    if (!inputBoxes.MoveNext()) break;
                                }
                            }
                        }
                        //Whatever
                        else if (outputBox.CanUseType(inputBoxes.Current.CurrentType))
                        {
                            inputBoxes.Current.CreateConnection(outputBox);
                            toBeDeselected.Add(outputBox.ParentNode);
                            if (!inputBoxes.MoveNext()) break;
                        }
                        else
                        {
                            //Do nothing
                        }
                    }
                }
            }
            foreach (var toDeselect in toBeDeselected)
            {
                Deselect(toDeselect);
            }

            AddToSelection(node);
        }
    }
}
