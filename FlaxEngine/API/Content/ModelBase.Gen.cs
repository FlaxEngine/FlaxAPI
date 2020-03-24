// This code was auto-generated. Do not modify it.

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace FlaxEngine
{
    /// <summary>
    /// Base class for asset types that can contain a model resource.
    /// </summary>
    [Tooltip("Base class for asset types that can contain a model resource.")]
    public abstract unsafe partial class ModelBase : BinaryAsset
    {
        /// <inheritdoc />
        protected ModelBase() : base()
        {
        }

        /// <summary>
        /// The minimum screen size to draw this model (the bottom limit). Used to cull small models. Set to 0 to disable this feature.
        /// </summary>
        [Tooltip("The minimum screen size to draw this model (the bottom limit). Used to cull small models. Set to 0 to disable this feature.")]
        public float MinScreenSize
        {
            get { return Internal_GetMinScreenSize(unmanagedPtr); }
            set { Internal_SetMinScreenSize(unmanagedPtr, value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern float Internal_GetMinScreenSize(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetMinScreenSize(IntPtr obj, float value);

        /// <summary>
        /// The list of material slots.
        /// </summary>
        [Tooltip("The list of material slots.")]
        public MaterialSlot[] MaterialSlots
        {
            get { return Internal_GetMaterialSlots(unmanagedPtr, typeof(MaterialSlot)); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern MaterialSlot[] Internal_GetMaterialSlots(IntPtr obj, System.Type resultArrayItemType0);

        /// <summary>
        /// Gets the amount of the material slots used by this model asset.
        /// </summary>
        [Tooltip("The amount of the material slots used by this model asset.")]
        public int MaterialSlotsCount
        {
            get { return Internal_GetMaterialSlotsCount(unmanagedPtr); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern int Internal_GetMaterialSlotsCount(IntPtr obj);

        /// <summary>
        /// Resizes the material slots collection. Updates meshes that were using removed slots.
        /// </summary>
        public void SetupMaterialSlots(int slotsCount)
        {
            Internal_SetupMaterialSlots(unmanagedPtr, slotsCount);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetupMaterialSlots(IntPtr obj, int slotsCount);

        /// <summary>
        /// Gets the material slot by the name.
        /// </summary>
        /// <param name="name">The slot name.</param>
        /// <returns>The material slot with the given name or null if cannot find it (asset may be not loaded yet).</returns>
        public MaterialSlot GetSlot(string name)
        {
            return Internal_GetSlot(unmanagedPtr, name);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern MaterialSlot Internal_GetSlot(IntPtr obj, string name);
    }
}
