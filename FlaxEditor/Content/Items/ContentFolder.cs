////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlaxEditor.Content
{
    /// <summary>
    /// Types of content directories.
    /// </summary>
    public enum ContentFolderType
    {
        /// <summary>
        /// The directory with assets.
        /// </summary>
        Content,

        /// <summary>
        /// The directory with script source files.
        /// </summary>
        Source,

        /// <summary>
        /// The directory with Editor private files.
        /// </summary>
        Editor,

        /// <summary>
        /// The directory with Engine private files.
        /// </summary>
        Engine,

        /// <summary>
        /// The other type of directory.
        /// </summary>
        Other
    } 

    /// <summary>
    /// Represents workspace directory item.
    /// </summary>
    public class ContentFolder : ContentItem
    {
        /// <summary>
        /// Gets the type of the folder.
        /// </summary>
        /// <value>
        /// The type of the folder.
        /// </value>
        public ContentFolderType FolderType { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContentFolder"/> class.
        /// </summary>
        /// <param name="path">The path to the item.</param>
        /// <param name="type">The folder type.</param>
        public ContentFolder(string path, ContentFolderType type)
            : base(path)
        {
            FolderType = type;
        }

        /// <inheritdoc />
        public override ContentItemType ItemType => ContentItemType.Folder;
    }
}
