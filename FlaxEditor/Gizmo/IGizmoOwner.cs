////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

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
        /// Gets the mouse ray (in world space of the viewport).
        /// </summary>
        /// <value>
        /// The mouse ray.
        /// </value>
        Ray MouseRay { get; }

        /*
        /// <summary>
        /// Gets maximum distance to translate objects using gizmo during single move
        /// </summary>
        /// <returns>Maximu mdistance</returns>
        virtual float GetMaxMoveDistance() const = 0;

        /// <summary>
        /// Gets mouse movement delta
        /// </summary>
        /// <returns>Mouse movement delta</returns>
        virtual Vector2 GetMosueDelta() const = 0;

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
        /// True if use snap mode by force
        /// </summary>
        /// <returns>True if use snap mode</returns>
        virtual bool UseForceSnapping() const
        {
            return false;
        }

        /// <summary>
        /// True if gizmo is now disabled
        /// </summary>
        /// <returns>True if disable gizmo, otherwise false</returns>
        virtual bool IsGizmoDisabled() const
        {
            return false;
        }

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
        virtual void OnTransformObject();

        public:

        /// <summary>
        /// Apply translation to the selected objects pool
        /// </summary>
        /// <param name="selection">Selected objects pool</param>
        /// <param name="translationDelta">Translation delta</param>
        virtual void ApplyTranslation(const Array<Actor*>& selection, const Vector3& translationDelta);

        /// <summary>
        /// Apply translation to the selected objects pool
        /// </summary>
        /// <param name="selection">Selected objects pool</param>
        /// <param name="rotationDelta">Rotation delta</param>
        virtual void ApplyRotation(const Array<Actor*>& selection, const Matrix& rotationDelta);

        /// <summary>
        /// Apply translation to the selected objects pool
        /// </summary>
        /// <param name="selection">Selected objects pool</param>
        /// <param name="scaleDelta">Scale delta</param>
        /// <param name="uniform">True if scale objects uniformly, otherwise false</param>
        virtual void ApplyScale(const Array<Actor*>& selection, const Vector3& scaleDelta, bool uniform);*/
    }
}
