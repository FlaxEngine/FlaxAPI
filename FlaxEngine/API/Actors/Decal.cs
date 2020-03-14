// Copyright (c) 2012-2020 Wojciech Figat. All rights reserved.

namespace FlaxEngine
{
    public sealed partial class Decal
    {
        /// <summary>
        /// Utility to crate a new virtual Material Instance asset, set its parent to the currently applied material, and assign it to the decal. Can be used to modify the decal material parameters from code.
        /// </summary>
        /// <returns>The created virtual material instance.</returns>
        public MaterialInstance CreateAndSetVirtualMaterialInstance()
        {
            var material = Material;
            if (material == null)
                throw new FlaxException("Cannot create virtual material. Decal has missing material.");

            if (material.WaitForLoaded())
                throw new FlaxException("Cannot create virtual material. Decal material failed to load.");

            var result = material.CreateVirtualInstance();
            if (result == null)
                throw new FlaxException("Cannot create virtual material.");
            Material = result;

            return result;
        }
    }
}
