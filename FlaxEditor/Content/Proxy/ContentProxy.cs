////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using FlaxEditor.Windows;
using FlaxEngine;

namespace FlaxEditor.Content
{
    /// <summary>
    /// Base class for asstes proxy objects used to manage <see cref="ContentItem"/>.
    /// </summary>
    public abstract class ContentProxy
    {
        /// <summary>
        /// Gets the asset type name (used by GUI, should be localizable).
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public abstract string Name { get; }

        /// <summary>
        /// Determines whether this proxy is for the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>
        ///   <c>true</c> if is proxy for asset item; otherwise, <c>false</c>.
        /// </returns>
        public abstract bool IsProxyFor(ContentItem item);

        /// <summary>
        /// Gets a value indicating whether this proxy if for assets.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this proxy is for assets; otherwise, <c>false</c>.
        /// </value>
        public virtual bool IsAsset => false;

        /// <summary>
        /// Gets the file extension used by the items managed by this proxy.
        /// </summary>
        /// <value>
        /// The file extension.
        /// </value>
        public abstract string FileExtension { get; }

        /// <summary>
        /// Opens the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>Opened window or null if cannot do it.</returns>
        public abstract EditorWindow Open(ContentItem item);

        /// <summary>
        /// Gets a value indicating whether content items used by this proxy can be exported.
        /// </summary>
        /// <value>
        ///   <c>true</c> if content items used by this proxy can be exported; otherwise, <c>false</c>.
        /// </value>
        public virtual bool CanExport => false;

        /// <summary>
        /// Exports the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="outputPath">The output path.</param>
        /// <returns>True if fails, otherwise false.</returns>
        public virtual bool Export(ContentItem item, string outputPath)
        {
            return true;
        }

        /// <summary>
        /// Determines whether this proxy can create items in the specified target location.
        /// </summary>
        /// <param name="targetLocation">The target location.</param>
        /// <returns>
        ///   <c>true</c> if this proxy can create items in the specified target location; otherwise, <c>false</c>.
        /// </returns>
        public virtual bool CanCreate(ContentFolder targetLocation)
        {
            return false;
        }

        /// <summary>
        /// Determines whether this proxy can reimport specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>
        ///   <c>true</c> if this proxy can reimport given item; otherwise, <c>false</c>.
        /// </returns>
        public virtual bool CanReimport(ContentItem item)
        {
            return CanCreate(item.ParentFolder);
        }

        /// <summary>
        /// Creates new item at the specified output path.
        /// </summary>
        /// <param name="outputPath">The output path.</param>
        /// <returns>True if fails, otherwise false.</returns>
        public virtual bool Create(string outputPath)
        {
            return true;
        }

        /// <summary>
        /// Gets the unique accent color for that asset type.
        /// </summary>
        /// <value>
        /// The color of the accent.
        /// </value>
        public abstract Color AccentColor { get; }
        /*
        /// <summary>
        /// Determines whether proxy is ready to render a thumbnail for the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>
        ///   <c>true</c> if is ready to render item preview otherwise, <c>false</c>.
        /// </returns>
        public abstract bool IsThumbnailRenderReady(ContentItem item);

        public abstract bool OnPreviewRenderBegin(ContentItem item, CustomRenderTask* task, ContainerControl guiRoot);

        public virtual void OnPreviewRenderEnd(ContentItem item, CustomRenderTask* task, ContainerControl guiRoot)
        {
        }*/
    }
}
