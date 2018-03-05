////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2018 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using FlaxEditor.Content;
using FlaxEditor.Content.GUI;
using FlaxEditor.GUI;
using FlaxEngine;
using FlaxEngine.Assertions;
using FlaxEngine.GUI;

namespace FlaxEditor.Windows
{
    /// <summary>
    /// One of the main editor windows used to present workspace content and user scripts.
    /// Provides various functionalities for asset operations.
    /// </summary>
    /// <seealso cref="FlaxEditor.Windows.EditorWindow" />
    public sealed partial class ContentWindow : EditorWindow
    {
        private bool _isWorkspaceDirty;
        private SplitPanel _split;
        private ContentView _view;

        private readonly ToolStrip _toolStrip;
	    private readonly ToolStripButton _importButton;
	    private readonly ToolStripButton _navigateBackwardButton;
	    private readonly ToolStripButton _navigateForwardButton;
	    private readonly ToolStripButton _nnavigateUpButton;

        private NavigationBar _navigationBar;
        private Tree _tree;

        private RootContentTreeNode _root;

        private bool _navigationUnlocked;
        private readonly Stack<ContentTreeNode> _navigationUndo = new Stack<ContentTreeNode>(32);
        private readonly Stack<ContentTreeNode> _navigationRedo = new Stack<ContentTreeNode>(32);

        private NewItem _newElement;

		/// <summary>
		/// Gets the toolstrip.
		/// </summary>
		public ToolStrip Toolstrip => _toolStrip;

		/// <summary>
		/// Gets the assets view.
		/// </summary>
		public ContentView View => _view;

		/// <summary>
		/// Initializes a new instance of the <see cref="ContentWindow"/> class.
		/// </summary>
		/// <param name="editor">The editor.</param>
		public ContentWindow(Editor editor)
            : base(editor, true, ScrollBars.None)
        {
            Title = "Content";

            // Content database events
            editor.ContentDatabase.OnWorkspaceModified += () => _isWorkspaceDirty = true;
            editor.ContentDatabase.ItemRemoved += ContentDatabaseOnItemRemoved;

            // Tool strip
            _toolStrip = new ToolStrip();
	        _importButton = (ToolStripButton)_toolStrip.AddButton(Editor.UI.GetIcon("Import32"), () => Editor.ContentImporting.ShowImportFileDialog(CurrentViewFolder)).LinkTooltip("Import content");
            _toolStrip.AddSeparator();
	        _navigateBackwardButton = (ToolStripButton)_toolStrip.AddButton(Editor.UI.GetIcon("ArrowLeft32"), NavigateBackward).LinkTooltip("Navigate backward");
	        _navigateForwardButton = (ToolStripButton)_toolStrip.AddButton(Editor.UI.GetIcon("ArrowRight32"), NavigateForward).LinkTooltip("Navigate forward");
	        _nnavigateUpButton = (ToolStripButton)_toolStrip.AddButton(Editor.UI.GetIcon("ArrowUp32"), NavigateUp).LinkTooltip("Navigate up");
            _toolStrip.Parent = this;

            // Navigation bar
            _navigationBar = new NavigationBar
            {
                Height = 32,
                Parent = this
            };

            // Split panel
            _split = new SplitPanel(Orientation.Horizontal, ScrollBars.Both, ScrollBars.Vertical)
            {
                DockStyle = DockStyle.Fill,
                SplitterValue = 0.2f,
                Parent = this
            };

            // Content structure tree
            _tree = new Tree(false);
            _tree.SelectedChanged += treeOnSelectedChanged;
            _tree.Parent = _split.Panel1;

            // Content View
            _view = new ContentView();
            _view.OnOpen += Open;
            _view.OnNavigateBack += NavigateBackward;
            _view.OnRename += Rename;
            _view.OnDelete += Delete;
            _view.OnDuplicate += Clone;
            _view.Parent = _split.Panel2;
        }

