// Copyright (c) 2012-2019 Wojciech Figat. All rights reserved.

using System;
using System.Collections.Generic;
using FlaxEditor.Surface.Elements;
using FlaxEditor.Surface.Undo;
using FlaxEngine;
using FlaxEngine.GUI;

namespace FlaxEditor.Surface
{
    /// <summary>
    /// Visject Surface node control.
    /// </summary>
    /// <seealso cref="SurfaceControl" />
    public class SurfaceNode : SurfaceControl
    {
        /// <summary>
        /// Flag used to discard node values setting during event sending for node UI flushing.
        /// </summary>
        protected bool _isDuringValuesEditing;

        /// <summary>
        /// The header rectangle (local space).
        /// </summary>
        protected Rectangle _headerRect;

        /// <summary>
        /// The close button rectangle (local space).
        /// </summary>
        protected Rectangle _closeButtonRect;

        /// <summary>
        /// The footer rectangle (local space).
        /// </summary>
        protected Rectangle _footerRect;

        /// <summary>
        /// The node archetype.
        /// </summary>
        public readonly NodeArchetype Archetype;

        /// <summary>
        /// The group archetype.
        /// </summary>
        public readonly GroupArchetype GroupArchetype;

        /// <summary>
        /// The elements collection.
        /// </summary>
        public readonly List<ISurfaceNodeElement> Elements = new List<ISurfaceNodeElement>();

        /// <summary>
        /// The values (node parameters in layout based on <see cref="NodeArchetype.DefaultValues"/>).
        /// </summary>
        public readonly object[] Values;

        /// <summary>
        /// Gets or sets the node title text.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// The identifier of the node.
        /// </summary>
        public readonly uint ID;

        /// <summary>
        /// Gets the type (packed GroupID (higher 16 bits) and TypeID (lower 16 bits)).
        /// </summary>
        public uint Type => ((uint)GroupArchetype.GroupID << 16) | Archetype.TypeID;

        /// <summary>
        /// The metadata.
        /// </summary>
        public readonly SurfaceMeta Meta = new SurfaceMeta();

        /// <summary>
        /// Occurs when node values collection gets changed.
        /// </summary>
        public event Action ValuesChanged;

        /// <summary>
        /// Initializes a new instance of the <see cref="SurfaceNode"/> class.
        /// </summary>
        /// <param name="id">The node id.</param>
        /// <param name="context">The surface context.</param>
        /// <param name="nodeArch">The node archetype.</param>
        /// <param name="groupArch">The group archetype.</param>
        public SurfaceNode(uint id, VisjectSurfaceContext context, NodeArchetype nodeArch, GroupArchetype groupArch)
        : base(context, nodeArch.Size.X + Constants.NodeMarginX * 2, nodeArch.Size.Y + Constants.NodeMarginY * 2 + Constants.NodeHeaderSize + Constants.NodeFooterSize)
        {
            Title = nodeArch.Title;
            ID = id;
            Archetype = nodeArch;
            GroupArchetype = groupArch;
            AutoFocus = false;
            TooltipText = nodeArch.Description;

            if (Archetype.DefaultValues != null)
            {
                Values = new object[Archetype.DefaultValues.Length];
                Array.Copy(Archetype.DefaultValues, Values, Values.Length);
            }
        }

        /// <summary>
        /// Calculates the size of the node including header, footer, and margins.
        /// </summary>
        /// <param name="width">The node area width.</param>
        /// <param name="height">The node area height.</param>
        /// <returns>The node control total size.</returns>
        protected virtual Vector2 CalculateNodeSize(float width, float height)
        {
            return new Vector2(width + Constants.NodeMarginX * 2, height + Constants.NodeMarginY * 2 + Constants.NodeHeaderSize + Constants.NodeFooterSize);
        }

        /// <summary>
        /// Resizes the node area.
        /// </summary>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        protected void Resize(float width, float height)
        {
            var prevSize = Size;
            Size = CalculateNodeSize(width, height);

            // Update boxes on width change
            if (!Mathf.NearEqual(prevSize.X, Size.X))
            {
                for (int i = 0; i < Elements.Count; i++)
                {
                    if (Elements[i] is OutputBox box)
                    {
                        box.Location = box.Archetype.Position + new Vector2(width, 0);
                    }
                }
            }
        }

