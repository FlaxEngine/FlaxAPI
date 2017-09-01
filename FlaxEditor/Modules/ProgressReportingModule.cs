////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using FlaxEditor.Progress.Handlers;

namespace FlaxEditor.Modules
{
    /// <summary>
    /// Helper module for engine long-operations progress reporting in the editor (eg. files importing, sttatic light baking, etc.).
    /// </summary>
    /// <seealso cref="FlaxEditor.Modules.EditorModule" />
    public sealed class ProgressReportingModule : EditorModule
    {
        /// <summary>
        /// The assets importing progress handler.
        /// </summary>
        public readonly ImportAssetsProgress ImportAssets = new ImportAssetsProgress();

        /// <summary>
        /// The scripts compilation progress handler.
        /// </summary>
        public readonly CompileScriptsProgress CompileScripts = new CompileScriptsProgress();

        /// <summary>
        /// The lightmaps baking progress handler.
        /// </summary>
        public readonly BakeLightmapsProgress BakeLightmaps = new BakeLightmapsProgress();

        /// <summary>
        /// The environment probes baking progress handler.
        /// </summary>
        public readonly BakeEnvProbesProgress BakeEnvProbes = new BakeEnvProbesProgress();

        internal ProgressReportingModule(Editor editor)
            : base(editor)
        {
        }
    }
}
