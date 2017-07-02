////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlaxEngine
{
    /// <summary>
    /// Represents single Level Of Detail for the Model.
    /// Contains collection of meshes.
    /// </summary>
    public sealed class ModelLOD
    {
        private Model _model;
        private int _lodIndex;

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
