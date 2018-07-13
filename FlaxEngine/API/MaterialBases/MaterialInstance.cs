// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

namespace FlaxEngine
{
    public sealed partial class MaterialInstance
    {
        /// <inheritdoc />
        public override MaterialInstance CreateVirtualInstance()
        {
            WaitForLoaded();

            var instance = Content.CreateVirtualAsset<MaterialInstance>();
            var baseMaterial = BaseMaterial;
            if (baseMaterial)
                baseMaterial.WaitForLoaded();
            instance.BaseMaterial = baseMaterial;

            // Copy parameters
            var src = Parameters;
            var dst = instance.Parameters;
            if (src != null && dst != null && src.Length == dst.Length)
            {
                for (int i = 0; i < src.Length; i++)
                {
                    if (src[i].IsPublic)
                        dst[i].Value = src[i].Value;
                }
            }

            return instance;
        }
    }
}
