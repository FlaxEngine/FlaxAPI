// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System.Collections.Generic;
using FlaxEditor.SceneGraph;
using FlaxEngine;

namespace FlaxEditor.Actions
{
    /// <summary>
    /// Implementation of <see cref="IUndoAction"/> used to delete a selection of <see cref="ActorNode"/>.
    /// </summary>
    /// <seealso cref="FlaxEditor.IUndoAction" />
    public sealed class DeleteActorsAction : IUndoAction
    {
        private List<ActorNode> _nodeParents;
        private byte[] _data;
        private bool _isInverted;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteActorsAction"/> class.
        /// </summary>
        /// <param name="objects">The objects.</param>
        /// <param name="isInverted">If set to <c>true</c> action will be inverted - instead of delete it will be create actors.</param>
        internal DeleteActorsAction(List<SceneGraphNode> objects, bool isInverted = false)
        {
            _isInverted = isInverted;
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
        public string ActionString => _isInverted ? "Create actors" : "Delete actors";

        /// <inheritdoc />
        public void Do()
        {
            if (_isInverted)
                Create();
            else
                Delete();
        }

        /// <inheritdoc />
        public void Undo()
        {
            if (_isInverted)
                Delete();
            else
                Create();
        }

        private void Delete()
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

        private void Create()
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
