// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System;
using System.Collections.Generic;
using FlaxEditor.Surface.Elements;
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
        /// <value>
        /// The type.
        /// </value>
        public uint Type => ((uint)GroupArchetype.GroupID << 16) | Archetype.TypeID;

        /// <summary>
        /// The metadata.
        /// </summary>
        public readonly SurfaceMeta Meta = new SurfaceMeta();

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

            if (Archetype.DefaultValues != null)
            {
                Values = new object[Archetype.DefaultValues.Length];
                Array.Copy(Archetype.DefaultValues, Values, Values.Length);
            }
        }

        /// <summary>
        /// Resizes the node area.
        /// </summary>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        protected void Resize(float width, float height)
        {
            var prevSize = Size;
            Size = new Vector2(width + Constants.NodeMarginX * 2, height + Constants.NodeMarginY * 2 + Constants.NodeHeaderSize + Constants.NodeFooterSize);

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

        internal void AddElement(ISurfaceNodeElement element)
        {
            Elements.Add(element);
            if (element is Control control)
                AddChild(control);
        }

        internal void RemoveElement(ISurfaceNodeElement element, bool dispose = true)
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

        /// <summary>
        /// Sets the value of the node parameter.
        /// </summary>
        /// <param name="index">The value index.</param>
        /// <param name="value">The value.</param>
        public virtual void SetValue(int index, object value)
        {
            Values[index] = value;
            Surface.MarkAsEdited();
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
            BackgroundColor = _isSelected ? Color.OrangeRed : style.BackgroundNormal;

            base.Draw();

            /*
            // Node layout lines rendering
            float marginX = SURFACE_NODE_MARGIN_X * _scale;
            float marginY = SURFACE_NODE_MARGIN_Y * _scale;
            float top = (SURFACE_NODE_HEADER_SIZE + SURFACE_NODE_MARGIN_Y) * _scale;
            float footer = SURFACE_NODE_FOOTER_SIZE * _scale;
            render.DrawRectangle(Rect(marginX, top, _width - 2 * marginX, _height - top - marginY - footer), Color::Red);
            */

            // Header
            Render2D.FillRectangle(_headerRect, style.BackgroundHighlighted);
            Render2D.DrawText(style.FontLarge, Title, _headerRect, style.Foreground, TextAlignment.Center, TextAlignment.Center);

            // Close button
            if ((Archetype.Flags & NodeFlags.NoCloseButton) == 0)
            {
                float alpha = _closeButtonRect.Contains(_mousePosition) ? 1.0f : 0.7f;
                Render2D.DrawSprite(style.Cross, _closeButtonRect, new Color(alpha));
            }

            // Footer
            Render2D.FillRectangle(_footerRect, GroupArchetype.Color);
        }

        /// <inheritdoc />
        public override bool OnMouseUp(Vector2 location, MouseButton buttons)
        {
            if (base.OnMouseUp(location, buttons))
                return true;

            // Close
            if ((Archetype.Flags & NodeFlags.NoCloseButton) == 0)
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
