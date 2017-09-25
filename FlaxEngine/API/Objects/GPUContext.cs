////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;

namespace FlaxEngine.Rendering
{
    /// <summary>
    /// Defines actors to draw sources.
    /// </summary>
    public enum ActorsSources
    {
        /// <summary>
        /// The actors from the loaded scenes.
        /// </summary>
        Scenes = 1,

        /// <summary>
        /// The actors from the custom collection.
        /// </summary>
        CustomActors = 2,

        /// <summary>
        /// The actors from the loaded scenes and custom collection.
        /// </summary>
        ScenesAndCustomActors = Scenes | CustomActors,
    }
    
    public partial class GPUContext
    {
        /// <summary>
        /// Draws scene.
        /// </summary>
        /// <param name="task">Calling render task.</param>
        /// <param name="output">Output texture.</param>
        /// <param name="buffers">Frame rendering buffers.</param>
        /// <param name="view">Rendering view description structure.</param>
        /// <param name="flags">Custom view flags collection.</param>
        /// <param name="mode">Custom view mode option.</param>
        /// <param name="customActors">Custom set of actors to render.</param>
        /// <param name="actorsSource">Actors source to use during rendering.</param>
#if UNIT_TEST_COMPILANT
		[Obsolete("Unit tests, don't support methods calls.")]
#endif
        [UnmanagedCall]
        public void DrawScene(RenderTask task, RenderTarget output, RenderBuffers buffers, RenderView view, ViewFlags flags, ViewMode mode, Actor[] customActors = null, ActorsSources actorsSource = ActorsSources.ScenesAndCustomActors, HashSet<PostProcessEffect> customPostFx = null)
        {
#if UNIT_TEST_COMPILANT
			throw new NotImplementedException("Unit tests, don't support methods calls. Only properties can be get or set.");
#else
            // Get unmanaged actors
            IntPtr[] actors = null;
            if (customActors != null)
            {
                actors = new IntPtr[customActors.Length];
                for (int i = 0; i < customActors.Length; i++)
                {
                    actors[i] = Object.GetUnmanagedPtr(customActors[i]);
                }
            }

            // Get unmanaged postFx
            IntPtr[] postFx = null;
            if (customPostFx != null)
            {
                var postFxList = new List<IntPtr>(customPostFx.Count);
                foreach (var e in customPostFx)
                {
                    if (e && e.CanRender)
                        postFxList.Add(e.unmanagedPtr);
                }
                if (postFxList.Count > 0)
                    postFx = postFxList.ToArray();
            }

            Internal_DrawScene(unmanagedPtr, Object.GetUnmanagedPtr(task), Object.GetUnmanagedPtr(output), Object.GetUnmanagedPtr(buffers), ref view, flags, mode, actors, actorsSource, postFx);
#endif
        }
    }
}
