// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System;

namespace FlaxEditor
{
    /// <summary>
    /// Helper class to record undo operations in a block with <c>using</c> keyword.
    /// <example>
    /// using(new UndoBlock(undo, obj, "Rename"))
    /// {
    ///     obj.Name = "super name";
    /// }
    /// </example>
    /// </summary>
    /// <seealso cref="System.IDisposable" />
    public class UndoBlock : IDisposable
    {
        private readonly Undo _undo;
        private readonly object _snapshotUndoInternal;

        /// <summary>
        ///     Creates new undo object for recording actions with using pattern.
        /// </summary>
        /// <param name="undo">The undo/redo object.</param>
        /// <param name="snapshotInstance">Instance of an object to record.</param>
        /// <param name="actionString">Name of action to be displayed in undo stack.</param>
        public UndoBlock(Undo undo, object snapshotInstance, string actionString)
        {
            _snapshotUndoInternal = snapshotInstance;
            _undo = undo;
            _undo.RecordBegin(_snapshotUndoInternal, actionString);
        }

        /// <inheritdoc />
        public void Dispose()
        {
            _undo.RecordEnd(_snapshotUndoInternal);
        }
    }
}
