// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using FlaxEngine;
using FlaxEngine.GUI;

namespace FlaxEditor.CustomEditors.GUI
{
    /// <summary>
    /// Custom property name label that fires mouse events for label and supports dragging.
    /// </summary>
    /// <seealso cref="FlaxEditor.CustomEditors.GUI.PropertyNameLabel" />
    public class DragablePropertyNameLabel : ClickablePropertyNameLabel
    {
        private bool _isLeftMouseButtonDown;

        /// <summary>
        /// Mouse drag action delegate.
        /// </summary>
        /// <param name="label">The label.</param>
        /// <returns>The drag data or null if not use drag.</returns>
        public delegate DragData DragDelegate(DragablePropertyNameLabel label);

        /// <summary>
        /// The mouse starts the drag. Callbacks gets the drag data.
        /// </summary>
        public DragDelegate Drag;

        /// <inheritdoc />
        public DragablePropertyNameLabel(string name)
        : base(name)
        {
        }

        /// <inheritdoc />
        public override bool OnMouseUp(Vector2 location, MouseButton buttons)
        {
            if (buttons == MouseButton.Left)
            {
                _isLeftMouseButtonDown = false;
            }

            base.OnMouseUp(location, buttons);
            return true;
        }

        /// <inheritdoc />
        public override bool OnMouseDown(Vector2 location, MouseButton buttons)
        {
            if (buttons == MouseButton.Left)
            {
                _isLeftMouseButtonDown = true;
            }

            base.OnMouseDown(location, buttons);
            return true;
        }

        /// <inheritdoc />
        public override void OnMouseLeave()
        {
            base.OnMouseLeave();

            if (_isLeftMouseButtonDown)
            {
                _isLeftMouseButtonDown = false;

                var data = Drag?.Invoke(this);
                if (data != null)
                {
                    DoDragDrop(data);
                }
            }
        }

        /// <inheritdoc />
        public override void OnDestroy()
        {
            // Unlink event
            Drag = null;

            base.OnDestroy();
        }
    }
}
