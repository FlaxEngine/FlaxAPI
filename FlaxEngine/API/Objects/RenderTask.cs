////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System;

namespace FlaxEngine.Rendering
{
	public partial class RenderTask
	{
        /// <summary>
        /// The view flags.
        /// </summary>
        public ViewFlags Flags = ViewFlags.DefaultGame;

        /// <summary>
        /// The view mode.
        /// </summary>
        public ViewMode Mode = ViewMode.Default;

        /// <summary>
        /// The rendering output surface.
        /// </summary>
        public RenderTarget Output;
        
	    internal RenderTask()
	    {
	    }

	    internal Actor[] CustomActors => null;

        internal void Internal_Begin(out IntPtr outputPtr, out ViewFlags flags, out ViewMode mode)//, out Actor[] customActors)
	    {
	        outputPtr = GetUnmanagedPtr(Output);
	        flags = Flags;
	        mode = Mode;
	        //customActors = CustomActors;
	    }
	}
}
