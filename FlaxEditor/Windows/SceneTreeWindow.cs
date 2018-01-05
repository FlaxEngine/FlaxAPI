////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2018 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using FlaxEditor.SceneGraph;
using FlaxEditor.SceneGraph.GUI;
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
        private bool _isUpdatingSelection;
        private bool _isMouseDown;
        private ContextMenu _contextMenu;
        private ContextMenuChildMenu _spawnMenu;

        private struct ActorsGroup
        {
            public string Name;
            public KeyValuePair<string, Type>[] Types;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SceneTreeWindow"/> class.
        /// </summary>
        /// <param name="editor">The editor.</param>
        public SceneTreeWindow(Editor editor)
            : base(editor, true, ScrollBars.Both)
        {
            Title = "Scene";

            // Create scene structure tree
            var root = editor.Scene.Root;
            root.TreeNode.Expand();
            _tree = new Tree(true);
            _tree.AddChild(root.TreeNode);
            _tree.SelectedChanged += Tree_OnSelectedChanged;
            _tree.RightClick += Tree_OnRightClick;
            _tree.Parent = this;

            // Spawnable actors (groups with single entry are inlined without a child menu)
            var groups = new[]
            {
                new ActorsGroup
                {
                    Types = new[] { new KeyValuePair<string, Type>("Actor", typeof(EmptyActor)) }
                },
                new ActorsGroup
                {
                    Types = new[] { new KeyValuePair<string, Type>("Model", typeof(ModelActor)) }
                },
                new ActorsGroup
                {
                    Types = new[] { new KeyValuePair<string, Type>("Camera", typeof(Camera)) }
                },
                new ActorsGroup
                {
                    Name = "Lights",
                    Types = new[]
                    {
                        new KeyValuePair<string, Type>("Directional Light", typeof(DirectionalLight)),
                        new KeyValuePair<string, Type>("Point Light", typeof(PointLight)),
                        new KeyValuePair<string, Type>("Spot Light", typeof(SpotLight)),
                        new KeyValuePair<string, Type>("Sky Light", typeof(SkyLight)),
                    }
                },
                new ActorsGroup
                {
                    Name = "Visuals",
                    Types = new[]
                    {
                        new KeyValuePair<string, Type>("Environment Probe", typeof(EnvironmentProbe)),
                        new KeyValuePair<string, Type>("Sky", typeof(Sky)),
                        new KeyValuePair<string, Type>("Skybox", typeof(Skybox)),
                        new KeyValuePair<string, Type>("Exponential Height Fog", typeof(ExponentialHeightFog)),
                    }
                },
                new ActorsGroup
                {
                    Name = "Physics",
                    Types = new[]
                    {
                        new KeyValuePair<string, Type>("Rigid Body", typeof(RigidBody)),
                        new KeyValuePair<string, Type>("Character Controller", typeof(CharacterController)),
                        new KeyValuePair<string, Type>("Box Collider", typeof(BoxCollider)),
                        new KeyValuePair<string, Type>("Sphere Collider", typeof(SphereCollider)),
                        new KeyValuePair<string, Type>("Capsule Collider", typeof(CapsuleCollider)),
                        new KeyValuePair<string, Type>("Mesh Collider", typeof(MeshCollider)),
                        new KeyValuePair<string, Type>("Fixed Joint", typeof(FixedJoint)),
                        new KeyValuePair<string, Type>("Distance Joint", typeof(DistanceJoint)),
                        new KeyValuePair<string, Type>("Slider Joint", typeof(SliderJoint)),
                        new KeyValuePair<string, Type>("Spherical Joint", typeof(SphericalJoint)),
                        new KeyValuePair<string, Type>("Hinge Joint", typeof(HingeJoint)),
                        new KeyValuePair<string, Type>("D6 Joint", typeof(D6Joint)),
                    }
                },
                new ActorsGroup
                {
                    Name = "CSG",
                    Types = new[]
                    {
                        new KeyValuePair<string, Type>("Box Brush", typeof(BoxBrush)),
                    }
                },
                new ActorsGroup
                {
                    Name = "Volumes",
                    Types = new[]
                    {
                        new KeyValuePair<string, Type>("PostFx Volume", typeof(PostFxVolume)),
                    }
                },
            };

            // Create context menu
            _contextMenu = new ContextMenu();
            _contextMenu.MinimumWidth = 120;
            _contextMenu.AddButton(1, "Duplicate");
            _contextMenu.AddButton(2, "Delete");
            _contextMenu.AddSeparator();
            _contextMenu.AddButton(3, "Copy");
            _contextMenu.AddButton(4, "Paste");
            _contextMenu.AddButton(5, "Cut");
            _contextMenu.AddSeparator();
            _spawnMenu = _contextMenu.AddChildMenu("New");
            var newActorCm = _spawnMenu.ContextMenu;
            for (int i = 0; i < groups.Length; i++)
            {
                var group = groups[i];

                if (group.Types.Length == 1)
                {
                    var button = newActorCm.AddButton(6 + i, group.Types[0].Key);
                    button.Tag = group.Types[0].Value;
                }
                else
                {
                    var groupCm = newActorCm.AddChildMenu(group.Name).ContextMenu;
                    for (int j = 0; j < group.Types.Length; j++)
                    {
                        var button = groupCm.AddButton(j, group.Types[j].Key);
                        button.Tag = group.Types[j].Value;
                    }
                    groupCm.OnButtonClicked += GroupCmOnButtonClicked;
                }
            }
            newActorCm.OnButtonClicked += ContextMenuOnButtonClicked;
            _contextMenu.VisibleChanged += ContextMenuOnVisibleChanged;
            _contextMenu.OnButtonClicked += ContextMenuOnButtonClicked;
        }

        private void GroupCmOnButtonClicked(int id, ContextMenu contextMenu)
        {
            Spawn((Type)contextMenu.GetButton(id).Tag);
        }

        private void ContextMenuOnVisibleChanged(Control control)
        {
            bool hasSthSelected = Editor.SceneEditing.HasSthSelected;

            _contextMenu.GetButton(1).Enabled = hasSthSelected;
            _contextMenu.GetButton(2).Enabled = hasSthSelected;
            _contextMenu.GetButton(3).Enabled = hasSthSelected;
            _contextMenu.GetButton(5).Enabled = hasSthSelected;
            _spawnMenu.Enabled = Editor.StateMachine.CurrentState.CanEditScene && SceneManager.IsAnySceneLoaded;
        }

        private void ContextMenuOnButtonClicked(int id, ContextMenu contextMenu)
        {
            switch (id)
            {
                case 1:
                    Editor.SceneEditing.Duplicate();
                    break;
                case 2:
                    Editor.SceneEditing.Delete();
                    break;
                case 3:
                    Editor.SceneEditing.Copy();
                    break;
                case 4:
                    Editor.SceneEditing.Paste();
                    break;
                case 5:
                    Editor.SceneEditing.Cut();
                    break;
                default:
                    Spawn((Type)contextMenu.GetButton(id).Tag);
                    break;
            }
        }

        private void Spawn(Type type)
        {
            // Create actor
            Actor actor = (Actor)FlaxEngine.Object.New(type);
            Actor parentActor = null;
            if (Editor.SceneEditing.HasSthSelected && Editor.SceneEditing.Selection[0] is ActorNode actorNode)
            {
                parentActor = actorNode.Actor;
                actorNode.TreeNode.Expand();
            }
            if (parentActor == null)
            {
                var scenes = SceneManager.Scenes;
                if (scenes.Length > 0)
                    parentActor = scenes[scenes.Length - 1];
            }
            if (parentActor != null)
            {
                // Use the same location
                actor.Transform = parentActor.Transform;

                // Rename actor to identify it easly
                actor.Name = StringUtils.IncrementNameNumber(type.Name, x => parentActor.GetChild(x) == null);
            }

            // Spawn it
            Editor.SceneEditing.Spawn(actor, parentActor);
        }

        /// <summary>
        /// Focuses search box.
        /// </summary>
        public void Search()
        {
            throw new NotImplementedException("TODO: scene tree window searching");
        }

        private void Tree_OnSelectedChanged(List<TreeNode> before, List<TreeNode> after)
        {
            // Check if lock events
            if (_isUpdatingSelection)
                return;

            if (after.Count > 0)
            {
                // Get actors from nodes
                var actors = new List<SceneGraphNode>(after.Count);
                for (int i = 0; i < after.Count; i++)
                {
                    if (after[i] is ActorTreeNode node && node.Actor)
                        actors.Add(node.ActorNode);
                }

                // Select
                Editor.SceneEditing.Select(actors);
            }
            else
            {
                // Deselect
                Editor.SceneEditing.Deselect();
            }
        }

        private void Tree_OnRightClick(TreeNode node, Vector2 location)
        {
            if (!Editor.StateMachine.CurrentState.CanEditScene)
                return;
            
            // Show context menu
            _contextMenu.Show(node, location);
        }

        /// <inheritdoc />
        public override void OnInit()
        {
            Editor.SceneEditing.OnSelectionChanged += OnOnSelectionChanged;
        }

        private void OnOnSelectionChanged()
        {
            _isUpdatingSelection = true;

            var selection = Editor.SceneEditing.Selection;
            if (selection.Count == 0)
            {
                _tree.Deselect();
            }
            else
            {
                // Find nodes to select
                var nodes = new List<TreeNode>(selection.Count);
                for (int i = 0; i < selection.Count; i++)
                {
                    if (selection[i] is ActorNode node)
                    {
                        nodes.Add(node.TreeNode);
                    }
                }

                // Select nodes
                _tree.Select(nodes);

                // For single node selected scroll view so user can see it
                if (nodes.Count == 1)
                {
                    ScrollViewTo(nodes[0]);
                }
            }

            _isUpdatingSelection = false;
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
            else if (((ContainerControl)_tree.GetChild(0)).ChildrenCount == 0)
            {
                overlayText = "No scene";
            }
            if (overlayText != null)
            {
                Render2D.DrawText(Style.Current.FontLarge, overlayText, GetClientArea(), new Color(0.8f), TextAlignment.Center, TextAlignment.Center);
            }

            base.Draw();
        }

        /// <inheritdoc />
        public override bool OnMouseDown(Vector2 location, MouseButton buttons)
        {
            if (base.OnMouseDown(location, buttons))
                return true;

            if (buttons == MouseButton.Right)
            {
                _isMouseDown = true;
                return true;
            }

            return false;
        }

        /// <inheritdoc />
        public override bool OnMouseUp(Vector2 location, MouseButton buttons)
        {
            if (base.OnMouseUp(location, buttons))
                return true;

            if (_isMouseDown && buttons == MouseButton.Right)
            {
                _isMouseDown = false;

                if (Editor.StateMachine.CurrentState.CanEditScene)
                {
                    // Show context menu
                    Editor.SceneEditing.Deselect();
                    _contextMenu.Show(this, location);
                }

                return true;
            }

            return false;
        }

        /// <inheritdoc />
        public override void OnLostFocus()
        {
            _isMouseDown = false;

            base.OnLostFocus();
        }

        /// <inheritdoc />
        public override void OnDestroy()
        {
            _contextMenu.Dispose();

            base.OnDestroy();
        }
    }
}
