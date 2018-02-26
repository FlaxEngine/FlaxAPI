////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2018 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.IO;
using FlaxEditor.Content;
using FlaxEditor.Content.Settings;
using FlaxEditor.Scripting;
using FlaxEngine;
using FlaxEngine.Assertions;

namespace FlaxEditor.Modules
{
    /// <summary>
    /// Manages assets database and searches for workspace directory changes.
    /// </summary>
    /// <seealso cref="FlaxEditor.Modules.EditorModule" />
    public sealed class ContentDatabaseModule : EditorModule
    {
        private bool _enableEvents;
        private bool _isDuringFastSetup;
        private int _itemsCreated;
        private int _itemsDeleted;
        private readonly Queue<MainContentTreeNode> _dirtyNodes = new Queue<MainContentTreeNode>();

        /// <summary>
        /// The project content directory.
        /// </summary>
        public MainContentTreeNode ProjectContent { get; private set; }

        /// <summary>
        /// The project source code directory.
        /// </summary>
        public MainContentTreeNode ProjectSource { get; private set; }

        /// <summary>
        /// The engine private content directory.
        /// </summary>
        public MainContentTreeNode EnginePrivate { get; private set; }

        /// <summary>
        /// The editor private content directory.
        /// </summary>
        public MainContentTreeNode EditorPrivate { get; private set; }

        /// <summary>
        /// The list with all content items proxy objects.
        /// </summary>
        public readonly List<ContentProxy> Proxy = new List<ContentProxy>(32);

        /// <summary>
        /// Occurs when new items is added to the workspace content database.
        /// </summary>
        public event Action<ContentItem> ItemAdded;

        /// <summary>
        /// Occurs when new items is removed from the workspace content database.
        /// </summary>
        public event Action<ContentItem> ItemRemoved;

        /// <summary>
        /// Occurs when workspace has been modified.
        /// </summary>
        public event Action OnWorkspaceModified;

        /// <summary>
        /// Gets the amount of created items.
        /// </summary>
        public int ItemsCreated => _itemsCreated;

        /// <summary>
        /// Gets the amount of deleted items.
        /// </summary>
        public int ItemsDeleted => _itemsDeleted;

        internal ContentDatabaseModule(Editor editor)
            : base(editor)
        {
            // Init content database after UI module
            InitOrder = -80;

            // Register AssetItems serialization helper (serialize ref ID only)
            FlaxEngine.Json.JsonSerializer.Settings.Converters.Add(new AssetItemConverter());
        }

        private void Content_OnAssetDisposing(Asset asset)
        {
            var item = Find(asset.ID);
            if (item != null)
            {
                Editor.Windows.CloseAllEditors(item);

                // Dispose
                item.Dispose();
            }
        }

