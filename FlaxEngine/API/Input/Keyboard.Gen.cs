// This code was auto-generated. Do not modify it.

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace FlaxEngine
{
    /// <summary>
    /// Represents a single hardware keyboard device. Used by the Input to report raw keyboard input events.
    /// </summary>
    [Tooltip("Represents a single hardware keyboard device. Used by the Input to report raw keyboard input events.")]
    public unsafe partial class Keyboard : InputDevice
    {
        /// <inheritdoc />
        protected Keyboard() : base()
        {
        }

        /// <summary>
        /// Gets the text entered during the current frame.
        /// </summary>
        [Tooltip("The text entered during the current frame.")]
        public string InputText
        {
            get { return Internal_GetInputText(unmanagedPtr); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern string Internal_GetInputText(IntPtr obj);

        /// <summary>
        /// Gets keyboard key state.
        /// </summary>
        /// <param name="key">Key ID to check.</param>
        /// <returns>True if user holds down the key identified by id, otherwise false.</returns>
        public bool GetKey(KeyboardKeys key)
        {
            return Internal_GetKey(unmanagedPtr, key);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_GetKey(IntPtr obj, KeyboardKeys key);

        /// <summary>
        /// Gets keyboard key down state.
        /// </summary>
        /// <param name="key">Key ID to check</param>
        /// <returns>True if user starts pressing down the key, otherwise false.</returns>
        public bool GetKeyDown(KeyboardKeys key)
        {
            return Internal_GetKeyDown(unmanagedPtr, key);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_GetKeyDown(IntPtr obj, KeyboardKeys key);

        /// <summary>
        /// Gets keyboard key up state.
        /// </summary>
        /// <param name="key">Key ID to check</param>
        /// <returns>True if user releases the key, otherwise false.</returns>
        public bool GetKeyUp(KeyboardKeys key)
        {
            return Internal_GetKeyUp(unmanagedPtr, key);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_GetKeyUp(IntPtr obj, KeyboardKeys key);
    }
}
