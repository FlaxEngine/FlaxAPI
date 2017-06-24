////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

namespace FlaxEngine.Rendering
{
    /// <summary>
    /// The base class for all post process effects used by the graphics pipeline.
    /// Allows to extend frame rendering logic and apply custom effects such as outline, nighvision, contrast etc.
    /// </summary>
    public class PostProcessEffect
    {
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="PostProcessEffect"/> is enabled.
        /// Disabled effects are skiped during rendering.
        /// </summary>
        /// <value>
        ///   <c>true</c> if enabled; otherwise, <c>false</c>.
        /// </value>
        public virtual bool Enabled { get; set; } = true;

        /// <summary>
        /// Gets or sets the material.
        /// </summary>
        /// <value>
        /// The material.
        /// </value>
        public virtual MaterialBase Material { get; set; }

        /// <summary>
        /// Creates the new post fx.
        /// </summary>
        /// <param name="material">The material to use.</param>
        /// <returns>The post fx.</returns>
        public static PostProcessEffect Create(MaterialBase material)
        {
            return new PostProcessEffect
            {
                Material = material
            };
        }
    }
}
