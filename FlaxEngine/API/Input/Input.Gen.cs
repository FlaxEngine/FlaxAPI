// This code was auto-generated. Do not modify it.

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace FlaxEngine
{
    /// <summary>
    /// The user input handling service.
    /// </summary>
    [Tooltip("The user input handling service.")]
    public static unsafe partial class Input
    {
        /// <summary>
        /// Gets the mouse (null if platform does not support mouse or it is not connected).
        /// </summary>
        [Tooltip("The mouse (null if platform does not support mouse or it is not connected).")]
        public static Mouse Mouse
        {
            get { return Internal_GetMouse(); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern Mouse Internal_GetMouse();

        /// <summary>
        /// Gets the keyboard (null if platform does not support keyboard or it is not connected).
        /// </summary>
        [Tooltip("The keyboard (null if platform does not support keyboard or it is not connected).")]
        public static Keyboard Keyboard
        {
            get { return Internal_GetKeyboard(); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern Keyboard Internal_GetKeyboard();

        /// <summary>
        /// Gets the gamepads.
        /// </summary>
        [Tooltip("The gamepads.")]
        public static Gamepad[] Gamepads
        {
            get { return Internal_GetGamepads(typeof(Gamepad)); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern Gamepad[] Internal_GetGamepads(System.Type resultArrayItemType0);

        /// <summary>
        /// Maps a discrete button or key press events to a "friendly name" that will later be bound to event-driven behavior. The end effect is that pressing (and/or releasing) a key, mouse button, or keypad button.
        /// </summary>
        [Tooltip("Maps a discrete button or key press events to a \"friendly name\" that will later be bound to event-driven behavior. The end effect is that pressing (and/or releasing) a key, mouse button, or keypad button.")]
        public static ActionConfig[] ActionMappings
        {
            get { return Internal_GetActionMappings(typeof(ActionConfig)); }
            set { Internal_SetActionMappings(value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern ActionConfig[] Internal_GetActionMappings(System.Type resultArrayItemType0);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetActionMappings(ActionConfig[] value);

        /// <summary>
        /// Maps keyboard, controller, or mouse inputs to a "friendly name" that will later be bound to continuous game behavior, such as movement. The inputs mapped in AxisMappings are continuously polled, even if they are just reporting that their input value.
        /// </summary>
        [Tooltip("Maps keyboard, controller, or mouse inputs to a \"friendly name\" that will later be bound to continuous game behavior, such as movement. The inputs mapped in AxisMappings are continuously polled, even if they are just reporting that their input value.")]
        public static AxisConfig[] AxisMappings
        {
            get { return Internal_GetAxisMappings(typeof(AxisConfig)); }
            set { Internal_SetAxisMappings(value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern AxisConfig[] Internal_GetAxisMappings(System.Type resultArrayItemType0);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetAxisMappings(AxisConfig[] value);

        /// <summary>
        /// Gets the gamepads count.
        /// </summary>
        [Tooltip("The gamepads count.")]
        public static int GamepadsCount
        {
            get { return Internal_GetGamepadsCount(); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern int Internal_GetGamepadsCount();

        /// <summary>
        /// Gets the text entered during the current frame (Unicode).
        /// </summary>
        [Tooltip("The text entered during the current frame (Unicode).")]
        public static string InputText
        {
            get { return Internal_GetInputText(); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern string Internal_GetInputText();

        /// <summary>
        /// Gets or sets the mouse position in game window coordinates.
        /// </summary>
        [Tooltip("The mouse position in game window coordinates.")]
        public static Vector2 MousePosition
        {
            get { Internal_GetMousePosition(out var resultAsRef); return resultAsRef; }
            set { Internal_SetMousePosition(ref value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_GetMousePosition(out Vector2 resultAsRef);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetMousePosition(ref Vector2 position);

        /// <summary>
        /// Gets or sets the mouse position in screen-space coordinates.
        /// </summary>
        [Tooltip("The mouse position in screen-space coordinates.")]
        public static Vector2 MouseScreenPosition
        {
            get { Internal_GetMouseScreenPosition(out var resultAsRef); return resultAsRef; }
            set { Internal_SetMouseScreenPosition(ref value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_GetMouseScreenPosition(out Vector2 resultAsRef);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetMouseScreenPosition(ref Vector2 position);

        /// <summary>
        /// Gets the mouse position change during the last frame.
        /// </summary>
        [Tooltip("The mouse position change during the last frame.")]
        public static Vector2 MousePositionDelta
        {
            get { Internal_GetMousePositionDelta(out var resultAsRef); return resultAsRef; }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_GetMousePositionDelta(out Vector2 resultAsRef);

        /// <summary>
        /// Gets the mouse wheel change during the last frame.
        /// </summary>
        [Tooltip("The mouse wheel change during the last frame.")]
        public static float MouseScrollDelta
        {
            get { return Internal_GetMouseScrollDelta(); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern float Internal_GetMouseScrollDelta();

        /// <summary>
        /// Gets the gamepads count.
        /// </summary>
        /// <param name="index">The gamepad index.</param>
        /// <returns>The gamepad device or null if index is invalid.</returns>
        public static Gamepad GetGamepad(int index)
        {
            return Internal_GetGamepad(index);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern Gamepad Internal_GetGamepad(int index);

        /// <summary>
        /// Gets the key state (true if key is being pressed during this frame).
        /// </summary>
        /// <param name="key">Key ID to check</param>
        /// <returns>True while the user holds down the key identified by id</returns>
        public static bool GetKey(KeyboardKeys key)
        {
            return Internal_GetKey(key);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_GetKey(KeyboardKeys key);

        /// <summary>
        /// Gets the key 'down' state (true if key was pressed in this frame).
        /// </summary>
        /// <param name="key">Key ID to check</param>
        /// <returns>True during the frame the user starts pressing down the key</returns>
        public static bool GetKeyDown(KeyboardKeys key)
        {
            return Internal_GetKeyDown(key);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_GetKeyDown(KeyboardKeys key);

        /// <summary>
        /// Gets the key 'up' state (true if key was released in this frame).
        /// </summary>
        /// <param name="key">Key ID to check</param>
        /// <returns>True during the frame the user releases the key</returns>
        public static bool GetKeyUp(KeyboardKeys key)
        {
            return Internal_GetKeyUp(key);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_GetKeyUp(KeyboardKeys key);

        /// <summary>
        /// Gets the mouse button state.
        /// </summary>
        /// <param name="button">Mouse button to check</param>
        /// <returns>True while the user holds down the button</returns>
        public static bool GetMouseButton(MouseButton button)
        {
            return Internal_GetMouseButton(button);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_GetMouseButton(MouseButton button);

        /// <summary>
        /// Gets the mouse button down state.
        /// </summary>
        /// <param name="button">Mouse button to check</param>
        /// <returns>True during the frame the user starts pressing down the button</returns>
        public static bool GetMouseButtonDown(MouseButton button)
        {
            return Internal_GetMouseButtonDown(button);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_GetMouseButtonDown(MouseButton button);

        /// <summary>
        /// Gets the mouse button up state.
        /// </summary>
        /// <param name="button">Mouse button to check</param>
        /// <returns>True during the frame the user releases the button</returns>
        public static bool GetMouseButtonUp(MouseButton button)
        {
            return Internal_GetMouseButtonUp(button);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_GetMouseButtonUp(MouseButton button);

        /// <summary>
        /// Gets the gamepad axis value.
        /// </summary>
        /// <param name="gamepadIndex">The gamepad index</param>
        /// <param name="axis">Gamepad axis to check</param>
        /// <returns>Axis value.</returns>
        public static float GetGamepadAxis(int gamepadIndex, GamepadAxis axis)
        {
            return Internal_GetGamepadAxis(gamepadIndex, axis);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern float Internal_GetGamepadAxis(int gamepadIndex, GamepadAxis axis);

        /// <summary>
        /// Gets the gamepad button state (true if being pressed during the current frame).
        /// </summary>
        /// <param name="gamepadIndex">The gamepad index</param>
        /// <param name="button">Gamepad button to check</param>
        /// <returns>True if user holds down the button, otherwise false.</returns>
        public static bool GetGamepadButton(int gamepadIndex, GamepadButton button)
        {
            return Internal_GetGamepadButton(gamepadIndex, button);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_GetGamepadButton(int gamepadIndex, GamepadButton button);

        /// <summary>
        /// Gets the gamepad button down state (true if was pressed during the current frame).
        /// </summary>
        /// <param name="gamepadIndex">The gamepad index</param>
        /// <param name="button">Gamepad button to check</param>
        /// <returns>True if user starts pressing down the button, otherwise false.</returns>
        public static bool GetGamepadButtonDown(int gamepadIndex, GamepadButton button)
        {
            return Internal_GetGamepadButtonDown(gamepadIndex, button);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_GetGamepadButtonDown(int gamepadIndex, GamepadButton button);

        /// <summary>
        /// Gets the gamepad button up state (true if was released during the current frame).
        /// </summary>
        /// <param name="gamepadIndex">The gamepad index</param>
        /// <param name="button">Gamepad button to check</param>
        /// <returns>True if user releases the button, otherwise false.</returns>
        public static bool GetGamepadButtonUp(int gamepadIndex, GamepadButton button)
        {
            return Internal_GetGamepadButtonUp(gamepadIndex, button);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_GetGamepadButtonUp(int gamepadIndex, GamepadButton button);

        /// <summary>
        /// Gets the gamepad axis value.
        /// </summary>
        /// <param name="gamepad">The gamepad</param>
        /// <param name="axis">Gamepad axis to check</param>
        /// <returns>Axis value.</returns>
        public static float GetGamepadAxis(InputGamepadIndex gamepad, GamepadAxis axis)
        {
            return Internal_GetGamepadAxis1(gamepad, axis);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern float Internal_GetGamepadAxis1(InputGamepadIndex gamepad, GamepadAxis axis);

        /// <summary>
        /// Gets the gamepad button state (true if being pressed during the current frame).
        /// </summary>
        /// <param name="gamepad">The gamepad</param>
        /// <param name="button">Gamepad button to check</param>
        /// <returns>True if user holds down the button, otherwise false.</returns>
        public static bool GetGamepadButton(InputGamepadIndex gamepad, GamepadButton button)
        {
            return Internal_GetGamepadButton1(gamepad, button);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_GetGamepadButton1(InputGamepadIndex gamepad, GamepadButton button);

        /// <summary>
        /// Gets the gamepad button down state (true if was pressed during the current frame).
        /// </summary>
        /// <param name="gamepad">The gamepad</param>
        /// <param name="button">Gamepad button to check</param>
        /// <returns>True if user starts pressing down the button, otherwise false.</returns>
        public static bool GetGamepadButtonDown(InputGamepadIndex gamepad, GamepadButton button)
        {
            return Internal_GetGamepadButtonDown1(gamepad, button);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_GetGamepadButtonDown1(InputGamepadIndex gamepad, GamepadButton button);

        /// <summary>
        /// Gets the gamepad button up state (true if was released during the current frame).
        /// </summary>
        /// <param name="gamepad">The gamepad</param>
        /// <param name="button">Gamepad button to check</param>
        /// <returns>True if user releases the button, otherwise false.</returns>
        public static bool GetGamepadButtonUp(InputGamepadIndex gamepad, GamepadButton button)
        {
            return Internal_GetGamepadButtonUp1(gamepad, button);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_GetGamepadButtonUp1(InputGamepadIndex gamepad, GamepadButton button);

        /// <summary>
        /// Gets the value of the virtual action identified by name. Use <see cref="ActionMappings"/> to get the current config.
        /// </summary>
        /// <param name="name">The action name.</param>
        /// <returns>True if action has been triggered in the current frame (e.g. button pressed), otherwise false.</returns>
        /// <seealso cref="ActionMappings"/>
        public static bool GetAction(string name)
        {
            return Internal_GetAction(name);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_GetAction(string name);

        /// <summary>
        /// Gets the value of the virtual axis identified by name. Use <see cref="AxisMappings"/> to get the current config.
        /// </summary>
        /// <param name="name">The action name.</param>
        /// <returns>The current axis value (e.g for gamepads it's in the range -1..1). Value is smoothed to reduce artifacts.</returns>
        /// <seealso cref="AxisMappings"/>
        public static float GetAxis(string name)
        {
            return Internal_GetAxis(name);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern float Internal_GetAxis(string name);

        /// <summary>
        /// Gets the raw value of the virtual axis identified by name with no smoothing filtering applied. Use <see cref="AxisMappings"/> to get the current config.
        /// </summary>
        /// <param name="name">The action name.</param>
        /// <returns>The current axis value (e.g for gamepads it's in the range -1..1). No smoothing applied.</returns>
        /// <seealso cref="AxisMappings"/>
        public static float GetAxisRaw(string name)
        {
            return Internal_GetAxisRaw(name);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern float Internal_GetAxisRaw(string name);
    }
}
