////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Reflection;
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
            /// The values 1.
            /// </summary>
            public object[] Values1;

            /// <summary>
            /// The values 2.
            /// </summary>
            public object[] Values2;

            /// <summary>
            /// The target object being modified (as FlaxEngine.Object).
            /// </summary>
            public FlaxEngine.Object Ref1;

            /// <summary>
            /// The target object being modified (as FlaxEditor.SceneGraph.SceneTreeNode).
            /// </summary>
            public SceneGraph.SceneTreeNode Ref2;
        }

        private struct DataPrepared
        {
            public MemberComparison[] Diff;
            public object TargetInstance;
        }

        // For objects that cannot be referenced in undo action like: FlaxEngine.Object or SceneTreeNode we store them in DataStorage,
        // otherwise here:
        private object TargetInstance;

        private MemberInfo[] Members;

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

            int count = diff.Count;
            var values1 = new object[count];
            var values2 = new object[count];
            Members = new MemberInfo[count];
            for (int i = 0; i < count; i++)
            {
                values1[i] = diff[i].Value1;
                values2[i] = diff[i].Value2;
                Members[i] = diff[i].Member;
            }

            Data = new DataStorage
            {
                Values1 = values1,
                Values2 = values2,
                Ref1 = useDataStorageForInstance ? targetInstance as FlaxEngine.Object : null,
                Ref2 = useDataStorageForInstance ? targetInstance as SceneGraph.SceneTreeNode : null,
            };
        }

        private DataPrepared PrepareData()
        {
            var data = Data;
            var count = data.Values1.Length;
            var diff = new MemberComparison[count];
            for (int i = 0; i < count; i++)
            {
                diff[i] = new MemberComparison(Members[i], data.Values1[i], data.Values2[i]);
            }
            return new DataPrepared
            {
                Diff = diff,
                TargetInstance = data.Ref1 ?? data.Ref2 ?? TargetInstance,
            };
        }

        /// <inheritdoc />
        public override string ActionString { get; }

        /// <inheritdoc />
        public override void Do()
        {
            var data = PrepareData();
            for (var i = 0; i < data.Diff.Length; i++)
            {
                var diff = data.Diff[i];
                diff.SetMemberValue(data.TargetInstance, diff.Value2);
            }
        }

        /// <inheritdoc />
        public override void Undo()
        {
            var data = PrepareData();
            for (var i = 0; i < data.Diff.Length; i++)
            {
                var diff = data.Diff[i];
                diff.SetMemberValue(data.TargetInstance, diff.Value1);
            }
        }
    }
}
