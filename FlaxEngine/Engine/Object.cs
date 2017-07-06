// Flax Engine scripting API

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
        [UnmanagedCall]
        public Guid ID
        {
            get { return id; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Object"/> class and calls backend to create unamanged object for ths instance.
        /// </summary>
        protected Object()
        {
            // Create unmanaged object
            Internal_ManagedInstanceCreated(this, out unmanagedPtr, out id);
        }

        private Object(IntPtr unmanagedPtr, ref Guid id)
        {
            this.unmanagedPtr = unmanagedPtr;
            this.id = id;
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
        internal static extern void Internal_ManagedInstanceCreated(Object managedInstance, out IntPtr unmanagedPtr, out Guid id);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_ManagedInstanceDeleted(IntPtr nativeInstance);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern Object Internal_FindObject(ref Guid id);

        #endregion
    }
}
