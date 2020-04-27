// This code was auto-generated. Do not modify it.

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace FlaxEngine
{
    /// <summary>
    /// General identifiers for potential force feedback channels. These will be mapped according to the platform specific implementation.
    /// </summary>
    [Tooltip("General identifiers for potential force feedback channels. These will be mapped according to the platform specific implementation.")]
    [StructLayout(LayoutKind.Sequential)]
    public unsafe partial struct GamepadVibrationState
    {
        /// <summary>
        /// The left large motor vibration.
        /// </summary>
        [Tooltip("The left large motor vibration.")]
        public float LeftLarge;

        /// <summary>
        /// The left small motor vibration.
        /// </summary>
        [Tooltip("The left small motor vibration.")]
        public float LeftSmall;

        /// <summary>
        /// The right large motor vibration.
        /// </summary>
        [Tooltip("The right large motor vibration.")]
        public float RightLarge;

        /// <summary>
        /// The right small motor vibration.
        /// </summary>
        [Tooltip("The right small motor vibration.")]
        public float RightSmall;
    }
}

namespace FlaxEngine
{
    /// <summary>
    /// Represents a single hardware gamepad device. Used by the Input to report raw gamepad input events.
    /// </summary>
    [Tooltip("Represents a single hardware gamepad device. Used by the Input to report raw gamepad input events.")]
    public unsafe partial class Gamepad : InputDevice
    {
        /// <inheritdoc />
        protected Gamepad() : base()
        {
        }

        /// <summary>
        /// Gets the gamepad device type identifier.
        /// </summary>
        [Tooltip("The gamepad device type identifier.")]
        public Guid ProductID
        {
            get { Internal_GetProductID(unmanagedPtr, out var resultAsRef); return resultAsRef; }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_GetProductID(IntPtr obj, out Guid resultAsRef);

        /// <summary>
        /// Gets the gamepad axis value.
        /// </summary>
        /// <param name="axis">Gamepad axis to check</param>
        /// <returns>Axis value.</returns>
        public float GetAxis(GamepadAxis axis)
        {
            return Internal_GetAxis(unmanagedPtr, axis);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern float Internal_GetAxis(IntPtr obj, GamepadAxis axis);

        /// <summary>
        /// Gets the gamepad button state (true if being pressed during the current frame).
        /// </summary>
        /// <param name="button">Gamepad button to check</param>
        /// <returns>True if user holds down the button, otherwise false.</returns>
        public bool GetButton(GamepadButton button)
        {
            return Internal_GetButton(unmanagedPtr, button);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_GetButton(IntPtr obj, GamepadButton button);

        /// <summary>
        /// Gets the gamepad button down state (true if was pressed during the current frame).
        /// </summary>
        /// <param name="button">Gamepad button to check</param>
        /// <returns>True if user starts pressing down the button, otherwise false.</returns>
        public bool GetButtonDown(GamepadButton button)
        {
            return Internal_GetButtonDown(unmanagedPtr, button);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_GetButtonDown(IntPtr obj, GamepadButton button);

        /// <summary>
        /// Gets the gamepad button up state (true if was released during the current frame).
        /// </summary>
        /// <param name="button">Gamepad button to check</param>
        /// <returns>True if user releases the button, otherwise false.</returns>
        public bool GetButtonUp(GamepadButton button)
        {
            return Internal_GetButtonUp(unmanagedPtr, button);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_GetButtonUp(IntPtr obj, GamepadButton button);

        /// <summary>
        /// Sets the state of the gamepad vibration. Ignored if controller does not support this.
        /// </summary>
        /// <param name="state">The state.</param>
        public void SetVibration(GamepadVibrationState state)
        {
            Internal_SetVibration(unmanagedPtr, ref state);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetVibration(IntPtr obj, ref GamepadVibrationState state);

        /// <summary>
        /// Sets the color of the gamepad light. Ignored if controller does not support this.
        /// </summary>
        /// <param name="color">The color.</param>
        public void SetColor(Color color)
        {
            Internal_SetColor(unmanagedPtr, ref color);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetColor(IntPtr obj, ref Color color);

        /// <summary>
        /// Resets the color of the gamepad light to the default. Ignored if controller does not support this.
        /// </summary>
        public void ResetColor()
        {
            Internal_ResetColor(unmanagedPtr);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_ResetColor(IntPtr obj);
    }
}
