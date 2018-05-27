// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

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

        /// <inheritdoc />
        public NodeElementArchetype Archetype { get; }

        /// <summary>
        /// Gets the surface.
        /// </summary>
        /// <value>
        /// The surface.
        /// </value>
        public VisjectSurface Surface => ParentNode.Surface;

        /// <summary>
        /// Initializes a new instance of the <see cref="SurfaceNodeElementControl"/> class.
        /// </summary>
        /// <param name="parentNode">The parent node.</param>
        /// <param name="archetype">The element archetype.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="canFocus">if set to <c>true</c> can focus this control.</param>
        protected SurfaceNodeElementControl(SurfaceNode parentNode, NodeElementArchetype archetype, float width, float height, bool canFocus)
        : base(archetype.ActualPositionX, archetype.ActualPositionY, width, height)
        {
            ParentNode = parentNode;
            Archetype = archetype;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SurfaceNodeElementControl"/> class.
        /// </summary>
        /// <param name="parentNode">The parent node.</param>
        /// <param name="archetype">The element archetype.</param>
        /// <param name="location">The location.</param>
        /// <param name="size">The size.</param>
        /// <param name="canFocus">if set to <c>true</c> can focus this control.</param>
        protected SurfaceNodeElementControl(SurfaceNode parentNode, NodeElementArchetype archetype, Vector2 location, Vector2 size, bool canFocus)
        : base(location, size)
        {
            CanFocus = canFocus;
            ParentNode = parentNode;
            Archetype = archetype;
        }
    }
}
