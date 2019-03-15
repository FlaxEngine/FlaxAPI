// Copyright (c) 2012-2019 Wojciech Figat. All rights reserved.

using System;
using System.Xml;
using FlaxEditor.Content;
using FlaxEditor.CustomEditors;
using FlaxEditor.GUI;
using FlaxEditor.GUI.Timeline;
using FlaxEditor.Viewport.Previews;
using FlaxEngine;
using FlaxEngine.GUI;

namespace FlaxEditor.Windows.Assets
{
    /// <summary>
    /// Particle System window allows to view and edit <see cref="ParticleSystem"/> asset.
    /// Note: it uses ClonedAssetEditorWindowBase which is creating cloned asset to edit/preview.
    /// </summary>
    /// <seealso cref="ParticleSystem" />
    /// <seealso cref="FlaxEditor.Windows.Assets.AssetEditorWindow" />
    public sealed class ParticleSystemWindow : ClonedAssetEditorWindowBase<ParticleSystem>
    {
        private readonly SplitPanel _split1;
        private readonly SplitPanel _split2;
        private readonly ParticleSystemTimeline _timeline;
        private readonly ParticleSystemPreview _preview;
        private readonly CustomEditorPresenter _propertiesEditor;

        private readonly ToolStripButton _saveButton;
        private bool _tmpParticleSystemIsDirty;
        private bool _isWaitingForTimelineLoad;

        /// <inheritdoc />
        public ParticleSystemWindow(Editor editor, AssetItem item)
        : base(editor, item)
        {
            // Split Panel 1
            _split1 = new SplitPanel(Orientation.Vertical, ScrollBars.None, ScrollBars.None)
            {
                DockStyle = DockStyle.Fill,
                SplitterValue = 0.6f,
                Parent = this
            };

            // Split Panel 2
            _split2 = new SplitPanel(Orientation.Horizontal, ScrollBars.None, ScrollBars.Vertical)
            {
                DockStyle = DockStyle.Fill,
                SplitterValue = 0.5f,
                Parent = _split1.Panel1
            };

            // Particles preview
            _preview = new ParticleSystemPreview(true)
            {
                PlaySimulation = true,
                Parent = _split2.Panel1
            };

            // Properties editor
            var propertiesEditor = new CustomEditorPresenter(null);
            propertiesEditor.Panel.Parent = _split2.Panel2;
            propertiesEditor.Modified += OnParticleSystemPropertyEdited;
            _propertiesEditor = propertiesEditor;

            // Timeline
            _timeline = new ParticleSystemTimeline(_preview)
            {
                DockStyle = DockStyle.Fill,
                Parent = _split1.Panel2,
                Enabled = false
            };

            // Toolstrip
            _saveButton = (ToolStripButton)_toolstrip.AddButton(Editor.Icons.Save32, Save).LinkTooltip("Save");
            _toolstrip.AddSeparator();
            _toolstrip.AddButton(editor.Icons.Docs32, () => Application.StartProcess(Utilities.Constants.DocsUrl + "manual/particles/index.html")).LinkTooltip("See documentation to learn more");
        }

        private void OnParticleSystemPropertyEdited()
        {
            _timeline.MarkAsEdited();
        }

        /// <summary>
        /// Refreshes temporary asset to see changes live when editing the timeline.
        /// </summary>
        /// <returns>True if cannot refresh it, otherwise false.</returns>
        public bool RefreshTempAsset()
        {
            // Early check
            if (_asset == null || _isWaitingForTimelineLoad)
                return true;

            // Check if surface has been edited
            if (_timeline.IsModified)
            {
                // TODO: saving timeline data
                //_timeline.Save();
            }

            return false;
        }

        /// <inheritdoc />
        public override void Save()
        {
            // Check if don't need to push any new changes to the original asset
            if (!IsEdited)
                return;

            // Just in case refresh data
            if (RefreshTempAsset())
            {
                // Error
                return;
            }

            // Copy shader cache from the temporary Particle Emitter (will skip compilation on Reload - faster)
            Guid dstId = _item.ID;
            Guid srcId = _asset.ID;
            Editor.Internal_CopyCache(ref dstId, ref srcId);

            // Update original Particle Emitter so user can see changes in the scene
            if (SaveToOriginal())
            {
                // Error
                return;
            }

            // Clear flag
            ClearEditedFlag();

            // Update
            _item.RefreshThumbnail();
        }

        /// <inheritdoc />
        protected override void UpdateToolstrip()
        {
            _saveButton.Enabled = IsEdited;

            base.UpdateToolstrip();
        }

        /// <inheritdoc />
        protected override void UnlinkItem()
        {
            _preview.System = null;
            _isWaitingForTimelineLoad = false;

            base.UnlinkItem();
        }

        /// <inheritdoc />
        protected override void OnAssetLinked()
        {
            _preview.System = _asset;
            _isWaitingForTimelineLoad = true;

            base.OnAssetLinked();
        }

        /// <inheritdoc />
        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);

            // Check if temporary asset need to be updated
            if (_tmpParticleSystemIsDirty)
            {
                // Clear flag
                _tmpParticleSystemIsDirty = false;

                // Update
                RefreshTempAsset();
            }

            // Check if need to load timeline
            if (_isWaitingForTimelineLoad && _asset.IsLoaded)
            {
                // Clear flag
                _isWaitingForTimelineLoad = false;

                // TODO: loading timeline data from the asset
                /*
                // Load timeline data from the asset
                byte[] data = _asset.LoadSurface(true);
                if (data == null)
                {
                    // Error
                    Editor.LogError("Failed to load Particle Emitter surface data.");
                    Close();
                    return;
                }

                // Load surface graph
                if (_surface.Load(data))
                {
                    // Error
                    Editor.LogError("Failed to load Particle Emitter surface.");
                    Close();
                    return;
                }
                */
                // Setup
                _timeline.Enabled = true;
                _propertiesEditor.BuildLayout();
                ClearEditedFlag();
            }
        }

        /// <inheritdoc />
        public override bool UseLayoutData => true;

        /// <inheritdoc />
        public override void OnLayoutSerialize(XmlWriter writer)
        {
            writer.WriteAttributeString("Split1", _split1.SplitterValue.ToString());
            writer.WriteAttributeString("Split2", _split2.SplitterValue.ToString());
            writer.WriteAttributeString("Split3", _timeline.Splitter.SplitterValue.ToString());
        }

        /// <inheritdoc />
        public override void OnLayoutDeserialize(XmlElement node)
        {
            float value1;

            if (float.TryParse(node.GetAttribute("Split1"), out value1))
                _split1.SplitterValue = value1;
            if (float.TryParse(node.GetAttribute("Split2"), out value1))
                _split2.SplitterValue = value1;
            if (float.TryParse(node.GetAttribute("Split3"), out value1))
                _timeline.Splitter.SplitterValue = value1;
        }

        /// <inheritdoc />
        public override void OnLayoutDeserialize()
        {
            _split1.SplitterValue = 0.6f;
            _split2.SplitterValue = 0.5f;
            _timeline.Splitter.SplitterValue = 0.2f;
        }
    }
}
