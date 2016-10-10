// Celelej Game Engine scripting API

using System;
using System.Runtime.CompilerServices;

namespace CelelejEngine
{
    /// <summary>
    /// Base class for all objects Celelej can reference.
    /// </summary>
    public abstract class Object
    {
        internal IntPtr unmanagedPtr;

        /// <summary>
        /// Notifies the unmanaged interop object that the managed instance was finalized.
        /// </summary>
        ~Object()
        {
            if (unmanagedPtr == IntPtr.Zero)
            {
                Debug.LogError("Script object is being finalized but doesn't have a pointer to its interop object. Type: " + GetType());
            }
            else
            {
                Internal_ManagedInstanceDeleted(unmanagedPtr);
            }
        }

        /// <summary>
        /// Check if object exists
        /// </summary>
        /// <param name="obj">Object to check</param>
        public static implicit operator bool(Object obj)
        {
            return obj != null && obj.unmanagedPtr != IntPtr.Zero;
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return unmanagedPtr.GetHashCode();
        }

        #region Internal Calls

        [MethodImpl(MethodImplOptions.InternalCall)]
        private static extern void Internal_ManagedInstanceDeleted(IntPtr nativeInstance);

        #endregion
    }
}
