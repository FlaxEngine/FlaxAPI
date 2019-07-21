// Copyright (c) 2012-2019 Wojciech Figat. All rights reserved.

using System;
using System.Collections.Generic;
using FlaxEditor.Content;
using FlaxEditor.Surface;
using FlaxEditor.Viewport.Previews;
using FlaxEngine;

// ReSharper disable UnusedMember.Local
// ReSharper disable UnusedMember.Global
// ReSharper disable MemberCanBePrivate.Local

namespace FlaxEditor.Windows.Assets
{
    /// <summary>
    /// Particle Emitter window allows to view and edit <see cref="ParticleEmitter"/> asset.
    /// </summary>
    /// <seealso cref="ParticleEmitter" />
    /// <seealso cref="ParticleEmitterSurface" />
    /// <seealso cref="ParticleEmitterPreview" />
    public sealed class ParticleEmitterWindow : VisjectSurfaceWindow<ParticleEmitter, ParticleEmitterSurface, ParticleEmitterPreview>
    {
        private enum NewParameterType
        {
            Float = ParameterType.Float,
            Bool = ParameterType.Bool,
            Integer = ParameterType.Integer,
            Vector2 = ParameterType.Vector2,
            Vector3 = ParameterType.Vector3,
            Vector4 = ParameterType.Vector4,
            Color = ParameterType.Color,
            Texture = ParameterType.Texture,
            RenderTarget = ParameterType.RenderTarget,
            RenderTargetArray = ParameterType.RenderTargetArray,
            RenderTargetVolume = ParameterType.RenderTargetVolume,
            Matrix = ParameterType.Matrix,
        }

        /// <summary>
        /// The properties proxy object.
        /// </summary>
        private sealed class PropertiesProxy
        {
            [EditorOrder(1000), EditorDisplay("Parameters"), CustomEditor(typeof(ParametersEditor)), NoSerialize]
            // ReSharper disable once UnusedAutoPropertyAccessor.Local
            public ParticleEmitterWindow Window { get; set; }

            [HideInEditor, Serialize]
            // ReSharper disable once UnusedMember.Local
            public List<SurfaceParameter> Parameters
            {
                get => Window.Surface.Parameters;
                set => throw new Exception("No setter.");
            }

            /// <summary>
            /// Gathers parameters from the specified ParticleEmitter.
            /// </summary>
            /// <param name="particleEmitterWin">The ParticleEmitter window.</param>
            public void OnLoad(ParticleEmitterWindow particleEmitterWin)
            {
                // Link
                Window = particleEmitterWin;
            }

            /// <summary>
            /// Clears temporary data.
            /// </summary>
            public void OnClean()
            {
                // Unlink
                Window = null;
            }
        }

        private readonly PropertiesProxy _properties;

        /// <inheritdoc />
        public ParticleEmitterWindow(Editor editor, AssetItem item)
        : base(editor, item)
        {
            NewParameterTypes = typeof(NewParameterType);

            // Asset preview
            _preview = new ParticleEmitterPreview(true)
            {
                PlaySimulation = true,
                Parent = _split2.Panel1
            };

            // Asset properties proxy
            _properties = new PropertiesProxy();
            _propertiesEditor.Select(_properties);

            // Surface
            _surface = new ParticleEmitterSurface(this, Save, _undo)
            {
                Parent = _split1.Panel1,
                Enabled = false
            };

            // Toolstrip
            _toolstrip.AddSeparator();
            _toolstrip.AddButton(editor.Icons.BracketsSlash32, () => ShowSourceCode(_asset)).LinkTooltip("Show generated shader source code");
            _toolstrip.AddButton(editor.Icons.Docs32, () => Application.StartProcess(Utilities.Constants.DocsUrl + "manual/particles/index.html")).LinkTooltip("See documentation to learn more");
        }

        /// <summary>
        /// Shows the ParticleEmitter source code window.
        /// </summary>
        /// <param name="particleEmitter">The ParticleEmitter asset.</param>
        public static void ShowSourceCode(ParticleEmitter particleEmitter)
        {
            var source = Editor.GetParticleEmitterShaderSourceCode(particleEmitter);
            Utilities.Utils.ShowSourceCode(source, "Particle Emitter GPU Simulation Source");
        }

        /// <inheritdoc />
        protected override void OnParamRenameUndo(RenameParamAction action)
        {
            base.OnParamRenameUndo(action);

            _refreshPropertiesOnLoad = true;
        }

        /// <inheritdoc />
        protected override void OnParamAddUndo(AddRemoveParamAction action)
        {
            base.OnParamAddUndo(action);

            _refreshPropertiesOnLoad = true;
        }

        /// <inheritdoc />
        protected override void OnParamRemoveUndo(AddRemoveParamAction action)
        {
            base.OnParamRemoveUndo(action);

            _refreshPropertiesOnLoad = true;
        }

        /// <inheritdoc />
        protected override void SetParameter(int index, object value)
        {
            Preview.PreviewActor.Parameters[index].Value = value;

            base.SetParameter(index, value);
        }

        /// <inheritdoc />
        protected override void UnlinkItem()
        {
            _properties.OnClean();
            _preview.Emitter = null;

            base.UnlinkItem();
        }

        /// <inheritdoc />
        protected override void OnAssetLinked()
        {
            _preview.Emitter = _asset;

            base.OnAssetLinked();
        }

        /// <inheritdoc />
        public override string SurfaceName => "Particle Emitter";

        /// <inheritdoc />
        public override byte[] SurfaceData
        {
            get => _asset.LoadSurface(true);
            set
            {
                // Save data to the temporary asset
                if (_asset.SaveSurface(value))
                {
                    // Error
                    _surface.MarkAsEdited();
                    Editor.LogError("Failed to save Particle Emitter surface data");
                }
                _preview.PreviewActor.ResetSimulation();
            }
        }

        /// <inheritdoc />
        protected override bool LoadSurface()
        {
            // Init asset properties and parameters proxy
            _properties.OnLoad(this);

            // Load surface data from the asset
            byte[] data = _asset.LoadSurface(true);
            if (data == null)
            {
                // Error
                Editor.LogError("Failed to load Particle Emitter surface data.");
                return true;
            }

            // Load surface graph
            if (_surface.Load(data))
            {
                // Error
                Editor.LogError("Failed to load Particle Emitter surface.");
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

        /// <inheritdoc />
        protected override bool SaveToOriginal()
        {
            // Copy shader cache from the temporary Particle Emitter (will skip compilation on Reload - faster)
            Guid dstId = _item.ID;
            Guid srcId = _asset.ID;
            Editor.Internal_CopyCache(ref dstId, ref srcId);

            return base.SaveToOriginal();
        }
    }
}
