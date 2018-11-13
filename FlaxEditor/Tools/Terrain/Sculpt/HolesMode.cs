// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System;
using FlaxEngine;

namespace FlaxEditor.Tools.Terrain.Sculpt
{
    /// <summary>
    /// Terrain holes creating tool mode. Edits terrain visibility map by editing area affected by brush.
    /// </summary>
    /// <seealso cref="FlaxEditor.Tools.Terrain.Sculpt.Mode" />
    public sealed class HolesMode : Mode
    {
        /// <inheritdoc />
        public override bool SupportsNegativeApply => true;

        /// <inheritdoc />
        public override bool EditsVisibilityMap => true;

        /// <summary>
        /// Initializes a new instance of the <see cref="HolesMode"/> class.
        /// </summary>
        public HolesMode()
        {
            Strength = 6.0f;
        }

        /// <inheritdoc />
        public override unsafe void Apply(ref ApplyParams p)
        {
            var strength = p.Strength * -1.0f;
            var brushPosition = p.Gizmo.CursorPosition;

            // Apply brush modification
            Profiler.BeginEvent("Apply Brush");
            for (int z = 0; z < p.ModifiedSize.Y; z++)
            {
                var zz = z + p.ModifiedOffset.Y;
                for (int x = 0; x < p.ModifiedSize.X; x++)
                {
                    var xx = x + p.ModifiedOffset.X;
                    var sourceVisibility = p.SourceData[zz * p.HeightmapSize + xx];

                    var samplePositionLocal = p.PatchPositionLocal + new Vector3(xx * FlaxEngine.Terrain.UnitsPerVertex, 0, zz * FlaxEngine.Terrain.UnitsPerVertex);
                    Vector3.Transform(ref samplePositionLocal, ref p.TerrainWorld, out Vector3 samplePositionWorld);
                    samplePositionWorld.Y = brushPosition.Y;

                    var paintAmount = p.Brush.Sample(ref brushPosition, ref samplePositionWorld);

                    // Note: C++ core performs visibility samples clamping during data update (visibility range is 0-1)
                    p.TempBuffer[z * p.ModifiedSize.X + x] = sourceVisibility + paintAmount * strength;
                }
            }
            Profiler.EndEvent();

            // Update terrain patch
            TerrainTools.ModifyVisibilityMap(p.Terrain, ref p.PatchCoord, new IntPtr(p.TempBuffer), ref p.ModifiedOffset, ref p.ModifiedSize);
        }
    }
}
