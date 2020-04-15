// This code was auto-generated. Do not modify it.

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace FlaxEngine
{
    /// <summary>
    /// Material function graph asset that contains reusable part of the material graph.
    /// </summary>
    [Tooltip("Material function graph asset that contains reusable part of the material graph.")]
    public unsafe partial class MaterialFunction : BinaryAsset
    {
        /// <inheritdoc />
        protected MaterialFunction() : base()
        {
        }

        /// <summary>
        /// Tries to load surface graph from the asset.
        /// </summary>
        /// <returns>The output surface data, or empty if failed to load.</returns>
        public byte[] LoadSurface()
        {
            return Internal_LoadSurface(unmanagedPtr);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern byte[] Internal_LoadSurface(IntPtr obj);

        /// <summary>
        /// Gets the function signature for Visject Surface editor.
        /// </summary>
        public void GetSignature(out int[] types, out string[] names)
        {
            Internal_GetSignature(unmanagedPtr, out types, out names);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_GetSignature(IntPtr obj, out int[] types, out string[] names);

        /// <summary>
        /// Updates the material graph surface (save new one, discards cached data, reloads asset).
        /// </summary>
        /// <param name="data">The surface graph data.</param>
        /// <returns>True if cannot save it, otherwise false.</returns>
        public bool SaveSurface(byte[] data)
        {
            return Internal_SaveSurface(unmanagedPtr, data);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_SaveSurface(IntPtr obj, byte[] data);
    }
}
