// Copyright (c) 2012-2020 Wojciech Figat. All rights reserved.

using System;
using System.Collections.Generic;
using System.Linq;
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
        public ParticleEmitterSurface(IVisjectSurfaceOwner owner, Action onSave, FlaxEditor.Undo undo)
        : base(owner, onSave, undo)
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

            var modulesGroups = Nodes.OfType<ParticleModules.ParticleModuleNode>().GroupBy(x => x.ModuleType).ToList();
            modulesGroups.Sort((a, b) => a.Key.CompareTo(b.Key));

            var width = _rootNode.Width;
            var rootPos = _rootNode.Location;
            var pos = rootPos;
            pos.Y += Constants.NodeHeaderSize + 1.0f + 7 * Constants.LayoutOffsetY + 6.0f + 4.0f;

            for (int i = 0; i < _rootNode.Headers.Length; i++)
            {
                var header = _rootNode.Headers[i];

                var modulesStart = pos - rootPos;
                var modules = modulesGroups.FirstOrDefault(x => x.Key == header.ModuleType);
                pos.Y += Constants.NodeHeaderSize + 2.0f;
                if (modules != null)
                {
                    foreach (var module in modules)
                    {
                        var height = module.Height;
                        module.Bounds = new Rectangle(pos.X, pos.Y, width, height);
                        pos.Y += height;
                    }
                }
                pos.Y += 20.0f;
                var modulesEnd = pos - rootPos;

                header.Bounds = new Rectangle(modulesStart.X, modulesStart.Y, width, modulesEnd.Y - modulesStart.Y);
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
        protected override NodeArchetype GetParameterGetterNodeArchetype(out ushort groupId)
        {
            groupId = 6;
            return Archetypes.Parameters.Nodes[1];
        }

        /// <inheritdoc />
        protected override bool ValidateDragItem(AssetItem assetItem)
        {
            if (assetItem.IsOfType<Texture>())
                return true;
            if (assetItem.IsOfType<CubeTexture>())
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
                    node = Context.SpawnNode(5, 11, args.SurfaceLocation, new object[] { assetItem.ID });
                }
                else if (assetItem.IsOfType<CubeTexture>())
                {
                    node = Context.SpawnNode(5, 12, args.SurfaceLocation, new object[] { assetItem.ID });
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
