////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System.Collections.Generic;
using FlaxEditor.Content;
using FlaxEditor.Content.GUI;
using FlaxEditor.GUI;
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
            // Check if can do this action
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
                    List<ContentItem> items = new List<ContentItem>(8);
                    for (int i = 0; i < _root.ChildrenCount; i++)
                    {
                        if (_root.GetChild(i) is ContentTreeNode node)
                        {
                            items.Add(node.Folder);
                        }
                    }
                    _view.ShowItems(items);
                }
                else
                {
                    // Show folder contents
                    _view.ShowItems(target.Folder.Children);
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
                UpdateUI();
            }

            _view.Focus();
        }

        /// <summary>
        /// Navigates backward.
        /// </summary>
        public void NavigateBackward()
        {
            // Check if navigation is unlocked
            if (_navigationUnlocked && _navigationUndo.Count > 0)
            {
                // Pop node
                ContentTreeNode node = _navigationUndo[0];
                _navigationUndo.RemoveAt(0);

                // Lock navigation
                _navigationUnlocked = false;

                // Add to Redo list
                _navigationRedo.Insert(0, SelectedNode);

                // Select node
                _view.ShowItems(node.Folder.Children);
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
                UpdateUI();

                // TODO: select that folder to use arrows to navigate easly
            }

            _view.Focus();
        }

        /// <summary>
        /// Navigates forward.
        /// </summary>
        public void NavigateForward()
        {
            // Check if navigation is unlocked
            if (_navigationUnlocked && _navigationRedo.Count > 0)
            {
                // Pop node
                ContentTreeNode node = _navigationRedo[0];
                _navigationRedo.RemoveAt(0);

                // Lock navigation
                _navigationUnlocked = false;

                // Add to Undo list
                _navigationUndo.Insert(0, SelectedNode);

                // Select node
                _view.ShowItems(node.Folder.Children);
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
                UpdateUI();
            }

            _view.Focus();
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
            UpdateUI();
        }

        private void UpdateNavigationBar()
        {
            if (_navigationBar == null)
                return;

            // Remove previous buttons
            _navigationBar.DisposeChildren();

            // Spawn buttons
            List<ContentTreeNode> nodes = new List<ContentTreeNode>(8);
            ContentTreeNode node = SelectedNode;
            while (node != _root && node != null)
            {
                nodes.Add(node);
                node = node.ParentNode;
            }
            float x = 1;
            float h = _toolStrip.ItemsHeight;
            for (int i = nodes.Count - 1; i >= 0; i--)
            {
                var button = new NavigationButton(nodes[i], x, ToolStrip.DefaultMarginV, h);
                button.PerformLayout();
                x += button.Width + 2;
                _navigationBar.AddChild(button);
            }

            // Update
            _navigationBar.PerformLayout();
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