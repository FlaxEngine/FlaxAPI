// Flax Engine scripting API

using System;
using System.Runtime.CompilerServices;

namespace FlaxEngine
{
    public partial class EnvironmentProbe
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
