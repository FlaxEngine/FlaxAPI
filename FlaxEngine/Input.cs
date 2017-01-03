// Flax Engine scripting API

using System.Runtime.CompilerServices;

namespace FlaxEngine
{
    /// <summary>
    /// The interface to get input information from Flax.
    /// </summary>
    public static class Input
    {
        /// <summary>
        /// The current mouse position in pixel coordinates.
        /// </summary>
        public static Vector2 MousePosition
        {
            get
            {
                Vector2 result;
                Internal_GetMousePosition(out result);
                return result;
            }
        }

        // TODO: maybe reduce amount of calls and let GetKey and GetMosueButtons functions be internal calls?

        /// <summary>
        /// Get keyboard key state
        /// </summary>
        /// <param name="key">Key to check</param>
        /// <returns>True while the user holds down the key identified by id</returns>
        public static bool GetKey(KeyCode key)
        {
            return Internal_GetKey((byte)key);
        }

        /// <summary>
        /// Get keyboard key down state
        /// </summary>
        /// <param name="key">Key to check</param>
        /// <returns>True during the frame the user starts pressing down the key</returns>
        public static bool GetKeyDown(KeyCode key)
        {
            return Internal_GetKeyDown((byte)key);
        }

        /// <summary>
        /// Get keyboard key up state
        /// </summary>
        /// <param name="key">Key to check</param>
        /// <returns>True during the frame the user releases the key</returns>
        public static bool GetKeyUp(KeyCode key)
        {
            return Internal_GetKeyUp((byte)key);
        }

        /// <summary>
        /// Get mouse button state
        /// </summary>
        /// <param name="button">Mouse button to check</param>
        /// <returns>True while the user holds down the button</returns>
        public static bool GetMouseButton(MouseButtons button)
        {
            return Internal_GetMouseButton((int)button);
        }

        /// <summary>
        /// Get mouse button down state
        /// </summary>
        /// <param name="button">Mouse button to check</param>
        /// <returns>True during the frame the user starts pressing down the button</returns>
        public static bool GetMouseButtonDown(MouseButtons button)
        {
            return Internal_GetMouseButtonDown((int)button);
        }

        /// <summary>
        /// Get mouse button up state
        /// </summary>
        /// <param name="button">Mouse button to check</param>
        /// <returns>True during the frame the user releases the button</returns>
        public static bool GetMouseButtonUp(MouseButtons button)
        {
            return Internal_GetMouseButtonUp((int)button);
        }

        #region Internal Calls

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_GetMousePosition(out Vector2 result);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_GetKey(byte key);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_GetKeyDown(byte key);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_GetKeyUp(byte key);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_GetMouseButton(int button);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_GetMouseButtonDown(int button);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_GetMouseButtonUp(int button);

        #endregion
    }
}
