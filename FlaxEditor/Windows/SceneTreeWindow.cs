////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using FlaxEditor.States;
using FlaxEngine;
using FlaxEngine.GUI;

namespace FlaxEditor.Windows
{
    /// <summary>
    /// Windows used to present loaded scenes collection and whole scene graph.
    /// </summary>
    /// <seealso cref="FlaxEditor.Windows.SceneEditorWindow" />
    public class SceneTreeWindow : SceneEditorWindow
    {
        private Tree _tree;
        private RootTreeNode _root;

        /// <summary>
        /// Initializes a new instance of the <see cref="SceneTreeWindow"/> class.
        /// </summary>
        /// <param name="editor">The editor.</param>
        public SceneTreeWindow(Editor editor)
            : base(editor, true, ScrollBars.Both)
        {
            Title = "Scene";
            
            // Create scene structure tree
            _root = new RootTreeNode();
            _root.Expand();
            _tree = new Tree(true);
            _tree.AddChild(_root);
            _tree.OnSelectedChanged += Tree_OnOnSelectedChanged;
            _tree.OnRightClick += Tree_OnOnRightClick;
            _tree.Parent = this;

            // TODO: create context menu

            // TODO: disable node action if scene editing is not allowed
        }

        private void Tree_OnOnSelectedChanged(List<TreeNode> before, List<TreeNode> after)
        {
            // TODO: change selected objects collection
        }

        private void Tree_OnOnRightClick(TreeNode node, Vector2 location)
        {
            // TODO: show context menu
        }

        /// <inheritdoc />
        public override void OnInit()
        {
        }

        /// <inheritdoc />
        public override void OnExit()
        {
            // Cleanup tree
            _root.DisposeChildren();
        }

        private void BuildSceneTree(ActorTreeNode node)
        {
            var children = node.Actor.GetChildren();
            for (int i = 0; i < children.Length; i++)
            {
                BuildSceneTree(node.AddChild(new ActorTreeNode(children[i])));
            }
        }

        /// <inheritdoc />
        public override void OnSceneLoaded(Scene scene, Guid sceneId)
        {
            var startTime = DateTime.UtcNow;

            // Build scene tree
            // TODO: make it faster by calling engine internaly only once to gather optimzied scene tree
            var sceneNode = new SceneTreeNode(scene);
            BuildSceneTree(sceneNode);
            sceneNode.Expand();

            // TODO: cache expanded/colapsed nodes per scene tree

            // Add to the tree
            bool wasLayoutLocked = _root.IsLayoutLocked;
            _root.IsLayoutLocked = true;
            _root.AddChild(sceneNode);
            _root.SortChildren();
            _root.IsLayoutLocked = wasLayoutLocked;
            _root.PerformLayout();

            var endTime = DateTime.UtcNow;
            var milliseconds = (int)(endTime - startTime).TotalMilliseconds;
            Debug.Log($"Created UI tree for scene \'{scene.Name}\' in {milliseconds} ms");
        }

        /// <inheritdoc />
        public override void OnSceneUnloading(Scene scene, Guid sceneId)
        {
            // Find scene tree node
            var node = _root.FindChild(scene);
            if (node != null)
            {
                Debug.Log($"Cleanup UI tree for scene \'{scene.Name}\'");

                // Cleanup
                node.Dispose();
            }
        }

        /// <inheritdoc />
        public override void Draw()
        {
            // Draw overlay
            string overlayText = null;
            var state = Editor.StateMachine.CurrentState;
            if (state is LoadingState)
            {
                overlayText = "Loading...";
            }
            else if (state is ChangingScenesState)
            {
                overlayText = "Loading scene...";
            }
            else if (_root.ChildrenCount == 0)
            {
                overlayText = "No scene";
            }
            if (overlayText != null)
            {
                Render2D.DrawText(Style.Current.FontLarge, overlayText, GetClientArea(), new Color(0.8f), TextAlignment.Center, TextAlignment.Center);
            }

            base.Draw();
        }
    }
}
