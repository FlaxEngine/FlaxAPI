////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System.Collections.Generic;
using FlaxEngine;

namespace FlaxEditor.Content.Settings
{
    /// <summary>
    /// The layers and objects tags settings. Allows to edit asset via editor.
    /// </summary>
    public sealed class LayersAndTagsSettings : SettingsBase
    {
        /// <summary>
        /// The tag names.
        /// </summary>
        [EditorOrder(10), EditorDisplay("Tags", "__inline__")]
        public List<string> Tags = new List<string>();

        /// <summary>
        /// The layers names.
        /// </summary>
        [EditorOrder(10), EditorDisplay("Layers", "__inline__"), MemberCollection(ReadOnly = true)]
        public string[] Layers = new string[32];
    }
}
