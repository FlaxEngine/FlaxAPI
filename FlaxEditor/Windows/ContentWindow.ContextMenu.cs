////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using FlaxEditor.Content;
using FlaxEngine;
using FlaxEngine.Assertions;
using FlaxEngine.GUI;

namespace FlaxEditor.Windows
{
    public partial class ContentWindow
    {
        private const int CM_SPAWN_BUTTON_ID_START = 100;

        private void ShowContextMenuForItem(ContentItem item, ref Vector2 location)
        {
            // TODO: verify this logic during elements searching

            Assert.IsNull(_newElement);

            // Cache data
            bool isValidElement = item != null;
            var proxy = Editor.ContentDatabase.GetProxy(item);
            ContentFolder folder = null;
            bool isFolder = false;
            if (isValidElement)
            {
                isFolder = item.IsFolder;
                folder = isFolder ? (ContentFolder)item : item.ParentFolder;
            }
            else
            {
                folder = CurrentViewFolder;
            }
            Assert.IsNotNull(folder);

            // Create context menu
            ContextMenuButton b;
            ContextMenuChildMenu c;
            ContextMenu cm = new ContextMenu();
            cm.Tag = item;
            cm.OnButtonClicked += OnItemCMButtonClicked;
            if (isValidElement)
            {
                b = cm.AddButton(0, "Open");
                b.Enabled = proxy != null || isFolder;

                cm.AddButton(14, "Show in explorer");

                if (item.HasDefaultThumbnail == false)
                {
                    cm.AddButton(1, "Refresh preview");
                }

                if (!isFolder)
                {
                    b = cm.AddButton(2, "Reimport");
                    b.Enabled = proxy != null && proxy.CanReimport(item) && item.IsAsset;

                    b = cm.AddButton(15, "Reimport all");

                    if (item is BinaryAssetItem binaryAsset)
                    {
                        string importPath;
                        if (!binaryAsset.GetImportPath(out importPath))
                        {
                            string importLocation = System.IO.Path.GetDirectoryName(importPath);
                            if (!string.IsNullOrEmpty(importLocation) && System.IO.Directory.Exists(importLocation))
                            {
                                b = cm.AddButton(8, "Show import location");
                            }
                        }
                    }
                }

                cm.AddButton(3, "Delete");

                cm.AddSeparator();

                b = cm.AddButton(4, "Export");
                b.Enabled = proxy != null && proxy.CanExport;

                b = cm.AddButton(5, "Clone");
                b.Enabled = !isFolder;

                b = cm.AddButton(6, "Rename");

                cm.AddButton(7, "Copy name to Clipboard");

                cm.AddButton(17, "Copy path to Clipboard");
            }
            else
            {
                cm.AddButton(14, "Show in explorer");

                b = cm.AddButton(13, "Refresh");

                b = cm.AddButton(15, "Reimport all");

                b = cm.AddButton(16, "Refresh all previews");
            }

            cm.AddSeparator();

            cm.AddButton(10, "New folder");

            c = cm.AddChildMenu("New");
            c.ContextMenu.Tag = item;
            c.ContextMenu.OnButtonClicked += OnItemCMButtonClicked;
            for (int i = 0; i < Editor.ContentDatabase.Proxy.Count; i++)
            {
                var p = Editor.ContentDatabase.Proxy[i];
                if (p.CanCreate(folder))
                {
                    c.ContextMenu.AddButton(CM_SPAWN_BUTTON_ID_START + i, p.Name);
                }
            }
            c.Enabled = c.ContextMenu.HasChildren;

            if (folder.CanHaveAssets)
            {
                cm.AddButton(12, "Import file");
            }

            // Show it
            cm.Show(this, location);
        }

        private void OnItemCMButtonClicked(int id, ContextMenu contextMenu)
        {
            ContentItem item = (ContentItem)contextMenu.Tag;
            ContentFolder currentFolder = CurrentViewFolder;

            switch (id)
            {
                case 0:
                    Open(item);
                    break;
                case 1:
                    item.RefreshThumbnail();
                    break;
                case 2:
                    Assert.IsTrue(item.IsAsset);
                    //Reimport((AssetElement*)el, null); // TODO: reimport asset
                    break;
                case 3:
                    Delete(item);
                    break;
                case 4:
                    //Export(el); // TODO: export asset
                    break;
                case 5:
                    _view.DuplicateSelection();
                    break;
                case 6:
                    Rename(item);
                    break;
                case 7:
                    Application.ClipboardText = item.NamePath;
                    break;
                case 8:
                    if (item is BinaryAssetItem binaryAsset)
                    {
                        string importPath;
                        if (!binaryAsset.GetImportPath(out importPath))
                        {
                            string importLocation = System.IO.Path.GetDirectoryName(importPath);
                            // TODO: show folder in explorer
                        }
                    }
                    break;
                case 10:
                    //newFolder(); // TODO: new folder
                    break;
                case 12:
                    _view.ClearSelection();
                    //import(); // TODO: import file
                    break;
                case 13:
                    Editor.ContentDatabase.RefreshFolder(item ?? currentFolder, true);
                    break;
                case 14:
                    // TODO: show asset in folder
                    //Application::StartProcess(*(el ? StringUtils::GetDirectoryName(item.GetPath()) : currentFolder.GetPath()));
                    break;
                case 15:
                    //ReimportViewAll(); // TODO: reimport assets in a view
                    break;
                case 16:
                    //RefreshPreviewViewAll(); // TODO: refresh thumbnails for assets in view
                    break;
                case 17:
                    Application.ClipboardText = item.Path;
                    break;
            }

            // New asset creation
            if (id >= CM_SPAWN_BUTTON_ID_START)
            {
                // Cache data
                int proxyIndex = id - CM_SPAWN_BUTTON_ID_START;
                var proxy = Editor.ContentDatabase.Proxy[proxyIndex];
                //newAsset(proxy, item); // TODO: create new asset
            }
        }
    }
}
