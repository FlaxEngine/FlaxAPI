// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

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
            /// The node evaluation context structure.
            /// </summary>
            [StructLayout(LayoutKind.Sequential)]
            public struct Context
            {
                /// <summary>
                /// The reserved data field.
                /// </summary>
                public IntPtr Reserved;

                /// <summary>
                /// The graph node identifier (unique per graph).
                /// </summary>
                public uint NodeId;

                /// <summary>
                /// The requested box identifier to evaluate its value.
                /// </summary>
                public int BoxId;
            }

            /// <summary>
            /// Loads the node data from the serialized values and prepares the node to run. In most cases this method is called from the content loading thread (not the main game thread).
            /// </summary>
            /// <param name="values">The node values array. The first item is always the typename of the custom node type, second one is node group name, others are customizable by editor node archetype.</param>
            public abstract void Load(object[] values);

            /// <summary>
            /// Evaluates the node based on inputs and node data.
            /// </summary>
            /// <param name="context">The evaluation context.</param>
            /// <returns>The node value for the given context (node values, output box id, etc.).</returns>
            public abstract object Evaluate(ref Context context);

            /// <summary>
            /// Check if th box of the given ID has valid connection to get its value.
            /// </summary>
            /// <param name="context">The context.</param>
            /// <param name="boxId">The input box identifier.</param>
            /// <returns>True if has connection, otherwise false..</returns>
            [MethodImpl(MethodImplOptions.InternalCall)]
            public static extern bool HasConnection(ref Context context, int boxId);

            /// <summary>
            /// Gets the value of the input box of the given ID. Throws the exception if box has no valid connection.
            /// </summary>
            /// <param name="context">The context.</param>
            /// <param name="boxId">The input box identifier.</param>
            /// <returns>The value.</returns>
            [MethodImpl(MethodImplOptions.InternalCall)]
            public static extern object GetInputValue(ref Context context, int boxId);
        }
    }
}
