// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System.Collections.Generic;
using System.Threading;
using FlaxEditor.Content;
using FlaxEngine;

namespace FlaxEditor.Surface
{
    /// <summary>
    /// The Visject Surface implementation for the materials editor.
    /// </summary>
    /// <seealso cref="FlaxEditor.Surface.VisjectSurface" />
    public class MaterialSurface : VisjectSurface
    {
        /// <inheritdoc />
        public MaterialSurface(IVisjectSurfaceOwner owner)
        : base(owner)
        {
        }

        /// <inheritdoc />
        protected override bool CanSpawnNodeType(NodeArchetype nodeArchetype)
        {
            if ((nodeArchetype.Flags & NodeFlags.AnimGraphOnly) != 0 || (nodeArchetype.Flags & NodeFlags.VisjectOnly) != 0)
                return false;

            return base.CanSpawnNodeType(nodeArchetype);
        }

        /// <inheritdoc />
        protected override bool ValidateDragItem(AssetItem assetItem)
        {
            switch (assetItem.ItemDomain)
            {
            case ContentDomain.Texture:
            case ContentDomain.CubeTexture:
            case ContentDomain.Material: return true;
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
                case ContentDomain.Texture:
                {
                    // Check if it's a normal map
                    bool isNormalMap = false;
                    var obj = FlaxEngine.Content.LoadAsync<Texture>(item.ID);
                    if (obj)
                    {
                        Thread.Sleep(50);

                        if (!obj.WaitForLoaded())
                        {
                            isNormalMap = obj.IsNormalMap;
                        }
                    }

                    node = SpawnNode(5, (ushort)(isNormalMap ? 4 : 1), args.SurfaceLocation, new object[] { item.ID });
                    break;
                }

                case ContentDomain.CubeTexture:
                {
                    node = SpawnNode(5, 3, args.SurfaceLocation, new object[] { item.ID });
                    break;
                }

                case ContentDomain.Material:
                {
                    node = SpawnNode(8, 1, args.SurfaceLocation, new object[] { item.ID });
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
