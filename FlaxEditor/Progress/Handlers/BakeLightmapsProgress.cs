// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using FlaxEngine;

namespace FlaxEditor.Progress.Handlers
{
    /// <summary>
    /// Static lightmaps baking progress reporting handler.
    /// </summary>
    /// <seealso cref="FlaxEditor.Progress.ProgressHandler" />
    public sealed class BakeLightmapsProgress : ProgressHandler
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BakeLightmapsProgress"/> class.
        /// </summary>
        public BakeLightmapsProgress()
        {
            Editor.LightmapsBakeStart += OnLightmapsBakeStart;
            Editor.LightmapsBakeProgress += OnLightmapsBakeProgress;
            Editor.LightmapsBakeEnd += OnLightmapsBakeEnd;
        }

        private void OnLightmapsBakeStart()
        {
            OnStart();
        }

        private void OnLightmapsBakeProgress(Editor.LightmapsBakeSteps step, float stepProgress, float totalProgress)
        {
            OnUpdate(totalProgress, string.Format("Building lightmaps {0}% ...", Utils.RoundTo2DecimalPlaces(totalProgress * 100)));
        }

        private void OnLightmapsBakeEnd(bool failed)
        {
            OnEnd();
        }
    }
}