        /// <summary>
        /// Gets the proxy object for the given content item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>Content proxy for that item or null if cannot find.</returns>
        public ContentProxy GetProxy(ContentItem item)
        {
            if (item != null)
            {
                for (int i = 0; i < Proxy.Count; i++)
                {
                    if (Proxy[i].IsProxyFor(item))
                    {
                        return Proxy[i];
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Gets the proxy object for the given file extension. Warning! Diffrent asset types may share the same file extension.
        /// </summary>
        /// <param name="extension">The file extension.</param>
        /// <returns>Content proxy for that item or null if cannot find.</returns>
        public ContentProxy GetProxy(string extension)
        {
            if (string.IsNullOrEmpty(extension))
                throw new ArgumentNullException();

            extension = StringUtils.NormalizeExtension(extension);

            for (int i = 0; i < Proxy.Count; i++)
            {
                if (Proxy[i].FileExtension == extension)
                {
                    return Proxy[i];
                }
            }

            return null;
        }

        /// <summary>
        /// Gets the proxy object for the given asset type id.
        /// </summary>
        /// <param name="typeName">The asset type name.</param>
        /// <param name="path">The asset path.</param>
        /// <returns>Asset proxy or null if cannot find.</returns>
        public AssetProxy GetAssetProxy(string typeName, string path)
        {
            for (int i = 0; i < Proxy.Count; i++)
            {
                if (Proxy[i] is AssetProxy proxy && proxy.AcceptsAsset(typeName, path))
                {
                    return proxy;
                }
            }

            return null;
        }

        /// <summary>
        /// Refreshes the given item folder. Tries to find new content items and remove not existing ones.
        /// </summary>
        /// <param name="item">Folder to refresh</param>
        /// <param name="checkSubDirs">True if search for changes inside a subdirectories, otherwise only top-most folder will be updated</param>
        public void RefreshFolder(ContentItem item, bool checkSubDirs)
        {
            // Peek folder to refresh
            ContentFolder folder = item.IsFolder ? item as ContentFolder : item.ParentFolder;
            if (folder == null)
                return;

            // Update
            loadFolder(folder.Node, checkSubDirs);
        }

        /// <summary>
        /// Tries to find item at the specified path.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>Found item or null if cannot find it.</returns>
        public ContentItem Find(string path)
        {
            Assert.IsFalse(_isDuringFastSetup);

            // Ensure path is normalized to the Flax format
            path = StringUtils.NormalizePath(path);
            
            // TODO: if it's a bottleneck try to optimize searching by spliting path

            var result = ProjectContent.Folder.Find(path);
            if (result != null)
                return result;
            result = ProjectSource.Folder.Find(path);
            if (result != null)
                return result;
            result = EnginePrivate.Folder.Find(path);
            if (result != null)
                return result;
            result = EditorPrivate.Folder.Find(path);

            return result;
        }

        /// <summary>
        /// Tries to find item with the specified ID.
        /// </summary>
        /// <param name="id">The item ID.</param>
        /// <returns>Found item or null if cannot find it.</returns>
        public ContentItem Find(Guid id)
        {
            Assert.IsFalse(_isDuringFastSetup);

            if (id == Guid.Empty)
                return null;

            // TODO: use AssetInfo via Content manager to get asset path very quickly (it's O(1))

            // TODO: if it's a bottleneck try to optimize searching by caching items IDs

            var result = ProjectContent.Folder.Find(id);
            if (result != null)
                return result;
            result = ProjectSource.Folder.Find(id);
            if (result != null)
                return result;
            result = EnginePrivate.Folder.Find(id);
            if (result != null)
                return result;
            result = EditorPrivate.Folder.Find(id);

            return result;
        }

        /// <summary>
        /// Tries to find script item at the specified path.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>Found script or null if cannot find it.</returns>
        public ScriptItem FindScript(string path)
        {
            return ProjectSource.Folder.Find(path) as ScriptItem;
        }

        /// <summary>
        /// Tries to find script item with the specified ID.
        /// </summary>
        /// <param name="id">The item ID.</param>
        /// <returns>Found script or null if cannot find it.</returns>
        public ScriptItem FindScript(Guid id)
        {
            if (id == Guid.Empty)
                return null;

            return ProjectSource.Folder.Find(id) as ScriptItem;
        }

        /// <summary>
        /// Tries to find script item with the specified name.
        /// </summary>
        /// <param name="scriptName">The name of the script.</param>
        /// <returns>Found script or null if cannot find it.</returns>
        public ScriptItem FindScriptWitScriptName(string scriptName)
        {
            return ProjectSource.Folder.FindScriptWitScriptName(scriptName);
        }

        /// <summary>
        /// Tries to find script item that is used by the specified script object.
        /// </summary>
        /// <param name="script">The instance of the script.</param>
        /// <returns>Found script or null if cannot find it.</returns>
        public ScriptItem FindScriptWitScriptName(Script script)
        {
            if (script)
            {
                var className = script.GetType().Name;
                var scriptName = ScriptItem.CreateScriptName(className);
                return FindScriptWitScriptName(scriptName);
            }

            return null;
        }

        private static void RenameAsset(ContentItem el, ref string newPath)
        {
            string oldPath = el.Path;

            // Check if use content pool
            if (el.IsAsset)
            {
                // Rename asset
                // Note: we use content backend because file may be in use or sth, it's safe
                if (FlaxEngine.Content.RenameAsset(oldPath, newPath))
                {
                    // Error
                    Editor.LogError(string.Format("Cannot rename asset \'{0}\' to \'{1}\'", oldPath, newPath));
                    return;
                }
            }
            else
            {
                // Rename file
                try
                {
                    File.Move(oldPath, newPath);
                }
                catch (Exception ex)
                {
                    // Error
                    Editor.LogWarning(ex);
                    Editor.LogError(string.Format("Cannot rename asset \'{0}\' to \'{1}\'", oldPath, newPath));
                    return;
                }
            }

            // Change path
            el.UpdatePath(newPath);
        }

        private static void UpdateAssetNewNameTree(ContentItem el)
        {
            string extension = Path.GetExtension(el.Path);
            string newPath = StringUtils.CombinePaths(el.ParentFolder.Path, el.ShortName + extension);

            // Special case for folders
            if (el.IsFolder)
            {
                // Cache data
                string oldPath = el.Path;
                var folder = (ContentFolder)el;

                // Create new folder
                try
                {
                    Directory.CreateDirectory(newPath);
                }
                catch (Exception ex)
                {
                    // Error
                    Editor.LogWarning(ex);
                    Editor.LogError(string.Format("Cannot move folder \'{0}\' to \'{1}\'", oldPath, newPath));
                    return;
                }

                // Change path
                el.UpdatePath(newPath);

                // Rename all child elements
                for (int i = 0; i < folder.Children.Count; i++)
                    UpdateAssetNewNameTree(folder.Children[i]);
            }
            else
            {
                RenameAsset(el, ref newPath);
            }
        }

        /// <summary>
        /// Moves the specified items to the diffrent location. Handles moving whole directories and single assets.
        /// </summary>
        /// <param name="items">The items.</param>
        /// <param name="newParent">The new parent.</param>

        public void Move(List<ContentItem> items, ContentFolder newParent)
        {
            for (int i = 0; i < items.Count; i++)
                Move(items[i], newParent);
        }

        /// <summary>
        /// Moves the specified item to the diffrent location. Handles moving whole directories and single assets.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="newParent">The new parent.</param>
        public void Move(ContentItem item, ContentFolder newParent)
        {
            if (newParent == null || item == null)
                throw new ArgumentNullException();
            
            // Skip nothing to change
            if (item.ParentFolder == newParent)
                return;

            var extension = Path.GetExtension(item.Path);
            var newPath = StringUtils.CombinePaths(newParent.Path, item.ShortName + extension);
            Move(item, newPath);
        }

        /// <summary>
        /// Moves the specified item to the diffrent location. Handles moving whole directories and single assets.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="newPath">The new path.</param>
        public void Move(ContentItem item, string newPath)
        {
            if (item == null || string.IsNullOrEmpty(newPath))
                throw new ArgumentNullException();

            if (item.IsFolder && Directory.Exists(newPath))
            {
                // Error
                MessageBox.Show("Cannot move folder. Target location already exists.");
                return;
            }
            if (!item.IsFolder && File.Exists(newPath))
            {
                // Error
                MessageBox.Show("Cannot move file. Target location already exists.");
                return;
            }

            // Find target parent
            var newDirPath = Path.GetDirectoryName(newPath);
            var newParent = Find(newDirPath) as ContentFolder;
            if (newParent == null)
            {
                // Error
                MessageBox.Show("Cannot move item. Missing target location.");
                return;
            }

            // Perform renaming
            {
                string oldPath = item.Path;

                // Special case for folders
                if (item.IsFolder)
                {
                    // Cache data
                    var folder = (ContentFolder)item;

                    // Create new folder
                    try
                    {
                        Directory.CreateDirectory(newPath);
                    }
                    catch (Exception ex)
                    {
                        // Error
                        Editor.LogWarning(ex);
                        Editor.LogError(string.Format("Cannot move folder \'{0}\' to \'{1}\'", oldPath, newPath));
                        return;
                    }

                    // Change path
                    item.UpdatePath(newPath);

                    // Rename all child elements
                    for (int i = 0; i < folder.Children.Count; i++)
                        UpdateAssetNewNameTree(folder.Children[i]);

                    // Delete old folder
                    try
                    {
                        Directory.Delete(oldPath, true);
                    }
                    catch (Exception ex)
                    {
                        // Error
                        Editor.LogWarning(ex);
                        Editor.LogWarning(string.Format("Cannot remove folder \'{0}\'", oldPath));
                        return;
                    }
                }
                else
                {
                    RenameAsset(item, ref newPath);
                }

                if (item.ParentFolder != null)
                    item.ParentFolder.Node.SortChildren();
            }

            // Link item
            item.ParentFolder = newParent;

            if (_enableEvents)
                OnWorkspaceModified?.Invoke();
        }

        /// <summary>
        /// Copies the specified item to the target location. Handles copying whole directories and single assets.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="targetPath">The target item path.</param>
        public void Copy(ContentItem item, string targetPath)
        {
            if (item == null || !item.Exists)
            {
                // Error
                MessageBox.Show("Cannot move item. It's missing.");
                return;
            }

            // Perform copy
            {
                string sourcePath = item.Path;

                // Special case for folders
                if (item.IsFolder)
                {
                    // Cache data
                    var folder = (ContentFolder)item;

                    // Create new folder if missing
                    if (!Directory.Exists(targetPath))
                    {
                        try
                        {
                            Directory.CreateDirectory(targetPath);
                        }
                        catch (Exception ex)
                        {
                            // Error
                            Editor.LogWarning(ex);
                            Editor.LogError(string.Format("Cannot copy folder \'{0}\' to \'{1}\'", sourcePath, targetPath));
                            return;
                        }
                    }

                    // Copy all child elements
                    for (int i = 0; i < folder.Children.Count; i++)
                    {
                        var child = folder.Children[i];
                        var childExtension = Path.GetExtension(child.Path);
                        var childTargetPath = StringUtils.CombinePaths(targetPath, child.ShortName + childExtension);
                        Copy(folder.Children[i], childTargetPath);
                    }
                }
                else
                {
                    // Check if use content pool
                    if (item.IsAsset || item.ItemType == ContentItemType.Scene)
                    {
                        // Rename asset
                        // Note: we use content backend because file may be in use or sth, it's safe
                        if (Editor.ContentEditing.CloneAssetFile(targetPath, sourcePath, Guid.NewGuid()))
                        {
                            // Error
                            Editor.LogError(string.Format("Cannot copy asset \'{0}\' to \'{1}\'", sourcePath, targetPath));
                            return;
                        }
                    }
                    else
                    {
                        // Copy file
                        try
                        {
                            File.Copy(sourcePath, targetPath, true);
                        }
                        catch (Exception ex)
                        {
                            // Error
                            Editor.LogWarning(ex);
                            Editor.LogError(string.Format("Cannot copy asset \'{0}\' to \'{1}\'", sourcePath, targetPath));
                            return;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Deletes the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        public void Delete(ContentItem item)
        {
            if (item == null)
                throw new ArgumentNullException();

            // Fire events
            if (_enableEvents)
                ItemRemoved?.Invoke(item);
            item.OnDelete();
            _itemsDeleted++;

            var path = item.Path;

            // Special case for folders
            if (item is ContentFolder folder)
            {
                // TODO: maybe dont' remove folders reqursive but at once?

                // Delete all children
	            if (folder.Children.Count > 0)
	            {
		            var children = folder.Children.ToArray();
		            for (int i = 0; i < children.Length; i++)
		            {
			            Delete(children[0]);
		            }
	            }

	            // Remove directory
                if (Directory.Exists(path))
                {
                    try
                    {
                        Directory.Delete(path, true);
                    }
                    catch (Exception ex)
                    {
                        // Error
                        Editor.LogWarning(ex);
                        Editor.LogWarning(string.Format("Cannot remove folder \'{0}\'", path));
                        return;
                    }
                }

                // Unlink from the parent
                item.ParentFolder = null;

                // Delete tree node
                folder.Node.Dispose();
            }
            else
            {
                // Check if it's an asset
                if (item.IsAsset)
                {
                    // Delete asset by using content pool
                    FlaxEngine.Content.DeleteAsset(path);
                }
                else
                {
                    // Delete file
                    if (File.Exists(path))
                        File.Delete(path);
                }

                // Unlink from the parent
                item.ParentFolder = null;

                // Delete item
                item.Dispose();
            }

            if (_enableEvents)
                OnWorkspaceModified?.Invoke();
        }

        private void loadFolder(ContentTreeNode node, bool checkSubDirs)
        {
            if (node == null)
                return;

            // Temporary data
            var folder = node.Folder;
            var path = folder.Path;

            // Check for missing files/folders (skip it during fast tree setup)
            if (!_isDuringFastSetup)
            {
                for (int i = 0; i < folder.Children.Count; i++)
                {
                    var child = folder.Children[i];
                    if (!child.Exists)
                    {
                        // Send info
                        Editor.Log(string.Format($"Content item \'{child.Path}\' has been removed"));

                        // Destroy it
                        Delete(child);

                        i--;
                    }
                }
            }

            // Find elements (use separate path for scripts and assets - perf stuff)
            // TODO: we could make it more modular
            if (node.CanHaveAssets)
            {
                LoadAssets(node, path);
            }
            if (node.CanHaveScripts)
            {
                LoadScripts(node, path);
            }

            // Get child directories
            var childFolders = Directory.GetDirectories(path);

            // Load child folders
            bool sortChildren = false;
            for (int i = 0; i < childFolders.Length; i++)
            {
                var childPath = StringUtils.NormalizePath(childFolders[i]);

                // Check if node already has that element (skip during init when we want to walk project dir very fast)
                ContentFolder childFolderNode = _isDuringFastSetup ? null : node.Folder.FindChild(childPath) as ContentFolder;
                if (childFolderNode == null)
                {
                    // Create node
                    ContentTreeNode n = new ContentTreeNode(node, childPath);
                    if (!_isDuringFastSetup)
                        sortChildren = true;

                    // Load child folder
                    loadFolder(n, true);

                    // Fire event
                    if (_enableEvents)
                    {
                        ItemAdded?.Invoke(n.Folder);
                        OnWorkspaceModified?.Invoke();
                    }
                    _itemsCreated++;
                }
                else if (checkSubDirs)
                {
                    // Update child folder
                    loadFolder(childFolderNode.Node, true);
                }
            }
            if (sortChildren)
                node.SortChildren();
        }

        private void LoadScripts(ContentTreeNode parent, string directory)
        {
            // Find files
            var files = Directory.GetFiles(directory, ScriptProxy.ExtensionFiler, SearchOption.TopDirectoryOnly);

            // Add them
            bool anyAdded = false;
            for (int i = 0; i < files.Length; i++)
            {
                var path = StringUtils.NormalizePath(files[i]);

                // Check if node already has that element (skip during init when we want to walk project dir very fast)
                if (_isDuringFastSetup || !parent.Folder.ContainsChild(path))
                {
                    // Create item object
                    var item = new ScriptItem(path);

                    // Link
                    item.ParentFolder = parent.Folder;

                    // Fire event
                    if (_enableEvents)
                    {
                        ItemAdded?.Invoke(item);
                        OnWorkspaceModified?.Invoke();
                    }
                    _itemsCreated++;
                    anyAdded = true;
                }
            }

            if (anyAdded && _enableEvents)
                ScriptsBuilder.MarkWorkspaceDirty();
        }

        private void LoadAssets(ContentTreeNode parent, string directory)
        {
            // Find files
            var files = Directory.GetFiles(directory, "*.*", SearchOption.TopDirectoryOnly);

            // Add them
            for (int i = 0; i < files.Length; i++)
            {
                var path = StringUtils.NormalizePath(files[i]);

                // Check if node already has that element (skip during init when we want to walk project dir very fast)
                if (_isDuringFastSetup || !parent.Folder.ContainsChild(path))
                {
                    // It can be any type of asset: binary, text, cooked, package, etc.
                    // The best idea is to just ask Flax.
                    // Flax isn't John Snow. Flax knows something :)
                    // Also Flax Content Layer is using smart caching so this query gonna be fast.

                    string typeName;
                    Guid id;
                    if (FlaxEngine.Content.GetAssetInfo(path, out typeName, out id))
                    {
                        var proxy = GetAssetProxy(typeName, path);
                        var item = proxy?.ConstructItem(path, typeName, ref id);

                        if (item != null)
                        {
                            // Link
                            item.ParentFolder = parent.Folder;

                            // Fire event
                            if (_enableEvents)
                            {
                                ItemAdded?.Invoke(item);
                                OnWorkspaceModified?.Invoke();
                            }
                            _itemsCreated++;
                        }
                    }
                }
            }
        }

        /// <inheritdoc />
        public override void OnInit()
        {
            FlaxEngine.Content.AssetDisposing += Content_OnAssetDisposing;

            // Setup content proxies
            Proxy.Add(new TextureProxy());
            Proxy.Add(new ModelProxy());
            Proxy.Add(new MaterialProxy());
            Proxy.Add(new MaterialInstanceProxy());
            Proxy.Add(new SpriteAtlasProxy());
            Proxy.Add(new CubeTextureProxy());
            Proxy.Add(new PreviewsCacheProxy());
            Proxy.Add(new FontProxy());
            Proxy.Add(new ScriptProxy());
            Proxy.Add(new SceneProxy());
            Proxy.Add(new IESProfileProxy());
            Proxy.Add(new CollisionDataProxy());
            Proxy.Add(new AudioClipProxy());
            Proxy.Add(new SpawnableJsonAssetProxy<PhysicalMaterial>());
            
            // Settings
            Proxy.Add(new SettingsProxy<GameSettings>());
            Proxy.Add(new SettingsProxy<TimeSettings>());
            Proxy.Add(new SettingsProxy<LayersAndTagsSettings>());
            Proxy.Add(new SettingsProxy<PhysicsSettings>());
            Proxy.Add(new SettingsProxy<GraphicsSettings>());
            Proxy.Add(new SettingsProxy<BuildSettings>());
            Proxy.Add(new SettingsProxy<InputSettings>());
            Proxy.Add(new SettingsProxy<WindowsPlatformSettings>());
            Proxy.Add(new SettingsProxy<UWPPlatformSettings>());
            Proxy.Add(new SettingsProxy<AudioSettings>());
            
            // Last add generic json (won't override other json proxies)
            Proxy.Add(new GenericJsonAssetProxy());

            // Create content folders nodes
            ProjectContent = new MainContentTreeNode(ContentFolderType.Content, Globals.ContentFolder);
            ProjectSource = new MainContentTreeNode(ContentFolderType.Source, Globals.SourceFolder);
            EnginePrivate = new MainContentTreeNode(ContentFolderType.Editor, Globals.EngineFolder);
            EditorPrivate = new MainContentTreeNode(ContentFolderType.Engine, Globals.EditorFolder);

            // Load all folders
            // TODO: we should create async task for gathering content and whole workspace contents if it takes too long
            // TODO: create progress bar in content window and after end we should enable events and update it
            _isDuringFastSetup = true;
            loadFolder(ProjectContent, true);
            loadFolder(ProjectSource, true);
            loadFolder(EnginePrivate, true);
            loadFolder(EditorPrivate, true);
            _isDuringFastSetup = false;

            // Enable events
            _enableEvents = true;
            Editor.ContentImporting.ImportFileEnd += ContentImporting_ImportFileDone;

            Editor.Log("Project database created. Items count: " + _itemsCreated);
        }

        private void ContentImporting_ImportFileDone(IFileEntryAction obj, bool failed)
        {
            if (failed)
                return;

            // Check if already has that element
            var item = Find(obj.ResultUrl);
            if (item is BinaryAssetItem binaryAssetItem)
            {
                // Get asset info from the registry (content layer will update cache it just after import)
                string typeName;
                Guid id;
                if (FlaxEngine.Content.GetAssetInfo(binaryAssetItem.Path, out typeName, out id))
                {
                    // If asset type id has been changed we HAVE TO close all windows that use it
                    // For eg. change texture to sprite atlas on reimport
                    if (binaryAssetItem.TypeName != typeName)
                    {
                        // Asset type has been changed!
                        Editor.LogWarning(string.Format("Asset \'{0}\' changed type from {1} to {2}", item.Path, binaryAssetItem.TypeName, typeName));
                        Editor.Windows.CloseAllEditors(item);

                        // Remove this item from the database and call refresh
                        var toRefresh = binaryAssetItem.ParentFolder;
                        binaryAssetItem.Dispose();
                        RefreshFolder(toRefresh, false);
                    }
                    else
                    {
                        // Refresh element data that could change during importing
                        binaryAssetItem.OnReimport(ref id);
                    }
                }
            }
        }

        internal void OnDirectoryEvent(MainContentTreeNode node, FileSystemEventArgs e)
        {
            // Ensure to be ready for external events
            if (_isDuringFastSetup)
                return;

            // TODO: maybe we could make it faster! since we have a path so it would be easy to just create or delete given file
            // TODO: but remember about subdirectories!

            // Switch type
            switch (e.ChangeType)
            {
                case WatcherChangeTypes.Created:
                case WatcherChangeTypes.Deleted:
                {
                    // We want to enqueue dir modification events for better stability
                    if (!_dirtyNodes.Contains(node))
                        _dirtyNodes.Enqueue(node);

                    break;
                }

                default: break;
            }
        }

        /// <inheritdoc />
        public override void OnUpdate()
        {
            while (_dirtyNodes.Count > 0)
            {
                // Get node
                var node = _dirtyNodes.Dequeue();

                // Refresh
                loadFolder(node, true);

                // Fire event
                if (_enableEvents)
                    OnWorkspaceModified?.Invoke();
            }
        }

        /// <inheritdoc />
        public override void OnExit()
        {
            FlaxEngine.Content.AssetDisposing -= Content_OnAssetDisposing;

            // Disable events
            _enableEvents = false;

            // Cleanup
            Proxy.ForEach(x => x.Dispose());
            if (ProjectContent != null)
            {
                ProjectContent.Dispose();
                ProjectContent = null;
            }
            if (ProjectSource != null)
            {
                ProjectSource.Dispose();
                ProjectSource = null;
            }
            if (EnginePrivate != null)
            {
                EnginePrivate.Dispose();
                EnginePrivate = null;
            }
            if (EditorPrivate != null)
            {
                EditorPrivate.Dispose();
                EditorPrivate = null;
            }
            Proxy.Clear();
        }
    }
}
