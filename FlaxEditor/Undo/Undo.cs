////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Linq;
using FlaxEditor.History;
using FlaxEditor.Utilities;
using FlaxEngine.Collections;

namespace FlaxEditor
{
    /// <summary>
    /// The undo/redo actions recording object.
    /// </summary>
    public class Undo : IDisposable
    {
        /// <summary>
        ///     Stack of undo actions for future disposal.
        /// </summary>
        private readonly OrderedDictionary<object, UndoInternal> _snapshots = new OrderedDictionary<object, UndoInternal>();

        /// <summary>
        /// Gets the undo operations stack.
        /// </summary>
        /// <value>
        /// The undo operations stack.
        /// </value>
        public HistoryStack UndoOperationsStack { get; } = new HistoryStack();

        /// <summary>
        /// Occurs when undo operation is done.
        /// </summary>
        public event Action UndoDone;

        /// <summary>
        /// Occurs when redo operation is done.
        /// </summary>
        public event Action RedoDone;

        /// <summary>
        /// Occurs when action is done and appended to the <see cref="Undo"/>.
        /// </summary>
        public event Action ActionDone;

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Undo"/> is enabled.
        /// </summary>
        /// <value>
        ///   <c>true</c> if enabled; otherwise, <c>false</c>.
        /// </value>
        public virtual bool Enabled { get; set; } = true;

        /// <summary>
        /// Gets a value indicating whether can do undo on last performed action.
        /// </summary>
        /// <value>
        ///   <c>true</c> if can perform undo; otherwise, <c>false</c>.
        /// </value>
        public bool CanUndo => UndoOperationsStack.HistoryCount > 0;

        /// <summary>
        /// Gets a value indicating whether can do redo on last undone action.
        /// </summary>
        /// <value>
        ///   <c>true</c> if can perform redo; otherwise, <c>false</c>.
        /// </value>
        public bool CanRedo => UndoOperationsStack.ReverseCount > 0;

        /// <summary>
        /// Gets the first name of the undo action.
        /// </summary>
        /// <value>
        /// The first name of the undo action.
        /// </value>
        public string FirstUndoName => UndoOperationsStack.PeekHistory().ActionString;

        /// <summary>
        /// Gets the first name of the redo action.
        /// </summary>
        /// <value>
        /// The first name of the redo action.
        /// </value>
        public string FirstRedoName => UndoOperationsStack.PeekReverse().ActionString;

        /// <summary>
        ///     Internal class for keeping reference of undo action.
        /// </summary>
        internal class UndoInternal
        {
            public string ActionString;
            public object SnapshotInstance;
            public ObjectSnapshot Snapshot;

            public UndoInternal(object snapshotInstance, string actionString)
            {
                ActionString = actionString;
                SnapshotInstance = snapshotInstance;
                Snapshot = ObjectSnapshot.CaptureSnapshot(snapshotInstance);
            }

            /// <summary>
            /// Creates the undo action object.
            /// </summary>
            /// <param name="diff">The difference.</param>
            /// <returns>The undo action.</returns>
            public UndoActionObject CreateUndoActionObject(List<MemberComparison> diff)
            {
                return new UndoActionObject(diff, ActionString, SnapshotInstance);
            }
        }

        /// <summary>
        ///     Begins recording for undo action.
        /// </summary>
        /// <param name="snapshotInstance">Instance of an object to record.</param>
        /// <param name="actionString">Name of action to be displayed in undo stack.</param>
        public void RecordBegin(object snapshotInstance, string actionString)
        {
            if (!Enabled)
                return;

            _snapshots.Add(snapshotInstance, new UndoInternal(snapshotInstance, actionString));
        }

        /// <summary>
        ///     Ends recording for undo action.
        /// </summary>
        /// <param name="snapshotInstance">Instance of an object to finish recording, if null take last provided.</param>
        public void RecordEnd(object snapshotInstance = null)
        {
            if (!Enabled)
                return;

            if (snapshotInstance == null)
            {
                snapshotInstance = _snapshots.Last().Key;
            }
            var changes = _snapshots[snapshotInstance].Snapshot.Compare(snapshotInstance);
            var action = _snapshots[snapshotInstance].CreateUndoActionObject(changes);
            UndoOperationsStack.Push(action);
            _snapshots.Remove(snapshotInstance);

            OnAction(action);
        }

        /// <summary>
        ///     Creates new undo action for provided instance of object.
        /// </summary>
        /// <param name="snapshotInstance">Instance of an object to record</param>
        /// <param name="actionString">Name of action to be displayed in undo stack.</param>
        /// <param name="actionsToSave">Action in after witch recording will be finished.</param>
        public void RecordAction(object snapshotInstance, string actionString, Action actionsToSave)
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
        public void RecordAction<T>(T snapshotInstance, string actionString, Action<T> actionsToSave)
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
        public void RecordAction(object snapshotInstance, string actionString, Action<object> actionsToSave)
        {
            RecordBegin(snapshotInstance, actionString);
            actionsToSave?.Invoke(snapshotInstance);
            RecordEnd(snapshotInstance);
        }

        /// <summary>
        /// Adds the action to the history.
        /// </summary>
        /// <param name="action">The action.</param>
        public void AddAction(IUndoAction action)
        {
            if (action == null)
                throw new ArgumentNullException();

            if (!Enabled)
                return;

            UndoOperationsStack.Push(action);

            OnAction(action);
        }

        /// <summary>
        ///     Undo last recorded action
        /// </summary>
        public void PerformUndo()
        {
            if (!Enabled || !CanUndo)
                return;

            var action = (IUndoAction)UndoOperationsStack.PopHistory();
            action.Undo();
            
            OnUndo(action);
        }

        /// <summary>
        ///     Redo last undone action
        /// </summary>
        public void PerformRedo()
        {
            if (!Enabled || !CanRedo)
                return;

            var action = (IUndoAction)UndoOperationsStack.PopReverse();
            action.Do();

            OnRedo(action);
        }

        /// <summary>
        /// Called when <see cref="Undo"/> performs action.
        /// </summary>
        /// <param name="action">The action.</param>
        protected virtual void OnAction(IUndoAction action)
        {
            ActionDone?.Invoke();
        }
        
        /// <summary>
        /// Called when <see cref="Undo"/> performs undo action.
        /// </summary>
        /// <param name="action">The action.</param>
        protected virtual void OnUndo(IUndoAction action)
        {
            UndoDone?.Invoke();
        }
        
        /// <summary>
        /// Called when <see cref="Undo"/> performs redo action.
        /// </summary>
        /// <param name="action">The action.</param>
        protected virtual void OnRedo(IUndoAction action)
        {
            RedoDone?.Invoke();
        }

        /// <summary>
        /// Clears the history.
        /// </summary>
        public void Clear()
        {
            _snapshots.Clear();
            UndoOperationsStack.Clear();
        }

        /// <inheritdoc />
        public void Dispose()
        {
            UndoDone = null;
            RedoDone = null;
            ActionDone = null;

            Clear();
        }
    }
}
