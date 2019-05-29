// Copyright (c) 2012-2019 Wojciech Figat. All rights reserved.

using FlaxEngine;

namespace FlaxEditor.Content.Settings
{
    /// <summary>
    /// The Linux platform settings asset archetype. Allows to edit asset via editor.
    /// </summary>
    public class LinuxPlatformSettings : SettingsBase
    {
        /// <summary>
        /// Use fullscreen mode as default.
        /// </summary>
        [EditorOrder(10), EditorDisplay("Window"), Tooltip("Use fullscreen mode as default.")]
        public bool FullscreenMode = false;

        /// <summary>
        /// The default game window width (in pixels).
        /// </summary>
        [EditorOrder(20), EditorDisplay("Window"), Tooltip("The default game window width (in pixels).")]
        public int ScreenWidth = 1280;

        /// <summary>
        /// The default game window height (in pixels).
        /// </summary>
        [EditorOrder(30), EditorDisplay("Window"), Tooltip("The default game window height (in pixels).")]
        public int ScreenHeight = 720;

        /// <summary>
        /// Enables resizing the game window by the user.
        /// </summary>
        [EditorOrder(40), EditorDisplay("Window"), Tooltip("Enables resizing the game window by the user.")]
        public bool ResizableWindow = false;

        /// <summary>
        /// Enables game running when application window loses focus.
        /// </summary>
        [EditorOrder(1010), EditorDisplay("Other", "Run In Background"), Tooltip("Enables game running when application window loses focus.")]
        public bool RunInBackground = false;

        /// <summary>
        /// Limits maximum amount of concurrent game instances running to one, otherwise user may launch application more than once.
        /// </summary>
        [EditorOrder(1020), EditorDisplay("Other"), Tooltip("Limits maximum amount of concurrent game instances running to one, otherwise user may launch application more than once.")]
        public bool ForceSingleInstance = false;
    }
}
