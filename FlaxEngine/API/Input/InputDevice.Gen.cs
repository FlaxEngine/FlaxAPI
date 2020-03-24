// This code was auto-generated. Do not modify it.

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace FlaxEngine
{
    /// <summary>
    /// Base class for all input device objects.
    /// </summary>
    [Tooltip("Base class for all input device objects.")]
    public abstract unsafe partial class InputDevice : FlaxEngine.Object
    {
        /// <inheritdoc />
        protected InputDevice() : base()
        {
        }

        /// <summary>
        /// Gets the name.
        /// </summary>
        [Tooltip("The name.")]
        public string Name
        {
            get { return Internal_GetName(unmanagedPtr); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern string Internal_GetName(IntPtr obj);
    }
}
