// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System.Collections.Generic;
using FlaxEditor.Content;
using FlaxEngine;

namespace FlaxEditor.Surface
{
    /// <summary>
    /// The Visject Surface implementation for the animation graph editor.
    /// </summary>
    /// <seealso cref="FlaxEditor.Surface.VisjectSurface" />
    public class AnimGraphSurface : VisjectSurface
    {
        /// <inheritdoc />
        public AnimGraphSurface(IVisjectSurfaceOwner owner)
        : base(owner)
        {
        }

        /// <inheritdoc />
        protected override bool CanSpawnNodeType(NodeArchetype nodeArchetype)
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
                    node = SpawnNode(9, 2, args.SurfaceLocation, new object[]
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
                    node = SpawnNode(9, 11, args.SurfaceLocation, new object[]
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
    }
}
