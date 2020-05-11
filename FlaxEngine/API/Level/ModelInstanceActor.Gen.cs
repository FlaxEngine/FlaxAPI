// This code was auto-generated. Do not modify it.

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace FlaxEngine
{
    /// <summary>
    /// Base class for actor types that use ModelInstanceEntries for mesh rendering.
    /// </summary>
    /// <seealso cref="Actor" />
    [Tooltip("Base class for actor types that use ModelInstanceEntries for mesh rendering.")]
    public abstract unsafe partial class ModelInstanceActor : Actor
    {
        /// <inheritdoc />
        protected ModelInstanceActor() : base()
        {
        }

        /// <summary>
        /// Gets or sets the model entries collection. Each entry contains data how to render meshes using this entry (transformation, material, shadows casting, etc.).
        /// </summary>
        [Serialize, EditorOrder(1000), EditorDisplay("Entries", EditorDisplayAttribute.InlineStyle), Collection(CanReorderItems = false, NotNullItems = true, ReadOnly = true)]
        [Tooltip("The model entries collection. Each entry contains data how to render meshes using this entry (transformation, material, shadows casting, etc.).")]
        public ModelInstanceEntry[] Entries
        {
            get { return Internal_GetEntries(unmanagedPtr, typeof(ModelInstanceEntry)); }
            set { Internal_SetEntries(unmanagedPtr, value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern ModelInstanceEntry[] Internal_GetEntries(IntPtr obj, System.Type resultArrayItemType0);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetEntries(IntPtr obj, ModelInstanceEntry[] value);

        /// <summary>
        /// Sets the material to the entry slot. Can be used to override the material of the meshes using this slot.
        /// </summary>
        /// <param name="entryIndex">The material slot entry index.</param>
        /// <param name="material">The material to set..</param>
        public void SetMaterial(int entryIndex, MaterialBase material)
        {
            Internal_SetMaterial(unmanagedPtr, entryIndex, FlaxEngine.Object.GetUnmanagedPtr(material));
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetMaterial(IntPtr obj, int entryIndex, IntPtr material);

        /// <summary>
        /// Utility to crate a new virtual Material Instance asset, set its parent to the currently applied material, and assign it to the entry. Can be used to modify the material parameters from code.
        /// </summary>
        /// <param name="entryIndex">The material slot entry index.</param>
        /// <returns>The created virtual material instance.</returns>
        public MaterialInstance CreateAndSetVirtualMaterialInstance(int entryIndex)
        {
            return Internal_CreateAndSetVirtualMaterialInstance(unmanagedPtr, entryIndex);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern MaterialInstance Internal_CreateAndSetVirtualMaterialInstance(IntPtr obj, int entryIndex);

        /// <summary>
        /// Determines if there is an intersection between the model actor mesh entry and a ray.
        /// If mesh data is available on the CPU performs exact intersection check with the geometry.
        /// Otherwise performs simple <see cref="BoundingBox"/> vs <see cref="Ray"/> test.
        /// For more efficient collisions detection and ray casting use physics.
        /// </summary>
        /// <param name="entryIndex">The material slot entry index to test.</param>
        /// <param name="ray">The ray to test.</param>
        /// <param name="distance">When the method completes and returns true, contains the distance of the intersection (if any valid).</param>
        /// <param name="normal">When the method completes, contains the intersection surface normal vector (if any valid).</param>
        /// <returns>True if the actor is intersected by the ray, otherwise false.</returns>
        public bool IntersectsEntry(int entryIndex, ref Ray ray, out float distance, out Vector3 normal)
        {
            return Internal_IntersectsEntry(unmanagedPtr, entryIndex, ref ray, out distance, out normal);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_IntersectsEntry(IntPtr obj, int entryIndex, ref Ray ray, out float distance, out Vector3 normal);

        /// <summary>
        /// Determines if there is an intersection between the model actor mesh entry and a ray.
        /// If mesh data is available on the CPU performs exact intersection check with the geometry.
        /// Otherwise performs simple <see cref="BoundingBox"/> vs <see cref="Ray"/> test.
        /// For more efficient collisions detection and ray casting use physics.
        /// </summary>
        /// <param name="ray">The ray to test.</param>
        /// <param name="distance">When the method completes and returns true, contains the distance of the intersection (if any valid).</param>
        /// <param name="normal">When the method completes, contains the intersection surface normal vector (if any valid).</param>
        /// <param name="entryIndex">When the method completes, contains the intersection entry index (if any valid).</param>
        /// <returns>True if the actor is intersected by the ray, otherwise false.</returns>
        public bool IntersectsEntry(ref Ray ray, out float distance, out Vector3 normal, out int entryIndex)
        {
            return Internal_IntersectsEntry1(unmanagedPtr, ref ray, out distance, out normal, out entryIndex);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_IntersectsEntry1(IntPtr obj, ref Ray ray, out float distance, out Vector3 normal, out int entryIndex);
    }
}
