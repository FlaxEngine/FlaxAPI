////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;

namespace FlaxEditor.Gizmo
{
    /// <summary>
    /// Represents collection of Gizmo tools where one is active and in use.
    /// </summary>
    /// <seealso cref="GizmoBase" />
    public class GizmosCollection : List<GizmoBase>
    {
        private GizmoBase _active;

        /// <summary>
        /// Occurs when active gizmo tool gets changed.
        /// </summary>
        public event Action OnActiveChanged;

        /// <summary>
        /// Gets or sets the active gizmo.
        /// </summary>
        /// <value>
        /// The active gizmo.
        /// </value>
        public GizmoBase Active
        {
            get => _active;
            set
            {
                if (_active == value)
                    return;
                if (value != null && !Contains(value))
                    throw new InvalidOperationException("Invalid Gizmo.");

                _active = value;
                OnActiveChanged?.Invoke();
            }
        }

        /// <summary>
        /// Removes the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        public new void Remove(GizmoBase item)
        {
            if (item == _active)
                Active = null;
            base.Remove(item);
        }

        /// <summary>
        /// Clears the collection.
        /// </summary>
        public new void Clear()
        {
            Active = null;
            base.Clear();
        }
    }
}
