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
        /// Camera's pitch angle clamp range (in degrees)
        /// </summary>
        public Vector2 CamPitchAngles;

        /// <summary>
        /// Gets a value indicating whether this viewport is animating movement.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this viewport is animating movement; otherwise, <c>false</c>.
        /// </value>
        public bool IsAnimatingMove => _moveStartTime > Mathf.Epsilon;

        /// <summary>
        /// Initializes a new instance of the <see cref="EditorViewportFPSCam"/> class.
        /// </summary>
        /// <param name="task">The task.</param>
        /// <param name="useWidgets">Enable/disable viewport widgets.</param>
        public EditorViewportFPSCam(SceneRenderTask task, bool useWidgets)
            : base(task, useWidgets)
        {
            _moveStartTime = -1;
            CamPitchAngles = new Vector2(-80, 80);

            // TODO: provide widget for chaging near and far plane
            //_camera.SetNearPlane(1);
            //_camera.SetFarPlane(10000);
        }

        /*/// <summary>
        /// Sets view.
        /// </summary>
        /// <param name="position">The view position.</param>
        /// <param name="direction">The view direction.</param>
        public void SetView(Vector3 position, Vector3 direction)
        {
            if (IsAnimatingMove)
                return;

            // Rotate and move
            _camera.SetPosition(position);
            _camera.SetDirection(direction);
            updateMouseAbs();
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
            _camera.SetPosition(position);
            _camera.SetOrientation(orientation);
            updateMouseAbs();
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
            _startMove = GetCamera()->GetTransform();
            _endMove = target;
            _moveStartTime = CEngine->GetTickService()->UnscaledTime.GetTotalSeconds();
        }

        private void updateMouseAbs()
        {
            // Change mouse position
            Vector3 euler = _camera.GetOrientation().GetEuler();
            _absMousePos.X = euler.Y;
            _absMousePos.Y = Mathf.Clamp(euler.X, CamPitchAngles.X, CamPitchAngles.Y);
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
                float time = CEngine->GetTickService()->UnscaledTime.GetTotalSeconds();
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
                _camera.SetTransform(targetTransform);
                updateMouseAbs();
            }
        }*/
    }
}