        /// <summary>
        /// Creates an element from the archetype and adds the element to the node.
        /// </summary>
        /// <param name="arch">The element archetype.</param>
        /// <returns>The created element. Null if the archetype is invalid.</returns>
        public ISurfaceNodeElement AddElement(NodeElementArchetype arch)
        {
            ISurfaceNodeElement element = null;
            switch (arch.Type)
            {
            case NodeElementType.Input:
                element = new InputBox(this, arch);
                break;
            case NodeElementType.Output:
                element = new OutputBox(this, arch);
                break;
            case NodeElementType.BoolValue:
                element = new BoolValue(this, arch);
                break;
            case NodeElementType.FloatValue:
                element = new FloatValue(this, arch);
                break;
            case NodeElementType.IntegerValue:
                element = new IntegerValue(this, arch);
                break;
            case NodeElementType.ColorValue:
                element = new ColorValue(this, arch);
                break;
            case NodeElementType.ComboBox:
                element = new ComboBoxElement(this, arch);
                break;
            case NodeElementType.Asset:
                element = new AssetSelect(this, arch);
                break;
            case NodeElementType.Text:
                element = new TextView(this, arch);
                break;
            case NodeElementType.TextBox:
                element = new TextBoxView(this, arch);
                break;
            case NodeElementType.SkeletonNodeSelect:
                element = new SkeletonNodeSelectElement(this, arch);
                break;
            case NodeElementType.BoxValue:
                element = new BoxValue(this, arch);
                break;
            case NodeElementType.EnumValue:
                element = new EnumValue(this, arch);
                break;
            }
            if (element != null)
            {
                AddElement(element);
            }

            return element;
        }

        /// <summary>
        /// Adds the element to the node.
        /// </summary>
        /// <param name="element">The element.</param>
        public void AddElement(ISurfaceNodeElement element)
        {
            Elements.Add(element);
            if (element is Control control)
                AddChild(control);
        }

        /// <summary>
        /// Removes the element from the node.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="dispose">if set to <c>true</c> dispose control after removing, otherwise false.</param>
        public void RemoveElement(ISurfaceNodeElement element, bool dispose = true)
        {
            if (element is Box box)
                box.RemoveConnections();
            Elements.Remove(element);
            if (element is Control control)
            {
                RemoveChild(control);
                if (dispose)
                    control.Dispose();
            }
        }

        /// <summary>
        /// Removes all connections from and to that node.
        /// </summary>
        public virtual void RemoveConnections()
        {
            for (int i = 0; i < Elements.Count; i++)
            {
                if (Elements[i] is Box box)
                    box.RemoveConnections();
            }

            UpdateBoxesTypes();
        }

        /// <summary>
        /// Gets a value indicating whether this node uses dependent boxes.
        /// </summary>
        public bool HasDependentBoxes => Archetype.DependentBoxes != null;

        /// <summary>
        /// Gets a value indicating whether this node uses independent boxes.
        /// </summary>
        public bool HasIndependentBoxes => Archetype.IndependentBoxes != null;

        /// <summary>
        /// Gets a value indicating whether this node has dependent boxes with assigned valid types. Otherwise any box has no dependent type assigned.
        /// </summary>
        public bool HasDependentBoxesSetup
        {
            get
            {
                if (Archetype.DependentBoxes == null || Archetype.IndependentBoxes == null)
                    return true;

                for (int i = 0; i < Archetype.DependentBoxes.Length; i++)
                {
                    var b = GetBox(Archetype.DependentBoxes[i]);
                    if (b != null && b.CurrentType == b.DefaultType)
                        return false;
                }

                return true;
            }
        }

        private static readonly List<SurfaceNode> UpdateStack = new List<SurfaceNode>();

