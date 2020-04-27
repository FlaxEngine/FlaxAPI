// Copyright (c) 2012-2020 Wojciech Figat. All rights reserved.

using System;
using System.Linq;
using System.Xml;
using FlaxEditor.Content;
using FlaxEditor.CustomEditors;
using FlaxEditor.CustomEditors.Editors;
using FlaxEditor.CustomEditors.GUI;
using FlaxEditor.GUI;
using FlaxEditor.GUI.Timeline;
using FlaxEditor.GUI.Timeline.Tracks;
using FlaxEditor.Surface;
using FlaxEditor.Viewport.Previews;
using FlaxEngine;
using FlaxEngine.GUI;
using Object = FlaxEngine.Object;

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
        /// <summary>
        /// The proxy object for editing particle system properties.
        /// </summary>
        private class GeneralProxy
        {
            private readonly ParticleSystemWindow _window;

            [EditorDisplay("Particle System"), EditorOrder(100), Limit(1), Tooltip("The timeline animation duration in frames.")]
            public int TimelineDurationFrames
            {
                get => _window.Timeline.DurationFrames;
                set => _window.Timeline.DurationFrames = value;
            }

            public GeneralProxy(ParticleSystemWindow window)
            {
                _window = window;
            }
        }

        /// <summary>
        /// The proxy object for editing particle system track properties.
        /// </summary>
        [CustomEditor(typeof(EmitterTrackProxyEditor))]
        private class EmitterTrackProxy
        {
            private readonly ParticleSystemWindow _window;
            private readonly ParticleEffect _effect;
            private readonly int _emitterIndex;
            private readonly ParticleEmitterTrack _track;

            [EditorDisplay("Particle Emitter"), EditorOrder(0), Tooltip("The name text.")]
            public string Name
            {
                get => _track.Name;
                set
                {
                    if (!_track.Timeline.IsTrackNameValid(value))
                    {
                        MessageBox.Show("Invalid name. It must be unique.", "Invalid track name", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    _track.Name = value;
                }
            }

            [EditorDisplay("Particle Emitter"), EditorOrder(100), Tooltip("The particle emitter to use for the track media event playback.")]
            public ParticleEmitter Emitter
            {
                get => _track.Asset;
                set => _track.Asset = value;
            }

            private bool HasEmitter => _track.Asset != null;

            [EditorDisplay("Particle Emitter"), VisibleIf("HasEmitter"), EditorOrder(200), Tooltip("The start frame of the media event.")]
            public int StartFrame
            {
                get => _track.Media.Count > 0 ? _track.TrackMedia.StartFrame : 0;
                set => _track.TrackMedia.StartFrame = value;
            }

            [EditorDisplay("Particle Emitter"), Limit(1), VisibleIf("HasEmitter"), EditorOrder(300), Tooltip("The total duration of the media event in the timeline sequence frames amount.")]
            public int DurationFrames
            {
                get => _track.Media.Count > 0 ? _track.TrackMedia.DurationFrames : 0;
                set => _track.TrackMedia.DurationFrames = value;
            }

            public EmitterTrackProxy(ParticleSystemWindow window, ParticleEffect effect, ParticleEmitterTrack track, int emitterIndex)
            {
                _window = window;
                _effect = effect;
                _emitterIndex = emitterIndex;
                _track = track;
            }

            private sealed class EmitterTrackProxyEditor : GenericEditor
            {
                private static object ParameterValueToOverride(GraphParamType type, object value)
                {
                    switch (type)
                    {
                    case GraphParamType.ChannelMask: return (int)value;
                    case GraphParamType.CubeTexture:
                    case GraphParamType.Texture:
                    case GraphParamType.NormalMap:
                    case GraphParamType.GPUTexture:
                    case GraphParamType.GPUTextureArray:
                    case GraphParamType.GPUTextureCube:
                    case GraphParamType.GPUTextureVolume: return ((Object)value)?.ID ?? Guid.Empty;
                    default: return value;
                    }
                }

                /// <inheritdoc />
                public override void Initialize(LayoutElementsContainer layout)
                {
                    base.Initialize(layout);

                    var value = Values[0] as EmitterTrackProxy;
                    if (value?._effect?.Parameters == null)
                        return;

                    var group = layout.Group("Parameters");
                    var parameters = value._effect.Parameters.Where(x => x.EmitterIndex == value._emitterIndex && x.Emitter == value.Emitter && x.IsPublic).ToArray();

                    if (!parameters.Any())
                    {
                        group.Label("No parameters", TextAlignment.Center);
                        return;
                    }

                    foreach (var p in parameters)
                    {
                        var type = VisjectSurface.GetParameterValueType((ParameterType)p.ParamType);
                        var defaultValue = p.Value;

                        // Parameter value accessor
                        var propertyValue = new CustomValueContainer(type, defaultValue, (instance, index) => p.Value, (instance, index, _) =>
                        {
                            if (p.Value == _)
                                return;

                            value._window._isEditingInstancedParameterValue = true;
                            p.Value = _;

                            var id = p.ParamIdentifier;
                            if (value._track.ParametersOverrides.ContainsKey(id))
                            {
                                value._track.ParametersOverrides[id] = ParameterValueToOverride(p.ParamType, _);
                                value._window.Timeline.OnEmittersParametersOverridesEdited();
                                value._window.MarkAsEdited();
                            }
                        });
                        propertyValue.SetDefaultValue(p.DefaultEmitterValue);

                        // Use label with parameter value override checkbox
                        var label = new CheckablePropertyNameLabel(p.Name);
                        label.CheckBox.Checked = value._track.ParametersOverrides.ContainsKey(p.ParamIdentifier);
                        label.CheckBox.Tag = value;
                        label.CheckChanged += nameLabel =>
                        {
                            var proxy = (EmitterTrackProxy)nameLabel.CheckBox.Tag;
                            if (nameLabel.CheckBox.Checked)
                                proxy._track.ParametersOverrides.Add(p.ParamIdentifier, ParameterValueToOverride(p.ParamType, p.Value));
                            else
                                proxy._track.ParametersOverrides.Remove(p.ParamIdentifier);
                            value._window.Timeline.OnEmittersParametersOverridesEdited();
                            proxy._window.Timeline.MarkAsEdited();
                        };

                        group.Property(label, propertyValue);
                        label.UpdateStyle();
                    }
                }
            }
        }

        /// <summary>
        /// The proxy object for editing folder track properties.
        /// </summary>
        private class FolderTrackProxy
        {
            private readonly FolderTrack _track;

            [EditorDisplay("Folder"), EditorOrder(0), Tooltip("The name text.")]
            public string Name
            {
                get => _track.Name;
                set
                {
                    if (!_track.Timeline.IsTrackNameValid(value))
                    {
                        MessageBox.Show("Invalid name. It must be unique.", "Invalid track name", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    _track.Name = value;
                }
            }

            [EditorDisplay("Folder"), EditorOrder(200), Tooltip("The folder icon color.")]
            public Color Color
            {
                get => _track.IconColor;
                set => _track.IconColor = value;
            }

            public FolderTrackProxy(FolderTrack track)
            {
                _track = track;
            }
        }

        private readonly SplitPanel _split1;
        private readonly SplitPanel _split2;
        private readonly ParticleSystemTimeline _timeline;
        private readonly ParticleSystemPreview _preview;
        private readonly CustomEditorPresenter _propertiesEditor1;
        private readonly CustomEditorPresenter _propertiesEditor2;
        private readonly ToolStripButton _saveButton;
        private bool _tmpParticleSystemIsDirty;
        private bool _isWaitingForTimelineLoad;
        private bool _isEditingInstancedParameterValue;
        private uint _parametersVersion;

        /// <summary>
        /// Gets the particle system preview.
        /// </summary>
        public ParticleSystemPreview Preview => _preview;

        /// <summary>
        /// Gets the timeline editor.
        /// </summary>
        public ParticleSystemTimeline Timeline => _timeline;

        /// <inheritdoc />
        public ParticleSystemWindow(Editor editor, AssetItem item)
        : base(editor, item)
        {
            // Split Panel 1
            _split1 = new SplitPanel(Orientation.Vertical, ScrollBars.None, ScrollBars.None)
            {
                AnchorPreset = AnchorPresets.StretchAll,
                Offsets = new Margin(0, 0, _toolstrip.Bottom, 0),
                SplitterValue = 0.6f,
                Parent = this
            };

            // Split Panel 2
            _split2 = new SplitPanel(Orientation.Horizontal, ScrollBars.None, ScrollBars.Vertical)
            {
                AnchorPreset = AnchorPresets.StretchAll,
                Offsets = Margin.Zero,
                SplitterValue = 0.5f,
                Parent = _split1.Panel1
            };

            // Particles preview
            _preview = new ParticleSystemPreview(true)
            {
                PlaySimulation = true,
                Parent = _split2.Panel1
            };

            // Timeline
            _timeline = new ParticleSystemTimeline(_preview)
            {
                AnchorPreset = AnchorPresets.StretchAll,
                Offsets = Margin.Zero,
                Parent = _split1.Panel2,
                Enabled = false
            };
            _timeline.Modified += OnTimelineModified;
            _timeline.SelectionChanged += OnTimelineSelectionChanged;

            // Properties editor (general)
            var propertiesEditor1 = new CustomEditorPresenter(null, string.Empty);
            propertiesEditor1.Panel.Parent = _split2.Panel2;
            propertiesEditor1.Modified += OnParticleSystemPropertyEdited;
            _propertiesEditor1 = propertiesEditor1;
            propertiesEditor1.Select(new GeneralProxy(this));

            // Properties editor (selection)
            var propertiesEditor2 = new CustomEditorPresenter(null, string.Empty);
            propertiesEditor2.Panel.Parent = _split2.Panel2;
            propertiesEditor2.Modified += OnParticleSystemPropertyEdited;
            _propertiesEditor2 = propertiesEditor2;

            // Toolstrip
            _saveButton = (ToolStripButton)_toolstrip.AddButton(Editor.Icons.Save32, Save).LinkTooltip("Save");
            _toolstrip.AddSeparator();
            _toolstrip.AddButton(editor.Icons.Docs32, () => Platform.OpenUrl(Utilities.Constants.DocsUrl + "manual/particles/index.html")).LinkTooltip("See documentation to learn more");
        }

        private void OnTimelineModified()
        {
            _tmpParticleSystemIsDirty = true;
            MarkAsEdited();
        }

        private void OnTimelineSelectionChanged()
        {
            if (_timeline.SelectedTracks.Count == 0)
            {
                _propertiesEditor2.Deselect();
                return;
            }

            var tracks = new object[_timeline.SelectedTracks.Count];
            var emitterTracks = _timeline.Tracks.Where(track => track is ParticleEmitterTrack).Cast<ParticleEmitterTrack>().ToList();
            for (var i = 0; i < _timeline.SelectedTracks.Count; i++)
            {
                var track = _timeline.SelectedTracks[i];
                if (track is ParticleEmitterTrack particleEmitterTrack)
                {
                    tracks[i] = new EmitterTrackProxy(this, Preview.PreviewActor, particleEmitterTrack, emitterTracks.IndexOf(particleEmitterTrack));
                }
                else if (track is FolderTrack folderTrack)
                {
                    tracks[i] = new FolderTrackProxy(folderTrack);
                }
                else
                {
                    throw new NotImplementedException("Invalid track type.");
                }
            }
            _propertiesEditor2.Select(tracks);
        }

        private void OnParticleSystemPropertyEdited()
        {
            if (_isEditingInstancedParameterValue)
                return;

            _timeline.MarkAsEdited();
        }

        /// <summary>
        /// Refreshes temporary asset to see changes live when editing the timeline.
        /// </summary>
        /// <returns>True if cannot refresh it, otherwise false.</returns>
        public bool RefreshTempAsset()
        {
            if (_asset == null || _isWaitingForTimelineLoad)
                return true;

            if (_timeline.IsModified)
            {
                _propertiesEditor1.BuildLayoutOnUpdate();
                _propertiesEditor2.BuildLayoutOnUpdate();
                _timeline.Save(_asset);
            }

            return false;
        }

        /// <inheritdoc />
        public override void Save()
        {
            if (!IsEdited)
                return;

            if (RefreshTempAsset())
            {
                return;
            }

            if (SaveToOriginal())
            {
                return;
            }

            ClearEditedFlag();
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
            _propertiesEditor2.Deselect();
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
            // Check if need to refresh the parameters
            if (_parametersVersion != _preview.PreviewActor.ParametersVersion)
            {
                _parametersVersion = _preview.PreviewActor.ParametersVersion;
                _propertiesEditor2.BuildLayoutOnUpdate();
            }

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

                // Load timeline data from the asset
                _timeline.Load(_asset);

                // Setup
                _timeline.Enabled = true;
                _propertiesEditor1.BuildLayout();
                _propertiesEditor2.Deselect();
                ClearEditedFlag();
            }

            // Clear flag
            _isEditingInstancedParameterValue = false;
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
