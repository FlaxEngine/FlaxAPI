////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

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
        /// <value>
        /// The gizmos.
        /// </value>
        GizmosCollection Gizmos { get; }

        /// <summary>
        /// Gets a value indicating whether left mouse button is pressed down.
        /// </summary>
        /// <value>
        ///   <c>true</c> if left mouse button is pressed down; otherwise, <c>false</c>.
        /// </value>
        bool IsLeftMouseButtonDown { get; }

        /// <summary>
        /// Gets a value indicating whether right mouse button is pressed down.
        /// </summary>
        /// <value>
        ///   <c>true</c> if right mouse button is pressed down; otherwise, <c>false</c>.
        /// </value>
        bool IsRightMouseButtonDown { get; }

        /// <summary>
        /// Gets the view position.
        /// </summary>
        /// <value>
        /// The view position.
        /// </value>
        Vector3 ViewPosition { get; }

        /// <summary>
        /// Gets the view orientation.
        /// </summary>
        /// <value>
        /// The view orientation.
        /// </value>
        Quaternion ViewOrientation { get; }

        /// <summary>
        /// Gets the view far clipping plane.
        /// </summary>
        /// <value>
        /// The view far plane.
        /// </value>
        float ViewFarPlane { get; }

        /// <summary>
        /// Gets the mouse ray (in world space of the viewport).
        /// </summary>
        /// <value>
        /// The mouse ray.
        /// </value>
        Ray MouseRay { get; }

        /// <summary>
        /// Gets the mouse movement delta.
        /// </summary>
        /// <value>
        /// The mouse movement delta.
        /// </value>
        Vector2 MouseDelta { get; }

        /// <summary>
        /// Gets a value indicating whether use grid snapping during gizmo operations.
        /// </summary>
        /// <value>
        ///   <c>true</c> if use grid snapping; otherwise, <c>false</c>.
        /// </value>
        bool UseSnapping { get; }

        /*
        /// <summary>
        /// Gets viewport ro use during gizmo rendering
        /// </summary>
        /// <returns>Viewport</returns>
        virtual EditorViewport* GetViewport() const = 0;

        /// <summary>
        /// Try to find hit actor under the current mouse location
        /// </summary>
        /// <param name="ray">Ray to test</param>
        /// <returns>Hit actor or nothing</returns>
        virtual Actor* RaycastScene(const Ray& ray) = 0;

        /// <summary>
        /// Tries to find actor with given id in objects pool
        /// </summary>
        /// <param name="id">ID of the actor to find</param>
        /// <returns>Found actor or null</returns>
        virtual Actor* FindActor(const Guid& id) const = 0;

        /// <summary>
        /// Gets undo/redo context
        /// </summary>
        /// <returns>UR service</returns>
        virtual URContext* GetUndoRedo() const = 0;

        /// <summary>
        /// Action fired when any Gizmo tool option gets changes
        /// </summary>
        virtual void OnGizmoOptionsChanged();

        /// <summary>
        /// Action fired when selection pool gets changed
        /// </summary>
        virtual void OnSelectionChanged();

        /// <summary>
        /// Action fired when gizmo ends transforming object(s)
        /// </summary>
        virtual void OnTransfomingEnd();

        /// <summary>
        /// Action fired when gizmo (or child undo/redo system) changes actor(s) transform
        /// </summary>
        virtual void OnTransformObject();*/
    }
}
