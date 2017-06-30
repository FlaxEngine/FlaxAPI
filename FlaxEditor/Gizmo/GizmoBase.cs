////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System.Collections.Generic;

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
        /// <value>
        /// The owner.
        /// </value>
        public IGizmoOwner Owner { get; }

        /// <summary>
        /// Gets a value indicating whether this gizmo is active.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this gizmo is active; otherwise, <c>false</c>.
        /// </value>
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
        public virtual void OnSelectionChanged(List<ISceneTreeNode> newSelection)
        {
        }
    }
}
