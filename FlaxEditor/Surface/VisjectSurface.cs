////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using FlaxEditor.GUI.Drag;
using FlaxEditor.Surface.Elements;
using FlaxEngine;
using FlaxEngine.GUI;

namespace FlaxEditor.Surface
{
    /// <summary>
    /// Visject Surface control for editing Nodes Graph.
    /// </summary>
    /// <seealso cref="FlaxEngine.GUI.ScrollableControl" />
    public partial class VisjectSurface : ScrollableControl
    {
        private bool _edited;
        private float _targeScale = 1.0f;
        private readonly List<SurfaceNode> _nodes = new List<SurfaceNode>(64);

        private bool _leftMouseDown;
        private bool _rightMouseDown;
        private Vector2 _leftMouseDownPos;
        private Vector2 _rightMouseDownPos;
        private Vector2 _mousePos;
        private float _mouseMoveAmount;

        private bool _isMovingSelection;
        private Box _startBox;
        private Box _lastBoxUnderMouse;

        //private VisjectCM _cmPrimaryMenu;
        private ContextMenu _cmSecondaryMenu;
        private Vector2 _cmStartPos;

        private DragItems _dragOverItems = new DragItems();

        /// <summary>
        /// The owner.
        /// </summary>
        public readonly IVisjectSurfaceOwner Owner;

        /// <summary>
        /// The surface type.
        /// </summary>
        public readonly SurfaceType Type;

        /// <summary>
        /// The style used by the surface.
        /// </summary>
        public readonly SurfaceStyle Style;

        /// <summary>
        /// Initializes a new instance of the <see cref="VisjectSurface"/> class.
        /// </summary>
        /// <param name="owner">The owner.</param>
        /// <param name="type">The type.</param>
        public VisjectSurface(IVisjectSurfaceOwner owner, SurfaceType type)
            : base(true, 0, 0, 100, 100)
        {
            Owner = owner;
            Type = type;
            Style = SurfaceStyle.CreateStyleHandler(Editor.Instance, Type);
            if (Style == null)
                throw new InvalidOperationException("Missing visject surface style.");
        }
    }
}
