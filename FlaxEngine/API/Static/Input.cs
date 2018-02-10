////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2018 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace FlaxEngine
{
    public static partial class Input
    {
        internal static int gamepadsVersion;
        internal static Gamepad[] gamepads;
        private static float lastScanTimeAccumulatedTime;

        /// <summary>
        /// Maps keyboard, controller, or mouse inputs to a "friendly name" that will later be bound to continuous game behavior, such as movement. The inputs mapped in AxisMappings are continuously polled, even if they are just reporting that their input value.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct ActionConfig
        {
            /// <summary>
            /// The action "friendly name" used to access it from code.
            /// </summary>
            [EditorOrder(0), Tooltip("The action \"friendly name\" used to access it from code.")]
            public string Name;

            /// <summary>
            /// The trigger mode. Allows to specify when input event should be fired.
            /// </summary>
            [EditorOrder(5), Tooltip("The action trigger mode. Allows to specify when input event should be fired.")]
            public InputActionMode Mode;

            /// <summary>
            /// The keyboard key to map for this action. Use <see cref="Keys.None"/> to ignore it.
            /// </summary>
            [EditorOrder(10), Tooltip("The keyboard key to map for this action. Use None to ignore it.")]
            public Keys Key;

            /// <summary>
            /// The mouse button to map for this action. Use <see cref="FlaxEngine.MouseButton.None"/> to ignore it.
            /// </summary>
            [EditorOrder(20), Tooltip("The mouse button to map for this action. Use None to ignore it.")]
            public MouseButton MouseButton;

			/// <summary>
			/// The gamepad button to map for this action. Use <see cref="GamePadButton.None"/> to ignore it.
			/// </summary>
			[EditorOrder(30), Tooltip("The gamepad button to map for this action. Use None to ignore it.")]
            public GamePadButton GampadButton;

            /// <summary>
            /// Which gamepad should be used.
            /// </summary>
            [EditorOrder(40), Tooltip("Which gamepad should be used.")]
            public InputGamepadIndex Gamepad;
        }

        /// <summary>
        /// Maps keyboard, controller, or mouse inputs to a "friendly name" that will later be bound to continuous game behavior, such as movement. The inputs mapped in AxisMappings are continuously polled, even if they are just reporting that their input value.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct AxisConfig
        {
            /// <summary>
            /// The axis "friendly name" used to access it from code.
            /// </summary>
            [EditorOrder(0), Tooltip("The axis \"friendly name\" used to access it from code.")]
            public string Name;

            /// <summary>
            /// The axis type (mouse, gamepad, etc.).
            /// </summary>
            [EditorOrder(10), Tooltip("The axis type (mouse, gamepad, etc.).")]
            public InputAxisType Axis;

            /// <summary>
            /// Which gamepad should be used.
            /// </summary>
            [EditorOrder(20), Tooltip("Which gamepad should be used.")]
            public InputGamepadIndex Gamepad;

            /// <summary>
            /// The button to be pressed for movement in positive direction. Use <see cref="Keys.None"/> to ignore it.
            /// </summary>
            [EditorOrder(30), Tooltip("The button to be pressed for movement in positive direction. None to ignore it.")]
            public Keys PositiveButton;

            /// <summary>
            /// The button to be pressed for movement in negative direction. Use <see cref="Keys.None"/> to ignore it.
            /// </summary>
            [EditorOrder(40), Tooltip("The button to be pressed for movement in negative direction. None to ignore it.")]
            public Keys NegativeButton;

            /// <summary>
            /// Any positive or negative values that are less than this number will register as zero. Useful for gamepads to specify the deadzone.
            /// </summary>
            [EditorOrder(50), Limit(0.0f, 100.0f, 0.01f), Tooltip("Any positive or negative values that are less than this number will register as zero. Useful for gamepads  to specify the deadzone.")]
            public float DeadZone;

            /// <summary>
            /// For keyboard input, a larger value will result in faster response time (in units/s). A lower value will be more smooth. For Mouse delta the value will scale the actual mouse delta.
            /// </summary>
            [EditorOrder(60), Limit(0.0f, 10000.0f, 0.1f), Tooltip("For keyboard input, a larger value will result in faster response time (in units/s). A lower value will be more smooth. For Mouse delta the value will scale the actual mouse delta.")]
            public float Sensitivity;

            /// <summary>
            /// For keyboard input describes how fast will the input recenter. Speed (in units/s) that output value will rest to neutral value if not when device at rest.
            /// </summary>
            [EditorOrder(70), Limit(0.0f, 10000.0f, 0.1f), Tooltip("For keyboard input describes how fast will the input recenter. Speed (in units/s) that output value will rest to neutral value if not when device at rest.")]
            public float Gravity;

            /// <summary>
            /// Additional scale parameter applied to the axis value. Allows to invert it or modify the range.
            /// </summary>
            [EditorOrder(80), Limit(-1000.0f, 1000.0f, 0.01f), Tooltip("Additional scale parameter applied to the axis value. Allows to invert it or modify the range.")]
            public float Scale;

            /// <summary>
            /// If enabled, the axis value will be immediately reset to zero after it receives opposite inputs. For keyboard input only.
            /// </summary>
            [EditorOrder(90), Tooltip("If enabled, the axis value will be immediately reset to zero after it receives opposite inputs. For keyboard input only.")]
            public bool Snap;
        }

        /// <summary>
        /// Maps a discrete button or key press events to a "friendly name" that will later be bound to event-driven behavior. The end effect is that pressing (and/or releasing) a key, mouse button, or keypad button.
        /// </summary>
        /// <remarks>
        /// Allocates the memory on get. Use <see cref="GetActionMappingsCount"/> and <see cref="GetActionMapping"/> to reduce dynamic memory allocations.
        /// </remarks>
        public static ActionConfig[] ActionMappings
        {
            get
            {
                int count = GetActionMappingsCount();
                var result = new ActionConfig[count];
                for (int i = 0; i < count; i++)
                {
                    GetActionMapping(i, out result[i]);
                }
                return result;
            }
        }

        /// <summary>
        /// Maps keyboard, controller, or mouse inputs to a "friendly name" that will later be bound to continuous game behavior, such as movement. The inputs mapped in AxisMappings are continuously polled, even if they are just reporting that their input value.
        /// </summary>
        /// <remarks>
        /// Allocates the memory on get. Use <see cref="GetAxisMappingsCount"/> and <see cref="GetAxisMapping"/> to reduce dynamic memory allocations.
        /// </remarks>
        public static AxisConfig[] AxisMappings
        {
            get
            {
                int count = GetAxisMappingsCount();
                var result = new AxisConfig[count];
                for (int i = 0; i < count; i++)
                {
                    GetAxisMapping(i, out result[i]);
                }
                return result;
            }
        }

        /// <summary>
        /// Gets the amount of assigned action mappings.
        /// </summary>
        /// <seealso cref="ActionMappings"/>
        /// <returns>The amount of mappings.</returns>
#if !UNIT_TEST_COMPILANT
        [MethodImpl(MethodImplOptions.InternalCall)]
        public static extern int GetActionMappingsCount();
#else
        public static int GetActionMappingsCount()
        {
            return 0;
        }
#endif
        /// <summary>
        /// Gets the amount of assigned axis mappings.
        /// </summary>
        /// <seealso cref="AxisMappings"/>
        /// <returns>The amount of mappings.</returns>
#if !UNIT_TEST_COMPILANT
        [MethodImpl(MethodImplOptions.InternalCall)]
        public static extern int GetAxisMappingsCount();
#else
        public static int GetAxisMappingsCount()
        {
            return 0;
        }
#endif

        /// <summary>
        /// Gets the action mapping configuration.
        /// </summary>
        /// <param name="index">The action mapping index.</param>
        /// <param name="result">The result configuration.</param>
        /// <seealso cref="ActionMappings"/>
#if !UNIT_TEST_COMPILANT
        [MethodImpl(MethodImplOptions.InternalCall)]
        public static extern void GetActionMapping(int index, out ActionConfig result);
#else
        public static void GetActionMapping(int index, out ActionConfig result)
        {
            result = new ActionConfig();
        }
#endif

        /// <summary>
        /// Gets the axis mapping configuration.
        /// </summary>
        /// <param name="index">The axis mapping index.</param>
        /// <param name="result">The result configuration.</param>
        /// <seealso cref="AxisMappings"/>
#if !UNIT_TEST_COMPILANT
        [MethodImpl(MethodImplOptions.InternalCall)]
        public static extern void GetAxisMapping(int index, out AxisConfig result);
#else
        public static void GetAxisMapping(int index, out AxisConfig result)
        {
            result = new AxisConfig();
        }
#endif

        /// <summary>
        /// Sets the action mapping configuration. Use <see cref="ActionMappings"/> to get the current config.
        /// </summary>
        /// <param name="value">The value to set.</param>
        /// <seealso cref="ActionMappings"/>
#if !UNIT_TEST_COMPILANT
        [MethodImpl(MethodImplOptions.InternalCall)]
        public static extern void SetActionMappings(ActionConfig[] value);
#else
        public static void SetActionMapping(ActionConfig[] value)
        {
        }
#endif

        /// <summary>
        /// Sets the axis mapping configuration. Use <see cref="AxisMappings"/> to get the current config.
        /// </summary>
        /// <param name="value">The value to set.</param>
        /// <seealso cref="AxisMappings"/>
#if !UNIT_TEST_COMPILANT
        [MethodImpl(MethodImplOptions.InternalCall)]
        public static extern void SetAxisMappings(AxisConfig[] value);
#else
        public static void SetAxisMapping(AxisConfig[] value)
        {
        }
#endif

        /// <summary>
        /// Gets the value of the virtual action identified by name. Use <see cref="ActionMappings"/> to get the current config.
        /// </summary>
        /// <param name="name">The action name.</param>
        /// <returns>True if action has been triggered in the current frame (e.g. button pressed), otherwise false.</returns>
        /// <seealso cref="ActionMappings"/>
#if !UNIT_TEST_COMPILANT
        [MethodImpl(MethodImplOptions.InternalCall)]
        public static extern bool GetAction(string name);
#else
        public static bool GetAction(string name)
        {
            return false;
        }
#endif

        /// <summary>
        /// Gets the value of the virtual axis identified by name. Use <see cref="AxisMappings"/> to get the current config.
        /// </summary>
        /// <param name="name">The action name.</param>
        /// <returns>The current axis value (e.g for gamepads it's in the range -1..1). Value is smoothed to redue artifacts.</returns>
        /// <seealso cref="AxisMappings"/>
#if !UNIT_TEST_COMPILANT
        [MethodImpl(MethodImplOptions.InternalCall)]
        public static extern float GetAxis(string name);
#else
        public static float GetAction(string name)
        {
            return 0.0f;
        }
#endif

        /// <summary>
        /// Gets the raw value of the virtual axis identified by name with no smoothing filtering applied. Use <see cref="AxisMappings"/> to get the current config.
        /// </summary>
        /// <param name="name">The action name.</param>
        /// <returns>The current axis value (e.g for gamepads it's in the range -1..1). No smoothing applied.</returns>
        /// <seealso cref="AxisMappings"/>
#if !UNIT_TEST_COMPILANT
        [MethodImpl(MethodImplOptions.InternalCall)]
        public static extern float GetAxisRaw(string name);
#else
        public static float GetAxisRaw(string name)
        {
            return 0.0f;
        }
#endif

        /// <summary>
        /// Event fired when virtual input action is triggered. Called before scripts update. See <see cref="ActionMappings"/> to edit configuration.
        /// </summary>
        /// <seealso cref="InputEvent"/>
        public static event Action<string> ActionTriggered;

        /// <summary>
        /// The gamepads changed event. Called when new gamepad device gets disconnected or added. Called always on main thread before the scripts update or during <see cref="ScanGamepads"/> call.
        /// </summary>
        public static event Action GamepadsChanged;

        /// <summary>
        /// Gets the gamepad devices detected by the engine.
        /// </summary>
        public static Gamepad[] Gamepads
        {
            get
            {
                if (gamepads == null)
                {
                    int count = Internal_GetGamepadsCount();
                    gamepads = new Gamepad[count];
                    for (int i = 0; i < count; i++)
                        gamepads[i] = new Gamepad(i, gamepadsVersion);
                }
                return gamepads;
            }
        }

        /// <summary>
        /// The automatic gamepads scanning interval in seconds. Calls <see cref="ScanGamepads"/>. Use value equal to 0 or lower to disable that feature. The default value is 2s.
        /// </summary>
        public static float AutoGamepadsScanInterval = 2.0f;

        /// <summary>
        /// Scans the connected gamepad devices to find the new ones.
        /// </summary>
#if !UNIT_TEST_COMPILANT
        [MethodImpl(MethodImplOptions.InternalCall)]
        public static extern void ScanGamepads();
#else
        public static void ScanGamepads()
        {
        }
#endif

        internal static void Init()
        {
            Scripting.Update += OnUpdate;
            lastScanTimeAccumulatedTime = -1.0f;
        }

        internal static void OnUpdate()
        {
            if (AutoGamepadsScanInterval > 0)
            {
                var dt = Time.DeltaTime;
                lastScanTimeAccumulatedTime += dt;
                if (lastScanTimeAccumulatedTime >= AutoGamepadsScanInterval)
                {
                    lastScanTimeAccumulatedTime = 0.0f;
                    ScanGamepads();
                }
            }
            else
            {
                lastScanTimeAccumulatedTime = 0;
            }
        }

        internal static void Internal_ActionTriggered(string name)
        {
            ActionTriggered?.Invoke(name);
        }

        internal static void Internal_GamepadsChanged()
        {
            gamepadsVersion++;
            gamepads = null;
            GamepadsChanged?.Invoke();
        }

        #region Internal Calls

#if !UNIT_TEST_COMPILANT
        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern int Internal_GetGamepadsCount();
#endif

        #endregion
    }
}
