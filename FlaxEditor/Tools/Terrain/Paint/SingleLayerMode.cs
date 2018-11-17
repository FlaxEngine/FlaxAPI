// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System;
using FlaxEngine;

namespace FlaxEditor.Tools.Terrain.Paint
{
    /// <summary>
    /// Paint tool mode. Edits terrain splatmap by painting with the single layer on top of the others.
    /// </summary>
    /// <seealso cref="FlaxEditor.Tools.Terrain.Paint.Mode" />
    public sealed class SingleLayerMode : Mode
    {
        /// <inheritdoc />
        public override unsafe void Apply(ref ApplyParams p)
        {
            var strength = p.Strength;
            var layer = 0; // TODO: paint layer selecting
            var brushPosition = p.Gizmo.CursorPosition;

            // Apply brush modification
            Profiler.BeginEvent("Apply Brush");
            for (int z = 0; z < p.ModifiedSize.Y; z++)
            {
                var zz = z + p.ModifiedOffset.Y;
                for (int x = 0; x < p.ModifiedSize.X; x++)
                {
                    var xx = x + p.ModifiedOffset.X;
                    var src = p.SourceData[zz * p.HeightmapSize + xx];

                    var samplePositionLocal = p.PatchPositionLocal + new Vector3(xx * FlaxEngine.Terrain.UnitsPerVertex, 0, zz * FlaxEngine.Terrain.UnitsPerVertex);
                    Vector3.Transform(ref samplePositionLocal, ref p.TerrainWorld, out Vector3 samplePositionWorld);

                    var paintAmount = p.Brush.Sample(ref brushPosition, ref samplePositionWorld);

                    p.TempBuffer[z * p.ModifiedSize.X + x] = new Color32(255, 0, 0, 0); //sourceHeight + paintAmount * strength;
                }
            }
            Profiler.EndEvent();

            // Update terrain patch
            TerrainTools.ModifySplatMap(p.Terrain, ref p.PatchCoord, new IntPtr(p.TempBuffer), ref p.ModifiedOffset, ref p.ModifiedSize);
        }
    }
}
