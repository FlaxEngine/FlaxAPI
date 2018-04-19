// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System;
using FlaxEngine;

namespace FlaxEditor.Content.Settings
{
    /// <summary>
    /// The Universal Windows Platform (UWP) platform settings asset archetype. Allows to edit asset via editor.
    /// </summary>
    public class UWPPlatformSettings : SettingsBase
    {
        /// <summary>
        /// The preferred launch windowing mode.
        /// </summary>
        public enum WindowMode
        {
            /// <summary>
            /// The full screen mode
            /// </summary>
            FullScreen = 0,

            /// <summary>
            /// The view size.
            /// </summary>
            ViewSize = 1,
        }

        /// <summary>
        /// The display orientation modes. Can be combined as flags.
        /// </summary>
        [Flags]
        public enum DisplayOrientations
        {
            /// <summary>
            /// The none.
            /// </summary>
            None = 0,

            /// <summary>
            /// The landscape.
            /// </summary>
            Landscape = 1,

            /// <summary>
            /// The landscape flipped.
            /// </summary>
            LandscapeFlipped = 2,

            /// <summary>
            /// The portrait.
            /// </summary>
            Portrait = 4,

            /// <summary>
            /// The portrait flipped.
            /// </summary>
            PortraitFlipped = 8,
        }

        /// <summary>
        /// The preferred launch windowing mode. Always fullscreen on Xbox.
        /// </summary>
        [EditorOrder(10), EditorDisplay("Window"), Tooltip("The preferred launch windowing mode. Always fullscreen on Xbox.")]
        public WindowMode PreferredLaunchWindowingMode = WindowMode.FullScreen;

        /// <summary>
        /// The display orientation modes. Can be combined as flags.
        /// </summary>
        [EditorOrder(20), EditorDisplay("Window"), Tooltip("The display orientation modes. Can be combined as flags.")]
        public DisplayOrientations AutoRotationPreferences = DisplayOrientations.Landscape | DisplayOrientations.LandscapeFlipped | DisplayOrientations.Portrait | DisplayOrientations.PortraitFlipped;

        /// <summary>
        /// The location of the package certificate (relative to the project).
        /// </summary>
        [EditorOrder(1010), EditorDisplay("Other"), Tooltip("The location of the package certificate (relative to the project).")]
        public string CertificateLocation;
    }
}
