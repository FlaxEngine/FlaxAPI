////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using FlaxEngine;
using FlaxEngine.Rendering;

namespace FlaxEditor.Viewport
{
    /// <summary>
    /// Viewport for Editor with FPS camera logic.
    /// </summary>
    /// <seealso cref="FlaxEditor.Viewport.EditorViewport" />
    public class EditorViewportFPSCam : EditorViewport
    {
        private Transform _startMove;
        private Transform _endMove;
        private float _moveStartTime;

        /// <summary>
        /// Gets a value indicating whether this viewport is animating movement.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this viewport is animating movement; otherwise, <c>false</c>.
        /// </value>
        public bool IsAnimatingMove => _moveStartTime > Mathf.Epsilon;

        /// <summary>
        /// The target point location. It's used to orbit around it whe user clicks Alt+LMB.
        /// </summary>
        public Vector3 TargetPoint;

        /// <summary>
        /// Initializes a new instance of the <see cref="EditorViewportFPSCam"/> class.
        /// </summary>
        /// <param name="task">The task.</param>
        /// <param name="useWidgets">Enable/disable viewport widgets.</param>
        public EditorViewportFPSCam(SceneRenderTask task, bool useWidgets)
            : base(task, useWidgets)
        {
            _moveStartTime = -1;

            _nearPlane = 1;
            _farPlane = 10000;
        }

        /// <summary>
        /// Sets view.
        /// </summary>
        /// <param name="position">The view position.</param>
        /// <param name="direction">The view direction.</param>
        public void SetView(Vector3 position, Vector3 direction)
        {
            if (IsAnimatingMove)
                return;

            // Rotate and move
            ViewPosition = position;
            ViewDirection = direction;
        }

        /// <summary>
        /// Sets view.
        /// </summary>
        /// <param name="position">The view position.</param>
        /// <param name="orientation">The view rotation.</param>
        public void SetView(Vector3 position, Quaternion orientation)
        {
            if (IsAnimatingMove)
                return;

            // Rotate and move
            ViewPosition = position;
            ViewOrientation = orientation;
        }

        /// <summary>
        /// Start animating viewport movement to the target transformation.
        /// </summary>
        /// <param name="position">The target position.</param>
        /// <param name="orientation">The target orientation.</param>
        public void MoveViewport(Vector3 position, Quaternion orientation)
        {
            MoveViewport(new Transform(position, orientation));
        }

        /// <summary>
        /// Start animating viewport movement to the target transformation.
        /// </summary>
        /// <param name="target">The target transform.</param>
        public void MoveViewport(Transform target)
        {
            _startMove = ViewTransform;
            _endMove = target;
            _moveStartTime = Time.UnscaledTime;
        }

        /// <inheritdoc />
        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);

            // Udate animated movement
            if (IsAnimatingMove)
            {
                // Calculate linear progress
                float animationDuration = 0.5f;
                float time = Time.UnscaledTime;
                float progress = (time - _moveStartTime) / animationDuration;

                // Check for end
                if (progress >= 1.0f)
                {
                    // Animation has been finished
                    _moveStartTime = -1;
                }

                // Animate camera
                float a = Mathf.Saturate(progress);
                a = a * a * a;
                Transform targetTransform = Transform.Lerp(_startMove, _endMove, a);
                targetTransform.Scale = Vector3.Zero;
                ViewPosition = targetTransform.Translation;
                ViewOrientation = targetTransform.Orientation;
            }
        }

        protected override void UpdateView(float dt, ref Vector3 moveDelta, ref Vector2 mouseDelta)
        {
            if (IsAnimatingMove)
                return;

            // Get current view properties
            float yaw = _yaw;
            float pitch = _pitch;
            var position = ViewPosition;
            var rotation = ViewOrientation;

            // Compute base vectors for camera movement
            var forward = Vector3.ForwardLH * rotation;
            var up = Vector3.Up * rotation;
            var right = Vector3.Cross(forward, up);

            // Dolly
            if (_input.IsPanning || _input.IsMoving || _input.IsRotating)
            {
                Vector3 move;
                Vector3.Transform(ref moveDelta, ref rotation, out move);
                position += move;
            }

            // Pan
            if (_input.IsPanning)
            {
                var panningSpeed = 0.8f;
                position -= right * (mouseDelta.X * panningSpeed);
                position -= up * (mouseDelta.Y * panningSpeed);
            }

            // Move
            if (_input.IsMoving)
            {
                // Move camera over XZ plane
                var projectedForward = Vector3.Normalize(new Vector3(forward.X, 0, forward.Z));
                position -= projectedForward * mouseDelta.Y;
                yaw += mouseDelta.X;
            }

            // Rotate or orbit
            if (_input.IsRotating || _input.IsOrbiting)
            {
                yaw += mouseDelta.X;
                pitch += mouseDelta.Y;
            }

            // Zoom in/out
            if (_input.IsZooming)
            {
                position += forward * (MouseWheelZoomSpeedFactor * _input.MouseWheelDelta * 0.1f);
                if (_input.IsAltDown)
                {
                    position += forward * (MouseSpeed * 40 * _mouseDeltaRight.ValuesSum);
                }
            }

            // Update view
            ViewPosition = position;
            Yaw = yaw;
            Pitch = pitch;
        }
    }
}
