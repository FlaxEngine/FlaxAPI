////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using FlaxEditor.CustomEditors.Editors;
using FlaxEngine;
using FlaxEngine.Rendering;

namespace FlaxEditor.CustomEditors.Dedicated
{
    /// <summary>
    /// <see cref="PostProcessSettings"/> editor.
    /// </summary>
    [CustomEditor(typeof(PostProcessSettings))]
    public sealed class PostProcessSettingsEditor : GenericEditor
    {
        /// <inheritdoc />
        public override DisplayStyle Style => DisplayStyle.InlineIntoParent;

        /// <inheritdoc />
        protected override List<ItemInfo> GetItemsForType(Type type)
        {
            return base.GetItemsForType(type, true, false);
        }
    }
}
