// This code was auto-generated. Do not modify it.

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace FlaxEngine
{
    /// <summary>
    /// Instance of the <seealso cref="Material" /> with custom set of material parameter values.
    /// </summary>
    [Tooltip("Instance of the <seealso cref=\"Material\" /> with custom set of material parameter values.")]
    public partial class MaterialInstance : MaterialBase
    {
        /// <inheritdoc />
        protected MaterialInstance() : base()
        {
        }

        /// <summary>
        /// Gets or sets the base material. If value gets changed parameters collection is restored to the default values of the new material.
        /// </summary>
        [Tooltip("The base material. If value gets changed parameters collection is restored to the default values of the new material.")]
        public MaterialBase BaseMaterial
        {
            get { return Internal_GetBaseMaterial(unmanagedPtr); }
            set { Internal_SetBaseMaterial(unmanagedPtr, FlaxEngine.Object.GetUnmanagedPtr(value)); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern MaterialBase Internal_GetBaseMaterial(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetBaseMaterial(IntPtr obj, IntPtr baseMaterial);

        /// <summary>
        /// Saves this asset to the file. Supported only in Editor.
        /// </summary>
        /// <remarks>If you use saving with the GPU mesh data then the call has to be provided from the thread other than the main game thread.</remarks>
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
