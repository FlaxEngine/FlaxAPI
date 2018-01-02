////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2018 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

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
        private readonly Undo Undo;
        private readonly object SnapshotUndoInternal;

        /// <summary>
        ///     Creates new undo object for recording actions with using pattern.
        /// </summary>
        /// <param name="undo">The undo/redo object.</param>
        /// <param name="snapshotInstance">Instance of an object to record.</param>
        /// <param name="actionString">Name of action to be displayed in undo stack.</param>
        public UndoBlock(Undo undo, object snapshotInstance, string actionString)
        {
            SnapshotUndoInternal = snapshotInstance;
            Undo = undo;
            Undo.RecordBegin(SnapshotUndoInternal, actionString);
        }

        /// <inheritdoc />
        public void Dispose()
        {
            Undo.RecordEnd(SnapshotUndoInternal);
        }
    }
}
