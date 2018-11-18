// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System;
using FlaxEngine.Rendering;

namespace FlaxEngine.GUI
{
    /// <summary>
    /// A common control used to present rendered frame in the UI.
    /// </summary>
    /// <seealso cref="FlaxEngine.GUI.ContainerControl" />
    [HideInEditor]
    public class RenderOutputControl : ContainerControl
    {
        /// <summary>
        /// The default back buffer format used by the GUI controls presenting rendered frames.
        /// </summary>
        public const PixelFormat DefaultBackBufferFormat = PixelFormat.R8G8B8A8_UNorm;

        /// <summary>
        /// The resize check timeout (in seconds).
        /// </summary>
        public const float ResizeCheckTime = 0.9f;

        /// <summary>
        /// The task.
        /// </summary>
        protected SceneRenderTask _task;

        /// <summary>
        /// The back buffer.
        /// </summary>
        protected RenderTarget _backBuffer;

        private RenderTarget _backBufferOld;
        private int _oldBackbufferLiveTimeLeft;
        private float _resizeTime;

        /// <summary>
        /// Gets the task.
        /// </summary>
        public SceneRenderTask Task => _task;

        /// <summary>
        /// Gets a value indicating whether render to that output only if parent window exists, otherwise false.
        /// </summary>
        public bool RenderOnlyWithWindow { get; set; } = true;

        /// <summary>
        /// Gets or sets the color of the tint used to color the backbuffer of the render output.
        /// </summary>
        public Color TintColor { get; set; } = Color.White;

        /// <summary>
        /// Gets or sets the brightness of the output.
        /// </summary>
        public float Brightness { get; set; } = 1.0f;

        /// <summary>
        /// Initializes a new instance of the <see cref="RenderOutputControl"/> class.
        /// </summary>
        /// <param name="task">The task. Cannot be null.</param>
        /// <exception cref="System.ArgumentNullException">Invalid task.</exception>
        public RenderOutputControl(SceneRenderTask task)
        {
            if (task == null)
                throw new ArgumentNullException();

            _backBuffer = RenderTarget.New();
            _resizeTime = ResizeCheckTime;

            _task = task;
            _task.Output = _backBuffer;
            _task.CanSkipRendering += CanSkipRendering;
            _task.End += OnEnd;
        }

        /// <summary>
        /// Enables this output rendering.
        /// </summary>
        public void Enable()
        {
            Task.Enabled = true;
        }

        /// <summary>
        /// Disables this output rendering.
        /// </summary>
        public void Disable()
        {
            Task.Enabled = false;
        }

        private bool walkTree(Control c)
        {
            while (c != null)
            {
                if (c is RootControl win)
                {
                    return false;
                }
                if (c.Visible == false)
                    break;
                c = c.Parent;
            }
            return true;
        }

        /// <summary>
        /// Performs a check if rendering a current frame can be skipped (if control size is too small, has missing data, etc.).
        /// </summary>
        /// <returns>True if skip rendering, otherwise false.</returns>
        protected virtual bool CanSkipRendering()
        {
            if (_task == null)
                return true;

            _task.Output = _backBuffer;

            // Disable task rendering if control is very small
            const float MinRenderSize = 4;
            if (Width < MinRenderSize || Height < MinRenderSize)
                return true;

            // Disable task rendering if control is not used in a window (has using ParentWindow)
            if (RenderOnlyWithWindow)
            {
                return walkTree(Parent);
            }

            return false;
        }

        /// <summary>
        /// Called when ask rendering ends.
        /// </summary>
        /// <param name="task">The task.</param>
        /// <param name="context">The GPU execution context.</param>
        protected virtual void OnEnd(SceneRenderTask task, GPUContext context)
        {
            // Check if was using old backbuffer
            if (_backBufferOld)
            {
                _oldBackbufferLiveTimeLeft--;
                if (_oldBackbufferLiveTimeLeft < 0)
                {
                    Object.Destroy(ref _backBufferOld);
                }
            }
        }

        /// <inheritdoc />
        public override void Update(float deltaTime)
        {
            // Check if need to resize the output
            _resizeTime += deltaTime;
            if (_resizeTime >= ResizeCheckTime)
            {
                _resizeTime = 0;
                SyncBackbufferSize();
            }

            base.Update(deltaTime);
        }

        /// <inheritdoc />
        public override void Draw()
        {
            // Draw backbuffer texture
            var buffer = _backBufferOld ? _backBufferOld : _backBuffer;
            var color = TintColor * Brightness;
            Render2D.DrawRenderTarget(buffer, new Rectangle(Vector2.Zero, Size), color);

            base.Draw();
        }

        /// <summary>
        /// Synchronizes size of the back buffer with the size of the control.
        /// </summary>
        public void SyncBackbufferSize()
        {
            int width = Mathf.CeilToInt(Width);
            int height = Mathf.CeilToInt(Height);
            if (_backBuffer == null || _backBuffer.Width == width && _backBuffer.Height == height)
                return;
            if (width < 1 || height < 1)
            {
                _backBuffer.Dispose();
                Object.Destroy(ref _backBufferOld);
                return;
            }

            // Cache old backbuffer to remove flickering effect
            if (_backBufferOld == null && _backBuffer.IsAllocated)
            {
                _backBufferOld = _backBuffer;
                _backBuffer = RenderTarget.New();
            }

            // Set timeout to remove old buffer
            _oldBackbufferLiveTimeLeft = 3;

            // Resize backbuffer
            _backBuffer.Init(DefaultBackBufferFormat, width, height);
            _task.Output = _backBuffer;
        }

        /// <inheritdoc />
        public override void OnDestroy()
        {
            if (IsDisposing)
                return;

            // Cleanup
            _task?.Dispose();
            Object.Destroy(ref _backBuffer);
            Object.Destroy(ref _backBufferOld);
            Object.Destroy(ref _task);

            base.OnDestroy();
        }
    }
}
