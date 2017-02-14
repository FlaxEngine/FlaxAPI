using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
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
        private static readonly OrderedDictionary<object, UndoInternal> _snapshots =
            new OrderedDictionary<object, UndoInternal>();

        public static HistoryStack UndoOperationsStack { get; } = new HistoryStack();

        /// <summary>
        ///     Internal class for keeping reference of undo action.
        /// </summary>
        internal class UndoInternal : IHistoryAction
        {
            public Guid Id { get; }
            public string ActionString { get; }
            public object CopyOfInstance { get; }
            public object SnapshotInstance { get; }

            public UndoInternal(object snapshotInstance, string actionString)
            {
                ActionString = actionString;
                Id = Guid.NewGuid();
                SnapshotInstance = snapshotInstance;
                CopyOfInstance = snapshotInstance.DeepClone();
            }

            public UndoActionObject CreateUndoActionObject(List<MemberComparison> diff)
            {
                return new UndoActionObject(diff, ActionString, Id, SnapshotInstance);
            }

        }

        /// <summary>
        ///     Begins recording for undo action.
        /// </summary>
        /// <param name="snapshotInstance">Instance of an object to record.</param>
        /// <param name="actionString">Name of action to be displayed in undo stack.</param>
        public static void RecordBegin(object snapshotInstance, string actionString)
        {
            _snapshots.Add(snapshotInstance, new UndoInternal(snapshotInstance, actionString));
        }

        /// <summary>
        ///     Ends recording for undo action.
        /// </summary>
        /// <param name="snapshotInstance">Instance of an object to finish recording, if null take last provided.</param>
        public static void RecordEnd(object snapshotInstance = null)
        {
            if (snapshotInstance == null)
            {
                snapshotInstance = _snapshots.Last().Key;
            }
            var changes = snapshotInstance.ReflectiveCompare(_snapshots[snapshotInstance].CopyOfInstance);
            UndoOperationsStack.Push(_snapshots[snapshotInstance].CreateUndoActionObject(changes));
            _snapshots.Remove(snapshotInstance);
        }

        /// <summary>
        ///     Creates new undo action for provided instance of object.
        /// </summary>
        /// <param name="snapshotInstance">Instance of an object to record</param>
        /// <param name="actionString">Name of action to be displayed in undo stack.</param>
        /// <param name="actionsToSave">Action in after witch recording will be finished.</param>
        public static void RecordAction(object snapshotInstance, string actionString, Action actionsToSave)
        {
            RecordBegin(snapshotInstance, actionString);
            actionsToSave?.Invoke();
            RecordEnd(snapshotInstance);
        }

        /// <summary>
        ///     Creates new undo action for provided instance of object.
        /// </summary>
        /// <param name="snapshotInstance">Instance of an object to record</param>
        /// <param name="actionString">Name of action to be displayed in undo stack.</param>
        /// <param name="actionsToSave">Action in after witch recording will be finished.</param>
        public static void RecordAction<T>(T snapshotInstance, string actionString, Action<T> actionsToSave)
            where T : new()
        {
            RecordBegin(snapshotInstance, actionString);
            actionsToSave?.Invoke(snapshotInstance);
            RecordEnd(snapshotInstance);
        }

        /// <summary>
        ///     Creates new undo action for provided instance of object.
        /// </summary>
        /// <param name="snapshotInstance">Instance of an object to record</param>
        /// <param name="actionString">Name of action to be displayed in undo stack.</param>
        /// <param name="actionsToSave">Action in after witch recording will be finished.</param>
        public static void RecordAction(object snapshotInstance, string actionString, Action<object> actionsToSave)
        {
            RecordBegin(snapshotInstance, actionString);
            actionsToSave?.Invoke(snapshotInstance);
            RecordEnd(snapshotInstance);
        }

        /// <summary>
        ///     Undo last recorded action
        /// </summary>
        public static UndoActionObject PerformUndo()
        {
            UndoActionObject operation = (UndoActionObject)UndoOperationsStack.PopHistory();
            foreach (var diff in operation.Diff)
            {
                if (diff.Member.MemberType == MemberTypes.Field)
                {
                    ((FieldInfo)diff.Member).SetValue(operation.TargetInstance, diff.Value2);
                }
                else
                {
                    ((PropertyInfo)diff.Member).SetValue(operation.TargetInstance, diff.Value2);
                }
            }
            return operation;
        }

        /// <summary>
        ///     Redo last undone action
        /// </summary>
        public static UndoActionObject PerformRedo()
        {
            UndoActionObject operation = (UndoActionObject)UndoOperationsStack.PopReverse();
            foreach (var diff in operation.Diff)
            {
                if (diff.Member.MemberType == MemberTypes.Field)
                {
                    ((FieldInfo)diff.Member).SetValue(operation.TargetInstance, diff.Value1);
                }
                else
                {
                    ((PropertyInfo)diff.Member).SetValue(operation.TargetInstance, diff.Value1);
                }
            }
            return operation;
        }

        private object SnapshotUndoInternal { get; }

        /// <summary>
        ///     Creates new undo object for recording actions with using pattern.
        /// </summary>
        /// <param name="snapshotInstance">Instance of an object to record.</param>
        /// <param name="actionString">Name of action to be displayed in undo stack.</param>
        public Undo(object snapshotInstance, string actionString)
        {
            RecordBegin(snapshotInstance, actionString);
            SnapshotUndoInternal = snapshotInstance;
        }

        /// <inheritdoc />
        public void Dispose()
        {
            RecordEnd(SnapshotUndoInternal);
        }
    }
}