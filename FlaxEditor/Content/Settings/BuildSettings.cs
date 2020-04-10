// Copyright (c) 2012-2020 Wojciech Figat. All rights reserved.

using System;
using System.ComponentModel;
using FlaxEngine;

namespace FlaxEditor.Content.Settings
{
    /// <summary>
    /// The game building settings container. Allows to edit asset via editor.
    /// </summary>
    public sealed class BuildSettings : SettingsBase
    {
        /// <summary>
        /// The maximum amount of assets to include into a single assets package. Assets will be split into several packages if need to.
        /// </summary>
        [DefaultValue(256)]
        [EditorOrder(10), Limit(32, short.MaxValue), EditorDisplay("General", "Max assets per package"), Tooltip("The maximum amount of assets to include into a single assets package. Assets will be split into several packages if need to.")]
        public int MaxAssetsPerPackage = 1024;

        /// <summary>
        /// The maximum size of the single assets package (in megabytes). Assets will be split into several packages if need to.
        /// </summary>
        [DefaultValue(256)]
        [EditorOrder(20), Limit(16, short.MaxValue), EditorDisplay("General", "Max package size (in MB)"), Tooltip("The maximum size of the single assets package (in megabytes). Assets will be split into several packages if need to.")]
        public int MaxPackageSizeMB = 1024;

        /// <summary>
        /// The game content cooking Keys. Use the same value for a game and DLC packages to support loading them by the build game. Use 0 to randomize it during building.
        /// </summary>
        [DefaultValue(0)]
        [EditorOrder(30), EditorDisplay("General"), Tooltip("The game content cooking Keys. Use the same value for a game and DLC packages to support loading them by the build game. Use 0 to randomize it during building.")]
        public int ContentKey = 0;

        /// <summary>
        /// Disables shaders compiler optimizations in cooked game. Can be used to debug shaders on a target platform or to speed up the shaders compilation time.
        /// </summary>
        [DefaultValue(false)]
        [EditorOrder(100), EditorDisplay("Content", "Shaders No Optimize"), Tooltip("Disables shaders compiler optimizations in cooked game. Can be used to debug shaders on a target platform or to speed up the shaders compilation time.")]
        public bool ShadersNoOptimize;

        /// <summary>
        /// Enables shader debug data generation for shaders in cooked game (depends on the target platform rendering backend).
        /// </summary>
        [DefaultValue(false)]
        [EditorOrder(110), EditorDisplay("Content"), Tooltip("Enables shader debug data generation for shaders in cooked game (depends on the target platform rendering backend).")]
        public bool ShadersGenerateDebugData;

        /// <summary>
        /// The build presets.
        /// </summary>
        [EditorOrder(1000), EditorDisplay("Presets", EditorDisplayAttribute.InlineStyle), Tooltip("Build presets configuration")]
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
                        Mode = BuildConfiguration.Development,
                    },
                    new BuildTarget
                    {
                        Name = "Windows 32bit",
                        Output = "Output\\Win32",
                        Platform = BuildPlatform.Windows32,
                        Mode = BuildConfiguration.Development,
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
                        Mode = BuildConfiguration.Development,
                    },
                    new BuildTarget
                    {
                        Name = "Windows 32bit",
                        Output = "Output\\Win32",
                        Platform = BuildPlatform.Windows32,
                        Mode = BuildConfiguration.Development,
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
