////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using FlaxEditor.CustomEditors;
using FlaxEngine;

namespace FlaxEditor.Content.Settings
{
    /// <summary>
    /// The game settings asset archetype. Allows to edit asset via editor.
    /// </summary>
    public sealed class GameSettings : SettingsBase
    {
        /// <summary>
        /// Reference to <see cref="TimeSettings"/> asset.
        /// </summary>
        [EditorOrder(1010), EditorDisplay("Other Settings"), AssetReference(typeof(TimeSettings), true), Tooltip("Reference to Time Settings asset")]
        public JsonAsset Time;
    }
}
