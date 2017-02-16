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
	/// Spot Light can emmit light from point in space in a given direction
	/// </summary>
	public sealed partial class SpotLight
	{
        // TODO: expose ShadowsSharpness

        /// <summary>
        /// Gets light scaled radius parameter
        /// </summary>
        [UnmanagedCall]
        public float ScaledRadius
        {
            get { return Scale.MaxValue * Radius; }
        }
    }
}