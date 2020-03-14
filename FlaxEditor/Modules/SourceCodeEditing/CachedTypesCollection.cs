// Copyright (c) 2012-2020 Wojciech Figat. All rights reserved.

using System;
using System.Collections.Generic;
using System.Reflection;
using Utils = FlaxEditor.Utilities.Utils;

namespace FlaxEditor.Modules.SourceCodeEditing
{
    /// <summary>
    /// Cached types collection container.
    /// </summary>
    public class CachedTypesCollection
    {
        private bool _hasValidData;
        private readonly int _capacity;
        private readonly Func<Type, bool> _checkFunc;
        private readonly Func<Assembly, bool> _checkAssembly;

        /// <summary>
        /// The type.
        /// </summary>
        protected readonly Type _type;

        /// <summary>
        /// The list.
        /// </summary>
        protected List<Type> _list;

        /// <summary>
        /// Initializes a new instance of the <see cref="CachedTypesCollection"/> class.
        /// </summary>
        /// <param name="capacity">The initial collection capacity.</param>
        /// <param name="type">The type of things to find. It can be attribute to find all classes with the given attribute defined.</param>
        /// <param name="checkFunc">Additional callback used to check if the given type is valid. Returns true if add type, otherwise false.</param>
        /// <param name="checkAssembly">Additional callback used to check if the given assembly is valid. Returns true if search for types in the given assembly, otherwise false.</param>
        public CachedTypesCollection(int capacity, Type type, Func<Type, bool> checkFunc, Func<Assembly, bool> checkAssembly)
        {
            _capacity = capacity;
            _type = type;
            _checkFunc = checkFunc;
            _checkAssembly = checkAssembly;
        }

        /// <summary>
        /// Gets all the types from the all loaded assemblies (including project scripts and scripts from the plugins).
        /// </summary>
        /// <returns>The types collection (readonly).</returns>
        public List<Type> Get()
        {
            if (!_hasValidData)
            {
                if (_list == null)
                    _list = new List<Type>(_capacity);
                _list.Clear();
                _hasValidData = true;

                Editor.Log("Searching for valid " + _type);
                var start = DateTime.Now;

                Search();

                var end = DateTime.Now;
                Editor.Log(string.Format("Found {0} types (in {1} ms)", _list.Count, (int)(end - start).TotalMilliseconds));
            }

            return _list;
        }

        /// <summary>
        /// Searches for the types and fills with data.
        /// </summary>
        protected virtual void Search()
        {
            // Special case for attributes
            if (_type.BaseType == typeof(Attribute))
            {
                Utils.GetTypesWithAttributeDefined(_type, _list, _checkFunc, _checkAssembly);
            }
            else
            {
                Utils.GetDerivedTypes(_type, _list, _checkFunc, _checkAssembly);
            }
        }

        /// <summary>
        /// Clears the types.
        /// </summary>
        public virtual void ClearTypes()
        {
            _list?.Clear();
            _hasValidData = false;
        }
    }
}
