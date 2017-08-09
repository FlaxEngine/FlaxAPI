////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.IO;

namespace FlaxEditor.Content.Import
{
    /// <summary>
    /// Creates new <see cref="FileEntry"/> for the given source file.
    /// </summary>
    /// <param name="url">The source file url.</param>
    /// <param name="resultUrl">The result file url.</param>
    /// <returns>The file entry.</returns>
    public delegate FileEntry CreateFileEntry(string url, string resultUrl);

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
        /// The result file path.
        /// </summary>
        public readonly string ResultUrl;

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
        /// <param name="resultUrl">The result file url.</param>
        public FileEntry(string url, string resultUrl)
        {
            Url = url;
            ResultUrl = resultUrl;
        }

        /// <summary>
        /// Performs file importing.
        /// </summary>
        /// <returns>True if failed, otherwise false.</returns>
        public virtual bool Import()
        {
            // Copy file by default
            if (!File.Exists(Url))
                return true;
            string folder = Path.GetDirectoryName(ResultUrl);
            if (folder != null && !Directory.Exists(folder))
                Directory.CreateDirectory(folder);
            File.Copy(Url, ResultUrl, true);
            return false;
        }

        /// <summary>
        /// The file types registered for importing. Key is a file extension (without a leading dot).
        /// Allows to plug custom importing options gather for diffrent input file types.
        /// </summary>
        public static readonly Dictionary<string, CreateFileEntry> FileTypes = new Dictionary<string, CreateFileEntry>(32);

        /// <summary>
        /// Creates the entry.
        /// </summary>
        /// <param name="url">The source file url.</param>
        /// <param name="resultUrl">The result file url.</param>
        /// <param name="isBinaryAsset">True if result file is binary asset.</param>
        /// <returns>Created file entry.</returns>
        public static FileEntry CreateEntry(string url, string resultUrl, bool isBinaryAsset)
        {
            // Get extension (without a dot)
            var extension = Path.GetExtension(url);
            if (string.IsNullOrEmpty(extension))
                throw new ArgumentException();
            if (extension[0] == '.')
                extension = extension.Remove(0, 1);

            // Check if use overriden type
            CreateFileEntry createDelegate;
            if (FileTypes.TryGetValue(extension, out createDelegate))
                return createDelegate(url, resultUrl);

            // Use default type
            return isBinaryAsset ? new AssetFileEntry(url, resultUrl) : new FileEntry(url, resultUrl);
        }

        internal static void RegisterDefaultTypes()
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

        private static FileEntry ImportModel(string url, string resultUrl)
        {
            return new ModelFileEntry(url, resultUrl);
        }

        private static FileEntry ImportTexture(string url, string resultUrl)
        {
            return new TextureFileEntry(url, resultUrl);
        }
    }
}
