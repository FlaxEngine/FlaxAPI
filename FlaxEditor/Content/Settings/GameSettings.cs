////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System;
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
        [EditorOrder(10), Tooltip("Reference to Time Settings asset")]
        public Guid Time;
    }
}
