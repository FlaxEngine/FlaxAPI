////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using FlaxEngine;

namespace FlaxEditor.Surface
{
    /// <summary>
    /// Surface node element archetype description.
    /// </summary>
    public sealed class NodeElementArchetype
    {
        /// <summary>
        /// The element type.
        /// </summary>
        public NodeElementType Type;

        /// <summary>
        /// Default element position in node that has default size.
        /// </summary>
        public Vector2 Position;

        /// <summary>
        /// Custom text value.
        /// </summary>
        public string Text;

        /// <summary>
        /// True if use single connections (for Input element).
        /// </summary>
        public bool Single;

        /// <summary>
        /// Index of the node value that is conencted with that element.
        /// </summary>
        public int ValueIndex;

        /// <summary>
        /// Unique ID of the box in the graph data to link it to this element (Output/Input elements).
        /// </summary>
        public int BoxID;

        /// <summary>
        /// Default connections type for that element (can be set of types).
        /// </summary>
        public ConnectionType ConnectionsType;

        /// <summary>
        /// Gets the actual element position on the x axis.
        /// </summary>
        public float ActualPositionX => Position.X + Constants.NodeMarginX;

        /// <summary>
        /// Gets the actual element position on the y axis.
        /// </summary>
        public float ActualPositionY => Position.Y + Constants.NodeMarginY + Constants.NodeHeaderSize;

        /// <summary>
        /// Node element archetypes factory object. Helps to build surface nodes archetypes.
        /// </summary>
        public static class Factory
        {
            /// <summary>
            /// Creates new Input box element description.
            /// </summary>
            /// <param name="ylevel">The y level.</param>
            /// <param name="text">The text.</param>
            /// <param name="single">If true then box can have only one connection, otherwise multiple connections are allowed.</param>
            /// <param name="type">The type.</param>
            /// <param name="id">The unique box identifier.</param>
            /// <param name="valueIndex">The index of the node variable linked as the input. Usefull to make a physical connection between input box and default value for it.</param>
            /// <returns>The archetype.</returns>
            public static NodeElementArchetype Input(int ylevel, string text, bool single, ConnectionType type, int id, int valueIndex = -1)
            {
                return new NodeElementArchetype
                {
                    Type = NodeElementType.Input,
                    Position = new Vector2(
                        Constants.NodeMarginX - Constants.BoxOffsetX,
                        Constants.NodeMarginY + Constants.NodeHeaderSize + ylevel * Constants.LayoutOffsetY),
                    Text = text,
                    Single = single,
                    ValueIndex = valueIndex,
                    BoxID = id,
                    ConnectionsType = type
                };
            }

            /// <summary>
            /// Creates new Output box element description.
            /// </summary>
            /// <param name="ylevel">The y level.</param>
            /// <param name="text">The text.</param>
            /// <param name="type">The type.</param>
            /// <param name="id">The unique box identifier.</param>
            /// <returns>The archetype.</returns>
            public static NodeElementArchetype Output(int ylevel, string text, ConnectionType type, int id)
            {
                return new NodeElementArchetype
                {
                    Type = NodeElementType.Output,
                    Position = new Vector2(
                        Constants.NodeMarginX - Constants.BoxSize + Constants.BoxOffsetX,
                        Constants.NodeMarginY + Constants.NodeHeaderSize + ylevel * Constants.LayoutOffsetY),
                    Text = text,
                    Single = false,
                    ValueIndex = -1,
                    BoxID = id,
                    ConnectionsType = type
                };
            }

            /// <summary>
            /// Creates new Bool value element description.
            /// </summary>
            /// <param name="x">The x location (in node area space).</param>
            /// <param name="y">The y location (in node area space).</param>
            /// <param name="valueIndex">The index of the node variable linked as the input. Usefull to make a physical connection between input box and default value for it.</param>
            /// <returns>The archetype.</returns>
            public static NodeElementArchetype Bool(float x, float y, int valueIndex = -1)
            {
                return new NodeElementArchetype
                {
                    Type = NodeElementType.BoolValue,
                    Position = new Vector2(x, y),
                    Text = null,
                    Single = false,
                    ValueIndex = valueIndex,
                    BoxID = -1,
                    ConnectionsType = ConnectionType.Invalid
                };
            }

