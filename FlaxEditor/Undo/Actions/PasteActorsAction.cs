////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System.Collections.Generic;
using FlaxEditor.SceneGraph;
using FlaxEngine;

namespace FlaxEditor.Actions
{
    /// <summary>
    /// Implementation of <see cref="IUndoAction"/> used to paste a set of <see cref="ActorNode"/>.
    /// </summary>
    /// <seealso cref="FlaxEditor.IUndoAction" />
    public sealed class PasteActorsAction : IUndoAction
    {
        private List<ActorNode> _nodeParents;
        private byte[] _data;

        internal PasteActorsAction(byte[] data, bool isDuplicate)
        {
            ActionString = isDuplicate ? "Duplicate actors" : "Paste actors";

            _nodeParents = new List<ActorNode>();
            _data = data;
        }

        /// <inheritdoc />
        public string ActionString { get; }

        /// <inheritdoc />
        public void Do()
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

        /// <inheritdoc />
        public void Undo()
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
    }
}
