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
    internal sealed class EditTerrainGizmo : GizmoBase
    {
        /// <summary>
        /// The parent mode.
        /// </summary>
        public readonly EditTerrainGizmoMode Mode;

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
                collector.AddDrawCall(terrain, ref Mode.PatchCoord, ref Mode.ChunkCoord, highlightMaterial);

                break;
            }
            case EditTerrainGizmoMode.Modes.Add:
            {
                // TODO: highlight patch to add location as quad

                break;
            }
            case EditTerrainGizmoMode.Modes.Remove:
            {
                // TODO: highlight selected patch to delete

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
                TerrainTools.RayCastChunk(terrain, ray, out closest, out Mode.PatchCoord, out Mode.ChunkCoord);

                sceneEditing.Select(hit);
            }
            else
            {
                sceneEditing.Deselect();
            }
        }
    }
}