        /// <summary>
        /// Shows popup dialog with UI to rename content item.
        /// </summary>
        /// <param name="item">The item to rename.</param>
        private void Rename(ContentItem item)
        {
            // Show element in view
            Select(item);

            // Show rename popup
            var popup = RenamePopup.Show(item, item.TextRectangle, item.ShortName, true);
            popup.Tag = item;
            popup.Renamed += renamePopup => Rename((ContentItem)renamePopup.Tag, renamePopup.Text);
            popup.Closed += renamePopup =>
                            {
                                // Check if was creating new element
                                if (_newElement != null)
                                {
                                    // Destroy mock control
                                    _newElement.ParentFolder = null;
                                    _newElement.Dispose();
                                    _newElement = null;
                                }
                            };

            // For new asset we want to mock the initial value so user can press just Enter to use default name
            if (_newElement != null)
            {
                popup.InitialValue = "?";
            }
        }

        internal void Rename(ContentItem item, string newShortName)
        {
            if (item == null)
                throw new ArgumentNullException();

            // Check if can rename this item
            if (!item.CanRename)
            {
                // Cannot
                MessageBox.Show("Cannot rename this item.", "Cannot rename", MessageBox.Buttons.OK, MessageBox.Icon.Error);
                return;
            }

            // Check if name is valid
            string hint;
            if (!Editor.ContentEditing.IsValidAssetName(item, newShortName, out hint))
            {
                // Invalid name
                MessageBox.Show("Given asset name is invalid. " + hint,
                    "Invalid name",
                    MessageBox.Buttons.OK,
                    MessageBox.Icon.Error);
                return;
            }

            // Ensure has parent
            if (item.ParentFolder == null)
            {
                // Error
                Editor.LogWarning("Cannot rename root items. " + item.Path);
                return;
            }

            // Cache data
            var extension = Path.GetExtension(item.Path);
            var newPath = StringUtils.CombinePaths(item.ParentFolder.Path, newShortName + extension);

            // Check if was renaming mock element
            // Note: we create `_newElement` and then rename it to create new asset
            var itemFolder = item.ParentFolder;
            if (_newElement == item)
            {
                try
                {
                    // Create new asset
                    var proxy = _newElement.Proxy;
                    Editor.Log(string.Format("Creating asset {0} in {1}", proxy.Name, newPath));
                    proxy.Create(newPath);
                }
                catch (Exception ex)
                {
                    Editor.LogWarning(ex);
                    Editor.LogError("Failed to create asset.");
                }
            }
            else
            {
                // Validate state
                Assert.IsNull(_newElement);

                // Rename asset
                Editor.Log(string.Format("Renaming asset {0} to {1}", item.Path, newShortName));
                Editor.ContentDatabase.Move(item, newPath);
            }

            if (_newElement != null)
            {
                // Destroy mock control
                _newElement.ParentFolder = null;
                _newElement.Dispose();
                _newElement = null;
            }

            // Refresh database and view now
            Editor.ContentDatabase.RefreshFolder(itemFolder, true);
            RefreshView();
        }

        private void Delete(ContentItem item)
        {
            Delete(new List<ContentItem> { item });
        }

        private void Delete(List<ContentItem> items)
        {
            // TODO: remove items that depend on diffrent items in the list: use wants to remove `folderA` and `folderA/asset.x`, we should just remove `folderA`
            var toDelete = new List<ContentItem>(items);

            // Ask user
            if (toDelete.Count == 1)
            {
                // Single item
                if (MessageBox.Show(string.Format("Are you sure to delete \'{0}\'?\nThis action cannot be undone. Files will be deleted permanently.", items[0].Path),
                        "Delete asset(s)",
                        MessageBox.Buttons.OKCancel,
                        MessageBox.Icon.Question)
                    != DialogResult.OK)
                {
                    // Break
                    return;
                }
            }
            else
            {
                // Many items
                if (MessageBox.Show(string.Format("Are you sure to delete {0} selected items?\nThis action cannot be undone. Files will be deleted permanently.", items.Count),
                        "Delete asset(s)",
                        MessageBox.Buttons.OKCancel,
                        MessageBox.Icon.Question)
                    != DialogResult.OK)
                {
                    // Break
                    return;
                }
            }

            // Clear navigation
            // TODO: just remove invalid locations from the history (those are removed)
            NavigationClearHistory();

            // Delete items
            for (int i = 0; i < toDelete.Count; i++)
                Editor.ContentDatabase.Delete(toDelete[i]);

            RefreshView();
        }

