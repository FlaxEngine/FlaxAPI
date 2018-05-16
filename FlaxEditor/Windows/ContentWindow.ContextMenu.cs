// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using FlaxEditor.Content;
using FlaxEngine;
using FlaxEngine.Assertions;
using FlaxEngine.GUI;

namespace FlaxEditor.Windows
{
    public partial class ContentWindow
    {
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
	        bool isRootFolder = CurrentViewFolder == _root.Folder;

            // Create context menu
            ContextMenuButton b;
            ContextMenuChildMenu c;
            ContextMenu cm = new ContextMenu();
            cm.Tag = item;
            if (isValidElement)
            {
                b = cm.AddButton("Open", () => Open(item));
                b.Enabled = proxy != null || isFolder;

                cm.AddButton("Show in explorer", () => Application.StartProcess(System.IO.Path.GetDirectoryName(item.Path)));

                if (item.HasDefaultThumbnail == false)
                {
                    cm.AddButton("Refresh thumbnail", item.RefreshThumbnail);
                }

                if (!isFolder)
                {
                    b = cm.AddButton("Reimport", ReimportSelection);
                    b.Enabled = proxy != null && proxy.CanReimport(item);
                    
                    if (item is BinaryAssetItem binaryAsset)
                    {
                        string importPath;
                        if (!binaryAsset.GetImportPath(out importPath))
                        {
                            string importLocation = System.IO.Path.GetDirectoryName(importPath);
                            if (!string.IsNullOrEmpty(importLocation) && System.IO.Directory.Exists(importLocation))
                            {
	                            cm.AddButton("Show import location", () => Application.StartProcess(importLocation));
                            }
                        }
                    }
                }

                cm.AddButton("Delete", () => Delete(item));

                cm.AddSeparator();

                // TODO: exportig assets
                //b = cm.AddButton(4, "Export");
                //b.Enabled = proxy != null && proxy.CanExport;

                b = cm.AddButton("Clone", _view.DuplicateSelection);
                b.Enabled = !isFolder;

	            cm.AddButton("Rename", () => Rename(item));

                cm.AddButton("Copy name to Clipboard", () => Application.ClipboardText = item.NamePath);

                cm.AddButton("Copy path to Clipboard", () => Application.ClipboardText = item.Path);
            }
	        else
	        {
		        cm.AddButton("Show in explorer", () => Application.StartProcess(CurrentViewFolder.Path));

		        cm.AddButton("Refresh", () => Editor.ContentDatabase.RefreshFolder(CurrentViewFolder, true));

		        cm.AddButton("Refresh all thumbnails", RefreshViewItemsThumbnails);
	        }

	        cm.AddSeparator();

	        if (!isRootFolder)
	        {
		        cm.AddButton("New folder", NewFolder);
	        }

	        c = cm.AddChildMenu("New");
	        c.ContextMenu.Tag = item;
	        int newItems = 0;
	        for (int i = 0; i < Editor.ContentDatabase.Proxy.Count; i++)
	        {
		        var p = Editor.ContentDatabase.Proxy[i];
		        if (p.CanCreate(folder))
		        {
			        c.ContextMenu.AddButton(p.Name, () => NewItem(p));
			        newItems++;

		        }
	        }
	        c.Enabled = newItems > 0;

	        if (folder.CanHaveAssets)
            {
                cm.AddButton("Import file", () =>
                {
	                _view.ClearSelection();
	                Editor.ContentImporting.ShowImportFileDialog(CurrentViewFolder);
				});
            }

            // Show it
            cm.Show(this, location);
        }

        /// <summary>
        /// Refreshes thumbnails for all the items in the view.
        /// </summary>
        private void RefreshViewItemsThumbnails()
        {
            var items = _view.Items;
            for (int i = 0; i < items.Count; i++)
            {
                items[i].RefreshThumbnail();
            }
        }

        /// <summary>
        /// Reimports the selected assets.
        /// </summary>
        private void ReimportSelection()
        {
            var selection = _view.Selection;
            for (int i = 0; i < selection.Count; i++)
            {
                if(selection[i] is BinaryAssetItem binaryAssetItem)
                    Editor.ContentImporting.Reimport(binaryAssetItem);
            }
        }
    }
}
