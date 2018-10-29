// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

namespace FlaxEditor.Surface
{
    /// <summary>
    /// Defines the context data and logic for the Visject Surface editor.
    /// </summary>
    public interface ISurfaceContext
    {
        /// <summary>
        /// Gets or sets the surface data. Used to load or save the surface to the data source.
        /// </summary>
        byte[] SurfaceData { get; set; }
    }
}
