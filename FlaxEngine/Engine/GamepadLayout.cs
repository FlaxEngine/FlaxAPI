// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System;
using System.Collections.Generic;

namespace FlaxEngine
{
    /// <summary>
    /// Gamepad buttons and axis mapping description.
    /// Allows converting input from the different gamepads into a universal format (see <see cref="Gamepad.ButtonTypes"/> and <see cref="Gamepad.AxisTypes"/>).
    /// </summary>
    public struct GamepadLayout
    {
        /// <summary>
        /// The buttons mapping. Index by gamepad button id from 0 to 31 (see <see cref="Gamepad.ButtonTypes"/>).
        /// </summary>
        public GamePadButton[] Buttons;

        /// <summary>
        /// The axis mapping. Index by gamepad axis id from 0 to 5 (see <see cref="Gamepad.AxisTypes"/>).
        /// </summary>
        public GamePadAxis[] Axis;

        /// <summary>
        /// The axis ranges mapping (X is scale, Y is offset. Eg. mappedVal = X * value + Y). It allows to invert any axis or map axis range.
        /// </summary>
        public Vector2[] AxisMap;

        /// <summary>
        /// Inverts the axis.
        /// </summary>
        /// <param name="axis">The axis.</param>
        public void InvertAxis(Gamepad.AxisTypes axis)
        {
            AxisMap[(int)axis].X *= -1;
        }

        /// <summary>
        /// Creates the default gamepad layout from the input buttons/axis mapping.
        /// </summary>
        /// <returns>The gamepad layout</returns>
        public static GamepadLayout CreateMapping()
        {
            GamepadLayout layout;
            layout.Buttons = new GamePadButton[32];
            layout.Axis = new GamePadAxis[6];
            layout.AxisMap = new Vector2[6];

            layout.Buttons[(int)Gamepad.ButtonTypes.A] = GamePadButton.A;
            layout.Buttons[(int)Gamepad.ButtonTypes.B] = GamePadButton.B;
            layout.Buttons[(int)Gamepad.ButtonTypes.X] = GamePadButton.X;
            layout.Buttons[(int)Gamepad.ButtonTypes.Y] = GamePadButton.Y;
            layout.Buttons[(int)Gamepad.ButtonTypes.LeftShoulder] = GamePadButton.LeftShoulder;
            layout.Buttons[(int)Gamepad.ButtonTypes.RightShoulder] = GamePadButton.RightShoulder;
            layout.Buttons[(int)Gamepad.ButtonTypes.Back] = GamePadButton.Back;
            layout.Buttons[(int)Gamepad.ButtonTypes.Start] = GamePadButton.Start;
            layout.Buttons[(int)Gamepad.ButtonTypes.LeftThumbstick] = GamePadButton.LeftThumb;
            layout.Buttons[(int)Gamepad.ButtonTypes.RightThumbstick] = GamePadButton.RightThumb;
            layout.Buttons[(int)Gamepad.ButtonTypes.LeftTrigger] = GamePadButton.LeftTrigger;
            layout.Buttons[(int)Gamepad.ButtonTypes.RightTrigger] = GamePadButton.RightTrigger;
            layout.Buttons[(int)Gamepad.ButtonTypes.DpadUp] = GamePadButton.DPadUp;
            layout.Buttons[(int)Gamepad.ButtonTypes.DpadDown] = GamePadButton.DPadDown;
            layout.Buttons[(int)Gamepad.ButtonTypes.DpadLeft] = GamePadButton.DPadLeft;
            layout.Buttons[(int)Gamepad.ButtonTypes.DpadRight] = GamePadButton.DPadRight;
            layout.Buttons[(int)Gamepad.ButtonTypes.LeftStickUp] = GamePadButton.LeftStickUp;
            layout.Buttons[(int)Gamepad.ButtonTypes.LeftStickDown] = GamePadButton.LeftStickDown;
            layout.Buttons[(int)Gamepad.ButtonTypes.LeftStickLeft] = GamePadButton.LeftStickLeft;
            layout.Buttons[(int)Gamepad.ButtonTypes.LeftStickRight] = GamePadButton.LeftStickRight;
            layout.Buttons[(int)Gamepad.ButtonTypes.RightStickUp] = GamePadButton.RightStickUp;
            layout.Buttons[(int)Gamepad.ButtonTypes.RightStickDown] = GamePadButton.RightStickDown;
            layout.Buttons[(int)Gamepad.ButtonTypes.RightStickLeft] = GamePadButton.RightStickLeft;
            layout.Buttons[(int)Gamepad.ButtonTypes.RightStickRight] = GamePadButton.RightStickRight;
            layout.Buttons[(int)Gamepad.ButtonTypes.DpadUpLeft] = GamePadButton.None;
            layout.Buttons[(int)Gamepad.ButtonTypes.DpadUpRight] = GamePadButton.None;
            layout.Buttons[(int)Gamepad.ButtonTypes.DpadDownLeft] = GamePadButton.None;
            layout.Buttons[(int)Gamepad.ButtonTypes.DpadDownRight] = GamePadButton.None;
            layout.Buttons[(int)Gamepad.ButtonTypes.Button1] = GamePadButton.None;
            layout.Buttons[(int)Gamepad.ButtonTypes.Button2] = GamePadButton.None;
            layout.Buttons[(int)Gamepad.ButtonTypes.Button3] = GamePadButton.None;
            layout.Buttons[(int)Gamepad.ButtonTypes.Button4] = GamePadButton.None;

            layout.Axis[(int)Gamepad.AxisTypes.LeftStickX] = GamePadAxis.LeftStickX;
            layout.Axis[(int)Gamepad.AxisTypes.LeftStickY] = GamePadAxis.LeftStickY;
            layout.Axis[(int)Gamepad.AxisTypes.RightStickX] = GamePadAxis.RightStickX;
            layout.Axis[(int)Gamepad.AxisTypes.RightStickY] = GamePadAxis.RightStickY;
            layout.Axis[(int)Gamepad.AxisTypes.LeftTrigger] = GamePadAxis.LeftTrigger;
            layout.Axis[(int)Gamepad.AxisTypes.RightTrigger] = GamePadAxis.RightTrigger;

            layout.AxisMap[0] = Vector2.UnitX;
            layout.AxisMap[1] = Vector2.UnitX;
            layout.AxisMap[2] = Vector2.UnitX;
            layout.AxisMap[3] = Vector2.UnitX;
            layout.AxisMap[4] = Vector2.UnitX;
            layout.AxisMap[5] = Vector2.UnitX;

            return layout;
        }

        /// <summary>
        /// Creates the gamepad layout from the input buttons/axis mapping.
        /// </summary>
        /// <param name="buttons">The buttons.</param>
        /// <param name="axis">The axis.</param>
        /// <returns>The gamepad layout</returns>
        public static GamepadLayout CreateMapping(Dictionary<Gamepad.ButtonTypes, GamePadButton> buttons, Dictionary<Gamepad.AxisTypes, GamePadAxis> axis)
        {
            if (buttons == null)
                throw new ArgumentNullException(nameof(buttons));
            if (axis == null)
                throw new ArgumentNullException(nameof(axis));

            GamepadLayout layout = CreateMapping();

            foreach (var e in buttons)
            {
                layout.Buttons[(int)e.Key] = e.Value;
            }
            foreach (var e in axis)
            {
                layout.Axis[(int)e.Key] = e.Value;
            }

            return layout;
        }
    }
}
