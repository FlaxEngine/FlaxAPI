// This code was auto-generated. Do not modify it.

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace FlaxEngine
{
    /// <summary>
    /// Allows to perform custom rendering using graphics pipeline.
    /// </summary>
    [Tooltip("Allows to perform custom rendering using graphics pipeline.")]
    public unsafe partial class RenderTask : FlaxEngine.Object
    {
        /// <inheritdoc />
        protected RenderTask() : base()
        {
        }

        /// <summary>
        /// Creates new instance of <see cref="RenderTask"/> object.
        /// </summary>
        /// <returns>The created object.</returns>
        public new static RenderTask New()
        {
            return Internal_Create(typeof(RenderTask)) as RenderTask;
        }

        /// <summary>
        /// Gets or sets a value indicating whether task is enabled.
        /// </summary>
        [Tooltip("Gets or sets a value indicating whether task is enabled.")]
        public bool Enabled
        {
            get { return Internal_GetEnabled(unmanagedPtr); }
            set { Internal_SetEnabled(unmanagedPtr, value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_GetEnabled(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetEnabled(IntPtr obj, bool value);

        /// <summary>
        /// The order of the task. Used for tasks rendering order. Lower first, higher later.
        /// </summary>
        [Tooltip("The order of the task. Used for tasks rendering order. Lower first, higher later.")]
        public int Order
        {
            get { return Internal_GetOrder(unmanagedPtr); }
            set { Internal_SetOrder(unmanagedPtr, value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern int Internal_GetOrder(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetOrder(IntPtr obj, int value);

        /// <summary>
        /// The amount of frames rendered by this task. It is auto incremented on task drawing.
        /// </summary>
        [Tooltip("The amount of frames rendered by this task. It is auto incremented on task drawing.")]
        public int FrameCount
        {
            get { return Internal_GetFrameCount(unmanagedPtr); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern int Internal_GetFrameCount(IntPtr obj);

        /// <summary>
        /// Determines whether this task can be rendered.
        /// </summary>
        [Tooltip("Determines whether this task can be rendered.")]
        public bool CanDraw
        {
            get { return Internal_CanDraw(unmanagedPtr); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_CanDraw(IntPtr obj);

        /// <summary>
        /// Called by graphics device to draw this task. Can be used to invoke task rendering nested inside another task - use on own risk!
        /// </summary>
        public void OnDraw()
        {
            Internal_OnDraw(unmanagedPtr);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_OnDraw(IntPtr obj);

        /// <summary>
        /// Changes the buffers and output size. Does nothing if size won't change. Called by window or user to resize rendering buffers.
        /// </summary>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <returns>True if cannot resize the buffers.</returns>
        public bool Resize(int width, int height)
        {
            return Internal_Resize(unmanagedPtr, width, height);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_Resize(IntPtr obj, int width, int height);
    }
}

namespace FlaxEngine
{
    /// <summary>
    /// Defines actors to draw sources.
    /// </summary>
    [Flags]
    [Tooltip("Defines actors to draw sources.")]
    public enum ActorsSources
    {
        /// <summary>
        /// The actors won't be rendered.
        /// </summary>
        [Tooltip("The actors won't be rendered.")]
        None = 0,

        /// <summary>
        /// The actors from the loaded scenes.
        /// </summary>
        [Tooltip("The actors from the loaded scenes.")]
        Scenes = 1,

        /// <summary>
        /// The actors from the custom collection.
        /// </summary>
        [Tooltip("The actors from the custom collection.")]
        CustomActors = 2,

        /// <summary>
        /// The actors from the loaded scenes and custom collection.
        /// </summary>
        [Tooltip("The actors from the loaded scenes and custom collection.")]
        ScenesAndCustomActors = Scenes | CustomActors,
    }
}

namespace FlaxEngine
{
    /// <summary>
    /// Render task which draws scene actors into the output buffer.
    /// </summary>
    /// <seealso cref="FlaxEngine.RenderTask" />
    [Tooltip("Render task which draws scene actors into the output buffer.")]
    public unsafe partial class SceneRenderTask : RenderTask
    {
        /// <inheritdoc />
        protected SceneRenderTask() : base()
        {
        }

        /// <summary>
        /// Creates new instance of <see cref="SceneRenderTask"/> object.
        /// </summary>
        /// <returns>The created object.</returns>
        public new static SceneRenderTask New()
        {
            return Internal_Create(typeof(SceneRenderTask)) as SceneRenderTask;
        }

        /// <summary>
        /// The output texture (can be null if using rendering to window swap chain). Can be sued to redirect the default scene rendering output to a texture.
        /// </summary>
        [Tooltip("The output texture (can be null if using rendering to window swap chain). Can be sued to redirect the default scene rendering output to a texture.")]
        public GPUTexture Output
        {
            get { return Internal_GetOutput(unmanagedPtr); }
            set { Internal_SetOutput(unmanagedPtr, FlaxEngine.Object.GetUnmanagedPtr(value)); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern GPUTexture Internal_GetOutput(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetOutput(IntPtr obj, IntPtr value);

        /// <summary>
        /// The scene rendering buffers. Created and managed by the task.
        /// </summary>
        [Tooltip("The scene rendering buffers. Created and managed by the task.")]
        public RenderBuffers Buffers
        {
            get { return Internal_GetBuffers(unmanagedPtr); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern RenderBuffers Internal_GetBuffers(IntPtr obj);

        /// <summary>
        /// The scene rendering camera. Can be used to override the rendering view properties based on the current camera setup.
        /// </summary>
        [Tooltip("The scene rendering camera. Can be used to override the rendering view properties based on the current camera setup.")]
        public Camera Camera
        {
            get { return Internal_GetCamera(unmanagedPtr); }
            set { Internal_SetCamera(unmanagedPtr, FlaxEngine.Object.GetUnmanagedPtr(value)); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern Camera Internal_GetCamera(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetCamera(IntPtr obj, IntPtr value);

        /// <summary>
        /// The render view description.
        /// </summary>
        [Tooltip("The render view description.")]
        public RenderView View
        {
            get { Internal_GetView(unmanagedPtr, out var resultAsRef); return resultAsRef; }
            set { Internal_SetView(unmanagedPtr, ref value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_GetView(IntPtr obj, out RenderView resultAsRef);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetView(IntPtr obj, ref RenderView value);

        /// <summary>
        /// The actors source to use (configures what objects to render).
        /// </summary>
        [Tooltip("The actors source to use (configures what objects to render).")]
        public ActorsSources ActorsSource
        {
            get { return Internal_GetActorsSource(unmanagedPtr); }
            set { Internal_SetActorsSource(unmanagedPtr, value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern ActorsSources Internal_GetActorsSource(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetActorsSource(IntPtr obj, ActorsSources value);

        /// <summary>
        /// Gets the rendering render task viewport.
        /// </summary>
        [Tooltip("The rendering render task viewport.")]
        public Viewport Viewport
        {
            get { Internal_GetViewport(unmanagedPtr, out var resultAsRef); return resultAsRef; }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_GetViewport(IntPtr obj, out Viewport resultAsRef);

        /// <summary>
        /// Gets the rendering output view.
        /// </summary>
        [Tooltip("The rendering output view.")]
        public GPUTextureView OutputView
        {
            get { return Internal_GetOutputView(unmanagedPtr); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern GPUTextureView Internal_GetOutputView(IntPtr obj);

        /// <summary>
        /// Marks the next rendered frame as camera cut. Used to clear the temporal effects history and prevent visual artifacts blended from the previous frames.
        /// </summary>
        public void CameraCut()
        {
            Internal_CameraCut(unmanagedPtr);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_CameraCut(IntPtr obj);

        /// <summary>
        /// Adds the custom actor to the rendering.
        /// </summary>
        /// <param name="actor">The actor.</param>
        public void AddCustomActor(Actor actor)
        {
            Internal_AddCustomActor(unmanagedPtr, FlaxEngine.Object.GetUnmanagedPtr(actor));
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_AddCustomActor(IntPtr obj, IntPtr actor);

        /// <summary>
        /// Removes the custom actor from the rendering.
        /// </summary>
        /// <param name="actor">The actor.</param>
        public void RemoveCustomActor(Actor actor)
        {
            Internal_RemoveCustomActor(unmanagedPtr, FlaxEngine.Object.GetUnmanagedPtr(actor));
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_RemoveCustomActor(IntPtr obj, IntPtr actor);
    }
}

namespace FlaxEngine
{
    /// <summary>
    /// The main game rendering task used by the engine.
    /// </summary>
    /// <remarks>
    /// For Main Render Task its <see cref="SceneRenderTask.Output"/> may be null because game can be rendered directly to the native window backbuffer.
    /// This allows to increase game rendering performance (reduced memory usage and data transfer).
    /// User should use post effects pipeline to modify the final frame.
    /// </remarks>
    /// <seealso cref="FlaxEngine.SceneRenderTask" />
    [Tooltip("The main game rendering task used by the engine.")]
    public unsafe partial class MainRenderTask : SceneRenderTask
    {
        /// <inheritdoc />
        protected MainRenderTask() : base()
        {
        }

        /// <summary>
        /// Creates new instance of <see cref="MainRenderTask"/> object.
        /// </summary>
        /// <returns>The created object.</returns>
        public new static MainRenderTask New()
        {
            return Internal_Create(typeof(MainRenderTask)) as MainRenderTask;
        }

        /// <summary>
        /// Gets the main game rendering task. Use it to plug custom rendering logic for your game.
        /// </summary>
        [Tooltip("The main game rendering task. Use it to plug custom rendering logic for your game.")]
        public static MainRenderTask Instance
        {
            get { return Internal_GetInstance(); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern MainRenderTask Internal_GetInstance();
    }
}

namespace FlaxEngine
{
    /// <summary>
    /// The high-level renderer context. Used to collect the draw calls for the scene rendering. Can be used to perform a custom rendering.
    /// </summary>
    [Tooltip("The high-level renderer context. Used to collect the draw calls for the scene rendering. Can be used to perform a custom rendering.")]
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct RenderContext
    {
        /// <summary>
        /// The render buffers.
        /// </summary>
        [Tooltip("The render buffers.")]
        public RenderBuffers Buffers;

        /// <summary>
        /// The render list.
        /// </summary>
        [Tooltip("The render list.")]
        public RenderList List;

        /// <summary>
        /// The render view.
        /// </summary>
        [Tooltip("The render view.")]
        public RenderView View;

        /// <summary>
        /// The proxy render view used to synchronize objects level of detail during rendering (eg. during shadow maps rendering passes). It's optional.
        /// </summary>
        [Tooltip("The proxy render view used to synchronize objects level of detail during rendering (eg. during shadow maps rendering passes). It's optional.")]
        public RenderView* LodProxyView;

        /// <summary>
        /// The scene rendering task that is a source of renderable objects (optional).
        /// </summary>
        [Tooltip("The scene rendering task that is a source of renderable objects (optional).")]
        public SceneRenderTask Task;
    }
}
