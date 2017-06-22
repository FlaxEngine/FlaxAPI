// Flax Engine scripting API

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlaxEngine
{
    public sealed class MaterialInstance : MaterialBase
    {
        /// <summary>
        /// The material instance asset type unique ID.
        /// </summary>
        public const int TypeID = 4;

        /// <summary>
        /// The asset type content domain.
        /// </summary>
        public const ContentDomain Domain = ContentDomain.Material;
    }
}
