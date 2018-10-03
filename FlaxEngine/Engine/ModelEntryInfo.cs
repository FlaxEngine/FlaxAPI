// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System;
using System.Runtime.CompilerServices;

namespace FlaxEngine
{
    /// <summary>
    /// Represents a part of the model actor entries collection. Contains information about how to render <see cref="Mesh"/>.
    /// </summary>
    public sealed class ModelEntryInfo
    {
        [Serialize]
        internal Actor _actor;

        [Serialize]
        internal readonly int _index;

        /// <summary>
        /// Gets or sets the mesh local transform.
        /// </summary>
        [EditorOrder(40), Tooltip("Custom mesh local transformation applied during rendering.")]
        public Transform Transform
        {
            get
            {
                Transform resultAsRef;
                Internal_GetMeshTransform(_actor.unmanagedPtr, _index, out resultAsRef);
                return resultAsRef;
            }
            set => Internal_SetMeshTransform(_actor.unmanagedPtr, _index, ref value);
        }

        /// <summary>
        /// Gets or sets the material used to render the mesh.
        /// If value if null then model asset mesh default material will be used as a fallback.
        /// </summary>
        [EditorOrder(10), Tooltip("The mesh material used for the rendering. If not assigned the default value will be used from the model asset.")]
        public MaterialBase Material
        {
            get => Internal_GetMeshMaterial(_actor.unmanagedPtr, _index);
            set => Internal_SetMeshMaterial(_actor.unmanagedPtr, _index, Object.GetUnmanagedPtr(value));
        }

        /// <summary>
        /// Gets or sets the scale in lightmap (per mesh).
        /// Final mesh scale in lightmap is also multiplied by <see cref="StaticModel.ScaleInLightmap"/> and global scene scale parameter.
        /// </summary>
        [EditorOrder(20), EditorDisplay(null, "Scale In Lightmap"), Limit(0, 10000, 0.1f), Tooltip("Per mesh scale factor in lightmap charts. Higher value increases the quality but reduces baking performance.")]
        public float ScaleInLightmap
        {
            get => Internal_GetMeshScaleInLightmap(_actor.unmanagedPtr, _index);
            set => Internal_SetMeshScaleInLightmap(_actor.unmanagedPtr, _index, value);
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="ModelEntryInfo"/> is visible.
        /// </summary>
        [EditorOrder(30), Tooltip("Determines whenever this mesh is visible.")]
        public bool Visible
        {
            get => Internal_GetMeshVisible(_actor.unmanagedPtr, _index);
            set => Internal_SetMeshVisible(_actor.unmanagedPtr, _index, value);
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="ModelEntryInfo"/> can receive decals.
        /// </summary>
        [EditorOrder(40), Tooltip("Determines whenever this mesh can receive decals.")]
        public bool ReceiveDecals
        {
            get => Internal_GetMeshReceiveDecals(_actor.unmanagedPtr, _index);
            set => Internal_SetMeshReceiveDecals(_actor.unmanagedPtr, _index, value);
        }

        /// <summary>
        /// Gets or sets the shadows casting mode.
        /// </summary>
        [EditorOrder(50), Tooltip("Shadows casting mode by this mesh.")]
        public ShadowsCastingMode ShadowsMode
        {
            get => Internal_GetMeshShadowsMode(_actor.unmanagedPtr, _index);
            set => Internal_SetMeshShadowsMode(_actor.unmanagedPtr, _index, value);
        }

        /// <summary>
        /// Gets the mesh entry index.
        /// </summary>
        [HideInEditor]
        public int Index => _index;

        internal ModelEntryInfo()
        {
            // Used by the serialization system
        }

        internal ModelEntryInfo(Actor model, int index)
        {
            _actor = model;
            _index = index;
        }

        /// <summary>
        /// Determines if there is an intersection between the model actor mesh entry and a ray.
        /// If mesh data is available on the CPU performs exact intersection check with the geometry.
        /// Otherwise performs simple <see cref="BoundingBox"/> vs <see cref="Ray"/> test.
        /// For more efficient collisions detection and ray casting use physics.
        /// </summary>
        /// <param name="ray">The ray to test.</param>
        /// <param name="distance">When the method completes and returns true, contains the distance of the intersection.</param>
        /// <returns>True if the actor is intersected by the ray, otherwise false.</returns>
        public bool Intersects(Ray ray, out float distance)
        {
            return Internal_IntersectsEntry(_actor.unmanagedPtr, _index, ref ray, out distance);
        }

#if !UNIT_TEST_COMPILANT
        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_GetMeshTransform(IntPtr obj, int index, out Transform result);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetMeshTransform(IntPtr obj, int index, ref Transform value);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern MaterialBase Internal_GetMeshMaterial(IntPtr obj, int index);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetMeshMaterial(IntPtr obj, int index, IntPtr value);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern float Internal_GetMeshScaleInLightmap(IntPtr obj, int index);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetMeshScaleInLightmap(IntPtr obj, int index, float value);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_GetMeshVisible(IntPtr obj, int index);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetMeshVisible(IntPtr obj, int index, bool value);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_GetMeshReceiveDecals(IntPtr obj, int index);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetMeshReceiveDecals(IntPtr obj, int index, bool value);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern ShadowsCastingMode Internal_GetMeshShadowsMode(IntPtr obj, int index);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetMeshShadowsMode(IntPtr obj, int index, ShadowsCastingMode value);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_IntersectsEntry(IntPtr obj, int index, ref Ray ray, out float distance);
#endif
    }
}
