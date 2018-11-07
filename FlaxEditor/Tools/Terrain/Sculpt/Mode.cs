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
        [EditorOrder(0), Limit(0, 1, 0.01f), Tooltip("The tool strength (normalized to range 0-1). Defines the intensity of the sculpt operation to make it stronger or mre subtle.")]
        public float Strength = 0.3f;

        public unsafe void Apply(Brush brush, ref Options options, SculptTerrainGizmoMode gizmo, FlaxEngine.Terrain terrain)
        {
            // Combine final apply strength
            float strength = Strength * options.Strength * options.DeltaTime;
            if (strength <= 0.0f)
                return;

            // Prepare
            var chunkSize = terrain.ChunkSize;
            var vertexCount = chunkSize * 4 + 1;
            var heightmapLength = vertexCount * vertexCount;
            float* tempBuffer = (float*)gizmo.GetHeightmapTempBuffer(heightmapLength * sizeof(float)).ToPointer();

            // Process all the patches under the cursor
            for (int patchIndex = 0; patchIndex < gizmo.PatchesUnderCursor.Count; patchIndex++)
            {
                var patch = gizmo.PatchesUnderCursor[patchIndex];

                // Get the patch heightmap data (cached internally by the c++ core in editor)
                var sourceHeightmap = (float*)TerrainTools.GetHeightmapData(terrain, ref patch.PatchCoord).ToPointer();
                // TODO: record patch data if gizmo has just started editing this chunk (for undo)

                // TODO: perform the actual heightmap modification

                // temporary heightmap editing
                for (int i = 0; i < heightmapLength; i++)
                {
                    //sourceHeightmap[i] += 4.0f;
                    //tempBuffer[i] = (float)i / heightmapLength * 1000.0f;
                    tempBuffer[i] = sourceHeightmap[i] + 100.0f;
                }

                var modifiedOffset = new Int2(0);
                var modifiedSize = new Int2(vertexCount, vertexCount);
                TerrainTools.ModifyHeightMap(terrain, ref patch.PatchCoord, new IntPtr(tempBuffer), ref modifiedOffset, ref modifiedSize);
            }
        }
    }
}
