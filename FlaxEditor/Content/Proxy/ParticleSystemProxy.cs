// Copyright (c) 2012-2019 Wojciech Figat. All rights reserved.

using System;
using FlaxEditor.Content.Thumbnails;
using FlaxEditor.Viewport.Previews;
using FlaxEditor.Windows;
using FlaxEditor.Windows.Assets;
using FlaxEngine;
using FlaxEngine.GUI;

namespace FlaxEditor.Content
{
    /// <summary>
    /// A <see cref="ParticleSystem"/> asset proxy object.
    /// </summary>
    /// <seealso cref="FlaxEditor.Content.BinaryAssetProxy" />
    public class ParticleSystemProxy : BinaryAssetProxy
    {
        private ParticleSystemPreview _preview;
        private ThumbnailRequest _warmupRequest;

        /// <inheritdoc />
        public override string Name => "Particle System";

        /// <inheritdoc />
        public override EditorWindow Open(Editor editor, ContentItem item)
        {
            return new ParticleSystemWindow(editor, item as AssetItem);
        }

        /// <inheritdoc />
        public override Color AccentColor => Color.FromRGB(0xFF790200);

        /// <inheritdoc />
        public override ContentDomain Domain => ContentDomain.Other;

        /// <inheritdoc />
        public override Type AssetType => typeof(ParticleSystem);

        /// <inheritdoc />
        public override bool CanCreate(ContentFolder targetLocation)
        {
            return targetLocation.CanHaveAssets;
        }

        /// <inheritdoc />
        public override void Create(string outputPath, object arg)
        {
            if (Editor.CreateAsset(Editor.NewAssetType.ParticleSystem, outputPath))
                throw new Exception("Failed to create new asset.");
        }

        /// <inheritdoc />
        public override void OnThumbnailDrawPrepare(ThumbnailRequest request)
        {
            if (_preview == null)
            {
                _preview = new ParticleEmitterPreview(false);
                _preview.RenderOnlyWithWindow = false;
                _preview.Task.Enabled = false;

                var eyeAdaptation = _preview.PostFxVolume.EyeAdaptation;
                eyeAdaptation.Mode = EyeAdaptationMode.None;
                eyeAdaptation.OverrideFlags |= EyeAdaptationSettings.Override.Mode;
                _preview.PostFxVolume.EyeAdaptation = eyeAdaptation;

                _preview.Size = new Vector2(PreviewsCache.AssetIconSize, PreviewsCache.AssetIconSize);
                _preview.SyncBackbufferSize();
            }

            // Mark for initial warmup
            request.Tag = 0;
        }

        /// <inheritdoc />
        public override bool CanDrawThumbnail(ThumbnailRequest request)
        {
            var state = (int)request.Tag;
            if (state == 2)
                return true;

            // Allow only one request at once during warmup time
            if (_warmupRequest != null && _warmupRequest != request)
                return false;

            // Ensure assets are ready to be used
            if (!_preview.HasLoadedAssets)
                return false;
            var asset = (ParticleSystem)request.Asset;
            if (!asset.IsLoaded)
                return false;

            if (state == 0)
            {
                // Start the warmup
                _warmupRequest = request;
                request.Tag = 1;
                _preview.System = asset;
            }
            else if (_preview.PreviewActor.Time >= Mathf.Min(0.2f * asset.Duration, 0.6f))
            {
                // End the warmup
                request.Tag = 2;
                _preview.FitIntoView();
                return true;
            }

            // Handle warmup time for the preview
            _preview.PreviewActor.UpdateSimulation();
            return false;
        }

        /// <inheritdoc />
        public override void OnThumbnailDrawBegin(ThumbnailRequest request, ContainerControl guiRoot, GPUContext context)
        {
            _preview.Parent = guiRoot;

            _preview.Task.OnRender(context);
        }

        /// <inheritdoc />
        public override void OnThumbnailDrawEnd(ThumbnailRequest request, ContainerControl guiRoot)
        {
            if (_warmupRequest == request)
            {
                _warmupRequest = null;
            }

            _preview.System = null;
            _preview.Parent = null;
        }

        /// <inheritdoc />
        public override void OnThumbnailDrawCleanup(ThumbnailRequest request)
        {
            if (_warmupRequest == request)
            {
                _warmupRequest = null;
            }
        }

        /// <inheritdoc />
        public override void Dispose()
        {
            if (_preview != null)
            {
                _preview.Dispose();
                _preview = null;
            }

            base.Dispose();
        }
    }
}
