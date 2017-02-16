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
	/// Environment Probe can capture space around the objects to provide reflections
	/// </summary>
	public sealed partial class EnvironmentProbe
	{
        // TODO: HasProbe, IsUsingCustomProbe, SetCustomprobe
        // TODO: Bake()

        /// <summary>
        /// Gets probe scaled radius parameter
        /// </summary>
        [UnmanagedCall]
        public float ScaledRadius
        {
            get { return Scale.MaxValue * Radius; }
        }
    }
}