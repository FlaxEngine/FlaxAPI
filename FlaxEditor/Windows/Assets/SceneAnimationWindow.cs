// Copyright (c) 2012-2019 Wojciech Figat. All rights reserved.

using System;
using System.Xml;
using FlaxEditor.Content;
using FlaxEditor.CustomEditors.Editors;
using FlaxEditor.GUI;
using FlaxEditor.GUI.Timeline;
using FlaxEngine;
using FlaxEngine.GUI;
using Object = FlaxEngine.Object;

namespace FlaxEditor.Windows.Assets
{
    /// <summary>
    /// Scene Animation window allows to view and edit <see cref="SceneAnimation"/> asset.
    /// Note: it uses ClonedAssetEditorWindowBase which is creating cloned asset to edit/preview.
    /// </summary>
    /// <seealso cref="SceneAnimation" />
    /// <seealso cref="FlaxEditor.Windows.Assets.AssetEditorWindow" />
    public sealed class SceneAnimationWindow : ClonedAssetEditorWindowBase<SceneAnimation>
    {
        private SceneAnimationTimeline _timeline;
        private ToolStripButton _saveButton;
        private FlaxObjectRefPickerControl _previewPlayerPicker;
        private bool _tmpSceneAnimationIsDirty;
        private bool _isWaitingForTimelineLoad;
        private Guid _cachedPlayerId;

        /// <summary>
        /// Gets the timeline editor.
        /// </summary>
        public SceneAnimationTimeline Timeline => _timeline;

        /// <inheritdoc />
        public SceneAnimationWindow(Editor editor, AssetItem item)
        : base(editor, item)
        {
            SceneManager.ActorDeleted += OnActorDeleted;

            // Timeline
            _timeline = new SceneAnimationTimeline
            {
                DockStyle = DockStyle.Fill,
                Parent = this,
                Enabled = false
            };
            _timeline.Modified += OnTimelineModified;
            _timeline.PlayerChanged += OnTimelinePlayerChanged;

            // Toolstrip
            _saveButton = (ToolStripButton)_toolstrip.AddButton(Editor.Icons.Save32, Save).LinkTooltip("Save");

            // Preview player picker
            var previewPlayerPickerContainer = new ContainerControl();
            var previewPlayerPickerLabel = new Label
            {
                DockStyle = DockStyle.Left,
                VerticalAlignment = TextAlignment.Center,
                HorizontalAlignment = TextAlignment.Far,
                Width = 60.0f,
                Text = "Player:",
                Parent = previewPlayerPickerContainer,
            };
            _previewPlayerPicker = new FlaxObjectRefPickerControl
            {
                Location = new Vector2(previewPlayerPickerLabel.Right + 4.0f, 8.0f),
                Width = 140.0f,
                Type = typeof(SceneAnimationPlayer),
                Parent = previewPlayerPickerContainer,
            };
            previewPlayerPickerContainer.Width = _previewPlayerPicker.Right + 2.0f;
            previewPlayerPickerContainer.Parent = _toolstrip;
            _previewPlayerPicker.ValueChanged += OnPreviewPlayerPickerChanged;
        }

        /// <inheritdoc />
        public override void OnSceneUnloading(Scene scene, Guid sceneId)
        {
            base.OnSceneUnloading(scene, sceneId);

            if (scene == _timeline.Player?.Scene)
            {
                var id = _timeline.Player.ID;
                _timeline.Player = null;
                _cachedPlayerId = id;
            }
        }

        private void OnActorDeleted(Actor actor)
        {
            if (actor == _timeline.Player)
            {
                var id = actor.ID;
                _timeline.Player = null;
                _cachedPlayerId = id;
            }
        }

        private void OnTimelinePlayerChanged()
        {
            _previewPlayerPicker.Value = _timeline.Player;
            _cachedPlayerId = _timeline.Player?.ID ?? Guid.Empty;
        }

        private void OnPreviewPlayerPickerChanged()
        {
            _timeline.Player = _previewPlayerPicker.Value as SceneAnimationPlayer;
        }

        private void OnTimelineModified()
        {
            _tmpSceneAnimationIsDirty = true;
            MarkAsEdited();
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

            // Update original asset
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
            _isWaitingForTimelineLoad = false;

            base.UnlinkItem();
        }

        /// <inheritdoc />
        protected override void OnAssetLinked()
        {
            _isWaitingForTimelineLoad = true;

            base.OnAssetLinked();
        }

        /// <inheritdoc />
        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);

            // Check if temporary asset need to be updated
            if (_tmpSceneAnimationIsDirty)
            {
                // Clear flag
                _tmpSceneAnimationIsDirty = false;

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
                ClearEditedFlag();
            }

            // Try to reassign the player
            if (_timeline.Player == null && _cachedPlayerId != Guid.Empty)
            {
                var obj = Object.Find<SceneAnimationPlayer>(ref _cachedPlayerId);
                if (obj)
                {
                    _cachedPlayerId = Guid.Empty;
                    _timeline.Player = obj;
                }
            }
        }

        /// <inheritdoc />
        public override bool UseLayoutData => true;

        /// <inheritdoc />
        public override void OnLayoutSerialize(XmlWriter writer)
        {
            writer.WriteAttributeString("TimelineSplitter", _timeline.Splitter.SplitterValue.ToString());
        }

        /// <inheritdoc />
        public override void OnLayoutDeserialize(XmlElement node)
        {
            float value1;

            if (float.TryParse(node.GetAttribute("TimelineSplitter"), out value1))
                _timeline.Splitter.SplitterValue = value1;
        }

        /// <inheritdoc />
        public override void OnLayoutDeserialize()
        {
            _timeline.Splitter.SplitterValue = 0.2f;
        }

        /// <inheritdoc />
        public override void Dispose()
        {
            if (IsDisposing)
                return;

            SceneManager.ActorDeleted -= OnActorDeleted;

            _timeline = null;
            _saveButton = null;
            _previewPlayerPicker = null;

            base.Dispose();
        }
    }
}
