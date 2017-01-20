// Flax Engine scripting API

using System;
using System.Runtime.CompilerServices;

namespace FlaxEngine
{
    public partial class SpotLight
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
