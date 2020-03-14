// Copyright (c) 2012-2020 Wojciech Figat. All rights reserved.

namespace FlaxEditor.CustomEditors.Editors
{
    /// <summary>
    /// The hack editor for PostFxMaterials collection in PostProcessSettings that synchronizes the collection value with the instance. 
    /// </summary>
    /// <seealso cref="FlaxEditor.CustomEditors.Editors.ArrayEditor" />
    internal class PostFxMaterials : ArrayEditor
    {
        /// <inheritdoc />
        protected override bool NeedsValuePropagationUp => true;
    }
}
