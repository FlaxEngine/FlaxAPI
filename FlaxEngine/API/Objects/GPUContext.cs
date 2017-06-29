////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlaxEngine.Rendering
{
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
#if UNIT_TEST_COMPILANT
		[Obsolete("Unit tests, don't support methods calls.")]
#endif
	    [UnmanagedCall]
	    public void DrawScene(RenderTask task, RenderTarget output, RenderBuffers buffers, RenderView view, ViewFlags flags, ViewMode mode, Actor[] customActors)
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

	        Internal_DrawScene(unmanagedPtr, Object.GetUnmanagedPtr(task), Object.GetUnmanagedPtr(output), Object.GetUnmanagedPtr(buffers), ref view, flags, mode, actors);
#endif
	    }
    }
}
