// Flax Engine scripting API

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlaxEngine
{
    /// <summary>
    /// The shader asset contains a GPU programs called shaders used to draw graphics and visual effects.
    /// </summary>
    /// <seealso cref="FlaxEngine.BinaryAsset" />
    public sealed class Shader : BinaryAsset
    {
        /// <summary>
        /// The shader asset type unique ID.
        /// </summary>
        public const int TypeID = 7;

        /// <summary>
        /// The asset type content domain.
        /// </summary>
        public const ContentDomain Domain = ContentDomain.Shader;
    }
}
