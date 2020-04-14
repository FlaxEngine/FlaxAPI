// Copyright (c) 2012-2020 Wojciech Figat. All rights reserved.

using System;
using System.Collections.Generic;
using FlaxEditor.Content;
using FlaxEditor.Surface.ContextMenu;
using FlaxEngine;
using Animation = FlaxEditor.Surface.Archetypes.Animation;

namespace FlaxEditor.Surface
{
    /// <summary>
    /// The Visject Surface implementation for the animation graph editor.
    /// </summary>
    /// <seealso cref="FlaxEditor.Surface.VisjectSurface" />
    public class AnimGraphSurface : VisjectSurface
    {
        private static readonly List<GroupArchetype> StateMachineGroupArchetypes = new List<GroupArchetype>(new[]
        {
            // Customized Animations group with special nodes to use here
            new GroupArchetype
            {
                GroupID = 9,
                Name = "State Machine",
                Color = new Color(105, 179, 160),
                Archetypes = new[]
                {
                    new NodeArchetype
                    {
                        TypeID = 20,
                        Create = (id, context, arch, groupArch) => new Animation.StateMachineState(id, context, arch, groupArch),
                        Title = "State",
                        Description = "The animation states machine state node",
                        Flags = NodeFlags.AnimGraph,
                        DefaultValues = new object[]
                        {
                            "State",
                            Utils.GetEmptyArray<byte>(),
                            Utils.GetEmptyArray<byte>(),
                        },
                        Size = new Vector2(100, 0),
                    },
                }
            }
        });

        private static readonly GroupArchetype StateMachineTransitionGroupArchetype = new GroupArchetype
        {
            GroupID = 9,
            Name = "Transition",
            Color = new Color(105, 179, 160),
            Archetypes = new[]
            {
                new NodeArchetype
                {
                    TypeID = 23,
                    Title = "Transition Source State Anim",
                    Description = "The animation state machine transition source state animation data information",
                    Flags = NodeFlags.AnimGraph,
                    Size = new Vector2(270, 110),
                    Elements = new[]
                    {
                        NodeElementArchetype.Factory.Output(0, "Length", ConnectionType.Float, 0),
                        NodeElementArchetype.Factory.Output(1, "Time", ConnectionType.Float, 1),
                        NodeElementArchetype.Factory.Output(2, "Normalized Time", ConnectionType.Float, 2),
                        NodeElementArchetype.Factory.Output(3, "Reaming Time", ConnectionType.Float, 3),
                        NodeElementArchetype.Factory.Output(4, "Reaming Normalized Time", ConnectionType.Float, 4),
                    }
                },
            }
        };

        /// <summary>
        /// The state machine editing context menu.
        /// </summary>
        protected VisjectCM _cmStateMachineMenu;

        /// <summary>
        /// The state machine transition editing context menu.
        /// </summary>
        protected VisjectCM _cmStateMachineTransitionMenu;

        private bool _isRegisteredForScriptsReload;

        /// <inheritdoc />
        public AnimGraphSurface(IVisjectSurfaceOwner owner, Action onSave, FlaxEditor.Undo undo)
        : base(owner, onSave, undo)
        {
            // Find custom nodes for Anim Graph
            var customNodes = Editor.Instance.CodeEditing.AnimGraphNodes.GetArchetypes();
            if (customNodes != null && customNodes.Count > 0)
            {
                AddCustomNodes(customNodes);

                // Check if any of the nodes comes from the game scripts - those can be reloaded at runtime so prevent crashes
                if (Editor.Instance.CodeEditing.AnimGraphNodes.HasTypeFromGameScripts)
                {
                    _isRegisteredForScriptsReload = true;
                    ScriptsBuilder.ScriptsReloadBegin += OnScriptsReloadBegin;
                }
            }
        }

        private void OnScriptsReloadBegin()
        {
            Owner.OnSurfaceClose();

            // TODO: make reload soft: dispose default primary context menu, update existing custom nodes to new ones or remove if invalid
        }

        /// <inheritdoc />
        protected override void OnContextChanged()
        {
            base.OnContextChanged();

            VisjectCM menu = null;

            // Override surface primary context menu for state machine editing
            if (Context?.Context is Animation.StateMachine)
            {
                if (_cmStateMachineMenu == null)
                {
                    _cmStateMachineMenu = new VisjectCM(new VisjectCM.InitInfo
                    {
                        Groups = StateMachineGroupArchetypes,
                        CanSpawnNode = (arch) => true,
                    });
                    _cmStateMachineMenu.ShowExpanded = true;
                }
                menu = _cmStateMachineMenu;
            }

            // Override surface primary context menu for state machine transition editing
            if (Context?.Context is Animation.StateMachineTransition)
            {
                if (_cmStateMachineTransitionMenu == null)
                {
                    _cmStateMachineTransitionMenu = new VisjectCM(new VisjectCM.InitInfo
                    {
                        Groups = NodeFactory.DefaultGroups,
                        CanSpawnNode = CanSpawnNodeType,
                        ParametersGetter = null,
                        CustomNodesGroup = GetCustomNodes(),
                    });
                    _cmStateMachineTransitionMenu.AddGroup(StateMachineTransitionGroupArchetype);
                }
                menu = _cmStateMachineTransitionMenu;
            }

            SetPrimaryMenu(menu);
        }

        /// <inheritdoc />
        public override bool CanSpawnNodeType(NodeArchetype nodeArchetype)
        {
            return (nodeArchetype.Flags & NodeFlags.AnimGraph) != 0 && base.CanSpawnNodeType(nodeArchetype);
        }

        /// <inheritdoc />
        protected override bool ValidateDragItem(AssetItem assetItem)
        {
            if (assetItem.IsOfType<SkeletonMask>())
                return true;
            if (assetItem.IsOfType<FlaxEngine.Animation>())
                return true;
            return base.ValidateDragItem(assetItem);
        }

        /// <inheritdoc />
        protected override void HandleDragDropAssets(List<AssetItem> objects, DragDropEventArgs args)
        {
            for (int i = 0; i < objects.Count; i++)
            {
                var assetItem = objects[i];
                SurfaceNode node = null;

                if (assetItem.IsOfType<SkeletonMask>())
                {
                    node = Context.SpawnNode(9, 2, args.SurfaceLocation, new object[]
                    {
                        assetItem.ID,
                        1.0f,
                        true,
                        0.0f,
                    });
                }
                else if (assetItem.IsOfType<FlaxEngine.Animation>())
                {
                    node = Context.SpawnNode(9, 11, args.SurfaceLocation, new object[]
                    {
                        0.0f,
                        assetItem.ID,
                    });
                }

                if (node != null)
                {
                    args.SurfaceLocation.X += node.Width + 10;
                }
            }
            base.HandleDragDropAssets(objects, args);
        }

        /// <inheritdoc />
        public override void OnDestroy()
        {
            if (_cmStateMachineMenu != null)
            {
                _cmStateMachineMenu.Dispose();
                _cmStateMachineMenu = null;
            }
            if (_cmStateMachineTransitionMenu != null)
            {
                _cmStateMachineTransitionMenu.Dispose();
                _cmStateMachineTransitionMenu = null;
            }
            if (_isRegisteredForScriptsReload)
            {
                _isRegisteredForScriptsReload = false;
                ScriptsBuilder.ScriptsReloadBegin -= OnScriptsReloadBegin;
            }

            base.OnDestroy();
        }
    }
}
