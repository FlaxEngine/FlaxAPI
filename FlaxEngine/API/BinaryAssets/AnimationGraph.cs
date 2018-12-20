// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System;

namespace FlaxEngine
{
    public sealed partial class AnimationGraph
    {
        /// <summary>
        /// The custom node attribute that allows to specify the editor node type getter node.
        /// </summary>
        /// <seealso cref="System.Attribute" />
        [AttributeUsage(AttributeTargets.Class)]
        public class CustomNodeArchetypeAttribute : Attribute
        {
            /// <summary>
            /// The full typename of the class that contains <see cref="Method"/> to call for custom node archetype gather.
            /// </summary>
            /// <remarks>This property is type of string because scripts or game plugins assembly don't reference editor scripts or editor plugin assemblies that are used to define editor types and editor tools.</remarks>
            public readonly string TypeName;

            /// <summary>
            /// The name of the function to use for accessing the node archetype (it mist be parameter-less, return type FlaxEditor.Surface.NodeArchetype and static).
            /// </summary>
            /// <remarks>This property is type of string because scripts or game plugins assembly don't reference editor scripts or editor plugin assemblies that are used to define editor types and editor tools.</remarks>
            public readonly string Method;

            /// <summary>
            /// Initializes a new instance of the <see cref="CustomNodeArchetypeAttribute"/> class.
            /// </summary>
            /// <param name="typename">The the class that contains a node archetype get function.</param>
            /// <param name="method">The the editor UI node archetype descriptor getter method. It must be a static parameter-less method that returns FlaxEditor.Surface.NodeArchetype type defined in FlaxEditor assembly.</param>
            public CustomNodeArchetypeAttribute(string typename, string method)
            {
                TypeName = typename;
                Method = method;
            }
        }

        /// <summary>
        /// Base class for all custom nodes. Allows to override it and define own Anim Graph nodes in game scripts or via plugins. Ensure to use <see cref="CustomNodeArchetypeAttribute"/> in order to allow using this node logic in editor.
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
