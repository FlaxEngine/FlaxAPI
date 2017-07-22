////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Linq;
using FlaxEditor.GUI.Drag;
using FlaxEditor.Surface.ContextMenu;
using FlaxEditor.Surface.Elements;
using FlaxEngine;
using FlaxEngine.Assertions;
using FlaxEngine.GUI;

namespace FlaxEditor.Surface
{
    /// <summary>
    /// Visject Surface control for editing Nodes Graph.
    /// </summary>
    /// <seealso cref="FlaxEngine.GUI.ContainerControl" />
    public partial class VisjectSurface : ContainerControl
    {
        // TODO: stuff to finish
        // - surface parameters tracking and editing
        // - dragging asset items over
        // - undo/redo support
        
        private class SurfaceControl : ContainerControl
        {
            /// <inheritdoc />
            public SurfaceControl()
                : base(false)
            {
                ClipChildren = false;
            }

            /// <inheritdoc />
            public override bool IntersectsContent(ref Vector2 locationParent, out Vector2 location)
            {
                location = PointFromParent(locationParent);
                return true;
            }
        }

        private SurfaceControl _surface;

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

        private DragAssets _dragOverItems = new DragAssets();

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
        /// Gets or sets the view position (upper left corner of the view).
        /// </summary>
        /// <value>
        /// The view position.
        /// </value>
        public Vector2 ViewPosition
        {
            get => _surface.Location;
            set => _surface.Location = value;
        }

        /// <summary>
        /// Gets a value indicating whether user is selecting nodes.
        /// </summary>
        /// <value>
        ///   <c>true</c> if user is selecting nodes; otherwise, <c>false</c>.
        /// </value>
        public bool IsSelecting => _leftMouseDown && !_isMovingSelection && _startBox == null;

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

