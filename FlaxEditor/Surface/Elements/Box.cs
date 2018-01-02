////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2018 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System.Collections.Generic;
using FlaxEngine;
using FlaxEngine.Assertions;

namespace FlaxEditor.Surface.Elements
{
    /// <summary>
    /// Surface boxes base class (for input and output boxes). Boxes can be connected.
    /// </summary>
    /// <seealso cref="FlaxEditor.Surface.SurfaceNodeElementControl" />
    public abstract class Box : SurfaceNodeElementControl
    {
        /// <summary>
        /// The current connection type. It's subset or equal to <see cref="DefaultType"/>.
        /// </summary>
        protected ConnectionType _currentType;

        /// <summary>
        /// The cached color for the current box type.
        /// </summary>
        protected Color _currentTypeColor;

        /// <summary>
        /// Unique box ID within single node.
        /// </summary>
        public int ID => Archetype.BoxID;

        /// <summary>
        /// Allowed connections type.
        /// </summary>
        public ConnectionType DefaultType => Archetype.ConnectionsType;

        /// <summary>
        /// List with all connections to oher boxes.
        /// </summary>
        public readonly List<Box> Connections = new List<Box>();

        /// <summary>
        /// Gets a value indicating whether this box has any connection.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this box has any connection; otherwise, <c>false</c>.
        /// </value>
        public bool HasAnyConnection => Connections.Count > 0;

        /// <summary>
        /// Gets a value indicating whether this box has single connection.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this box has single connection; otherwise, <c>false</c>.
        /// </value>
        public bool HasSingleConnection => Connections.Count == 1;

        /// <summary>
        /// Gets a value indicating whether this instance is output box.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is output; otherwise, <c>false</c>.
        /// </value>
        public abstract bool IsOutput { get; }

        /// <summary>
        /// Gets or sets the current type of the box connections.
        /// </summary>
        /// <value>
        /// The current type.
        /// </value>
        public ConnectionType CurrentType
        {
            get => _currentType;
            set
            {
                if (_currentType != value)
                {
                    // Check if need to remove connections
                    if ((value & _currentType) == 0)
                    {
                        RemoveConnections();
                    }

                    // Set new value
                    _currentType = value;

                    // Cache color
                    Surface.Style.GetConnectionColor(_currentType, out _currentTypeColor);

                    // Fire event
                    OnCurrentTypeChanged();
                }
            }
        }

        /// <summary>
        /// Gets the connection origin point (in surface space).
        /// </summary>
        internal Vector2 ConnectionOrigin => Parent.PointToParent(Center);

        /// <inheritdoc />
        protected Box(SurfaceNode parentNode, NodeElementArchetype archetype, Vector2 location)
            : base(parentNode, archetype, location, new Vector2(Constants.BoxSize), false)
        {
            _currentType = DefaultType;
            Surface.Style.GetConnectionColor(_currentType, out _currentTypeColor);
        }

        /// <summary>
        /// Determines whether this box can use the specified type as a conection.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>
        ///   <c>true</c> if this box can use the specified type; otherwise, <c>false</c>.
        /// </returns>
        public bool CanUseType(ConnectionType type)
        {
            // Check drect connection
            if (Surface.CanUseDirectCast(_currentType, type))
            {
                // Can
                return true;
            }

            // Check independent and if there is box with bigger potencial because it may block current one from changing type
            var parentArch = ParentNode.Archetype;
            var boxes = parentArch.IndependentBoxes;
            if (boxes != null)
            {
                for (int i = 0; i < boxes.Length; i++)
                {
                    if (boxes[i] == -1)
                        break;

                    // Get box
                    var b = ParentNode.GetBox(boxes[i]);

                    // Check if its the same and tested type matches the default value type
                    if (b == this && (parentArch.DefaultType & type) != 0)
                    {
                        // Can
                        return true;
                    }
                    // Check if box exists and has any connection
                    if (b != null && b.HasAnyConnection)
                    {
                        // Cannot
                        return false;
                    }
                }
            }

            // Cannot
            return false;
        }

        /// <summary>
        /// Removes all existing connections of that box.
        /// </summary>
        public void RemoveConnections()
        {
            // Check if sth is conected
            if (HasAnyConnection)
            {
                // Remove all connections
                List<Box> toUpdate = new List<Box>(1 + Connections.Count);
                toUpdate.Add(this);
                for (int i = 0; i < Connections.Count; i++)
                {
                    var targetBox = Connections[i];
                    targetBox.Connections.Remove(this);
                    toUpdate.Add(targetBox);
                }
                Connections.Clear();

                // Update
                for (int i = 0; i < toUpdate.Count; i++)
                    toUpdate[i].ConnectionTick();
                toUpdate.Clear();
            }
        }