        private string GetClonedAssetPath(ContentItem item)
        {
            string sourcePath = item.Path;
            string sourceFolder = Path.GetDirectoryName(sourcePath);
            string destinationPath;
            int i = 0;

            // Find new name for clone
            if (item.IsFolder)
            {
                do
                {
                    destinationPath = StringUtils.CombinePaths(sourceFolder, string.Format("{0} Copy ({1})", item.ShortName, i++));
                } while (Directory.Exists(destinationPath));
            }
            else
            {
                string extension = Path.GetExtension(sourcePath);
                do
                {
                    // TODO: better renaming cloned assets
                    /*// Generate new name
                    Function<bool, const String&> f;
                    f.Bind<ContentWindow, &ContentWindow::isElementNameValid>(this);
                    String name = StringUtils::IncrementNameNumber(el->GetName(), &f);
                    _tmpList = nullptr;*/

                    destinationPath = StringUtils.CombinePaths(sourceFolder, string.Format("{0} Copy ({1}){2}", item.ShortName, i++, extension));

                } while (File.Exists(destinationPath));
            }

            return destinationPath;
        }

        private void Clone(ContentItem item)
        {
            // Skip null
            if (item == null)
                return;

            // TODO: don't allow to duplicate items without ParentFolder - like root items (Content, Source, Engien and Editor dirs)

            // Clone item
            var targetPath = GetClonedAssetPath(item);
            Editor.ContentDatabase.Copy(item, targetPath);

            // Refresh this folder now and try to find duplicated item
            Editor.ContentDatabase.RefreshFolder(item.ParentFolder, true);
            RefreshView();
            var targetItem = item.ParentFolder.FindChild(targetPath);

            // Start renaming it
            if (targetItem != null)
            {
                Rename(targetItem);
            }
        }

        private void Clone(List<ContentItem> items)
        {
            // Skip empty or null case
            if (items == null || items.Count == 0)
                return;

            // TODO: don't allow to duplicate items without ParentFolder - like root items (Content, Source, Engien and Editor dirs)

            // Check if it's just a single item
            if (items.Count == 1)
            {
                Clone(items[0]);
            }
            else
            {
                // TODO: remove items that depend on diffrent items in the list: use wants to remove `folderA` and `folderA/asset.x`, we should just remove `folderA`
                var toDuplicate = new List<ContentItem>(items);

                // Duplicate every item
                for (int i = 0; i < toDuplicate.Count; i++)
                {
                    var item = toDuplicate[i];
                    Editor.ContentDatabase.Copy(item, GetClonedAssetPath(item));
                }
            }
        }

        /// <summary>
        /// Stars creating the folder.
        /// </summary>
        private void NewFolder()
        {
            // Construct path
            var parentFolder = SelectedNode.Folder;
            string destinationPath;
            int i = 0;
            do
            {
                destinationPath = StringUtils.CombinePaths(parentFolder.Path, string.Format("New Folder ({0})", i++));
            } while (Directory.Exists(destinationPath));

            // Create new folder
            Directory.CreateDirectory(destinationPath);

            // Refresh parent folder now and try to find duplicated item
            // Note: we should spawn new items directly, content database should do it to propagate events in a proper way
            Editor.ContentDatabase.RefreshFolder(parentFolder, true);
            RefreshView();
            var targetItem = parentFolder.FindChild(destinationPath);

            // Start renaming it
            if (targetItem != null)
            {
                Rename(targetItem);
            }
        }

