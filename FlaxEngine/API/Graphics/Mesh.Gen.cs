// This code was auto-generated. Do not modify it.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace FlaxEngine
{
    /// <summary>
    /// Represents part of the model that is made of vertices and can be rendered using custom material and transformation.
    /// </summary>
    [Tooltip("Represents part of the model that is made of vertices and can be rendered using custom material and transformation.")]
    public unsafe partial class Mesh : FlaxEngine.Object
    {
        /// <inheritdoc />
        protected Mesh() : base()
        {
        }

        /// <summary>
        /// Gets or sets the index of the material slot to use during this mesh rendering.
        /// </summary>
        [Tooltip("The index of the material slot to use during this mesh rendering.")]
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
        /// Determines whether this mesh has a vertex colors buffer.
        /// </summary>
        [Tooltip("Determines whether this mesh has a vertex colors buffer.")]
        public bool HasVertexColors
        {
            get { return Internal_HasVertexColors(unmanagedPtr); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_HasVertexColors(IntPtr obj);

        /// <summary>
        /// Determines whether this mesh contains valid lightmap texture coordinates data.
        /// </summary>
        [Tooltip("Determines whether this mesh contains valid lightmap texture coordinates data.")]
        public bool HasLightmapUVs
        {
            get { return Internal_HasLightmapUVs(unmanagedPtr); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_HasLightmapUVs(IntPtr obj);

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

        /// <summary>
        /// Draws the mesh.
        /// </summary>
        /// <param name="renderContext">The rendering context.</param>
        /// <param name="material">The material to use for rendering.</param>
        /// <param name="world">The world transformation of the model.</param>
        /// <param name="flags">The object static flags.</param>
        /// <param name="receiveDecals">True if rendered geometry can receive decals, otherwise false.</param>
        public void Draw(ref RenderContext renderContext, MaterialBase material, ref Matrix world, StaticFlags flags = StaticFlags.None, bool receiveDecals = true)
        {
            Internal_Draw(unmanagedPtr, ref renderContext, FlaxEngine.Object.GetUnmanagedPtr(material), ref world, flags, receiveDecals);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_Draw(IntPtr obj, ref RenderContext renderContext, IntPtr material, ref Matrix world, StaticFlags flags, bool receiveDecals);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern FlaxEngine.Object Internal_GetParentModel(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_UpdateMeshInt(IntPtr obj, int vertexCount, int triangleCount, Array verticesObj, Array trianglesObj, Array normalsObj, Array tangentsObj, Array uvObj, Array colorsObj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_UpdateMeshUShort(IntPtr obj, int vertexCount, int triangleCount, Array verticesObj, Array trianglesObj, Array normalsObj, Array tangentsObj, Array uvObj, Array colorsObj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_UpdateTrianglesInt(IntPtr obj, int triangleCount, Array trianglesObj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_UpdateTrianglesUShort(IntPtr obj, int triangleCount, Array trianglesObj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_DownloadBuffer(IntPtr obj, bool forceGpu, Array resultObj, int typeI);
    }
}
