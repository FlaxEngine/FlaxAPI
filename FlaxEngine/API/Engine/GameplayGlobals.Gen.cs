// This code was auto-generated. Do not modify it.

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace FlaxEngine
{
    /// <summary>
    /// The global gameplay variables container asset that can be accessed across whole project.
    /// </summary>
    [Tooltip("The global gameplay variables container asset that can be accessed across whole project.")]
    public unsafe partial class GameplayGlobals : BinaryAsset
    {
        /// <inheritdoc />
        protected GameplayGlobals() : base()
        {
        }

        /// <summary>
        /// Gets or sets the values (run-time).
        /// </summary>
        [Tooltip("The values (run-time).")]
        public System.Collections.Generic.Dictionary<string, object> Values
        {
            get { return Internal_GetValues(unmanagedPtr); }
            set { Internal_SetValues(unmanagedPtr, value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern System.Collections.Generic.Dictionary<string, object> Internal_GetValues(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetValues(IntPtr obj, System.Collections.Generic.Dictionary<string, object> values);

        /// <summary>
        /// Gets or sets the default values (edit-time).
        /// </summary>
        [Tooltip("The default values (edit-time).")]
        public System.Collections.Generic.Dictionary<string, object> DefaultValues
        {
            get { return Internal_GetDefaultValues(unmanagedPtr); }
            set { Internal_SetDefaultValues(unmanagedPtr, value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern System.Collections.Generic.Dictionary<string, object> Internal_GetDefaultValues(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetDefaultValues(IntPtr obj, System.Collections.Generic.Dictionary<string, object> values);

        /// <summary>
        /// Gets the value of the global variable (it must be added first).
        /// </summary>
        /// <param name="name">The variable name.</param>
        /// <returns>The value.</returns>
        public object GetValue(string name)
        {
            return Internal_GetValue(unmanagedPtr, name);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern object Internal_GetValue(IntPtr obj, string name);

        /// <summary>
        /// Sets the value of the global variable (it must be added first).
        /// </summary>
        /// <param name="name">The variable name.</param>
        /// <param name="value">The value.</param>
        public void SetValue(string name, object value)
        {
            Internal_SetValue(unmanagedPtr, name, value);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetValue(IntPtr obj, string name, object value);

        /// <summary>
        /// Resets the variables values to default values.
        /// </summary>
        public void ResetValues()
        {
            Internal_ResetValues(unmanagedPtr);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_ResetValues(IntPtr obj);

        /// <summary>
        /// Saves this asset to the file. Supported only in Editor.
        /// </summary>
        /// <param name="path">The custom asset path to use for the saving. Use empty value to save this asset to its own storage location. Can be used to duplicate asset. Must be specified when saving virtual asset.</param>
        /// <returns>True if cannot save data, otherwise false.</returns>
        public bool Save(string path = null)
        {
            return Internal_Save(unmanagedPtr, path);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_Save(IntPtr obj, string path);
    }
}
