// Copyright (c) 2012-2019 Wojciech Figat. All rights reserved.

using System;
using System.Collections.Generic;
using FlaxEditor.Surface.Elements;

namespace FlaxEditor.Surface.Undo
{
    /// <summary>
    /// Edit Visject Surface node boxes connections undo action.
    /// </summary>
    /// <seealso cref="FlaxEditor.IUndoAction" />
    class EditNodeConnections : IUndoAction
    {
        private VisjectSurfaceContext _context;
        private readonly uint _nodeId;
        private BoxHandle[][] _before;
        private BoxHandle[][] _after;

        public EditNodeConnections(VisjectSurfaceContext context, SurfaceNode node)
        {
            _context = context;

            _nodeId = node.ID;

            CaptureConnections(node, out _before);
        }

        public void End()
        {
            var node = _context.FindNode(_nodeId);
            if (node == null)
                throw new Exception("Missing node");

            CaptureConnections(node, out _after);
        }

        private void CaptureConnections(SurfaceNode node, out BoxHandle[][] boxesConnections)
        {
            var boxes = node.GetBoxes();
            boxesConnections = new BoxHandle[boxes.Count][];
            for (int i = 0; i < boxesConnections.Length; i++)
            {
                var box = boxes[i];
                var connections = new BoxHandle[box.Connections.Count];
                boxesConnections[i] = connections;
                for (int j = 0; j < connections.Length; j++)
                {
                    var other = box.Connections[j];
                    connections[j] = new BoxHandle(other);
                }
            }
        }

        /// <inheritdoc />
        public string ActionString => "Edit connections";

        /// <inheritdoc />
        public void Do()
        {
            Execute(_after);
        }

        /// <inheritdoc />
        public void Undo()
        {
            Execute(_before);
        }

        private void Execute(BoxHandle[][] boxesConnections)
        {
            var node = _context.FindNode(_nodeId);
            if (node == null)
                throw new Exception("Missing node");

            var toUpdate = new HashSet<Box>();
            var boxes = node.GetBoxes();

            for (var i = 0; i < boxes.Count; i++)
            {
                var box = boxes[i];
                toUpdate.Add(box);

                for (int j = 0; j < box.Connections.Count; j++)
                {
                    var other = box.Connections[j];
                    toUpdate.Add(other);
                    other.Connections.Remove(box);
                }

                box.Connections.Clear();
            }

            for (var i = 0; i < boxes.Count; i++)
            {
                var box = boxes[i];
                var connections = boxesConnections[i];

                for (int j = 0; j < connections.Length; j++)
                {
                    var other = connections[j].GetBox(_context);
                    toUpdate.Add(other);
                    box.Connections.Add(other);
                    other.Connections.Add(box);
                }
            }

            foreach (var box in toUpdate)
            {
                box.OnConnectionsChanged();
                box.ConnectionTick();
            }

            _context.MarkAsModified();
        }

        /// <inheritdoc />
        public void Dispose()
        {
            _context = null;
            _before = null;
            _after = null;
        }
    }
}
