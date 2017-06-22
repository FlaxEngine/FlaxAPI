// Flax Engine scripting API

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlaxEngine
{
    public sealed class CubeTexture : BinaryAsset
    {
        /// <summary>
        /// The cube texture asset type unique ID.
        /// </summary>
        public const int TypeID = 8;

        /// <summary>
        /// The asset type content domain.
        /// </summary>
        public const ContentDomain Domain = ContentDomain.CubeTexture;
    }
}
