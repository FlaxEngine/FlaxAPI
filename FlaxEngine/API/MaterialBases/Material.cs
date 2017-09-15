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
	public sealed partial class Material
	{
	    /// <summary>
	    /// The material asset type unique ID.
	    /// </summary>
	    public const int TypeID = 2;

	    /// <summary>
	    /// The asset type content domain.
	    /// </summary>
	    public const ContentDomain Domain = ContentDomain.Material;

        /// <summary>
        /// Creates the virtual material instance of this material which allows to override any material parameters.
        /// </summary>
        /// <returns>The created virtual material instance asset.</returns>
        public MaterialInstance CreateVirtualInstance()
        {
            var instance = Content.CreateVirtualAsset<MaterialInstance>();
            instance.BaseMaterial = this;
            return instance;
        }
    }
}
