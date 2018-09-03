// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System;
using System.Collections.Generic;
using FlaxEditor.SceneGraph;
using FlaxEngine;
using FlaxEngine.GUI;

namespace FlaxEditor.GUI.Drag
{
    /// <summary>
    /// Helper class for handling <see cref="ActorNode"/> drag and drop.
    /// </summary>
    /// <seealso cref="Actor" />
    /// <seealso cref="ActorNode" />
    public sealed class DragActors : DragHelper<ActorNode>
    {
        /// <summary>
        /// The default prefix for drag data used for <see cref="ActorNode"/>.
        /// </summary>
        public const string DragPrefix = "ACTOR!?";

        /// <inheritdoc />
        protected override void GatherObjects(DragDataText data, Func<ActorNode, bool> validateFunc)
        {
            var items = ParseData(data);
            for (int i = 0; i < items.Length; i++)
            {
                if (validateFunc(items[i]))
                    Objects.Add(items[i]);
            }
        }

        /// <summary>
        /// Tries to parse the drag data to extract <see cref="ActorNode"/> collection.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns>Gathered objects or empty array if cannot get any valid.</returns>
        public static ActorNode[] ParseData(DragDataText data)
        {
            if (data.Text.StartsWith(DragPrefix))
            {
                // Remove prefix and parse spitted names
                var ids = data.Text.Remove(0, DragPrefix.Length).Split('\n');
                var results = new List<ActorNode>(ids.Length);
                for (int i = 0; i < ids.Length; i++)
                {
                    // Find element
                    Guid id;
                    if (Guid.TryParse(ids[i], out id))
                    {
                        var obj = Editor.Instance.Scene.GetActorNode(id);

                        // Check it
                        if (obj != null)
                            results.Add(obj);
                    }
                }

                return results.ToArray();
            }

            return new ActorNode[0];
        }

        /// <summary>
        /// Gets the drag data.
        /// </summary>
        /// <param name="actor">The actor.</param>
        /// <returns>The data.</returns>
        public static DragDataText GetDragData(Actor actor)
        {
            if (actor == null)
                throw new ArgumentNullException();

            return new DragDataText(DragPrefix + actor.ID.ToString("N"));
        }

        /// <summary>
        /// Gets the drag data.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>The data.</returns>
        public static DragDataText GetDragData(ActorNode item)
        {
            if (item == null)
                throw new ArgumentNullException();

            return new DragDataText(DragPrefix + item.ID.ToString("N"));
        }

        /// <summary>
        /// Gets the drag data.
        /// </summary>
        /// <param name="items">The items.</param>
        /// <returns>The data.</returns>
        public static DragDataText GetDragData(IEnumerable<ActorNode> items)
        {
            if (items == null)
                throw new ArgumentNullException();

            string text = DragPrefix;
            foreach (var item in items)
                text += item.ID.ToString("N") + '\n';
            return new DragDataText(text);
        }
    }
}
