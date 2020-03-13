// This code was auto-generated. Do not modify it.

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace FlaxEngine
{
    /// <summary>
    /// Raw bytes container asset.
    /// </summary>
    [Tooltip("Raw bytes container asset.")]
    public unsafe partial class RawDataAsset : BinaryAsset
    {
        /// <inheritdoc />
        protected RawDataAsset() : base()
        {
        }

        /// <summary>
        /// The bytes array stored in the asset.
        /// </summary>
        [Tooltip("The bytes array stored in the asset.")]
        public byte[] Data
        {
            get { return Internal_GetData(unmanagedPtr, typeof(byte)); }
            set { Internal_SetData(unmanagedPtr, value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern byte[] Internal_GetData(IntPtr obj, System.Type resultArrayItemType0);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetData(IntPtr obj, byte[] value);

        /// <summary>
        /// Saves this asset to the file. Supported only in Editor.
        /// </summary>
        /// <param name="path">The custom asset path to use for the saving. Use empty value to save this asset to its own storage location. Can be used to duplicate asset. Must be specified when saving virtual asset.</param>
        /// <returns>True if failed, otherwise false.</returns>
        public bool Save(string path = null)
        {
            return Internal_Save(unmanagedPtr, path);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_Save(IntPtr obj, string path);
    }
}
