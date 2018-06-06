// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

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

        /// <inheritdoc />
        public virtual void Update(float deltaTime)
        {
        }

        /// <inheritdoc />
        public abstract void UpdateView(float dt, ref Vector3 moveDelta, ref Vector2 mouseDelta, out bool centerMouse);
    }
}
