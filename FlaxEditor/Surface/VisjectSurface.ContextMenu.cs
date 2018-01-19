////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2018 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using FlaxEditor.Surface.ContextMenu;
using FlaxEditor.Surface.Elements;
using FlaxEngine;
using FlaxEngine.GUI;

namespace FlaxEditor.Surface
{
    public partial class VisjectSurface
    {
	    private ContextMenuButton _cmDeleteNodeButton;
	    private ContextMenuButton _cmRemoveBoxConnectionsButton;

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
        public void ShowSecondaryCM(SurfaceNode node, Vector2 location)
        {
            // Select that node
            Select(node);

            // Update context menu buttons
	        _cmDeleteNodeButton.Enabled = (node.Archetype.Flags & NodeFlags.NoRemove) == 0;
            //
            var boxUnderMouse = GetChildAtRecursive(location) as Box;
	        _cmRemoveBoxConnectionsButton.Enabled = boxUnderMouse != null && boxUnderMouse.HasAnyConnection;
	        _cmRemoveBoxConnectionsButton.Tag = boxUnderMouse;

            // Show secondary context menu
            _cmStartPos = location;
            _cmSecondaryMenu.Tag = node;
            _cmSecondaryMenu.Show(this, location);
        }

        private void OnPrimaryMenuButtonClick(VisjectCMItem visjectCmItem)
        {
            SpawnNode(visjectCmItem.GroupArchetype, visjectCmItem.NodeArchetype, _surface.PointFromParent(_cmStartPos));
        }
    }
}