        /// <summary>
        /// Starts creating new item.
        /// </summary>
        /// <param name="proxy">The new item proxy.</param>
        private void NewItem(ContentProxy proxy)
        {
            Assert.IsNull(_newElement);

            string proxyName = proxy.Name;
            ContentFolder parentFolder = CurrentViewFolder;
            string parentFolderPath = parentFolder.Path;

            // Create asset name
            string path;
            string extension = '.' + proxy.FileExtension;
            int i = 0;
            do
            {
                path = StringUtils.CombinePaths(parentFolderPath, string.Format("{0} {1}", proxyName, i++) + extension);
            } while (parentFolder.FindChild(path) != null);

            // Create new asset proxy, add to view and rename it
            _newElement = new NewItem(path, proxy);
            _newElement.ParentFolder = parentFolder;
            RefreshView();
            Rename(_newElement);
        }

        private void ContentDatabaseOnItemRemoved(ContentItem contentItem)
        {
            if (contentItem is ContentFolder folder)
            {
                var node = folder.Node;

                // Check if current location contains it as a parent
                if (contentItem.Find(CurrentViewFolder))
                {
                    // Navigate to root to prevent leaks
                    ShowRoot();
                }

                // Check if folder is in navigation
                if (_navigationRedo.Contains(node) || _navigationUndo.Contains(node))
                {
                    // Clear all to prevent leaks
                    NavigationClearHistory();
                }
            }
        }

        /// <summary>
        /// Opens the specified content item.
        /// </summary>
        /// <param name="item">The content item.</param>
        public void Open(ContentItem item)
        {
            if (item == null)
                throw new ArgumentNullException();

            // Check if it's a folder
            if (item.IsFolder)
            {
                // Show folder
                var folder = (ContentFolder)item;
                folder.Node.Expand();
                _tree.Select(folder.Node);
                _view.SelectFirstItem();
                return;
            }

            // Open it
            Editor.ContentEditing.Open(item);
        }

        /// <summary>
        /// Selects the specified asset in the content view.
        /// </summary>
        /// <param name="asset">The asset to select.</param>
        public void Select(Asset asset)
        {
            var item = Editor.ContentDatabase.Find(asset.ID);
            if (item != null)
                Select(item);
        }

        /// <summary>
        /// Selects the specified item in the content view.
        /// </summary>
        /// <param name="item">The item to select.</param>
        public void Select(ContentItem item)
        {
            if (item == null)
                throw new ArgumentNullException();

            if (!_navigationUnlocked)
                return;
            var parent = item.ParentFolder;
            if (parent == null || !parent.Visible)
                return;

            // Ensure that window is visible
            FocusOrShow();

            // Navigate to the parent directory
            Navigate(parent.Node);

            // Select and scroll to cover in view
            _view.Select(item);
            _split.Panel2.ScrollViewTo(item);

            // Focus
            _view.Focus();
        }
		
        private void RefreshView()
        {
            RefreshView(SelectedNode);
        }

        private void RefreshView(ContentTreeNode target)
        {
            if (target == _root)
            {
                // Special case for root folder
                List<ContentItem> items = new List<ContentItem>(8);
                for (int i = 0; i < _root.ChildrenCount; i++)
                {
                    if (_root.GetChild(i) is ContentTreeNode node)
                    {
                        items.Add(node.Folder);
                    }
                }
                _view.ShowItems(items);
            }
            else
            {
                // Show folder contents
                _view.ShowItems(target.Folder.Children);
            }
        }

        private void UpdateUI()
        {
            UpdateToolstrip();
            UpdateNavigationBar();
        }

	    private void UpdateToolstrip()
	    {
		    if (_toolStrip == null)
			    return;

		    // Update buttons
		    var folder = CurrentViewFolder;
		    _importButton.Enabled = folder != null && folder.CanHaveAssets;
		    _navigateBackwardButton.Enabled = _navigationUndo.Count > 0;
		    _navigateForwardButton.Enabled = _navigationRedo.Count > 0;
		    _nnavigateUpButton.Enabled = folder != null && _tree.SelectedNode != _root;
	    }

