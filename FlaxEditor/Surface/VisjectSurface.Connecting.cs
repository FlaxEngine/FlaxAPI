// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using FlaxEngine;

namespace FlaxEditor.Surface
{
    /// <summary>
    /// The Visject Surface connection creation handler object.
    /// </summary>
    public interface IConnectionInstigator
    {
        /// <summary>
        /// Gets the connection origin point (in surface node space).
        /// </summary>
        Vector2 ConnectionOrigin { get; }

        /// <summary>
        /// Determines whether this surface object is connected with the specified other object.
        /// </summary>
        /// <param name="other">The other object to check.</param>
        /// <returns><c>true</c> if connection between given two objects exists; otherwise, <c>false</c>.</returns>
        bool AreConnected(IConnectionInstigator other);

        /// <summary>
        /// Determines whether this surface object can be connected with the specified other object.
        /// </summary>
        /// <param name="other">The other object to check.</param>
        /// <returns><c>true</c> if connection can be created; otherwise, <c>false</c>.</returns>
        bool CanConnectWith(IConnectionInstigator other);

        /// <summary>
        /// Draws the connecting line.
        /// </summary>
        /// <param name="startPos">The start position.</param>
        /// <param name="endPos">The end position.</param>
        /// <param name="color">The color.</param>
        void DrawConnectingLine(ref Vector2 startPos, ref Vector2 endPos, ref Color color);

        /// <summary>
        /// Created the new connection with the specified other object.
        /// </summary>
        /// <param name="other">The other.</param>
        void Connect(IConnectionInstigator other);
    }

    public partial class VisjectSurface
    {
        /// <summary>
        /// Checks if can use direct conversion from one type to another.
        /// </summary>
        /// <param name="from">Source type.</param>
        /// <param name="to">Target type.</param>
        /// <returns>True if can use direct conversion, otherwise false.</returns>
        public bool CanUseDirectCast(ConnectionType from, ConnectionType to)
        {
            bool result = (from & to) != 0;
            if (!result)
            {
                // Implicit casting is supported for primitive types
                switch (from)
                {
                case ConnectionType.Bool:
                case ConnectionType.Integer:
                case ConnectionType.Float:
                case ConnectionType.Vector2:
                case ConnectionType.Vector3:
                case ConnectionType.Vector4:
                case ConnectionType.Rotation:
                    switch (to)
                    {
                    case ConnectionType.Bool:
                    case ConnectionType.Integer:
                    case ConnectionType.Float:
                    case ConnectionType.Vector2:
                    case ConnectionType.Vector3:
                    case ConnectionType.Vector4:
                    case ConnectionType.Rotation:
                        result = true;
                        break;
                    }
                    break;
                }
            }

            return result;
        }

        /// <summary>
        /// Begins connecting surface objects action.
        /// </summary>
        /// <param name="instigator">The connection instigator (eg. start box).</param>
        public void ConnectingStart(IConnectionInstigator instigator)
        {
            if (instigator != null && instigator != _connectionInstigator)
            {
                _connectionInstigator = instigator;
                StartMouseCapture();
            }
        }

        /// <summary>
        /// Callback for surface objects connections instigators to indicate mouse over control event (used to draw preview connections).
        /// </summary>
        /// <param name="instigator">The instigator.</param>
        public void ConnectingOver(IConnectionInstigator instigator)
        {
            _lastInstigatorUnderMouse = instigator;
        }

        /// <summary>
        /// Ends connecting surface objects action.
        /// </summary>
        /// <param name="end">The end object (eg. end box).</param>
        public void ConnectingEnd(IConnectionInstigator end)
        {
            // Ensure that there was a proper start box
            if (_connectionInstigator == null)
                return;

            var start = _connectionInstigator;
            _connectionInstigator = null;

            // Check if boxes are different and end box is specified
            if (start == end || end == null)
                return;

            // Connect them
            if (start.CanConnectWith(end))
            {
                start.Connect(end);
            }
        }
    }
}
