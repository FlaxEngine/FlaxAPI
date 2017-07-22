////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System;
using FlaxEditor.Surface.ContextMenu;
using FlaxEditor.Surface.Elements;
using FlaxEngine;

namespace FlaxEditor.Surface
{
    public partial class VisjectSurface
    {
        /// <summary>
        /// Shows the primary menu.
        /// </summary>
        /// <param name="location">The location in teh Surface Space.</param>
        public void ShowPrimaryMenu(Vector2 location)
        {
            _cmPrimaryMenu.Show(this, location);
        }

        /// <summary>
        /// Shows the secondary context menu for the given node.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="location">The location in the Surface Space.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public void ShowSecondaryCM(SurfaceNode node, Vector2 location)
        {
            // Select that node
            Select(node);

            // Update context menu buttons
            var deleteNodeButton = _cmSecondaryMenu.GetButton(2);
            deleteNodeButton.Enabled = (node.Archetype.Flags & NodeFlags.NoRemove) == 0;
            //
            var removeBoxConnectionsButton = _cmSecondaryMenu.GetButton(4);
            var boxUnderMouse = GetChildAtRecursive(location) as Box;
            removeBoxConnectionsButton.Enabled = boxUnderMouse != null && boxUnderMouse.HasAnyConnection;
            removeBoxConnectionsButton.Tag = boxUnderMouse;

            // Show secondary context menu
            _cmStartPos = location;
            _cmSecondaryMenu.Tag = node;
            _cmSecondaryMenu.Show(this, location);
        }

        private void OnPrimaryMenuButtonClick(VisjectCMItem visjectCmItem)
        {
            SpawnNode(visjectCmItem.GroupArchetype, visjectCmItem.NodeArchetype, _surface.PointFromParent(_cmStartPos));
        }

        private void OnSecondaryMenuButtonClick(int id, FlaxEngine.GUI.ContextMenu contextMenu)
        {
            var nodeUnderMouse = (SurfaceNode)contextMenu.Tag;
            switch (id)
            {
                case 1:
                    Owner.OnSurfaceSave();
                    break;
                case 2:
                    Delete(nodeUnderMouse);
                    break;
                case 3:
                    nodeUnderMouse.RemoveConnections();
                    MarkAsEdited();
                    break;
                case 4:
                {
                    var boxUnderMouse = (Box)_cmSecondaryMenu.GetButton(4).Tag;
                    boxUnderMouse.RemoveConnections();
                    MarkAsEdited();
                    break;
                }
            }
        }
    }
}
