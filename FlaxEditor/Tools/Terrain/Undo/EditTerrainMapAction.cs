// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using FlaxEngine;

namespace FlaxEditor.Tools.Terrain.Undo
{
    /// <summary>
    /// The terrain heightmap or visibility map editing action that records before and after states to swap between unmodified and modified terrain data.
    /// </summary>
    /// <seealso cref="FlaxEditor.IUndoAction" />
    public abstract class EditTerrainMapAction : IUndoAction
    {
        private struct PatchData
        {
            public Int2 PatchCoord;
            public IntPtr Before;
            public IntPtr After;
            public object Tag;
        }

        /// <summary>
        /// The terrain.
        /// </summary>
        protected readonly FlaxEngine.Terrain _terrain;

        /// <summary>
        /// The heightmap length (vertex count).
        /// </summary>
        protected readonly int _heightmapLength;

        /// <summary>
        /// The heightmap data size (in bytes).
        /// </summary>
        protected readonly int _heightmapDataSize;

        private readonly List<PatchData> _patches;

        /// <summary>
        /// Gets a value indicating whether this action has any modification to the terrain (recorded patches changes).
        /// </summary>
        public bool HasAnyModification => _patches.Count > 0;

        /// <summary>
        /// Initializes a new instance of the <see cref="EditTerrainMapAction"/> class.
        /// </summary>
        /// <remarks>Use <see cref="AddPatch"/> to mark new patches to record and <see cref="OnEditingEnd"/> to finalize patches data after editing action.</remarks>
        /// <param name="terrain">The terrain.</param>
        /// <param name="stride">The data stride (eg. sizeof(float)).</param>
        protected EditTerrainMapAction(FlaxEngine.Terrain terrain, int stride)
        {
            _terrain = terrain;
            _patches = new List<PatchData>(4);
            var chunkSize = terrain.ChunkSize;
            var heightmapSize = chunkSize * FlaxEngine.Terrain.PatchEdgeChunksCount + 1;
            _heightmapLength = heightmapSize * heightmapSize;
            _heightmapDataSize = _heightmapLength * stride;
        }

        /// <summary>
        /// Checks if the patch at the given coordinates has been already added.
        /// </summary>
        /// <param name="patchCoord">The patch coordinates.</param>
        /// <returns>True if patch has been added, otherwise false.</returns>
        public bool HashPatch(ref Int2 patchCoord)
        {
            for (int i = 0; i < _patches.Count; i++)
            {
                if (_patches[i].PatchCoord == patchCoord)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Adds the patch to the action and records its current state.
        /// </summary>
        /// <param name="patchCoord">The patch coordinates.</param>
        /// <param name="tag">The custom argument (per patch).</param>
        public void AddPatch(ref Int2 patchCoord, object tag = null)
        {
            var data = Marshal.AllocHGlobal(_heightmapDataSize);
            var source = GetData(ref patchCoord, tag);
            Utils.MemoryCopy(source, data, _heightmapDataSize);
            _patches.Add(new PatchData
            {
                PatchCoord = patchCoord,
                Before = data,
                After = IntPtr.Zero,
                Tag = tag,
            });
        }

        /// <summary>
        /// Called when terrain action editing ends. Record the `after` state of the patches.
        /// </summary>
        public void OnEditingEnd()
        {
            if (_patches.Count == 0)
                return;

            for (int i = 0; i < _patches.Count; i++)
            {
                var patch = _patches[i];
                if (patch.After != IntPtr.Zero)
                    throw new InvalidOperationException("Invalid terrain edit undo action usage.");

                var data = Marshal.AllocHGlobal(_heightmapDataSize);
                var source = GetData(ref patch.PatchCoord, patch.Tag);
                Utils.MemoryCopy(source, data, _heightmapDataSize);
                patch.After = data;
                _patches[i] = patch;
            }

            Editor.Instance.Scene.MarkSceneEdited(_terrain.Scene);
        }

        /// <inheritdoc />
        public abstract string ActionString { get; }

        /// <inheritdoc />
        public void Do()
        {
            Set(x => x.After);
        }

        /// <inheritdoc />
        public void Undo()
        {
            Set(x => x.Before);
        }

        /// <inheritdoc />
        public void Dispose()
        {
            // Ensure to release memory
            for (int i = 0; i < _patches.Count; i++)
            {
                Marshal.FreeHGlobal(_patches[i].Before);
                Marshal.FreeHGlobal(_patches[i].After);
            }
            _patches.Clear();
        }

        private void Set(Func<PatchData, IntPtr> dataGetter)
        {
            for (int i = 0; i < _patches.Count; i++)
            {
                var patch = _patches[i];
                var data = dataGetter(patch);
                SetData(ref patch.PatchCoord, data, patch.Tag);
            }

            Editor.Instance.Scene.MarkSceneEdited(_terrain.Scene);
        }

        /// <summary>
        /// Gets the patch data.
        /// </summary>
        /// <param name="patchCoord">The patch coordinates.</param>
        /// <param name="tag">The custom argument (per patch).</param>
        /// <returns>The data buffer (pointer to unmanaged memory).</returns>
        protected abstract IntPtr GetData(ref Int2 patchCoord, object tag);

        /// <summary>
        /// Sets the patch data.
        /// </summary>
        /// <param name="patchCoord">The patch coordinates.</param>
        /// <param name="data">The patch data.</param>
        /// <param name="tag">The custom argument (per patch).</param>
        protected abstract void SetData(ref Int2 patchCoord, IntPtr data, object tag);
    }
}
