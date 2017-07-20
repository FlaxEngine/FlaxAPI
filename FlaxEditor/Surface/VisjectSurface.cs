////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using FlaxEditor.GUI.Drag;
using FlaxEditor.Surface.ContextMenu;
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
        // TODO: stuff to finish
        // - surface scale animation
        // - set initial scale to 0.5
        // - nodes removing
        // - nodes selecting
        // - nodes spawning
        // - nodes moving
        // - zooming
        // - moving around the surface
        // - connecting nodes
        // - surface parameters tracking and editing
        // - dragging asset items over
        // - undo/redo support
        // - drawing backgroud
        // - drawing connections

        private bool _edited;
        private float _targeScale = 1.0f;
        private readonly List<SurfaceNode> _nodes = new List<SurfaceNode>(64);

        private bool _leftMouseDown;
        private bool _rightMouseDown;
        private Vector2 _leftMouseDownPos = Vector2.Minimum;
        private Vector2 _rightMouseDownPos = Vector2.Minimum;
        private Vector2 _mousePos = Vector2.Minimum;
        private float _mouseMoveAmount;

        private bool _isMovingSelection;
        private Box _startBox;
        private Box _lastBoxUnderMouse;

        private VisjectCM _cmPrimaryMenu;
        private FlaxEngine.GUI.ContextMenu _cmSecondaryMenu;
        private Vector2 _cmStartPos = Vector2.Minimum;

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
        /// Gets a value indicating whether surface is edited.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this surface is edited; otherwise, <c>false</c>.
        /// </value>
        public bool IsEdited => _edited;

        /// <summary>
        /// Initializes a new instance of the <see cref="VisjectSurface"/> class.
        /// </summary>
        /// <param name="owner">The owner.</param>
        /// <param name="type">The type.</param>
        public VisjectSurface(IVisjectSurfaceOwner owner, SurfaceType type)
            : base(true, 0, 0, 100, 100)
        {
            DockStyle = DockStyle.Fill;

            Owner = owner;
            Type = type;
            Style = SurfaceStyle.CreateStyleHandler(Editor.Instance, Type);
            if (Style == null)
                throw new InvalidOperationException("Missing visject surface style.");

            // Create primary menu (for nodes spawning)
            _cmPrimaryMenu = new VisjectCM(type);
            _cmPrimaryMenu.OnItemClicked += OnPrimaryMenuButtonClick;

            // Create secondary menu (for other actions)
            _cmSecondaryMenu = new FlaxEngine.GUI.ContextMenu();
            _cmSecondaryMenu.AddButton(1, "Save");
            _cmSecondaryMenu.AddSeparator();
            _cmSecondaryMenu.AddButton(2, "Delete node");
            _cmSecondaryMenu.AddButton(3, "Remove all connections to that node");
            _cmSecondaryMenu.AddButton(4, "Remove all connections to that box");
            _cmSecondaryMenu.OnButtonClicked += OnSecondaryMenuButtonClick;

            // Set initial scale to provide nice zoom in effect on startup
            //Scale = new Vector2(0.5f);
        }

        private void SetScale(float scale)
        {
            // Disable scalig during selecting nodes
            if (_leftMouseDown)
                return;

            // Clamp
            scale = Mathf.Clamp(scale, 0.1f, 1.6f);

            // Check if value will change
            if (Mathf.Abs(scale - _targeScale) > 0.0001f)
            {
                // Set nw target scale
                _targeScale = scale;
            }
        }

        /// <summary>
        /// Clears the selection.
        /// </summary>
        public void ClearSelection()
        {
            for (int i = 0; i < _nodes.Count; i++)
                _nodes[i].IsSelected = false;
        }

        /// <summary>
        /// Deletes the specified node.
        /// </summary>
        /// <param name="node">The node.</param>
        public void Delete(SurfaceNode node)
        {
            throw new NotImplementedException("TODO: delete nodes");
        }

        /// <summary>
        /// Deletes the selected nodes.
        /// </summary>
        public void DeleteSelection()
        {
            throw new NotImplementedException("TODO: delete selected nodes");
        }

        private void OnPrimaryMenuButtonClick(VisjectCMItem visjectCmItem)
        {
            throw new NotImplementedException("TODO: spawn nodes");
        }

        private void OnSecondaryMenuButtonClick(int id, FlaxEngine.GUI.ContextMenu contextMenu)
        {
            throw new NotImplementedException("TODO: custom actions menu");
        }

        /// <inheritdoc />
        public override void Draw()
        {
            // Cache data
            var style = FlaxEngine.GUI.Style.Current;
            var rect = new Rectangle(Vector2.Zero, Size);

            // Draw background
            var background = Owner.GetSurfaceBackground();
            if (background && background.ResidentMipLevels > 0)
            {
                var bSize = background.Size;
                float bw = bSize.X;
                float bh = bSize.Y;
                Vector2 pos = Vector2.Mod(bSize);

                if (pos.X > 0)
                    pos.X -= bw;
                if (pos.Y > 0)
                    pos.Y -= bh;

                int maxI = Mathf.CeilToInt(rect.Width / bw + 1.0f);
                int maxJ = Mathf.CeilToInt(rect.Height / bh + 1.0f);

                for (int i = 0; i < maxI; i++)
                {
                    for (int j = 0; j < maxJ; j++)
                    {
                        Render2D.DrawTexture(background, new Rectangle(pos.X + i * bw, pos.Y + j * bh, bw, bh), Color.White, false);
                    }
                }
            }

            // Base
            base.Draw();
            
            Render2D.DrawText(style.FontTitle, string.Format("Scale: {0}", Scale), rect, Enabled ? Color.Red : Color.Black);
            
            // Draw border
            if (ContainsFocus)
                Render2D.DrawRectangle(new Rectangle(0, 0, rect.Width - 2, rect.Height - 2), style.BackgroundSelected);

            // Draw disabled overlay
            if (!Enabled)
                Render2D.FillRectangle(rect, new Color(0.2f, 0.2f, 0.2f, 0.5f), true);
        }

        /// <inheritdoc />
        public override void OnDestroy()
        {
            // Cleanup
            _cmPrimaryMenu.Dispose();
            _cmSecondaryMenu.Dispose();

            base.OnDestroy();
        }
    }
}
