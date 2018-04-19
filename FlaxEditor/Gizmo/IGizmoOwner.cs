// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System.Collections.Generic;
using FlaxEngine;

namespace FlaxEditor.Gizmo
{
    /// <summary>
    /// Describies objects that can own gizmo tools.
    /// </summary>
    public interface IGizmoOwner
    {
        /// <summary>
        /// Gets the gizmos collection.
        /// </summary>
        GizmosCollection Gizmos { get; }

        /// <summary>
        /// Gets a value indicating whether left mouse button is pressed down.
        /// </summary>
        bool IsLeftMouseButtonDown { get; }

        /// <summary>
        /// Gets a value indicating whether right mouse button is pressed down.
        /// </summary>
        bool IsRightMouseButtonDown { get; }

        /// <summary>
        /// Gets the view position.
        /// </summary>
        Vector3 ViewPosition { get; }

        /// <summary>
        /// Gets the view orientation.
        /// </summary>
        Quaternion ViewOrientation { get; }

        /// <summary>
        /// Gets the view far clipping plane.
        /// </summary>
        float ViewFarPlane { get; }

        /// <summary>
        /// Gets the mouse ray (in world space of the viewport).
        /// </summary>
        Ray MouseRay { get; }

        /// <summary>
        /// Gets the mouse movement delta.
        /// </summary>
        Vector2 MouseDelta { get; }

        /// <summary>
        /// Gets a value indicating whether use grid snapping during gizmo operations.
        /// </summary>
        bool UseSnapping { get; }

        /// <summary>
        /// Gets a value indicating whether duplicate objects during gizmo operation (eg. when tranforming).
        /// </summary>
        bool UseDuplicate { get; }
		
        /// <summary>
        /// Gets a <see cref="FlaxEditor.Undo"/> object used by the gizmo owner.
        /// </summary>
        Undo Undo { get; }
    }
}
