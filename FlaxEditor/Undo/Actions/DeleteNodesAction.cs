////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System.Collections.Generic;
using FlaxEditor.SceneGraph;
using FlaxEngine;

namespace FlaxEditor.Actions
{
    /// <summary>
    /// Implementation of <see cref="IUndoAction"/> used to delete a selection of <see cref="SceneGraphNode"/>.
    /// </summary>
    /// <seealso cref="FlaxEditor.IUndoAction" />
    public sealed class DeleteNodesAction : IUndoAction
    {
        private List<ActorNode> _nodes;
        private byte[] _data;

        internal DeleteNodesAction(List<SceneGraphNode> objects)
        {
            _nodes = new List<ActorNode>(objects.Count);
            List<Actor> actors = new List<Actor>(objects.Count);
            for (int i = 0; i < objects.Count; i++)
            {
                if (objects[i] is ActorNode node)
                {
                    _nodes.Add(node);
                    actors.Add(node.Actor);
                }
            }

            _data = Actor.ToBytes(actors.ToArray());
        }

        /// <inheritdoc />
        public string ActionString => "Delete object(s)";

        /// <inheritdoc />
        public void Do()
        {
            // Remove objects
            for (int i = 0; i < _nodes.Count; i++)
            {
                var node = _nodes[i];
                Editor.Instance.Scene.MarkSceneEdited(node.ParentScene);
                node.Delete();
            }
            _nodes.Clear();
        }

        /// <inheritdoc />
        public void Undo()
        {
            // Restore objects
            var actors = Actor.FromBytes(_data);
            if (actors == null)
                return;
            for (int i = 0; i < actors.Length; i++)
            {
                var foundNode = SceneGraphFactory.FindNode(actors[i].ID);
                if (foundNode is ActorNode node)
                {
                    _nodes.Add(node);
                    Editor.Instance.Scene.MarkSceneEdited(node.ParentScene);
                }
            }
        }
    }
}
