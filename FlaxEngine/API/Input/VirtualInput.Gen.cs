// This code was auto-generated. Do not modify it.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace FlaxEngine
{
    /// <summary>
    /// Maps keyboard, controller, or mouse inputs to a "friendly name" that will later be bound to continuous game behavior, such as movement. The inputs mapped in AxisMappings are continuously polled, even if they are just reporting that their input value.
    /// </summary>
    [Tooltip("Maps keyboard, controller, or mouse inputs to a \"friendly name\" that will later be bound to continuous game behavior, such as movement. The inputs mapped in AxisMappings are continuously polled, even if they are just reporting that their input value.")]
    [StructLayout(LayoutKind.Sequential)]
    public unsafe partial struct ActionConfig
    {
        /// <summary>
        /// The action "friendly name" used to access it from code.
        /// </summary>
        [EditorOrder(0)]
        [Tooltip("The action \"friendly name\" used to access it from code.")]
        public string Name;

        /// <summary>
        /// The trigger mode. Allows to specify when input event should be fired.
        /// </summary>
        [EditorOrder(5)]
        [Tooltip("The trigger mode. Allows to specify when input event should be fired.")]
        public InputActionMode Mode;

        /// <summary>
        /// The keyboard key to map for this action. Use <see cref="Keys.None"/> to ignore it.
        /// </summary>
        [EditorOrder(10)]
        [Tooltip("The keyboard key to map for this action. Use <see cref=\"Keys.None\"/> to ignore it.")]
        public Keys Key;

        /// <summary>
        /// The mouse button to map for this action. Use <see cref="MouseButton.None"/> to ignore it.
        /// </summary>
        [EditorOrder(20)]
        [Tooltip("The mouse button to map for this action. Use <see cref=\"MouseButton.None\"/> to ignore it.")]
        public MouseButton MouseButton;

        /// <summary>
        /// The gamepad button to map for this action. Use <see cref="GamepadButton.None"/> to ignore it.
        /// </summary>
        [EditorOrder(30)]
        [Tooltip("The gamepad button to map for this action. Use <see cref=\"GamepadButton.None\"/> to ignore it.")]
        public GamepadButton GamepadButton;

        /// <summary>
        /// Which gamepad should be used.
        /// </summary>
        [EditorOrder(40)]
        [Tooltip("Which gamepad should be used.")]
        public InputGamepadIndex Gamepad;
    }
}

namespace FlaxEngine
{
    /// <summary>
    /// Maps keyboard, controller, or mouse inputs to a "friendly name" that will later be bound to continuous game behavior, such as movement. The inputs mapped in AxisMappings are continuously polled, even if they are just reporting that their input value.
    /// </summary>
    [Tooltip("Maps keyboard, controller, or mouse inputs to a \"friendly name\" that will later be bound to continuous game behavior, such as movement. The inputs mapped in AxisMappings are continuously polled, even if they are just reporting that their input value.")]
    [StructLayout(LayoutKind.Sequential)]
    public unsafe partial struct AxisConfig
    {
        /// <summary>
        /// The axis "friendly name" used to access it from code.
        /// </summary>
        [EditorOrder(0)]
        [Tooltip("The axis \"friendly name\" used to access it from code.")]
        public string Name;

        /// <summary>
        /// The axis type (mouse, gamepad, etc.).
        /// </summary>
        [EditorOrder(10)]
        [Tooltip("The axis type (mouse, gamepad, etc.).")]
        public InputAxisType Axis;

        /// <summary>
        /// Which gamepad should be used.
        /// </summary>
        [EditorOrder(20)]
        [Tooltip("Which gamepad should be used.")]
        public InputGamepadIndex Gamepad;

        /// <summary>
        /// The button to be pressed for movement in positive direction. Use <see cref="Keys.None"/> to ignore it.
        /// </summary>
        [EditorOrder(30)]
        [Tooltip("The button to be pressed for movement in positive direction. Use <see cref=\"Keys.None\"/> to ignore it.")]
        public Keys PositiveButton;

        /// <summary>
        /// The button to be pressed for movement in negative direction. Use <see cref="Keys.None"/> to ignore it.
        /// </summary>
        [EditorOrder(40)]
        [Tooltip("The button to be pressed for movement in negative direction. Use <see cref=\"Keys.None\"/> to ignore it.")]
        public Keys NegativeButton;

        /// <summary>
        /// Any positive or negative values that are less than this number will register as zero. Useful for gamepads to specify the deadzone.
        /// </summary>
        [EditorOrder(50)]
        [Tooltip("Any positive or negative values that are less than this number will register as zero. Useful for gamepads to specify the deadzone.")]
        public float DeadZone;

        /// <summary>
        /// For keyboard input, a larger value will result in faster response time (in units/s). A lower value will be more smooth. For Mouse delta the value will scale the actual mouse delta.
        /// </summary>
        [EditorOrder(60)]
        [Tooltip("For keyboard input, a larger value will result in faster response time (in units/s). A lower value will be more smooth. For Mouse delta the value will scale the actual mouse delta.")]
        public float Sensitivity;

        /// <summary>
        /// For keyboard input describes how fast will the input recenter. Speed (in units/s) that output value will rest to neutral value if not when device at rest.
        /// </summary>
        [EditorOrder(70)]
        [Tooltip("For keyboard input describes how fast will the input recenter. Speed (in units/s) that output value will rest to neutral value if not when device at rest.")]
        public float Gravity;

        /// <summary>
        /// Additional scale parameter applied to the axis value. Allows to invert it or modify the range.
        /// </summary>
        [EditorOrder(80)]
        [Tooltip("Additional scale parameter applied to the axis value. Allows to invert it or modify the range.")]
        public float Scale;

        /// <summary>
        /// If enabled, the axis value will be immediately reset to zero after it receives opposite inputs. For keyboard input only.
        /// </summary>
        [EditorOrder(90)]
        [Tooltip("If enabled, the axis value will be immediately reset to zero after it receives opposite inputs. For keyboard input only.")]
        public bool Snap;
    }
}
