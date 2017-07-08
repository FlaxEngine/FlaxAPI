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
	public sealed partial class EnvironmentProbe
	{
        /// <summary>
        /// Gets a value indicating whether this instance has probe texture assigned.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance has probe; otherwise, <c>false</c>.
        /// </value>
        public bool HasProbe => Probe;
	}
}
