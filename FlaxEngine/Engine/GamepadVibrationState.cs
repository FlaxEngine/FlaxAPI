// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

namespace FlaxEngine
{
    /// <summary>
    /// General identifiers for potential force feedback channels. These will be mapped according to the platform specific implementation.
    /// </summary>
    /// <remarks>
    /// For example, the PS4 only listens to the `large` channels and ignores the rest, while the Xbox One could
    /// map the `large` to the handle motors and `small` to the trigger motors. And iOS can map `LeftSmall` to
    /// its single motor.
    /// </remarks>
    public struct GamepadVibrationState
    {
        /// <summary>
        /// The left large motor vibration.
        /// </summary>
        public float LeftLarge;

        /// <summary>
        /// The left small motor vibration.
        /// </summary>
        public float LeftSmall;

        /// <summary>
        /// The right large motor vibration.
        /// </summary>
        public float RightLarge;

        /// <summary>
        /// The right small motor vibration.
        /// </summary>
        public float RightSmall;

        /// <summary>
        /// Initializes a new instance of the <see cref="GamepadVibrationState"/> struct.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        public GamepadVibrationState(float left, float right)
        {
            LeftLarge = left;
            LeftSmall = left;
            RightLarge = right;
            RightSmall = right;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GamepadVibrationState"/> struct.
        /// </summary>
        /// <param name="leftLarge">The left large.</param>
        /// <param name="leftSmall">The left small.</param>
        /// <param name="rightLarge">The right large.</param>
        /// <param name="rightSmall">The right small.</param>
        public GamepadVibrationState(float leftLarge, float leftSmall, float rightLarge, float rightSmall)
        {
            LeftLarge = leftLarge;
            LeftSmall = leftSmall;
            RightLarge = rightLarge;
            RightSmall = rightSmall;
        }
    }
}
