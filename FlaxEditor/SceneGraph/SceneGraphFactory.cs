////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
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
    public class SceneGraphFactory
    {
        /// <summary>
        /// The custom nodes types. Key = object type, Value = custom graph node type
        /// </summary>
        public readonly Dictionary<Type, Type> CustomNodesTypes = new Dictionary<Type, Type>();

        /// <summary>
        /// Initializes a new instance of the <see cref="SceneGraphFactory"/> class.
        /// </summary>
        public SceneGraphFactory()
        {
            // Init default nodes types
            CustomNodesTypes.Add(typeof(Camera), typeof(CameraNode));
            CustomNodesTypes.Add(typeof(EnvironmentProbe), typeof(EnvironmentProbeNode));
            CustomNodesTypes.Add(typeof(PointLight), typeof(PointLightNode));
            CustomNodesTypes.Add(typeof(DirectionalLight), typeof(DirectionalLightNode));
            CustomNodesTypes.Add(typeof(SpotLight), typeof(SpotLightNode));
            CustomNodesTypes.Add(typeof(Skybox), typeof(SkyboxNode));
            CustomNodesTypes.Add(typeof(Sky), typeof(SkyNode));
        }

        /// <summary>
        /// Builds the scene tree.
        /// </summary>
        /// <param name="scene">The scene.</param>
        /// <returns>The root scene node.</returns>
        public SceneNode BuildSceneTree(Scene scene)
        {
            // TODO: make it faster by calling engine internaly only once to gather optimzied scene tree -> but do it in late stage of editor development - no early optimalization!

            var sceneNode = new SceneNode(scene);

            BuildSceneTree(sceneNode);

            return sceneNode;
        }

        private void BuildSceneTree(ActorNode node)
        {
            var children = node.Actor.GetChildren();
            for (int i = 0; i < children.Length; i++)
            {
                var actor = children[i];

                try
                {
                    ActorNode childNode;

                    // Try to pick custom node type for that actor object
                    Type customType;
                    if (CustomNodesTypes.TryGetValue(actor.GetType(), out customType))
                    {
                        // Use custom type
                        childNode = (ActorNode) Activator.CreateInstance(customType, new object[] {actor});
                    }
                    else
                    {
                        // Use default type for actors
                        childNode = new ActorNode(actor);
                    }

                    childNode.ParentNode = node;
                    BuildSceneTree(childNode);
                }
                catch (Exception ex)
                {
                    // Error
                    Debug.LogWarning($"Failed to create scene graph node for actor {actor.Name} (type: {actor.GetType()}).");
                    Debug.LogException(ex);
                }
            }
        }
    }
}
