// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System;
using FlaxEditor.Content.Thumbnails;
using FlaxEditor.Viewport.Previews;
using FlaxEditor.Windows;
using FlaxEditor.Windows.Assets;
using FlaxEngine;
using FlaxEngine.GUI;
using FlaxEngine.Rendering;
using Object = FlaxEngine.Object;

namespace FlaxEditor.Content
{
    /// <summary>
    /// Content proxy for <see cref="PrefabItem"/>.
    /// </summary>
    /// <seealso cref="FlaxEditor.Content.JsonAssetBaseProxy" />
    public sealed class PrefabProxy : JsonAssetBaseProxy
    {
        private PrefabPreview _preview;

        /// <summary>
        /// The prefab files extension.
        /// </summary>
        public static readonly string Extension = "prefab";

        /// <summary>
        /// The prefab asset data typename.
        /// </summary>
        public static readonly string AssetTypename = typeof(Prefab).FullName;

        /// <inheritdoc />
        public override string Name => "Prefab";

        /// <inheritdoc />
        public override ContentDomain Domain => ContentDomain.Prefab;

        /// <inheritdoc />
        public override string FileExtension => Extension;

        /// <inheritdoc />
        public override EditorWindow Open(Editor editor, ContentItem item)
        {
            return new PrefabWindow(editor, (AssetItem)item);
        }

        /// <inheritdoc />
        public override bool IsProxyFor(ContentItem item)
        {
            return item is PrefabItem;
        }

        /// <inheritdoc />
        public override bool IsProxyFor<T>()
        {
            return typeof(T) == typeof(Prefab);
        }

        /// <inheritdoc />
        public override Color AccentColor => Color.FromRGB(0x7eef21);

        /// <inheritdoc />
        public override string TypeName => AssetTypename;

        /// <inheritdoc />
        public override AssetItem ConstructItem(string path, string typeName, ref Guid id)
        {
            return new PrefabItem(path, id);
        }

        /// <inheritdoc />
        public override bool CanCreate(ContentFolder targetLocation)
        {
            return targetLocation.CanHaveAssets;
        }

        /// <inheritdoc />
        public override void Create(string outputPath, object arg)
        {
            var actor = arg as Actor;
            if (actor == null)
            {
                // Create default prefab root object
                actor = EmptyActor.New();
                actor.Name = "Root";

                // Cleanup it after usage
                Object.Destroy(actor, 20.0f);
            }

            Editor.CreatePrefab(outputPath, actor, true);
        }

        /// <inheritdoc />
        public override void OnThumbnailDrawPrepare(ThumbnailRequest request)
        {
            if (_preview == null)
            {
                _preview = new PrefabPreview(false);
                _preview.RenderOnlyWithWindow = false;
                _preview.Task.Enabled = false;
                _preview.PostFxVolume.Settings.Eye_Technique = EyeAdaptationTechnique.None;
                _preview.PostFxVolume.Settings.Eye_Exposure = 0.1f;
                _preview.PostFxVolume.Settings.data.Flags4 |= 0b1001;
                _preview.Size = new Vector2(PreviewsCache.AssetIconSize, PreviewsCache.AssetIconSize);
                _preview.SyncBackbufferSize();
            }

            // TODO: disable streaming for asset during thumbnail rendering (and restore it after)
        }

        /// <inheritdoc />
        public override bool CanDrawThumbnail(ThumbnailRequest request)
        {
            if (!_preview.HasLoadedAssets)
                return false;

            // Check if asset is streamed enough
            var asset = (Prefab)request.Asset;
            return asset.IsLoaded;
        }

        private void Prepare(Actor actor)
        {
            if (actor is TextRender textRender)
            {
                textRender.UpdateLayout();
            }

            for (int i = 0; i < actor.ChildrenCount; i++)
            {
                Prepare(actor.GetChild(i));
            }
        }

        /// <inheritdoc />
        public override void OnThumbnailDrawBegin(ThumbnailRequest request, ContainerControl guiRoot, GPUContext context)
        {
            _preview.Prefab = (Prefab)request.Asset;
            _preview.Parent = guiRoot;

            // Update some actors data (some actor types update bounds/data later but its required to be done before rendering)
            Prepare(_preview.Instance);

            // Auto fit
            float targetSize = 30.0f;
            BoundingBox bounds;
            Editor.GetActorEditorBox(_preview.Instance, out bounds);
            float maxSize = Mathf.Max(0.001f, bounds.Size.MaxValue);
            _preview.Instance.Scale = new Vector3(targetSize / maxSize);
            _preview.Instance.Position = Vector3.Zero;

            _preview.Task.Internal_Render(context);
        }

        /// <inheritdoc />
        public override void OnThumbnailDrawEnd(ThumbnailRequest request, ContainerControl guiRoot)
        {
            _preview.Prefab = null;
            _preview.Parent = null;
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
