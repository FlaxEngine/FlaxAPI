// Copyright (c) 2012-2020 Wojciech Figat. All rights reserved.

using FlaxEditor.Content;
using FlaxEditor.Surface;
using FlaxEngine;
using FlaxEngine.GUI;

namespace FlaxEditor.Windows.Assets
{
    /// <summary>
    /// Material function window allows to view and edit <see cref="MaterialFunction"/> asset.
    /// </summary>
    /// <seealso cref="MaterialFunction" />
    /// <seealso cref="MaterialFunctionSurface" />
    public sealed class MaterialFunctionWindow : VisjectFunctionSurfaceWindow<MaterialFunction, MaterialFunctionSurface>
    {
        /// <inheritdoc />
        public MaterialFunctionWindow(Editor editor, AssetItem item)
        : base(editor, item)
        {
            // Surface
            _surface = new MaterialFunctionSurface(this, Save, _undo)
            {
                AnchorPreset = AnchorPresets.StretchAll,
                Offsets = new Margin(0, 0, _toolstrip.Bottom, 0),
                Parent = this,
                Enabled = false
            };

            // Toolstrip
            _toolstrip.AddSeparator();
            _toolstrip.AddButton(editor.Icons.Docs32, () => Platform.OpenUrl(Utilities.Constants.DocsUrl + "manual/graphics/materials/index.html")).LinkTooltip("See documentation to learn more");
        }

        /// <inheritdoc />
        public override string SurfaceName => "Material Function";

        /// <inheritdoc />
        public override byte[] SurfaceData
        {
            get => _asset.LoadSurface();
            set
            {
                if (_asset.SaveSurface(value))
                {
                    _surface.MarkAsEdited();
                    Editor.LogError("Failed to save material surface data");
                }
                _asset.Reload();
            }
        }

        /// <inheritdoc />
        protected override bool LoadSurface()
        {
            if (_surface.Load())
            {
                Editor.LogError("Failed to load material surface.");
                return true;
            }
            return false;
        }

        /// <inheritdoc />
        protected override bool SaveSurface()
        {
            _surface.Save();
            return false;
        }
    }
}
