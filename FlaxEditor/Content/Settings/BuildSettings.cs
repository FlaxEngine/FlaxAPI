////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2018 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System;
using FlaxEngine;

namespace FlaxEditor.Content.Settings
{
    /// <summary>
    /// The game building settings container. Allows to edit asset via editor.
    /// </summary>
    public sealed class BuildSettings : SettingsBase
    {
        /// <summary>
        /// The maximum amount of assets to include into a single assets package. Assets will be spli into several packages if need to.
        /// </summary>
        [EditorOrder(10), Limit(32, short.MaxValue), EditorDisplay("General", "Max assets per package"), Tooltip("The maximum amount of assets to include into a single assets package. Assets will be spli into several packages if need to.")]
        public int MaxAssetsPerPackage = 256;

        /// <summary>
        /// The maximum size of the single assets package (in megabytes). Assets will be spli into several packages if need to.
        /// </summary>
        [EditorOrder(20), Limit(16, short.MaxValue), EditorDisplay("General", "Max package size (in MB)"), Tooltip("The maximum size of the single assets package (in megabytes). Assets will be spli into several packages if need to.")]
        public int MaxPackageSizeMB = 256;

        /// <summary>
        /// The game content cooking Keys. Use the same value for a game and DLC packages to support loading them by the builded game. Use 0 to randomize it during building.
        /// </summary>
        [EditorOrder(30), EditorDisplay("General"), Tooltip("The game content cooking Keys. Use the same value for a game and DLC packages to support loading them by the builded game. Use 0 to randomize it during building.")]
        public int ContentKey = 0;

        /// <summary>
        /// The build presets.
        /// </summary>
        [EditorOrder(100), EditorDisplay("Presets", EditorDisplayAttribute.InlineStyle), Tooltip("Build presets configuration")]
        public BuildPreset[] Presets =
        {
            new BuildPreset
            {
                Name = "Development",
                Targets = new[]
                {
                    new BuildTarget
                    {
                        Name = "Windows 64bit",
                        Output = "Output\\Win64",
                        Platform = BuildPlatform.Windows64,
                        Mode = BuildMode.Debug,
                    },
                    new BuildTarget
                    {
                        Name = "Windows 32bit",
                        Output = "Output\\Win32",
                        Platform = BuildPlatform.Windows32,
                        Mode = BuildMode.Debug,
                    },
                }
            },
            new BuildPreset
            {
                Name = "Release",
                Targets = new[]
                {
                    new BuildTarget
                    {
                        Name = "Windows 64bit",
                        Output = "Output\\Win64",
                        Platform = BuildPlatform.Windows64,
                        Mode = BuildMode.Release,
                    },
                    new BuildTarget
                    {
                        Name = "Windows 32bit",
                        Output = "Output\\Win32",
                        Platform = BuildPlatform.Windows32,
                        Mode = BuildMode.Release,
                    },
                }
            },
        };

        /// <summary>
        /// Gets the preset of the given name (ignore case search) or returns null if cannot find it.
        /// </summary>
        /// <param name="name">The preset name.</param>
        /// <returns>Found preset or null if is missing.</returns>
        public BuildPreset GetPreset(string name)
        {
            if (Presets != null)
            {
                for (int i = 0; i < Presets.Length; i++)
                {
                    if (string.Equals(Presets[i].Name, name, StringComparison.OrdinalIgnoreCase))
                        return Presets[i];
                }
            }
            return null;
        }
    }
}