        /// <summary>
        /// Updates dependant/independent boxes types.
        /// </summary>
        public void UpdateBoxesTypes()
        {
            // Check there is no need to use box types dependency feature
            if (Archetype.DependentBoxes == null && Archetype.IndependentBoxes == null)
            {
                // Back
                return;
            }

            // Prevent recursive loop call that might happen
            if (UpdateStack.Contains(this))
            {
                return;
            }
            UpdateStack.Add(this);

            var independentBoxesLength = Archetype.IndependentBoxes?.Length;
            var dependentBoxesLength = Archetype.DependentBoxes?.Length;

            // Get type to assign to all dependent boxes
            ConnectionType type = Archetype.DefaultType;
            for (int i = 0; i < independentBoxesLength; i++)
            {
                var b = GetBox(Archetype.IndependentBoxes[i]);
                if (b != null && b.HasAnyConnection)
                {
                    // Check if that type if part of default type
                    if ((Archetype.DefaultType & b.Connections[0].DefaultType) != 0)
                    {
                        type = b.Connections[0].CurrentType;
                        break;
                    }
                }
            }

            // Assign connection type
            for (int i = 0; i < dependentBoxesLength; i++)
            {
                var b = GetBox(Archetype.DependentBoxes[i]);
                if (b != null)
                {
                    // Set new type
                    b.CurrentType = type;
                }
            }

            // Validate minor independent boxes to fit main one
            for (int i = 0; i < independentBoxesLength; i++)
            {
                var b = GetBox(Archetype.IndependentBoxes[i]);
                if (b != null)
                {
                    // Set new type
                    b.CurrentType = type;
                }
            }

            UpdateStack.Remove(this);
        }

        /// <summary>
        /// Tries to get box with given ID.
        /// </summary>
        /// <param name="id">The box id.</param>
        /// <returns>Box or null if cannot find.</returns>
        public Box GetBox(int id)
        {
            // TODO: maybe create local cache for boxes? but not a dictionary, use lookup table because ids are usually small (less than 20)
            for (int i = 0; i < Elements.Count; i++)
            {
                if (Elements[i] is Box box && box.ID == id)
                    return box;
            }
            return null;
        }

        /// <summary>
        /// Tries to get box with given ID.
        /// </summary>
        /// <param name="id">The box id.</param>
        /// <param name="result">Box or null if cannot find.</param>
        /// <returns>True fi box has been found, otherwise false.</returns>
        public bool TryGetBox(int id, out Box result)
        {
            // TODO: maybe create local cache for boxes? but not a dictionary, use lookup table because ids are usually small (less than 20)
            for (int i = 0; i < Elements.Count; i++)
            {
                if (Elements[i] is Box box && box.ID == id)
                {
                    result = box;
                    return true;
                }
            }

            result = null;
            return false;
        }

        internal List<Box> GetBoxes()
        {
            var result = new List<Box>();
            for (int i = 0; i < Elements.Count; i++)
            {
                if (Elements[i] is Box box)
                    result.Add(box);
            }
            return result;
        }

        internal void GetBoxes(List<Box> result)
        {
            result.Clear();
            for (int i = 0; i < Elements.Count; i++)
            {
                if (Elements[i] is Box box)
                    result.Add(box);
            }
        }

