// This code was auto-generated. Do not modify it.

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace FlaxEngine
{
    /// <summary>
    /// The Animation Graph is used to evaluate a final pose for the animated model for the current frame.
    /// </summary>
    [Tooltip("The Animation Graph is used to evaluate a final pose for the animated model for the current frame.")]
    public unsafe partial class AnimationGraph : BinaryAsset
    {
        /// <inheritdoc />
        protected AnimationGraph() : base()
        {
        }

        /// <summary>
        /// Gets the base model asset used for the animation preview and the skeleton layout source.
        /// </summary>
        [Tooltip("The base model asset used for the animation preview and the skeleton layout source.")]
        public SkinnedModel BaseModel
        {
            get { return Internal_GetBaseModel(unmanagedPtr); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern SkinnedModel Internal_GetBaseModel(IntPtr obj);

        /// <summary>
        /// Tries to load surface graph from the asset.
        /// </summary>
        /// <returns>The surface data or empty if failed to load it.</returns>
        public byte[] LoadSurface()
        {
            return Internal_LoadSurface(unmanagedPtr);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern byte[] Internal_LoadSurface(IntPtr obj);

        /// <summary>
        /// Updates the animation graph surface (save new one, discard cached data, reload asset).
        /// </summary>
        /// <param name="data">Stream with graph data.</param>
        /// <returns>True if cannot save it, otherwise false.</returns>
        public bool SaveSurface(byte[] data)
        {
            return Internal_SaveSurface(unmanagedPtr, data);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_SaveSurface(IntPtr obj, byte[] data);
    }
}
