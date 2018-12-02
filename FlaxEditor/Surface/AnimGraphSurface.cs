// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System;
using System.Collections.Generic;
using System.Linq;
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
                        Flags = NodeFlags.AnimGraphOnly,
                        DefaultValues = new object[]
                        {
                            "State",
                            Enumerable.Empty<byte>() as byte[],
                            Enumerable.Empty<byte>() as byte[],
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
                    Flags = NodeFlags.AnimGraphOnly,
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

        /// <inheritdoc />
        public AnimGraphSurface(IVisjectSurfaceOwner owner, Action onSave)
        : base(owner, onSave)
        {
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
                    _cmStateMachineMenu = new VisjectCM(StateMachineGroupArchetypes, (arch) => true);
                    _cmStateMachineMenu.ShowExpanded = true;
                }
                menu = _cmStateMachineMenu;
            }

            // Override surface primary context menu for state machine transition editing
            if (Context?.Context is Animation.StateMachineTransition)
            {
                if (_cmStateMachineTransitionMenu == null)
                {
                    _cmStateMachineTransitionMenu = new VisjectCM(NodeFactory.DefaultGroups, CanSpawnNodeType);
                    _cmStateMachineTransitionMenu.AddGroup(StateMachineTransitionGroupArchetype);
                }
                menu = _cmStateMachineTransitionMenu;
            }

            SetPrimaryMenu(menu);
        }

        /// <inheritdoc />
        public override bool CanSpawnNodeType(NodeArchetype nodeArchetype)
        {
            if ((nodeArchetype.Flags & NodeFlags.MaterialOnly) != 0 || (nodeArchetype.Flags & NodeFlags.VisjectOnly) != 0)
                return false;
            return base.CanSpawnNodeType(nodeArchetype);
        }

        /// <inheritdoc />
        protected override bool ValidateDragItem(AssetItem assetItem)
        {
            switch (assetItem.ItemDomain)
            {
            case ContentDomain.SkeletonMask:
            case ContentDomain.Animation: return true;
            }
            return base.ValidateDragItem(assetItem);
        }

        /// <inheritdoc />
        protected override void HandleDragDropAssets(List<AssetItem> objects, DragDropEventArgs args)
        {
            for (int i = 0; i < objects.Count; i++)
            {
                var item = objects[i];
                SurfaceNode node = null;
                switch (item.ItemDomain)
                {
                case ContentDomain.Animation:
                {
                    node = Context.SpawnNode(9, 2, args.SurfaceLocation, new object[]
                    {
                        item.ID,
                        1.0f,
                        true,
                        0.0f,
                    });
                    break;
                }
                case ContentDomain.SkeletonMask:
                {
                    node = Context.SpawnNode(9, 11, args.SurfaceLocation, new object[]
                    {
                        0.0f,
                        item.ID,
                    });
                    break;
                }
                }
                if (node != null)
                {
                    args.SurfaceLocation.X += node.Width + 10;
                }
            }
            base.HandleDragDropAssets(objects, args);
        }

        /// <inheritdoc />
        public override void Dispose()
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

            base.Dispose();
        }
    }
}
