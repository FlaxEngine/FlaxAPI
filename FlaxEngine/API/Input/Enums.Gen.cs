// This code was auto-generated. Do not modify it.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace FlaxEngine
{
    /// <summary>
    /// Hardware mouse cursor behaviour.
    /// </summary>
    [Tooltip("Hardware mouse cursor behaviour.")]
    public enum CursorLockMode
    {
        /// <summary>
        /// The default mode.
        /// </summary>
        [Tooltip("The default mode.")]
        None = 0,

        /// <summary>
        /// Cursor position is locked to the center of the game window.
        /// </summary>
        [Tooltip("Cursor position is locked to the center of the game window.")]
        Locked = 1,
    }
}

namespace FlaxEngine
{
    /// <summary>
    /// Mouse buttons types.
    /// </summary>
    [Tooltip("Mouse buttons types.")]
    public enum MouseButton
    {
        /// <summary>
        /// No button.
        /// </summary>
        [Tooltip("No button.")]
        None = 0,

        /// <summary>
        /// Left button.
        /// </summary>
        [Tooltip("Left button.")]
        Left = 1,

        /// <summary>
        /// Middle button.
        /// </summary>
        [Tooltip("Middle button.")]
        Middle = 2,

        /// <summary>
        /// Right button.
        /// </summary>
        [Tooltip("Right button.")]
        Right = 3,

        /// <summary>
        /// Extended button 1 (or XButton1).
        /// </summary>
        [Tooltip("Extended button 1 (or XButton1).")]
        Extended1 = 4,

        /// <summary>
        /// Extended button 2 (or XButton2).
        /// </summary>
        [Tooltip("Extended button 2 (or XButton2).")]
        Extended2 = 5,
    }
}

namespace FlaxEngine
{
    /// <summary>
    /// Axis for gamepad.
    /// </summary>
    [Tooltip("Axis for gamepad.")]
    public enum GamepadAxis
    {
        /// <summary>
        /// No axis.
        /// </summary>
        [Tooltip("No axis.")]
        None = 0,

        /// <summary>
        /// The X-Axis of the left thumb stick.
        /// </summary>
        [Tooltip("The X-Axis of the left thumb stick.")]
        LeftStickX = 1,

        /// <summary>
        /// The Y-Axis of the left thumb stick.
        /// </summary>
        [Tooltip("The Y-Axis of the left thumb stick.")]
        LeftStickY = 2,

        /// <summary>
        /// The X-Axis of the right thumb stick.
        /// </summary>
        [Tooltip("The X-Axis of the right thumb stick.")]
        RightStickX = 3,

        /// <summary>
        /// The Y-Axis of the right thumb stick.
        /// </summary>
        [Tooltip("The Y-Axis of the right thumb stick.")]
        RightStickY = 4,

        /// <summary>
        /// The left trigger.
        /// </summary>
        [Tooltip("The left trigger.")]
        LeftTrigger = 5,

        /// <summary>
        /// The right trigger.
        /// </summary>
        [Tooltip("The right trigger.")]
        RightTrigger = 6,

        /// <summary>
        /// The count of items in the GamepadAxis enum.
        /// </summary>
        [Tooltip("The count of items in the GamepadAxis enum.")]
        MAX,
    }
}

namespace FlaxEngine
{
    /// <summary>
    /// Buttons for gamepad.
    /// </summary>
    [Tooltip("Buttons for gamepad.")]
    public enum GamepadButton
    {
        /// <summary>
        /// No buttons.
        /// </summary>
        [Tooltip("No buttons.")]
        None = 0,

        /// <summary>
        /// PadUp button (DPad / Directional Pad).
        /// </summary>
        [Tooltip("PadUp button (DPad / Directional Pad).")]
        DPadUp = 1,

        /// <summary>
        /// PadDown button (DPad / Directional Pad).
        /// </summary>
        [Tooltip("PadDown button (DPad / Directional Pad).")]
        DPadDown = 2,

