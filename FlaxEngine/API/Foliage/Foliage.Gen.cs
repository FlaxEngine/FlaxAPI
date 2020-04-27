// This code was auto-generated. Do not modify it.

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace FlaxEngine
{
    /// <summary>
    /// Represents a foliage actor that contains a set of instanced meshes.
    /// </summary>
    /// <seealso cref="Actor" />
    [Tooltip("Represents a foliage actor that contains a set of instanced meshes.")]
    public sealed unsafe partial class Foliage : Actor
    {
        private Foliage() : base()
        {
        }

        /// <summary>
        /// Creates new instance of <see cref="Foliage"/> object.
        /// </summary>
        /// <returns>The created object.</returns>
        public static Foliage New()
        {
            return Internal_Create(typeof(Foliage)) as Foliage;
        }

        /// <summary>
        /// The foliage instances types used by the current foliage actor. It's read-only.
        /// </summary>
        [HideInEditor, NoSerialize]
        [Tooltip("The foliage instances types used by the current foliage actor. It's read-only.")]
        public FoliageType[] FoliageTypes
        {
            get { return Internal_GetFoliageTypes(unmanagedPtr, typeof(FoliageType)); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern FoliageType[] Internal_GetFoliageTypes(IntPtr obj, System.Type resultArrayItemType0);

        /// <summary>
        /// Gets the total amount of the instanced of foliage.
        /// </summary>
        [Tooltip("The total amount of the instanced of foliage.")]
        public int InstancesCount
        {
            get { return Internal_GetInstancesCount(unmanagedPtr); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern int Internal_GetInstancesCount(IntPtr obj);

        /// <summary>
        /// Gets the total amount of the types of foliage.
        /// </summary>
        [Tooltip("The total amount of the types of foliage.")]
        public int FoliageTypesCount
        {
            get { return Internal_GetFoliageTypesCount(unmanagedPtr); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern int Internal_GetFoliageTypesCount(IntPtr obj);

        /// <summary>
        /// Gets or sets the global density scale for all foliage instances. The default value is 1. Use values from range 0-1. Lower values decrease amount of foliage instances in-game. Use it to tweak game performance for slower devices.
        /// </summary>
        [Tooltip("The global density scale for all foliage instances. The default value is 1. Use values from range 0-1. Lower values decrease amount of foliage instances in-game. Use it to tweak game performance for slower devices.")]
        public static float GlobalDensityScale
        {
            get { return Internal_GetGlobalDensityScale(); }
            set { Internal_SetGlobalDensityScale(value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern float Internal_GetGlobalDensityScale();

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetGlobalDensityScale(float value);

        /// <summary>
        /// Gets the foliage instance by index.
        /// </summary>
        /// <param name="index">The zero-based index of the foliage instance.</param>
        /// <returns>The foliage instance data.</returns>
        public FoliageInstance GetInstance(int index)
        {
            Internal_GetInstance(unmanagedPtr, index, out var resultAsRef); return resultAsRef;
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_GetInstance(IntPtr obj, int index, out FoliageInstance resultAsRef);

        /// <summary>
        /// Gets the foliage type.
        /// </summary>
        /// <param name="index">The zero-based index of the foliage type.</param>
        /// <returns>The foliage type.</returns>
        public FoliageType GetFoliageType(int index)
        {
            return Internal_GetFoliageType(unmanagedPtr, index);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern FoliageType Internal_GetFoliageType(IntPtr obj, int index);

        /// <summary>
        /// Adds the type of the foliage.
        /// </summary>
        /// <param name="model">The model to assign. It cannot be null nor already used by the other instance type (it must be unique within the given foliage actor).</param>
        public void AddFoliageType(Model model)
        {
            Internal_AddFoliageType(unmanagedPtr, FlaxEngine.Object.GetUnmanagedPtr(model));
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_AddFoliageType(IntPtr obj, IntPtr model);

        /// <summary>
        /// Removes the foliage instance type and all foliage instances using this type.
        /// </summary>
        /// <param name="index">The zero-based index of the foliage instance type.</param>
        public void RemoveFoliageType(int index)
        {
            Internal_RemoveFoliageType(unmanagedPtr, index);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_RemoveFoliageType(IntPtr obj, int index);

        /// <summary>
        /// Gets the total amount of the instanced that use the given foliage type.
        /// </summary>
        /// <param name="index">The zero-based index of the foliage type.</param>
        /// <returns>The foliage type instances count.</returns>
        public int GetFoliageTypeInstancesCount(int index)
        {
            return Internal_GetFoliageTypeInstancesCount(unmanagedPtr, index);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern int Internal_GetFoliageTypeInstancesCount(IntPtr obj, int index);

        /// <summary>
        /// Adds the new foliage instance. Ensure to always call <see cref="RebuildClusters"/> after editing foliage to sync cached data (call it once after editing one or more instances).
        /// </summary>
        /// <remarks>Input instance bounds, instance random and world matrix are ignored (recalculated).</remarks>
        /// <param name="instance">The instance.</param>
        public void AddInstance(ref FoliageInstance instance)
        {
            Internal_AddInstance(unmanagedPtr, ref instance);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_AddInstance(IntPtr obj, ref FoliageInstance instance);

        /// <summary>
        /// Removes the foliage instance. Ensure to always call <see cref="RebuildClusters"/> after editing foliage to sync cached data (call it once after editing one or more instances).
        /// </summary>
        /// <param name="index">The zero-based index of the instance to remove.</param>
        public void RemoveInstance(int index)
        {
            Internal_RemoveInstance(unmanagedPtr, index);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_RemoveInstance(IntPtr obj, int index);

        /// <summary>
        /// Sets the foliage instance transformation. Ensure to always call <see cref="RebuildClusters"/> after editing foliage to sync cached data (call it once after editing one or more instances).
        /// </summary>
        /// <param name="index">The zero-based index of the foliage instance.</param>
        /// <param name="value">The value.</param>
        public void SetInstanceTransform(int index, ref Transform value)
        {
            Internal_SetInstanceTransform(unmanagedPtr, index, ref value);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetInstanceTransform(IntPtr obj, int index, ref Transform value);

        /// <summary>
        /// Rebuilds the foliage clusters used as internal acceleration structures (quad tree).
        /// </summary>
        public void RebuildClusters()
        {
            Internal_RebuildClusters(unmanagedPtr);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_RebuildClusters(IntPtr obj);

        /// <summary>
        /// Updates the cull distance for all foliage instances and for created clusters.
        /// </summary>
        public void UpdateCullDistance()
        {
            Internal_UpdateCullDistance(unmanagedPtr);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_UpdateCullDistance(IntPtr obj);

        /// <summary>
        /// Determines if there is an intersection between the current object or any it's child and a ray.
        /// </summary>
        /// <param name="ray">The ray to test.</param>
        /// <param name="distance">When the method completes, contains the distance of the intersection (if any valid).</param>
        /// <param name="normal">When the method completes, contains the intersection surface normal vector (if any valid).</param>
        /// <param name="instanceIndex">When the method completes, contains zero-based index of the foliage instance that is the closest to the ray.</param>
        /// <returns>True whether the two objects intersected, otherwise false.</returns>
        public bool Intersects(ref Ray ray, out float distance, out Vector3 normal, out int instanceIndex)
        {
            return Internal_Intersects(unmanagedPtr, ref ray, out distance, out normal, out instanceIndex);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_Intersects(IntPtr obj, ref Ray ray, out float distance, out Vector3 normal, out int instanceIndex);
    }
}
