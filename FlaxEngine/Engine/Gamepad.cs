// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System;
using System.Runtime.CompilerServices;

#pragma warning disable 1591

namespace FlaxEngine
{
    /// <summary>
    /// Represents a single hardware gamepad device. Used by the Input to report raw gamepad input events.
    /// </summary>
    public sealed class Gamepad
    {
        /// <summary>
        /// Gamepad button codes.
        /// </summary>
        public enum ButtonTypes
        {
            A = 0,
            B = 1,
            X = 2,
            Y = 3,
            LeftShoulder = 4,
            RightShoulder = 5,
            Back = 6,
            Start = 7,
            LeftThumbstick = 8,
            RightThumbstick = 9,
            LeftTrigger = 10,
            RightTrigger = 11,
            DpadUp = 12,
            DpadDown = 13,
            DpadLeft = 14,
            DpadRight = 15,
            LeftStickUp = 16,
            LeftStickDown = 17,
            LeftStickLeft = 18,
            LeftStickRight = 19,
            RightStickUp = 20,
            RightStickDown = 21,
            RightStickLeft = 22,
            RightStickRight = 23,
            DpadUpLeft = 24,
            DpadUpRight = 25,
            DpadDownLeft = 26,
            DpadDownRight = 27,
            Button1 = 28,
            Button2 = 29,
            Button3 = 30,
            Button4 = 31,
        }

        /// <summary>
        /// Gamepad axis codes.
        /// </summary>
        public enum AxisTypes
        {
            LeftStickX = 0,
            LeftStickY = 1,
            RightStickX = 2,
            RightStickY = 3,
            LeftTrigger = 4,
            RightTrigger = 5,
        }

        /// <summary>
        /// The universal gamepad state description. All hardware gamepad device handlers should map input to match this structure.
        /// Later on, each gamepad may use individual layout for a game.
        /// </summary>
        public struct State
        {
            /// <summary>
            /// The buttons state (pressed if true).
            /// </summary>
            public bool[] Buttons;

            /// <summary>
            /// The axis state (normalized value).
            /// </summary>
            public float[] Axis;
        }

        private readonly int _index;
        private readonly int _version;

        internal Gamepad(int index, int version)
        {
            _index = index;
            _version = version;
        }

        /// <summary>
        /// Gets the device identifier.
        /// </summary>
        public Guid ProductID
        {
            get
            {
                Guid id;
                if (_version != Input.gamepadsVersion)
                    throw new AccessViolationException();
                Internal_GetProductID(_index, out id);
                return id;
            }
        }

        /// <summary>
        /// Gets the device name (provided by the OS).
        /// </summary>
        public string Name
        {
            get
            {
                if (_version != Input.gamepadsVersion)
                    throw new AccessViolationException();
                return Internal_GetName(_index);
            }
        }

        /// <summary>
        /// Sets the gamepad vibration.
        /// </summary>
        /// <param name="state">The state.</param>
        public void SetVibration(GamepadVibrationState state)
        {
            if (_version != Input.gamepadsVersion)
                throw new AccessViolationException();
            Internal_SetVibration(_index, state.LeftLarge, state.LeftSmall, state.RightLarge, state.LeftSmall);
        }

        /// <summary>
        /// Gets the raw device state.
        /// </summary>
        /// <param name="state">The state.</param>
        public void GetState(out State state)
        {
            if (_version != Input.gamepadsVersion)
                throw new AccessViolationException();
            state = new State
            {
                Buttons = new bool[32],
                Axis = new float[6]
            };
            Internal_GetState(_index, state.Buttons, state.Axis);
        }

        /// <summary>
        /// Gets the gamepad axis value.
        /// </summary>
        /// <param name="axis">Gamepad axis to check</param>
        /// <returns>Axis value.</returns>
        public float GetAxis(GamePadAxis axis)
        {
            if (_version != Input.gamepadsVersion)
                throw new AccessViolationException();
            return Internal_GetAxis(_index, axis);
        }

        /// <summary>
        /// Gets the gamepad button state (true if being pressed during the current frame).
        /// </summary>
        /// <param name="button">Gamepad button to check</param>
        /// <returns>True if user holds down the button, otherwise false.</returns>
        public bool GetButton(GamePadButton button)
        {
            if (_version != Input.gamepadsVersion)
                throw new AccessViolationException();
            int state = Internal_GetButton(_index, button);
            return (state & 1) != 0;
        }

        /// <summary>
        /// Gets the gamepad button down state (true if was pressed during the current frame).
        /// </summary>
        /// <param name="button">Gamepad button to check</param>
        /// <returns>True if user starts pressing down the button, otherwise false.</returns>
        public bool GetButtonDown(GamePadButton button)
        {
            if (_version != Input.gamepadsVersion)
                throw new AccessViolationException();
            int state = Internal_GetButton(_index, button);
            return (state & 1) != 0 && (state & 2) == 0;
        }

        /// <summary>
        /// Gets the gamepad button up state (true if was released during the current frame).
        /// </summary>
        /// <param name="button">Gamepad button to check</param>
        /// <returns>True if user releases the button, otherwise false.</returns>
        public bool GetButtonUp(GamePadButton button)
        {
            if (_version != Input.gamepadsVersion)
                throw new AccessViolationException();
            int state = Internal_GetButton(_index, button);
            return (state & 1) == 0 && (state & 2) != 0;
        }

        /// <summary>
        /// Sets the gamepad buttons/axis layout. Use <see cref="GamepadLayout.CreateMapping()"/> to generate proper layout and override the default logic.
        /// </summary>
        /// <param name="layout">The layout.</param>
        public void SetLayout(GamepadLayout layout)
        {
            if (_version != Input.gamepadsVersion)
                throw new AccessViolationException();
            if (layout.Buttons == null || layout.Axis == null || layout.AxisMap == null)
                throw new ArgumentNullException();
            Internal_SetLayout(_index, layout.Buttons, layout.Axis, layout.AxisMap);
        }

        #region Internal Calls

#if !UNIT_TEST_COMPILANT
        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_GetProductID(int index, out Guid id);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern string Internal_GetName(int index);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetVibration(int index, float l1, float l2, float r1, float r2);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_GetState(int index, bool[] buttons, float[] axis);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern int Internal_GetButton(int index, GamePadButton button);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern float Internal_GetAxis(int index, GamePadAxis axis);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern float Internal_SetLayout(int index, GamePadButton[] buttons, GamePadAxis[] axis, Vector2[] axisMapping);
#endif

        #endregion
    }
}
