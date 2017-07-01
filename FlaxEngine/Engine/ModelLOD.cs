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

        internal ModelLOD(Model model)
        {
            _model = model;
        }
    }
}