        /// <summary>
        /// PadLeft button (DPad / Directional Pad).
        /// </summary>
        [Tooltip("PadLeft button (DPad / Directional Pad).")]
        DPadLeft = 3,

        /// <summary>
        /// PadRight button (DPad / Directional Pad).
        /// </summary>
        [Tooltip("PadRight button (DPad / Directional Pad).")]
        DPadRight = 4,

        /// <summary>
        /// Start button.
        /// </summary>
        [Tooltip("Start button.")]
        Start = 5,

        /// <summary>
        /// Back button.
        /// </summary>
        [Tooltip("Back button.")]
        Back = 6,

        /// <summary>
        /// Left thumbstick button.
        /// </summary>
        [Tooltip("Left thumbstick button.")]
        LeftThumb = 7,

        /// <summary>
        /// Right thumbstick button.
        /// </summary>
        [Tooltip("Right thumbstick button.")]
        RightThumb = 8,

        /// <summary>
        /// Left shoulder button.
        /// </summary>
        [Tooltip("Left shoulder button.")]
        LeftShoulder = 9,

        /// <summary>
        /// Right shoulder button.
        /// </summary>
        [Tooltip("Right shoulder button.")]
        RightShoulder = 10,

        /// <summary>
        /// Left trigger button.
        /// </summary>
        [Tooltip("Left trigger button.")]
        LeftTrigger = 11,

        /// <summary>
        /// Right trigger button.
        /// </summary>
        [Tooltip("Right trigger button.")]
        RightTrigger = 12,

        /// <summary>
        /// A (face button down).
        /// </summary>
        [Tooltip("A (face button down).")]
        A = 13,

        /// <summary>
        /// B (face button right).
        /// </summary>
        [Tooltip("B (face button right).")]
        B = 14,

        /// <summary>
        /// X (face button left).
        /// </summary>
        [Tooltip("X (face button left).")]
        X = 15,

        /// <summary>
        /// Y (face button up).
        /// </summary>
        [Tooltip("Y (face button up).")]
        Y = 16,

        /// <summary>
        /// The left stick up.
        /// </summary>
        [Tooltip("The left stick up.")]
        LeftStickUp = 17,

        /// <summary>
        /// The left stick down.
        /// </summary>
        [Tooltip("The left stick down.")]
        LeftStickDown = 18,

        /// <summary>
        /// The left stick left.
        /// </summary>
        [Tooltip("The left stick left.")]
        LeftStickLeft = 19,

        /// <summary>
        /// The left stick right.
        /// </summary>
        [Tooltip("The left stick right.")]
        LeftStickRight = 20,

        /// <summary>
        /// The right stick up.
        /// </summary>
        [Tooltip("The right stick up.")]
        RightStickUp = 21,

        /// <summary>
        /// The right stick down.
        /// </summary>
        [Tooltip("The right stick down.")]
        RightStickDown = 22,

        /// <summary>
        /// The right stick left.
        /// </summary>
        [Tooltip("The right stick left.")]
        RightStickLeft = 23,

        /// <summary>
        /// The right stick right.
        /// </summary>
        [Tooltip("The right stick right.")]
        RightStickRight = 24,

        /// <summary>
        /// The count of items in the GamepadButton enum.
        /// </summary>
        [Tooltip("The count of items in the GamepadButton enum.")]
        MAX,
    }
}

namespace FlaxEngine
{
    /// <summary>
    /// The input action event trigger modes.
    /// </summary>
    [Tooltip("The input action event trigger modes.")]
    public enum InputActionMode
    {
        /// <summary>
        /// User is pressing the key/button.
        /// </summary>
        [Tooltip("User is pressing the key/button.")]
        Pressing = 0,

        /// <summary>
        /// User pressed the key/button (but wasn't pressing it in the previous frame).
        /// </summary>
        [Tooltip("User pressed the key/button (but wasn't pressing it in the previous frame).")]
        Press = 1,

