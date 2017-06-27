////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using FlaxEditor.Content;
using FlaxEditor.Content.GUI;
using FlaxEditor.GUI;
using FlaxEngine;
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
        private bool _isReady;
        private SplitPanel _split;
        private ContentView _view;

        private ToolStrip _toolStrip;

        private NavigationBar _navigationBar;
        private Tree _tree;

        private ContentTreeNode _root;

        private bool _navigationUnlocked;
        private readonly Stack<ContentTreeNode> _navigationUndo = new Stack<ContentTreeNode>(32);
        private readonly Stack<ContentTreeNode> _navigationRedo = new Stack<ContentTreeNode>(32);

        private NewItem _newElement;
        //private AssetsPreviewManager _previewManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContentWindow"/> class.
        /// </summary>
        /// <param name="editor">The editor.</param>
        public ContentWindow(Editor editor)
            : base(editor, true, ScrollBars.None)
        {
            Title = "Content";

            // Tool strip
            _toolStrip = new ToolStrip();
            _toolStrip.AddButton(0, Editor.UI.GetIcon("Import32"));//.LinkTooltip(GetSharedTooltip(), "Import content");// Import
            _toolStrip.AddSeparator();
            _toolStrip.AddButton(1, Editor.UI.GetIcon("ArrowLeft32"));//.LinkTooltip(GetSharedTooltip(), "Navigate backward");// Backward
            _toolStrip.AddButton(2, Editor.UI.GetIcon("ArrowRight32"));//.LinkTooltip(GetSharedTooltip(), "Navigate forward");// Forward
            _toolStrip.AddButton(3, Editor.UI.GetIcon("ArrowUp32"));//.LinkTooltip(GetSharedTooltip(), "Navigate up");// Up
            _toolStrip.OnButtonClicked += toolstripButtonClicked;
            _toolStrip.Parent = this;

            // Navigation bar
            _navigationBar = new NavigationBar();
            _navigationBar.Height = 32;
            _navigationBar.Parent = this;

            // Split panel
            _split = new SplitPanel(Orientation.Horizontal, ScrollBars.Both, ScrollBars.Vertical);
            _split.DockStyle = DockStyle.Fill;
            _split.SplitterValue = 0.2f;
            _split.Parent = this;

            // Content structure tree
            _tree = new Tree(false);
            _tree.OnSelectedChanged += treeOnSelectedChanged;
            _tree.Parent = _split.Panel1;

            // Content View
            _view = new ContentView();
            // TODO: bind for content view events
            _view.OnOpen += Open;
            _view.OnNavigateBack += NavigateBackward;
            //_view.OnRename.Bind < ContentWindow, &ContentWindow::Rename > (this);
            //_view.OnDelete.Bind < ContentWindow, &ContentWindow::view_OnDelete > (this);
            //_view.OnDuplicate.Bind < ContentWindow, &ContentWindow::CloneSelection > (this);
            _view.Parent = _split.Panel2;
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
        /// Selects the specified item in the content view.
        /// </summary>
        /// <param name="item">The item to select.</param>
        public void Select(ContentItem item)
        {
            if (item == null)
                throw new ArgumentNullException();

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

        private void toolstripButtonClicked(int id)
        {
            switch (id)
            {
                // Import
		        //case 0: import(); break; // TODO: importing

                // Backward
                case 1: NavigateBackward(); break;

                // Forward
                case 2: NavigateForward(); break;

                // Up
                case 3: NavigateUp(); break;
            }
        }

        private void viewOnOpen(ContentItem item)
        {
            //Open(item);
        }

        private void viewOnDelete()
        {
            // Check if has any selected items
            //Delete(_view.Selection);
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
            _toolStrip.GetButton(0).Enabled = folder != null && folder.CanHaveAssets;
            _toolStrip.GetButton(1).Enabled = _navigationUndo.Count > 0;
            _toolStrip.GetButton(2).Enabled = _navigationRedo.Count > 0;
            _toolStrip.GetButton(3).Enabled = folder != null && _tree.SelectedNode != _root;
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
            const bool ShowFlaxFolders = true;

            // Setup content root node
            _root = new ContentTreeNode(null, string.Empty);
            _root.Expand();
            addFolder2Root(Editor.ContentDatabase.ProjectContent);
            addFolder2Root(Editor.ContentDatabase.ProjectSource);
            if (ShowFlaxFolders)
            {
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

            // Mark as ready
            _isReady = true;

            // TODO: load last viewed folder
        }

        /// <inheritdoc />
        public override void OnExit()
        {
            // Enter uneady mode
            _isReady = false;

            // TODO: save last viewed folder

            // Clear view
            _view.ClearItems();
            
            // Unlink used directories
            while (_root.HasChildren)
            {
                removeFolder2Root((MainContentTreeNode)_root.GetChild(0));
            }
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
    }
}
