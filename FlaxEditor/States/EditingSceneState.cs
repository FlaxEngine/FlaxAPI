// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System;

namespace FlaxEditor.States
{
    /// <summary>
    /// In this state user may edit scene and use editor normally.
    /// </summary>
    /// <seealso cref="FlaxEditor.States.EditorState" />
    public sealed class EditingSceneState : EditorState
    {
        /// <inheritdoc />
        public override bool CanUseToolbox => true;

        /// <inheritdoc />
        public override bool CanUseUndoRedo => true;

        /// <inheritdoc />
        public override bool CanChangeScene => true;

        /// <inheritdoc />
        public override bool CanEditScene => true;

        /// <inheritdoc />
        public override bool CanEnterPlayMode => true;

        /// <inheritdoc />
        public override bool CanReloadScripts => true;

        internal EditingSceneState(Editor editor)
        : base(editor)
        {
        }
    }
}
