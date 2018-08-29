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

        internal static void Dispose(Assembly assembly)
        {
            Debug.Write(LogType.Log, assembly.FullName + ", " + assembly.Location);
        }

        internal static void Dispose()
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
