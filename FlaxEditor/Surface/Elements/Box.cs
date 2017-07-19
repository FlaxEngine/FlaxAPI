////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
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
        /// The current connection type. It's subset or equal to <see cref="Type"/>.
        /// </summary>
        protected ConnectionType _currentType;

        /// <summary>
        /// The cached color for the current box type.
        /// </summary>
        protected Color _currentTypeColor;

        /// <summary>
        /// Unique box ID within single node.
        /// </summary>
        public readonly int ID;

        /// <summary>
        /// Allowed connections type.
        /// </summary>
        public readonly ConnectionType Type;

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

        public ConnectionType CurrentType
        {
            get => _currentType;
            set
            {
                if (_currentType != value)
                {
                    // Check if need to remove connections
                    if ((value & _currentType) != 0)
                    {
                        RemoveConnections();
                    }

                    // Set new value
                    _currentType = value;

                    // Cache color
                    switch (_currentType)
                    {
                        case ConnectionType.Impulse: _currentTypeColor = VisjectStyle::Colors::Impulse; break;
                        case ConnectionType.Bool: _currentTypeColor = VisjectStyle::Colors::Box; break;
                        case ConnectionType.Integer: _currentTypeColor = VisjectStyle::Colors::Integer; break;
                        case ConnectionType.Float: _currentTypeColor = VisjectStyle::Colors::Float; break;
                        case ConnectionType.Vector2:
                        case ConnectionType.Vector3:
                        case ConnectionType.Vector4:
                        case ConnectionType.Vector: _currentTypeColor = VisjectStyle::Colors::Vector; break;
                        case ConnectionType.String: _currentTypeColor = VisjectStyle::Colors::String; break;
                        case ConnectionType.Object: _currentTypeColor = VisjectStyle::Colors::Object; break;
                        case ConnectionType.Rotation: _currentTypeColor = VisjectStyle::Colors::Rotation; break;
                        case ConnectionType.Transform: _currentTypeColor = VisjectStyle::Colors::Transform; break;
                        case ConnectionType.Box: _currentTypeColor = VisjectStyle::Colors::Box; break;
                        default: _currentTypeColor = VisjectStyle::Colors::Default; break;
                    }

                    // Fire event
                    OnCurrentTypeChanged();
                }
            }
        }

        /// <inheritdoc />
        protected Box(SurfaceNode parentNode, ref Vector2 location, ref Vector2 size, bool canFocus) 
            : base(parentNode, ref location, ref size, canFocus)
        {
            // TODO: set type and ID from archetype
            _currentType = Type;
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
            if (ParentNode.Surface.CanUseDirectCast(_currentType, type))
            {
                // Can
                return true;
            }

            // Check independent and if there is box with bigger potencial because it may block current one from changing type
            var parentArch = ParentNode.Archetype;
            var boxes = parentArch.IndependentBoxes;
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
                if (b != null && b.HasConnection)
                {
                    // Cannot
                    return false;
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
        public bool IsSingle => true; // TODO: gather this from the box archetype!!!

        /// <summary>
        /// True if box type depends on other boxes types of the node.
        /// </summary>
        /// <returns>True if is dependant, otherwise false.</returns>
        public bool IsDependentBox
        {
            get
            {
                var boxes = ParentNode.Archetype.DependentBoxes;
                for (int i = 0; i < boxes; i++)
                {
                    int index = boxes[i];
                    if (index == -1)
                        break;
                    if (index == ID)
                        return true;
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
                for (int i = 0; i < boxes; i++)
                {
                    int index = boxes[i];
                    if (index == -1)
                        break;
                    if (index == ID)
                        return true;
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
            var r = new Rectangle(Vector2.Zero, Size);

            // Size culling
            const float minBoxSize = 5.0f;
            if (r.Size.LengthSquared < minBoxSize * minBoxSize)
                return;

            // Debuging boxes size
            //render->DrawRectangle(r, Color::Orange); return;

            // Ensure to have sprites loaded
            if (_shouldTryLoadIcons)
            {
                _shouldTryLoadIcons = false;
                var ui = Editor.Instance.UI;
                _boxOpen = ui.GetIcon(VisjectStyle::Icons::BoxOpen);
                _boxClose = ui.GetIcon(VisjectStyle::Icons::BoxClose);
                _arrowOpen = ui.GetIcon(VisjectStyle::Icons::ArowOpen);
                _arrowClose = ui.GetIcon(VisjectStyle::Icons::ArowClose);
            }

            // Check if is impulse
            bool hasConnections = HasAnyConnection;
            float alpha = Enabled ? 1.0f : 0.6f;
            if (_currentType == ConnectionType.Impulse)
            {
                Render2D.DrawSprite(hasConnections ? _arrowClose : _arrowOpen, r, VisjectStyle::Colors::Impulse * alpha);
            }
            else
            {
                Render2D.DrawSprite(hasConnections ? _boxClose : _boxOpen, r, getTypeColor() * alpha);
            }
        }
        
        /// <inheritdoc />
        public override bool OnMouseDown(Vector2 location, MouseButtons buttons)
        {
            Focus();

            if (buttons == MouseButtons.Left)
            {
                ParentNode.Surface.ConnectingEnd(this);
            }

            return true;
        }

        /// <inheritdoc />
        public override bool OnMouseUp(Vector2 location, MouseButtons buttons)
        {
            bool result = false;

            if (buttons == MouseButtons.Left)
            {
                ParentNode.Surface.ConnectingStart(this);
                result = true;
            }

            return result;
        }
    }
}
