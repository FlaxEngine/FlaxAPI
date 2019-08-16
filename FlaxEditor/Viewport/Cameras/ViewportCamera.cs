// Copyright (c) 2012-2019 Wojciech Figat. All rights reserved.

using FlaxEngine;

namespace FlaxEditor.Viewport.Cameras
{
    /// <summary>
    /// Base class for <see cref="EditorViewport"/> view controllers.
    /// </summary>
    /// <seealso cref="FlaxEditor.Viewport.Cameras.IViewportCamera" />
    public abstract class ViewportCamera : IViewportCamera
    {
        private EditorViewport _viewport;

        /// <summary>
        /// Gets the parent viewport.
        /// </summary>
        public EditorViewport Viewport
        {
            get => _viewport;
            internal set => _viewport = value;
        }

        /// <summary>
        /// Sets view orientation and position to match the arc ball camera style view.
        /// </summary>
        /// <param name="orientation">The view rotation.</param>
        /// <param name="orbitCenter">The orbit center location.</param>
        /// <param name="orbitRadius">The orbit radius.</param>
        public void SerArcBallView(Quaternion orientation, Vector3 orbitCenter, float orbitRadius)
        {
            // Rotate
            Viewport.ViewOrientation = orientation;

            // Move camera to look at orbit center point
            Vector3 localPosition = Viewport.ViewDirection * (-1 * orbitRadius);
            Viewport.ViewPosition = orbitCenter + localPosition;
        }

        /// <inheritdoc />
        public virtual void Update(float deltaTime)
        {
        }

        /// <inheritdoc />
        public abstract void UpdateView(float dt, ref Vector3 moveDelta, ref Vector2 mouseDelta, out bool centerMouse);
    }
}
