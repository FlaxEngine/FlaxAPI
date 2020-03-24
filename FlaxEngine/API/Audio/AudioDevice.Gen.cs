// This code was auto-generated. Do not modify it.

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace FlaxEngine
{
    /// <summary>
    /// Represents a single audio device.
    /// </summary>
    [Tooltip("Represents a single audio device.")]
    public unsafe partial class AudioDevice : FlaxEngine.Object
    {
        /// <inheritdoc />
        protected AudioDevice() : base()
        {
        }

        /// <summary>
        /// The device name.
        /// </summary>
        [Tooltip("The device name.")]
        public string Name
        {
            get { return Internal_GetName(unmanagedPtr); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern string Internal_GetName(IntPtr obj);
    }
}
