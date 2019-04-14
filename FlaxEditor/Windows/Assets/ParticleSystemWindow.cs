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
        /// <summary>
        /// The proxy object for editing particle system properties.
        /// </summary>
        private class GeneralProxy
        {
            [HideInEditor]
            public readonly ParticleSystemWindow Window;

            [EditorDisplay("Particle System"), EditorOrder(100), Limit(1), Tooltip("The timeline animation duration in frames.")]
            public int TimelineDuration
            {
                get => Window.Timeline.DurationFrames;
                set => Window.Timeline.DurationFrames = value;
            }

            public GeneralProxy(ParticleSystemWindow window)
            {
                Window = window;
            }
        }

        /// <summary>
        /// The proxy object for editing particle system track properties.
        /// </summary>
        private class EmitterTrackProxy
        {
            [HideInEditor]
            public readonly ParticleEmitterTrack Track;

            [EditorDisplay("Particle Emitter"), EditorOrder(0), Tooltip("The name text.")]
            public string Name
            {
                get => Track.Name;
                set
                {
                    if (!Track.Timeline.IsTrackNameValid(value))
                    {
                        MessageBox.Show("Invalid name. It must be unique.", "Invalid track name", MessageBox.Buttons.OK, MessageBox.Icon.Warning);
                        return;
                    }

                    Track.Name = value;
                }
            }

            [EditorDisplay("Particle Emitter"), EditorOrder(100), Tooltip("The particle emitter to use for the track media event playback.")]
            public ParticleEmitter Emitter
            {
                get => Track.Emitter;
                set => Track.Emitter = value;
            }

            private bool HasEmitter => Track.Emitter != null;

            [EditorDisplay("Particle Emitter"), VisibleIf("HasEmitter"), EditorOrder(200), Tooltip("The start frame of the media event.")]
            public int Start
            {
                get => Track.Media.Count > 0 ? Track.EmitterMedia.StartFrame : 0;
                set => Track.EmitterMedia.StartFrame = value;
            }

            [EditorDisplay("Particle Emitter"), Limit(1), VisibleIf("HasEmitter"), EditorOrder(300), Tooltip("The total duration of the media event in the timeline sequence frames amount.")]
            public int Duration
            {
                get => Track.Media.Count > 0 ? Track.EmitterMedia.DurationFrames : 0;
                set => Track.EmitterMedia.DurationFrames = value;
            }

            public EmitterTrackProxy(ParticleEmitterTrack track)
            {
                Track = track;
            }
        }

        /// <summary>
        /// The proxy object for editing folder track properties.
        /// </summary>
        private class FolderTrackProxy
        {
            [HideInEditor]
            public readonly FolderTrack Track;

            [EditorDisplay("Folder"), EditorOrder(0), Tooltip("The name text.")]
            public string Name
            {
                get => Track.Name;
                set
                {
                    if (!Track.Timeline.IsTrackNameValid(value))
                    {
                        MessageBox.Show("Invalid name. It must be unique.", "Invalid track name", MessageBox.Buttons.OK, MessageBox.Icon.Warning);
                        return;
                    }

                    Track.Name = value;
                }
            }

            [EditorDisplay("Folder"), EditorOrder(200), Tooltip("The folder icon color.")]
            public Color Color
            {
                get => Track.IconColor;
                set => Track.IconColor = value;
            }

            public FolderTrackProxy(FolderTrack track)
            {
                Track = track;
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

            // Timeline
            _timeline = new ParticleSystemTimeline(_preview)
            {
                DockStyle = DockStyle.Fill,
                Parent = _split1.Panel2,
                Enabled = false
            };
            _timeline.Modified += OnTimelineModified;
            _timeline.SelectionChanged += OnTimelineSelectionChanged;

            // Toolstrip
            _saveButton = (ToolStripButton)_toolstrip.AddButton(Editor.Icons.Save32, Save).LinkTooltip("Save");
            _toolstrip.AddSeparator();
            _toolstrip.AddButton(editor.Icons.Docs32, () => Application.StartProcess(Utilities.Constants.DocsUrl + "manual/particles/index.html")).LinkTooltip("See documentation to learn more");
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
            for (var i = 0; i < _timeline.SelectedTracks.Count; i++)
            {
                var track = _timeline.SelectedTracks[i];
                if (track is ParticleEmitterTrack particleEmitterTrack)
                {
                    tracks[i] = new EmitterTrackProxy(particleEmitterTrack);
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
                _timeline.Save(_asset);
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

            // Update original particle system so user can see changes in the scene
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
