////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using FlaxEngine;
using FlaxEngine.GUI;

namespace FlaxEditor.Surface
{
    /// <summary>
    /// Base class for <see cref="SurfaceNode"/> element controls. Implements <see cref="ISurfaceNodeElement"/>.
    /// </summary>
    /// <seealso cref="FlaxEngine.GUI.Control" />
    /// <seealso cref="FlaxEditor.Surface.ISurfaceNodeElement" />
    public abstract class SurfaceNodeElementControl : Control, ISurfaceNodeElement
    {
        /// <inheritdoc />
        public SurfaceNode ParentNode { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SurfaceNodeElementControl"/> class.
        /// </summary>
        /// <param name="parentNode">The parent node.</param>
        /// <param name="location">The location.</param>
        /// <param name="size">The size.</param>
        /// <param name="canFocus">if set to <c>true</c> can focus this control.</param>
        protected SurfaceNodeElementControl(SurfaceNode parentNode, ref Vector2 location, ref Vector2 size, bool canFocus)
            : base(canFocus, location, size)
        {
            ParentNode = parentNode;
        }
    }
}
