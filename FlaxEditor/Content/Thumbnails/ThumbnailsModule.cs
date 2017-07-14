////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using FlaxEditor.Modules;
using FlaxEngine;
using FlaxEngine.GUI;
using FlaxEngine.Rendering;
using Object = FlaxEngine.Object;

namespace FlaxEditor.Content.Thumbnails
{
    /// <summary>
    /// Manages asset thumbnails rendering and presentation.
    /// </summary>
    /// <seealso cref="FlaxEditor.Modules.EditorModule" />
    public sealed class ThumbnailsModule : EditorModule, IContentItemOwner
    {
        // TODO: free atlas slots for deleted assets
        // TODO: dont flush atlases every frame - do it once per second

        private readonly List<PreviewsCache> _cache = new List<PreviewsCache>(4);
        private readonly string _cacheFolder;

        private readonly List<ThumbnailRequest> _requests = new List<ThumbnailRequest>(128);
        private readonly PreviewRoot _guiRoot = new PreviewRoot();
        private CustomRenderTask _task;
        private RenderTarget _output;

        internal ThumbnailsModule(Editor editor)
            : base(editor)
        {
            _cacheFolder = Path.Combine(Globals.ProjectCacheFolder, "Thumbnails");
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private AssetProxy GetProxy(AssetItem item)
        {
            return Editor.ContentDatabase.GetProxy(item) as AssetProxy;
        }

        /// <summary>
        /// Requests the item preview.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        public void RequestPreview(ContentItem item)
        {
            if (item == null)
                throw new ArgumentNullException();

            // Check if use default icon
            var defaultThumbnail = item.DefaultThumbnailName;
            if (!string.IsNullOrEmpty(defaultThumbnail))
            {
                item.Thumbnail = Editor.Instance.UI.GetIcon(defaultThumbnail);
                return;
            }

            // We cache previews only for items with 'ID', for now we support only AssetItems
            var assetItem = item as AssetItem;
            if (assetItem == null)
                return;

            // Ensure that there is valid proxy for that item
            var proxy = Editor.ContentDatabase.GetProxy(item) as AssetProxy;
            if (proxy == null)
            {
                Debug.LogWarning($"Cannot generate preview for item {item.Path}. Cannot find proxy for it.");
                return;
            }

            lock (_requests)
            {
                // Check if element hasn't been already processed for generating preview
                if (FindRequest(assetItem) == null)
                {
                    // Check each cache atlas
                    for (int i = 0; i < _cache.Count; i++)
                    {
                        var sprite = _cache[i].FindSlot(assetItem.ID);
                        if (sprite.IsValid)
                        {
                            // Found!
                            item.Thumbnail = sprite;
                            return;
                        }
                    }

                    // Add request
                    AddRequest(assetItem, proxy);
                }
            }
        }

        /// <summary>
        /// Deletes the item preview from the cache.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        public void DeletePreview(ContentItem item)
        {
            if (item == null)
                throw new ArgumentNullException();

            // We cache previews only for items with 'ID', for now we support only AssetItems
            var assetItem = item as AssetItem;
            if (assetItem == null)
                return;

            lock (_requests)
            {
                // Cancel loading
                RemoveRequest(assetItem);

                // Find atlas with preview and remove it
                for (int i = 0; i < _cache.Count; i++)
                {
                    if (_cache[i].ReleaseSlot(assetItem.ID))
                    {
                        break;
                    }
                }
            }
        }

        #region IContentItemOwner

        /// <inheritdoc />
        void IContentItemOwner.OnItemDeleted(ContentItem item)
        {
            if (item is AssetItem assetItem)
            {
                lock (_requests)
                {
                    RemoveRequest(assetItem);
                }
            }
        }

        /// <inheritdoc />
        void IContentItemOwner.OnItemRenamed(ContentItem item)
        {
        }

        /// <inheritdoc />
        void IContentItemOwner.OnItemDispose(ContentItem item)
        {
            if (item is AssetItem assetItem)
            {
                lock (_requests)
                {
                    RemoveRequest(assetItem);
                }
            }
        }

        #endregion

        /// <inheritdoc />
        public override void OnInit()
        {
            // Create cache folder
            if (!Directory.Exists(_cacheFolder))
            {
                Directory.CreateDirectory(_cacheFolder);
            }

            // Find atlases in a Editor cache directory
            /*var files = Directory.GetFiles(_cacheFolder, "cache_*.flax", SearchOption.TopDirectoryOnly);
            int atlases = 0;
            for (int i = 0; i < files.Length; i++)
            {
                // Load asset
                var asset = FlaxEngine.Content.LoadAsync(files[i]);
                if (asset == null)
                    continue;

                // Validate type
                if (asset is PreviewsCache atlas)
                {
                    // Cache atlas
                    atlases++;
                    _cache.Add(atlas);
                }
                else
                {
                    // Skip asset
                    Debug.LogWarning(string.Format("Asset \'{0}\' is inside Editor\'s private directory for Assets Thumbnails Cache. Please move it.", asset.Name));
                }
            }
            Debug.Log(string.Format("Previews cache count: {0} (capacity for {1} icons)", atlases, atlases * PreviewsCache.AssetIconsPerAtlas));
            */
            // Create render task but disabled for now
            _output = RenderTarget.New();
            _output.Init(PreviewsCache.AssetIconsAtlasFormat, PreviewsCache.AssetIconSize, PreviewsCache.AssetIconSize);
            _task = RenderTask.Create<CustomRenderTask>();
            _task.Order = 50; // Render this task later
            _task.Enabled = false;
            _task.OnRender += OnRender;
        }

        private void OnRender(GPUContext context)
        {
            lock (_requests)
            {
                // Check if there is ready next asset to render thumbnail for it
                // But don't check whole queue, only a few items
                if (!GetReadyItem(1))
                {
                    // Disable task
                    _task.Enabled = false;
                    return;
                }

                // Get asset to refresh
                var request = _requests[0];
                    ..._requests.RemoveAt(0);

                // Setup
                _guiRoot.RemoveChildren();
                _guiRoot.AccentColor = request.Proxy.AccentColor;

                // Call proxy to prepare for thumbnail rendering
                // It can setup preview scene and additional GUI
                proxy.OnThumbnailDrawBegin(item, _guiRoot, context);
                _guiRoot.UnlockChildrenRecursive();

                // Draw preview
                context.Clear(_output, Color.Black);
                Render2D.CallDrawing(context, _output, _guiRoot);

                // Call proxy and cleanup UI (delete create controls, shared controls should be unlinked during OnThumbnailDrawEnd event)
                proxy.OnThumbnailDrawEnd(item, _guiRoot);
                _guiRoot.DisposeChildren();

                // Find atlas with an free slot
                var atlas = GetValidAtlas();
                if (atlas == null)
                {
                    // Error
                    _task.Enabled = false;
                    _requests.Clear();
                    Debug.LogError("Failed to get atlas.");
                    return;
                }

                // Copy backbuffer with rendered preview into atlas
                Sprite icon = atlas.OccupySlot(_output, item.ID);
                if (!icon.IsValid)
                {
                    // Error
                    _task.Enabled = false;
                    _requests.Clear();
                    Debug.LogError("Failed to occupy previews cache atlas slot.");
                    return;
                }

                // Assign new preview icon
                item.Thumbnail = icon;

                Debug.Log("icon " + icon.Index + " -> " + item.Path);
            }
        }

        private bool GetReadyItem(int maxChecks)
        {
            maxChecks = Mathf.Min(maxChecks, _requests.Count);
            for (int i = 0; i < maxChecks; i++)
            {
                // Check if first item is ready
                var request = _requests[i];

                try
                {
                    if (request.CanDrawThumbnail)
                    {
                        // For non frst elements do the swap with keeping order
                        if (i != 0)
                        {
                            _requests.RemoveAt(i);
                            _requests.Insert(0, request);
                        }

                        return true;
                    }
                }
                catch (Exception ex)
                {
                    // Exception thrown during `CanDrawThumbnail` means we cannot render preview for it
                    Debug.LogException(ex);
                    Debug.LogWarning($"Failed to prepare thumbnail rendering for {request.Item.ShortName}.");
                    _requests.RemoveAt(i);
                    i--;
                }
            }

            return false;
        }

        private void StartPreviewsQueue()
        {
            // Ensure to have valid atlas
            GetValidAtlas();

            // Enable task
            _task.Enabled = true;
        }

        #region Requests Management

        private ThumbnailRequest FindRequest(AssetItem item)
        {
            for (int i = 0; i < _requests.Count; i++)
            {
                if (_requests[i].Item == item)
                    return _requests[i];
            }
            return null;
        }

        private void AddRequest(AssetItem item, AssetProxy proxy)
        {
            var request = new ThumbnailRequest(item, proxy);
            _requests.Add(request);
            item.AddReference(this);
        }

        private void RemoveRequest(ThumbnailRequest request)
        {
            request.Dispose();
            _requests.Remove(request);
            request.Item.RemoveReference(this);
        }

        private void RemoveRequest(AssetItem item)
        {
            var request = FindRequest(item);
            if (request != null)
                RemoveRequest(request);
        }

        #endregion

        #region Atlas Management

        private PreviewsCache CreateAtlas()
        {
            // Create atlas path
            var path = Path.Combine(_cacheFolder, string.Format("cache_{0:N}.flax", Guid.NewGuid()));

            // Create atlas
            if (PreviewsCache.Create(path))
            {
                // Error
                Debug.LogError("Failed to create thumbnails atlas.");
                return null;
            }

            // Load atlas
            var atlas = FlaxEngine.Content.Load<PreviewsCache>(path);
            if (atlas == null)
            {
                // Error
                Debug.LogError("Failed to load thumbnails atlas.");
                return null;
            }

            // Register new atlas
            _cache.Add(atlas);

            return atlas;
        }

        private void Flush()
        {
            for (int i = 0; i < _cache.Count; i++)
            {
                _cache[i].Flush();
            }
        }

        private bool HasAllAtlasesLoaded()
        {
            for (int i = 0; i < _cache.Count; i++)
            {
                if (!_cache[i].IsLoaded)
                {
                    return false;
                }
            }
            return true;
        }

        private PreviewsCache GetValidAtlas()
        {
            // Check if has no free slots
            for (int i = 0; i < _cache.Count; i++)
            {
                if (_cache[i].HasFreeSlot)
                {
                    return _cache[i];
                }
            }

            // Create new atlas
            return CreateAtlas();
        }

        #endregion

        /// <inheritdoc />
        public override void OnUpdate()
        {
            // Wait some frames before start generating previews (late init feature)
            if (Time.RealtimeSinceStartup < 1.0f || HasAllAtlasesLoaded() == false)
            {
                // Back
                return;
            }

            lock (_requests)
            {
                // Check if has any request pending
                int count = _requests.Count;
                if (count > 0)
                {
                    // Check if has no rendering task enabled
                    if (_task.Enabled == false)
                    {
                        if (GetReadyItem(count))
                        {
                            // Start generating preview
                            StartPreviewsQueue();
                        }
                    }
                }
                else
                {
                    // Flush data
                    Flush();
                }
            }
        }

        /// <inheritdoc />
        public override void OnExit()
        {
            _task.Enabled = false;

            lock (_requests)
            {
                // Clear data
                while(_requests.Count > 0)
                    RemoveRequest(_requests[0]);
                _cache.Clear();
            }

            _guiRoot.Dispose();
            _task.Dispose();
            Object.Destroy(ref _output);
        }

        private class PreviewRoot : ContainerControl
        {
            /// <summary>
            /// The item accent color to draw.
            /// </summary>
            public Color AccentColor;

            /// <inheritdoc />
            public PreviewRoot()
                : base(false, 0, 0, PreviewsCache.AssetIconSize, PreviewsCache.AssetIconSize)
            {
                AccentColor = Color.Pink;
                IsLayoutLocked = false;
            }

            /// <inheritdoc />
            public override void Draw()
            {
                base.Draw();

                // Draw accent
                const float accentHeight = 2;
                Render2D.FillRectangle(new Rectangle(0, Height - accentHeight, Width, accentHeight), AccentColor);
            }

            /// <inheritdoc />
            protected override void SetSizeInternal(Vector2 size)
            {
                // Cannot change default preview size
            }
        }
    }
}
