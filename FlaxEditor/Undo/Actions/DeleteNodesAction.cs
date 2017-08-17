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
        private List<ActorNode> _nodeParents;
        private byte[] _data;

        internal DeleteNodesAction(List<SceneGraphNode> objects)
        {
            _nodeParents = new List<ActorNode>(objects.Count);
            var actorNodes = new List<ActorNode>(objects.Count);
            var actors = new List<Actor>(objects.Count);
            for (int i = 0; i < objects.Count; i++)
            {
                if (objects[i] is ActorNode node)
                {
                    actorNodes.Add(node);
                    actors.Add(node.Actor);
                }
            }
            actorNodes.BuildNodesParents(_nodeParents);

            _data = Actor.ToBytes(actors.ToArray());
        }

        /// <inheritdoc />
        public string ActionString => "Delete object(s)";

        /// <inheritdoc />
        public void Do()
        {
            // Remove objects
            for (int i = 0; i < _nodeParents.Count; i++)
            {
                var node = _nodeParents[i];
                Editor.Instance.Scene.MarkSceneEdited(node.ParentScene);
                node.Delete();
            }
            _nodeParents.Clear();
        }

        /// <inheritdoc />
        public void Undo()
        {
            // Restore objects
            var actors = Actor.FromBytes(_data);
            if (actors == null)
                return;
            var actorNodes = new List<ActorNode>(actors.Length);
            for (int i = 0; i < actors.Length; i++)
            {
                var foundNode = SceneGraphFactory.FindNode(actors[i].ID);
                if (foundNode is ActorNode node)
                {
                    actorNodes.Add(node);
                }
            }
            actorNodes.BuildNodesParents(_nodeParents);
            for (int i = 0; i < _nodeParents.Count; i++)
            {
                Editor.Instance.Scene.MarkSceneEdited(_nodeParents[i].ParentScene);
            }
        }
    }
}
