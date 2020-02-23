// Copyright (c) 2012-2020 Wojciech Figat. All rights reserved.

namespace FlaxEngine
{
    partial class Material
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