        /// <summary>
        /// Implementation of Depth-First traversal over the graph of surface nodes.
        /// </summary>
        /// <returns>The list of nodes as a result of depth-first traversal algorithm execution.</returns>
        public IEnumerable<SurfaceNode> DepthFirstTraversal()
        {
            // Reference: https://github.com/stefnotch/flax-custom-visject-plugin/blob/a26a98b40f909a0b10c2259b858e058290003dce/Source/Editor/ExpressionGraphSurface.cs#L231

            // The states of a node are 
            // null  Nothing   (unvisited and not on the stack)
            // false Processing(  visited and     on the stack)
            // true  Completed (  visited and not on the stack)
            Dictionary<SurfaceNode, bool> nodeState = new Dictionary<SurfaceNode, bool>();
            Stack<SurfaceNode> toProcess = new Stack<SurfaceNode>();
            List<SurfaceNode> output = new List<SurfaceNode>();

            // Start processing the nodes (backwards)
            toProcess.Push(this);
            while (toProcess.Count > 0)
            {
                var node = toProcess.Peek();

                // We have never seen this node before
                if (!nodeState.ContainsKey(node))
                {
                    // We are now processing it
                    nodeState.Add(node, false);
                }
                else
                {
                    // Otherwise, we are done processing it
                    nodeState[node] = true;

                    // Remove it from the stack
                    toProcess.Pop();

                    // And add it to the output
                    output.Add(node);
                }

                // For all parents, push them onto the stack if they haven't been visited yet
                var elements = node.Elements;
                for (int i = 0; i < elements.Count; i++)
                {
                    if (node.Elements[i] is InputBox box && box.HasAnyConnection)
                    {
                        if (box.HasSingleConnection)
                        {
                            // Get the parent node
                            var parentNode = box.Connections[0].ParentNode;

                            // It has been visited previously
                            if (nodeState.TryGetValue(parentNode, out bool state))
                            {
                                if (state == false)
                                {
                                    // It's still processing, so there must be a cycle!
                                    throw new Exception("Cycle detected!");
                                }
                            }
                            else
                            {
                                // It hasn't been visited, add it to the stack
                                toProcess.Push(parentNode);
                            }
                        }
                        else
                        {
                            throw new Exception("Input box has more than one connection");
                        }
                    }
                }
            }

            return output;
        }

        /// <summary>
        /// Draws all the connections between surface objects related to this node.
        /// </summary>
        /// <param name="mousePosition">The current mouse position (in surface-space).</param>
        public virtual void DrawConnections(ref Vector2 mousePosition)
        {
            for (int j = 0; j < Elements.Count; j++)
            {
                if (Elements[j] is OutputBox ob && ob.HasAnyConnection)
                {
                    ob.DrawConnections();
                }
            }
        }

        /// <inheritdoc />
        protected override bool ShowTooltip => base.ShowTooltip && _headerRect.Contains(ref _mousePosition) && !Surface.IsLeftMouseButtonDown && !Surface.IsRightMouseButtonDown && !Surface.IsPrimaryMenuOpened;

        /// <inheritdoc />
        public override bool OnShowTooltip(out string text, out Vector2 location, out Rectangle area)
        {
            var result = base.OnShowTooltip(out text, out location, out area);

            // Change the position
            location = new Vector2(_headerRect.Width * 0.5f, _headerRect.Bottom);

            return result;
        }

        /// <inheritdoc />
        public override bool OnTestTooltipOverControl(ref Vector2 location)
        {
            return _headerRect.Contains(ref location) && ShowTooltip;
        }

        /// <inheritdoc />
        public override bool CanSelect(ref Vector2 location)
        {
            return _headerRect.MakeOffseted(Location).Contains(ref location);
        }

        /// <inheritdoc />
        public override void OnSurfaceLoaded()
        {
            base.OnSurfaceLoaded();

            UpdateBoxesTypes();

            for (int i = 0; i < Elements.Count; i++)
            {
                if (Elements[i] is Box box)
                    box.OnConnectionsChanged();
            }
        }

        /// <inheritdoc />
        public override void OnDeleted()
        {
            RemoveConnections();

            base.OnDeleted();
        }

        /// <summary>
        /// Sets the value of the node parameter.
        /// </summary>
        /// <param name="index">The value index.</param>
        /// <param name="value">The value.</param>
        /// <param name="graphEdited">True if graph has been edited (nodes structure or parameter value).</param>
        public virtual void SetValue(int index, object value, bool graphEdited = true)
        {
            if (_isDuringValuesEditing)
                return;

            _isDuringValuesEditing = true;

            var before = Surface.Undo != null ? (object[])Values.Clone() : null;

            Values[index] = value;
            OnValuesChanged();
            Surface.MarkAsEdited(graphEdited);

            Surface.Undo?.AddAction(new EditNodeValuesAction(this, before, graphEdited));

            _isDuringValuesEditing = false;
        }

