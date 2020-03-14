// Copyright (c) 2012-2020 Wojciech Figat. All rights reserved.

using System;
using FlaxEngine;

namespace FlaxEditor.SceneGraph.Actors
{
    /// <summary>
    /// Scene tree node for <see cref="AnimatedModel"/> actor type.
    /// </summary>
    /// <seealso cref="ActorNode" />
    public sealed class AnimatedModelNode : ActorNode
    {
        /// <summary>
        /// Single model material slot entry node.
        /// </summary>
        /// <seealso cref="FlaxEditor.SceneGraph.ActorChildNode{T}" />
        public sealed class EntryNode : ActorChildNode<AnimatedModelNode>
        {
            /// <summary>
            /// Gets the animated model actor.
            /// </summary>
            public AnimatedModel Model => (AnimatedModel)_actor.Actor;

            /// <summary>
            /// Gets the entry.
            /// </summary>
            public ModelEntryInfo Entry => ((AnimatedModel)_actor.Actor).Entries[Index];

            /// <inheritdoc />
            public EntryNode(AnimatedModelNode actor, Guid id, int index)
            : base(actor, id, index)
            {
            }

            /// <inheritdoc />
            public override Transform Transform
            {
                get => _actor.Transform;
                set => _actor.Transform = value;
            }

            /// <inheritdoc />
            public override object EditableObject => Entry;

            /// <inheritdoc />
            public override bool RayCastSelf(ref RayCastData ray, out float distance, out Vector3 normal)
            {
                return Entry.Intersects(ray.Ray, out distance, out normal);
            }
        }

        /// <inheritdoc />
        public AnimatedModelNode(Actor actor)
        : base(actor)
        {
            var modelActor = (AnimatedModel)actor;
            modelActor.EntriesChanged += BuildNodes;
            BuildNodes(modelActor);
        }

        private void BuildNodes(AnimatedModel actor)
        {
            // Clear previous
            DisposeChildNodes();

            // Build new collection
            var entries = actor.Entries;
            if (entries != null)
            {
                ChildNodes.Capacity = Mathf.Max(ChildNodes.Capacity, entries.Length);

                var id = ID;
                var idBytes = id.ToByteArray();
                for (int i = 0; i < entries.Length; i++)
                {
                    idBytes[0] += 1;
                    AddChildNode(new EntryNode(this, new Guid(idBytes), i));
                }
            }
        }

        /// <inheritdoc />
        public override bool RayCastSelf(ref RayCastData ray, out float distance, out Vector3 normal)
        {
            // Disable this node hit - MeshNodes handles ray casting
            distance = 0;
            normal = Vector3.Up;
            return false;
        }

        /// <inheritdoc />
        public override void OnDispose()
        {
            var modelActor = (AnimatedModel)Actor;
            modelActor.EntriesChanged -= BuildNodes;

            base.OnDispose();
        }
    }
}
