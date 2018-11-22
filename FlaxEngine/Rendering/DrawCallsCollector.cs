// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System;
using System.Collections.Generic;

namespace FlaxEngine.Rendering
{
    /// <summary>
    /// Helper class to collect GPU drawing requests and send them back to the Engine.
    /// </summary>
    public sealed class DrawCallsCollector
    {
        private readonly List<RenderTask.DrawCall> _drawCalls = new List<RenderTask.DrawCall>();

        internal RenderTask.DrawCall[] DrawCalls => _drawCalls.Count > 0 ? _drawCalls.ToArray() : null;

        /// <summary>
        /// Clears this instance.
        /// </summary>
        public void Clear()
        {
            _drawCalls.Clear();
        }

        /// <summary>
        /// Adds the draw call (single model drawing). Calculates target mesh level of detail and picks a proper meshes to draw (based on a material slot index).
        /// </summary>
        /// <param name="model">The model mesh to render. Cannot be null.</param>
        /// <param name="materialSlotIndex">The material slot index to draw.</param>
        /// <param name="material">The material to apply during rendering. Cannot be null.</param>
        /// <param name="bounds">The bounds of the model instance that is being drawn (model instance bounds).</param>
        /// <param name="world">The world matrix used to transform mesh geometry during rendering. Use <see cref="Matrix.Identity"/> to render mesh 'as is'.</param>
        /// <param name="flags">The static flags. Used to describe type of the geometry.</param>
        /// <param name="receiveDecals">True if rendered geometry can receive decals, otherwise false.</param>
        public void AddDrawCall(Model model, int materialSlotIndex, MaterialBase material, ref BoundingSphere bounds, ref Matrix world, StaticFlags flags = StaticFlags.None, bool receiveDecals = true)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            // Pick a proper LOD
            int lodIndex = RenderTask.Internal_ComputeModelLOD(model.unmanagedPtr, ref bounds, IntPtr.Zero);
            var lods = model.LODs;
            if (lods == null || lods.Length < lodIndex || lodIndex < 0)
                return;
            var lod = lods[lodIndex];

            // Draw meshes
            for (int i = 0; i < lod.Meshes.Length; i++)
            {
                if (lod.Meshes[i].MaterialSlotIndex == materialSlotIndex)
                {
                    AddDrawCall(lod.Meshes[i], material, ref world, flags);
                }
            }
        }

        /// <summary>
        /// Adds the draw call (single model drawing). Calculates target mesh level of detail and picks a proper meshes to draw (based on a material slot index).
        /// </summary>
        /// <param name="model">The model mesh to render. Cannot be null.</param>
        /// <param name="materialSlotIndex">The material slot index to draw.</param>
        /// <param name="material">The material to apply during rendering. Cannot be null.</param>
        /// <param name="lodIndex">The model Level Of Detail to draw (zero-based index).</param>
        /// <param name="world">The world matrix used to transform mesh geometry during rendering. Use <see cref="Matrix.Identity"/> to render mesh 'as is'.</param>
        /// <param name="flags">The static flags. Used to describe type of the geometry.</param>
        /// <param name="receiveDecals">True if rendered geometry can receive decals, otherwise false.</param>
        public void AddDrawCall(Model model, int materialSlotIndex, MaterialBase material, int lodIndex, ref Matrix world, StaticFlags flags = StaticFlags.None, bool receiveDecals = true)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            // Pick a proper LOD
            var lods = model.LODs;
            if (lods == null || lods.Length < lodIndex || lodIndex < 0)
                return;
            var lod = lods[lodIndex];

