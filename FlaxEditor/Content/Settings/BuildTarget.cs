// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System;
using FlaxEngine;

namespace FlaxEditor.Content.Settings
{
    /// <summary>
    /// A <see cref="GameCooker"/> game building target with configuration properties.
    /// </summary>
    [Serializable]
    public class BuildTarget
    {
        /// <summary>
        /// The name of the target.
        /// </summary>
        [EditorOrder(10), Tooltip("Name of the target")]
        public string Name;

        /// <summary>
        /// The output folder path.
        /// </summary>
        [EditorOrder(20), Tooltip("Output folder path")]
        public string Output;

        /// <summary>
        /// The target platform.
        /// </summary>
        [EditorOrder(30), Tooltip("Target platform")]
        public BuildPlatform Platform;

        /// <summary>
        /// The configuration mode.
        /// </summary>
        [EditorOrder(30), Tooltip("Configuration build mode")]
        public BuildMode Mode;

        /// <summary>
        /// The pre-build action command line.
        /// </summary>
        [EditorOrder(100)]
        public string PreBuildAction;

        /// <summary>
        /// The post-build action command line.
        /// </summary>
        [EditorOrder(110)]
        public string PostBuildAction;

        /// <summary>
        /// The defines.
        /// </summary>
        [EditorOrder(1000), Tooltip("Custom macros")]
        public string[] Defines;

        /// <summary>
        /// Gets the build options computed from the target configuration.
        /// </summary>
        [HideInEditor, NoSerialize]
        public virtual BuildOptions Options
        {
            get
            {
                BuildOptions options = BuildOptions.None;
                if (Mode == BuildMode.Debug)
                    options |= BuildOptions.Debug;
                return options;
            }
        }
    }
}
