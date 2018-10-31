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
                        Create = (id, surface, arch, groupArch) => new Animation.StateMachineState(id, surface, arch, groupArch),
                        Title = "State",
                        Description = "The animation states machine state node",
                        Flags = NodeFlags.AnimGraphOnly,
                        DefaultValues = new object[]
                        {
                            "State",
                            Enumerable.Empty<byte>() as byte[],
                        },
                        Size = new Vector2(100, 0),
                    },
                }
            }
        });

        /// <summary>
        /// The state machine editing context menu.
        /// </summary>
        protected VisjectCM _cmStateMachineMenu;

        /// <inheritdoc />
        public AnimGraphSurface(IVisjectSurfaceOwner owner, Action onSave)
        : base(owner, onSave)
        {
        }

        /// <inheritdoc />
        protected override void OnContextChanged()
        {
            base.OnContextChanged();

            // Override surface primary context menu for state machine editing
            bool isStateMachineOpen = Context?.Context is Archetypes.Animation.StateMachine;
            if (isStateMachineOpen && _cmStateMachineMenu == null)
            {
                _cmStateMachineMenu = new VisjectCM(StateMachineGroupArchetypes, (arch) => true);
                _cmStateMachineMenu.OnItemClicked += OnPrimaryMenuButtonClick;
            }
            var menu = isStateMachineOpen ? _cmStateMachineMenu : null;
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
            base.Dispose();
        }
    }
}
