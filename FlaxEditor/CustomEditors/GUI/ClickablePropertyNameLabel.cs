// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using FlaxEngine;

namespace FlaxEditor.CustomEditors.GUI
{
    /// <summary>
    /// Custom property name label that fires mouse events for label.
    /// </summary>
    /// <seealso cref="FlaxEditor.CustomEditors.GUI.PropertyNameLabel" />
    public class ClickablePropertyNameLabel : PropertyNameLabel
    {
        /// <summary>
        /// Mouse action delegate.
        /// </summary>
        /// <param name="label">The label.</param>
        /// <param name="location">The mouse location.</param>
        public delegate void MouseDelegate(ClickablePropertyNameLabel label, Vector2 location);

        /// <summary>
        /// The mouse left button clicks on the label.
        /// </summary>
        public MouseDelegate MouseLeftClick;

        /// <summary>
        /// The mouse right button clicks on the label.
        /// </summary>
        public MouseDelegate MouseRightClick;

        /// <summary>
        /// The mouse left button double clicks on the label.
        /// </summary>
        public MouseDelegate MouseLeftDoubleClick;

        /// <summary>
        /// The mouse right button double clicks on the label.
        /// </summary>
        public MouseDelegate MouseRightDoubleClick;

        /// <inheritdoc />
        public ClickablePropertyNameLabel(string name)
        : base(name)
        {
        }

        /// <inheritdoc />
        public override bool OnMouseUp(Vector2 location, MouseButton buttons)
        {
            // Fire events
            if (buttons == MouseButton.Left)
            {
                MouseLeftClick?.Invoke(this, location);
            }
            else if (buttons == MouseButton.Right)
            {
                MouseRightClick?.Invoke(this, location);
            }

            return true;
        }

        /// <inheritdoc />
        public override bool OnMouseDoubleClick(Vector2 location, MouseButton buttons)
        {
            // Fire events
            if (buttons == MouseButton.Left)
            {
                MouseLeftDoubleClick?.Invoke(this, location);
            }
            else if (buttons == MouseButton.Right)
            {
                MouseRightDoubleClick?.Invoke(this, location);
            }

            return true;
        }

        /// <inheritdoc />
        public override void OnDestroy()
        {
            // Unlink events
            MouseLeftClick = null;
            MouseRightClick = null;
            MouseLeftDoubleClick = null;
            MouseRightDoubleClick = null;

            base.OnDestroy();
        }
    }
}