	    private void addFolder2Root(MainContentTreeNode node)
        {
            // Add to the root
            _root.AddChild(node);
        }

        private void removeFolder2Root(MainContentTreeNode node)
        {
            // Remove from the root
            _root.RemoveChild(node);
        }

        /// <inheritdoc />
        public override void OnInit()
        {
            // Setup content root node
            _root = new RootContentTreeNode();
            _root.Expand();
            addFolder2Root(Editor.ContentDatabase.ProjectContent);
            addFolder2Root(Editor.ContentDatabase.ProjectSource);
            if (Editor.IsDevInstance())
            {
                // Flax internal assets locations
                addFolder2Root(Editor.ContentDatabase.EnginePrivate);
                addFolder2Root(Editor.ContentDatabase.EditorPrivate);
            }
            _tree.AddChild(_root);
            _root.SortChildrenRecursive();

            // Setup navigation
            _navigationUnlocked = true;
            _tree.Select(_root);
            NavigationClearHistory();

            // Update UI layout
            UnlockChildrenRecursive();
            PerformLayout();
            
            // TODO: load last viewed folder
        }

        /// <inheritdoc />
        public override void Update(float deltaTime)
        {
            // Handle workspace modification events but only once per frame
            if (_isWorkspaceDirty)
            {
                _isWorkspaceDirty = false;
                RefreshView();
            }

            base.Update(deltaTime);
        }

        /// <inheritdoc />
        public override void OnExit()
        {
            // TODO: save last viewed folder

            // Clear view
            _view.ClearItems();

            // Unlink used directories
            if (_root != null)
            {
                while (_root.HasChildren)
                {
                    removeFolder2Root((MainContentTreeNode)_root.GetChild(0));
                }
            }
        }

        /// <inheritdoc />
        public override bool OnMouseUp(Vector2 location, MouseButton buttons)
        {
            // Check if it's a right mouse button
            if (buttons == MouseButton.Right)
            {
                // Find control that is under the mouse
                var c = GetChildAtRecursive(location);
                
                if (c is ContentItem item)
                {
                    if (_view.IsSelected(item) == false)
                        _view.Select(item);
                    ShowContextMenuForItem(item, ref location);
                }
                else if (c is ContentView)
                {
                    ShowContextMenuForItem(null, ref location);
                }
                else if (c is ContentTreeNode node)
                {
                    _tree.Select(node);
                    ShowContextMenuForItem(node.Folder, ref location);
                }

                return true;
            }

            return base.OnMouseUp(location, buttons);
        }

        /// <inheritdoc />
        protected override void PerformLayoutSelf()
        {
            base.PerformLayoutSelf();

            // Update navigation panel
            if (_toolStrip != null && _navigationBar != null)
            {
                var lastTiilstripButton = _toolStrip.LastButton;
                var bounds = new Rectangle(
                    new Vector2(lastTiilstripButton.Right + 8.0f, 0),
                    new Vector2(Width - _navigationBar.X - 8.0f, _navigationBar.Height)
                );
                _navigationBar.Bounds = bounds;
            }
        }

	    /// <inheritdoc />
	    public override bool UseLayoutData => true;
		
	    /// <inheritdoc />
	    public override void OnLayoutSerialize(XmlWriter writer)
	    {
		    writer.WriteAttributeString("Split", _split.SplitterValue.ToString());
		    writer.WriteAttributeString("Scale", _view.Scale.ToString());
	    }

	    /// <inheritdoc />
	    public override void OnLayoutDeserialize(XmlElement node)
	    {
		    float value1;

			if (float.TryParse(node.GetAttribute("Split"), out value1))
			    _split.SplitterValue = value1;
		    if (float.TryParse(node.GetAttribute("Scale"), out value1))
			    _view.Scale = value1;
	    }
	    
	    /// <inheritdoc />
	    public override void OnLayoutDeserialize()
	    {
		    _split.SplitterValue = 0.2f;
		    _view.Scale = 1.0f;
	    }
    }
}
