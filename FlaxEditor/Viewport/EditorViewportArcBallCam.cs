////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using FlaxEngine;
using FlaxEngine.Rendering;

namespace FlaxEditor.Viewport
{
    /// <summary>
    /// Viewport for Editor with Arc Ball camera logic
    /// </summary>
    /// <seealso cref="FlaxEditor.Viewport.EditorViewport" />
    public class EditorViewportArcBallCam : EditorViewport
    {
        private Vector3 _orbitCenter;
        private float _orbitRadius;

        /// <summary>
        /// Initializes a new instance of the <see cref="EditorViewportArcBallCam"/> class.
        /// </summary>
        /// <param name="task">The task.</param>
        /// <param name="useWidgets">Enable/disable viewport widgets.</param>
        /// <param name="orbitRadius">The orbit radius.</param>
        /// <param name="orbitCenter">The orbit center position.</param>
        public EditorViewportArcBallCam(SceneRenderTask task, bool useWidgets, float orbitRadius, Vector3 orbitCenter)
            : base(task, useWidgets)
        {
            _nearPlane = 1;
            _farPlane = 10000;

            _orbitRadius = orbitRadius;
            _orbitCenter = orbitCenter;

            UpdatePosition();
        }

        /// <summary>
        /// Sets view direction.
        /// </summary>
        /// <param name="direction">The view direction.</param>
        public void SetView(Vector3 direction)
        {
            // Change mouse position
            Quaternion orientation = Quaternion.LookRotation(direction, Vector3.Up);
            Vector3 euler = orientation.EulerAngles;
            YawPitch = new Vector2(euler.Y, euler.X);

            // Rotate
            ViewDirection = direction;

            // Update view position
            UpdatePosition();
        }

        /// <summary>
        /// Sets view orientation.
        /// </summary>
        /// <param name="orientation">The view rotation.</param>
        public void SetView(Quaternion orientation)
        {
            // Change mouse position
            Vector3 euler = orientation.EulerAngles;
            YawPitch = new Vector2(euler.Y, euler.X);

            // Rotate
            ViewOrientation = orientation;

            // Update view position
            UpdatePosition();
        }

        private void UpdatePosition()
        {
            // Move camera to look at orbit center point
            Vector3 localPosition = ViewDirection * (-1 * _orbitRadius);
            ViewPosition = _orbitCenter + localPosition;
        }

        protected override void UpdateMouse(float dt, ref Vector3 move)
        {
            // Rotate
            Quaternion rotation;
            Quaternion.RotationYawPitchRoll(_yawPitch.X * Mathf.DegreesToRadians, _yawPitch.Y * Mathf.DegreesToRadians, 0, out rotation);
            ViewOrientation = rotation;

            // Move
            Vector3 moveLocal;
            Vector3.Transform(ref move, ref rotation, out moveLocal);
            ViewPosition += moveLocal;

            // Update view position
            UpdatePosition();
        }
    }
}
