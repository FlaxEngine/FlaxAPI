// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System;
using System.Runtime.CompilerServices;

namespace FlaxEngine
{
    /// <summary>
    /// Represents part of the model that is made of verticies which can be rendered (can have own transformation and material).
    /// </summary>
    public sealed class Mesh
    {
        // TODO: use hash to check if data is valid (like MaterialParameter)
        internal Model _model;

        internal readonly int _lodIndex;
        internal readonly int _meshIndex;

        /// <summary>
        /// Gets the parent model asset.
        /// </summary>
        public Model ParentModel => _model;

        /// <summary>
        /// Gets the parent level of detail object.
        /// </summary>
        public ModelLOD ParentLOD => _model.LODs[_lodIndex];

        /// <summary>
        /// Gets the index of the mesh (in the parnet level of detail).
        /// </summary>
        public int MeshIndex => _meshIndex;

        /// <summary>
        /// Gets the index of the material slot to use during this mesh rendering.
        /// </summary>
        public int MaterialSlotIndex
        {
            get => Internal_GetMaterialSlotIndex(_model.unmanagedPtr, _lodIndex, _meshIndex);
            set => Internal_SetMaterialSlotIndex(_model.unmanagedPtr, _lodIndex, _meshIndex, value);
        }

        /// <summary>
        /// Gets the material slot used by this mesh during rendering.
        /// </summary>
        public MaterialSlot MaterialSlot => _model.MaterialSlots[MaterialSlotIndex];

        /// <summary>
        /// Gets the triangle count.
        /// </summary>
        public int Triangles => Internal_GetTriangleCount(_model.unmanagedPtr, _lodIndex, _meshIndex);

        /// <summary>
        /// Gets the vertex count.
        /// </summary>
        public int Vertices => Internal_GetVertexCount(_model.unmanagedPtr, _lodIndex, _meshIndex);

        internal Mesh(Model model, int lodIndex, int meshIndex)
        {
            _model = model;
            _lodIndex = lodIndex;
            _meshIndex = meshIndex;
        }

        /// <summary>
        /// Updates the model mesh vertex and index buffer data.
        /// Can be used only for virtual assets (see <see cref="Asset.IsVirtual"/> and <see cref="Content.CreateVirtualAsset{T}"/>).
        /// Mesh data will be cached and uploaded to the GPU with a delay.
        /// </summary>
        /// <param name="vertices">The mesh verticies positions. Cannot be null.</param>
        /// <param name="triangles">The mesh index buffer (triangles). Uses 32-bit stride buffer. Cannot be null.</param>
        /// <param name="normals">The normal vectors (per vertex).</param>
        /// <param name="tangents">The normal vectors (per vertex). Use null to compute them from normal vectors.</param>
        /// <param name="uv">The texture cordinates (per vertex).</param>
        /// <param name="colors">The vertex colors (per vertex).</param>
        public void UpdateMesh(Vector3[] vertices, int[] triangles, Vector3[] normals = null, Vector3[] tangents = null, Vector2[] uv = null, Color32[] colors = null)
        {
            // Validate state and input
            if (!_model.IsVirtual)
                throw new InvalidOperationException("Only virtual models can be updated at runtime.");
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
            if (colors != null && colors.Length != vertices.Length)
                throw new ArgumentOutOfRangeException(nameof(colors));

            if (Internal_UpdateMeshInt(_model.unmanagedPtr, _lodIndex, _meshIndex, vertices, triangles, normals, tangents, uv, colors))
                throw new FlaxException("Failed to update mesh data.");
        }

        /// <summary>
        /// Updates the model mesh vertex and index buffer data.
        /// Can be used only for virtual assets (see <see cref="Asset.IsVirtual"/> and <see cref="Content.CreateVirtualAsset{T}"/>).
        /// Mesh data will be cached and uploaded to the GPU with a delay.
        /// </summary>
        /// <param name="vertices">The mesh verticies positions. Cannot be null.</param>
        /// <param name="triangles">The mesh index buffer (triangles). Uses 16-bit stride buffer. Cannot be null.</param>
        /// <param name="normals">The normal vectors (per vertex).</param>
        /// <param name="tangents">The tangent vectors (per vertex). Use null to compute them from normal vectors.</param>
        /// <param name="uv">The texture cordinates (per vertex).</param>
        /// <param name="colors">The vertex colors (per vertex).</param>
        public void UpdateMesh(Vector3[] vertices, ushort[] triangles, Vector3[] normals = null, Vector3[] tangents = null, Vector2[] uv = null, Color32[] colors = null)
        {
            // Validate state and input
            if (!_model.IsVirtual)
                throw new InvalidOperationException("Only virtual models can be updated at runtime.");
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
            if (colors != null && colors.Length != vertices.Length)
                throw new ArgumentOutOfRangeException(nameof(colors));

            if (Internal_UpdateMeshUShort(_model.unmanagedPtr, _lodIndex, _meshIndex, vertices, triangles, normals, tangents, uv, colors))
                throw new FlaxException("Failed to update mesh data.");
        }

#if !UNIT_TEST_COMPILANT
        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern int Internal_GetMaterialSlotIndex(IntPtr obj, int lodIndex, int meshIndex);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetMaterialSlotIndex(IntPtr obj, int lodIndex, int meshIndex, int value);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern int Internal_GetTriangleCount(IntPtr obj, int lodIndex, int meshIndex);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern int Internal_GetVertexCount(IntPtr obj, int lodIndex, int meshIndex);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_UpdateMeshInt(IntPtr obj, int lodIndex, int meshIndex, Vector3[] vertices, int[] triangles, Vector3[] normals, Vector3[] tangents, Vector2[] uv, Color32[] colors);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_UpdateMeshUShort(IntPtr obj, int lodIndex, int meshIndex, Vector3[] vertices, ushort[] triangles, Vector3[] normals, Vector3[] tangents, Vector2[] uv, Color32[] colors);
#endif
    }
}
