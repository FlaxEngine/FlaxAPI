// Flax Engine scripting API

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
    }
}
