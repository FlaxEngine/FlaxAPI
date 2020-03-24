// This code was auto-generated. Do not modify it.

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace FlaxEngine
{
    /// <summary>
    /// Asset objects base class.
    /// </summary>
    [Tooltip("Asset objects base class.")]
    public abstract unsafe partial class Asset : FlaxEngine.Object
    {
        /// <inheritdoc />
        protected Asset() : base()
        {
        }

        /// <summary>
        /// Gets asset's reference count. Asset will be automatically unloaded when this reaches zero.
        /// </summary>
        [Tooltip("Gets asset's reference count. Asset will be automatically unloaded when this reaches zero.")]
        public int ReferencesCount
        {
            get { return Internal_GetReferencesCount(unmanagedPtr); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern int Internal_GetReferencesCount(IntPtr obj);

        /// <summary>
        /// Gets the path to the asset storage.
        /// </summary>
        [Tooltip("The path to the asset storage.")]
        public string Path
        {
            get { return Internal_GetPath(unmanagedPtr); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern string Internal_GetPath(IntPtr obj);

        /// <summary>
        /// Returns true if asset is loaded, otherwise false.
        /// </summary>
        [Tooltip("Returns true if asset is loaded, otherwise false.")]
        public bool IsLoaded
        {
            get { return Internal_IsLoaded(unmanagedPtr); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_IsLoaded(IntPtr obj);

        /// <summary>
        /// Returns true if last asset loading failed, otherwise false.
        /// </summary>
        [Tooltip("Returns true if last asset loading failed, otherwise false.")]
        public bool LastLoadFailed
        {
            get { return Internal_LastLoadFailed(unmanagedPtr); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_LastLoadFailed(IntPtr obj);

        /// <summary>
        /// Determines whether this asset is virtual (generated or temporary, has no storage so it won't be saved).
        /// </summary>
        [Tooltip("Determines whether this asset is virtual (generated or temporary, has no storage so it won't be saved).")]
        public bool IsVirtual
        {
            get { return Internal_IsVirtual(unmanagedPtr); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_IsVirtual(IntPtr obj);

        /// <summary>
        /// Reloads the asset.
        /// </summary>
        public void Reload()
        {
            Internal_Reload(unmanagedPtr);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_Reload(IntPtr obj);

        /// <summary>
        /// Stops the current thread execution and waits until asset will be loaded (loading will fail, success or be cancelled).
        /// </summary>
        /// <param name="timeoutInMilliseconds">Custom timeout value in milliseconds.</param>
        /// <returns>True if cannot load that asset (failed or has been cancelled), otherwise false.</returns>
        public bool WaitForLoaded(double timeoutInMilliseconds = 30000.0)
        {
            return Internal_WaitForLoaded(unmanagedPtr, timeoutInMilliseconds);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_WaitForLoaded(IntPtr obj, double timeoutInMilliseconds);

        /// <summary>
        /// Gets the asset references. Supported only in Editor.
        /// </summary>
        /// <remarks>
        /// For some asset types (e.g. scene or prefab) it may contain invalid asset ids due to not perfect gather method,
        /// which is optimized to perform scan very quickly. Before using those ids perform simple validation via Content cache API.
        /// The result collection contains only 1-level-deep references (only direct ones) and is invalid if asset is not loaded.
        /// Also the output data may have duplicated asset ids or even invalid ids (Guid.Empty).
        /// </remarks>
        /// <returns>The collection of the asset ids referenced by this asset.</returns>
        public Guid[] GetReferences()
        {
            return Internal_GetReferences(unmanagedPtr, typeof(Guid));
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern Guid[] Internal_GetReferences(IntPtr obj, System.Type resultArrayItemType0);
    }
}
