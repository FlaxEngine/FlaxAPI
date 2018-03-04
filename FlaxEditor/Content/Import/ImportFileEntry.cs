////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2018 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.IO;

namespace FlaxEditor.Content.Import
{
    /// <summary>
    /// Creates new <see cref="ImportFileEntry"/> for the given source file.
    /// </summary>
    /// <param name="url">The source file url.</param>
    /// <param name="resultUrl">The result file url.</param>
    /// <returns>The file entry.</returns>
    public delegate ImportFileEntry ImportFileEntryHandler(string url, string resultUrl);

    /// <summary>
    /// File import entry.
    /// </summary>
    public class ImportFileEntry : IFileEntryAction
    {
        /// <inheritdoc />
        public string SourceUrl { get; }

        /// <inheritdoc />
        public string ResultUrl { get; }

        /// <summary>
        /// Gets a value indicating whether this entry has settings to modify.
        /// </summary>
        public virtual bool HasSettings => Settings != null;

        /// <summary>
        /// Gets or sets the settings object to modify.
        /// </summary>
        public virtual object Settings => null;

        /// <summary>
        /// Tries the override import settings.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <returns>True if settings override was successful and there is no need to edit them in dedicated dialog, otherwise false.</returns>
        public virtual bool TryOverrideSettings(object settings)
        {
            return false;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ImportFileEntry"/> class.
        /// </summary>
        /// <param name="url">The source file url.</param>
        /// <param name="resultUrl">The result file url.</param>
        public ImportFileEntry(string url, string resultUrl)
        {
            SourceUrl = url;
            ResultUrl = resultUrl;
        }

        /// <summary>
        /// Performs file importing.
        /// </summary>
        /// <returns>True if failed, otherwise false.</returns>
        public virtual bool Import()
        {
            // Copy file by default
            if (!File.Exists(SourceUrl))
                return true;
            string folder = Path.GetDirectoryName(ResultUrl);
            if (folder != null && !Directory.Exists(folder))
                Directory.CreateDirectory(folder);
            File.Copy(SourceUrl, ResultUrl, true);
            return false;
        }

        /// <summary>
        /// The file types registered for importing. Key is a file extension (without a leading dot).
        /// Allows to plug custom importing options gather for diffrent input file types.
        /// </summary>
        public static readonly Dictionary<string, ImportFileEntryHandler> FileTypes = new Dictionary<string, ImportFileEntryHandler>(32);

        /// <summary>
        /// Creates the entry.
        /// </summary>
        /// <param name="url">The source file url.</param>
        /// <param name="resultUrl">The result file url.</param>
        /// <param name="isBinaryAsset">True if result file is binary asset.</param>
        /// <returns>Created file entry.</returns>
        public static ImportFileEntry CreateEntry(string url, string resultUrl, bool isBinaryAsset)
        {
            // Get extension (without a dot)
            var extension = Path.GetExtension(url);
            if (string.IsNullOrEmpty(extension))
                throw new ArgumentException();
            if (extension[0] == '.')
                extension = extension.Remove(0, 1);
            extension = extension.ToLower();

            // Check if use overriden type
            ImportFileEntryHandler createDelegate;
            if (FileTypes.TryGetValue(extension, out createDelegate))
                return createDelegate(url, resultUrl);

            // Use default type
            return isBinaryAsset ? new AssetImportEntry(url, resultUrl) : new ImportFileEntry(url, resultUrl);
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

			// Audio
	        FileTypes["wav"] = ImportAudio;
	        FileTypes["mp3"] = ImportAudio;
	        FileTypes["ogg"] = ImportAudio;
		}

        private static ImportFileEntry ImportModel(string url, string resultUrl)
        {
            return new ModelImportEntry(url, resultUrl);
        }

	    private static ImportFileEntry ImportAudio(string url, string resultUrl)
	    {
		    return new AudioImportEntry(url, resultUrl);
	    }

		private static ImportFileEntry ImportTexture(string url, string resultUrl)
        {
            return new TextureImportEntry(url, resultUrl);
        }

        /// <inheritdoc />
        public bool Execute()
        {
            return Import();
        }
    }
}
