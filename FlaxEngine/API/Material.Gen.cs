// This code was auto-generated. Do not modify it.

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace FlaxEngine
{
    /// <summary>
    /// Material asset that contains shader for rendering models on the GPU.
    /// </summary>
    [Tooltip("Material asset that contains shader for rendering models on the GPU.")]
    public partial class Material : MaterialBase
    {
        /// <inheritdoc />
        protected Material() : base()
        {
        }

        /// <summary>
        /// Tries to load surface graph from the asset.
        /// </summary>
        /// <param name="createDefaultIfMissing">True if create default surface if missing.</param>
        /// <returns>The output surface data, or empty if failed to load.</returns>
        public byte[] LoadSurface(bool createDefaultIfMissing)
        {
            return Internal_LoadSurface(unmanagedPtr, createDefaultIfMissing);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern byte[] Internal_LoadSurface(IntPtr obj, bool createDefaultIfMissing);

        /// <summary>
        /// Updates the material surface (save new one, discard cached data, reload asset).
        /// </summary>
        /// <param name="data">The surface graph data.</param>
        /// <param name="info">The material info structure.</param>
        /// <returns>True if cannot save it, otherwise false.</returns>
        public bool SaveSurface(byte[] data, MaterialInfo info)
        {
            return Internal_SaveSurface(unmanagedPtr, data, ref info);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_SaveSurface(IntPtr obj, byte[] data, ref MaterialInfo info);
    }
}
