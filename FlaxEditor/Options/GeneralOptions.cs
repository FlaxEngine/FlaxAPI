// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System.ComponentModel;
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
        /// Gets or sets a limit for the editor undo actions. Higher values may increase memory usage but also improve changes rollback history length.
        /// </summary>
        [DefaultValue(500)]
        [EditorDisplay("General"), EditorOrder(100), Tooltip("Limit for the editor undo actions. Higher values may increase memory usage but also improve changes rollback history length.")]
        public int UndoActionsCapacity { get; set; } = 500;

        /// <summary>
        /// Gets or sets a value indicating whether perform automatic scripts reload on main window focus.
        /// </summary>
        [DefaultValue(true)]
        [EditorDisplay("Scripting", "Auto Reload Scripts On Main Window Focus"), EditorOrder(500), Tooltip("Determines whether reload scripts after a change on main window focus.")]
        public bool AutoReloadScriptsOnMainWindowFocus { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether perform automatic CSG rebuild on brush change.
        /// </summary>
        [DefaultValue(true)]
        [EditorDisplay("CSG", "Auto Rebuild CSG"), EditorOrder(600), Tooltip("Determines whether perform automatic CSG rebuild on brush change.")]
        public bool AutoRebuildCSG { get; set; } = true;

        /// <summary>
        /// Gets or sets the auto CSG rebuilding timeout (in milliseconds). Use lower value for more frequent and responsive updates but higher complexity.
        /// </summary>
        [DefaultValue(50.0f), Range(0, 500)]
        [EditorDisplay("CSG", "Auto Rebuild CSG Timeout"), EditorOrder(601), Tooltip("Auto CSG rebuilding timeout (in milliseconds). Use lower value for more frequent and responsive updates but higher complexity.")]
        public float AutoRebuildCSGTimeoutMs { get; set; } = 50.0f;

        /// <summary>
        /// Gets or sets a value indicating whether enable editor analytics service.
        /// </summary>
        [DefaultValue(true)]
        [EditorDisplay("Analytics"), EditorOrder(1000), Tooltip("Enables or disables anonymous editor analytics service used to improve editor experience and the quality")]
        public bool EnableEditorAnalytics { get; set; } = true;
    }
}
