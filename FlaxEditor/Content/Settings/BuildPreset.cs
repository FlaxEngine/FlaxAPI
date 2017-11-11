////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using FlaxEngine;

namespace FlaxEditor.Content.Settings
{
    /// <summary>
    /// A <see cref="GameCooker"/> game building preset with set of build targets.
    /// </summary>
    public class BuildPreset
    {
        /// <summary>
        /// The name of the preset.
        /// </summary>
        [EditorOrder(10), Tooltip("Name of the preset")]
        public string Name;

        /// <summary>
        /// The target configurations.
        /// </summary>
        [EditorOrder(20), Tooltip("Target configurations")]
        public BuildTarget[] Targets;
    }
}
