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
        /// Action delegate called before scene rendering. Should prepare <see cref="SceneRenderTask.View"/> structure for rendering.
        /// </summary>
        /// <param name="task">The task.</param>
        public delegate void BeginDelegate(SceneRenderTask task);

        /// <summary>
        /// The custom set of actors to render.
        /// If collection is empty whole scene actors will be used.
        /// </summary>
        public readonly List<Actor> CustomActors = new List<Actor>();

        /// <summary>
        /// The rendering output surface.
        /// It needs to be assigned by the user to perform rendering.
        /// </summary>
        public RenderTarget Output;

        /// <summary>
        /// The frame rendering buffers.
        /// Task is creating and resizing them during rendering.
        /// </summary>
        public RenderBuffers Buffers;

        /// <summary>
        /// The view flags.
        /// </summary>
        public ViewFlags Flags = ViewFlags.DefaultGame;

        /// <summary>
        /// The view mode.
        /// </summary>
        public ViewMode Mode = ViewMode.Default;

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

        /// <summary>
        /// The action called on rendering begin.
        /// </summary>
        public BeginDelegate OnBegin;

        internal SceneRenderTask()
        {
        }

        /// <inheritdoc />
        public override void Dispose()
        {
            if (Output)
                Output.Dispose();

            base.Dispose();
        }

        internal override bool Internal_Begin(out IntPtr outputPtr)
        {
            base.Internal_Begin(out outputPtr);

            if (CanSkipRendering != null && CanSkipRendering())
                return false;

            outputPtr = GetUnmanagedPtr(Output);

            // Allow to render only if has buffers and the output
            return Output && Output.IsAllocated;
        }

        internal override void Internal_Render(GPUContext context)
        {
            // Copy flags
            View.Flags = Flags;
            View.Mode = Mode;

            // Create buffers if missing
            if (Buffers == null)
                Buffers = RenderBuffers.Create();

            // Prepare view
            if (Camera != null)
                View.CopyFrom(Camera);
            OnBegin?.Invoke(this);

            // Resize buffers
            Buffers.Size = Output.Size;

            // Call scene rendering
            //if (CustomActors.Count > 0)
            //    customActors = CustomActors.ToArray();



            // TODO: draw scene
            context.Clear(Output, new Color(View.Direction.X, View.Direction.Y, View.Direction.Z, 1.0f));
        }
    }
}
