// Celelej Game Engine scripting API

using System;
using System.Runtime.CompilerServices;

namespace CelelejEngine
{
    /// <summary>
    /// Base class for all actor types on the scene
    /// </summary>
    public abstract class Actor : Object
    {
        /// <summary>
        /// Gets or sets parent actor (or null if actor has no parent)
        /// </summary>
        public Actor Parent
        {
            get { return Internal_GetParent(unmanagedPtr); }
            set { Internal_SetParent(unmanagedPtr, GetUnmanagedPtr(value), false); }
        }

        /// <summary>
        /// Gets or sets actor name
        /// </summary>
        public string Name
        {
            get { return Internal_GetName(unmanagedPtr); }
            set { Internal_SetName(unmanagedPtr, value); }
        }

        /// <summary>
        /// Sets parent actor
        /// </summary>
        /// <param name="newParent">New parent to assign</param>
        /// <param name="worldPositionStays">Should actor world positions remain the same after parent change?</param>
        public void SetParent(Actor newParent, bool worldPositionStays = true)
        {
            Internal_SetParent(unmanagedPtr, GetUnmanagedPtr(newParent) , worldPositionStays);
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return string.Format("{0} ({1})", Internal_GetName(unmanagedPtr), GetType().Name);
        }

        #region Internal Calls

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern Actor Internal_GetParent(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetParent(IntPtr obj, IntPtr newParent, bool worldPositionStays);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern string Internal_GetName(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetName(IntPtr obj, string newName);

        #endregion
    }
}
