// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System;
using FlaxEngine;

namespace FlaxEditor.Tools.Terrain.Undo
{
    /// <summary>
    /// The terrain visibility map editing action that records before and after states to swap between unmodified and modified terrain data.
    /// </summary>
    /// <seealso cref="FlaxEditor.IUndoAction" />
    /// <seealso cref="EditTerrainMapAction" />
    public class EditTerrainVisibilityMapAction : EditTerrainMapAction
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EditTerrainVisibilityMapAction"/> class.
        /// </summary>
        /// <param name="terrain">The terrain.</param>
        public EditTerrainVisibilityMapAction(FlaxEngine.Terrain terrain)
        : base(terrain)
        {
        }

        /// <inheritdoc />
        public override string ActionString => "Edit terrain visibility map";

        /// <inheritdoc />
        protected override IntPtr GetData(ref Int2 patchCoord)
        {
            return TerrainTools.GetVisibilityMapData(_terrain, ref patchCoord);
        }

        /// <inheritdoc />
        protected override void SetData(ref Int2 patchCoord, IntPtr data)
        {
            var offset = Int2.Zero;
            var size = new Int2((int)Mathf.Sqrt(_heightmapLength));
            if (TerrainTools.ModifyVisibilityMap(_terrain, ref patchCoord, data, ref offset, ref size))
                throw new FlaxException("Failed to modify the visibility map.");
        }
    }
}
