////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;

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
        /// Returns true if that folder can import/manage scripts.
        /// </summary>
        /// <returns>True if can contain scripts for project, otherwise false</returns>
        public bool CanHaveScripts => FolderType == ContentFolderType.Source;

        /// <summary>
        /// Returns true if that folder can import/manage assets.
        /// </summary>
        /// <returns>True if can contain assets for project, otherwise false</returns>
        public bool CanHaveAssets => FolderType == ContentFolderType.Content || FolderType == ContentFolderType.Editor || FolderType == ContentFolderType.Engine;

        /// <summary>
        /// Returns true if that folder belongs to the project workspace.
        /// </summary>
        /// <returns>True if folder belogns to the project workspace otherwise false</returns>
        public  bool IsProjectOnly => FolderType == ContentFolderType.Content || FolderType ==  ContentFolderType.Source;

        /// <summary>
        /// Returns true if that folder belongs to the Engine or Editor private files.
        /// </summary>
        /// <returns>True if folder belogns to Engine private files otherwise false</returns>
        public bool IsEnginePrivate => FolderType == ContentFolderType.Editor || FolderType == ContentFolderType.Engine;

        /// <summary>
        /// The subitems of this folder.
        /// </summary>
        public readonly List<ContentItem> Children = new List<ContentItem>();

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

        /// <summary>
        /// Tries to find child element with given path
        /// </summary>
        /// <param name="path">Element path to find</param>
        /// <returns>Found element of null</returns>
        public ContentItem FindChild(string path)
        {
            for (int i = 0; i < Children.Count; i++)
            {
                if (Children[i].Path == path)
                    return Children[i];
            }

            return null;
        }

        /// <summary>
        /// Check if folder contains child element with given path
        /// </summary>
        /// <param name="path">Element path to find</param>
        /// <returns>True if contains that element, otherwise false</returns>
        public bool ContaisnChild(string path)
        {
            return FindChild(path) != null;
        }

        /// <inheritdoc />
        public override ContentItemType ItemType => ContentItemType.Folder;

        /// <inheritdoc />
        public override bool CanRename => ParentFolder != null; // Deny rename action for root folders

        /// <inheritdoc />
        public override string DefaultPreviewName => "Folder64";

        /// <inheritdoc />
        public override ContentItem Find(string path)
        {
            // TODO: split name into parts and check each going tree sructure level down - make it faster

            if (Path == path)
                return this;

            for (int i = 0; i < Children.Count; i++)
            {
                var result = Children[i].Find(path);
                if (result != null)
                    return result;
            }

            return null;
        }

        /// <inheritdoc />
        public override bool Find(ContentItem item)
        {
            if (item == this)
                return true;

            for (int i = 0; i < Children.Count; i++)
            {
                if (Children[i].Find(item))
                    return true;
            }

            return false;
        }

        /// <inheritdoc />
        public override ContentItem Find(Guid id)
        {
            for (int i = 0; i < Children.Count; i++)
            {
                var result = Children[i].Find(id);
                if (result != null)
                    return result;
            }

            return null;
        }

        /// <inheritdoc />
        public override ScriptItem FindScriptWitScriptName(string scriptName)
        {
            for (int i = 0; i < Children.Count; i++)
            {
                var result = Children[i].FindScriptWitScriptName(scriptName);
                if (result != null)
                    return result;
            }

            return null;
        }
    }
}
