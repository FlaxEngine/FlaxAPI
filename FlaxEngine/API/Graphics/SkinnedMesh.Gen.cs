// This code was auto-generated. Do not modify it.

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace FlaxEngine
{
    /// <summary>
    /// Represents part of the skinned model that is made of vertices and can be rendered using custom material, transformation and skeleton bones hierarchy.
    /// </summary>
    [Tooltip("Represents part of the skinned model that is made of vertices and can be rendered using custom material, transformation and skeleton bones hierarchy.")]
    public unsafe partial class SkinnedMesh : FlaxEngine.Object
    {
        /// <inheritdoc />
        protected SkinnedMesh() : base()
        {
        }

        /// <summary>
        /// Gets or sets the material slot index.
        /// </summary>
        [Tooltip("The material slot index.")]
        public int MaterialSlotIndex
        {
            get { return Internal_GetMaterialSlotIndex(unmanagedPtr); }
            set { Internal_SetMaterialSlotIndex(unmanagedPtr, value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern int Internal_GetMaterialSlotIndex(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetMaterialSlotIndex(IntPtr obj, int value);

        /// <summary>
        /// Gets the triangle count.
        /// </summary>
        [Tooltip("The triangle count.")]
        public int TriangleCount
        {
            get { return Internal_GetTriangleCount(unmanagedPtr); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern int Internal_GetTriangleCount(IntPtr obj);

        /// <summary>
        /// Gets the vertex count.
        /// </summary>
        [Tooltip("The vertex count.")]
        public int VertexCount
        {
            get { return Internal_GetVertexCount(unmanagedPtr); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern int Internal_GetVertexCount(IntPtr obj);

        /// <summary>
        /// Determines whether this mesh is using 16 bit index buffer, otherwise it's 32 bit.
        /// </summary>
        [Tooltip("Determines whether this mesh is using 16 bit index buffer, otherwise it's 32 bit.")]
        public bool Use16BitIndexBuffer
        {
            get { return Internal_Use16BitIndexBuffer(unmanagedPtr); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_Use16BitIndexBuffer(IntPtr obj);

        /// <summary>
        /// Gets the box.
        /// </summary>
        [Tooltip("The box.")]
        public BoundingBox Box
        {
            get { Internal_GetBox(unmanagedPtr, out var resultAsRef); return resultAsRef; }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_GetBox(IntPtr obj, out BoundingBox resultAsRef);

        /// <summary>
        /// Gets the sphere.
        /// </summary>
        [Tooltip("The sphere.")]
        public BoundingSphere Sphere
        {
            get { Internal_GetSphere(unmanagedPtr, out var resultAsRef); return resultAsRef; }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_GetSphere(IntPtr obj, out BoundingSphere resultAsRef);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern FlaxEngine.Object Internal_GetParentModel(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_UpdateMeshInt(IntPtr obj, Array verticesObj, Array trianglesObj, Array blendIndicesObj, Array blendWeightsObj, Array normalsObj, Array tangentsObj, Array uvObj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_UpdateMeshUShort(IntPtr obj, Array verticesObj, Array trianglesObj, Array blendIndicesObj, Array blendWeightsObj, Array normalsObj, Array tangentsObj, Array uvObj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_DownloadBuffer(IntPtr obj, bool forceGpu, Array resultObj, int typeI);
    }
}
