// This code was auto-generated. Do not modify it.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace FlaxEngine
{
    /// <summary>
    /// Interface for GPU device adapter.
    /// </summary>
    [Tooltip("Interface for GPU device adapter.")]
    public unsafe partial class GPUAdapter : FlaxEngine.Object
    {
        /// <inheritdoc />
        protected GPUAdapter() : base()
        {
        }

        /// <summary>
        /// Gets the GPU vendor identifier.
        /// </summary>
        [Tooltip("The GPU vendor identifier.")]
        public uint VendorId
        {
            get { return Internal_GetVendorId(unmanagedPtr); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern uint Internal_GetVendorId(IntPtr obj);

        /// <summary>
        /// Gets a string that contains the adapter description. Used for presentation to the user.
        /// </summary>
        [Tooltip("Gets a string that contains the adapter description. Used for presentation to the user.")]
        public string Description
        {
            get { return Internal_GetDescription(unmanagedPtr); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern string Internal_GetDescription(IntPtr obj);

        /// <summary>
        /// Returns true if adapter's vendor is AMD.
        /// </summary>
        [Tooltip("Returns true if adapter's vendor is AMD.")]
        public bool IsAMD
        {
            get { return Internal_IsAMD(unmanagedPtr); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_IsAMD(IntPtr obj);

        /// <summary>
        /// Returns true if adapter's vendor is Intel.
        /// </summary>
        [Tooltip("Returns true if adapter's vendor is Intel.")]
        public bool IsIntel
        {
            get { return Internal_IsIntel(unmanagedPtr); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_IsIntel(IntPtr obj);

        /// <summary>
        /// Returns true if adapter's vendor is Nvidia.
        /// </summary>
        [Tooltip("Returns true if adapter's vendor is Nvidia.")]
        public bool IsNVIDIA
        {
            get { return Internal_IsNVIDIA(unmanagedPtr); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_IsNVIDIA(IntPtr obj);

        /// <summary>
        /// Returns true if adapter's vendor is Microsoft.
        /// </summary>
        [Tooltip("Returns true if adapter's vendor is Microsoft.")]
        public bool IsMicrosoft
        {
            get { return Internal_IsMicrosoft(unmanagedPtr); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_IsMicrosoft(IntPtr obj);
    }
}
