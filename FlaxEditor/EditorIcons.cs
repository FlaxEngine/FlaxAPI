// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System.Reflection;
using FlaxEngine;

#pragma warning disable 1591

namespace FlaxEditor
{
    /// <summary>
    /// The editor icons atlas. Cached internally to improve performance.
    /// </summary>
    /// <remarks>
    /// Postfix number informs about the sprite resolution (in pixels).
    /// </remarks>
    public sealed class EditorIcons
    {
        public Sprite FolderClosed12;
        public Sprite FolderOpened12;
        public Sprite DragBar12;
        public Sprite ArrowDown12;
        public Sprite ArrowRight12;
        public Sprite Search12;
        public Sprite Settings12;
        public Sprite Cross12;
        public Sprite CheckBoxIntermediate12;
        public Sprite CheckBoxTick12;
        public Sprite StatusBarSizeGrip12;

        public Sprite ArrowRightBorder16;
        public Sprite World16;
        public Sprite ScaleStep16;
        public Sprite RotateStep16;
        public Sprite Grid16;
        public Sprite Translate16;
        public Sprite Rotate16;
        public Sprite Scale16;
        public Sprite Link16;
        public Sprite Docs16;

        public Sprite Save32;
        public Sprite Undo32;
        public Sprite Redo32;
        public Sprite Translate32;
        public Sprite Rotate32;
        public Sprite Scale32;
        public Sprite Play32;
        public Sprite Pause32;
        public Sprite Step32;
        public Sprite Stop32;
        public Sprite PageScale32;
        public Sprite Bone32;
        public Sprite Docs32;
        public Sprite Import32;
        public Sprite AddDoc32;
        public Sprite RemoveDoc32;
        public Sprite BracketsSlash32;
        public Sprite Find32;
        public Sprite Reload32;
        public Sprite ArrowLeft32;
        public Sprite ArrowRight32;
        public Sprite ArrowUp32;
        public Sprite Error32;
        public Sprite Warning32;
        public Sprite Info32;
        public Sprite UV32;
        public Sprite Image32;
        public Sprite Link32;

        public Sprite Add48;
        public Sprite Paint48;
        public Sprite Foliage48;
        public Sprite Mountain48;

        public Sprite Plugin64;
        public Sprite Document64;
        public Sprite Script64;
        public Sprite Folder64;
        public Sprite Scene64;

        public Sprite Logo128;

        public Sprite VisjectBoxOpen;
        public Sprite VisjectBoxClose;
        public Sprite VisjectArrowOpen;
        public Sprite VisjectArrowClose;

        public Sprite AssetShadow;
        public Sprite ColorWheel;
        public Sprite Windows;
        public Sprite XboxOne;
        public Sprite WindowsStore;

        internal void GetIcons()
        {
            // Load asset
            var iconsAtlas = FlaxEngine.Content.LoadInternal<SpriteAtlas>(EditorAssets.IconsAtlas);
            if (iconsAtlas == null)
            {
                Editor.LogError("Cannot load editor icons atlas.");
                return;
            }
            if (iconsAtlas.WaitForLoaded())
            {
                Editor.LogError("Failed to load editor icons atlas.");
                return;
            }

            // Find icons
            var fields = GetType().GetFields(BindingFlags.Instance | BindingFlags.Public);
            for (int i = 0; i < fields.Length; i++)
            {
                var field = fields[i];
                var sprite = iconsAtlas.GetSprite(field.Name);
                if (!sprite.IsValid)
                {
                    Editor.LogWarning(string.Format("Failed to load sprite icon \'{0}\'.", field.Name));
                }
                field.SetValue(this, sprite);
            }
        }
    }
}