        /// <summary>
        /// Updates state on connection data changed.
        /// </summary>
        public void ConnectionTick()
        {
            // Update node boxes types management
            ParentNode.ConnectionTick(this);
        }

        /// <summary>
        /// Checks if box is connected with the other one.
        /// </summary>
        /// <param name="box">The other box.</param>
        /// <returns>True if both boxes are connected, otherwise false.</returns>
        public bool AreConnected(Box box)
        {
            bool result = Connections.Contains(box);
            Assert.IsTrue(box == null || result == box.Connections.Contains(this));
            return result;
        }

        /// <summary>
        /// Break connection to the other box (works in a both ways).
        /// </summary>
        /// <param name="box">The other box.</param>
        public void BreakConnection(Box box)
        {
            // Break link
            bool r1 = box.Connections.Remove(this);
            bool r2 = Connections.Remove(box);

            // Ensure data was fine and connection was valid
            Assert.AreEqual(r1, r2);

            // Update
            ConnectionTick();
            box.ConnectionTick();
        }

        /// <summary>
        /// Create connection to the other box (works in a both ways).
        /// </summary>
        /// <param name="box">The other box.</param>
        public void CreateConnection(Box box)
        {
            // Check if any box can have only single conenction
            if (box.IsSingle)
                box.RemoveConnections();
            if (IsSingle)
                RemoveConnections();

            // Add link
            box.Connections.Add(this);
            Connections.Add(box);

            // Ensure data is fine and connection is valid
            Assert.IsTrue(AreConnected(box));

            // Update
            ConnectionTick();
            box.ConnectionTick();
        }

        /// <summary>
        /// True if box can use only single connection.
        /// </summary>
        /// <returns>True if only single conenction.</returns>
        public bool IsSingle => Archetype.Single;

        /// <summary>
        /// True if box type depends on other boxes types of the node.
        /// </summary>
        /// <returns>True if is dependant, otherwise false.</returns>
        public bool IsDependentBox
        {
            get
            {
                var boxes = ParentNode.Archetype.DependentBoxes;
                if (boxes != null)
                {
                    for (int i = 0; i < boxes.Length; i++)
                    {
                        int index = boxes[i];
                        if (index == -1)
                            break;
                        if (index == ID)
                            return true;
                    }
                }

                return false;
            }
        }

        /// <summary>
        /// True if box type doesn't depend on other boxes types of the node.
        /// </summary>
        /// <returns>True if is independant, otherwise false.</returns>
        public bool IsIndependentBox
        {
            get
            {
                var boxes = ParentNode.Archetype.IndependentBoxes;
                if (boxes != null)
                {
                    for (int i = 0; i < boxes.Length; i++)
                    {
                        int index = boxes[i];
                        if (index == -1)
                            break;
                        if (index == ID)
                            return true;
                    }
                }

                return false;
            }
        }

        /// <summary>
        /// Called when current box type changed.
        /// </summary>
        protected virtual void OnCurrentTypeChanged()
        {
        }

        /// <summary>
        /// Draws the box GUI using <see cref="Render2D"/>.
        /// </summary>
        protected void DrawBox()
        {
            var rect = new Rectangle(Vector2.Zero, Size);

            // Size culling
            const float minBoxSize = 5.0f;
            if (rect.Size.LengthSquared < minBoxSize * minBoxSize)
                return;

            // Debuging boxes size
            //Render2D.DrawRectangle(rect, Color.Orange); return;
            
            // Draw icon
            bool hasConnections = HasAnyConnection;
            float alpha = Enabled ? 1.0f : 0.6f;
            Color color = _currentTypeColor * alpha;
            var style = Surface.Style;
            Sprite icon;
            if (_currentType == ConnectionType.Impulse)
                icon = hasConnections ? style.Icons.ArowClose : style.Icons.ArowOpen;
            else
                icon = hasConnections ? style.Icons.BoxClose : style.Icons.BoxOpen;
            Render2D.DrawSprite(icon, rect, color);
        }

        /// <inheritdoc />
        public override bool OnMouseDown(Vector2 location, MouseButton buttons)
        {
            if (buttons == MouseButton.Left)
            {
                Surface.ConnectingStart(this);
            }

            return true;
        }

        /// <inheritdoc />
        public override void OnMouseMove(Vector2 location)
        {
            Surface.OnMosueOverBox(this);
            base.OnMouseMove(location);
        }

        /// <inheritdoc />
        public override bool OnMouseUp(Vector2 location, MouseButton buttons)
        {
            bool result = false;

            if (buttons == MouseButton.Left)
            {
                Surface.ConnectingEnd(this);
                result = true;
            }

            return result;
        }
    }
}
