// Copyright (c) 2012-2020 Wojciech Figat. All rights reserved.

using System.ComponentModel;
using FlaxEngine;

namespace FlaxEditor.Content.Settings
{
    /// <summary>
    /// The PlayStation 4 platform settings asset archetype. Allows to edit asset via editor.
    /// </summary>
    public class PS4PlatformSettings : SettingsBase
    {
        /// <summary>
        /// Application icon texture (asset id).
        /// </summary>
        [DefaultValue(null)]
        [EditorOrder(100), EditorDisplay("General"), Tooltip("Application icon texture (asset id).")]
        public Texture Icon;

        /// <summary>
        /// Background image texture (asset id).
        /// </summary>
        [DefaultValue(null)]
        [EditorOrder(110), EditorDisplay("General"), Tooltip(" Background image texture (asset id).")]
        public Texture BackgroundImage;
    }
}
