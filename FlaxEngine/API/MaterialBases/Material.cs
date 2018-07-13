// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

namespace FlaxEngine
{
    public sealed partial class Material
    {
        /// <inheritdoc />
        public override MaterialInstance CreateVirtualInstance()
        {
            var instance = Content.CreateVirtualAsset<MaterialInstance>();
            instance.BaseMaterial = this;
            return instance;
        }
    }
}
