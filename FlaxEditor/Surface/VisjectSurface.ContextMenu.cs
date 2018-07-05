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

            //Smart(tm) connecting
            var bothBoxes = Selection
                .OrderBy(s => s.Top)
                .SelectMany(s => s.GetBoxes())
                .Where(box => box.IsOutput)
                .Zip(
                    node.GetBoxes()
                    .Where(box => !box.IsOutput),
                    (a, b) => new { a, b }); //TODO: refactor to use tuples

            foreach (var dualBox in bothBoxes)
            {
                dualBox.a.CreateConnection(dualBox.b);
                Deselect(dualBox.a.ParentNode);
            }

            AddToSelection(node);
        }
    }
}
