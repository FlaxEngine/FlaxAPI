using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using FlaxEditor.History;
using FlaxEditor.Utilities;
using FlaxEngine.Collections;
using FlaxEngine.Utilities;

namespace FlaxEditor
{
    public partial class Undo : IDisposable
    {
        /// <summary>
        ///     Stack of undo actions for future disposal.
        /// </summary>
        private static readonly OrderedDictionary<object, UndoInternal> _snapshoots =
            new OrderedDictionary<object, UndoInternal>();

        /// <summary>
        ///     Internal class for keeping reference of undo action.
        /// </summary>
        internal class UndoInternal : IHistoryAction
        {
            public Guid Id { get; }
            public string ActionString { get; }
            public object CopyOfInstance { get; }

            public UndoInternal(object snapshootInstance, string actionString)
            {
                ActionString = actionString;
                Id = Guid.NewGuid();
                CopyOfInstance = snapshootInstance.DeepClone();
            }
        }

        /// <summary>
        ///     Begins recording for undo action.
        /// </summary>
        /// <param name="snapshootInstance">Instance of an object to record.</param>
        /// <param name="actionString">Name of action to be displayed in undo stack.</param>
        public static void RecordBegin(object snapshootInstance, string actionString)
        {
            _snapshoots.Add(snapshootInstance, new UndoInternal(snapshootInstance, actionString));
        }

        /// <summary>
        ///     Ends recording for undo action.
        /// </summary>
        /// <param name="snapshootInstance">Instance of an object to finish recording, if null take last provided.</param>
        public static void RecordEnd(object snapshootInstance = null)
        {
            if (snapshootInstance == null)
            {
                snapshootInstance = _snapshoots.Last().Key;
            }
            var changes = snapshootInstance.ReflectiveCompare(_snapshoots[snapshootInstance].CopyOfInstance);
            _snapshoots.Remove(snapshootInstance);
        }

        /// <summary>
        ///     Creates new undo action for provided instance of object.
        /// </summary>
        /// <param name="snapshootInstance">Instance of an object to record</param>
        /// <param name="actionString">Name of action to be displayed in undo stack.</param>
        /// <param name="actionsToSave">Action in after witch recording will be finished.</param>
        public static void RecordAction(object snapshootInstance, string actionString, Action actionsToSave)
        {
            RecordBegin(snapshootInstance, actionString);
            actionsToSave?.Invoke();
            RecordEnd(snapshootInstance);
        }

        /// <summary>
        ///     Undo last recorded action
        /// </summary>
        public static void PerformUndo()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     Redo last undone action
        /// </summary>
        public static void PerformRedo()
        {
            throw new NotImplementedException();
        }

        private object SnapshotUndoInternal { get; }

        /// <summary>
        ///     Creates new undo object for recording actions with using pattern.
        /// </summary>
        /// <param name="snapshootInstance">Instance of an object to record.</param>
        /// <param name="actionString">Name of action to be displayed in undo stack.</param>
        public Undo(object snapshootInstance, string actionString)
        {
            RecordBegin(snapshootInstance, actionString);
            SnapshotUndoInternal = snapshootInstance;
        }

        /// <inheritdoc />
        public void Dispose()
        {
            RecordEnd(SnapshotUndoInternal);
        }
    }
}