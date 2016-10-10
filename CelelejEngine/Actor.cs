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
            get { return Internal_GetParent(this); }
            set { Internal_SetParent(this, value, false); }
        }

        /// <summary>
        /// Sets parent actor
        /// </summary>
        /// <param name="newParent">New parent to assign</param>
        /// <param name="worldPositionStays">Should actor world positions remain the same after parent change?</param>
        public void SetParent(Actor newParent, bool worldPositionStays = true)
        {
            Internal_SetParent(this, newParent, worldPositionStays);
        }

        #region Internal Calls

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern Actor Internal_GetParent(Actor obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetParent(Actor obj, Actor newParent, bool worldPositionStays);

        #endregion
    }
}
