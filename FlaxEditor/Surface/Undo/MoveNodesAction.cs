// Copyright (c) 2012-2019 Wojciech Figat. All rights reserved.

using System;
using FlaxEngine;

namespace FlaxEditor.Surface.Undo
{
    /// <summary>
    /// Move Visject Surface nodes undo action.
    /// </summary>
    /// <seealso cref="FlaxEditor.IUndoAction" />
    class MoveNodesAction : IUndoAction
    {
        private VisjectSurfaceContext _context;
        private uint[] _nodeIds;
        private readonly Vector2 _locationDelta;

        public MoveNodesAction(VisjectSurfaceContext context, uint[] nodeIds, Vector2 locationDelta)
        {
            _context = context;
            _nodeIds = nodeIds;
            _locationDelta = locationDelta;
        }

        /// <inheritdoc />
        public string ActionString => "Move nodes";

        /// <inheritdoc />
        public void Do()
        {
            Apply(_locationDelta);
        }

        /// <inheritdoc />
        public void Undo()
        {
            Apply(-_locationDelta);
        }

        private void Apply(Vector2 delta)
        {
            foreach (var nodeId in _nodeIds)
            {
                var node = _context.FindNode(nodeId);
                if (node == null)
                    throw new Exception("Missing node.");

                node.Location += delta;
            }

            _context.MarkAsModified(false);
        }

        /// <inheritdoc />
        public void Dispose()
        {
            _context = null;
            _nodeIds = null;
        }
    }
}
