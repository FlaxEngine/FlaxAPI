// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

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
        /// The global custom post processing effects applied to all <see cref="SceneRenderTask"/> (applied to tasks that have <see cref="AllowGlobalCustomPostFx"/> turned on).
        /// </summary>
        public static readonly HashSet<PostProcessEffect> GlobalCustomPostFx = new HashSet<PostProcessEffect>();

        /// <summary>
        /// Action delegate called before scene rendering. Should prepare <see cref="SceneRenderTask.View"/> structure for rendering.
        /// </summary>
        /// <param name="task">The task.</param>
        /// <param name="context">The GPU execution context.</param>
        public delegate void BeginDelegate(SceneRenderTask task, GPUContext context);

        /// <summary>
        /// Action delegate called after scene rendering.
        /// </summary>
        /// <param name="task">The task.</param>
        /// <param name="context">The GPU execution context.</param>
        public delegate void EndDelegate(SceneRenderTask task, GPUContext context);

        /// <summary>
        /// Action delegate called during rendering scene part to the view. Should submit custom draw calls using <see cref="DrawCallsCollector"/>.
        /// </summary>
        /// <param name="collector">The draw calls collector.</param>
        public delegate void DrawDelegate(DrawCallsCollector collector);

        /// <summary>
        /// The actors source to use during rendering.
        /// </summary>
        public ActorsSources ActorsSource = ActorsSources.Scenes;

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
        /// Task is creating and resizing them during rendering if need to.
        /// Size of the buffers always equals size of the output.
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
        /// The custom post processing effects.
        /// </summary>
        public readonly HashSet<PostProcessEffect> CustomPostFx = new HashSet<PostProcessEffect>();

        /// <summary>
        /// True if allow using global custom PostFx when rendering this task.
        /// </summary>
        public bool AllowGlobalCustomPostFx = true;

        /// <summary>
        /// The action called on rendering begin.
        /// </summary>
        public event BeginDelegate Begin;

        /// <summary>
        /// The action called on rendering end.
        /// </summary>
        public event EndDelegate End;

        /// <summary>
        /// The action called on view rendering to collect draw calls.
        /// It allows to extend rendering pipeline and draw custom geometry non-existing in the scene or custom actors set.
        /// </summary>
        public event DrawDelegate Draw;

        /// <summary>
        /// The amount of frame rendered by this task. Is auto incremented on scene rendering.
        /// </summary>
        public int FrameCount;

        private readonly DrawCallsCollector _collector = new DrawCallsCollector();
        private readonly HashSet<PostProcessEffect> _postFx = new HashSet<PostProcessEffect>();

        internal SceneRenderTask()
        {
            // Init view defaults
            View.MaxShadowsQuality = Quality.Ultra;
            View.ModelLODDistanceFactor = 1.0f;
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
                Buffers = RenderBuffers.New();

            // Prepare view
            OnBegin(context);
            Viewport viewport = new Viewport(Vector2.Zero, Buffers.Size);
            if (Camera != null)
            {
                View.CopyFrom(Camera, ref viewport);
            }

            // Get custom post effects to render (registered ones and from the current camera)
            _postFx.Clear();
            foreach (var e in CustomPostFx)
                _postFx.Add(e);
            if (AllowGlobalCustomPostFx)
            {
                foreach (var e in GlobalCustomPostFx)
                    _postFx.Add(e);
            }
            if (Camera != null)
            {
                var perCameraPostFx = Camera.GetScripts<PostProcessEffect>();
                for (int i = 0; i < perCameraPostFx.Length; i++)
                {
                    _postFx.Add(perCameraPostFx[i]);
                }
            }

            // Call scene rendering
            context.DrawScene(this, Output, Buffers, View, Flags, Mode, CustomActors, ActorsSource, _postFx);
            FrameCount++;

            // Finish
            OnEnd(context);
        }

        /// <summary>
        /// Called when on rendering begin.
        /// </summary>
        /// <param name="context">The GPU execution context.</param>
        protected virtual void OnBegin(GPUContext context)
        {
            Begin?.Invoke(this, context);

            // Resize buffers
            if (Output)
                Buffers.Size = Output.Size;
        }

        /// <summary>
        /// Called when on rendering end.
        /// </summary>
        /// <param name="context">The GPU execution context.</param>
        protected virtual void OnEnd(GPUContext context)
        {
            End?.Invoke(this, context);
        }

        internal override DrawCall[] Internal_Draw()
        {
            if (Draw != null)
            {
                // Collect draw calls and send them packed back to be rendered
                _collector.Clear();
                Draw(_collector);
                return _collector.DrawCalls;
            }

            return null;
        }
    }
}
