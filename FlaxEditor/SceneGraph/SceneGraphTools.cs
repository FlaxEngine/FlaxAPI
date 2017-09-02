////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;

namespace FlaxEditor.SceneGraph
{
    /// <summary>
    /// Set of tools for <see cref="SceneGraphNode"/> processing.
    /// </summary>
    public static class SceneGraphTools
    {
        /// <summary>
        /// Delegate for scene graph action exeuction callback.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <returns>True if call deeper, otherwise skip calling node children.</returns>
        public delegate bool GraphExecuteCallbackDelegate(SceneGraphNode node);

        /// <summary>
        /// Executes the custom action on the graph nodes.
        /// </summary>
        /// <param name="node">The node</param>
        /// <param name="callback">The callback.</param>
        public static void ExecuteOnGraph(this SceneGraphNode node, GraphExecuteCallbackDelegate callback)
        {
            for (int i = 0; i < node.ChildNodes.Count; i++)
            {
                if (callback(node.ChildNodes[i]))
                {
                    ExecuteOnGraph(node.ChildNodes[i], callback);
                }
            }
        }

        /// <summary>
        /// Builds the array made of input nodes that are at the top level. The result collection contains only nodes that don't have parent nodes in the given collection.
        /// </summary>
        /// <typeparam name="T">The scene graph node type.</typeparam>
        /// <param name="nodes">The nodes.</param>
        /// <returns>The result.</returns>
        public static T[] BuildNodesParents<T>(this List<T> nodes)
            where T : SceneGraphNode
        {
            var list = new List<T>();
            BuildNodesParents(nodes, list);
            return list.ToArray();
        }

        /// <summary>
        /// Builds the list made of input nodes that are at the top level. The result collection contains only nodes that don't have parent nodes in the given collection.
        /// </summary>
        /// <typeparam name="T">The scene graph node type.</typeparam>
        /// <param name="nodes">The nodes.</param>
        /// <param name="result">The result.</param>
        public static void BuildNodesParents<T>(this List<T> nodes, List<T> result)
            where T : SceneGraphNode
        {
            if (nodes == null || result == null)
                throw new ArgumentNullException();

            result.Clear();

            for (var i = 0; i < nodes.Count; i++)
            {
                var target = nodes[i];

                // Check if any other object in selection is parent object of this one
                bool isChild = false;
                for (var j = 0; j < nodes.Count; j++)
                {
                    var test = nodes[j];
                    if (i != j && test.ContainsInHierarchy(target))
                    {
                        isChild = true;
                        break;
                    }
                }

                if (!isChild)
                    result.Add(target);
            }
        }

        /// <summary>
        /// Builds the array made of all nodes in the input list and child tree. The result collection contains all nodes in the tree.
        /// </summary>
        /// <param name="nodes">The nodes.</param>
        /// <returns>The result.</returns>
        public static SceneGraphNode[] BuildAllNodes(this List<SceneGraphNode> nodes)
        {
            var list = new List<SceneGraphNode>();
            BuildAllNodes(nodes, list);
            return list.ToArray();
        }

        private static void FillTree(SceneGraphNode node, List<SceneGraphNode> result)
        {
            result.AddRange(node.ChildNodes);
            for (int i = 0; i < node.ChildNodes.Count; i++)
            {
                FillTree(node.ChildNodes[i], result);
            }
        }

        /// <summary>
        /// Builds the list made of all nodes in the input list and child tree. The result collection contains all nodes in the tree.
        /// </summary>
        /// <param name="nodes">The nodes.</param>
        /// <param name="result">The result.</param>
        public static void BuildAllNodes(this List<SceneGraphNode> nodes, List<SceneGraphNode> result)
        {
            if (nodes == null || result == null)
                throw new ArgumentNullException();

            result.Clear();

            for (var i = 0; i < nodes.Count; i++)
            {
                var target = nodes[i];

                // Check if has been already added
                if (result.Contains(target))
                    continue;

                // Add whole child tree to the results
                result.Add(target);
                FillTree(target, result);
            }
        }
    }
}
