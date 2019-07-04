// Copyright (c) 2012-2019 Wojciech Figat. All rights reserved.

using System.Collections.Generic;
using FlaxEngine.Json;

namespace FlaxEditor.Options
{
    /// <summary>
    /// Editor options data container object.
    /// </summary>
    public sealed class EditorOptions
    {
        /// <summary>
        /// The general options.
        /// </summary>
        public GeneralOptions General = new GeneralOptions();

        /// <summary>
        /// The interface options.
        /// </summary>
        public InterfaceOptions Interface = new InterfaceOptions();

        /// <summary>
        /// The visual options.
        /// </summary>
        public VisualOptions Visual = new VisualOptions();

        /// <summary>
        /// The source code options.
        /// </summary>
        public SourceCodeOptions SourceCode = new SourceCodeOptions();

        /// <summary>
        /// The custom settings collection. Can be used by the editor plugins to provide customization for options. Key is a settings tab name and the value is the serialized settings object (for a custom editor).
        /// </summary>
        public readonly Dictionary<string, string> CustomSettings = new Dictionary<string, string>();

        /// <summary>
        /// Tries to load the custom settings object by the given key. If settings are missing it creates a new default object of this type.
        /// </summary>
        /// <typeparam name="T">The settings object type.</typeparam>
        /// <param name="name">The name.</param>
        /// <returns>The loaded settings object</returns>
        public T GetCustomSettings<T>(string name) where T : class, new()
        {
            if (CustomSettings.TryGetValue(name, out var json))
            {
                return JsonSerializer.Deserialize<T>(json);
            }
            return new T();
        }
    }
}
