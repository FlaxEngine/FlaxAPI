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

        internal Mesh(Model model, int lodIndex, int meshIndex)
        {
            _model = model;
            _lodIndex = lodIndex;
            _meshIndex = meshIndex;
        }
    }
}
