// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System;
using System.Runtime.CompilerServices;

namespace FlaxEngine
{
    /// <summary>
    /// Represents part of the skinned model that is made of verticies which can be rendered.
    /// </summary>
    public sealed class SkinnedMesh
    {
        internal SkinnedModel _skinnedModel;
        internal readonly int _index;

        /// <summary>
        /// Gets the parent skinned model asset.
        /// </summary>
        public SkinnedModel ParentSkinnedModel => _skinnedModel;

        /// <summary>
        /// Gets the index of the mesh.
        /// </summary>
        public int MeshIndex => _index;

        /// <summary>
        /// Gets the index of the material slot to use during this mesh rendering.
        /// </summary>
        public int MaterialSlotIndex
        {
            get => Internal_GetMaterialSlotIndex(_skinnedModel.unmanagedPtr, _index);
            set => Internal_SetMaterialSlotIndex(_skinnedModel.unmanagedPtr, _index, value);
        }

        /// <summary>
        /// Gets the material slot used by this mesh during rendering.
        /// </summary>
        public MaterialSlot MaterialSlot => _skinnedModel.MaterialSlots[MaterialSlotIndex];

        /// <summary>
        /// Gets the triangle count.
        /// </summary>
        public int Triangles => Internal_GetTriangleCount(_skinnedModel.unmanagedPtr, _index);

        /// <summary>
        /// Gets the vertex count.
        /// </summary>
        public int Vertices => Internal_GetVertexCount(_skinnedModel.unmanagedPtr, _index);

        internal SkinnedMesh(SkinnedModel model, int index)
        {
            _skinnedModel = model;
            _index = index;
        }

        /// <summary>
        /// Updates the skinned model mesh vertex and index buffer data.
        /// Can be used only for virtual assets (see <see cref="Asset.IsVirtual"/> and <see cref="Content.CreateVirtualAsset{T}"/>).
        /// Mesh data will be cached and uploaded to the GPU with a delay.
        /// </summary>
        /// <param name="vertices">The mesh verticies positions. Cannot be null.</param>
        /// <param name="triangles">The mesh index buffer (triangles). Uses 32-bit stride buffer. Cannot be null.</param>
        /// <param name="blendIndices">The skinned mesh blend indices buffer. Contains indices of the skeleton bones (up to 4 bones per vertex) to use for vertex position blending. Cannot be null.</param>
        /// <param name="blendWeights">The skinned mesh blend weights buffer (normalized). Contains weights per blend bone (up to 4 bones per vertex) of the skeleton bones to mix for vertex position blending. Cannot be null.</param>
        /// <param name="normals">The normal vectors (per vertex).</param>
        /// <param name="tangents">The normal vectors (per vertex). Use null to compute them from normal vectors.</param>
        /// <param name="uv">The texture cordinates (per vertex).</param>
        public void UpdateMesh(Vector3[] vertices, int[] triangles, Int4[] blendIndices, Vector4[] blendWeights, Vector3[] normals = null, Vector3[] tangents = null, Vector2[] uv = null)
        {
            // Validate state and input
            if (!_skinnedModel.IsVirtual)
                throw new InvalidOperationException("Only virtual skinned models can be updated at runtime.");
            if (vertices == null)
                throw new ArgumentNullException(nameof(vertices));
            if (triangles == null)
                throw new ArgumentNullException(nameof(triangles));
            if (triangles.Length == 0 || triangles.Length % 3 != 0)
                throw new ArgumentOutOfRangeException(nameof(triangles));
            if (normals != null && normals.Length != vertices.Length)
                throw new ArgumentOutOfRangeException(nameof(normals));
            if (tangents != null && tangents.Length != vertices.Length)
                throw new ArgumentOutOfRangeException(nameof(tangents));
            if (tangents != null && normals == null)
                throw new ArgumentException("If you specify tangents then you need to also provide normals for the mesh.");
            if (uv != null && uv.Length != vertices.Length)
                throw new ArgumentOutOfRangeException(nameof(uv));

            if (Internal_UpdateMeshInt(_skinnedModel.unmanagedPtr, _index, vertices, triangles, blendIndices, blendWeights, normals, tangents, uv))
                throw new FlaxException("Failed to update mesh data.");
        }

        /// <summary>
        /// Updates the skinned model mesh vertex and index buffer data.
        /// Can be used only for virtual assets (see <see cref="Asset.IsVirtual"/> and <see cref="Content.CreateVirtualAsset{T}"/>).
        /// Mesh data will be cached and uploaded to the GPU with a delay.
        /// </summary>
        /// <param name="vertices">The mesh verticies positions. Cannot be null.</param>
        /// <param name="triangles">The mesh index buffer (triangles). Uses 16-bit stride buffer. Cannot be null.</param>
        /// <param name="blendIndices">The skinned mesh blend indices buffer. Contains indices of the skeleton bones (up to 4 bones per vertex) to use for vertex position blending. Cannot be null.</param>
        /// <param name="blendWeights">The skinned mesh blend weights buffer (normalized). Contains weights per blend bone (up to 4 bones per vertex) of the skeleton bones to mix for vertex position blending. Cannot be null.</param>
        /// <param name="normals">The normal vectors (per vertex).</param>
        /// <param name="tangents">The tangent vectors (per vertex). Use null to compute them from normal vectors.</param>
        /// <param name="uv">The texture cordinates (per vertex).</param>
        public void UpdateMesh(Vector3[] vertices, ushort[] triangles, Int4[] blendIndices, Vector4[] blendWeights, Vector3[] normals = null, Vector3[] tangents = null, Vector2[] uv = null)
        {
            // Validate state and input
            if (!_skinnedModel.IsVirtual)
                throw new InvalidOperationException("Only virtual skinned models can be updated at runtime.");
            if (vertices == null)
                throw new ArgumentNullException(nameof(vertices));
            if (triangles == null)
                throw new ArgumentNullException(nameof(triangles));
            if (triangles.Length == 0 || triangles.Length % 3 != 0)
                throw new ArgumentOutOfRangeException(nameof(triangles));
            if (normals != null && normals.Length != vertices.Length)
                throw new ArgumentOutOfRangeException(nameof(normals));
            if (tangents != null && tangents.Length != vertices.Length)
                throw new ArgumentOutOfRangeException(nameof(tangents));
            if (tangents != null && normals == null)
                throw new ArgumentException("If you specify tangents then you need to also provide normals for the mesh.");
            if (uv != null && uv.Length != vertices.Length)
                throw new ArgumentOutOfRangeException(nameof(uv));

            if (Internal_UpdateMeshUShort(_skinnedModel.unmanagedPtr, _index, vertices, triangles, blendIndices, blendWeights, normals, tangents, uv))
                throw new FlaxException("Failed to update mesh data.");
        }

#if !UNIT_TEST_COMPILANT
        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern int Internal_GetMaterialSlotIndex(IntPtr obj, int index);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetMaterialSlotIndex(IntPtr obj, int index, int value);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern int Internal_GetTriangleCount(IntPtr obj, int index);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern int Internal_GetVertexCount(IntPtr obj, int index);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_UpdateMeshInt(IntPtr obj, int meshIndex, Vector3[] vertices, int[] triangles, Int4[] blendIndices, Vector4[] blendWeights, Vector3[] normals, Vector3[] tangents, Vector2[] uv);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_UpdateMeshUShort(IntPtr obj, int meshIndex, Vector3[] vertices, ushort[] triangles, Int4[] blendIndices, Vector4[] blendWeights, Vector3[] normals, Vector3[] tangents, Vector2[] uv);
#endif
    }
}
