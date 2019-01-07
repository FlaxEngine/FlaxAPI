// Copyright (c) 2012-2019 Wojciech Figat. All rights reserved.

using FlaxEditor.Gizmo;

namespace FlaxEditor.Tools.Foliage
{
    /// <summary>
    /// Gizmo for editing foliage instances. Managed by the <see cref="EditFoliageGizmoMode"/>.
    /// </summary>
    /// <seealso cref="FlaxEditor.Gizmo.GizmoBase" />
    public sealed class EditFoliageGizmo : GizmoBase
    {
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
    }
}
