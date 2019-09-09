// Copyright (c) 2012-2019 Wojciech Figat. All rights reserved.

using System;
using FlaxEditor.Content;
using FlaxEditor.GUI.Drag;
using FlaxEditor.GUI.Timeline.Tracks;
using FlaxEditor.SceneGraph;
using FlaxEngine;

namespace FlaxEditor.GUI.Timeline
{
    /// <summary>
    /// The timeline editor for scene animation asset.
    /// </summary>
    /// <seealso cref="FlaxEditor.GUI.Timeline.Timeline" />
    public sealed class SceneAnimationTimeline : Timeline
    {
        private sealed class Proxy : ProxyBase<SceneAnimationTimeline>
        {
            /// <inheritdoc />
            public Proxy(SceneAnimationTimeline timeline)
            : base(timeline)
            {
            }
        }

        private SceneAnimationPlayer _player;

        /// <summary>
        /// Gets or sets the animation player actor used for the timeline preview.
        /// </summary>
        public SceneAnimationPlayer Player
        {
            get => _player;
            set
            {
                if (_player == value)
                    return;

                _player = value;

                UpdatePlaybackState();
                PlayerChanged?.Invoke();
            }
        }

        /// <summary>
        /// Occurs when the selected player gets changed.
        /// </summary>
        public event Action PlayerChanged;

        /// <summary>
        /// Initializes a new instance of the <see cref="SceneAnimationTimeline"/> class.
        /// </summary>
        public SceneAnimationTimeline()
        : base(PlaybackButtons.Play | PlaybackButtons.Stop)
        {
            PlaybackState = PlaybackStates.Disabled;
            PropertiesEditObject = new Proxy(this);

            // Setup track types
            TrackArchetypes.Add(FolderTrack.GetArchetype());
            TrackArchetypes.Add(PostProcessMaterialTrack.GetArchetype());
            TrackArchetypes.Add(NestedSceneAnimationTrack.GetArchetype());
            TrackArchetypes.Add(ScreenFadeTrack.GetArchetype());
            TrackArchetypes.Add(AudioTrack.GetArchetype());
            TrackArchetypes.Add(AudioVolumeTrack.GetArchetype());
            TrackArchetypes.Add(ActorTrack.GetArchetype());
            TrackArchetypes.Add(ScriptTrack.GetArchetype());
            TrackArchetypes.Add(KeyframesPropertyTrack.GetArchetype());
            TrackArchetypes.Add(CurvePropertyTrack.GetArchetype());
            TrackArchetypes.Add(StringPropertyTrack.GetArchetype());
            TrackArchetypes.Add(ObjectReferencePropertyTrack.GetArchetype());
            TrackArchetypes.Add(StructPropertyTrack.GetArchetype());
            TrackArchetypes.Add(ObjectPropertyTrack.GetArchetype());
            TrackArchetypes.Add(EventTrack.GetArchetype());
            TrackArchetypes.Add(CameraCutTrack.GetArchetype());
        }

        /// <inheritdoc />
        protected override void SetupDragDrop()
        {
            base.SetupDragDrop();

            DragHandlers.Add(new DragHandler(new DragActors(IsValidActor), OnDragActor));
            DragHandlers.Add(new DragHandler(new DragScripts(IsValidScript), OnDragScript));
            DragHandlers.Add(new DragHandler(new DragAssets(IsValidAsset), OnDragAsset));
        }

        private static bool IsValidActor(ActorNode actorNode)
        {
            return actorNode.Actor;
        }

        private static void OnDragActor(Timeline timeline, DragHelper drag)
        {
            foreach (var actorNode in ((DragActors)drag).Objects)
            {
                ActorTrack track;
                if (actorNode.Actor is Camera)
                    track = (CameraCutTrack)timeline.AddTrack(CameraCutTrack.GetArchetype());
                else
                    track = (ActorTrack)timeline.AddTrack(ActorTrack.GetArchetype());
                track.Actor = actorNode.Actor;
                track.Rename(actorNode.Name);
            }
        }

        private static bool IsValidScript(Script script)
        {
            return script && script.Actor;
        }

        private static void OnDragScript(Timeline timeline, DragHelper drag)
        {
            foreach (var script in ((DragScripts)drag).Objects)
            {
                var actor = script.Actor;
                var track = (ActorTrack)timeline.AddTrack(ActorTrack.GetArchetype());
                track.Actor = actor;
                track.Rename(actor.Name);
                track.AddScriptTrack(script);
            }
        }

