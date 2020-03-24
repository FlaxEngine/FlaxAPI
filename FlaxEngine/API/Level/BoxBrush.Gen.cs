// This code was auto-generated. Do not modify it.

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace FlaxEngine
{
    /// <summary>
    /// Represents a part of the CSG brush actor. Contains information about single surface.
    /// </summary>
    [Tooltip("Represents a part of the CSG brush actor. Contains information about single surface.")]
    [StructLayout(LayoutKind.Sequential)]
    public unsafe partial struct BrushSurface
    {
        /// <summary>
        /// The parent brush.
        /// </summary>
        [HideInEditor]
        [Tooltip("The parent brush.")]
        public BoxBrush Brush;

        /// <summary>
        /// The surface index in the parent brush surfaces list.
        /// </summary>
        [HideInEditor]
        [Tooltip("The surface index in the parent brush surfaces list.")]
        public int Index;

        /// <summary>
        /// The material used to render the brush surface.
        /// </summary>
        [EditorOrder(10), EditorDisplay("Brush")]
        [Tooltip("The material used to render the brush surface.")]
        public MaterialBase Material;

        /// <summary>
        /// The surface texture coordinates scale.
        /// </summary>
        [EditorOrder(30), EditorDisplay("Brush", "UV Scale"), Limit(-1000, 1000, 0.01f)]
        [Tooltip("The surface texture coordinates scale.")]
        public Vector2 TexCoordScale;

        /// <summary>
        /// The surface texture coordinates offset.
        /// </summary>
        [EditorOrder(40), EditorDisplay("Brush", "UV Offset"), Limit(-1000, 1000, 0.01f)]
        [Tooltip("The surface texture coordinates offset.")]
        public Vector2 TexCoordOffset;

        /// <summary>
        /// The surface texture coordinates rotation angle (in degrees).
        /// </summary>
        [EditorOrder(50), EditorDisplay("Brush", "UV Rotation")]
        [Tooltip("The surface texture coordinates rotation angle (in degrees).")]
        public float TexCoordRotation;

        /// <summary>
        /// The scale in lightmap (per surface).
        /// </summary>
        [EditorOrder(20), EditorDisplay("Brush", "Scale In Lightmap"), Limit(0, 10000, 0.1f)]
        [Tooltip("The scale in lightmap (per surface).")]
        public float ScaleInLightmap;
    }
}

namespace FlaxEngine
{
    /// <summary>
    /// Performs CSG box brush operation that adds or removes geometry.
    /// </summary>
    [Tooltip("Performs CSG box brush operation that adds or removes geometry.")]
    public unsafe partial class BoxBrush : Actor
    {
        /// <inheritdoc />
        protected BoxBrush() : base()
        {
        }

        /// <summary>
        /// Creates new instance of <see cref="BoxBrush"/> object.
        /// </summary>
        /// <returns>The created object.</returns>
        public static BoxBrush New()
        {
            return Internal_Create(typeof(BoxBrush)) as BoxBrush;
        }

