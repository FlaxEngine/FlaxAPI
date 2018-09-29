// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System;
using System.IO;
using FlaxEditor.Modules;
using FlaxEngine;

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

        private readonly string _optionsFilePath;

        internal OptionsModule(Editor editor)
        : base(editor)
        {
            // Always load options before the other modules setup
            InitOrder = -1000000;

            _optionsFilePath = Path.Combine(Editor.LocalCachePath, "EditorOptions.json");
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
