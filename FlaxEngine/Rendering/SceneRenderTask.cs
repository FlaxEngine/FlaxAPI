////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;

namespace FlaxEngine.Rendering
{
    /// <summary>
    /// Render task which draws scene actors into the output buffer.
    /// </summary>
    /// <seealso cref="FlaxEngine.Rendering.RenderTask" />
    public class SceneRenderTask : RenderTask
    {
        /// <summary>
        /// The custom set of actors to render.
        /// If collection is empty whole scene actors will be used.
        /// </summary>
        public readonly List<Actor> CustomActors = new List<Actor>();

        /// <summary>
        /// The rendering output surface.
        /// </summary>
        public RenderTarget Output;

        /// <summary>
        /// The rendering view description.
        /// </summary>
        public RenderView View;

        /// <summary>
        /// The custom camera actor to use during rendering.
        /// If not provided the default one will be used.
        /// </summary>
        public Camera Camera;

        /// <summary>
        /// The custom event to can skip rendering if need to. Returns true if should skip rendering a frame.
        /// </summary>
        public Func<bool> CanSkipRendering;

        internal SceneRenderTask()
        {
        }

        /// <inheritdoc />
        public override void Dispose()
        {
            if(Output)
                Output.Dispose();

            base.Dispose();
        }

        internal override bool Internal_Begin(out IntPtr outputPtr, out ViewFlags flags, out ViewMode mode, out Actor[] customActors)
        {
            base.Internal_Begin(out outputPtr, out flags, out mode, out customActors);

            if (CanSkipRendering != null && CanSkipRendering())
                return false;
            
            outputPtr = GetUnmanagedPtr(Output);
            if (CustomActors.Count > 0)
                customActors = CustomActors.ToArray();

            // Allow to render only if has buffers and the output
            return Output && Output.IsAllocated;
        }

        internal override void Internal_Render(GPUContext context)
        {
            // TODO: draw scene
            context.Clear(Output, Color.Brown);
        }
    }
}
