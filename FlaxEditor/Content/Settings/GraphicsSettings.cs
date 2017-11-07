////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using FlaxEngine;

namespace FlaxEditor.Content.Settings
{
    /// <summary>
    /// The graphics rendering settings container. Allows to edit asset via editor.
    /// </summary>
    public sealed class GraphicsSettings : SettingsBase
    {
        /// <summary>
        /// Enables rendering synchronization with the refresh rate of the display device to avoid "tearing" artifacts.
        /// </summary>
        [EditorOrder(20), EditorDisplay("General"), Tooltip("Enables rendering synchronization with the refresh rate of the display device to avoid \"tearing\" artifacts.")]
        public bool UseVSync = false;
    }
}
