// This code was auto-generated. Do not modify it.

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace FlaxEngine
{
    /// <summary>
    /// A special type of volume that defines the areas of the scene in which navigation meshes are generated.
    /// </summary>
    [Tooltip("A special type of volume that defines the areas of the scene in which navigation meshes are generated.")]
    public unsafe partial class NavMeshBoundsVolume : Actor
    {
        /// <inheritdoc />
        protected NavMeshBoundsVolume() : base()
        {
        }

        /// <summary>
        /// Creates new instance of <see cref="NavMeshBoundsVolume"/> object.
        /// </summary>
        /// <returns>The created object.</returns>
        public static NavMeshBoundsVolume New()
        {
            return Internal_Create(typeof(NavMeshBoundsVolume)) as NavMeshBoundsVolume;
        }

        /// <summary>
        /// Gets or sets the size of the volume (in local space).
        /// </summary>
        [EditorDisplay("Nav Mesh Bounds"), DefaultValue(typeof(Vector3), "1000,1000,1000"), EditorOrder(0)]
        [Tooltip("The size of the volume (in local space).")]
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
        /// Gets the volume bounding box (oriented in world space).
        /// </summary>
        [Tooltip("The volume bounding box (oriented in world space).")]
        public OrientedBoundingBox OrientedBox
        {
            get { Internal_GetOrientedBox(unmanagedPtr, out var resultAsRef); return resultAsRef; }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_GetOrientedBox(IntPtr obj, out OrientedBoundingBox resultAsRef);
    }
}
