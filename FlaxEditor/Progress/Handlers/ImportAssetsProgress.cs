////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using FlaxEditor.Content.Import;

namespace FlaxEditor.Progress.Handlers
{
    /// <summary>
    /// Importing assets progress reporting handler.
    /// </summary>
    /// <seealso cref="FlaxEditor.Progress.ProgressHandler" />
    public sealed class ImportAssetsProgress : ProgressHandler
    {
        private string _currentFilename;

        /// <summary>
        /// Initializes a new instance of the <see cref="ImportAssetsProgress"/> class.
        /// </summary>
        public ImportAssetsProgress()
        {
            var importing = Editor.Instance.ContentImporting;
            importing.ImportingQueueBegin += OnStart;
            importing.ImportingQueueEnd += OnEnd;
            importing.ImportFileBegin += OnImportFileBegin;
        }

        private void OnImportFileBegin(FileEntry fileEntry)
        {
            _currentFilename = System.IO.Path.GetFileName(fileEntry.Url);
            UpdateProgress();
        }

        private void UpdateProgress()
        {
            var importing = Editor.Instance.ContentImporting;
            var info = string.Format("Importing \'{0}\' ({1}/{2})...", _currentFilename, importing.ImportBatchDone, importing.ImportBatchSize);
            OnUpdate(importing.ImportingProgress, info);
        }
    }
}
