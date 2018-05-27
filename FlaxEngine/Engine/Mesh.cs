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

#if !UNIT_TEST_COMPILANT
        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern int Internal_GetMaterialSlotIndex(IntPtr obj, int lodIndex, int meshIndex);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetMaterialSlotIndex(IntPtr obj, int lodIndex, int meshIndex, int value);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern int Internal_GetTriangleCount(IntPtr obj, int lodIndex, int meshIndex);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern int Internal_GetVertexCount(IntPtr obj, int lodIndex, int meshIndex);
#endif
    }
}