        /// <summary>
        /// Sets the values of the node parameters.
        /// </summary>
        /// <param name="values">The values.</param>
        /// <param name="graphEdited">True if graph has been edited (nodes structure or parameter value).</param>
        public virtual void SetValues(object[] values, bool graphEdited = true)
        {
            if (_isDuringValuesEditing)
                return;

            if (values == null || Values == null || values.Length != Values.Length)
                throw new ArgumentException();

            _isDuringValuesEditing = true;

            var before = Surface.Undo != null ? (object[])Values.Clone() : null;

            Array.Copy(values, Values, values.Length);
            OnValuesChanged();
            Surface.MarkAsEdited(graphEdited);

            Surface.Undo?.AddAction(new EditNodeValuesAction(this, before, graphEdited));

            _isDuringValuesEditing = false;
        }

        internal void SetIsDuringValuesEditing(bool value)
        {
            _isDuringValuesEditing = value;
        }

        /// <summary>
        /// Called when node values set gets changed.
        /// </summary>
        public virtual void OnValuesChanged()
        {
            ValuesChanged?.Invoke();
        }

        /// <summary>
        /// Updates the given box connection.
        /// </summary>
        /// <param name="box">The box.</param>
        public virtual void ConnectionTick(Box box)
        {
            UpdateBoxesTypes();
        }

        /// <inheritdoc />
        protected override void UpdateRectangles()
        {
            const float footerSize = Constants.NodeFooterSize;
            const float headerSize = Constants.NodeHeaderSize;
            const float closeButtonMargin = Constants.NodeCloseButtonMargin;
            const float closeButtonSize = Constants.NodeCloseButtonSize;
            _headerRect = new Rectangle(0, 0, Width, headerSize);
            _closeButtonRect = new Rectangle(Width - closeButtonSize - closeButtonMargin, closeButtonMargin, closeButtonSize, closeButtonSize);
            _footerRect = new Rectangle(0, Height - footerSize, Width, footerSize);
        }

        /// <inheritdoc />
        public override void Draw()
        {
            var style = Style.Current;

            // Background
            var backgroundRect = new Rectangle(Vector2.Zero, Size);
            Render2D.FillRectangle(backgroundRect, style.BackgroundNormal);

            // Header
            var headerColor = style.BackgroundHighlighted;
            if (_headerRect.Contains(ref _mousePosition))
                headerColor *= 1.07f;
            Render2D.FillRectangle(_headerRect, headerColor);
            Render2D.DrawText(style.FontLarge, Title, _headerRect, style.Foreground, TextAlignment.Center, TextAlignment.Center);

            // Close button
            if ((Archetype.Flags & NodeFlags.NoCloseButton) == 0)
            {
                float alpha = _closeButtonRect.Contains(_mousePosition) ? 1.0f : 0.7f;
                Render2D.DrawSprite(style.Cross, _closeButtonRect, new Color(alpha));
            }

            // Footer
            Render2D.FillRectangle(_footerRect, GroupArchetype.Color);

            DrawChildren();

            // Selection outline
            if (_isSelected)
            {
                var colorTop = Color.Orange;
                var colorBottom = Color.OrangeRed;
                Render2D.DrawRectangle(backgroundRect, colorTop, colorTop, colorBottom, colorBottom);
            }
        }

        /// <inheritdoc />
        public override bool OnMouseUp(Vector2 location, MouseButton buttons)
        {
            if (base.OnMouseUp(location, buttons))
                return true;

            // Close
            if (buttons == MouseButton.Left && (Archetype.Flags & NodeFlags.NoCloseButton) == 0)
            {
                if (_closeButtonRect.Contains(ref location))
                {
                    Surface.Delete(this);
                    return true;
                }
            }
            // Secondary Context Menu
            if (buttons == MouseButton.Right)
            {
                if (!IsSelected)
                    Surface.Select(this);
                var tmp = PointToParent(ref location);
                Surface.ShowSecondaryCM(Parent.PointToParent(ref tmp));
                return true;
            }

            return false;
        }
    }
}
