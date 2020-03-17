// This code was auto-generated. Do not modify it.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using FlaxEngine;

namespace FlaxEditor
{
    /// <summary>
    /// Asset which contains set of asset items thumbnails (cached previews).
    /// </summary>
    [Tooltip("Asset which contains set of asset items thumbnails (cached previews).")]
    public sealed unsafe partial class PreviewsCache : SpriteAtlas
    {
        private PreviewsCache() : base()
        {
        }

        /// <summary>
        /// Determines whether this atlas is ready (is loaded and has texture streamed).
        /// </summary>
        [Tooltip("Determines whether this atlas is ready (is loaded and has texture streamed).")]
        public bool IsReady
        {
            get { return Internal_IsReady(unmanagedPtr); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_IsReady(IntPtr obj);

        /// <summary>
        /// Determines whether this atlas has one or more free slots for the asset preview.
        /// </summary>
        [Tooltip("Determines whether this atlas has one or more free slots for the asset preview.")]
        public bool HasFreeSlot
        {
            get { return Internal_HasFreeSlot(unmanagedPtr); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_HasFreeSlot(IntPtr obj);

        /// <summary>
        /// Finds the preview icon for given asset ID.
        /// </summary>
        /// <param name="id">The asset id to find preview for it.</param>
        /// <returns>The output sprite slot handle or invalid if invalid in nothing found.</returns>
        public SpriteHandle FindSlot(Guid id)
        {
            Internal_FindSlot(unmanagedPtr, ref id, out var resultAsRef); return resultAsRef;
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_FindSlot(IntPtr obj, ref Guid id, out SpriteHandle resultAsRef);

        /// <summary>
        /// Occupies the atlas slot.
        /// </summary>
        /// <param name="source">The source texture to insert.</param>
        /// <param name="id">The asset identifier.</param>
        /// <returns>The added sprite slot handle or invalid if invalid in failed to occupy slot.</returns>
        public SpriteHandle OccupySlot(GPUTexture source, Guid id)
        {
            Internal_OccupySlot(unmanagedPtr, FlaxEngine.Object.GetUnmanagedPtr(source), ref id, out var resultAsRef); return resultAsRef;
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_OccupySlot(IntPtr obj, IntPtr source, ref Guid id, out SpriteHandle resultAsRef);

        /// <summary>
        /// Releases the used slot.
        /// </summary>
        /// <param name="id">The asset identifier.</param>
        /// <returns>True if slot has been release, otherwise it was not found.</returns>
        public bool ReleaseSlot(Guid id)
        {
            return Internal_ReleaseSlot(unmanagedPtr, ref id);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_ReleaseSlot(IntPtr obj, ref Guid id);

        /// <summary>
        /// Flushes atlas data from the GPU to the asset storage (saves data).
        /// </summary>
        public void Flush()
        {
            Internal_Flush(unmanagedPtr);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_Flush(IntPtr obj);

        /// <summary>
        /// Creates a new atlas.
        /// </summary>
        /// <param name="outputPath">The output asset file path.</param>
        /// <returns>True if this previews cache is flushing, otherwise false.</returns>
        public static bool Create(string outputPath)
        {
            return Internal_Create(outputPath);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_Create(string outputPath);
    }
}
