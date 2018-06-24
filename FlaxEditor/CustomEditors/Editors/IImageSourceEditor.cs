// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using FlaxEngine;
using FlaxEngine.GUI;

namespace FlaxEditor.CustomEditors.Editors
{
    /// <summary>
    /// Default implementation of the inspector used to edit <see cref="IImageSource"/> type properties.
    /// </summary>
    /// <seealso cref="IImageSource"/>
    /// <seealso cref="ObjectSwitcherEditor"/>
    [CustomEditor(typeof(IImageSource)), DefaultEditor]
    public sealed class IImageSourceEditor : ObjectSwitcherEditor
    {
        /// <inheritdoc />
        protected override OptionType[] Options => new[]
        {
            new OptionType("Texture", typeof(TextureImageSource)),
            new OptionType("Sprite", typeof(SpriteImageSource)),
            new OptionType("Render Target", typeof(RenderTargetImageSource)),
        };
    }
}
