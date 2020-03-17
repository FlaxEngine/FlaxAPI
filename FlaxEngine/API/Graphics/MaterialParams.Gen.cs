// This code was auto-generated. Do not modify it.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace FlaxEngine
{
    /// <summary>
    /// Material variable object. Allows to modify material parameter value at runtime.
    /// </summary>
    [Tooltip("Material variable object. Allows to modify material parameter value at runtime.")]
    public unsafe partial class MaterialParameter : FlaxEngine.Object
    {
        /// <inheritdoc />
        protected MaterialParameter() : base()
        {
        }

        /// <summary>
        /// Gets the parameter ID (not the parameter instance Id but the original parameter ID).
        /// </summary>
        [Tooltip("The parameter ID (not the parameter instance Id but the original parameter ID).")]
        public Guid ParameterID
        {
            get { Internal_GetParameterID(unmanagedPtr, out var resultAsRef); return resultAsRef; }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_GetParameterID(IntPtr obj, out Guid resultAsRef);

        /// <summary>
        /// Gets the parameter type.
        /// </summary>
        [Tooltip("The parameter type.")]
        public MaterialParameterType ParameterType
        {
            get { return Internal_GetParameterType(unmanagedPtr); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern MaterialParameterType Internal_GetParameterType(IntPtr obj);

        /// <summary>
        /// Gets the parameter name.
        /// </summary>
        [Tooltip("The parameter name.")]
        public string Name
        {
            get { return Internal_GetName(unmanagedPtr); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern string Internal_GetName(IntPtr obj);

        /// <summary>
        /// Returns true is parameter is public visible.
        /// </summary>
        [Tooltip("Returns true is parameter is public visible.")]
        public bool IsPublic
        {
            get { return Internal_IsPublic(unmanagedPtr); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_IsPublic(IntPtr obj);

        /// <summary>
        /// Returns true is parameter is overriding the value.
        /// </summary>
        [Tooltip("Returns true is parameter is overriding the value.")]
        public bool IsOverride
        {
            get { return Internal_IsOverride(unmanagedPtr); }
            set { Internal_SetIsOverride(unmanagedPtr, value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_IsOverride(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetIsOverride(IntPtr obj, bool value);

        /// <summary>
        /// Gets or sets the value of the parameter.
        /// </summary>
        [Tooltip("The value of the parameter.")]
        public object Value
        {
            get { return Internal_GetValue(unmanagedPtr); }
            set { Internal_SetValue(unmanagedPtr, value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern object Internal_GetValue(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetValue(IntPtr obj, object value);
    }
}
