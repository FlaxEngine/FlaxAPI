// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System;

namespace FlaxEngine.Rendering
{
    /// <summary>
    /// Provides ability to perform custom rendering using <see cref="GPUContext"/>.
    /// </summary>
    /// <seealso cref="FlaxEngine.Rendering.RenderTask" />
    public class CustomRenderTask : RenderTask
    {
        /// <summary>
        /// The custom action to perform during rendering.
        /// </summary>
        public Action<GPUContext> OnRender;

        internal CustomRenderTask()
        {
        }

        internal override bool Internal_Begin(out IntPtr outputPtr)
        {
            base.Internal_Begin(out outputPtr);

            // Allow to render only if has linked callback
            return OnRender != null;
        }

        internal override void Internal_Render(GPUContext context)
        {
            OnRender(context);
        }
    }
}
