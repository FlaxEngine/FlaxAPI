////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2018 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

namespace FlaxEngine
{
	public sealed partial class Material
	{
	    /// <summary>
	    /// The asset type content domain.
	    /// </summary>
	    public const ContentDomain Domain = ContentDomain.Material;

        /// <inheritdoc />
        public override MaterialInstance CreateVirtualInstance()
        {
            var instance = Content.CreateVirtualAsset<MaterialInstance>();
            instance.BaseMaterial = this;
            return instance;
        }
    }
}
