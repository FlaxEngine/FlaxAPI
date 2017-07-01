////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlaxEngine
{
    /// <summary>
    /// Represents part of the model that is made of verticies which can be rendered (can have own transformation and material).
    /// </summary>
    public sealed class Mesh
    {
        private Model _model;

        internal Mesh(Model model)
        {
            _model = model;
        }
    }
}
