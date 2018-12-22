// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System;
using System.Collections.Generic;
using System.Reflection;
using FlaxEditor.Surface;
using FlaxEngine;
using Utils = FlaxEditor.Utilities.Utils;

namespace FlaxEditor.Modules.SourceCodeEditing
{
    /// <summary>
    /// Cached collection of custom nodes types including cached node archetypes for each one of them.
    /// </summary>
    /// <seealso cref="FlaxEditor.Modules.SourceCodeEditing.CachedTypesCollection" />
    public class CachedCustomAnimGraphNodesCollection : CachedTypesCollection
    {
        private List<NodeArchetype> _archetypes;

        /// <summary>
        /// Gets a value indicating whether this instance has any type from game scripts (or editor scripts) - those can be reloaded at runtime so prevent crashes.
        /// </summary>
        public bool HasTypeFromGameScripts { get; protected set; }

        /// <inheritdoc />
        public CachedCustomAnimGraphNodesCollection(int capacity, Type type, Func<Type, bool> checkFunc, Func<Assembly, bool> checkAssembly)
        : base(capacity, type, checkFunc, checkAssembly)
        {
        }

        /// <summary>
        /// Gets the cached archetypes.
        /// </summary>
        /// <returns>The archetypes collection.</returns>
        public List<NodeArchetype> GetArchetypes()
        {
            Get();
            return _archetypes;
        }

        /// <inheritdoc />
        protected override void Search()
        {
            base.Search();

            HasTypeFromGameScripts = false;

            if (_list.Count == 0)
            {
                _archetypes?.Clear();
                return;
            }

            if (_archetypes == null)
                _archetypes = new List<NodeArchetype>(_list.Capacity);
            _archetypes.Clear();

            for (int i = _list.Count - 1; i >= 0 && _list.Count > 0; i--)
            {
                var nodeType = _list[i];
                var arch = GetArchetype(nodeType);
                if (arch != null)
                {
                    // Cache type
                    _archetypes.Add(arch);
                }
                else
                {
                    // Invalid type
                    _list.RemoveAt(i);
                    i--;
                    Editor.LogWarning(string.Format("Anim Graph custom node type {0} has missing or invalid CustomNodeArchetype attribute or invalid node archetype descriptor.", nodeType.FullName));
                }
            }
        }

        private NodeArchetype GetArchetype(Type nodeType)
        {
            // Peek attribute
            var attr = nodeType.GetCustomAttribute<AnimationGraph.CustomNodeArchetypeAttribute>();
            if (attr?.TypeName == null || attr.Method == null)
                return null;

            // Find type
            var getterType = Utils.GetType(attr.TypeName);
            if (getterType == null)
                return null;

            // Find method
            var getterMethod = getterType.GetMethod(attr.Method, BindingFlags.Static | BindingFlags.Public);
            if (getterMethod == null)
                return null;

            // Validate method signature
            if (!getterMethod.IsStatic || getterMethod.ReturnType != typeof(NodeArchetype) || getterMethod.GetParameters().Length != 0)
                return null;

            // Invoke method
            try
            {
                var arch = (NodeArchetype)getterMethod.Invoke(null, null);

                // Validate archetype
                if (arch.Tag != null || string.IsNullOrEmpty(arch.Title))
                    return null;
                if (arch.DefaultValues == null || arch.DefaultValues.Length < 2 || string.IsNullOrEmpty(arch.DefaultValues[0] as string) || string.IsNullOrEmpty(arch.DefaultValues[1] as string))
                    return null;

                // Check if type comes from scripts that can be reloaded at runtime
                HasTypeFromGameScripts |= Utils.IsTypeFromGameScripts(getterType) || Utils.IsTypeFromGameScripts(nodeType);

                return arch;
            }
            catch (Exception ex)
            {
                Editor.LogWarning("Failed to get the custom node archetype.");
                Editor.LogWarning(ex);
            }

            return null;
        }

        /// <inheritdoc />
        public override void ClearTypes()
        {
            base.ClearTypes();

            _archetypes?.Clear();
        }
    }
}
