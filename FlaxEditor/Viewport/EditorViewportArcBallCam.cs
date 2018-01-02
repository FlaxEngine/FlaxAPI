////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2018 Flax Engine. All rights reserved.
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

        /// <inheritdoc />
        protected override void UpdateView(float dt, ref Vector3 moveDelta, ref Vector2 mouseDelta)
        {
            // Rotate
            YawPitch += mouseDelta;

            // Zoom
            if (_input.IsZooming)
            {
                _orbitRadius = Mathf.Clamp(_orbitRadius - (MouseWheelZoomSpeedFactor * _input.MouseWheelDelta * 25.0f), 0.001f, 10000.0f);
            }

            // Update view position
            UpdatePosition();
        }
    }
}