            // Surface control used to navigate around the view (scale and move it)
            _surface = new SurfaceControl();
            _surface.Parent = this;

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
            _surface.Scale = new Vector2(0.5f);
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
                // Set new target scale
                _targeScale = scale;
            }
        }

        private void AddScale(float delta)
        {
            SetScale(_targeScale + delta);
        }

        /// <summary>
        /// Mark surface as edited.
        /// </summary>
        /// <param name="graphEdited">True if graph has been edited (nodes structure or parameter value).</param>
        public void MarkAsEdited(bool graphEdited = true)
        {
            if (!_edited)
            {
                _edited = true;
                Owner.OnSurfaceEditedChanged();
            }

            if (graphEdited)
            {
                Owner.OnSurfaceGraphEdited();
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
        /// Adds the specified node to the selection.
        /// </summary>
        /// <param name="node">The node.</param>
        public void AddToSelection(SurfaceNode node)
        {
            node.IsSelected = true;
        }

        /// <summary>
        /// Selects the specified node.
        /// </summary>
        /// <param name="node">The node.</param>
        public void Select(SurfaceNode node)
        {
            ClearSelection();

            node.IsSelected = true;
        }

        /// <summary>
        /// Deselects the specified node.
        /// </summary>
        /// <param name="node">The node.</param>
        public void Deselect(SurfaceNode node)
        {
            node.IsSelected = false;
        }

        /// <summary>
        /// Deletes the specified node.
        /// </summary>
        /// <param name="node">The node.</param>
        public void Delete(SurfaceNode node)
        {
            if ((node.Archetype.Flags & NodeFlags.NoRemove) == 0)
            {
                node.RemoveConnections();
                node.Dispose();

                _nodes.Remove(node);

                MarkAsEdited();
            }
        }

        /// <summary>
        /// Deletes the selected nodes.
        /// </summary>
        public void DeleteSelection()
        {
            bool edited = false;

            for (int i = 0; i < _nodes.Count; i++)
            {
                var node = _nodes[i];

                if (node.IsSelected && (node.Archetype.Flags & NodeFlags.NoRemove) == 0)
                {
                    node.RemoveConnections();
                    node.Dispose();

                    _nodes.RemoveAt(i);
                    i--;

                    edited = true;
                }
            }

            if (edited)
                MarkAsEdited();
        }

        /// <summary>
        /// Spawns the node.
        /// </summary>
        /// <param name="groupID">The group archetype ID.</param>
        /// <param name="typeID">The node archetype ID.</param>
        /// <param name="location">The location.</param>
        /// <param name="customValues">The custom values array. Must match node archetype <see cref="NodeArchetype.DefaultValues"/> size. Pass null to use default values.</param>
        /// <returns>Created node.</returns>
        public SurfaceNode SpawnNode(ushort groupID, ushort typeID, Vector2 location, object[] customValues = null)
        {
            GroupArchetype groupArchetype;
            NodeArchetype nodeArchetype;
            if (NodeFactory.GetArchetype(groupID, typeID, out groupArchetype, out nodeArchetype))
            {
                return SpawnNode(groupArchetype, nodeArchetype, location, customValues);
            }

            return null;
        }

        /// <summary>
        /// Spawns the node.
        /// </summary>
        /// <param name="groupArchetype">The group archetype.</param>
        /// <param name="nodeArchetype">The node archetype.</param>
        /// <param name="location">The location.</param>
        /// <param name="customValues">The custom values array. Must match node archetype <see cref="NodeArchetype.DefaultValues"/> size. Pass null to use default values.</param>
        /// <returns>Created node.</returns>
        public SurfaceNode SpawnNode(GroupArchetype groupArchetype, NodeArchetype nodeArchetype, Vector2 location, object[] customValues = null)
        {
            if (groupArchetype == null || nodeArchetype == null)
                throw new ArgumentNullException();
            Assert.IsTrue(groupArchetype.Archetypes.Contains(nodeArchetype));

            // Create node
            var node = NodeFactory.CreateNode(this, groupArchetype, nodeArchetype);
            if (node == null)
            {
                Debug.LogError("Failed to create node.");
                return null;
            }

            // Intiialize
            if (customValues != null)
            {
                if (node.Values != null && node.Values.Length == customValues.Length)
                    Array.Copy(customValues, node.Values, customValues.Length);
                else
                    throw new InvalidOperationException("Invalid node custom values.");
            }
            OnNodeLoaded(node);
            node.OnSurfaceLoaded();
            node.Location = location;

            return node;
        }

        /// <summary>
        /// Called when node gets loaded and should be added to the surface. Creates node elements from the archetype.
        /// </summary>
        /// <param name="node">The node.</param>
        private void OnNodeLoaded(SurfaceNode node)
        {
            // Create child elements of the node based on it's archetype
            for (int i = 0; i < node.Archetype.Elements.Length; i++)
            {
                var arch = node.Archetype.Elements[i];
                ISurfaceNodeElement element = null;
                switch (arch.Type)
                {
                    case NodeElementType.Input:
                        element = new InputBox(node, arch);
                        break;
                    case NodeElementType.Output:
                        element = new OutputBox(node, arch);
                        break;
                    case NodeElementType.BoolValue:
                        element = new BoolValue(node, arch);
                        break;
                    case NodeElementType.FloatValue:
                        //element = new FloatValue(node, arch); // TODO: finish it
                        break;
                    case NodeElementType.InteagerValue:
                        //nelement = ew InteagerValue(node, arch); // TODO: finish it
                        break;
                    case NodeElementType.ColorValue:
                        //element = new ColorValue(node, arch); // TODO: finish it
                        break;
                    case NodeElementType.ComboBox:
                        element = new ComboBoxElement(node, arch);
                        break;
                    case NodeElementType.Asset:
                        element = new AssetSelect(node, arch);
                        break;
                    case NodeElementType.Text:
                        element = new TextView(node, arch);
                        break;
                    case NodeElementType.RotationValue:
                        //element = new RotationValue(node, arch); // TODO: finish it
                        break;
                }
                if (element != null)
                {
                    // Link element
                    node.AddElement(element);
                }
            }

            // Load metadata
            // TODO: finsih nodes metadata storage like in c++
            /*auto meta = node.Meta.GetEntry(11);
            if (meta && meta->IsLoaded)
            {
                VisjectSurfaceMeta11* meta11 = (VisjectSurfaceMeta11*)meta->Data.GetData();
                node.Data->SetPosition(meta11->Position);
                node.Data->_isSelected = meta11->Selected;
            }
            */

            // Link node
            _nodes.Add(node);
            node.OnLoaded();
            node.Parent = _surface;

            // TODO: add archetypes validation
            /*
#if DEBUG

            // Validate achetype boxes ids (search for duplicates)
            for (int i = 0; i < node.Boxes.Count - 1; i++)
            {
                byte id = node.Boxes[i]->ID;
                for (int j = i + 1; j < node.Boxes.Count(); j++)
                {
                    if (id == node.Boxes[j]->ID)
                    {
                        // Data Leak
                        throw new InvalidDataException("Invalid node archetype.");
                    }
                }
            }

#endif*/
        }

        /// <inheritdoc />
        public override void Update(float deltaTime)
        {
            // Update scale
            var currentScale = _surface.Scale.X;
            if (Mathf.Abs(_targeScale - currentScale) > 0.001f)
            {
                var scale = new Vector2(Mathf.Lerp(currentScale, _targeScale, deltaTime * 10.0f));
                _surface.Scale = scale;
            }

            base.Update(deltaTime);
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
                        Render2D.DrawTexture(background, new Rectangle(pos.X + i * bw, pos.Y + j * bh, bw, bh), Color.White);
                    }
                }
            }

            // Selection
            if (IsSelecting)
            {
                var selectionRect = Rectangle.FromPoints(_leftMouseDownPos, _mousePos);
                Render2D.FillRectangle(selectionRect, Color.Orange * 0.13f, true);
                Render2D.DrawRectangle(selectionRect, Color.Orange);
            }

            // Push surface view transform (scale and offset)
            Render2D.PushTransform(ref _surface._cachedTransform);

            // Draw all connections at once to boost batching process
            for (int i = 0; i < _nodes.Count; i++)
            {
                var node = _nodes[i];
                for (int j = 0; j < node.Elements.Count; j++)
                {
                    if (node.Elements[j] is OutputBox ob && ob.HasAnyConnection)
                    {
                        ob.DrawConnections();
                    }
                }
            }

            // Draw connecting line
            if (_startBox != null)
            {
                // Get start position
                Vector2 startPos = _startBox.ConnectionOrigin;

                // Check if mouse is over any of box
                Vector2 endPos = _surface.PointFromParent(_mousePos);
                Color lineColor = Style.Colors.Connecting;
                if (_lastBoxUnderMouse != null)
                {
                    // Check if can connect boxes
                    bool canConnect = CanConnectBoxes(_startBox, _lastBoxUnderMouse);
                    lineColor = canConnect ? Style.Colors.ConnectingValid : Style.Colors.ConnectingInvalid;
                    endPos = _lastBoxUnderMouse.ConnectionOrigin;
                }

                // Draw connection
                OutputBox.DrawConnection(ref startPos, ref endPos, ref lineColor);
            }

            Render2D.PopTransform();

            // Base
            base.Draw();

            //Render2D.DrawText(style.FontTitle, string.Format("Scale: {0}", _surface.Scale), rect, Enabled ? Color.Red : Color.Black);

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
