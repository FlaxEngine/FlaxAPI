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
    public class UndoActionObject : IUndoAction
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UndoActionObject"/> class.
        /// </summary>
        /// <param name="diff">The difference.</param>
        /// <param name="actionString">The action string.</param>
        /// <param name="targetInstance">The target instance.</param>
        public UndoActionObject(List<MemberComparison> diff, string actionString, object targetInstance)
        {
            Diff = diff;
            ActionString = actionString;
            TargetInstance = targetInstance;
        }

        /// <summary>
        /// Gets the difference.
        /// </summary>
        public List<MemberComparison> Diff { get; }

        /// <summary>
        /// Gets the target object being modified.
        /// </summary>
        public object TargetInstance { get; }
        
        /// <inheritdoc />
        public string ActionString { get; }

        /// <inheritdoc />
        public void Do()
        {
            for (var i = 0; i < Diff.Count; i++)
            {
                var diff = Diff[i];
                diff.SetMemberValue(TargetInstance, diff.Value2);
            }
        }

        /// <inheritdoc />
        public void Undo()
        {
            for (var i = 0; i < Diff.Count; i++)
            {
                var diff = Diff[i];
                diff.SetMemberValue(TargetInstance, diff.Value1);
            }
        }
    }
}
