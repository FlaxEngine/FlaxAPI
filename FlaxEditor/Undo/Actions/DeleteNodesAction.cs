////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using FlaxEditor.SceneGraph;
using FlaxEngine.Assertions;

namespace FlaxEditor.Actions
{
    /// <summary>
    /// Implementation of <see cref="IUndoAction"/> used to delete a selection of <see cref="SceneGraphNode"/>.
    /// </summary>
    /// <seealso cref="FlaxEditor.IUndoAction" />
    public sealed class DeleteNodesAction : IUndoAction
    {
        /// <summary>
        /// Represents single entry of the node being removed
        /// </summary>
        private class Entry
        {
            private SceneGraphNode.DeserializeHandler Deserializer;
            private SceneGraphNode Node;
            private int OrderInParent;
            private byte[] Data;

            /// <summary>
            /// Initializes a new instance of the <see cref="Entry"/> class.
            /// </summary>
            /// <param name="node">The target node.</param>
            public Entry(SceneGraphNode node)
            {
                Deserializer = node.Deserializer;
                Node = node;
                OrderInParent = node.OrderInParent;
                Data = node.Serialize();
            }

            /// <summary>
            /// Deletes this entry node.
            /// </summary>
            public void Delete()
            {
                Assert.IsNotNull(Node);

                Editor.Instance.Scene.MarkSceneEdited(Node.ParentScene);

                Node.Delete();
                Node = null;
            }

            /// <summary>
            /// Restores this entry node.
            /// </summary>
            public void Restore()
            {
                Assert.IsNull(Node);

                Node = Deserializer(Data);
                if(Node == null)
                    throw new Exception("Failed to deserialize scene graph node.");

                Editor.Instance.Scene.MarkSceneEdited(Node.ParentScene);
            }
            
            /// <summary>
            /// Updates the entry order in parent.
            /// </summary>
            public void UpdateOrder()
            {
                Assert.IsNotNull(Node);
                Node.OrderInParent = OrderInParent;
            }
        }

        private Entry[] _entries;

        internal DeleteNodesAction(List<SceneGraphNode> objects)
        {
            _entries = new Entry[objects.Count];
            for (int i = 0; i < objects.Count; i++)
            {
                _entries[i] = new Entry(objects[i]);
            }
        }

        /// <inheritdoc />
        public string ActionString => "Delete object(s)";

        /// <inheritdoc />
        public void Do()
        {
            // Remove objects
            for (int i = 0; i < _entries.Length; i++)
            {
                _entries[i].Delete();
            }
        }

        /// <inheritdoc />
        public void Undo()
        {
            // Restore objects
            for (int i = 0; i < _entries.Length; i++)
            {
                _entries[i].Restore();
            }

            // Fix order in parent
            for (int i = 0; i < _entries.Length; i++)
            {
                _entries[i].UpdateOrder();
            }
        }
    }
}
