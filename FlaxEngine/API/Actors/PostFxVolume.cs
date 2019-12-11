// Copyright (c) 2012-2019 Wojciech Figat. All rights reserved.

namespace FlaxEngine
{
    public sealed partial class PostFxVolume
    {
        /// <summary>
        /// Adds the post fx material to the settings.
        /// </summary>
        /// <param name="material">The material.</param>
        public void AddPostFxMaterial(MaterialBase material)
        {
            var materials = PostFxMaterials;
            materials.AddMaterial(material);
            PostFxMaterials = materials;
        }

        /// <summary>
        /// Removes the post fx material from the settings.
        /// </summary>
        /// <param name="material">The material.</param>
        public void RemovePostFxMaterial(MaterialBase material)
        {
            var materials = PostFxMaterials;
            materials.RemoveMaterial(material);
            PostFxMaterials = materials;
        }
    }
}
