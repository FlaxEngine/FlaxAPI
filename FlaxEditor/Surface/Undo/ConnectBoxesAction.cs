// Copyright (c) 2012-2019 Wojciech Figat. All rights reserved.

using System.Collections.Generic;
using FlaxEditor.Surface.Elements;
using FlaxEngine.Utilities;

namespace FlaxEditor.Surface.Undo
{
    /// <summary>
    /// Edit Visject Surface node boxes connection undo action.
    /// </summary>
    /// <seealso cref="FlaxEditor.IUndoAction" />
    class ConnectBoxesAction : IUndoAction
    {
        private VisjectSurfaceContext _context;
        private bool _connect;
        private BoxHandle _input;
        private BoxHandle _output;
        private BoxHandle[] _inputBefore;
        private BoxHandle[] _inputAfter;
        private BoxHandle[] _outputBefore;
        private BoxHandle[] _outputAfter;

        public ConnectBoxesAction(VisjectSurfaceContext context, InputBox iB, OutputBox oB, bool connect)
        {
            _context = context;
            _connect = connect;

            _input = new BoxHandle(iB);
            _output = new BoxHandle(oB);

            CaptureConnections(iB, out _inputBefore);
            CaptureConnections(oB, out _outputBefore);
        }

        public void End()
        {
            var iB = _input.GetBox(_context);
            var oB = _output.GetBox(_context);

            CaptureConnections(iB, out _inputAfter);
            CaptureConnections(oB, out _outputAfter);
        }

        private void CaptureConnections(Box box, out BoxHandle[] connections)
        {
            connections = new BoxHandle[box.Connections.Count];
            for (int i = 0; i < connections.Length; i++)
            {
                var other = box.Connections[i];
                connections[i] = new BoxHandle(other);
            }
        }

        /// <inheritdoc />
        public string ActionString => _connect ? "Connect boxes" : "Disconnect boxes";

        /// <inheritdoc />
        public void Do()
        {
            Execute(_inputAfter, _outputAfter);
        }

        /// <inheritdoc />
        public void Undo()
        {
            Execute(_inputBefore, _outputBefore);
        }

        private void Execute(BoxHandle[] input, BoxHandle[] output)
        {
            var iB = _input.GetBox(_context);
            var oB = _output.GetBox(_context);

            var toUpdate = new HashSet<Box>();
            toUpdate.Add(iB);
            toUpdate.Add(oB);
            toUpdate.AddRange(iB.Connections);
            toUpdate.AddRange(oB.Connections);

            for (int i = 0; i < iB.Connections.Count; i++)
            {
                var box = iB.Connections[i];
                box.Connections.Remove(iB);
            }
            for (int i = 0; i < oB.Connections.Count; i++)
            {
                var box = oB.Connections[i];
                box.Connections.Remove(oB);
            }

            iB.Connections.Clear();
            oB.Connections.Clear();

            for (int i = 0; i < input.Length; i++)
            {
                var box = input[i].GetBox(_context);
                iB.Connections.Add(box);
                box.Connections.Add(iB);
            }
            for (int i = 0; i < output.Length; i++)
            {
                var box = output[i].GetBox(_context);
                oB.Connections.Add(box);
                box.Connections.Add(oB);
            }

            toUpdate.AddRange(iB.Connections);
            toUpdate.AddRange(oB.Connections);

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
            _inputBefore = null;
            _inputAfter = null;
            _outputBefore = null;
            _outputAfter = null;
        }
    }
}
