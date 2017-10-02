////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Linq;
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
        private Dictionary<Guid, Guid> _idsMapping;
        private List<ActorNode> _nodeParents;
        private byte[] _data;
        private Guid _pasteParent;

        private PasteActorsAction(byte[] data, Guid[] objectIds, ref Guid pasteParent, bool isDuplicate)
        {
            ActionString = isDuplicate ? "Duplicate actors" : "Paste actors";

            _pasteParent = pasteParent;
            _idsMapping = new Dictionary<Guid, Guid>(objectIds.Length * 4);
            for (int i = 0; i < objectIds.Length; i++)
            {
                _idsMapping[objectIds[i]] = Guid.NewGuid();
            }

            _nodeParents = new List<ActorNode>();
            _data = data;
        }

        internal static PasteActorsAction Paste(byte[] data, Guid pasteParent)
        {
            var objectIds = Actor.TryGetSerializedObjectsIds(data);
            if (objectIds == null)
                return null;

            return new PasteActorsAction(data, objectIds, ref pasteParent, false);
        }

        internal static PasteActorsAction Duplicate(byte[] data, Guid pasteParent)
        {
            var objectIds = Actor.TryGetSerializedObjectsIds(data);
            if (objectIds == null)
                return null;

            return new PasteActorsAction(data, objectIds, ref pasteParent, true);
        }

        /// <inheritdoc />
        public string ActionString { get; }

        /// <inheritdoc />
        public void Do()
        {
            // Restore objects
            var actors = Actor.FromBytes(_data, _idsMapping);
            if (actors == null)
                return;
            var actorNodes = new List<ActorNode>(actors.Length);
            Scene[] scenes = null;
            for (int i = 0; i < actors.Length; i++)
            {
                var actor = actors[i];

                // Check if has no parent linked (broken reference eg. old parent not existing)
                if (actor.Parent == null)
                {
                    // Link to the first scene root
                    if (scenes == null)
                        scenes = SceneManager.Scenes;
                    actor.SetParent(scenes[0], false);
                }

                var foundNode = SceneGraphFactory.FindNode(actor.ID);
                if (foundNode is ActorNode node)
                {
                    actorNodes.Add(node);
                }
            }

            actorNodes.BuildNodesParents(_nodeParents);

            var pasteParentNode = Editor.Instance.Scene.GetActorNode(_pasteParent);
            if (pasteParentNode != null)
            {
                // Move pasted actors to the parent target (if specified and valid)
                for (int i = 0; i < _nodeParents.Count; i++)
                {
                    _nodeParents[i].Actor.SetParent(pasteParentNode.Actor, false);
                }
            }

            for (int i = 0; i < _nodeParents.Count; i++)
            {
                // Fix name collisions (only for parents)
                var node = _nodeParents[i];
                var parent = node.Actor?.Parent;
                if (parent != null)
                {
                    string name = node.Name;
                    Actor[] children = parent.GetChildren();
                    if (children.Count(x => x.Name == name) > 1)
                    {
                        // Generate new name
                        node.Actor.Name = StringUtils.IncrementNameNumber(name, x => children.All(y => y.Name != x));
                    }
                }

                Editor.Instance.Scene.MarkSceneEdited(node.ParentScene);
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