        /// <summary>
        /// Brush surfaces scale in lightmap
        /// </summary>
        [EditorOrder(30), DefaultValue(1.0f), EditorDisplay("CSG", "Scale In Lightmap"), Limit(0, 1000.0f, 0.1f)]
        [Tooltip("Brush surfaces scale in lightmap")]
        public float ScaleInLightmap
        {
            get { return Internal_GetScaleInLightmap(unmanagedPtr); }
            set { Internal_SetScaleInLightmap(unmanagedPtr, value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern float Internal_GetScaleInLightmap(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetScaleInLightmap(IntPtr obj, float value);

        /// <summary>
        /// Gets or sets the brush proxies per surface.
        /// </summary>
        [Serialize, EditorOrder(100), EditorDisplay("Surfaces", EditorDisplayAttribute.InlineStyle), MemberCollection(CanReorderItems = false, NotNullItems = true, ReadOnly = true)]
        [Tooltip("The brush proxies per surface.")]
        public BrushSurface[] Surfaces
        {
            get { return Internal_GetSurfaces(unmanagedPtr, typeof(BrushSurface)); }
            set { Internal_SetSurfaces(unmanagedPtr, value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern BrushSurface[] Internal_GetSurfaces(IntPtr obj, System.Type resultArrayItemType0);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetSurfaces(IntPtr obj, BrushSurface[] value);

        /// <summary>
        /// Gets or sets the CSG brush mode.
        /// </summary>
        [EditorOrder(10), DefaultValue(BrushMode.Additive), EditorDisplay("CSG")]
        [Tooltip("The CSG brush mode.")]
        public BrushMode Mode
        {
            get { return Internal_GetMode(unmanagedPtr); }
            set { Internal_SetMode(unmanagedPtr, value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern BrushMode Internal_GetMode(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetMode(IntPtr obj, BrushMode value);

        /// <summary>
        /// Gets or sets the brush center (in local space).
        /// </summary>
        [EditorOrder(21), DefaultValue(typeof(Vector3), "0,0,0"), EditorDisplay("CSG")]
        [Tooltip("The brush center (in local space).")]
        public Vector3 Center
        {
            get { Internal_GetCenter(unmanagedPtr, out var resultAsRef); return resultAsRef; }
            set { Internal_SetCenter(unmanagedPtr, ref value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_GetCenter(IntPtr obj, out Vector3 resultAsRef);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetCenter(IntPtr obj, ref Vector3 value);

        /// <summary>
        /// Gets or sets the brush size.
        /// </summary>
        [EditorOrder(20), EditorDisplay("CSG")]
        [Tooltip("The brush size.")]
        public Vector3 Size
        {
            get { Internal_GetSize(unmanagedPtr, out var resultAsRef); return resultAsRef; }
            set { Internal_SetSize(unmanagedPtr, ref value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_GetSize(IntPtr obj, out Vector3 resultAsRef);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetSize(IntPtr obj, ref Vector3 value);

        /// <summary>
        /// Gets the volume bounding box (oriented).
        /// </summary>
        [Tooltip("The volume bounding box (oriented).")]
        public OrientedBoundingBox OrientedBox
        {
            get { Internal_GetOrientedBox(unmanagedPtr, out var resultAsRef); return resultAsRef; }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_GetOrientedBox(IntPtr obj, out OrientedBoundingBox resultAsRef);

        /// <summary>
        /// Determines if there is an intersection between the brush surface and a ray.
        /// If collision data is available on the CPU performs exact intersection check with the geometry.
        /// Otherwise performs simple <see cref="BoundingBox"/> vs <see cref="Ray"/> test.
        /// For more efficient collisions detection and ray casting use physics.
        /// </summary>
        /// <param name="surfaceIndex">The brush surface index..</param>
        /// <param name="ray">The ray to test.</param>
        /// <param name="distance">When the method completes and returns true, contains the distance of the intersection (if any valid).</param>
        /// <param name="normal">When the method completes, contains the intersection surface normal vector (if any valid).</param>
        /// <returns>True if the actor is intersected by the ray, otherwise false.</returns>
        public bool Intersects(int surfaceIndex, ref Ray ray, out float distance, out Vector3 normal)
        {
            return Internal_Intersects(unmanagedPtr, surfaceIndex, ref ray, out distance, out normal);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_Intersects(IntPtr obj, int surfaceIndex, ref Ray ray, out float distance, out Vector3 normal);

        /// <summary>
        /// Gets the brush surface triangles array (group by 3 vertices).
        /// </summary>
        /// <param name="surfaceIndex">The brush surface index..</param>
        /// <param name="outputData">The output vertices buffer with triangles or empty if no data loaded.</param>
        public void GetVertices(int surfaceIndex, out Vector3[] outputData)
        {
            Internal_GetVertices(unmanagedPtr, surfaceIndex, out outputData, typeof(Vector3));
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_GetVertices(IntPtr obj, int surfaceIndex, out Vector3[] outputData, System.Type resultArrayItemType0);
    }
}
