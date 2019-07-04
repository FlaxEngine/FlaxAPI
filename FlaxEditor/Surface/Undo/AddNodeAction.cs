// Copyright (c) 2012-2019 Wojciech Figat. All rights reserved.

using System;
using FlaxEngine;

namespace FlaxEditor.Surface.Undo
{
    /// <summary>
    /// Add Visject Surface node undo action.
    /// </summary>
    /// <seealso cref="FlaxEditor.IUndoAction" />
    class AddNodeAction : IUndoAction
    {
        private VisjectSurfaceContext _context;
        private uint _nodeId;
        private ushort _groupId;
        private ushort _typeId;
        private Vector2 _nodeLocation;
        private object[] _nodeValues;

        public AddNodeAction(VisjectSurfaceContext context, SurfaceNode node)
        {
            _context = context;
            _nodeId = node.ID;
        }

        /// <inheritdoc />
        public string ActionString => "Add node";

        /// <inheritdoc />
        public void Do()
        {
            if (_nodeId == 0)
                throw new Exception("Node already added.");

            // Create node
            var node = NodeFactory.CreateNode(_context.Surface.NodeArchetypes, _nodeId, _context, _groupId, _typeId);
            if (node == null)
                throw new Exception("Failed to create node.");
            _context.Nodes.Add(node);

            // Initialize
            if (node.Values != null && node.Values.Length == _nodeValues.Length)
                Array.Copy(_nodeValues, node.Values, _nodeValues.Length);
            else
                throw new InvalidOperationException("Invalid node values.");
            _context.OnControlLoaded(node);
            node.OnSurfaceLoaded();
            node.Location = _nodeLocation;
            _context.OnControlSpawned(node);

            _context.MarkAsModified();
        }

        /// <inheritdoc />
        public void Undo()
        {
            var node = _context.FindNode(_nodeId);
            if (node == null)
                throw new Exception("Missing node to remove.");

            // Cache node state
            _nodeId = node.ID;
            _groupId = node.GroupArchetype.GroupID;
            _typeId = node.Archetype.TypeID;
            _nodeLocation = node.Location;
            _nodeValues = (object[])node.Values.Clone();

            // Remove node
            _context.Nodes.Remove(node);
            _context.OnControlDeleted(node);
            _context.MarkAsModified();
        }

        /// <inheritdoc />
        public void Dispose()
        {
            _context = null;
        }
    }
}
