// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using FlaxEditor.Gizmo;
using FlaxEditor.SceneGraph;
using FlaxEditor.SceneGraph.Actors;
using FlaxEngine;
using FlaxEngine.Rendering;

namespace FlaxEditor.Tools.Terrain
{
    /// <summary>
    /// Gizmo for carving terrain. Managed by the <see cref="SculptTerrainGizmoMode"/>.
    /// </summary>
    /// <seealso cref="FlaxEditor.Gizmo.GizmoBase" />
    public sealed class SculptTerrainGizmo : GizmoBase
    {
        /// <summary>
        /// The parent mode.
        /// </summary>
        public readonly SculptTerrainGizmoMode Mode;

        /// <summary>
        /// Initializes a new instance of the <see cref="SculptTerrainGizmo"/> class.
        /// </summary>
        /// <param name="owner">The owner.</param>
        /// <param name="mode">The mode.</param>
        public SculptTerrainGizmo(IGizmoOwner owner, SculptTerrainGizmoMode mode)
        : base(owner)
        {
            Mode = mode;
        }

        private FlaxEngine.Terrain SelectedTerrain
        {
            get
            {
                var sceneEditing = Editor.Instance.SceneEditing;
                var terrainNode = sceneEditing.SelectionCount == 1 ? sceneEditing.Selection[0] as TerrainNode : null;
                return (FlaxEngine.Terrain)terrainNode?.Actor;
            }
        }

        /// <inheritdoc />
        public override void Draw(DrawCallsCollector collector)
        {
            base.Draw(collector);

            if (!IsActive)
                return;

            var terrain = SelectedTerrain;
            if (!terrain)
                return;
            
            if (Mode.HasValidHit)
            {
                var brushPosition = Mode.CursorPosition;
                var brushMaterial = Mode.CurrentBrush.GetBrushMaterial(ref brushPosition);
                if (!brushMaterial)
                    return;
                
                for (int i = 0; i < Mode.ChunksUnderCursor.Count; i++)
                {
                    var chunk = Mode.ChunksUnderCursor[i];

                    // TODO: drawing proper brush visualization
                    collector.AddDrawCall(terrain, ref chunk.PatchCoord, ref chunk.ChunkCoord, brushMaterial);
                }
            }
        }

        /// <inheritdoc />
        public override void Update(float dt)
        {
            base.Update(dt);

            if (!IsActive)
                return;

            var terrain = SelectedTerrain;
            if (!terrain)
                return;

            // Perform detailed tracing to find cursor location on the terrain
            var mouseRay = Owner.MouseRay;
            if (TerrainTools.RayCastChunk(terrain, mouseRay, out var closest, out var patchCoord, out var chunkCoord))
            {
                var hitLocation = mouseRay.GetPoint(closest);
                Mode.SetCursor(ref hitLocation);
            }
            else
            {
                Mode.ClearCursor();
            }
        }

        /// <inheritdoc />
        public override void Pick()
        {
            // Get mouse ray and try to hit terrain
            var ray = Owner.MouseRay;
            var closest = float.MaxValue;
            var rayCastFlags = SceneGraphNode.RayCastData.FlagTypes.SkipColliders;
            var hit = Editor.Instance.Scene.Root.RayCast(ref ray, ref closest, rayCastFlags) as TerrainNode;

            // Update selection
            var sceneEditing = Editor.Instance.SceneEditing;
            if (hit != null)
            {
                sceneEditing.Select(hit);
            }
            else
            {
                sceneEditing.Deselect();
            }
        }
    }
}
