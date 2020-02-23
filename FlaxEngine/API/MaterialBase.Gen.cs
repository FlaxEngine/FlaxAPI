// This code was auto-generated. Do not modify it.

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace FlaxEngine
{
    /// <summary>
    /// Base class for <see cref="Material"/> and <see cref="MaterialInstance"/>.
    /// </summary>
    /// <seealso cref="FlaxEngine.BinaryAsset" />
    [Tooltip("Base class for <see cref=\"Material\"/> and <see cref=\"MaterialInstance\"/>.")]
    public abstract partial class MaterialBase : BinaryAsset
    {
        /// <inheritdoc />
        protected MaterialBase() : base()
        {
        }

        /// <summary>
        /// Gets the material parameters collection.
        /// </summary>
        [Tooltip("The material parameters collection.")]
        public MaterialParameter[] Parameters
        {
            get { return Internal_GetParameters(unmanagedPtr, typeof(MaterialParameter)); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern MaterialParameter[] Internal_GetParameters(IntPtr obj, System.Type resultArrayItemType0);

        /// <summary>
        /// Gets the material info, structure which describes material surface.
        /// </summary>
        [Tooltip("The material info, structure which describes material surface.")]
        public MaterialInfo Info
        {
            get { Internal_Info(unmanagedPtr, out var resultAsRef); return resultAsRef; }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_Info(IntPtr obj, out MaterialInfo resultAsRef);

        /// <summary>
        /// Gets the material parameter.
        /// </summary>
        public MaterialParameter GetParameter(string name)
        {
            return Internal_GetParameter(unmanagedPtr, name);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern MaterialParameter Internal_GetParameter(IntPtr obj, string name);

        /// <summary>
        /// Gets the material parameter value.
        /// </summary>
        public object GetParameterValue(string name)
        {
            return Internal_GetParameterValue(unmanagedPtr, name);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern object Internal_GetParameterValue(IntPtr obj, string name);

        /// <summary>
        /// Sets the material parameter value (and sets IsOverride to true).
        /// </summary>
        public void SetParameterValue(string name, object value)
        {
            Internal_SetParameterValue(unmanagedPtr, name, value);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetParameterValue(IntPtr obj, string name, object value);
    }
}
