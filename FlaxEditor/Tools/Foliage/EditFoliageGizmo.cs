// Copyright (c) 2012-2019 Wojciech Figat. All rights reserved.

using FlaxEditor.Gizmo;
using FlaxEngine;
using FlaxEngine.Rendering;

namespace FlaxEditor.Tools.Foliage
{
    /// <summary>
    /// Gizmo for editing foliage instances. Managed by the <see cref="EditFoliageGizmoMode"/>.
    /// </summary>
    /// <seealso cref="FlaxEditor.Gizmo.GizmoBase" />
    public sealed class EditFoliageGizmo : GizmoBase
    {
        private MaterialBase _highlightMaterial;

        /// <summary>
        /// The parent mode.
        /// </summary>
        public readonly EditFoliageGizmoMode Mode;

        /// <summary>
        /// Initializes a new instance of the <see cref="EditFoliageGizmo"/> class.
        /// </summary>
        /// <param name="owner">The owner.</param>
        /// <param name="mode">The mode.</param>
        public EditFoliageGizmo(IGizmoOwner owner, EditFoliageGizmoMode mode)
        : base(owner)
        {
            Mode = mode;
        }

        /// <inheritdoc />
        public override void Draw(DrawCallsCollector collector)
        {
            base.Draw(collector);

            if (!IsActive || !_highlightMaterial)
                return;

            var foliage = Mode.SelectedFoliage;
            if (!foliage)
                return;
            var instanceIndex = Mode.SelectedInstanceIndex;
            if (instanceIndex < 0 || instanceIndex >= foliage.InstancesCount)
                return;

            foliage.GetInstance(instanceIndex, out var instance);
            var model = FoliageTools.GetFoliageTypeModel(foliage, instance.Type);
            if (model)
            {
                Matrix world;
                foliage.GetLocalToWorldMatrix(out world);
                Matrix matrix;
                instance.Transform.GetWorld(out matrix);
                Matrix instanceWorld;
                Matrix.Multiply(ref matrix, ref world, out instanceWorld);
                collector.AddDrawCall(model, _highlightMaterial, ref instance.Bounds, ref instanceWorld, StaticFlags.None, false);
            }
        }

        /// <inheritdoc />
        public override void Pick()
        {
            // Get mouse ray and try to hit foliage instance
            var foliage = Mode.SelectedFoliage;
            if (!foliage)
                return;
            var ray = Owner.MouseRay;
            FoliageTools.Intersects(foliage, ray, out _, out _, out var instanceIndex);
            Mode.SelectedInstanceIndex = instanceIndex;
        }

        /// <inheritdoc />
        public override void OnActivated()
        {
            base.OnActivated();

            _highlightMaterial = FlaxEngine.Content.LoadAsyncInternal<MaterialBase>(EditorAssets.HighlightMaterial);
        }

        /// <inheritdoc />
        public override void OnDeactivated()
        {
            _highlightMaterial = null;

            base.OnDeactivated();
        }
    }
}
