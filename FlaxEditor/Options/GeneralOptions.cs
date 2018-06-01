// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using FlaxEngine;

namespace FlaxEditor.Options
{
    /// <summary>
    /// General editor options data container.
    /// </summary>
    [CustomEditor(typeof(Editor<GeneralOptions>))]
    public sealed class GeneralOptions
    {
        /// <summary>
        /// Gets or sets a value indicating whether perform automatic scripts reload on main window focus.
        /// </summary>
        [EditorDisplay("Scripting", "Auto Reload Scripts On Main Window Focus"), EditorOrder(500), Tooltip("Determines whether reload scripts after a change on main window focus.")]
        public bool AutoReloadScriptsOnMainWindowFocus { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether perform automatic CSG rebuild on brush change.
        /// </summary>
        [EditorDisplay("CSG", "Auto Rebuild CSG"), EditorOrder(600), Tooltip("Determines whether perform automatic CSG rebuild on brush change.")]
        public bool AutoRebuildCSG { get; set; } = true;

        /// <summary>
        /// Gets or sets the auto CSG rebuilding timeout (in milliseconds). Use lower value for more frequent and responsive updates but higher complexity.
        /// </summary>
        [EditorDisplay("CSG", "Auto Rebuild CSG Timeout"), Range(0, 500), EditorOrder(601), Tooltip("Auto CSG rebuilding timeout (in milliseconds). Use lower value for more frequent and responsive updates but higher complexity.")]
        public float AutoRebuildCSGTimeoutMs { get; set; } = 50;

        /// <summary>
        /// Gets or sets a value indicating whether enable editor analytics service.
        /// </summary>
        [EditorDisplay("Analytics"), EditorOrder(1000), Tooltip("Enables or disables anonymous editor analytics service used to improve editor experience and the quality")]
        public bool EnableEditorAnalytics { get; set; } = true;
    }
}
