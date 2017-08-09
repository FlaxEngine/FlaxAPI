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

        private long _workerEndFlag;
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

        /// <summary>
        /// Occurs when assets importing starts.
        /// </summary>
        public event Action ImportingQueueBegin;

        /// <summary>
        /// Occurs when file is being imported.
        /// </summary>
        public event Action<FileEntry> ImportFileBegin;

        /// <summary>
        /// Import file end delegate.
        /// </summary>
        /// <param name="entry">The imported file entry.</param>
        /// <param name="failed">if set to <c>true</c> if importing failed, otherwise false.</param>
        public delegate void ImportFileEndDelegate(FileEntry entry, bool failed);

        /// <summary>
        /// Occurs when file importing end.
        /// </summary>
        public event ImportFileEndDelegate ImportFileEnd;

        /// <summary>
        /// Occurs when assets importing ends.
        /// </summary>
        public event Action ImportingQueueEnd;

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

            lock (_requests)
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
            lock (_requests)
            {
                _requests.Add(new Request(inputPath, outputPath));
            }
        }

        private void WorkerMain()
        {
            FileEntry entry;
            bool wasLastTickWorking = false;

            while (Interlocked.Read(ref _workerEndFlag) == 0)
            {
                // Try to get entry to process
                lock (_requests)
                {
                    if (_importingQueue.Count > 0)
                        entry = _importingQueue.Dequeue();
                    else
                        entry = null;
                }

                // Check if has any no job
                bool inThisTickWork = entry != null;
                if (inThisTickWork)
                {
                    // Check if begin importing
                    if (!wasLastTickWorking)
                    {
                        Debug.Log("importing start");
                        _importBatchDone = 0;
                        ImportingQueueBegin?.Invoke();
                    }

                    // Import file
                    ImportFileBegin?.Invoke(entry);
                    // TODO: expose importing content to c#
                    //if (AssetsImportingManager::Instance()->Import(data.InputPath, data.OutputPath, data.Argument) == false)
                    bool failed = false;
                    _importBatchDone++;
                    ImportFileEnd?.Invoke(entry, failed);
                }
                else
                {
                    // Check if end importing
                    if (wasLastTickWorking)
                    {
                        Debug.Log("importing end");
                        _importBatchDone = _importBatchSize = 0;
                        ImportingQueueEnd?.Invoke();
                    }

                    // Wait some time
                    Thread.Sleep(100);
                }

                wasLastTickWorking = inThisTickWork;
            }
        }

        private void StartWorker()
        {
            if (_workerThread != null)
                return;

            Debug.Log("StartWorker");

            _workerEndFlag = 0;
            _workerThread = new Thread(WorkerMain);
            _workerThread.Name = "Content Importer";
            _workerThread.Priority = ThreadPriority.Highest;
            _workerThread.Start();
        }

        private void EndWorker()
        {
            if (_workerThread == null)
                return;

            Debug.Log("EndWorker");

            Interlocked.Increment(ref _workerEndFlag);
            Thread.Sleep(0);

            _workerThread.Join(1000);
            _workerThread.Abort();
            _workerThread = null;
        }

        /// <inheritdoc />
        public override void OnInit()
        {
            FileEntry.RegisterDefaultTypes();
        }

        /// <inheritdoc />
        public override void OnUpdate()
        {
            // Check if has no requests to process
            if (_requests.Count == 0)
                return;

            try
            {
                Debug.Log("convert requests to entries");

                // Get entries
                List<FileEntry> entries = new List<FileEntry>(_requests.Count);
                bool needSettingsDialog = false;
                for (int i = 0; i < _requests.Count; i++)
                {
                    Debug.Log(" ----> " + _requests[i].InputPath + "  ->  " + _requests[i].OutputPath);
                    var entry = FileEntry.CreateEntry(_requests[i].InputPath, _requests[i].OutputPath);
                    if (entry != null)
                    {
                        entries.Add(entry);
                        needSettingsDialog |= entry.HasSettings;
                    }
                }
                _requests.Clear();

                // Check if need to show importing dialog or can just pass requests
                if (needSettingsDialog)
                {
                    Debug.Log("use import fiels dialog " + entries.Count);
                }
                else
                {
                    Debug.Log("use direct import " + entries.Count);

                    _importBatchSize += entries.Count;
                    for (int i = 0; i < entries.Count; i++)
                        _importingQueue.Enqueue(entries[i]);
                }
            }
            catch (Exception ex)
            {
                // Error
                Editor.LogWarning(ex);
                Editor.LogError("Failed to process files import request.");
            }
        }

        /// <inheritdoc />
        public override void OnExit()
        {
            EndWorker();
        }
    }
}
