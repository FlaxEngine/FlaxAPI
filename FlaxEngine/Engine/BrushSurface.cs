// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System;
using System.Runtime.CompilerServices;

namespace FlaxEngine
{
    /// <summary>
    /// Represents a part of the CSG brush actor. Contains information about single surface.
    /// </summary>
    public class BrushSurface
    {
        [Serialize]
        internal BoxBrush _brushActor;

        [Serialize]
        internal readonly int _index;

        /// <summary>
        /// Gets the parent brush actor.
        /// </summary>
        [HideInEditor]
        public BoxBrush ParentActor => _brushActor;

        /// <summary>
        /// Gets or sets the material used to render the brush surface.
        /// </summary>
        [EditorOrder(10), EditorDisplay("Brush")]
        public MaterialBase Material
        {
            get => Internal_GetMaterial(_brushActor.unmanagedPtr, _index);
            set => Internal_SetMaterial(_brushActor.unmanagedPtr, _index, Object.GetUnmanagedPtr(value));
        }

        /// <summary>
        /// Gets or sets the scale in lightmap (per surface).
        /// </summary>
        [EditorOrder(20), EditorDisplay("Brush", "Scale In Lightmap"), Limit(0, 10000, 0.1f)]
        public float ScaleInLightmap
        {
            get => Internal_GetScaleInLightmap(_brushActor.unmanagedPtr, _index);
            set => Internal_SetScaleInLightmap(_brushActor.unmanagedPtr, _index, value);
        }

        /// <summary>
        /// Gets or sets the surface texture coordinates scale.
        /// </summary>
        [EditorOrder(30), EditorDisplay("Brush", "UV Scale"), Limit(-1000, 1000, 0.01f), Tooltip("Texture coordinates scale factor")]
        public Vector2 TexCoordScale
        {
            get
            {
                Vector2 result;
                Internal_GetUvScale(_brushActor.unmanagedPtr, _index, out result);
                return result;
            }
            set => Internal_SetUvScale(_brushActor.unmanagedPtr, _index, ref value);
        }

        /// <summary>
        /// Gets or sets the surface texture coordinates offset.
        /// </summary>
        [EditorOrder(40), EditorDisplay("Brush", "UV Offset"), Limit(-1000, 1000, 0.01f), Tooltip("Texture coordinates offset (additive)")]
        public Vector2 TexCoordOffset
        {
            get
            {
                Vector2 result;
                Internal_GetUvOffset(_brushActor.unmanagedPtr, _index, out result);
                return result;
            }
            set => Internal_SetUvOffset(_brushActor.unmanagedPtr, _index, ref value);
        }

        /// <summary>
        /// Gets or sets the surface texture coordinates rotation angle (in degrees).
        /// </summary>
        [EditorOrder(50), EditorDisplay("Brush", "UV Rotation"), Tooltip("Texture coordinates rotation angle (in degrees)")]
        public float TexCoordRotation
        {
            get => Internal_GetUvRotation(_brushActor.unmanagedPtr, _index);
            set => Internal_SetUvRotation(_brushActor.unmanagedPtr, _index, value);
        }

        /// <summary>
        /// Gets the brush surface index.
        /// </summary>
        [HideInEditor]
        public int Index => _index;

        internal BrushSurface()
        {
            // Used by the serialization system
        }

        internal BrushSurface(BoxBrush brush, int index)
        {
            _brushActor = brush;
            _index = index;
        }

        /// <summary>
        /// Determines if there is an intersection between the brush surface and a ray.
        /// If collision data is available on the CPU performs exact intersection check with the geometry.
        /// Otherwise performs simple <see cref="BoundingBox"/> vs <see cref="Ray"/> test.
        /// For more efficient collisions detection and ray casting use physics.
        /// </summary>
        /// <param name="ray">The ray to test.</param>
        /// <param name="distance">When the method completes and returns true, contains the distance of the intersection.</param>
        /// <returns>True if the actor is intersected by the ray, otherwise false.</returns>
        public bool Intersects(ref Ray ray, out float distance)
        {
            return Internal_Intersects(_brushActor.unmanagedPtr, _index, ref ray, out distance);
        }

        /// <summary>
        /// Gets the brush surface triangles array (group by 3 vertices).
        /// </summary>
        /// <returns>The vertices buffer with triangles or empty if no data loaded.</returns>
        public Vector3[] GetVertices()
        {
            return Internal_GetVertices(_brushActor.unmanagedPtr, _index);
        }

#if !UNIT_TEST_COMPILANT
        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern MaterialBase Internal_GetMaterial(IntPtr obj, int index);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetMaterial(IntPtr obj, int index, IntPtr value);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern float Internal_GetScaleInLightmap(IntPtr obj, int index);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetScaleInLightmap(IntPtr obj, int index, float value);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_GetUvScale(IntPtr obj, int index, out Vector2 result);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetUvScale(IntPtr obj, int index, ref Vector2 value);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_GetUvOffset(IntPtr obj, int index, out Vector2 result);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetUvOffset(IntPtr obj, int index, ref Vector2 value);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern float Internal_GetUvRotation(IntPtr obj, int index);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetUvRotation(IntPtr obj, int index, float value);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_Intersects(IntPtr obj, int index, ref Ray ray, out float distance);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern Vector3[] Internal_GetVertices(IntPtr obj, int index);
#endif
    }
}
