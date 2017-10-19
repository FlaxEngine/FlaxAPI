////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System;
using FlaxEngine.Rendering;

namespace FlaxEngine.GUI
{
    /// <summary>
    /// A common control used to present rendered frame in the UI.
    /// </summary>
    /// <seealso cref="FlaxEngine.GUI.ContainerControl" />
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

        protected SceneRenderTask _task;
        private RenderTarget _backBuffer;
        private RenderTarget _backBufferOld;
        private int _oldBackbufferLiveTimeLeft;
        private float _resizeTime;

        /// <summary>
        /// Gets the task.
        /// </summary>
        /// <value>
        /// The task.
        /// </value>
        public SceneRenderTask Task => _task;

        /// <summary>
        /// Gets a value indicating whether render to that output only if parent window exists, otherwise false.
        /// </summary>
        /// <value>
        ///   <c>true</c> if render only with window attached; otherwise, <c>false</c>.
        /// </value>
        public bool RenderOnlyWithWindow { get; set; } = true;

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
                if (c is Window win)
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
            _task.Output = _backBuffer;

            // Disable task rendering if control is very small
            const float MinRenderSize = 4;
            if (Width < MinRenderSize || Height < MinRenderSize)
                return true;

            // Disable task rendering if control is not used in a window (has issing ParentWindow)
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
        protected virtual void OnEnd(SceneRenderTask task)
        {
            // Check if was using old backuffer
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
            Render2D.DrawRenderTarget(buffer, new Rectangle(Vector2.Zero, Size), Color.White);

            base.Draw();
        }
        
        /// <summary>
        /// Synchronizes size of the back buffer with the size of the control.
        /// </summary>
        public void SyncBackbufferSize()
        {
            int width = (int)Width;
            int height = (int)Height;
            if (_backBuffer.Width == width && _backBuffer.Height == height)
                return;
            if (width < 1 || height < 1)
            {
                _backBuffer.Dispose();
                Object.Destroy(ref _backBufferOld);
                return;
            }

            // Cache old backuffer to remove flckering effect
            if (_backBufferOld == null && _backBuffer.IsAllocated)
            {
                _backBufferOld = _backBuffer;
                _backBuffer = RenderTarget.New();
            }

            // Set timout to remove old buffer
            _oldBackbufferLiveTimeLeft = 3;

            // Resize backbuffer
            _backBuffer.Init(DefaultBackBufferFormat, width, height);
            _task.Output = _backBuffer;
        }

        /// <inheritdoc />
        public override void OnDestroy()
        {
            // Cleanup
            _task.Dispose();
            Object.Destroy(ref _backBuffer);
            Object.Destroy(ref _backBufferOld);

            base.OnDestroy();
        }
    }
}
