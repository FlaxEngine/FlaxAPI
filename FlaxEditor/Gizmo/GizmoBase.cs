////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2018 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System.Collections.Generic;
using FlaxEditor.SceneGraph;
using FlaxEngine.Rendering;

namespace FlaxEditor.Gizmo
{
    /// <summary>
    /// Base class for all Gizmo controls that can be attached to the <see cref="IGizmoOwner"/>.
    /// </summary>
    public abstract class GizmoBase
    {
        /// <summary>
        /// Gets the gizmo wner.
        /// </summary>
        public IGizmoOwner Owner { get; }

        /// <summary>
        /// Gets a value indicating whether this gizmo is active.
        /// </summary>
        public bool IsActive => Owner.Gizmos.Active == this;

        /// <summary>
        /// Initializes a new instance of the <see cref="GizmoBase"/> class.
        /// </summary>
        /// <param name="owner">The gizmos owner.</param>
        protected GizmoBase(IGizmoOwner owner)
        {
            // Link
            Owner = owner;
            Owner.Gizmos.Add(this);
        }

        /// <summary>
        /// Called when selected objects collection gets changed.
        /// </summary>
        /// <param name="newSelection">The new selection pool.</param>
        public virtual void OnSelectionChanged(List<SceneGraphNode> newSelection)
        {
        }

        /// <summary>
        /// Updates the gizmo logic (called even if not active).
        /// </summary>
        /// <param name="dt">Update delta time (in seconds).</param>
        public virtual void Update(float dt)
        {
        }

        /// <summary>
        /// Draws the gizmo.
        /// </summary>
        /// <param name="collector">The draw calls collector.</param>
        public virtual void Draw(DrawCallsCollector collector)
        {
        }
    }
}
