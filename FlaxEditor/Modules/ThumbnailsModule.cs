// Flax Engine scripting API

using System;
using System.Collections.Generic;
using FlaxEditor.Content;

namespace FlaxEditor.Modules
{
    /// <summary>
    /// Manages asset thumbnails rendering and presentation.
    /// </summary>
    /// <seealso cref="FlaxEditor.Modules.EditorModule" />
    public sealed class ThumbnailsModule : EditorModule, IContentItemOwner
    {
        private readonly List<PreviewsCache> _cache = new List<PreviewsCache>();
        private string _cacheFolder;

        private readonly List<ContentItem> _requests = new List<ContentItem>(128);

        internal ThumbnailsModule(Editor editor)
            : base(editor)
        {
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

            // TODO: finish previews atlas interop
            /*lock (_requests)
            {
                // Check if element hasn't been already processed for generating preview
                if (!_requests.Contains(item))
                {
                    // Check each cache atlas
                    // Note: we cache previews only for items with 'ID', for now we support only AssetItems
                    if (item is AssetItem assetItem)
                    {
                        Sprite sprite;
                        for (int i = 0; i < _cache.Count; i++)
                        {
                            if (_cache[i].FindPreview(assetItem.ID, out sprite))
                            {
                                // Found!
                                item.Thumbnail = sprite;
                                return;
                            }
                        }
                    }

                    // Add request
                    item.AddReference(this);
                    _requests.Add(item);
                }
            }*/
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

            // TODO: finish previews atlas interop
            /*lock (_requests)
            {
                // Cancel loading
                _requests.Remove(item);
                item.RemoveReference(this);

                // Find atlas with preview and remove it
                // Note: we cache previews only for items with 'ID', for now we support only AssetItems
                if (item is AssetItem assetItem)
                {
                    for (int i = 0; i < _cache.Count; i++)
                    {
                        if (_cache[i].ReleaseSlot(assetItem.ID))
                        {
                            break;
                        }
                    }
                }
            }*/
        }

        #region IContentItemOwner

        /// <inheritdoc />
        void IContentItemOwner.OnItemDeleted(ContentItem item)
        {
            lock (_requests)
            {
                _requests.Remove(item);
            }
        }

        /// <inheritdoc />
        void IContentItemOwner.OnItemRenamed(ContentItem item)
        {
        }

        /// <inheritdoc />
        void IContentItemOwner.OnItemDispose(ContentItem item)
        {
            lock (_requests)
            {
                _requests.Remove(item);
            }
        }

        #endregion

        /// <inheritdoc />
        public override void OnExit()
        {
            lock (_requests)
            {
                // Clear data
                for (int i = 0; i < _requests.Count; i++)
                {
                    _requests[i].RemoveReference(this);
                }
                _requests.Clear();
                _cache.Clear();
            }
        }
    }
}