        private static bool IsValidAsset(AssetItem assetItem)
        {
            if (assetItem is BinaryAssetItem binaryAssetItem)
            {
                if (typeof(MaterialBase).IsAssignableFrom(binaryAssetItem.Type))
                {
                    var material = FlaxEngine.Content.Load<MaterialBase>(binaryAssetItem.ID);
                    if (material && !material.WaitForLoaded() && material.IsPostFx)
                        return true;
                }
                else if (typeof(SceneAnimation).IsAssignableFrom(binaryAssetItem.Type))
                {
                    var sceneAnimation = FlaxEngine.Content.Load<SceneAnimation>(binaryAssetItem.ID);
                    if (sceneAnimation)
                        return true;
                }
                else if (typeof(AudioClip).IsAssignableFrom(binaryAssetItem.Type))
                {
                    var audioClip = FlaxEngine.Content.Load<AudioClip>(binaryAssetItem.ID);
                    if (audioClip)
                        return true;
                }
            }

            return false;
        }

        private static void OnDragAsset(Timeline timeline, DragHelper drag)
        {
            foreach (var assetItem in ((DragAssets)drag).Objects)
            {
                if (assetItem is BinaryAssetItem binaryAssetItem)
                {
                    if (typeof(MaterialBase).IsAssignableFrom(binaryAssetItem.Type))
                    {
                        var material = FlaxEngine.Content.Load<MaterialBase>(binaryAssetItem.ID);
                        if (material && !material.WaitForLoaded() && material.IsPostFx)
                        {
                            var track = (PostProcessMaterialTrack)timeline.AddTrack(PostProcessMaterialTrack.GetArchetype());
                            track.Asset = material;
                            track.Rename(assetItem.ShortName);
                        }
                    }
                    else if (typeof(SceneAnimation).IsAssignableFrom(binaryAssetItem.Type))
                    {
                        var sceneAnimation = FlaxEngine.Content.Load<SceneAnimation>(binaryAssetItem.ID);
                        if (!sceneAnimation || sceneAnimation.WaitForLoaded())
                            continue;
                        var track = (NestedSceneAnimationTrack)timeline.AddTrack(NestedSceneAnimationTrack.GetArchetype());
                        track.Asset = sceneAnimation;
                        track.TrackMedia.DurationFrames = sceneAnimation.DurationFrames;
                        track.Rename(assetItem.ShortName);
                    }
                    else if (typeof(AudioClip).IsAssignableFrom(binaryAssetItem.Type))
                    {
                        var audioClip = FlaxEngine.Content.Load<AudioClip>(binaryAssetItem.ID);
                        if (!audioClip || audioClip.WaitForLoaded())
                            continue;
                        var track = (AudioTrack)timeline.AddTrack(AudioTrack.GetArchetype());
                        track.Asset = audioClip;
                        track.TrackMedia.Duration = audioClip.Length;
                        track.Rename(assetItem.ShortName);
                    }
                }
            }
        }

        private void UpdatePlaybackState()
        {
            PlaybackStates state;
            if (!_player)
                state = PlaybackStates.Disabled;
            else if (_player.IsPlaying)
                state = PlaybackStates.Playing;
            else if (_player.IsPaused)
                state = PlaybackStates.Paused;
            else if (_player.IsStopped)
                state = PlaybackStates.Stopped;
            else
                state = PlaybackStates.Disabled;

            PlaybackState = state;

            if (_player && _player.Animation && _player.Animation.IsLoaded)
            {
                CurrentFrame = (int)(_player.Time * _player.Animation.FramesPerSecond);
            }
            else
            {
                CurrentFrame = 0;
            }
        }

        /// <inheritdoc />
        public override void OnPlay()
        {
            _player.Play();

            base.OnPlay();
        }

        /// <inheritdoc />
        public override void OnPause()
        {
            _player.Pause();

            base.OnPause();
        }

        /// <inheritdoc />
        public override void OnStop()
        {
            _player.Stop();

            base.OnStop();
        }

        /// <inheritdoc />
        public override void OnSeek(int frame)
        {
            _player.Time = frame / _player.Animation.FramesPerSecond;

            base.OnSeek(frame);
        }

        /// <inheritdoc />
        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);

            UpdatePlaybackState();
        }

        /// <summary>
        /// Loads the timeline from the specified <see cref="FlaxEngine.SceneAnimation"/> asset.
        /// </summary>
        /// <param name="asset">The asset.</param>
        public void Load(SceneAnimation asset)
        {
            var data = asset.LoadTimeline();
            Load(data);
        }

        /// <summary>
        /// Saves the timeline data to the <see cref="FlaxEngine.SceneAnimation"/> asset.
        /// </summary>
        /// <param name="asset">The asset.</param>
        public void Save(SceneAnimation asset)
        {
            var data = Save();
            asset.SaveTimeline(data);
        }
    }
}
