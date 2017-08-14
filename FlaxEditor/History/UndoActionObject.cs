////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using FlaxEditor.Utilities;

namespace FlaxEditor.History
{
    /// <summary>
    /// Undo action object.
    /// </summary>
    /// <seealso cref="IUndoAction" />
    [Serializable]
    public class UndoActionObject : UndoActionBase<UndoActionObject.DataStorage>
    {
        /// <summary>
        /// The data.
        /// </summary>
        [Serializable]
        public struct DataStorage
        {
            /// <summary>
            /// The difference.
            /// </summary>
            public MemberComparison[] Diff;

            /// <summary>
            /// The target object being modified.
            /// </summary>
            public object TargetInstance;
        }

        // For objects that cannot be referenced in undo action like: FlaxEngine.Object or SceneTreeNode we store them in DataStorage,
        // otherwise here:
        private object TargetInstance;

        /// <summary>
        /// Initializes a new instance of the <see cref="UndoActionObject"/> class.
        /// </summary>
        /// <param name="diff">The difference.</param>
        /// <param name="actionString">The action string.</param>
        /// <param name="targetInstance">The target instance.</param>
        public UndoActionObject(List<MemberComparison> diff, string actionString, object targetInstance)
        {
            bool useDataStorageForInstance = targetInstance is FlaxEngine.Object || targetInstance is SceneGraph.SceneTreeNode;

            ActionString = actionString;
            TargetInstance = useDataStorageForInstance ? null : targetInstance;
            Data = new DataStorage
            {
                Diff = diff.ToArray(),
                TargetInstance = useDataStorageForInstance ? targetInstance : null
            };
        }

        /// <inheritdoc />
        public override string ActionString { get; }

        /// <inheritdoc />
        public override void Do()
        {
            var data = Data;
            var targetInstance = data.TargetInstance ?? TargetInstance;
            for (var i = 0; i < data.Diff.Length; i++)
            {
                var diff = data.Diff[i];
                diff.SetMemberValue(targetInstance, diff.Value2);
            }
        }

        /// <inheritdoc />
        public override void Undo()
        {
            var data = Data;
            var targetInstance = data.TargetInstance ?? TargetInstance;
            for (var i = 0; i < data.Diff.Length; i++)
            {
                var diff = data.Diff[i];
                diff.SetMemberValue(targetInstance, diff.Value1);
            }
        }
    }
}
