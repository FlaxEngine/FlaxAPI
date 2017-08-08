////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System.Collections.Generic;

namespace FlaxEditor.Content.Import
{
    /// <summary>
    /// Creates new <see cref="FileEntry"/> for the given source file.
    /// </summary>
    /// <param name="url">The source file url.</param>
    /// <returns>The file entry.</returns>
    public delegate FileEntry CreateFileEntry(string url);

    /// <summary>
    /// File import entry.
    /// </summary>
    public class FileEntry
    {
        /// <summary>
        /// The path to the source file.
        /// </summary>
        public readonly string Url;

        /// <summary>
        /// Gets a value indicating whether this entry has settings to modify.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this entr has settings; otherwise, <c>false</c>.
        /// </value>
        public virtual bool HasSettings => false;

        /// <summary>
        /// Gets the settings object to modify.
        /// </summary>
        /// <value>
        /// The settings object.
        /// </value>
        public virtual object Settings => null;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileEntry"/> class.
        /// </summary>
        /// <param name="url">The source file url.</param>
        public FileEntry(string url)
        {
            Url = url;
        }

        /// <summary>
        /// The file types registered for importing. Key is a file extension (with dot).
        /// </summary>
        public static readonly Dictionary<string, CreateFileEntry> FileTypes = new Dictionary<string, CreateFileEntry>(32);

        internal void RegisterDefaultTypes()
        {
            // Textures
            FileTypes["tga"] = ImportTexture;
            FileTypes["png"] = ImportTexture;
            FileTypes["bmp"] = ImportTexture;
            FileTypes["gif"] = ImportTexture;
            FileTypes["tiff"] = ImportTexture;
            FileTypes["jpeg"] = ImportTexture;
            FileTypes["jpg"] = ImportTexture;

            // Models
            FileTypes["obj"] = ImportModel;
            FileTypes["fbx"] = ImportModel;
            FileTypes["x"] = ImportModel;
            FileTypes["dae"] = ImportModel;
            //
            FileTypes["blend"] = ImportModel;
            FileTypes["bvh"] = ImportModel;
            FileTypes["ase"] = ImportModel;
            FileTypes["ply"] = ImportModel;
            FileTypes["dxf"] = ImportModel;
            FileTypes["ifc"] = ImportModel;
            FileTypes["nff"] = ImportModel;
            FileTypes["smd"] = ImportModel;
            FileTypes["vta"] = ImportModel;
            FileTypes["mdl"] = ImportModel;
            FileTypes["md2"] = ImportModel;
            FileTypes["md3"] = ImportModel;
            FileTypes["md5mesh"] = ImportModel;
            FileTypes["q3o"] = ImportModel;
            FileTypes["q3s"] = ImportModel;
            FileTypes["ac"] = ImportModel;
            FileTypes["stl"] = ImportModel;
            FileTypes["lwo"] = ImportModel;
            FileTypes["lws"] = ImportModel;
            FileTypes["lxo"] = ImportModel;
        }

        private static FileEntry ImportModel(string url)
        {
            return new ModelFileEntry(url);
        }

        private static FileEntry ImportTexture(string url)
        {
            return new TextureFileEntry(url);
        }
    }
}
