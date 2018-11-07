// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System;
using FlaxEditor.Tools.Terrain.Brushes;
using FlaxEngine;

namespace FlaxEditor.Tools.Terrain.Sculpt
{
    /// <summary>
    /// The base class for terran sculpt tool modes.
    /// </summary>
    public abstract class Mode
    {
        /// <summary>
        /// The options container for the terrain editing apply.
        /// </summary>
        public struct Options
        {
            /// <summary>
            /// If checked, modification apply method should be inverted.
            /// </summary>
            public bool Invert;

            /// <summary>
            /// The master strength parameter to apply when editing the terrain.
            /// </summary>
            public float Strength;

            /// <summary>
            /// The delta time (in seconds) for the terrain modification apply. Used to scale the strength. Adjusted to handle low-FPS.
            /// </summary>
            public float DeltaTime;
        }

        /// <summary>
        /// The tool strength (normalized to range 0-1). Defines the intensity of the sculpt operation to make it stronger or mre subtle.
        /// </summary>
        [EditorOrder(0), Limit(0, 5, 0.01f), Tooltip("The tool strength (normalized to range 0-1). Defines the intensity of the sculpt operation to make it stronger or mre subtle.")]
        public float Strength = 1.2f;

        public unsafe void Apply(Brush brush, ref Options options, SculptTerrainGizmoMode gizmo, FlaxEngine.Terrain terrain)
        {
            // Combine final apply strength
            float strength = Strength * options.Strength * options.DeltaTime;
            if (strength <= 0.0f)
                return;
            if (options.Invert)
                strength *= -1;

            // Prepare
            var chunkSize = terrain.ChunkSize;
            var heightmapSize = chunkSize * FlaxEngine.Terrain.PatchEdgeChunksCount + 1;
            var heightmapLength = heightmapSize * heightmapSize;
            var patchSize = chunkSize * FlaxEngine.Terrain.UnitsPerVertex * FlaxEngine.Terrain.PatchEdgeChunksCount;
            float* tempBuffer = (float*)gizmo.GetHeightmapTempBuffer(heightmapLength * sizeof(float)).ToPointer();
            var brushPosition = gizmo.CursorPosition;

            // Get brush bounds in terrain local space
            var brushBounds = gizmo.CursorBrushBounds;
            terrain.GetLocalToWorldMatrix(out var terrainWorld);
            terrain.GetWorldToLocalMatrix(out var terrainInvWorld);
            BoundingBox.Transform(ref brushBounds, ref terrainInvWorld, out var brushBoundsLocal);

            // Process all the patches under the cursor
            for (int patchIndex = 0; patchIndex < gizmo.PatchesUnderCursor.Count; patchIndex++)
            {
                var patch = gizmo.PatchesUnderCursor[patchIndex];
                var patchPositionLocal = new Vector3(patch.PatchCoord.X * patchSize, 0, patch.PatchCoord.Y * patchSize);

                // Get the patch data (cached internally by the c++ core in editor)
                var sourceDataPtr = TerrainTools.GetHeightmapData(terrain, ref patch.PatchCoord);
                if (sourceDataPtr == IntPtr.Zero)
                {
                    throw new FlaxException("Cannot modify terrain. Loading heightmap failed. See log for more info.");
                }
                var sourceData = (float*)sourceDataPtr.ToPointer();
                // TODO: record patch data if gizmo has just started editing this chunk (for undo)

                // Calculate patch heightmap area to modify by brush
                var brushPatchMin = 0;
                var modifiedOffset = new Int2(0);
                var modifiedSize = new Int2(heightmapSize, heightmapSize);

                // Apply brush modification
                Profiler.BeginEvent("Apply Brush");
                for (int z = 0; z < modifiedSize.Y; z++)
                {
                    var zz = z + modifiedOffset.Y;
                    for (int x = 0; x < modifiedSize.X; x++)
                    {
                        var xx = x + modifiedOffset.X;
                        var sourceHeight = sourceData[zz * heightmapSize + xx];

                        var samplePositionLocal = patchPositionLocal + new Vector3(xx * FlaxEngine.Terrain.UnitsPerVertex, sourceHeight, zz * FlaxEngine.Terrain.UnitsPerVertex);
                        Vector3.Transform(ref samplePositionLocal, ref terrainWorld, out Vector3 samplePositionWorld);

                        var paintAmount = brush.Sample(ref brushPosition, ref samplePositionWorld);

                        tempBuffer[z * modifiedSize.X + x] = sourceHeight + paintAmount * strength;
                    }
                }
                Profiler.EndEvent();

                // Update terrain patch
                TerrainTools.ModifyHeightMap(terrain, ref patch.PatchCoord, new IntPtr(tempBuffer), ref modifiedOffset, ref modifiedSize);
            }
        }
    }
}
