////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

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
        /// <value>
        /// The parent model.
        /// </value>
        public Model ParentModel => _model;

        /// <summary>
        /// Gets the parent level of detail object.
        /// </summary>
        /// <value>
        /// The parent LOD object.
        /// </value>
        public ModelLOD ParentLOD => _model.LODs[_lodIndex];

        /// <summary>
        /// Gets the index of the mesh (in the parnet level of detail).
        /// </summary>
        public int MeshIndex => _meshIndex;

        /// <summary>
        /// Gets the index of the material slot to use during this mesh rendering.
        /// </summary>
        public int MaterialSlotIndex => Model.Internal_GetMeshMaterialSlotIndex(_model.unmanagedPtr, _lodIndex, _meshIndex);
        
        internal Mesh(Model model, int lodIndex, int meshIndex)
        {
            _model = model;
            _lodIndex = lodIndex;
            _meshIndex = meshIndex;
        }
    }
}
