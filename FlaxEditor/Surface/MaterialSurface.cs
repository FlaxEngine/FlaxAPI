// Copyright (c) 2012-2020 Wojciech Figat. All rights reserved.

using System;
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
        public MaterialSurface(IVisjectSurfaceOwner owner, Action onSave, FlaxEditor.Undo undo)
        : base(owner, onSave, undo)
        {
        }

        /// <inheritdoc />
        public override string GetConnectionTypeName(ConnectionType type)
        {
            switch (type)
            {
            case ConnectionType.Impulse: return "Material";
            case ConnectionType.Object: return "Texture";
            default: return base.GetConnectionTypeName(type);
            }
        }

        /// <inheritdoc />
        public override bool CanSpawnNodeType(NodeArchetype nodeArchetype)
        {
            return (nodeArchetype.Flags & NodeFlags.MaterialGraph) != 0 && base.CanSpawnNodeType(nodeArchetype);
        }

        /// <inheritdoc />
        protected override bool ValidateDragItem(AssetItem assetItem)
        {
            if (assetItem.IsOfType<Texture>())
                return true;
            if (assetItem.IsOfType<CubeTexture>())
                return true;
            if (assetItem.IsOfType<MaterialBase>())
                return true;
            if (assetItem.IsOfType<MaterialFunction>())
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

                if (assetItem.IsOfType<Texture>())
                {
                    // Check if it's a normal map
                    bool isNormalMap = false;
                    var obj = FlaxEngine.Content.LoadAsync<Texture>(assetItem.ID);
                    if (obj)
                    {
                        Thread.Sleep(50);

                        if (!obj.WaitForLoaded())
                        {
                            isNormalMap = obj.IsNormalMap;
                        }
                    }

                    node = Context.SpawnNode(5, (ushort)(isNormalMap ? 4 : 1), args.SurfaceLocation, new object[] { assetItem.ID });
                }
                else if (assetItem.IsOfType<CubeTexture>())
                {
                    node = Context.SpawnNode(5, 3, args.SurfaceLocation, new object[] { assetItem.ID });
                }
                else if (assetItem.IsOfType<MaterialBase>())
                {
                    node = Context.SpawnNode(8, 1, args.SurfaceLocation, new object[] { assetItem.ID });
                }
                else if (assetItem.IsOfType<MaterialFunction>())
                {
                    node = Context.SpawnNode(1, 24, args.SurfaceLocation, new object[] { assetItem.ID });
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
