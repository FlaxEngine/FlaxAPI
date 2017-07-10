// Flax Engine scripting API

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlaxEngine
{
    /// <summary>
    /// Base class for <see cref="Material"/> and <see cref="MaterialInstance"/>.
    /// </summary>
    /// <seealso cref="FlaxEngine.BinaryAsset" />
    public abstract class MaterialBase : BinaryAsset
    {
        /// <summary>
        /// Gets the material info, structure which describies material surface.
        /// </summary>
        public abstract Rendering.MaterialInfo Info { get; }
    }
}
