// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System;

namespace FlaxEngine
{
    public sealed partial class AnimationGraph
    {
        /// <summary>
        /// The custom attribute that allows to specify the class that contains node archetype getter methods.
        /// </summary>
        /// <seealso cref="System.Attribute" />
        [AttributeUsage(AttributeTargets.Class)]
        public class CustomNodeArchetypeFactoryAttribute : Attribute
        {
        }

        /// <summary>
        /// Base class for all custom nodes. Allows to override it and define own Anim Graph nodes in game scripts or via plugins.
        /// </summary>
        /// <remarks>See official documentation to learn more how to use and create custom nodes in Anim Graph.</remarks>
        public abstract class CustomNode
        {
            /// <summary>
            /// Loads the node data from the serialized values and prepares the node to run. In most cases this method is called from the content loading thread (not the main game thread).
            /// </summary>
            /// <param name="values">The node values array. The first item is always the typename of the custom node type, second one is node group name, others are customizable by editor node archetype.</param>
            public abstract void Load(object[] values);

            /// <summary>
            /// Evaluates the node based on inputs and node data.
            /// </summary>
            public abstract void Evaluate();
        }
    }
}
