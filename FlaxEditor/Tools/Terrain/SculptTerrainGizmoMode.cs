// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System;
using System.Collections.Generic;
using FlaxEditor.SceneGraph.Actors;
using FlaxEditor.Tools.Terrain.Brushes;
using FlaxEditor.Tools.Terrain.Sculpt;
using FlaxEditor.Viewport;
using FlaxEditor.Viewport.Modes;
using FlaxEngine;

namespace FlaxEditor.Tools.Terrain
{
    /// <summary>
    /// Terrain carving tool mode.
    /// </summary>
    /// <seealso cref="FlaxEditor.Viewport.Modes.EditorGizmoMode" />
    public class SculptTerrainGizmoMode : EditorGizmoMode
    {
        /// <summary>
        /// The terrain carving gizmo.
        /// </summary>
        public SculptTerrainGizmo Gizmo;

        /// <summary>
        /// The tool modes.
        /// </summary>
        public enum ModeTypes
        {
            /// <summary>
            /// The sculpt mode.
            /// </summary>
            Sculpt,

            /// <summary>
            /// The smooth mode.
            /// </summary>
            Smooth,
        }

        /// <summary>
        /// The brush types.
        /// </summary>
        public enum BrushTypes
        {
            /// <summary>
            /// The circle brush.
            /// </summary>
            CircleBrush,
        }

        private readonly Mode[] _modes =
        {
            new SculptMode(),
            new SmoothMode(),
        };

        private readonly Brush[] _brushes =
        {
            new CircleBrush(),
        };

        private ModeTypes _modeType = ModeTypes.Sculpt;
        private BrushTypes _brushType = BrushTypes.CircleBrush;

        /// <summary>
        /// Occurs when tool mode gets changed.
        /// </summary>
        public event Action ToolModeChanged;

        /// <summary>
        /// Gets the current tool mode (enum).
        /// </summary>
        public ModeTypes ToolModeType
        {
            get => _modeType;
            set
            {
                if (_modeType != value)
                {
                    _modeType = value;
                    ToolModeChanged?.Invoke();
                }
            }
        }

        /// <summary>
        /// Gets the current tool mode.
        /// </summary>
        public Mode CurrentMode => _modes[(int)_modeType];

        /// <summary>
        /// Gets the sculpt mode instance.
        /// </summary>
        public SculptMode SculptMode => _modes[(int)ModeTypes.Sculpt] as SculptMode;

        /// <summary>
        /// Gets the smooth mode instance.
        /// </summary>
        public SmoothMode SmoothMode => _modes[(int)ModeTypes.Smooth] as SmoothMode;

        /// <summary>
        /// Occurs when tool brush gets changed.
        /// </summary>
        public event Action ToolBrushChanged;

        /// <summary>
        /// Gets the current tool brush (enum).
        /// </summary>
        public BrushTypes ToolBrushType
        {
            get => _brushType;
            set
            {
                if (_brushType != value)
                {
                    _brushType = value;
                    ToolBrushChanged?.Invoke();
                }
            }
        }

        /// <summary>
        /// Gets the current brush.
        /// </summary>
        public Brush CurrentBrush => _brushes[(int)_brushType];

        /// <summary>
        /// Gets the circle brush instance.
        /// </summary>
        public CircleBrush CircleBrush => _brushes[(int)BrushTypes.CircleBrush] as CircleBrush;

        /// <summary>
        /// The last valid cursor position of the brush (in world space).
        /// </summary>
        public Vector3 CursorPosition { get; private set; }

        /// <summary>
        /// Flag used to indicate whenever last cursor position of the brush is valid.
        /// </summary>
        public bool HasValidHit { get; private set; }

        /// <summary>
        /// The selected terrain patches coordinates collection that are under cursor (affected by the brush).
        /// </summary>
        public readonly List<Int2> PatchesUnderCursor = new List<Int2>();

        /// <summary>
        /// The selected terrain chunk coordinates collection that are under cursor (affected by the brush). A XY components contain patch coordinates, ZW components contain chunk coordinates.
        /// </summary>
        public readonly List<Int4> ChunksUnderCursor = new List<Int4>();

        /// <summary>
        /// Gets the selected terrain actor (see <see cref="Modules.SceneEditingModule"/>).
        /// </summary>
        public FlaxEngine.Terrain SelectedTerrain
        {
            get
            {
                var sceneEditing = Editor.Instance.SceneEditing;
                var terrainNode = sceneEditing.SelectionCount == 1 ? sceneEditing.Selection[0] as TerrainNode : null;
                return (FlaxEngine.Terrain)terrainNode?.Actor;
            }
        }

        /// <summary>
        /// Gets the world bounds of the brush located at the current cursor position (defined by <see cref="CursorPosition"/>). Valid only if <see cref="HasValidHit"/> is set to true.
        /// </summary>
        public BoundingBox CursorBrushBounds
        {
            get
            {
                const float brushExtentY = 10000.0f;
                float brushSizeHalf = CurrentBrush.Size * 0.5f;
                Vector3 center = CursorPosition;

                BoundingBox box;
                box.Minimum = new Vector3(center.X - brushSizeHalf, center.Y - brushSizeHalf - brushExtentY, center.Z - brushSizeHalf);
                box.Maximum = new Vector3(center.X + brushSizeHalf, center.Y + brushSizeHalf + brushExtentY, center.Z + brushSizeHalf);
                return box;
            }
        }

        /// <inheritdoc />
        public override void Init(MainEditorGizmoViewport viewport)
        {
            base.Init(viewport);

            Gizmo = new SculptTerrainGizmo(viewport, this);
        }

        /// <inheritdoc />
        public override void OnActivated()
        {
            base.OnActivated();

            Viewport.Gizmos.Active = Gizmo;
            ClearCursor();
        }

        /// <summary>
        /// Clears the cursor location information cached within the gizmo mode.
        /// </summary>
        public void ClearCursor()
        {
            HasValidHit = false;
            PatchesUnderCursor.Clear();
            ChunksUnderCursor.Clear();
        }

        /// <summary>
        /// Sets the cursor location in the world space. Updates the brush location and cached affected chunks.
        /// </summary>
        /// <param name="hitPosition">The cursor hit location on the selected terrain.</param>
        public void SetCursor(ref Vector3 hitPosition)
        {
            HasValidHit = true;
            CursorPosition = hitPosition;

            // Find patches and chunks affected by the brush
            DebugDraw.DrawBox(CursorBrushBounds, Color.Red);
        }
    }
}
