////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using FlaxEngine.GUI;

namespace FlaxEditor.Surface
{
    /// <summary>
    /// Visject Surface node control.
    /// </summary>
    /// <seealso cref="FlaxEngine.GUI.ContainerControl" />
    public class SurfaceNode : ContainerControl
    {
        /// <summary>
        /// The surface.
        /// </summary>
        public readonly VisjectSurface Surface;
        
        public SurfaceNode(VisjectSurface surface)
            : base(true, 0, 0, 100, 100)
        {
            Surface = surface;
        }
    }
}
