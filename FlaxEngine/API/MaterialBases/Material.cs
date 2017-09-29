////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

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

        /// <inheritdoc />
        public override MaterialInstance CreateVirtualInstance()
        {
            var instance = Content.CreateVirtualAsset<MaterialInstance>();
            instance.BaseMaterial = this;
            return instance;
        }
    }
}
