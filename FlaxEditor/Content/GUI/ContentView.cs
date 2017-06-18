////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using FlaxEditor.Windows;
using FlaxEngine.GUI;

namespace FlaxEditor.Content.GUI
{
    /// <summary>
    /// Main control for <see cref="ContentWindow"/> used to present collection of <see cref="ContentItem"/>.
    /// </summary>
    /// <seealso cref="FlaxEngine.GUI.ContainerControl" />
    /// <seealso cref="FlaxEditor.Content.IContentItemOwner" />
    public class ContentView : ContainerControl, IContentItemOwner
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ContentView"/> class.
        /// </summary>
        public ContentView()
            : base(true)
        {
        }

        /// <summary>
        /// Clears the items in the view.
        /// </summary>
        public void ClearItems()
        {
            // TODO: finish this
            /*ClearSelection();

            for (int i = 0; i < _elements.Count; i++)
            {
                _elements[i]->SetParent(nullptr);
                _elements[i]->RemoveUIRef(this);
            }

            _elements.Clear();

            PerformLayout();*/
        }

        /// <summary>
        /// Shows the items collection in the view.
        /// </summary>
        /// <param name="items">The items to show.</param>
        public void ShowItems(List<ContentItem> items)
        {
            // TODO: finish this
        }

        /// <summary>
        /// Called when user clicks on an item.
        /// </summary>
        /// <param name="item">The item.</param>
        public void OnItemClick(ContentItem item)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Called when user wants to rename item.
        /// </summary>
        /// <param name="item">The item.</param>
        public void OnRename(ContentItem item)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Called when user wants to open item.
        /// </summary>
        /// <param name="item">The item.</param>
        public void OnOpen(ContentItem item)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        void IContentItemOwner.OnItemDeleted(ContentItem item)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        void IContentItemOwner.OnItemRenamed(ContentItem item)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        void IContentItemOwner.OnItemDispose(ContentItem item)
        {
            throw new NotImplementedException();
        }
    }
}
