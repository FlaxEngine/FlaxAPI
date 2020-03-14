// Copyright (c) 2012-2020 Wojciech Figat. All rights reserved.

using System;
using FlaxEditor.Content.Settings;
using FlaxEngine;

namespace FlaxEditor.Content.Create
{
    /// <summary>
    /// Engine settings asset creating handler. Allows to specify type of the settings to create (e.g. <see cref="GameSettings"/>, <see cref="TimeSettings"/>, etc.).
    /// </summary>
    /// <seealso cref="FlaxEditor.Content.Create.CreateFileEntry" />
    public class SettingsCreateEntry : CreateFileEntry
    {
        /// <summary>
        /// Types of the settings assets that can be created.
        /// </summary>
        public enum SettingsTypes
        {
            /// <summary>
            /// The game settings.
            /// </summary>
            GameSettings,

            /// <summary>
            /// The audio settings.
            /// </summary>
            AudioSettings,

            /// <summary>
            /// The time settings.
            /// </summary>
            TimeSettings,

            /// <summary>
            /// The layers and tags settings.
            /// </summary>
            LayersAndTagsSettings,

            /// <summary>
            /// The physics settings.
            /// </summary>
            PhysicsSettings,

            /// <summary>
            /// The graphics settings.
            /// </summary>
            GraphicsSettings,

            /// <summary>
            /// The navigation settings.
            /// </summary>
            NavigationSettings,

            /// <summary>
            /// The build settings.
            /// </summary>
            BuildSettings,

            /// <summary>
            /// The input settings.
            /// </summary>
            InputSettings,

            /// <summary>
            /// The Windows settings.
            /// </summary>
            WindowsPlatformSettings,

            /// <summary>
            /// The UWP settings.
            /// </summary>
            UWPPlatformSettings,

            /// <summary>
            /// The Linux settings.
            /// </summary>
            LinuxPlatformSettings,
        }

        private static readonly Type[] _types =
        {
            typeof(GameSettings),
            typeof(AudioSettings),
            typeof(TimeSettings),
            typeof(LayersAndTagsSettings),
            typeof(PhysicsSettings),
            typeof(FlaxEditor.Content.Settings.GraphicsSettings),
            typeof(NavigationSettings),
            typeof(BuildSettings),
            typeof(InputSettings),
            typeof(WindowsPlatformSettings),
            typeof(UWPPlatformSettings),
            typeof(LinuxPlatformSettings),
        };

        /// <summary>
        /// The create options.
        /// </summary>
        public class Options
        {
            /// <summary>
            /// The type.
            /// </summary>
            [Tooltip("Type of the settings asset to create")]
            public SettingsTypes Type = SettingsTypes.GameSettings;
        }

        private readonly Options _options = new Options();

        /// <inheritdoc />
        public override object Settings => _options;

        /// <summary>
        /// Initializes a new instance of the <see cref="SettingsCreateEntry"/> class.
        /// </summary>
        /// <param name="resultUrl">The result file url.</param>
        public SettingsCreateEntry(string resultUrl)
        : base("Settings", resultUrl)
        {
        }

        /// <inheritdoc />
        public override bool Create()
        {
            // Create settings asset object and serialize it to pure json asset
            var data = Activator.CreateInstance(_types[(int)_options.Type]);
            return Editor.SaveJsonAsset(ResultUrl, data);
        }
    }
}
