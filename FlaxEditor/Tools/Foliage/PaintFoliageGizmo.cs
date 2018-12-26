// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System;
using FlaxEditor.Gizmo;
using FlaxEditor.SceneGraph;
using FlaxEditor.SceneGraph.Actors;
using FlaxEngine;
using FlaxEngine.Rendering;

namespace FlaxEditor.Tools.Foliage
{
    /// <summary>
    /// Gizmo for painting with foliage. Managed by the <see cref="PaintFoliageGizmoMode"/>.
    /// </summary>
    /// <seealso cref="FlaxEditor.Gizmo.GizmoBase" />
    public sealed class PaintFoliageGizmo : GizmoBase
    {
        private FlaxEngine.Foliage _paintFoliage;
        private Model _brushModel;

        /// <summary>
        /// The parent mode.
        /// </summary>
        public readonly PaintFoliageGizmoMode Mode;

        /// <summary>
        /// Gets a value indicating whether gizmo tool is painting the foliage.
        /// </summary>
        public bool IsPainting => _paintFoliage != null;

        /// <summary>
        /// Occurs when foliage paint has been started.
        /// </summary>
        public event Action PaintStarted;

        /// <summary>
        /// Occurs when foliage paint has been ended.
        /// </summary>
        public event Action PaintEnded;

        /// <summary>
        /// Initializes a new instance of the <see cref="PaintFoliageGizmo"/> class.
        /// </summary>
        /// <param name="owner">The owner.</param>
        /// <param name="mode">The mode.</param>
        public PaintFoliageGizmo(IGizmoOwner owner, PaintFoliageGizmoMode mode)
        : base(owner)
        {
            Mode = mode;
        }

        private FlaxEngine.Foliage SelectedFoliage
        {
            get
            {
                var sceneEditing = Editor.Instance.SceneEditing;
                var foliageNode = sceneEditing.SelectionCount == 1 ? sceneEditing.Selection[0] as FoliageNode : null;
                return (FlaxEngine.Foliage)foliageNode?.Actor;
            }
        }

        /// <inheritdoc />
        public override void Draw(DrawCallsCollector collector)
        {
            base.Draw(collector);

            if (!IsActive)
                return;

            var terrain = SelectedFoliage;
            if (!terrain)
                return;

            if (Mode.HasValidHit)
            {
                var brushPosition = Mode.CursorPosition;
                var brushColor = new Color(1.0f, 0.85f, 0.0f); // TODO: expose to editor options
                var brushMaterial = Mode.CurrentBrush.GetBrushMaterial(ref brushPosition, ref brushColor);
                if (!_brushModel)
                {
                    _brushModel = FlaxEngine.Content.LoadAsyncInternal<Model>("Editor/Primitives/Sphere");
                }

                // Draw paint brush
                if (_brushModel && brushMaterial)
                {
                    Matrix transform = Matrix.Scaling(Mode.CurrentBrush.Size * 0.01f) * Matrix.Translation(brushPosition);
                    collector.AddDrawCall(_brushModel, 0, brushMaterial, 0, ref transform, StaticFlags.None, false);
                }
            }
        }

        /// <summary>
        /// Called to start foliage painting
        /// </summary>
        /// <param name="foliage">The foliage.</param>
        private void PaintStart(FlaxEngine.Foliage foliage)
        {
            // Skip if already is painting
            if (IsPainting)
                return;

            _paintFoliage = foliage;
            PaintStarted?.Invoke();
        }

        /// <summary>
        /// Called to update foliage painting logic.
        /// </summary>
        /// <param name="dt">The delta time (in seconds).</param>
        private void PaintUpdate(float dt)
        {
            // Skip if is not painting
            if (!IsPainting)
                return;

            // Edit the foliage
            Profiler.BeginEvent("Paint Foliage");
            // TODO: paint foliage
            Profiler.EndEvent();
        }

        /// <summary>
        /// Called to end foliage painting.
        /// </summary>
        private void PaintEnd()
        {
            // Skip if nothing was painted
            if (!IsPainting)
                return;

            _paintFoliage = null;
            PaintEnded?.Invoke();
        }

        /// <inheritdoc />
        public override void Update(float dt)
        {
            base.Update(dt);

            // Check if gizmo is not active
            if (!IsActive)
            {
                PaintEnd();
                return;
            }

            // Check if no foliage is selected
            var foliage = SelectedFoliage;
            if (!foliage)
            {
                PaintEnd();
                return;
            }

            // Check if selected foliage was changed during painting
            if (foliage != _paintFoliage && IsPainting)
            {
                PaintEnd();
            }

            // Perform detailed tracing to find cursor location for the foliage placement
            var mouseRay = Owner.MouseRay;
            var ray = Owner.MouseRay;
            var closest = float.MaxValue;
            var rayCastFlags = SceneGraphNode.RayCastData.FlagTypes.None;
            var hit = Editor.Instance.Scene.Root.RayCast(ref ray, ref closest, rayCastFlags);
            if (hit != null)
            {
                var hitLocation = mouseRay.GetPoint(closest);
                Mode.SetCursor(ref hitLocation);
            }
            // No hit
            else
            {
                Mode.ClearCursor();
            }

            // Handle painting
            if (Owner.IsLeftMouseButtonDown)
                PaintStart(foliage);
            else
                PaintEnd();
            PaintUpdate(dt);
        }
    }
}
