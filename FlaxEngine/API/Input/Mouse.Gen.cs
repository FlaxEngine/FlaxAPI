// This code was auto-generated. Do not modify it.

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace FlaxEngine
{
    /// <summary>
    /// Represents a single hardware mouse device. Used by the Input to report raw mouse input events.
    /// </summary>
    /// <remarks>
    /// The mouse device position is in screen-space (not game client window space).
    /// </remarks>
    [Tooltip("Represents a single hardware mouse device. Used by the Input to report raw mouse input events.")]
    public unsafe partial class Mouse : InputDevice
    {
        /// <inheritdoc />
        protected Mouse() : base()
        {
        }

        /// <summary>
        /// Gets the position of the mouse in the screen-space coordinates.
        /// </summary>
        [Tooltip("The position of the mouse in the screen-space coordinates.")]
        public Vector2 Position
        {
            get { Internal_GetPosition(unmanagedPtr, out var resultAsRef); return resultAsRef; }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_GetPosition(IntPtr obj, out Vector2 resultAsRef);

        /// <summary>
        /// Gets the delta position of the mouse in the screen-space coordinates.
        /// </summary>
        [Tooltip("The delta position of the mouse in the screen-space coordinates.")]
        public Vector2 PositionDelta
        {
            get { Internal_GetPositionDelta(unmanagedPtr, out var resultAsRef); return resultAsRef; }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_GetPositionDelta(IntPtr obj, out Vector2 resultAsRef);

        /// <summary>
        /// Gets the mouse wheel change during the last frame.
        /// </summary>
        [Tooltip("The mouse wheel change during the last frame.")]
        public float ScrollDelta
        {
            get { return Internal_GetScrollDelta(unmanagedPtr); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern float Internal_GetScrollDelta(IntPtr obj);

        /// <summary>
        /// Gets the mouse button state (true if being pressed during the current frame).
        /// </summary>
        /// <param name="button">Mouse button to check</param>
        /// <returns>True if user holds down the button, otherwise false.</returns>
        public bool GetButton(MouseButton button)
        {
            return Internal_GetButton(unmanagedPtr, button);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_GetButton(IntPtr obj, MouseButton button);

        /// <summary>
        /// Gets the mouse button down state (true if was pressed during the current frame).
        /// </summary>
        /// <param name="button">Mouse button to check</param>
        /// <returns>True if user starts pressing down the button, otherwise false.</returns>
        public bool GetButtonDown(MouseButton button)
        {
            return Internal_GetButtonDown(unmanagedPtr, button);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_GetButtonDown(IntPtr obj, MouseButton button);

        /// <summary>
        /// Gets the mouse button up state (true if was released during the current frame).
        /// </summary>
        /// <param name="button">Mouse button to check</param>
        /// <returns>True if user releases the button, otherwise false.</returns>
        public bool GetButtonUp(MouseButton button)
        {
            return Internal_GetButtonUp(unmanagedPtr, button);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_GetButtonUp(IntPtr obj, MouseButton button);
    }
}