        /// <summary>
        /// User released the key/button (was pressing it in the previous frame).
        /// </summary>
        [Tooltip("User released the key/button (was pressing it in the previous frame).")]
        Release = 2,
    }
}

namespace FlaxEngine
{
    /// <summary>
    /// The input gamepad index.
    /// </summary>
    [Tooltip("The input gamepad index.")]
    public enum InputGamepadIndex
    {
        /// <summary>
        /// All detected gamepads.
        /// </summary>
        [Tooltip("All detected gamepads.")]
        All = -1,

        /// <summary>
        /// The gamepad no. 0.
        /// </summary>
        [Tooltip("The gamepad no. 0.")]
        Gamepad0 = 0,

        /// <summary>
        /// The gamepad no. 1.
        /// </summary>
        [Tooltip("The gamepad no. 1.")]
        Gamepad1 = 1,

        /// <summary>
        /// The gamepad no. 2.
        /// </summary>
        [Tooltip("The gamepad no. 2.")]
        Gamepad2 = 2,

        /// <summary>
        /// The gamepad no. 3.
        /// </summary>
        [Tooltip("The gamepad no. 3.")]
        Gamepad3 = 3,

        /// <summary>
        /// The gamepad no. 4.
        /// </summary>
        [Tooltip("The gamepad no. 4.")]
        Gamepad4 = 4,

        /// <summary>
        /// The gamepad no. 5.
        /// </summary>
        [Tooltip("The gamepad no. 5.")]
        Gamepad5 = 5,
    }
}

namespace FlaxEngine
{
    /// <summary>
    /// The input axes types.
    /// </summary>
    [Tooltip("The input axes types.")]
    public enum InputAxisType
    {
        /// <summary>
        /// The mouse X-Axis (mouse delta position scaled by the sensitivity).
        /// </summary>
        [Tooltip("The mouse X-Axis (mouse delta position scaled by the sensitivity).")]
        MouseX = 0,

        /// <summary>
        /// The mouse Y-Axis (mouse delta position scaled by the sensitivity).
        /// </summary>
        [Tooltip("The mouse Y-Axis (mouse delta position scaled by the sensitivity).")]
        MouseY = 1,

        /// <summary>
        /// The mouse wheel (mouse wheel delta scaled by the sensitivity).
        /// </summary>
        [Tooltip("The mouse wheel (mouse wheel delta scaled by the sensitivity).")]
        MouseWheel = 2,

        /// <summary>
        /// The gamepad X-Axis of the left thumb stick.
        /// </summary>
        [Tooltip("The gamepad X-Axis of the left thumb stick.")]
        GamepadLeftStickX = 3,

        /// <summary>
        /// The gamepad Y-Axis of the left thumb stick.
        /// </summary>
        [Tooltip("The gamepad Y-Axis of the left thumb stick.")]
        GamepadLeftStickY = 4,

        /// <summary>
        /// The gamepad X-Axis of the right thumb stick.
        /// </summary>
        [Tooltip("The gamepad X-Axis of the right thumb stick.")]
        GamepadRightStickX = 5,

        /// <summary>
        /// The gamepad Y-Axis of the right thumb stick.
        /// </summary>
        [Tooltip("The gamepad Y-Axis of the right thumb stick.")]
        GamepadRightStickY = 6,

        /// <summary>
        /// The gamepad left trigger.
        /// </summary>
        [Tooltip("The gamepad left trigger.")]
        GamepadLeftTrigger = 7,

        /// <summary>
        /// The gamepad right trigger.
        /// </summary>
        [Tooltip("The gamepad right trigger.")]
        GamepadRightTrigger = 8,

        /// <summary>
        /// The keyboard only mode. For key inputs.
        /// </summary>
        [Tooltip("The keyboard only mode. For key inputs.")]
        KeyboardOnly = 9,
    }
}
