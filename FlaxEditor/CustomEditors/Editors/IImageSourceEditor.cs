// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using FlaxEngine;
using FlaxEngine.GUI;

namespace FlaxEditor.CustomEditors.Editors
{
    /// <summary>
    /// Default implementation of the inspector used to edit <see cref="IBrush"/> type properties.
    /// </summary>
    /// <seealso cref="IBrush"/>
    /// <seealso cref="ObjectSwitcherEditor"/>
    [CustomEditor(typeof(IBrush)), DefaultEditor]
    public sealed class IImageSourceEditor : ObjectSwitcherEditor
    {
        /// <inheritdoc />
        protected override OptionType[] Options => new[]
        {
            new OptionType("Texture", typeof(TextureBrush)),
            new OptionType("Sprite", typeof(SpriteBrush)),
            new OptionType("Render Target", typeof(RenderTargetBrush)),
        };
    }
}
