// This code was auto-generated. Do not modify it.

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace FlaxEngine
{
    /// <summary>
    /// The skinned model skeleton bones boolean masking data.
    /// </summary>
    [Tooltip("The skinned model skeleton bones boolean masking data.")]
    public unsafe partial class SkeletonMask : BinaryAsset
    {
        /// <inheritdoc />
        protected SkeletonMask() : base()
        {
        }

        /// <summary>
        /// The referenced skinned model skeleton that defines the masked nodes hierarchy.
        /// </summary>
        [Tooltip("The referenced skinned model skeleton that defines the masked nodes hierarchy.")]
        public SkinnedModel Skeleton
        {
            get { return Internal_GetSkeleton(unmanagedPtr); }
            set { Internal_SetSkeleton(unmanagedPtr, FlaxEngine.Object.GetUnmanagedPtr(value)); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern SkinnedModel Internal_GetSkeleton(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetSkeleton(IntPtr obj, IntPtr value);

        /// <summary>
        /// Gets or sets the per-skeleton node mask (by name).
        /// </summary>
        [Tooltip("The per-skeleton node mask (by name).")]
        public string[] MaskedNodes
        {
            get { return Internal_GetMaskedNodes(unmanagedPtr); }
            set { Internal_SetMaskedNodes(unmanagedPtr, value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern string[] Internal_GetMaskedNodes(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetMaskedNodes(IntPtr obj, string[] value);

        /// <summary>
        /// Gets the per-skeleton-node boolean mask (read-only).
        /// </summary>
        [Tooltip("The per-skeleton-node boolean mask (read-only).")]
        public bool[] NodesMask
        {
            get { return Internal_GetNodesMask(unmanagedPtr); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool[] Internal_GetNodesMask(IntPtr obj);

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
