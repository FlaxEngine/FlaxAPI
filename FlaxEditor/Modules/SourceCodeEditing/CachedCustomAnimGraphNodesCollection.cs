// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System;
using System.Collections.Generic;
using System.Reflection;
using FlaxEditor.Surface;
using FlaxEngine;
using MethodInfo = System.Reflection.MethodInfo;
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

                var methods = nodeType.GetMethods(BindingFlags.Static | BindingFlags.Public);

                for (int j = 0; j < methods.Length; j++)
                {
                    var arch = GetArchetype(methods[j]);

                    if (arch != null)
                    {
                        // Cache type
                        _archetypes.Add(arch);
                    }
                }
            }
        }

        private NodeArchetype GetArchetype(MethodBase method)
        {
            // Validate method signature
            if (!method.IsStatic ||
                !method.IsPublic ||
                method as MethodInfo == null ||
                ((MethodInfo)method).ReturnType != typeof(NodeArchetype) ||
                method.GetParameters().Length != 0 ||
                method.IsGenericMethod)
                return null;

            // Invoke method
            try
            {
                var arch = (NodeArchetype)method.Invoke(null, null);

                // Validate archetype
                if (arch.Tag != null || string.IsNullOrEmpty(arch.Title))
                {
                    Editor.LogWarning(string.Format("Method {0} from {1} returned invalid node archetype. Tag must be null and title must be specified.", method, method.DeclaringType));
                    return null;
                }
                if (arch.DefaultValues == null || arch.DefaultValues.Length < 2 || string.IsNullOrEmpty(arch.DefaultValues[0] as string) || string.IsNullOrEmpty(arch.DefaultValues[1] as string))
                {
                    Editor.LogWarning(string.Format("Method {0} from {1} returned invalid node archetype. Default values are invalid.", method, method.DeclaringType));
                    return null;
                }

                // Check if type comes from scripts that can be reloaded at runtime
                HasTypeFromGameScripts |= Utils.IsTypeFromGameScripts(method.DeclaringType);

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
