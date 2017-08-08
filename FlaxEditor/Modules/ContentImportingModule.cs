////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Threading;
using FlaxEditor.Content;
using FlaxEditor.Content.Import;
using FlaxEngine;

namespace FlaxEditor.Modules
{
    /// <summary>
    /// Imports assets and other resources to the project. Provides per asset import settings editing.
    /// </summary>
    /// <seealso cref="FlaxEditor.Modules.EditorModule" />
    public sealed class ContentImportingModule : EditorModule
    {
        private struct Request
        {
            public string InputPath;
            public string OutputPath;

            public Request(string input, string output)
            {
                InputPath = input;
                OutputPath = output;
            }
        }

        // Amount of requests done/total used to calculate importing progress

        private int _importBatchDone;
        private int _importBatchSize;

        // Firstly service is collecting import requests and then performs actual importing in the background.

        private readonly Queue<FileEntry> _importingQueue = new Queue<FileEntry>();
        private readonly List<Request> _requests = new List<Request>();

        private Thread _workerThread;

        /// <summary>
        /// Gets a value indicating whether this instance is importing assets.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is importing; otherwise, <c>false</c>.
        /// </value>
        public bool IsImporting => _importBatchSize > 0;

        /// <summary>
        /// Gets the importing assets progress.
        /// </summary>
        /// <value>
        /// The importing progress.
        /// </value>
        public float ImportingProgress => _importBatchSize > 0 ? (float)_importBatchDone / _importBatchSize : 1.0f;

        /// <inheritdoc />
        internal ContentImportingModule(Editor editor)
            : base(editor)
        {
        }

        /// <summary>
        /// Shows the dialog for selecting files to import.
        /// </summary>
        public void ShowImportFileDialog()
        {
            // TODO: add API to show files selecting dialog
            throw new NotImplementedException("import files dialog");
        }

        /// <summary>
        /// Reimports the specified <see cref="BinaryAssetItem"/> item.
        /// </summary>
        /// <param name="item">The item.</param>
        public void Reimport(BinaryAssetItem item)
        {
            string importPath;
            if (item != null && item.GetImportPath(out importPath))
                Import(importPath, item.Path);
        }

        /// <summary>
        /// Imports the specified files.
        /// </summary>
        /// <param name="files">The files.</param>
        /// <param name="targetLocation">The target location.</param>
        public void Import(List<string> files, ContentFolder targetLocation)
        {
            if (targetLocation == null)
                throw new ArgumentNullException();

            lock (_importingQueue)
            {
                bool skipDialog = false;
                for (int i = 0; i < files.Count; i++)
                {
                    Import(files[i], targetLocation, ref skipDialog);
                }
            }
        }

        /// <summary>
        /// Imports the specified file.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <param name="targetLocation">The target location.</param>
        public void Import(string file, ContentFolder targetLocation)
        {
            bool skipDialog = false;
            Import(file, targetLocation, ref skipDialog);
        }

        private void Import(string inputPath, ContentFolder targetLocation, ref bool skipDialog)
        {
            if (targetLocation == null)
                throw new ArgumentNullException();

            var extension = System.IO.Path.GetExtension(inputPath);
            var proxy = Editor.ContentDatabase.GetProxy(extension);

            // Get output file extension, this assumes that only binary files have diffrent extension than source file
            string outputExtension;
            if (proxy != null && !proxy.IsAsset)
            {
                // Use proxy file extension
                outputExtension = proxy.FileExtension;

                // Check if can place source files here
                if (!targetLocation.CanHaveScripts)
                {
                    // Error
                    if (!skipDialog)
                    {
                        skipDialog = true;
                        MessageBox.Show("Target location cannot have scripts. Use Source folder for your game source code.", "Cannot import assets", MessageBox.Buttons.OK, MessageBox.Icon.Error);
                    }
                    return;
                }
            }
            else
            {
                // Assume binary asset
                outputExtension = "flax";
                if (!targetLocation.CanHaveAssets)
                {
                    // Error
                    if (!skipDialog)
                    {
                        skipDialog = true;
                        MessageBox.Show("Target location cannot have assets. Use Content folder for your game assets.", "Cannot import assets", MessageBox.Buttons.OK, MessageBox.Icon.Error);
                    }
                    return;
                }
            }

            var shortName = System.IO.Path.GetFileNameWithoutExtension(inputPath);
            var outputPath = System.IO.Path.Combine(targetLocation.Path, shortName + outputExtension);

            Import(inputPath, outputPath);
        }

        /// <summary>
        /// Imports the specified file to the target destnation.
        /// Actual importing is done later after gathering settings from the user via <see cref="ImportFilesDialog"/>.
        /// </summary>
        /// <param name="inputPath">The input path.</param>
        /// <param name="outputPath">The output path.</param>
        public void Import(string inputPath, string outputPath)
        {
            lock (_importingQueue)
            {
                _requests.Add(new Request(inputPath, outputPath));
            }
        }

        private void StartWorker()
        {
            if (_workerThread != null)
                return;

            // ..
        }

        private void EndWorker()
        {
            if (_workerThread == null)
                return;

            // ..
        }

        /// <inheritdoc />
        public override void OnInit()
        {
            FileEntry.RegisterDefaultTypes();
        }
    }
}
