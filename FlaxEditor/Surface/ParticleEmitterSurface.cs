// Copyright (c) 2012-2019 Wojciech Figat. All rights reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using FlaxEditor.Content;
using FlaxEditor.Surface.Archetypes;
using FlaxEngine;

namespace FlaxEditor.Surface
{
    /// <summary>
    /// The Visject Surface implementation for the particle emitter graph editor.
    /// </summary>
    /// <seealso cref="FlaxEditor.Surface.VisjectSurface" />
    public class ParticleEmitterSurface : VisjectSurface
    {
        internal Particles.ParticleEmitterNode _rootNode;

        /// <summary>
        /// Gets the root node of the emitter graph.
        /// </summary>
        public Particles.ParticleEmitterNode RootNode => _rootNode;

        /// <inheritdoc />
        public ParticleEmitterSurface(IVisjectSurfaceOwner owner, Action onSave)
        : base(owner, onSave)
        {
        }

        /// <summary>
        /// Arranges the particle modules nodes.
        /// </summary>
        public void ArrangeModulesNodes()
        {
            if (IsLayoutLocked)
                return;
            if (_rootNode == null)
                throw new InvalidOperationException("Missing root node.");

            IsLayoutLocked = true;

            var modules = Nodes.OfType<ParticleModules.ParticleModuleNode>();
            var width = _rootNode.Width;
            var pos = _rootNode.Location + new Vector2(0, 500);
            foreach (var module in modules)
            {
                var height = module.Height;
                module.Bounds = new Rectangle(pos.X, pos.Y, width, height);
                pos.Y += height + 20;
            }
            _rootNode.Height = pos.Y - _rootNode.Location.Y;

            IsLayoutLocked = false;
            PerformLayout();
        }

        /// <inheritdoc />
        public override bool CanSpawnNodeType(NodeArchetype nodeArchetype)
        {
            return (nodeArchetype.Flags & NodeFlags.ParticleEmitterGraph) != 0 && base.CanSpawnNodeType(nodeArchetype);
        }

        /// <inheritdoc />
        protected override bool ValidateDragItem(AssetItem assetItem)
        {
            return false; // TODO: add support for sampling textures in Particle Emitter graph
            switch (assetItem.ItemDomain)
            {
            case ContentDomain.CubeTexture:
            case ContentDomain.Texture: return true;
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

                    node = Context.SpawnNode(5, (ushort)(isNormalMap ? 4 : 1), args.SurfaceLocation, new object[] { item.ID });
                    break;
                }

                case ContentDomain.CubeTexture:
                {
                    node = Context.SpawnNode(5, 3, args.SurfaceLocation, new object[] { item.ID });
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
