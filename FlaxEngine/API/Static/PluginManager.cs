// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System;
using System.Collections.Generic;
using System.Reflection;

namespace FlaxEngine
{
    public static partial class PluginManager
    {
        private static readonly List<GamePlugin> _gamePlugins = new List<GamePlugin>();
        private static readonly List<Plugin> _editorPlugins = new List<Plugin>();

        /// <summary>
        /// Gets the loaded and enabled game plugins.
        /// </summary>
        public static IReadOnlyList<GamePlugin> GamePlugins => _gamePlugins;

        /// <summary>
        /// Gets the loaded and enabled editor plugins.
        /// </summary>
        public static IReadOnlyList<Plugin> EditorPlugins => _editorPlugins;

        /// <summary>
        /// Determines whether can load the specified plugin.
        /// </summary>
        /// <param name="pluginDesc">The plugin description.</param>
        /// <returns>True if load it, otherwise false.</returns>
        public delegate bool CanLoadPluginDelegate(ref PluginDescription pluginDesc);

        /// <summary>
        /// Determines whether can load the specified plugin.
        /// </summary>
        public static CanLoadPluginDelegate CanLoadPlugin = DefaultCanLoadPlugin;

        /// <summary>
        /// The default implementation for <see cref="CanLoadPlugin"/>.
        /// </summary>
        /// <param name="pluginDesc">The plugin description.</param>
        /// <returns>True if load it, otherwise false.</returns>
        public static bool DefaultCanLoadPlugin(ref PluginDescription pluginDesc)
        {
            return !pluginDesc.DisabledByDefault;
        }

        private static void InvokeInitialize(Plugin plugin)
        {
            try
            {
                Debug.Write(LogType.Log, "Loading plugin " + plugin);
                plugin.Initialize();
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
                Debug.LogErrorFormat("Failed to initialize plugin {0}. {1}", plugin, ex.Message);
            }
        }

        private static void InvokeDeinitialize(Plugin plugin)
        {
            try
            {
                Debug.Write(LogType.Log, "Unloading plugin " + plugin);
                plugin.Deinitialize();
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
                Debug.LogErrorFormat("Failed to deinitialize plugin {0}. {1}", plugin, ex.Message);
            }
        }

        internal static void Internal_LoadPlugin(Type type, bool isEditor)
        {
            if (type == null)
                throw new ArgumentNullException();

            // Create and check if use it
            var plugin = (Plugin)Activator.CreateInstance(type);
            var desc = plugin.Description;
            if (!CanLoadPlugin(ref desc))
            {
                Debug.Write(LogType.Log, "Skip loading plugin " + plugin);
                return;
            }

            // Init
            InvokeInitialize(plugin);

            // Register
            if (isEditor)
                _editorPlugins.Add(plugin);
            else
                _gamePlugins.Add((GamePlugin)plugin);
        }

        internal static void Internal_Dispose(Assembly assembly)
        {
            for (int i = _editorPlugins.Count - 1; i >= 0 && _editorPlugins.Count > 0; i--)
            {
                if (_editorPlugins[i].GetType().Assembly == assembly)
                {
                    InvokeDeinitialize(_editorPlugins[i]);
                    _editorPlugins.RemoveAt(i);
                }
            }

            for (int i = _gamePlugins.Count - 1; i >= 0 && _gamePlugins.Count > 0; i--)
            {
                if (_gamePlugins[i].GetType().Assembly == assembly)
                {
                    InvokeDeinitialize(_gamePlugins[i]);
                    _gamePlugins.RemoveAt(i);
                }
            }
        }

        internal static void Internal_Dispose()
        {
            int pluginsCount = _editorPlugins.Count + _gamePlugins.Count;
            if (pluginsCount == 0)
                return;
            Debug.Write(LogType.Log, string.Format("Unloading {0} plugins", pluginsCount));

            for (int i = _editorPlugins.Count - 1; i >= 0 && _editorPlugins.Count > 0; i--)
            {
                InvokeDeinitialize(_editorPlugins[i]);
                _editorPlugins.RemoveAt(i);
            }

            for (int i = _gamePlugins.Count - 1; i >= 0 && _gamePlugins.Count > 0; i--)
            {
                InvokeDeinitialize(_gamePlugins[i]);
                _gamePlugins.RemoveAt(i);
            }
        }
    }
}