            // Draw meshes
            for (int i = 0; i < lod.Meshes.Length; i++)
            {
                if (lod.Meshes[i].MaterialSlotIndex == materialSlotIndex)
                {
                    AddDrawCall(lod.Meshes[i], material, ref world, flags);
                }
            }
        }

        /// <summary>
        /// Adds the draw call (single mesh drawing).
        /// </summary>
        /// <param name="mesh">The mesh to render. Cannot be null.</param>
        /// <param name="material">The material to apply during rendering. Cannot be null.</param>
        /// <param name="world">The world matrix used to transform mesh geometry during rendering. Use <see cref="Matrix.Identity"/> to render mesh 'as is'.</param>
        /// <param name="flags">The static flags. Used to describe type of the geometry.</param>
        /// <param name="receiveDecals">True if rendered geometry can receive decals, otherwise false.</param>
        public void AddDrawCall(Mesh mesh, MaterialBase material, ref Matrix world, StaticFlags flags = StaticFlags.None, bool receiveDecals = true)
        {
            if (mesh == null)
                throw new ArgumentNullException(nameof(mesh));
            if (material == null)
                throw new ArgumentNullException(nameof(material));

            var drawCall = new RenderTask.DrawCall
            {
                Type = RenderTask.DrawCall.Types.Mesh,
                Flags = flags,
                LodIndex = mesh._lodIndex,
                Index0 = new Int2(mesh._meshIndex, receiveDecals ? 1 : 0),
                Object = Object.GetUnmanagedPtr(mesh.ParentModel),
                Material = Object.GetUnmanagedPtr(material),
                World = world
            };

            _drawCalls.Add(drawCall);
        }

        /// <summary>
        /// Adds the draw call (single terrain chunk drawing).
        /// </summary>
        /// <param name="terrain">The terrain to render. Cannot be null.</param>
        /// <param name="patchCoord">The terrain patch coordinates.</param>
        /// <param name="chunkCoord">The terrain chunk coordinates.</param>
        /// <param name="material">The material to apply during rendering. Cannot be null.</param>
        /// <param name="lodIndex">The geometry Level Of Detail index.</param>
        public void AddDrawCall(Terrain terrain, ref Int2 patchCoord, ref Int2 chunkCoord, MaterialBase material, int lodIndex = -1)
        {
            if (terrain == null)
                throw new ArgumentNullException(nameof(terrain));
            if (material == null)
                throw new ArgumentNullException(nameof(material));

            var drawCall = new RenderTask.DrawCall
            {
                Type = RenderTask.DrawCall.Types.TerrainChunk,
                Flags = StaticFlags.None,
                LodIndex = lodIndex,
                Index0 = patchCoord,
                Index1 = chunkCoord,
                Object = terrain.unmanagedPtr,
                Material = Object.GetUnmanagedPtr(material),
            };

            _drawCalls.Add(drawCall);
        }

        /// <summary>
        /// Adds the draw call (single terrain patch drawing).
        /// </summary>
        /// <param name="terrain">The terrain to render. Cannot be null.</param>
        /// <param name="patchCoord">The terrain patch coordinates.</param>
        /// <param name="material">The material to apply during rendering. Cannot be null.</param>
        /// <param name="lodIndex">The geometry Level Of Detail index.</param>
        public void AddDrawCall(Terrain terrain, ref Int2 patchCoord, MaterialBase material, int lodIndex = -1)
        {
            if (terrain == null)
                throw new ArgumentNullException(nameof(terrain));
            if (material == null)
                throw new ArgumentNullException(nameof(material));

            for (int i = 0; i < 16; i++)
            {
                var drawCall = new RenderTask.DrawCall
                {
                    Type = RenderTask.DrawCall.Types.TerrainChunk,
                    Flags = StaticFlags.None,
                    LodIndex = lodIndex,
                    Index0 = patchCoord,
                    Index1 = new Int2(i % 4, i / 4),
                    Object = terrain.unmanagedPtr,
                    Material = Object.GetUnmanagedPtr(material),
                };

                _drawCalls.Add(drawCall);
            }
        }

        /// <summary>
        /// Executes the draw calls.
        /// </summary>
        /// <param name="context">The GPU command context.</param>
        /// <param name="task">The render task.</param>
        /// <param name="output">The output texture.</param>
        /// <param name="pass">The rendering pass mode.</param>
        public void ExecuteDrawCalls(GPUContext context, RenderTask task, RenderTarget output, RenderPass pass)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));
            if (task == null)
                throw new ArgumentNullException(nameof(task));
            if (output == null)
                throw new ArgumentNullException(nameof(output));

            GPUContext.Internal_ExecuteDrawCalls(context.unmanagedPtr, task.unmanagedPtr, output.unmanagedPtr, DrawCalls, pass);
        }
    }
}
