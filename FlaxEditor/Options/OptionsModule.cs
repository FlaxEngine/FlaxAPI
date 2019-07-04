// Copyright (c) 2012-2019 Wojciech Figat. All rights reserved.

using System;
using System.Collections.Generic;
using System.IO;
using FlaxEditor.Modules;
using FlaxEngine;
using FlaxEngine.Json;

namespace FlaxEditor.Options
{
    /// <summary>
    /// Editor options management module.
    /// </summary>
    /// <seealso cref="FlaxEditor.Modules.EditorModule" />
    public sealed class OptionsModule : EditorModule
    {
        /// <summary>
        /// The current editor options. Don't modify the values directly (local session state lifetime), use <see cref="Apply"/>.
        /// </summary>
        public EditorOptions Options = new EditorOptions();

        /// <summary>
        /// Occurs when editor options get changed (reloaded or applied).
        /// </summary>
        public event Action<EditorOptions> OptionsChanged;

        /// <summary>
        /// Occurs when editor options get changed (reloaded or applied).
        /// </summary>
        public event Action CustomSettingsChanged;

        /// <summary>
        /// The custom settings factory delegate. It should return the default settings object for a given options contenxt.
        /// </summary>
        /// <returns>The custom settings object.</returns>
        public delegate object CreateCustomSettingsDelegate();

        private readonly string _optionsFilePath;
        private readonly Dictionary<string, CreateCustomSettingsDelegate> _customSettings = new Dictionary<string, CreateCustomSettingsDelegate>();

        /// <summary>
        /// Gets the custom settings factories. Each entry defines the custom settings type identified by teh given key name. The value si a factory function that returns the default options fpr a given type.
        /// </summary>
        public IReadOnlyDictionary<string, CreateCustomSettingsDelegate> CustomSettings => _customSettings;

        internal OptionsModule(Editor editor)
        : base(editor)
        {
            // Always load options before the other modules setup
            InitOrder = -1000000;

            _optionsFilePath = Path.Combine(Editor.LocalCachePath, "EditorOptions.json");
        }

        /// <summary>
        /// Adds the custom settings factory.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="factory">The factory function.</param>
        public void AddCustomSettings(string name, CreateCustomSettingsDelegate factory)
        {
            if (_customSettings.ContainsKey(name))
                throw new ArgumentException(string.Format("Custom settings \'{0}\' already added.", name), nameof(name));

            Editor.Log(string.Format("Add custom editor settings \'{0}\'", name));
            _customSettings.Add(name, factory);
            if (!Options.CustomSettings.ContainsKey(name))
            {
                Options.CustomSettings.Add(name, JsonSerializer.Serialize(factory(), typeof(object)));
            }
            CustomSettingsChanged?.Invoke();
        }

        /// <summary>
        /// Removes the custom settings factory.
        /// </summary>
        /// <param name="name">The name.</param>
        public void RemoveCustomSettings(string name)
        {
            if (!_customSettings.ContainsKey(name))
                throw new ArgumentException(string.Format("Custom settings \'{0}\' has not been added.", name), nameof(name));

            Editor.Log(string.Format("Remove custom editor settings \'{0}\'", name));
            _customSettings.Remove(name);
            CustomSettingsChanged?.Invoke();
        }

        /// <summary>
        /// Loads the settings from the file.
        /// </summary>
        public void Load()
        {
            Editor.Log("Loading editor options");

            try
            {
                // Load asset
                var asset = FlaxEngine.Content.LoadAsync<JsonAsset>(_optionsFilePath);
                if (asset == null)
                {
                    Editor.LogWarning("Missing or invalid editor settings");
                    return;
                }
                if (asset.WaitForLoaded())
                {
                    Editor.LogWarning("Failed to load editor settings");
                    return;
                }

                // Deserialize data
                var assetObj = asset.CreateInstance();
                if (assetObj is EditorOptions options)
                {
                    // Add missing custom options
                    foreach (var e in _customSettings)
                    {
                        if (!options.CustomSettings.ContainsKey(e.Key))
                            options.CustomSettings.Add(e.Key, JsonSerializer.Serialize(e.Value()));
                    }

                    Options = options;
                    OnOptionsChanged();
                }
                else
                {
                    Editor.LogWarning("Failed to deserialize editor settings");
                }
            }
            catch (Exception ex)
            {
                Editor.LogError("Failed to load editor options.");
                Editor.LogWarning(ex);
            }
        }

        /// <summary>
        /// Applies the specified options and updates the dependant services.
        /// </summary>
        /// <param name="options">The new options.</param>
        public void Apply(EditorOptions options)
        {
            Options = options;
            OnOptionsChanged();
            Save();
        }

        private void Save()
        {
            // Update file
            Editor.SaveJsonAsset(_optionsFilePath, Options);

            // Special case for editor analytics
            var editorAnalyticsTrackingFile = Path.Combine(Editor.LocalCachePath, "noTracking");
            if (Options.General.EnableEditorAnalytics)
            {
                if (!File.Exists(editorAnalyticsTrackingFile))
                {
                    File.WriteAllText(editorAnalyticsTrackingFile, "Don't track me, please.");
                }
            }
            else
            {
                if (File.Exists(editorAnalyticsTrackingFile))
                {
                    File.Delete(editorAnalyticsTrackingFile);
                }
            }
        }

        private void OnOptionsChanged()
        {
            Editor.Log("Editor options changed!");

#if !UNIT_TEST_COMPILANT
            // Sync C++ backend options
            Editor.InternalOptions internalOptions;
            internalOptions.AutoReloadScriptsOnMainWindowFocus = (byte)(Options.General.AutoReloadScriptsOnMainWindowFocus ? 1 : 0);
            internalOptions.AutoRebuildCSG = (byte)(Options.General.AutoRebuildCSG ? 1 : 0);
            internalOptions.AutoRebuildCSGTimeoutMs = Options.General.AutoRebuildCSGTimeoutMs;
            internalOptions.AutoRebuildNavMesh = (byte)(Options.General.AutoRebuildNavMesh ? 1 : 0);
            internalOptions.AutoRebuildNavMeshTimeoutMs = Options.General.AutoRebuildNavMeshTimeoutMs;
            Editor.Internal_SetOptions(ref internalOptions);
#endif

            // Send event
            OptionsChanged?.Invoke(Options);
        }

        /// <inheritdoc />
        public override void OnInit()
        {
            Editor.Log("Options file path: " + _optionsFilePath);
            Load();
        }
    }
}
