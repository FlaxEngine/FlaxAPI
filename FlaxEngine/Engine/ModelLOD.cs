////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

namespace FlaxEngine
{
    /// <summary>
    /// Represents single Level Of Detail for the Model.
    /// Contains collection of meshes.
    /// </summary>
    public sealed class ModelLOD
    {
        // TODO: use hash to check if data is valid (like MaterialParameter)
        internal readonly Model _model;
        internal readonly int _lodIndex;

        /// <summary>
        /// Gets the parent model asset.
        /// </summary>
        /// <value>
        /// The parent model.
        /// </value>
        public Model ParentModel => _model;

        /// <summary>
        /// The meshes array.
        /// </summary>
        public readonly Mesh[] Meshes;

        internal ModelLOD(Model model, int lodIndex, int meshesCount)
        {
            _model = model;
            _lodIndex = lodIndex;
            Meshes = new Mesh[meshesCount];
            for (int i = 0; i < meshesCount; i++)
            {
                Meshes[i] = new Mesh(model, lodIndex, i);
            }
        }
    }
}
