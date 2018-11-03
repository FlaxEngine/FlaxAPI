// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using FlaxEditor.Gizmo;
using FlaxEditor.SceneGraph;
using FlaxEditor.SceneGraph.Actors;
using FlaxEngine;
using FlaxEngine.Rendering;

namespace FlaxEditor.Tools.Terrain
{
    /// <summary>
    /// Gizmo for picking terrain chunks and patches. Managed by the <see cref="EditTerrainGizmoMode"/>.
    /// </summary>
    /// <seealso cref="FlaxEditor.Gizmo.GizmoBase" />
    public sealed class EditTerrainGizmo : GizmoBase
    {
        /// <summary>
        /// The parent mode.
        /// </summary>
        public readonly EditTerrainGizmoMode Mode;

        /// <summary>
        /// Initializes a new instance of the <see cref="EditTerrainGizmo"/> class.
        /// </summary>
        /// <param name="owner">The owner.</param>
        /// <param name="mode">The mode.</param>
        public EditTerrainGizmo(IGizmoOwner owner, EditTerrainGizmoMode mode)
        : base(owner)
        {
            Mode = mode;
        }

        /// <inheritdoc />
        public override void Draw(DrawCallsCollector collector)
        {
            base.Draw(collector);

            if (!IsActive)
                return;

            var highlightMaterial = FlaxEngine.Content.LoadAsyncInternal<MaterialBase>(EditorAssets.HighlightTerrainMaterial);
            var sceneEditing = Editor.Instance.SceneEditing;
            var terrainNode = sceneEditing.SelectionCount == 1 ? sceneEditing.Selection[0] as TerrainNode : null;
            if (terrainNode == null)
                return;
            var terrain = terrainNode.Actor as FlaxEngine.Terrain;

            switch (Mode.EditMode)
            {
            case EditTerrainGizmoMode.Modes.Edit:
            {
                // Highlight selected chunk
                var patchCoord = Mode.SelectedPatchCoord;
                var chunkCoord = Mode.SelectedChunkCoord;
                collector.AddDrawCall(terrain, ref patchCoord, ref chunkCoord, highlightMaterial);

                break;
            }
            case EditTerrainGizmoMode.Modes.Add:
            {
                // TODO: highlight patch to add location as quad

                break;
            }
            case EditTerrainGizmoMode.Modes.Remove:
            {
                // Highlight selected patch
                var patchCoord = Mode.SelectedPatchCoord;
                collector.AddDrawCall(terrain, ref patchCoord, highlightMaterial);

                break;
            }
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
                // Perform detailed tracing
                var terrain = (FlaxEngine.Terrain)hit.Actor;
                TerrainTools.RayCastChunk(terrain, ray, out closest, out var patchCoord, out var chunkCoord);
                Mode.SetSelectedChunk(ref patchCoord, ref chunkCoord);

                sceneEditing.Select(hit);
            }
            else
            {
                sceneEditing.Deselect();
            }
        }
    }
}
