////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System;
using FlaxEngine;

namespace FlaxEditor.SceneGraph.Actors
{
    /// <summary>
    /// Scene tree node for <see cref="ModelActor"/> actor type.
    /// </summary>
    /// <seealso cref="ActorNode" />
    public sealed class ModelActorNode : ActorNode
    {
        /// <summary>
        /// Single model mesh entry node.
        /// </summary>
        /// <seealso cref="FlaxEditor.SceneGraph.ActorChildNode{T}" />
        public sealed class MeshNode : ActorChildNode<ModelActorNode>
        {
            /// <summary>
            /// Gets the model actor.
            /// </summary>
            public ModelActor ModelActor => (ModelActor)_actor.Actor;

            /// <summary>
            /// Gets the mesh entry.
            /// </summary>
            public ModelEntryInfo Entry => ((ModelActor)_actor.Actor).Entries[Index];

            /// <inheritdoc />
            public MeshNode(ModelActorNode actor, Guid id, int index)
                : base(actor, id, index)
            {
            }

            /// <inheritdoc />
            public override Transform Transform
            {
                get => _actor.Actor.Transform.LocalToWorld(Entry.Transform);
                set => Entry.Transform = _actor.Actor.Transform.WorldToLocal(value);
            }

            /// <inheritdoc />
            public override object EditableObject => Entry;

            /// <inheritdoc />
            public override bool RayCastSelf(ref Ray ray, out float distance)
            {
                return Entry.Intersects(ray, out distance);
            }
        }

        /// <inheritdoc />
        public ModelActorNode(Actor actor)
            : base(actor)
        {
            var modelActor = (ModelActor)actor;
            modelActor.EntriesChanged += BuildNodes;
            BuildNodes(modelActor);
        }

        private void BuildNodes(ModelActor actor)
        {
            // Clear previous
            DisposeChildNodes();

            // Build new collection
            var entries = actor.Entries;
            if (entries != null)
            {
                ChildNodes.Capacity = entries.Length;

                var id = ID;
                var idBytes = id.ToByteArray();
                for (int i = 0; i < entries.Length; i++)
                {
                    idBytes[0] += 1;
                    AddChildNode(new MeshNode(this, new Guid(idBytes), i));
                }
            }
        }

        /// <inheritdoc />
        public override bool RayCastSelf(ref Ray ray, out float distance)
        {
            // Disable this node hit - MeshNodes handles ray casting
            distance = 0;
            return false;
        }
    }
}