            /// <summary>
            /// Creates new Inteager value element description.
            /// </summary>
            /// <param name="x">The x location (in node area space).</param>
            /// <param name="y">The y location (in node area space).</param>
            /// <param name="valueIndex">The index of the node variable linked as the input. Usefull to make a physical connection between input box and default value for it.</param>
            /// <returns>The archetype.</returns>
            public static NodeElementArchetype Inteager(float x, float y, int valueIndex = -1)
            {
                return new NodeElementArchetype
                {
                    Type = NodeElementType.InteagerValue,
                    Position = new Vector2(Constants.NodeMarginX + x, Constants.NodeMarginY + Constants.NodeHeaderSize + y),
                    Text = null,
                    Single = false,
                    ValueIndex = valueIndex,
                    BoxID = -1,
                    ConnectionsType = ConnectionType.Invalid
                };
            }

            /// <summary>
            /// Creates new Color value element description.
            /// </summary>
            /// <param name="x">The x location (in node area space).</param>
            /// <param name="y">The y location (in node area space).</param>
            /// <param name="valueIndex">The index of the node variable linked as the input. Usefull to make a physical connection between input box and default value for it.</param>
            /// <returns>The archetype.</returns>
            public static NodeElementArchetype Color(float x, float y, int valueIndex = -1)
            {
                return new NodeElementArchetype
                {
                    Type = NodeElementType.ColorValue,
                    Position = new Vector2(Constants.NodeMarginX + x, Constants.NodeMarginY + Constants.NodeHeaderSize + y),
                    Text = null,
                    Single = false,
                    ValueIndex = valueIndex,
                    BoxID = -1,
                    ConnectionsType = ConnectionType.Invalid
                };
            }

            /// <summary>
            /// Creates new Asset picker element description.
            /// </summary>
            /// <param name="x">The x location (in node area space).</param>
            /// <param name="y">The y location (in node area space).</param>
            /// <param name="valueIndex">The index of the node variable linked as the input. Usefull to make a physical connection between input box and default value for it.</param>
            /// <param name="domain">The allowed assets domain to use.</param>
            /// <returns>The archetype.</returns>
            public static NodeElementArchetype Asset(float x, float y, int valueIndex, ContentDomain domain)
            {
                return new NodeElementArchetype
                {
                    Type = NodeElementType.ColorValue,
                    Position = new Vector2(x, y),
                    Text = null,
                    Single = false,
                    ValueIndex = valueIndex,
                    BoxID = (int)domain, // Pack it to int
                    ConnectionsType = ConnectionType.Invalid
                };
            }

            /// <summary>
            /// Creates new Combo Box element description.
            /// </summary>
            /// <param name="x">The x location (in node area space).</param>
            /// <param name="y">The y location (in node area space).</param>
            /// <param name="width">The width of the element.</param>
            /// <param name="valueIndex">The index of the node variable linked as the input. Usefull to make a physical connection between input box and default value for it.</param>
            /// <param name="values">The set of combo box items to present. May be nul if provided at runtime.</param>
            /// <returns>The archetype.</returns>
            public static NodeElementArchetype CmoboBox(float x, float y, int width, int valueIndex = -1, string[] values = null)
            {
                return new NodeElementArchetype
                {
                    Type = NodeElementType.ComboBox,
                    Position = new Vector2(x, y),
                    Text = values != null ? string.Join("\n", values) : null, // Pack all values to string separated with new line characters
                    Single = false,
                    ValueIndex = valueIndex,
                    BoxID = width, // Use Box ID to store combo box width
                    ConnectionsType = ConnectionType.Invalid
                };
            }

            /// <summary>
            /// Creates new Text element description.
            /// </summary>
            /// <param name="x">The x location (in node area space).</param>
            /// <param name="y">The y location (in node area space).</param>
            /// <param name="text">The text to show.</param>
            /// <returns>The archetype.</returns>
            public static NodeElementArchetype Text(float x, float y, string text)
            {
                return new NodeElementArchetype
                {
                    Type = NodeElementType.Text,
                    Position = new Vector2(x, y),
                    Text = text,
                    Single = false,
                    ValueIndex = -1,
                    BoxID = 0,
                    ConnectionsType = ConnectionType.Invalid
                };
            }
        }
    }
}
