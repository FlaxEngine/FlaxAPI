////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FlaxEditor.SceneGraph;
using FlaxEngine;
using FlaxEngine.GUI;

namespace FlaxEditor.GUI.Drag
{
    /// <summary>
    /// Helper class for handling actor type drag and drop (for spawning).
    /// </summary>
    /// <seealso cref="Actor" />
    /// <seealso cref="ActorNode" />
    public sealed class DragActorType : DragHelper<Type>
    {
        /// <summary>
        /// The default prefix for drag data used for actor type drag and drop.
        /// </summary>
        public const string DragPrefix = "ATYPE!?";

        /// <inheritdoc />
        protected override void GetherObjects(DragDataText data, Func<Type, bool> validateFunc)
        {
            var items = ParseData(data);
            for (int i = 0; i < items.Length; i++)
            {
                if (validateFunc(items[i]))
                    Objects.Add(items[i]);
            }
        }

        private static Assembly GetAssemblyByName(string name)
        {
            return AppDomain.CurrentDomain.GetAssemblies().SingleOrDefault(assembly => assembly.GetName().Name == name);
        }

        /// <summary>
        /// Tries to parse the drag data to extract <see cref="Type"/> collection.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns>Gathered objects or empty array if cannot get any valid.</returns>
        public static Type[] ParseData(DragDataText data)
        {
            if (data.Text.StartsWith(DragPrefix))
            {
                // Remove prefix and parse splited names
                var types = data.Text.Remove(0, DragPrefix.Length).Split('\n');
                var results = new List<Type>(types.Length);
                var assembly = GetAssemblyByName("FlaxEngine");
                if (assembly != null)
                {
                    for (int i = 0; i < types.Length; i++)
                    {
                        // Find type
                        var obj = assembly.GetType(types[i]);
                        if (obj != null)
                            results.Add(obj);
                    }

                    return results.ToArray();
                }
                else
                {
                    Editor.LogWarning("Failed to get FlaxEngine assembly to spawn actor type");
                }
            }

            return new Type[0];
        }

        /// <summary>
        /// Gets the drag data.
        /// </summary>
        /// <param name="actorType">The actor type.</param>
        /// <returns>The data.</returns>
        public static DragDataText GetDragData(Type actorType)
        {
            if (actorType == null)
                throw new ArgumentNullException();

            return new DragDataText(DragPrefix + actorType.FullName);
        }
    }
}
