// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

namespace FlaxEngine
{
    /// <summary>
    /// Interface for objects that can be transformed.
    /// </summary>
    public interface ITransformable
    {
        /// <summary>
        /// Gets or sets the transform.
        /// </summary>
        /// <value>
        /// The transform.
        /// </value>
        Transform Transform { get; set; }
    }
}
