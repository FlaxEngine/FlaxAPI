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
            public bool IsBinaryAsset;
            public object Settings;

            public Request(string input, string output, bool isBinaryAsset, object settings)
            {
                InputPath = input;
                OutputPath = output;
                IsBinaryAsset = isBinaryAsset;
                Settings = settings;
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
        /// Gets the amount of files done in the current import batch.
        /// </summary>
        public float ImportBatchDone => _importBatchDone;

        /// <summary>
        /// Gets the size of the current import batch (imported files + files to import left).
        /// </summary>
        public int ImportBatchSize => _importBatchSize;

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
        /// <param name="targetLocation">The target location.</param>
        public void ShowImportFileDialog(ContentFolder targetLocation)
        {
            // Ask user to select files to import
            var files = MessageBox.OpenFileDialog(Editor.Windows.MainWindow, null, "All files (*.*)\0*.*\0", true, "Select files to import");
            if (files != null && files.Length > 0)
            {
                Import(files, targetLocation);
            }
        }

        /// <summary>
        /// Reimports the specified <see cref="BinaryAssetItem"/> item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="settings">The import settings to override.</param>
        public void Reimport(BinaryAssetItem item, object settings = null)
        {
            string importPath;
            if (item != null && !item.GetImportPath(out importPath))
            {
                // Check if input file is missing
                if (!System.IO.File.Exists(importPath))
                {
                    Editor.LogWarning(string.Format("Cannot reimport asset \'{0}\'. File \'{1}\' does not exist.", item.Path, importPath));

                    // Ask user to select new file location
                    var title = string.Format("Please find missing \'{0}\' file for asset \'{1}\'", importPath, item.ShortName);
                    var files = MessageBox.OpenFileDialog(Editor.Windows.MainWindow, null, "All files (*.*)\0*.*\0", false, title);
                    if (files != null && files.Length > 0)
                        importPath = files[0];

                    // Validate file path again
                    if (!System.IO.File.Exists(importPath))
                        return;
                }

                Import(importPath, item.Path, true, settings);
            }
        }

        /// <summary>
        /// Imports the specified files.
        /// </summary>
        /// <param name="files">The files.</param>
        /// <param name="targetLocation">The target location.</param>
        public void Import(IEnumerable<string> files, ContentFolder targetLocation)
        {
            if (targetLocation == null)
                throw new ArgumentNullException();

            lock (_requests)
            {
                bool skipDialog = false;
                foreach (var file in files)
                {
                    Import(file, targetLocation, ref skipDialog);
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

            var extension = System.IO.Path.GetExtension(inputPath) ?? string.Empty;

            // Check if given file extension is a binary asset (.flax files) and can be imported by the engine
            bool isBinaryAsset = Editor.CanImport(extension);
            string outputExtension;
            if (isBinaryAsset)
            {
                // Flax it up!
                outputExtension = ".flax";

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
            else
            {
                // Preserve file extension (will copy file to the import location)
                outputExtension = extension;

                // Check if can place source files here
                if (extension.Equals(ScriptProxy.Extension, StringComparison.OrdinalIgnoreCase) && !targetLocation.CanHaveScripts)
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

            var shortName = System.IO.Path.GetFileNameWithoutExtension(inputPath);
            var outputPath = System.IO.Path.Combine(targetLocation.Path, shortName + outputExtension);

            Import(inputPath, outputPath, isBinaryAsset);
        }

        /// <summary>
        /// Imports the specified file to the target destnation.
        /// Actual importing is done later after gathering settings from the user via <see cref="ImportFilesDialog"/>.
        /// </summary>
        /// <param name="inputPath">The input path.</param>
        /// <param name="outputPath">The output path.</param>
        /// <param name="isBinaryAsset">True if output file is a binary asset.</param>
        /// <param name="settings">Import settings to override.</param>
        private void Import(string inputPath, string outputPath, bool isBinaryAsset, object settings = null)
        {
            lock (_requests)
            {
                _requests.Add(new Request(inputPath, outputPath, isBinaryAsset, settings));
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
                        _importBatchDone = 0;
                        ImportingQueueBegin?.Invoke();
                    }

                    // Import file
                    bool failed = true;
                    try
                    {
                        ImportFileBegin?.Invoke(entry);
                        failed = entry.Import();
                    }
                    catch (Exception ex)
                    {
                        Editor.LogWarning(ex);
                    }
                    finally
                    {
                        if (failed)
                        {
                            Editor.LogWarning("Failed to import " + entry.Url);
                        }

                        _importBatchDone++;
                        ImportFileEnd?.Invoke(entry, failed);
                    }
                }
                else
                {
                    // Check if end importing
                    if (wasLastTickWorking)
                    {
                        _importBatchDone = _importBatchSize = 0;
                        ImportingQueueEnd?.Invoke();
                    }

                    // Wait some time
                    Thread.Sleep(100);
                }

                wasLastTickWorking = inThisTickWork;
            }
        }

        internal void LetThemBeImportedxD(List<FileEntry> entries)
        {
            int count = entries.Count;
            if (count > 0)
            {
                _importBatchSize += count;
                for (int i = 0; i < count; i++)
                    _importingQueue.Enqueue(entries[i]);

                StartWorker();
            }
        }

        private void StartWorker()
        {
            if (_workerThread != null)
                return;
            
            _workerEndFlag = 0;
            _workerThread = new Thread(WorkerMain)
            {
                Name = "Content Importer",
                Priority = ThreadPriority.Highest
            };
            _workerThread.Start();
        }

        private void EndWorker()
        {
            if (_workerThread == null)
                return;
            
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

            lock (_requests)
            {
                try
                {
                    // Get entries
                    List<FileEntry> entries = new List<FileEntry>(_requests.Count);
                    bool needSettingsDialog = false;
                    for (int i = 0; i < _requests.Count; i++)
                    {
                        var request = _requests[i];
                        var entry = FileEntry.CreateEntry(request.InputPath, request.OutputPath, request.IsBinaryAsset);
                        if (entry != null)
                        {
                            if (request.Settings != null && entry.TryOverrideSettings(request.Settings))
                            {
                                // Use overriden settings
                            }
                            else
                            {
                                needSettingsDialog |= entry.HasSettings;
                            }
                            
                            entries.Add(entry);
                        }
                    }
                    _requests.Clear();

                    // Check if need to show importing dialog or can just pass requests
                    if (needSettingsDialog)
                    {
                        var dialog = new ImportFilesDialog(entries);
                        dialog.Show(Editor.Windows.MainWindow);
                    }
                    else
                    {
                        LetThemBeImportedxD(entries);
                    }
                }
                catch (Exception ex)
                {
                    // Error
                    Editor.LogWarning(ex);
                    Editor.LogError("Failed to process files import request.");
                }
            }
        }

        /// <inheritdoc />
        public override void OnExit()
        {
            EndWorker();
        }
    }
}
