// Copyright (c) 2012-2019 Wojciech Figat. All rights reserved.

using System;
using System.IO;
using FlaxEngine;

namespace FlaxEditor
{
    /// <summary>
    /// Helper utilities for the plugins management in Editor.
    /// </summary>
    public static class PluginUtils
    {
        /// <summary>
        /// Tries the get plugin icon (plugin may not have it).
        /// </summary>
        /// <param name="plugin">The plugin.</param>
        /// <returns>The found texture asset to be used as a icon, or null if missing or invalid.</returns>
        public static Texture TryGetPluginIcon(Plugin plugin)
        {
            if (plugin == null)
                throw new ArgumentNullException();

            var type = plugin.GetType();
            var assembly = type.Assembly;
            var assemblyPath = assembly.Location;
            var assemblyName = assembly.GetName().Name;
            var dotEditorPos = assemblyName.LastIndexOf(".Editor", StringComparison.OrdinalIgnoreCase);
            if (dotEditorPos != -1)
                assemblyName = assemblyName.Substring(dotEditorPos);

            var iconPath = Path.Combine(Path.GetDirectoryName(assemblyPath), assemblyName + ".Icon.flax");

            if (!File.Exists(iconPath))
                return null;

            return FlaxEngine.Content.LoadAsync<Texture>(iconPath);
        }

        private static int CountSubTypes(Type[] types, Type type)
        {
            int result = 0;
            for (int i = 0; i < types.Length; i++)
            {
                if (types[i].IsSubclassOf(type))
                    result++;
            }
            return result;
        }

        private static Type FirstOfSubType(Type[] types, Type type)
        {
            for (int i = 0; i < types.Length; i++)
            {
                if (types[i].IsSubclassOf(type))
                    return types[i];
            }
            return null;
        }

        /// <summary>
        /// Gets the plugin to export (editor or game or both). Searches the game scripts assemblies only. Performs validation.
        /// </summary>
        /// <param name="gamePlugin">The game plugin.</param>
        /// <param name="editorPlugin">The editor plugin.</param>
        /// <param name="errorMsg">If searching fails, then it contains a result message with error information. Can be used to inform user about the actual problem.</param>
        /// <returns>True if found plugin is valid, otherwise false.</returns>
        public static bool GetPluginToExport(out GamePlugin gamePlugin, out EditorPlugin editorPlugin, out string errorMsg)
        {
            // Init
            gamePlugin = null;
            editorPlugin = null;

            // Cache data
            var allAssemblies = AppDomain.CurrentDomain.GetAssemblies();
            var gameAssembly = Utils.GetAssemblyByName("Game", allAssemblies);
            var gameEditorAssembly = Utils.GetAssemblyByName("Game.Editor", allAssemblies);
            var gameAssemblyTypes = gameAssembly.GetTypes();
            var gameEditorAssemblyTypes = gameEditorAssembly.GetTypes();

            // Get those plugins
            var gamePluginType = FirstOfSubType(gameAssemblyTypes, typeof(GamePlugin));
            var editorPluginType = FirstOfSubType(gameEditorAssemblyTypes, typeof(EditorPlugin));
            if (gamePluginType == null && editorPluginType == null)
            {
                errorMsg = "Cannot find any plugins in game scripts to export.";
                return false;
            }

            // Count plugins (assembly can contain only one plugin)
            if (CountSubTypes(gameAssemblyTypes, typeof(GamePlugin)) > 1)
            {
                errorMsg = "Game assembly can contain only one GamePlugin implementation to export it.";
                return false;
            }
            if (CountSubTypes(gameAssemblyTypes, typeof(EditorPlugin)) != 0)
            {
                errorMsg = "Game assembly cannot contain any EditorPlugin implementation";
                return false;
            }
            if (CountSubTypes(gameEditorAssemblyTypes, typeof(EditorPlugin)) > 1)
            {
                errorMsg = "Editor assembly can contain only one EditorPlugin implementation to export it.";
                return false;
            }
            if (CountSubTypes(gameEditorAssemblyTypes, typeof(GamePlugin)) != 0)
            {
                errorMsg = "Editor assembly cannot contain any GamePlugin implementation";
                return false;
            }

            // Create objects
            try
            {
                if (gamePluginType != null)
                {
                    gamePlugin = (GamePlugin)Activator.CreateInstance(gamePluginType);
                }
                if (editorPluginType != null)
                {
                    editorPlugin = (EditorPlugin)Activator.CreateInstance(editorPluginType);
                }
            }
            catch (Exception ex)
            {
                Editor.LogWarning(ex);
                errorMsg = "Failed to create plugin objects. See log to learn more.";
                return false;
            }

            // Validate relation
            if (gamePlugin != null && editorPlugin != null)
            {
                if (editorPlugin.GamePluginType != gamePluginType)
                {
                    errorMsg = "Cannot export game and editor plugins because editor plugin is not specifying game plugin type with GamePluginType property.";
                    return false;
                }
            }

            errorMsg = null;
            return true;
        }
    }
}
