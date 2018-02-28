////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2018 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using FlaxEditor.SceneGraph.Actors;
using FlaxEngine;

namespace FlaxEditor.SceneGraph
{
    /// <summary>
    /// Factory service for Scene Graph nodes creating.
    /// </summary>
    public static class SceneGraphFactory
    {
        /// <summary>
        /// The custom nodes types. Key = object type, Value = custom graph node type
        /// </summary>
        public static readonly Dictionary<Type, Type> CustomNodesTypes = new Dictionary<Type, Type>();

        /// <summary>
        /// The nodes collection where key is ID.
        /// </summary>
        public static readonly Dictionary<Guid, SceneGraphNode> Nodes = new Dictionary<Guid, SceneGraphNode>();

        /// <summary>
        /// Tries to find the node by the given ID.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Found node or null if cannot.</returns>
        public static SceneGraphNode FindNode(Guid id)
        {
            if (id == Guid.Empty)
                return null; 

            SceneGraphNode result;
            Nodes.TryGetValue(id, out result);
            return result;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SceneGraphFactory"/> class.
        /// </summary>
        static SceneGraphFactory()
        {
            // Init default nodes types
            CustomNodesTypes.Add(typeof(Camera), typeof(CameraNode));
            CustomNodesTypes.Add(typeof(EnvironmentProbe), typeof(EnvironmentProbeNode));
            CustomNodesTypes.Add(typeof(PointLight), typeof(PointLightNode));
            CustomNodesTypes.Add(typeof(DirectionalLight), typeof(DirectionalLightNode));
            CustomNodesTypes.Add(typeof(SpotLight), typeof(SpotLightNode));
            CustomNodesTypes.Add(typeof(Skybox), typeof(SkyboxNode));
            CustomNodesTypes.Add(typeof(Sky), typeof(SkyNode));
            CustomNodesTypes.Add(typeof(ExponentialHeightFog), typeof(ExponentialHeightFogNode));
            CustomNodesTypes.Add(typeof(SkyLight), typeof(SkyLightNode));
            CustomNodesTypes.Add(typeof(PostFxVolume), typeof(PostFxVolumeNode));
            CustomNodesTypes.Add(typeof(ModelActor), typeof(ModelActorNode));
            CustomNodesTypes.Add(typeof(BoxBrush), typeof(BoxBrushNode));
            CustomNodesTypes.Add(typeof(TextRender), typeof(TextRenderNode));
            CustomNodesTypes.Add(typeof(AudioListener), typeof(AudioListenerNode));
            CustomNodesTypes.Add(typeof(AudioSource), typeof(AudioSourceNode));
        }

        /// <summary>
        /// Builds the scene tree.
        /// </summary>
        /// <param name="scene">The scene.</param>
        /// <returns>The root scene node.</returns>
        public static SceneNode BuildSceneTree(Scene scene)
        {
            // TODO: make it faster by calling engine internaly only once to gather optimzied scene tree -> but do it in late stage of editor development - no early optimalization!

            var sceneNode = new SceneNode(scene);

            BuildSceneTree(sceneNode);

            // Unlock tree UI
            sceneNode.TreeNode.UnlockChildrenRecursive();

            return sceneNode;
        }

        /// <summary>
        /// Builds the actor node. Warning! Don't create duplicated nodes.
        /// </summary>
        /// <param name="actor">The actor.</param>
        /// <returns>Created node or null if failed.</returns>
        public static ActorNode BuildActorNode(Actor actor)
        {
            ActorNode result = null;

            try
            {
                // Try to pick custom node type for that actor object
                Type customType;
                if (CustomNodesTypes.TryGetValue(actor.GetType(), out customType))
                {
                    // Use custom type
                    result = (ActorNode)Activator.CreateInstance(customType, new object[] { actor });
                }
                else
                {
                    // Use default type for actors
                    result = new ActorNode(actor);
                }

                // Build children
                BuildSceneTree(result);
            }
            catch (Exception ex)
            {
                // Error
                Debug.LogWarning($"Failed to create scene graph node for actor {actor.Name} (type: {actor.GetType()}).");
                Debug.LogException(ex);
            }

            // Unlock tree UI
            result?.TreeNode.UnlockChildrenRecursive();

            return result;
        }

        private static void BuildSceneTree(ActorNode node)
        {
            var children = node.Actor.GetChildren();
            for (int i = 0; i < children.Length; i++)
            {
                var childNode = BuildActorNode(children[i]);
                if (childNode != null)
                    childNode.ParentNode = node;
            }
        }
    }
}
