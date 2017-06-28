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
        /// Disposes render task data and child components (output and buffers).
        /// </summary>
        public virtual void Dispose()
        {
        }

        internal RenderTask()
        {
        }
        
        internal virtual bool Internal_Begin(out IntPtr outputPtr)
        {
            outputPtr = IntPtr.Zero;
            return true;
        }

        internal virtual void Internal_Render(GPUContext context)
        {
        }
    }
}
