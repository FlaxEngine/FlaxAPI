////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;

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
        /// Disposes render task data and child components (output and buffers).
        /// </summary>
        public virtual void Dispose()
        {
        }

        internal RenderTask()
        {
        }
        
        internal virtual bool Internal_Begin(out IntPtr outputPtr, out ViewFlags flags, out ViewMode mode, out Actor[] customActors)
        {
            outputPtr = IntPtr.Zero;
            flags = Flags;
            mode = Mode;
            customActors = null;

            return true;
        }

        internal virtual void Internal_Render(GPUContext context)
        {
        }
    }
}
