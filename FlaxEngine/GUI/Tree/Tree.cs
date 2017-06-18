////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlaxEngine.GUI
{
    /// <summary>
    /// Tree control.
    /// </summary>
    /// <seealso cref="FlaxEngine.GUI.ContainerControl" />
    public class Tree : ContainerControl
    {
        /// <summary>
        /// The key updates timeout in seconds.
        /// </summary>
        public static float KeyUpdateTimeout = 0.12f;

        /// <summary>
        /// Delegate for selected tree nodes collection change.
        /// </summary>
        /// <param name="before">The before state.</param>
        /// <param name="after">The after state.</param>
        public delegate void SelectionChangedDelegate(List<TreeNode> before, List<TreeNode> after);

        /// <summary>
        /// Delegate for node click events.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="location">The location.</param>
        public delegate void NodeClickDelegate(TreeNode node, Vector2 location);

        private float _keyUpdateTime;
        private bool _supportMultiSelect;
        
        /// <summary>
        /// Action fired when tree nodes selection gets changed.
        /// </summary>
        public event SelectionChangedDelegate OnSelectedChanged;

        /// <summary>
        /// Action fired when ouse goes right click up on node.
        /// </summary>
        public event NodeClickDelegate OnRightClick;

        /// <summary>
        /// List with all selected nodes
        /// </summary>
        public readonly List<TreeNode> Selection = new List<TreeNode>();

        /// <summary>
        /// Gets the first selected node or null.
        /// </summary>
        public TreeNode SelectedNode => Selection.Count > 0 ? Selection[0] : null;

        /// <summary>
        /// Initializes a new instance of the <see cref="Tree"/> class.
        /// </summary>
        /// <param name="supportMultiSelect">True if support multi selection for tree nodes, otherwise false.</param>
        public Tree(bool supportMultiSelect)
            : base(false, 0, 0, 100, 100)
        {
            _supportMultiSelect = supportMultiSelect;
            _keyUpdateTime = KeyUpdateTimeout * 10;
        }

        internal void OnRightClickInternal(TreeNode node, ref Vector2 location)
        {
            OnRightClick?.Invoke(node, location);
        }

        /// <summary>
        /// Selects single tree node.
        /// </summary>
        /// <param name="node">Node to select.</param>
        public void Select(TreeNode node)
        {
            if(node == null)
                throw new ArgumentNullException();

            // Check if won't change
            if (Selection.Count == 1 && SelectedNode == node)
                return;

            // Cache previous state
            var prev = new List<TreeNode>(Selection);

            // Update selection
            Selection.Clear();
            Selection.Add(node);

            // Ensure that node can be visible (all it's parents are expanded)
            node.ExpandAllParents();

            // Fire event
            OnSelectedChanged?.Invoke(prev, Selection);

            Focus();
        }

        /// <summary>
        /// Selects tree nodes.
        /// </summary>
        /// <param name="nodes">Nodes to select.</param>
        public void Select(List<TreeNode> nodes)
        {
            if (nodes == null)
                throw new ArgumentNullException();

            // Check if won't change
            if (Selection.Count == nodes.Count && Selection.SequenceEqual(nodes))
                return;

            // Cache previous state
            var prev = new List<TreeNode>(Selection);

            // Update selection
            Selection.Clear();
            if (_supportMultiSelect)
                Selection.AddRange(nodes);
            else if (nodes.Count > 0)
                Selection.Add(nodes[0]);

            // Ensure that every selected node can be visible (all it's parents are expanded)
            // TODO: maybe use faster tree walk or faster algorythm?
            for (int i = 0; i < Selection.Count; i++)
            {
                Selection[i].ExpandAllParents();
            }

            // Fire event
            OnSelectedChanged?.Invoke(prev, Selection);

            Focus();
        }

        /// <summary>
        /// Adds or removes node to/from the selection
        /// </summary>
        /// <param name="node">The node.</param>
        public void AddOrRemoveSelection(TreeNode node)
        {
            // Cache previous state
            var prev = new List<TreeNode>(Selection);

            // Check if is selected
            int index = Selection.IndexOf(node);
            if (index != -1)
            {
                // Remove
                Selection.RemoveAt(index);
            }
            else
            {
                if (!_supportMultiSelect)
                    Selection.Clear();

                // Add
                Selection.Add(node);
            }

            // Fire event
            OnSelectedChanged?.Invoke(prev, Selection);

            Focus();
        }

        private void walkSelectRangeExpandedTree(List<TreeNode> selection, TreeNode node, ref Rectangle range)
        {
            for (int i = 0; i < node.ChildrenCount; i++)
            {
                if (node.GetChild(i) is TreeNode child)
                {
                    Vector2 pos = node.PointToWindow(Vector2.One);
                    if (range.Contains(pos))
                    {
                        selection.Add(child);
                    }

                    if (child.IsExpanded)
                        walkSelectRangeExpandedTree(selection, child, ref range);
                }
            }
        }

        private Rectangle calcNodeRangeRect(TreeNode node)
        {
            Vector2 pos = node.PointToWindow(Vector2.One);
            return new Rectangle(pos, new Vector2(10000, 4));
        }

        /// <summary>
        /// Selects tree nodes range (used to select part of the tree using Shift+Mouse).
        /// </summary>
        /// <param name="endNode">End range node</param>
        public void SelectRange(TreeNode endNode)
        {
            if (_supportMultiSelect && Selection.Count > 0)
            {
                // Cache previous state
                var prev = new List<TreeNode>(Selection);

                // Update selection
                var selectionRect = calcNodeRangeRect(Selection[0]);
                for (int i = 1; i < Selection.Count; i++)
                {
                    selectionRect = Rectangle.Union(selectionRect, calcNodeRangeRect(Selection[i]));
                }
                var endNodeRect = calcNodeRangeRect(endNode);
                if (endNodeRect.Top - Mathf.Epsilon <= selectionRect.Top)
                {
                    float diff = selectionRect.Top - endNodeRect.Top;
                    selectionRect.Location.Y -= diff;
                    selectionRect.Size.Y += diff;
                }
                else if (endNodeRect.Bottom + Mathf.Epsilon >= selectionRect.Bottom)
                {
                    float diff = endNodeRect.Bottom - selectionRect.Bottom;
                    selectionRect.Size.Y += diff;
                }
                Selection.Clear();
                walkSelectRangeExpandedTree(Selection, _children[0] as TreeNode, ref selectionRect);

                // Check if changed
                if (Selection.Count != prev.Count || !Selection.SequenceEqual(prev))
                {
                    // Fire event
                    OnSelectedChanged?.Invoke(prev, Selection);
                }

                Focus();
            }
            else
            {
                Select(endNode);
            }
        }

        private void walkSelectExpandedTree(List<TreeNode> selection, TreeNode node)
        {
            for (int i = 0; i < node.ChildrenCount; i++)
            {
                if (node.GetChild(i) is TreeNode child)
                {
                    selection.Add(child);
                    if (child.IsExpanded)
                        walkSelectExpandedTree(selection, child);
                }
            }
        }

        /// <summary>
        /// Select all expanded nodes
        /// </summary>
        public void SelectAllExpaned()
        {
            if (_supportMultiSelect)
            {
                // Cache previous state
                var prev = new List<TreeNode>(Selection);

                // Update selection
                Selection.Clear();
                walkSelectExpandedTree(Selection, _children[0] as TreeNode);

                // Check if changed
                if (Selection.Count != prev.Count || !Selection.SequenceEqual(prev))
                {
                    // Fire event
                    OnSelectedChanged?.Invoke(prev, Selection);
                }

                Focus();
            }
        }

        /// <summary>
        /// Updates the tree width.
        /// </summary>
        public void UpdateWidth()
        {
            // Use max of parent clint area width and root node width
            float width = 1;
            if (HasParent)
                width = Parent.GetClientArea().Width;
            for (int i = 0; i < _children.Count; i++)
            {
                if (_children[i] is TreeNode node)
                    width = Mathf.Max(width, node.MinimumWidth);
            }
            Width = width;
        }

        /// <inheritdoc />
        public override void Update(float deltaTime)
        {
            var node = SelectedNode;
            /*
            // TODO: finish input per window
            // Check if has focus and if any node is focused and it isn't a root
            if (ContainsFocus && node != null && !node.IsRoot)
            {
                // Check if can perform update
                if (_keyUpdateTime >= KeyUpdateTimeout)
                {
                    bool keyUpArrow = Input::GetKeyUp(KEY_UP, this);
                    bool keyDownArrow = Input::GetKeyUp(KEY_DOWN, this);

                    // Check if arrow flags are dffrent
                    if (keyDownArrow != keyUpArrow)
                    {
                        var nodeParent = node.Parent;
                        var parentNode = dynamic_cast<CTreeNode*>(nodeParent);
                        var myIndex = nodeParent.GetChildIndex(node);
                        ASSERT(myIndex != INVALID_INDEX);

                        // Up
                        if (keyUpArrow)
                        {
                            TreeNode toSelect = null;
                            if (myIndex == 0)
                            {
                                // Select parent (if it exists and it isn't a root)
                                if (parentNode && !parentNode.IsRoot)
                                    toSelect = parentNode;
                            }
                            else
                            {
                                // Select previous parent child
                                toSelect = dynamic_cast<CTreeNode*>(nodeParent->GetChild(myIndex - 1));

                                // Check if is valid and expanded and has any children
                                if (toSelect && toSelect->IsExpanded() && toSelect->HasChildren())
                                {
                                    // Select last child
                                    toSelect = dynamic_cast<CTreeNode*>(toSelect->GetChild(toSelect->GetChildrenCount() - 1));
                                }
                            }
                            if (toSelect)
                            {
                                // Focus
                                toSelect->Focus();

                                // Select
                                Select(toSelect);
                            }
                        }
                        // Down
                        else
                        {
                            CTreeNode* toSelect = nullptr;
                            if (node->IsExpanded() && node->HasChildren())
                            {
                                // Select first child
                                toSelect = dynamic_cast<CTreeNode*>(node->GetChild(0));
                            }
                            else if (myIndex == nodeParent->GetChildrenCount() - 1)
                            {
                                // Select next node after parent
                                if (parentNode)
                                {
                                    int32 parentIndex = parentNode->GetIndexInParent();
                                    if (parentIndex != INVALID_INDEX && parentIndex < parentNode->GetParent()->GetChildrenCount() - 1)
                                    {
                                        toSelect = dynamic_cast<CTreeNode*>(parentNode->GetParent()->GetChild(parentIndex + 1));
                                    }
                                }
                            }
                            else
                            {
                                // Select next parent child
                                toSelect = dynamic_cast<CTreeNode*>(nodeParent->GetChild(myIndex + 1));
                            }
                            if (toSelect)
                            {
                                // Focus
                                toSelect->Focus();

                                // Select
                                Select(toSelect);
                            }
                        }

                        // Reset time
                        _keyUpdateTime = 0.0f;
                    }
                }
                else
                {
                    // Update time
                    _keyUpdateTime += deltaTime;
                }

                if (Input::GetKeyDown(KEY_RIGHT))
                {
                    // Check if is expanded
                    if (node->IsExpanded())
                    {
                        // Select first child if has
                        if (node->HasChildren())
                            Select((CTreeNode*)node->GetChild(0));
                    }
                    else
                    {
                        // Expand selected node
                        node->Expand();
                    }
                }
                else if (Input::GetKeyDown(KEY_LEFT))
                {
                    if (node->IsCollapsed())
                    {
                        // Select parent if has and is not a root
                        if (node->HasParent() && !((CTreeNode*)node->GetParent())->isRoot())
                            Select((CTreeNode*)node->GetParent());
                    }
                    else
                    {
                        // Collapse selected node
                        node->Collapse();
                    }
                }
            }
            */
            base.Update(deltaTime);
        }

        /// <inheritdoc />
        public override bool OnKeyDown(KeyCode key)
        {
            // Check if can use multi selection
            if (_supportMultiSelect)
            {
                // TODO: finish this
                //throw new NotImplementedException("Tree.OnKeyDown -> Ctrl+A action");
                /*bool isCtrlDown = Input.GetKey(KEY_CONTROL);
                
                // Select all expanded nodes
                if (key == KeyCode.A && isCtrlDown)
                {
                    SelectAllExpaned();
                    return true;
                }*/
            }

            return base.OnKeyDown(key);
        }

        /// <inheritdoc />
        public override void OnGotFocus()
        {
            // Reset timer
            _keyUpdateTime = 0;

            base.OnGotFocus();
        }

        /// <inheritdoc />
        public override void OnChildResized(Control control)
        {
            // Find valid size for the tree
            var rightBottom = Vector2.Zero;
            for (int i = 0; i < _children.Count; i++)
            {
                if (_children[i].Visible)
                {
                    rightBottom = Vector2.Max(rightBottom, _children[i].BottomRight);
                }
            }
            Height = rightBottom.Y;

            base.OnChildResized(control);
        }

        /// <inheritdoc />
        public override void OnParentResized(ref Vector2 oldSize)
        {
            UpdateWidth();

            base.OnParentResized(ref oldSize);
        }

        /// <inheritdoc />
        protected override void PerformLayoutSelf()
        {
            UpdateWidth();
        }
    }
}
