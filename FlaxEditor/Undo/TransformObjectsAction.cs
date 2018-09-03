// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System;
using System.Collections.Generic;
using FlaxEditor.Modules;
using FlaxEditor.SceneGraph;
using FlaxEngine;

namespace FlaxEditor
{
    /// <summary>
    /// Implementation of <see cref="IUndoAction"/> used to transform a selection of <see cref="SceneGraphNode"/>.
    /// The same logic could be achieved using <see cref="UndoMultiBlock"/> but it would be slower.
    /// Since we use this kind of action very ofter (for <see cref="FlaxEditor.Gizmo.TransformGizmo"/> operations) it's better to provide faster implementation.
    /// </summary>
    /// <seealso cref="FlaxEditor.IUndoAction" />
    public sealed class TransformObjectsAction : UndoActionBase<TransformObjectsAction.DataStorage>, ISceneEditAction
    {
        /// <summary>
        /// The undo data.
        /// </summary>
        [Serializable]
        public struct DataStorage
        {
            /// <summary>
            /// The selection pool.
            /// </summary>
            public SceneGraphNode[] Selection;

            /// <summary>
            /// The 'before' state.
            /// </summary>
            public Transform[] Before;

            /// <summary>
            /// The 'after' state.
            /// </summary>
            public Transform[] After;
        }

        internal TransformObjectsAction(List<SceneGraphNode> selection, List<Transform> before)
        {
            Data = new DataStorage
            {
                Selection = selection.ToArray(),
                Before = before.ToArray(),
                After = selection.ConvertAll(x => x.Transform).ToArray(),
            };
        }

        /// <inheritdoc />
        public override string ActionString => "Transform object(s)";

        /// <inheritdoc />
        public override void Do()
        {
            var data = Data;
            for (int i = 0; i < data.Selection.Length; i++)
            {
                data.Selection[i].Transform = data.After[i];
            }
        }

        /// <inheritdoc />
        public override void Undo()
        {
            var data = Data;
            for (int i = 0; i < data.Selection.Length; i++)
            {
                data.Selection[i].Transform = data.Before[i];
            }
        }

        void ISceneEditAction.MarkSceneEdited(SceneModule sceneModule)
        {
            var data = Data;
            for (int i = 0; i < data.Selection.Length; i++)
            {
                sceneModule.MarkSceneEdited(data.Selection[i].ParentScene);
            }
        }
    }
}
