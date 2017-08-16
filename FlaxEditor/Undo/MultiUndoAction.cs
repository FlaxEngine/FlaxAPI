////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Linq;

namespace FlaxEditor
{
    /// <summary>
    /// Implementation of <see cref="IUndoAction"/> that contains one or more child actions performed at once. Allows to merge diffrent actions.
    /// </summary>
    /// <seealso cref="FlaxEditor.IUndoAction" />
    public class MultiUndoAction : IUndoAction
    {
        private IUndoAction[] _actions;

        /// <summary>
        /// Initializes a new instance of the <see cref="MultiUndoAction"/> class.
        /// </summary>
        /// <param name="actions">The actions to include within this multi action.</param>
        public MultiUndoAction(IEnumerable<IUndoAction> actions)
        {
            _actions = actions?.ToArray() ?? throw new ArgumentNullException();
            if (_actions.Length == 0)
                throw new ArgumentException("Empty actions collection.");
        }

        /// <inheritdoc />
        public string ActionString => _actions[0].ActionString;

        /// <inheritdoc />
        public void Do()
        {
            for (int i = 0; i < _actions.Length; i++)
            {
                _actions[i].Do();
            }
        }

        /// <inheritdoc />
        public void Undo()
        {
            for (int i = 0; i < _actions.Length; i++)
            {
                _actions[i].Undo();
            }
        }
    }
}
