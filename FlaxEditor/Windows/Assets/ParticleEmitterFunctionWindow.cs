// Copyright (c) 2012-2020 Wojciech Figat. All rights reserved.

using FlaxEditor.Content;
using FlaxEditor.Surface;
using FlaxEngine;
using FlaxEngine.GUI;

namespace FlaxEditor.Windows.Assets
{
    /// <summary>
    /// Particle function window allows to view and edit <see cref="ParticleEmitterFunction"/> asset.
    /// </summary>
    /// <seealso cref="ParticleEmitterFunction" />
    /// <seealso cref="ParticleEmitterFunctionSurface" />
    public sealed class ParticleEmitterFunctionWindow : VisjectFunctionSurfaceWindow<ParticleEmitterFunction, ParticleEmitterFunctionSurface>
    {
        /// <inheritdoc />
        public ParticleEmitterFunctionWindow(Editor editor, AssetItem item)
        : base(editor, item)
        {
            // Surface
            _surface = new ParticleEmitterFunctionSurface(this, Save, _undo)
            {
                AnchorPreset = AnchorPresets.StretchAll,
                Offsets = new Margin(0, 0, _toolstrip.Bottom, 0),
                Parent = this,
                Enabled = false
            };

            // Toolstrip
            _toolstrip.AddSeparator();
            _toolstrip.AddButton(editor.Icons.Docs32, () => Platform.OpenUrl(Utilities.Constants.DocsUrl + "manual/particles/index.html")).LinkTooltip("See documentation to learn more");
        }

        /// <inheritdoc />
        public override string SurfaceName => "Particle Emitter Function";

        /// <inheritdoc />
        public override byte[] SurfaceData
        {
            get => _asset.LoadSurface();
            set
            {
                if (_asset.SaveSurface(value))
                {
                    _surface.MarkAsEdited();
                    Editor.LogError("Failed to save surface data");
                }
                _asset.Reload();
            }
        }

        /// <inheritdoc />
        protected override bool LoadSurface()
        {
            if (_surface.Load())
            {
                Editor.LogError("Failed to load surface.");
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
