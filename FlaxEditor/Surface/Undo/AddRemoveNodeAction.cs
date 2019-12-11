// Copyright (c) 2012-2019 Wojciech Figat. All rights reserved.

using System;
using FlaxEngine;

namespace FlaxEditor.Surface.Undo
{
    /// <summary>
    /// Add Visject Surface node undo action.
    /// </summary>
    /// <seealso cref="FlaxEditor.IUndoAction" />
    class AddRemoveNodeAction : IUndoAction
    {
        private readonly bool _isAdd;
        private VisjectSurface _surface;
        private ContextHandle _context;
        private uint _nodeId;
        private ushort _groupId;
        private ushort _typeId;
        private Vector2 _nodeLocation;
        private object[] _nodeValues;

        public AddRemoveNodeAction(SurfaceNode node, bool isAdd)
        {
            _surface = node.Surface;
            _context = new ContextHandle(node.Context);
            _nodeId = node.ID;
            _isAdd = isAdd;
        }

        /// <inheritdoc />
        public string ActionString => _isAdd ? "Add node" : "Remove node";

        /// <inheritdoc />
        public void Do()
        {
            if (_isAdd)
                Add();
            else
                Remove();
        }

        /// <inheritdoc />
        public void Undo()
        {
            if (_isAdd)
                Remove();
            else
                Add();
        }

        private void Add()
        {
            var context = _context.Get(_surface);
            if (_nodeId == 0)
                throw new Exception("Node already added.");

            // Create node
            var node = NodeFactory.CreateNode(context.Surface.NodeArchetypes, _nodeId, context, _groupId, _typeId);
            if (node == null)
                throw new Exception("Failed to create node.");
            context.Nodes.Add(node);

            // Initialize
            if (node.Values != null && node.Values.Length == _nodeValues.Length)
                Array.Copy(_nodeValues, node.Values, _nodeValues.Length);
            else if (_nodeValues != null && _nodeValues.Length != 0)
                throw new InvalidOperationException("Invalid node values.");
            context.OnControlLoaded(node);
            node.OnSurfaceLoaded();
            node.Location = _nodeLocation;

            context.MarkAsModified();
        }

        private void Remove()
        {
            var context = _context.Get(_surface);
            var node = context.FindNode(_nodeId);
            if (node == null)
                throw new Exception("Missing node to remove.");

            // Cache node state
            _nodeId = node.ID;
            _groupId = node.GroupArchetype.GroupID;
            _typeId = node.Archetype.TypeID;
            _nodeLocation = node.Location;
            _nodeValues = (object[])node.Values?.Clone();

            // Remove node
            context.Nodes.Remove(node);
            context.OnControlDeleted(node);
            context.MarkAsModified();
        }

        /// <inheritdoc />
        public void Dispose()
        {
            _surface = null;
            _nodeValues = null;
        }
    }
}
