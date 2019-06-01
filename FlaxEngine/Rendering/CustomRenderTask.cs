// Copyright (c) 2012-2019 Wojciech Figat. All rights reserved.

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
        public Action<GPUContext> Render;

        internal CustomRenderTask()
        {
        }

        internal override bool OnBegin(out IntPtr outputPtr)
        {
            base.OnBegin(out outputPtr);

            // Allow to render only if has linked callback
            return Render != null;
        }

        internal override void OnRender(GPUContext context)
        {
            Render(context);
        }
    }
}
