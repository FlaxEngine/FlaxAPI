////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System.Collections.Generic;
using FlaxEditor.Content;
using FlaxEngine.GUI;

namespace FlaxEditor.Windows
{
    public partial class ContentWindow
    {
        private void treeOnSelectedChanged(List<TreeNode> from, List<TreeNode> to)
        {
            // Navigate
            var source = from.Count > 0 ? from[0] as ContentTreeNode : null;
            var target = to.Count > 0 ? to[0] as ContentTreeNode : null;
            navigate(source, target);
        }

        /// <summary>
        /// Navigates to the specified target content location.
        /// </summary>
        /// <param name="target">The target.</param>
        public void Navigate(ContentTreeNode target)
        {
            navigate(SelectedNode, target);
        }

        private void navigate(ContentTreeNode source, ContentTreeNode target)
        {
            // TODO: finish this
            /*// Check if can do this action
            if (target != null && _navigationUnlocked && source != target)
            {
                // Lock navigation
                _navigationUnlocked = false;

                // Check if already added to the Undo on the top
                if (source != null && (_navigationUndo.Count == 0 || _navigationUndo[0] != source))
                {
                    // Add to Undo list
                    _navigationUndo.Insert(0, source);
                }

                // Show folder contents and select tree node
                if (target == _root)
                {
                    // Special case for root folder
                    List<ContentItem> elements = new List<ContentItem>(8);
                    for (int i = 0; i < _root.ChildrenCount; i++)
                    {
                        if (_root.GetChild(i) is ContentTreeNode node)
                        {
                            elements.Add(node.Folder);
                        }
                    }
                    _view.ShowElements(elements);
                }
                else
                {
                    // Show folder contents
                    _view.ShowElements(target.Folder.Children);
                }
                _tree.Select(target);
                target.ExpandAllParents();

                // Clear redo list
                _navigationRedo.Clear();

                // Set valid sizes for stacks
                //RedoList.SetSize(32);
                //UndoList.SetSize(32);

                // Unlcok navigation
                _navigationUnlocked = true;

                // Update UI
                updateToolstrip();
            }

            _view.Focus();*/
        }

        /// <summary>
        /// Navigates backward.
        /// </summary>
        public void NavigateBackward()
        {
            // TODO: finish this
            /*// Check if navigation is unlocked
            if (_navigationUnlocked && _navigationUndo.Count > 0)
            {
                // Pop node
                ContentTreeNode node = _navigationUndo[0];
                _navigationUndo.RemoveAtKeepOrder(0);

                // Lock navigation
                _navigationUnlocked = false;

                // Add to Redo list
                _navigationRedo.Insert(0, SelectedNode);

                // Select node
                _view.ShowElements(node.Folder.Children);
                _tree.Select(node);
                node.ExpandAllParents();

                // Set valid sizes for stacks
                //RedoList.SetSize(32);
                //UndoList.SetSize(32);

                // Clear search form and update view
                //ClearSearch();

                // Unlcok navigation
                _navigationUnlocked = true;

                // Update UI
                updateToolstrip();

                // TODO: select that folder to use arrows to navigate easly
            }

            _view.Focus();*/
        }

        /// <summary>
        /// Navigates forward.
        /// </summary>
        public void NavigateForward()
        {
            // TODO: finish this
            /*// Check if navigation is unlocked
            if (_navigationUnlocked && _navigationRedo.Count > 0)
            {
                // Pop node
                ContentTreeNode node = _navigationRedo[0];
                _navigationRedo.RemoveAtKeepOrder(0);

                // Lock navigation
                _navigationUnlocked = false;

                // Add to Undo list
                _navigationUndo.Insert(0, SelectedNode);

                // Select node
                _view.ShowElements(node.Folder.Children);
                _tree.Select(node);
                node.ExpandAllParents();

                // Set valid sizes for stacks
                //RedoList.SetSize(32);
                //UndoList.SetSize(32);

                // Clear search form and update view
                //ClearSearch();

                // Unlcok navigation
                _navigationUnlocked = true;

                // Update UI
                updateToolstrip();
            }

            _view.Focus();*/
        }

        /// <summary>
        /// Navigates directory up.
        /// </summary>
        public void NavigateUp()
        {
            ContentTreeNode target = _root;
            ContentTreeNode current = SelectedNode;

            if (current?.Folder.ParentFolder != null)
            {
                target = current.Folder.ParentFolder.Node;
            }

            Navigate(target);
        }

        /// <summary>
        /// Clears the navigation history.
        /// </summary>
        public void NavigationClearHistory()
        {
            _navigationUndo.Clear();
            _navigationRedo.Clear();
            updateToolstrip();
        }

        private void navigationBarUpdate()
        {
            // TODO: finish this
            // Remove previous buttons
            /*_navigationBar.DeleteChildren();

            // Spawn buttons
            Array<ContentTreeNode*> nodes(8);
            ContentTreeNode* node = GetSelectedNode();
            while (node != _root && node)
            {
                nodes.Add(node);
                node = node.GetParentNode();
            }
            float x = 1;
            float h = _toolStrip.GetItemsHeight();
            for (int32 i = nodes.Count() - 1; i >= 0; i--)
            {
                var button = new CContentNavigationButton(h, nodes[i], x, CToolStrip_MarginV);
                button.PerformLayout();
                x += button.GetWidth() + 2;
                _navigationBar.AddControl(button);
            }

            // Update
            _navigationBar.PerformLayout();*/
        }

        /// <summary>
        /// Gets the selected tree node.
        /// </summary>
        /// <value>
        /// The selected node.
        /// </value>
        public ContentTreeNode SelectedNode => _tree.SelectedNode as ContentTreeNode;

        /// <summary>
        /// Gets the current view folder.
        /// </summary>
        /// <value>
        /// The current view folder.
        /// </value>
        public ContentFolder CurrentViewFolder
        {
            get
            {
                var node = SelectedNode;
                return node?.Folder;
            }
        }

        /// <summary>
        /// Shows the root folder.
        /// </summary>
        public void ShowRoot()
        {
            // Show root folder
            _tree.Select(_root);
        }
    }
}
