////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Runtime.CompilerServices;

namespace FlaxEngine
{
    /// <summary>
    /// Base class for all objects Flax can reference.
    /// </summary>
    public abstract class Object
    {
        [NonSerialized]
        internal IntPtr unmanagedPtr = IntPtr.Zero;

        [NonSerialized]
        internal Guid id = Guid.Empty;

        /// <summary>
        /// Gets unique object ID
        /// </summary>
        [HideInEditor]
        public Guid ID => id;

        /// <summary>
        /// Initializes a new instance of the <see cref="Object"/>.
        /// Always called from C++.
        /// </summary>
        protected Object()
        {
        }

        /// <summary>
        /// Notifies the unmanaged interop object that the managed instance was finalized.
        /// </summary>
        ~Object()
        {
            if (unmanagedPtr != IntPtr.Zero)
            {
                Internal_ManagedInstanceDeleted(unmanagedPtr);
            }
        }

        /// <summary>
        /// Creates the new instance of the Object.
        /// All unused objects should be released using <see cref="Destroy"/>.
        /// </summary>
        /// <typeparam name="T">Type of the object.</typeparam>
        /// <returns>Create object.</returns>
        public static T New<T>() where T : Object
        {
            return Internal_Create(typeof(T)) as T;
        }

        /// <summary>
        /// Destroys the specified object and clears the reference variable.
        /// The object obj will be destroyed now or after the time specified in seconds from now.
        /// If obj is a Script it will be removed from the Actor and deleted.
        /// If obj is an Actor it will be removed from the Scene and deleted as well as all its Scripts and all children of the Actor.
        /// Actual object destruction is always delayed until after the current Update loop, but will always be done before rendering.
        /// </summary>
        /// <param name="obj">The object to destroy.</param>
        /// <param name="timeLeft">The time left to destroy object (in seconds).</param>
        public static void Destroy(Object obj, float timeLeft = 0.0f)
        {
            Internal_Destroy(GetUnmanagedPtr(obj), timeLeft);
        }

        /// <summary>
        /// Destroys the specified object and clears the reference variable.
        /// The object obj will be destroyed now or after the time specified in seconds from now.
        /// If obj is a Script it will be removed from the Actor and deleted.
        /// If obj is an Actor it will be removed from the Scene and deleted as well as all its Scripts and all children of the Actor.
        /// Actual object destruction is always delayed until after the current Update loop, but will always be done before rendering.
        /// </summary>
        /// <param name="obj">The object to destroy.</param>
        /// <param name="timeLeft">The time left to destroy object (in seconds).</param>
        public static void Destroy<T>(ref T obj, float timeLeft = 0.0f) where T : Object
        {
            if (obj)
            {
                Internal_Destroy(obj.unmanagedPtr, timeLeft);
                obj = null;
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

        internal static IntPtr GetUnmanagedPtr(Object obj)
        {
            return obj != null ? obj.unmanagedPtr : IntPtr.Zero;
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return unmanagedPtr.GetHashCode();
        }

        #region Internal Calls

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern Object Internal_Create(Type type);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_ManagedInstanceDeleted(IntPtr nativeInstance);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_Destroy(IntPtr obj, float timeLeft);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern Object Internal_FindObject(ref Guid id);

        #endregion
    }
}
